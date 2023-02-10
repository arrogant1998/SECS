namespace SECS_Code
{
    public class Test
    {
        //public static System.Timers.Timer td;
        public static Thread td;
        private static readonly JctSecsSendLib secsLib_send = new();
        public static CancellationTokenSource cancellationToken = new CancellationTokenSource();
        public static CancellationToken token = cancellationToken.Token;
        private static int teat_interval = 1000;
        private static int i = 0;
        public Test()
        {
            /*
            td = new System.Timers.Timer();
            td.Interval = teat_interval;
            td.Elapsed += Send_Thread;
            td.Enabled = true;
            */
            td = new Thread(Send_Thread);
            td.Start();
        }

        public void test()
        {
            while (true)
            {
                /*
                    using SecsMessage s2f23 = new SecsMessage(2, 23)
                    {
                        SecsItem = Item.L(
                            Item.L(
                                Item.A("TRID"),
                                Item.J("JIS8"),
                                Item.TF(true),
                                Item.B(0x00, 0x05, 0x13)
                                ),
                            Item.L(
                                Item.F4(1, 1),
                                Item.F8(2, 2)
                                ),
                            Item.L(
                                Item.I1(1, 1),
                                Item.I2(2, 2),
                                Item.I4(3, 3),
                                Item.I8(4, 4)
                                ),
                            Item.L(
                                Item.U1(1, 1),
                                Item.U2(2, 2),
                                Item.U4(3, 3),
                                Item.U8(4, 4)
                                ),
                            Item.L(
                                Item.A(),
                                Item.J(),
                                Item.TF(),
                                Item.B(),
                                Item.F4(),
                                Item.F8(),
                                Item.I1(),
                                Item.I2(),
                                Item.I4(),
                                Item.I8(),
                                Item.U1(),
                                Item.U2(),
                                Item.U4(),
                                Item.U8()
                                ),
                            Item.L(),
                            Item.L(
                                Item.L()
                                )

                            )
                    };
                MessageHeader header = new MessageHeader(0, s2f23.ReplyExpected, s2f23.S, s2f23.F, MessageType.Select_req, 16);
                byte[] bytes_test = new byte[31] { 0x00, 0x00, 0x00, 0x1b, 0x00, 0x00, 0x01, 0x0e, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 0x02, 0x41, 0x04, 0x54, 0x72, 0x61, 0x79, 0x41, 0x07, 0x31, 0x2e, 0x32, 0x2e, 0x31, 0x2e, 0x31 };

                byte[] test_Comb_header = header.EncodeTo();
                byte[] test_Comb_message = ExtensionHelper.EncodeTo(s2f23.SecsItem);
                byte[] test_Comb = ExtensionHelper.ToCombine(test_Comb_header, test_Comb_message);
                var result = ExtensionHelper.DecodeTo(test_Comb);
                foreach (var item in test_Comb)
                {
                    Console.Write(item.ToString("x2") + " ");
                }
                Console.WriteLine();
                Console.WriteLine("-------------");
                byte[] test = result.Item1.EncodeTo();
                Console.WriteLine("Message Header:");
                foreach (var item in test)
                {
                    Console.Write(item.ToString("x2") + " ");
                }

                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                MessageHeader header_decode = MessageHeader.decode(test);
                Console.WriteLine(header_decode.DeviceId);
                Console.WriteLine(header_decode.S);
                Console.WriteLine(header_decode.F);
                Console.WriteLine(header_decode.ReplyExpection);
                Console.WriteLine(header_decode.messagetype);
                Console.WriteLine(header_decode.id);
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Message:");
                foreach (Item item in result.Item2.SecsItem.Value)
                {
                    if (item.Format == SecsFormat.List)
                    {
                        Console.WriteLine($"{item.Format} {item.Count}------------------------[");
                        foreach (Item item2 in item.Value)
                        {
                            Console.WriteLine($"{item2.Format} {item2.Count}: {item2.Value}");
                            foreach (var bytes in item2._rawData)
                            {
                                Console.Write(bytes.ToString("x2") + " ");
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine($"]");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Format} {item.Count}: {item.Value}");
                        foreach (var bytes in item._rawData)
                        {
                            Console.Write(bytes.ToString("x2") + " ");
                        }
                        Console.WriteLine();
                    }
                }

                */
                /*
                byte[] test = HsmsConnection.SendControlMessage((MessageType)type[i]);
                foreach (var item in test)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
                if(i < 4)
                {
                    i++;
                }
                else
                {
                    i = 0;
                }


                
                */

                Thread.Sleep(teat_interval);
            }
        }
        //object sender, EventArgs e
        static void Send_Thread()
        {
            //secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(1, 1)], token, "");
            Thread.Sleep(teat_interval * 5);
            while (true)
            {
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(1, 1)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(1, 3)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(1, 11)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(1, 13)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(1, 15)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(1, 17)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 13)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 15)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 17)], token, "");
                Thread.Sleep(teat_interval);
                
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 23)], token, "");
                Thread.Sleep(teat_interval);
                
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 25)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 29)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 31)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 33)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 35)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 37)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 39)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 41)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(2, 65)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(4, 19)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(4, 21)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(4, 23)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(4, 27)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(4, 29)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(4, 31)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(4, 33)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(5, 1)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(5, 3)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(5, 5)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(5, 7)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(5, 9)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(5, 11)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(5, 13)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(5, 15)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(5, 17)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(6, 1)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(6, 5)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(6, 11)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(6, 15)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(6, 19)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(6, 23)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(6, 27)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(6, 29)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(7, 1)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(7, 3)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(7, 5)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(7, 17)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(7, 19)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(7, 23)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(7, 25)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(7, 27)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(7, 29)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(10, 3)], token, "");
                Thread.Sleep(teat_interval);
                secsLib_send.SendMessage(secsLib_send.secsFunction[new Tuple<byte, byte>(10, 5)], token, "");
                Thread.Sleep(teat_interval);

            }
        }
    }
}
