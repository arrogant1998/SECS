namespace SECS_Code
{
    public class JctSecsReplyLib
    {
        public delegate void SecsHandler(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation);
        public Dictionary<Tuple<byte, byte>, SecsHandler> secsFunction = new Dictionary<Tuple<byte, byte>, SecsHandler>();

        public JctSecsReplyLib()
        {
            try
            {
                /* ##動態增加物件方法
                string _tmp = "'S2F42'W\r\n<L [2]\r\n<A [1] '1'>\r\n<A [1] '0'>\r\n>\r\n.";
                SecsMessage tmp_ = _tmp.ToSecsMessage();
                Console.WriteLine(tmp_);
                using var replyMessage = new SecsMessage(1, 2, false)--> 是否需要回傳 false 為不需要回傳
                */
                secsFunction.Add(new Tuple<byte, byte>(1, 1), OnS1F1);
                secsFunction.Add(new Tuple<byte, byte>(1, 3), OnS1F3);
                secsFunction.Add(new Tuple<byte, byte>(1, 11), OnS1F11);
                secsFunction.Add(new Tuple<byte, byte>(1, 13), OnS1F13);
                secsFunction.Add(new Tuple<byte, byte>(1, 15), OnS1F15);
                secsFunction.Add(new Tuple<byte, byte>(1, 17), OnS1F17);
                secsFunction.Add(new Tuple<byte, byte>(2, 13), OnS2F13);
                secsFunction.Add(new Tuple<byte, byte>(2, 15), OnS2F15);
                secsFunction.Add(new Tuple<byte, byte>(2, 17), OnS2F17);
                secsFunction.Add(new Tuple<byte, byte>(2, 23), OnS2F23);
                secsFunction.Add(new Tuple<byte, byte>(2, 25), OnS2F25);
                secsFunction.Add(new Tuple<byte, byte>(2, 29), OnS2F29);
                secsFunction.Add(new Tuple<byte, byte>(2, 31), OnS2F31);
                secsFunction.Add(new Tuple<byte, byte>(2, 33), OnS2F33);
                secsFunction.Add(new Tuple<byte, byte>(2, 35), OnS2F35);
                secsFunction.Add(new Tuple<byte, byte>(2, 37), OnS2F37);
                secsFunction.Add(new Tuple<byte, byte>(2, 39), OnS2F39);
                secsFunction.Add(new Tuple<byte, byte>(2, 41), OnS2F41);
                secsFunction.Add(new Tuple<byte, byte>(2, 65), OnS2F65);
                secsFunction.Add(new Tuple<byte, byte>(4, 19), OnS4F19);
                secsFunction.Add(new Tuple<byte, byte>(4, 21), OnS4F21);
                secsFunction.Add(new Tuple<byte, byte>(4, 23), OnS4F23);
                secsFunction.Add(new Tuple<byte, byte>(4, 27), OnS4F27);
                secsFunction.Add(new Tuple<byte, byte>(4, 29), OnS4F29);
                secsFunction.Add(new Tuple<byte, byte>(4, 31), OnS4F31);
                secsFunction.Add(new Tuple<byte, byte>(4, 33), OnS4F33);
                secsFunction.Add(new Tuple<byte, byte>(5, 1), OnS5F1);
                secsFunction.Add(new Tuple<byte, byte>(5, 3), OnS5F3);
                secsFunction.Add(new Tuple<byte, byte>(5, 5), OnS5F5);
                secsFunction.Add(new Tuple<byte, byte>(5, 7), OnS5F7);
                secsFunction.Add(new Tuple<byte, byte>(5, 9), OnS5F9);
                secsFunction.Add(new Tuple<byte, byte>(5, 11), OnS5F11);
                secsFunction.Add(new Tuple<byte, byte>(5, 13), OnS5F13);
                secsFunction.Add(new Tuple<byte, byte>(5, 15), OnS5F15);
                secsFunction.Add(new Tuple<byte, byte>(5, 17), OnS5F17);
                secsFunction.Add(new Tuple<byte, byte>(6, 1), OnS6F1);
                secsFunction.Add(new Tuple<byte, byte>(6, 5), OnS6F5);
                secsFunction.Add(new Tuple<byte, byte>(6, 11), OnS6F11);
                secsFunction.Add(new Tuple<byte, byte>(6, 15), OnS6F15);
                secsFunction.Add(new Tuple<byte, byte>(6, 19), OnS6F19);
                secsFunction.Add(new Tuple<byte, byte>(6, 23), OnS6F23);
                secsFunction.Add(new Tuple<byte, byte>(6, 27), OnS6F27);
                secsFunction.Add(new Tuple<byte, byte>(6, 29), OnS6F29);
                secsFunction.Add(new Tuple<byte, byte>(7, 1), OnS7F1);
                secsFunction.Add(new Tuple<byte, byte>(7, 3), OnS7F3);
                secsFunction.Add(new Tuple<byte, byte>(7, 5), OnS7F5);
                secsFunction.Add(new Tuple<byte, byte>(7, 17), OnS7F17);
                secsFunction.Add(new Tuple<byte, byte>(7, 19), OnS7F19);
                secsFunction.Add(new Tuple<byte, byte>(7, 23), OnS7F23);
                secsFunction.Add(new Tuple<byte, byte>(7, 25), OnS7F25);
                secsFunction.Add(new Tuple<byte, byte>(7, 27), OnS7F27);
                secsFunction.Add(new Tuple<byte, byte>(7, 29), OnS7F29);
                secsFunction.Add(new Tuple<byte, byte>(10, 3), OnS10F3);
                secsFunction.Add(new Tuple<byte, byte>(10, 5), OnS10F5);
            }
            catch (Exception ex)
            {
                Utility.LogException(ex);
            }
        }

        public void ReplyMessage(SecsHandler secsHandler, MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                secsHandler.Invoke(header, secsMessage, cancellation);

            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS1F1(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 2, false)
                {
                    SecsItem = Item.L(
                        Item.A("MDLN"),
                        Item.A("1.0")
                        )
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS1F3(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 4, false)
                {
                    SecsItem = Item.U1(1, 65)
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
                Utility.LogException(ex);
            }
        }

        public async void OnS1F11(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 12, false)
                {
                    SecsItem = Item.L(
                        Item.U4(1),
                        Item.A("SVNAME"),
                        Item.A("UNITS")
                        )
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }
        public async void OnS1F13(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 14, false)
                {
                    SecsItem = Item.L(
                        Item.B(0x00),
                        Item.L(
                            )
                        )
                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);

            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS1F15(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 16, false)
                {
                    SecsItem = Item.B(0)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);

            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS1F17(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 18, false)
                {
                    SecsItem = Item.B(0)

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);

            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F13(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 14, false)
                {
                    SecsItem = Item.A("ECV")

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F15(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 16, false)
                {
                    SecsItem = Item.B(0)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F17(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 18, false)
                {
                    SecsItem = Item.A(DateTime.Now.ToString("yyyyMMddhhmmssfff"))
                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F23(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            uint TRID = 0;
            string DSPER = ""; //time
            uint TOTSMP = 0;   //count
            uint REPGSZ = 0;
            List<uint> SVID = new();
            double time = 0.0;
            int i = 0;
            try
            {
                foreach(Item item in secsMessage.SecsItem.Value)
                {
                    if(item.Format == SecsFormat.L)
                    {
                        foreach (Item bytes in item.Value)
                        {
                            Console.WriteLine($"byte_Count: {bytes.Count}");
                            foreach (var item1 in bytes.Value)
                            {
                                Console.WriteLine($"item1: {item1}");
                                SVID.Add((uint)item1);
                            }                            
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            foreach (var bytes in item.Value)
                            {
                                TRID = (uint)bytes;
                            }
                        }else if(i == 1)
                        {
                            DSPER = (string)item.Value;
                        }
                        else if (i == 2)
                        {
                            foreach (var bytes in item.Value)
                            {
                                TOTSMP = (uint)bytes;
                            }
                        }
                        else if (i == 3)
                        {
                            foreach (var bytes in item.Value)
                            {
                                REPGSZ = (uint)bytes;
                            }
                        }

                        i++;
                    }
                }
                Console.WriteLine($"TRID: {TRID}");
                Console.WriteLine($"DSPER: {DSPER}");
                Console.WriteLine($"TOTSMP: {TOTSMP}");
                Console.WriteLine($"REPGSZ: {REPGSZ}");
                Console.Write("SVID: ");
                foreach(var item in SVID)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                if (DSPER.Length == 8)
                {
                    time = TimeSpan.ParseExact(DSPER, "hhmmssff", null).TotalSeconds * 1000;
                }
                else if (DSPER.Length == 6)
                {
                    time = TimeSpan.ParseExact(DSPER, "hhmmss", null).TotalSeconds * 1000;
                }
                SendData.time = (int)time;
                HsmsConnection.sendData.count = (int)TOTSMP;
                
                if (HsmsConnection.sendData.count != 0 && SendData.time != 0)
                {
                    SendData.send_flag = true;
                }
                else
                {
                    HsmsConnection.sendData.i = 0;
                    SendData.send_flag = false;
                }
                
                using var replyMessage = new SecsMessage(2, 24, false)
                {
                    SecsItem = Item.B(0)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, header.id);
                //await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F25(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 26, false)
                {
                    SecsItem = Item.B(0x00, 0x01, 0x03, 0x03, 0x0a, 0x0d, 0x1b, 0x5d, 0x18, 0x18, 0x18, 0x1a, 0x04, 0x13, 0x7f, 0x80, 0xfe, 0xff)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F29(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 30, false)
                {
                    SecsItem = Item.L(
                        Item.U4(1, 220),
                        Item.A("ECNAME"),
                        Item.A("ECMIN"),
                        Item.A("ECMAX"),
                        Item.A("ECDEF"),
                        Item.A("UNITS")
                        )

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F31(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {

                var str = string.Join("", secsMessage.SecsItem.Value);
                Console.WriteLine(str);
                byte success = 0;
                //Win10 無法使用
                WinAPI.SystemTime systemTime = new WinAPI.SystemTime()
                {
                    wYear = ushort.Parse(str.Substring(0, 4)),
                    wMonth = ushort.Parse(str.Substring(4, 2)),
                    wDay = ushort.Parse(str.Substring(6, 2)),
                    wHour = ushort.Parse(str.Substring(8, 2)),
                    wMinute = ushort.Parse(str.Substring(10, 2)),
                    wSecond = ushort.Parse(str.Substring(12, 2))
                };
                if (!WinAPI.SetLocalTime(ref systemTime))
                {
                    success = 1;
                }
                else
                {
                    success = 0;
                }
                using var replyMessage = new SecsMessage(2, 32, false)
                {
                    SecsItem = Item.B(success)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F33(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 34, false)
                {
                    SecsItem = Item.B(0)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F35(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 36, false)
                {
                    SecsItem = Item.B(0)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F37(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 38, false)
                {
                    SecsItem = Item.B(0)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F39(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 40, false)
                {
                    SecsItem = Item.B(0)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F41(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {


                using var replyMessage = new SecsMessage(2, 42, false)
                {

                    SecsItem = Item.L(
                        Item.B(0),
                        Item.L(
                            Item.A("1"),
                            Item.A("0")
                            )
                        )
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS2F65(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 66, false)
                {
                    SecsItem = Item.B(0)

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS4F19(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 20, false)
                {
                    SecsItem = Item.L(
                        Item.B(0),
                        Item.L(
                            Item.U4(1, 101)
                            ),
                        Item.L(
                            Item.TF(true),
                            Item.L(
                                Item.L(
                                Item.U4(0),
                                Item.A("no error")
                                )
                                )
                            )
                        )

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS4F21(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 22, false)
                {
                    SecsItem = Item.L(
                            Item.TF(true),
                            Item.L(
                                Item.L(
                                    Item.U4(0),
                                    Item.A("no error")
                                    )
                                )

                        )

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS4F23(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 24, false)
                {

                };

                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS4F27(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 28, false)
                {

                };

            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS4F29(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 30, false)
                {

                };

            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS4F31(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 32, false)
                {

                };

            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS4F33(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 34, false)
                {

                };

            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS5F1(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 2, false)
                {
                    SecsItem = Item.B(1)


                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS5F3(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 4, false)
                {
                    SecsItem = Item.B(1)


                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS5F5(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 6, false)
                {
                    SecsItem = Item.L(
                        Item.L(
                            Item.B(1),
                            Item.U4(1000),
                            Item.A("ALTX")
                            )
                        )


                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS5F7(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 8, false)
                {
                    SecsItem = Item.L(
                        Item.L(
                            Item.B(1),
                            Item.U4(1000),
                            Item.A("ALTX")
                            )
                        )


                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS5F9(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 10, false)
                {
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS5F11(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 12, false)
                {
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS5F13(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 14, false)
                {
                    SecsItem = Item.L(
                        Item.A("10"),
                        Item.L(
                            Item.TF(true),
                            Item.L(
                                Item.U4(0),
                                Item.A("ERRTEXT")
                                )
                            )
                        )
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS5F15(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 16, false)
                {
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS5F17(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 18, false)
                {
                    SecsItem = Item.L(
                        Item.A("10"),
                        Item.L(
                            Item.TF(true),
                            Item.L(
                                Item.U4(0),
                                Item.A("ERRTEXT")
                                )
                            )
                        )
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS6F1(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 2, false)
                {
                    SecsItem = Item.B(0)
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS6F5(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 6, false)
                {
                    SecsItem = Item.B(0)
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS6F11(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 12, false)
                {
                    SecsItem = Item.B(0)
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS6F15(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 16, false)
                {
                    SecsItem = Item.L(
                        Item.U4(1, 1),
                        Item.U4(1, 4050),
                        Item.L(
                            Item.L(
                                Item.U4(1, 1),
                                Item.L(
                                    Item.A("V")
                                    )
                                )
                            )
                        )
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS6F19(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 20, false)
                {
                    SecsItem = Item.L(
                        Item.A("V")
                        )

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS6F23(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 24, false)
                {
                    SecsItem = Item.B(0)

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS6F27(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 28, false)
                {
                    SecsItem = Item.A("TRID")

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS6F29(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 30, false)
                {
                    SecsItem = Item.L(
                        Item.A("TRID"),
                        Item.L(
                            Item.L(
                                Item.U4(1, 1),
                                Item.L(
                                    Item.A("V")
                                    )
                                )
                            ),
                        Item.U4(0)
                        )

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS7F1(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 2, false)
                {
                    SecsItem = Item.B(0)

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS7F3(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 4, false)
                {
                    SecsItem = Item.B(10)

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS7F5(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 6, false)
                {
                    SecsItem = Item.L(
                        Item.A("RRID"),
                        Item.B(0)
                        )

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS7F17(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 18, false)
                {
                    SecsItem = Item.B(0)

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS7F19(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 20, false)
                {
                    SecsItem = Item.L(
                        Item.A("PPID")
                        )

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS7F23(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 24, false)
                {
                    SecsItem = Item.B(0)

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS7F25(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 26, false)
                {
                    SecsItem = Item.L(
                        Item.A("PPID"),
                        Item.A("gemsim"),
                        Item.A("1.0"),
                        Item.L(
                            Item.L(
                                Item.A("{50 54}"),
                                Item.L(
                                    Item.A("{185.0}")
                                    )
                                )
                            )
                        )

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS7F27(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 28, false)
                {

                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS7F29(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 30, false)
                {
                    SecsItem = Item.B(0)
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS10F3(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(10, 4, false)
                {
                    SecsItem = Item.B(0)
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

        public async void OnS10F5(MessageHeader header, SecsMessage secsMessage, CancellationToken cancellation)
        {
            try
            {
                using var replyMessage = new SecsMessage(10, 6, false)
                {
                    SecsItem = Item.B(0)
                };
                header.S = replyMessage.S;
                header.F = replyMessage.F;
                header.ReplyExpection = replyMessage.ReplyExpected;
                byte[] replybytes = ExtensionHelper.ToCombine(header.EncodeTo(), ExtensionHelper.EncodeTo(replyMessage.SecsItem));
                await HsmsConnection._socket.SendAsync(replybytes);
                PrintMessage.MessageOut(replyMessage, header.id);
            }
            catch (Exception ex)
            {
               Utility.LogException(ex);
            }
        }

    }
}
