using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Resources;
using System.Threading;

namespace SECS_Code
{
    public class HsmsConnection
    {
        delegate int Decoder(byte[] data, int length, ref int index, out int need);
        readonly Decoder[] decoders;
        volatile bool _isDisposed;
        public static ConnectionState connectionState;
        public static System.Timers.Timer timer7 = new System.Timers.Timer();
        public static System.Timers.Timer timer8 = new System.Timers.Timer();
        public static System.Timers.Timer LinkTest = new System.Timers.Timer();
        public static bool IsActive;
        private CancellationToken _stoppingToken;
        private CancellationTokenSource? _cancellationTokenSourceForPipeDecoder;
        public static byte[] ControlMessage_Select = new byte[14];
        public static IPAddress IpAddress;
        //
        private readonly ConcurrentDictionary<int, TaskCompletionSourceToken> _replyExpectedMsgs = new ConcurrentDictionary<int, TaskCompletionSourceToken>();

        private static readonly SecsMessage ControlMessage_ = new SecsMessage(0, 0);
        private static readonly ArraySegment<byte> ControlMessageLengthBytes = new ArraySegment<byte>(new byte[] { 0, 0, 0, 10 });
        //
        public static ushort DeviceId;
        public static int Port;
        public static int T3;
        public static int T5;
        public static int T6;
        public static int T7;
        public static int T8;
        public static bool LinkTest_temp;
        public static ArraySegment<byte> _remainBytes;
        public static int _currentStep;
        public static uint _messageLength;
        public byte[] recvBuffer;
        public static Socket _socket;
        public static int LinkTestInterval;
        public static IConfiguration configuration;
        readonly Action _stopImpl;
        readonly Func<Task> _startImpl;
        public static JctMQTT jctMQTT = new();
        public static SendData sendData = new();
        public readonly JctSecsReplyLib secsLib = new();
        public static bool LinkTest_Enabled
        {
            get { return LinkTest.Enabled; }
            set
            {
                if (LinkTest.Enabled != value)
                {
                    LinkTest.Interval = LinkTestInterval;
                    LinkTest.Enabled = value;
                }

            }
        }
        public static bool T8_Enabled = false;
        //test
        public static int id_bytes = 0;
        public async Task Start() => _startImpl();
        public HsmsConnection(IConfiguration _configuration)
        {
            JctMQTT.Handle_Received_Application_Message();
            configuration = _configuration;
            IpAddress = IPAddress.Parse(_configuration["ip"]);
            Port = int.Parse(_configuration["port"]);
            IsActive = bool.Parse(_configuration["IsActive"]);
            T3 = int.Parse(_configuration["T3"]);
            T5 = int.Parse(_configuration["T5"]);
            T6 = int.Parse(_configuration["T6"]);
            T7 = int.Parse(_configuration["T7"]);
            T8 = int.Parse(_configuration["T8"]);
            LinkTest_temp = bool.Parse(_configuration["LinkTest"]);
            DeviceId = ushort.Parse(_configuration["DeviceId"]);
            LinkTestInterval = int.Parse(_configuration["LinkTestInterval"]);
            timer7.Elapsed += delegate
            {
                Console.WriteLine("T7 Timeout");
                ChangeConnectionState(ConnectionState.Retry);
            };

            timer8.Elapsed += delegate
            {
                T8_Enabled = true;
                Console.WriteLine("T8 Timeout");
                ChangeConnectionState(ConnectionState.Retry);
            };
            LinkTest.Elapsed += delegate
            {
                if (connectionState == ConnectionState.Selected)
                {

                    try
                    {
                        id_bytes++;
                        using SecsMessage test = new SecsMessage(6, 1)
                        {
                            SecsItem = Item.L(
                                        Item.U4(1),
                                        Item.U4(1),
                                        Item.A(DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                                        Item.L(
                                               from slotInfo in JctMQTT.send_data
                                               select
                                                     Item.F4(slotInfo)
                                            )
                                        )
                        };
                        PrintMessage.MessageOut(test, id_bytes);
                        SendDataMessageAsync(test, id_bytes);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"LinkTestError: " + ex.Message);
                    }
                    
                    
                    //SendControlMessage(MessageType.Linktest_req, systemByte);

                }
            };
            
            
            if (IsActive)
            {
                
                
                _startImpl = async delegate
                {
                    Utility.Log(5, "Mode:Active");
                    Utility.Log(5, $"Remote IP:{IpAddress}");
                    Utility.Log(5, $"Port:{Port}");
                    Utility.Log(2, "Try to Connect....");
                    var connected = false;
                    do
                    {
                        if (_isDisposed)
                        {
                            return;
                        }

                        ChangeConnectionState(ConnectionState.Connecting);
                        try
                        {
                            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                            {
                                Blocking = false,
                                ReceiveBufferSize = 8192,
                            };
                            await socket.ConnectAsync(IpAddress, Port).ConfigureAwait(false);


                            _socket = socket;
                            _socket.NoDelay = true;
                            ChangeConnectionState(ConnectionState.Connected);
                            connected = true;
                        }
                        catch (Exception ex) when (!_isDisposed)
                        {
                            //Utility.LogException(ex);
                            Utility.Log(4, $"Start T5 Timer: {T5 / 1000} sec.");
                            await Task.Delay(T5).ConfigureAwait(false);
                        }
                    } while (!connected);
                    int systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                    await SendControlMessage(MessageType.Select_req, systemByte);
                };

                _stopImpl = delegate { };
            }
            else
            {
                
                var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IpAddress, Port));
                server.Listen(0);
                
                _startImpl = async delegate
                {
                    Utility.Log(5, "Mode:Passive");
                    Utility.Log(5, $"Remote IP:{IpAddress}");
                    Utility.Log(5, $"Port:{Port}");
                    Utility.Log(2, "Wait Connect....");
                    var connected = false;
                    do
                    {
                        if (_isDisposed)
                        {
                            return;
                        }

                        ChangeConnectionState(ConnectionState.Connecting);
                        try
                        {
                            _socket = await server.AcceptAsync().ConfigureAwait(false);
                            _socket.NoDelay = true;
                            _socket.Blocking = false;
                            _socket.ReceiveBufferSize = 8192;
                            ChangeConnectionState(ConnectionState.Connected);
                            connected = true;
                        }
                        catch (Exception ex) when (!_isDisposed)
                        {
                            Utility.LogException(ex);
                            await Task.Delay(5000).ConfigureAwait(false);
                        }
                    } while (!connected);
                };

                _stopImpl = delegate
                {
                    if (_isDisposed)
                    {
                        server.Close();
                    }
                };
            }
        }
        public async Task SendControlMessage(MessageType messageType, int systemByte)
        {
            try
            {
                var token = new TaskCompletionSourceToken(ControlMessage_, systemByte, messageType);
                if ((byte)messageType % 2 == 1 && messageType != MessageType.Seperate_req)
                {
                    _replyExpectedMsgs[systemByte] = token;
                }
                var sendEventArgs = new SocketAsyncEventArgs
                {
                    BufferList = new List<ArraySegment<byte>>(2)
                {
                    ControlMessageLengthBytes,
                    new ArraySegment<byte>(new MessageHeader
                    {
                        DeviceId = 0xffff,
                        messagetype = messageType,
                        id = systemByte
                    }.EncodeTo())
                },
                    UserToken = token
                };
                sendEventArgs.Completed += SendControlMessage_Completed;
                if (!_socket.SendAsync(sendEventArgs))
                    SendControlMessage_Completed(_socket, sendEventArgs);
            }
            catch(Exception ex)
            {
                Console.WriteLine("SendControlMessage");
                Utility.LogException(ex);
            }
        }

        private void SendControlMessage_Completed(object sender, SocketAsyncEventArgs e)
        {
            var completeToken = e.UserToken as TaskCompletionSourceToken;
            if (e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                return;
            }

            Utility.Log(4, $"Sent Control message: {completeToken.MsgType}");
            if (_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
                if (!completeToken.Task.Wait(T6))
                {
                    Utility.Log(1, $"T6 Timeout: {T6 / 1000} sec.");
                    ChangeConnectionState(ConnectionState.Retry);
                }
                _replyExpectedMsgs.TryRemove(completeToken.Id, out TaskCompletionSourceToken _);
            }
        }


        //接收時需先確認是否大於14

        public void ChangeConnectionState(ConnectionState state)
        {
            connectionState = state;
            switch (state)
            {
                case ConnectionState.Connected:
                    Utility.Log(4, "TCP/IP Connected Successfully!");
                    Utility.Log(4, "Start T7 Timer");
                    _cancellationTokenSourceForPipeDecoder = new CancellationTokenSource();
                    Task.Run(() => StartReceive(_cancellationTokenSourceForPipeDecoder.Token));
                    timer7.Enabled = true;
                    timer7.Interval = T7;
                    break;
                case ConnectionState.Selected:
                    Utility.Log(4, "Stop T7 Timer");
                    Utility.Log(4, "HSMS Connected Successfully!");
                    T8_Enabled = false;
                    timer7.Enabled = false;
                    //LinkTest_Enabled = LinkTest_temp;
                    break;
                case ConnectionState.Retry:
                    if (_isDisposed)
                        return;
                    Reset();
                    break;
            }
        }
        void Reset()
        {
            Utility.Log(2, $"Reset....");
            timer7.Stop();
            timer8.Stop();
            LinkTest.Stop();
            _replyExpectedMsgs.Clear();
            _cancellationTokenSourceForPipeDecoder.Cancel();
            //_cancellationTokenSourceForPipeDecoder.Dispose();
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _socket = null;
            }
            if (!(connectionState == ConnectionState.Selected))
            {
                _startImpl();
            }

        }

        public bool Decode(byte[] bytes, int index, int length)
        {
            if (_remainBytes.Count == 0)
            {
                int need = Decode(bytes, length, ref index);
                int remainLength = length - index;
                if (remainLength > 0)
                {
                    var temp = new byte[remainLength + need];
                    Array.Copy(bytes, index, temp, 0, remainLength);
                    _remainBytes = new ArraySegment<byte>(temp, remainLength, need);
                    Utility.Log(5, $"Remain Length: {_remainBytes.Offset}, Need: {_remainBytes.Count}");
                }
                else
                {
                    _remainBytes = new ArraySegment<byte>();
                }
            }
            else if (length - index >= _remainBytes.Count)
            {
                Array.Copy(bytes, index, _remainBytes.Array, _remainBytes.Offset, _remainBytes.Count);
                index = _remainBytes.Count;
                byte[] temp = _remainBytes.Array;
                _remainBytes = new ArraySegment<byte>();
                if (Decode(temp, 0, temp.Length))
                    Decode(bytes, index, length);
            }
            else
            {
                int remainLength = length - index;
                Array.Copy(bytes, index, _remainBytes.Array, _remainBytes.Offset, remainLength);
                _remainBytes = new ArraySegment<byte>(_remainBytes.Array, _remainBytes.Offset + remainLength, _remainBytes.Count - remainLength);
                Utility.Log(5, $"Remain Length: {_remainBytes.Offset}, Need: {_remainBytes.Count}");
            }
            return _messageLength > 0;
        }

        int Decode(byte[] bytes, int length, ref int index)
        {
            int need;
            int nexStep = _currentStep;
            do
            {
                _currentStep = nexStep;
                nexStep = decoders[_currentStep](bytes, length, ref index, out need);
            } while (nexStep != _currentStep);
            return need;
        }
        /*
        void ReceiveComplete(IAsyncResult iar)
        {
            try
            {
                int count = _socket.EndReceive(iar);

                timer8.Enabled = false;

                if (count == 0)
                {
                    Console.WriteLine("Received 0 byte data. Close the socket.");
                    ChangeConnectionState(ConnectionState.Retry);
                    return;
                }

                if (Decode(recvBuffer, 0, count))
                {
                    Console.WriteLine("Start T8 Timer");
                    timer8.Interval = T8;
                    timer8.Enabled = true;
                }

                _socket.BeginReceive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None, ReceiveComplete, null);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("unexpected NullReferenceException:" + ex.ToString());
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"RecieveComplete socket error:{ex.Message + ex}, ErrorCode:{ex.SocketErrorCode}", ex);
                ChangeConnectionState(ConnectionState.Retry);
            }
            catch (Exception ex)
            {
                Console.WriteLine("unexpected exception", ex);
                ChangeConnectionState(ConnectionState.Retry);
            }
        }
        */

        private async Task StartReceive(CancellationToken cancellation)
        {
            
            try
            {
                byte[] receive_count = new byte[4];
                uint receive_byts = 0; //表示需要接收多少bytes
                int receive_bytes_temp = 0; //已經接收多少bytes
                int receive_bytes_remain = 0; //剩下多少bytes
                byte[] memory = new byte[8192];
                bool section_flag = false; //分段flag
                byte[] section_bytes = new byte[0]; //分段總長陣列
                Utility.Log(2, "Start Listening.....");
                while (true)
                {
                    var received = await _socket.ReceiveAsync(memory, SocketFlags.None);
                    if(received > 0)
                    {
                        for (int i = 0; i < received; i++)
                        {
                            Console.Write("0x" + memory[i].ToString("x2") + ", ");
                        }
                        Console.WriteLine();
                    }
                    
                    if (received > 0)
                    {
                        if (!section_flag)
                        {
                            receive_count[0] = memory[3];
                            receive_count[1] = memory[2];
                            receive_count[2] = memory[1];
                            receive_count[3] = memory[0];
                            receive_byts = BitConverter.ToUInt32(receive_count);
                            receive_byts = receive_byts + 4;
                        }
                        if (received < receive_byts)
                        {
                            
                            if (!section_flag)
                            {
                                Console.WriteLine("Start T8 Timer");
                                StartT8Timer();
                                receive_bytes_temp = received;
                                section_bytes = new byte[receive_byts];
                                Array.Copy(memory, 0, section_bytes, 0, received);
                            }
                            else
                            {
                                Array.Copy(memory, 0, section_bytes, receive_bytes_temp, received);
                                receive_bytes_temp = receive_bytes_temp + received;
                                
                            }
                            if(receive_bytes_temp == receive_byts)
                            {
                                
                                StopT8Timer();
                                Console.WriteLine("Stop T8 Timer");
                                section_flag = false;
                                receive_byts = 0;
                                receive_comp(section_bytes, cancellation);
                            }
                            else
                            {
                                section_flag = true;
                            }
                        }
                        else if(received > receive_byts)
                        {
                            if (section_flag) // 分段黏包
                            {
                                Array.Copy(memory, 0, section_bytes, receive_bytes_temp, (receive_byts - receive_bytes_temp));
                                receive_bytes_remain = (int)receive_byts - receive_bytes_temp;
                                receive_bytes_temp = receive_bytes_temp + ((int)receive_byts - receive_bytes_temp);
                                if (receive_bytes_temp == receive_byts)
                                {

                                    StopT8Timer();
                                    Console.WriteLine("Stop T8 Timer");
                                    section_flag = false;
                                    receive_byts = 0;
                                    receive_comp(section_bytes, cancellation);
                                }
                                //檢查剩下封包
                                receive_count[0] = memory[receive_bytes_remain + 3];
                                receive_count[1] = memory[receive_bytes_remain + 2];
                                receive_count[2] = memory[receive_bytes_remain + 1];
                                receive_count[3] = memory[receive_bytes_remain + 0];
                                receive_byts = BitConverter.ToUInt32(receive_count);
                                receive_byts = receive_byts + 4;
                                Console.WriteLine($"remain receive_byts: {receive_byts}");
                                if((received - receive_bytes_remain) == receive_byts)
                                {
                                    byte[] bytes = new byte[receive_byts];
                                    Array.Copy(memory, receive_bytes_remain, bytes, 0, receive_byts);
                                    foreach(var item in bytes)
                                    {
                                        Console.Write(item.ToString("x2") + ", ");
                                    }
                                    Console.WriteLine();
                                    receive_comp(bytes, cancellation);
                                }
                                else
                                {
                                    Console.WriteLine("Start T8 Timer");
                                    StartT8Timer();
                                    receive_bytes_temp = received - receive_bytes_remain;
                                    section_bytes = new byte[receive_byts];
                                    Array.Copy(memory, 0, section_bytes, 0, received);
                                    section_flag = true;
                                }
                            }
                            else
                            {
                                //第一次黏包
                                byte[] bytes = new byte[receive_byts];
                                Array.Copy(memory, 0, bytes, 0, receive_byts);
                                receive_comp(bytes, cancellation);
                                receive_bytes_remain = (int)receive_byts;
                                //檢查剩下封包
                                while (receive_bytes_remain < received)
                                {
                                    receive_count[0] = memory[receive_bytes_remain + 3];
                                    receive_count[1] = memory[receive_bytes_remain + 2];
                                    receive_count[2] = memory[receive_bytes_remain + 1];
                                    receive_count[3] = memory[receive_bytes_remain + 0];
                                    receive_byts = BitConverter.ToUInt32(receive_count);
                                    receive_byts = receive_byts + 4;
                                    if ((received - receive_bytes_remain) == receive_byts)
                                    {
                                        section_flag = false;
                                        bytes = new byte[receive_byts];
                                        Array.Copy(memory, receive_bytes_remain, bytes, 0, receive_byts);
                                        foreach (var item in bytes)
                                        {
                                            Console.Write(item.ToString("x2") + ", ");
                                        }
                                        Console.WriteLine();
                                        receive_comp(bytes, cancellation);
                                        break;
                                    }
                                    else
                                    {
                                        if ((received - receive_bytes_remain) < receive_byts) // 剩下的資料為封包分段
                                        {
                                            Console.WriteLine("Start T8 Timer");
                                            StartT8Timer();
                                            receive_bytes_temp = received - receive_bytes_remain;
                                            section_bytes = new byte[receive_byts];
                                            Array.Copy(memory, receive_bytes_remain, section_bytes, 0, received);
                                            section_flag = true;
                                            break;
                                        }
                                        else
                                        {
                                                bytes = new byte[receive_byts];
                                                Array.Copy(memory, receive_bytes_remain, bytes, 0, receive_byts);
                                                receive_comp(bytes, cancellation);
                                                receive_bytes_remain += (int)receive_byts;                                            
                                            // 需加入多個封包產生黏包問題

                                        }


                                    }
                                }
                            }
                        }
                        else
                        {
                            byte[] bytes = new byte[received];
                            Array.Copy(memory, 0, bytes, 0, received);
                            receive_comp(bytes, cancellation);
                        }

                        

                    }
                }
            } catch (Exception ex)
            {
                
                if (cancellation.IsCancellationRequested || T8_Enabled)
                {
                    return;
                }
                if(_socket.ReceiveTimeout == T8)
                {
                    Utility.Log(1, "T8 TimeOut...");
                    ChangeConnectionState(ConnectionState.Retry);
                }
                Utility.LogException(ex);
                Utility.Log(1, "DisConnect.....");
                 ChangeConnectionState(ConnectionState.Retry);
            }
        }
        
        private async Task receive_comp(byte[] bytes, CancellationToken cancellation)
        {
            int systemByte;
            if (bytes[9] == 0)
            {
                // DataMessage
                var result = ExtensionHelper.DecodeTo(bytes);
                systemByte = result.Item1.id;
                PrintMessage.MessageIn(result.Item2, systemByte);
                if (result.Item1.DeviceId != DeviceId && result.Item1.S != 9 && result.Item1.F != 1)
                {
                    Utility.Log(5, "Received Unrecognized Device Id Message");
                    SecsMessage DeviceIdErrorMsg = new SecsMessage(9, 1, false)
                    {
                        SecsItem = Item.B(result.Item1.EncodeTo())
                    };
                    int NewsystemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                    SendDataMessageAsync(DeviceIdErrorMsg, NewsystemByte);
                }
                else
                {
                    if (result.Item2.F % 2 != 0)
                    {
                        if (result.Item2.S != 9)
                        {
                            secsLib.ReplyMessage(secsLib.secsFunction[new Tuple<byte, byte>(result.Item2.S, result.Item2.F)], result.Item1, result.Item2, cancellation);
                        }
                        else
                        {
                            var headerBytes = result.Item2.SecsItem._rawData;
                            systemByte = BitConverter.ToInt32(new byte[] { headerBytes[11], headerBytes[10], headerBytes[9], headerBytes[8] }, 0);
                            Utility.Log(5, $"Error SystemBytes: {systemByte}");
                        }

                    }
                    else
                    {
                        //Console.WriteLine("Revice Message : " + result.Item2.ToString());
                        //Console.WriteLine("after_systemByte: " + systemByte);
                        if (_replyExpectedMsgs.TryGetValue(systemByte, out var ar))
                        {
                            ar.SetResult(result.Item2);
                            //ar.HandleReplyMessage(result.Item2);
                        }

                    }
                }
            }
            else
            {
                byte[] header_bytes = new byte[10];
                Array.Copy(bytes, 4, header_bytes, 0, 10);
                MessageHeader header = MessageHeader.decode(header_bytes);
                bool Select_flag = false;
                // ControlMessage
                if ((int)bytes[9] % 2 == 0)
                {
                    if (_replyExpectedMsgs.TryGetValue(header.id, out var ar))
                    {
                        ar.SetResult(ControlMessage_);
                    }
                    else
                    {
                        Utility.Log(5, $"Received Unexpected Control Message: {header.messagetype}");
                        Select_flag = true;
                    }
                }

                Utility.Log(5, $"Receive Cotrol Message: {header.messagetype}");

                switch (header.messagetype)
                {
                    case MessageType.Select_req:
                        systemByte = header.id;
                        await SendControlMessage(MessageType.Select_rsp, systemByte);
                        if (Select_flag)
                        {
                            ChangeConnectionState(ConnectionState.Retry);
                        }
                        else
                        {
                            ChangeConnectionState(ConnectionState.Selected);
                        }

                        break;
                    case MessageType.Select_rsp:
                        switch (header.F)
                        {
                            case 0:
                                ChangeConnectionState(ConnectionState.Selected);
                                break;
                            case 1:
                                Utility.Log(5, "Communication Already Active.");
                                break;
                            case 2:
                                Utility.Log(5, "Connection Not Ready");
                                break;
                            case 3:
                                Utility.Log(5, "Connection Exhaust");
                                break;
                            default:
                                Utility.Log(5, "Connection Status is unknown.");
                                break;
                        }
                        break;
                    case MessageType.Linktest_req:
                        systemByte = header.id;
                        await SendControlMessage(MessageType.Linktest_rsp, systemByte);
                        break;
                    case MessageType.Seperate_req:
                        ChangeConnectionState(ConnectionState.Retry);
                        break;
                    default:
                        break;

                }


            }
        }
        internal async Task<SecsMessage> SendDataMessageAsync(SecsMessage secsMessage, int systemByte)
        {
            if(connectionState != ConnectionState.Selected)
            {
                throw new SecsException("Device is not Selected");
            }

            var token = new TaskCompletionSourceToken(secsMessage, systemByte, MessageType.Data_Message);
            if (secsMessage.ReplyExpected)
            {
                _replyExpectedMsgs[systemByte] = token;
            }

            var header = new MessageHeader()
            {
                S = secsMessage.S,
                F = secsMessage.F,
                ReplyExpection = secsMessage.ReplyExpected,
                DeviceId = DeviceId,
                id = systemByte
            };
            var bufferList = secsMessage.RawDatas.Value;
            bufferList[1] = new ArraySegment<byte>(header.EncodeTo());
            /*
            foreach(var item in bufferList)
            {
                for (int i = item.Offset; i < (item.Offset + item.Count); i++)
                {
                    Console.WriteLine("   [{0}] : {1}", i, item.Array[i]);
                }
                Console.WriteLine("---------------------------------------------");
            }
            */
            var sendEventArgs = new SocketAsyncEventArgs
            {
                BufferList = bufferList,
                UserToken = token,
            };
            PrintMessage.MessageOut(secsMessage, systemByte);
            sendEventArgs.Completed += SendDataMessage_Completed;
            if (!_socket.SendAsync(sendEventArgs))
                SendDataMessage_Completed(_socket, sendEventArgs);
            return await token.Task;
        }

        private async void SendDataMessage_Completed(object sender, SocketAsyncEventArgs e)
        {
            var completeToken = e.UserToken as TaskCompletionSourceToken;
            if(e.SocketError != SocketError.Success)
            {
                completeToken.SetException(new SocketException((int)e.SocketError));
                ChangeConnectionState(ConnectionState.Retry);
                return;
            }

            //Console.WriteLine($"[{DateTime.Now.ToString("yyyyMMddHHmmssfff")}]" + "Send Data Message: " + completeToken.MessageSent.ToString());

            if (!_replyExpectedMsgs.ContainsKey(completeToken.Id))
            {
                
                completeToken.SetResult(null);
                return;
                
            }

            try
            {
                await completeToken.Task.WaitAsync(TimeSpan.FromMilliseconds(T3)).ConfigureAwait(false);
            }
            catch (TimeoutException)
            {
                Utility.Log(4, $"T3 Timeout[id={(uint)completeToken.Id}]: {T3 / 1000} sec.");
                LinkTest_Enabled = false;
                //Test.T3_flag = true;
            }
            finally
            {
                //Console.WriteLine("Out: " + completeToken.Id);
                var test = _replyExpectedMsgs.TryRemove(completeToken.Id, out TaskCompletionSourceToken _);
                //Console.WriteLine("SUCCESS?: " + test);
            }
        }
        public static void StartT8Timer()
        {
            timer8.Interval = T8;
            timer8.Enabled = true;
        }
        public static void StopT8Timer()
        {
            timer8.Stop();
            timer8.Enabled = false;
        }
    }
}
