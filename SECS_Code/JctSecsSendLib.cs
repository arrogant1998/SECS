using Microsoft.AspNetCore.Razor.Hosting;

namespace SECS_Code
{
    public class JctSecsSendLib
    {
        public delegate void SecsSendHandler(CancellationToken cancellation, string tmp);
        public static uint count = Convert.ToUInt32(HsmsConnection.configuration["count"]);
        public Dictionary<Tuple<byte, byte>, SecsSendHandler> secsFunction = new Dictionary<Tuple<byte, byte>, SecsSendHandler>();
        private static int systemByte;
        public JctSecsSendLib()
        {
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

        public void SendMessage(SecsSendHandler secsHandler, CancellationToken cancellation, string tmp)
        {
            try
            {
                secsHandler.Invoke(cancellation, tmp);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS1F1(CancellationToken cancellation, string tmp)
        {
            try
            {
                
                using SecsMessage replyMessage = new SecsMessage(1, 1)
                {

                };
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, $"OnS1F1:{ex.Message}");
            }
        }

        public async void OnS1F3(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 3)
                {
                    SecsItem = Item.L(
                            Item.U4(1000500)
                        )
                };
                //回傳值為TF
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, $"OnS1F3:{ex.Message}");
            }
        }

        public async void OnS1F11(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 11)
                {
                    SecsItem = Item.L(
                        Item.U4(1000500),
                        Item.U4(1000501),
                        Item.U4(1000502) // 所有SVID
                        )
                };
                //回傳值為TF
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }
        public async void OnS1F13(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 13)
                {
                    SecsItem = Item.L(
                            Item.A(""),
                            Item.A("")
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS1F15(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 15)
                {
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, $"OnS1F15{ex.Message}");
            }
        }

        public async void OnS1F17(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(1, 17)
                {
                };
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, $"OnS1F17{ex.Message}");
            }
        }

        public async void OnS2F13(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 13)
                {
                    SecsItem = Item.U4(1, 210)

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F15(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 15)
                {
                    SecsItem = Item.L(
                        Item.L(
                            Item.U4(300001),
                            Item.A("0")
                            )
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F17(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 17)
                {
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F23(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 23)
                {
                    SecsItem = Item.L(
                        Item.U4(1),
                        Item.A("00000100"),
                        Item.U4(count),
                        Item.U4(1),
                        Item.L(
                            Item.U4(1000500)
                            )
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F25(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 25)
                {
                    SecsItem = Item.B(0x00, 0x01, 0x03, 0x03, 0x0a, 0x0d, 0x1b, 0x5d, 0x18, 0x18, 0x18, 0x1a, 0x04, 0x13, 0x7f, 0x80, 0xfe, 0xff)

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F29(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 29)
                {
                    SecsItem = Item.L(
                        Item.U4(1, 220)
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F31(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 31)
                {
                    SecsItem = Item.A(DateTime.Now.ToString("yyyyMMddhhmmssfff"))

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F33(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 33)
                {
                    SecsItem = Item.L(
                        Item.U4(1,1),
                        Item.L(
                            Item.L(
                                Item.U4(1,1),
                                Item.L(
                                    Item.A("810")
                                    )
                                )
                            )
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F35(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 35)
                {
                    SecsItem = Item.L(
                        Item.U4(1,1),
                        Item.L(
                            Item.L(
                                Item.L(
                                    Item.U4(1,1),
                                    Item.U4(1,4050)
                                    )
                                )
                            )
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F37(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 37)
                {
                    SecsItem = Item.L(
                        Item.TF(true),
                        Item.L(
                            Item.U4(1,1)
                            )
                    )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F39(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 39)
                {
                    SecsItem = Item.L(
                        Item.U4(1,1),
                        Item.U4(649)
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F41(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 41)
                {

                    SecsItem = Item.L(
                        Item.A("RCMD"),
                        Item.L(
                            Item.L(
                                Item.A("CPNAME"),
                                Item.A("CPVAL")
                                )
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS2F65(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(2, 65)
                {
                    SecsItem = Item.L(
                        Item.TF(true),
                        Item.A("TRID")
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS4F19(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 19)
                {
                    SecsItem = Item.L(
                        Item.U4(1, 810),
                        Item.L(
                            Item.A("TRJOBNAME"),
                            Item.L(
                                Item.L(
                                    Item.U4(1,810),
                                    Item.U4(1,810),
                                    Item.A("TROBJNAME"),
                                    Item.U4(1, 810),
                                    Item.U1(1,1),
                                    Item.A("TRRCP"),
                                    Item.A("TRPTNR"),
                                    Item.U4(1,810),
                                    Item.U1(1,1),
                                    Item.U1(1,1),
                                    Item.U4(1,810),
                                    Item.TF(true)
                                    )
                                )
                            )
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS4F21(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 21)
                {
                    SecsItem = Item.L(
                            Item.B(0x01),
                            Item.A("TRCMDNAME"),
                            Item.L(
                                Item.L(
                                    Item.A("CPAME"),
                                    Item.A("CPVAL")
                                    )
                                )

                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS4F23(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 23)
                {
                    SecsItem = Item.L(
                        Item.B(0x60),
                        Item.A("TRJOBNAME"),
                        Item.U1(1,1),
                        Item.L(
                            Item.TF(true),
                            Item.L(
                                Item.L(
                                    Item.U4(1,1),
                                    Item.A("ERRTEXT")
                                    )
                                )
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);

            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS4F27(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 27, false)
                {
                    SecsItem = Item.L(
                        Item.A("EQAME"),
                        Item.L(
                            Item.U4(1, 810),
                            Item.U4(1, 810),
                            Item.A("TROBJNAME"),
                            Item.U4(1, 810),
                            Item.U1(1, 1),
                            Item.A("TRPTNR"),
                            Item.U4(1, 810),
                            Item.U1(1, 1),
                            Item.U1(1, 1),
                            Item.U4(1, 810)
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS4F29(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 29, false)
                {
                    SecsItem = Item.L(
                        Item.U4(3),
                        Item.U4(1001),
                        Item.A("HOCMDNAME"),
                        Item.L(
                            Item.L(
                                Item.A("CPNAME"),
                                Item.A("CPVAL")
                                )
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS4F31(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 31, false)
                {
                    SecsItem = Item.L(
                        Item.U4(3),
                        Item.U4(1001),
                        Item.L(
                            Item.TF(true),
                            Item.L(
                                Item.L(
                                    Item.U4(0),
                                    Item.A("ERRTEXT")
                                    )
                                )
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS4F33(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(4, 33, false)
                {
                    SecsItem = Item.L(
                        Item.U4(3),
                        Item.L(
                            Item.TF(true),
                            Item.L(
                                Item.U4(0),
                                Item.A("ERRTEXT")
                                )
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS5F1(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 1)
                {
                    SecsItem = Item.L(
                        Item.B(0x00),
                        Item.U4(10001),
                        Item.A("ALTX")
                        )


                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS5F3(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 3)
                {
                    SecsItem = Item.L(
                        Item.B(0x00),
                        Item.U4(10001)
                        )


                };
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS5F5(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 5)
                {
                    SecsItem = Item.U4(10001)



                };
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS5F7(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 7)
                {
                };
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS5F9(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 9)
                {
                    SecsItem = Item.L(
                        Item.A(DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                        Item.A("EXID"),
                        Item.A("EXTYPE"),
                        Item.A("EXMESSAGE"),
                        Item.L(
                            Item.A("EXRECVRA")
                            )
                        )
                };
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS5F11(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 11)
                {
                    SecsItem = Item.L(
                        Item.A(DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                        Item.A("EXID"),
                        Item.A("EXTYPE"),
                        Item.A("EXMESSAGE")
                        )
                };
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS5F13(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 13)
                {
                    SecsItem = Item.L(
                        Item.A("EXID"),
                        Item.A("EXRECVRA")
                        )
                };
                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS5F15(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 15)
                {
                    SecsItem = Item.L(
                        Item.A(DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                        Item.A("EXID"),
                        Item.L(
                            Item.TF(true),
                            Item.L(
                                Item.U4(0),
                                Item.A("ERRTEXT")
                                )
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS5F17(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(5, 17)
                {
                    SecsItem = Item.A("EXID")

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS6F1(CancellationToken cancellation, string tmp)
        {
            
            try
            {
                using var replyMessage = new SecsMessage(6, 1)
                {
                    /*
                    SecsItem = Item.L(
                        Item.A("TRID"),
                        Item.U4(1),
                        Item.A(DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                        Item.L(
                            Item.U1(1,1)
                            )
                        )
                    */
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS6F5(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 5)
                {
                    SecsItem = Item.L(
                        Item.U4(649),
                        Item.U4(649)
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS6F11(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 11)
                {
                    SecsItem = Item.L(
                        Item.U4(1001),
                        Item.U4(1002),
                        Item.L(
                            Item.L(
                                Item.U4(1003),
                                Item.L(
                                    Item.A("V")
                                    )
                                )
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS6F15(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 15)
                {
                    SecsItem = Item.U4(1, 1)
                        
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS6F19(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 19)
                {
                    SecsItem = Item.U4(649)

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS6F23(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 23)
                {
                    SecsItem = Item.U1(0)

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS6F27(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 27)
                {
                    SecsItem = Item.L(
                        Item.U4(1),
                        Item.A("TRID"),
                        Item.L(
                            Item.L(
                                Item.L(
                                    Item.U4(249),
                                    Item.L(
                                        Item.A("V")
                                        )
                                    )
                                )
                            )
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS6F29(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(6, 29)
                {
                    SecsItem = Item.A("TRID")

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS7F1(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 1)
                {
                    SecsItem = Item.L(
                        Item.A("PPID"),
                        Item.U4(329)
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS7F3(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 3)
                {
                    SecsItem = Item.L(
                        Item.A("PPID"),
                        Item.B(0)
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS7F5(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 5)
                {
                    SecsItem = Item.A("PPID")

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS7F17(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 17)
                {
                    SecsItem = Item.A("PPID")

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS7F19(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 19)
                {
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS7F23(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 23)
                {
                    SecsItem = Item.L(
                        Item.A("PPID"),
                        Item.A("MDLN"),
                        Item.A("SOFTREV"),
                        Item.L(
                            Item.L(
                                Item.A("CCODE"),
                                Item.A("PPARM")
                                )
                            )
                        )

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS7F25(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 25)
                {
                    SecsItem = Item.A("PPID")


                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS7F27(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 27)
                {
                    SecsItem = Item.L(
                        Item.A("PPID"),
                        Item.L(
                            Item.L(
                                Item.U4(1,0),
                                Item.U4(1,1),
                                Item.A("ERRW7")
                                )
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS7F29(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(7, 29)
                {
                    SecsItem = Item.U4(1,322)
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS10F3(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(10, 3)
                {
                    SecsItem = Item.L(
                        Item.B(132),
                        Item.A("Text")
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS10F5(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(10, 5)
                {
                    SecsItem = Item.L(
                        Item.B(0x00),
                        Item.L(
                            Item.A("Text")
                            )
                        )
                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }
        //S9Fy
        /*
        public async void OnS9F3(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(9, 3, false)
                {
                    SecsItem = Item.B(0x00)

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS9F5(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(9, 5, false)
                {
                    SecsItem = Item.B(0x00)

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS9F7(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(9, 7, false)
                {
                    SecsItem = Item.B(0x00)

                };
                await DeviceWorker.secsGem.SendAsync(replyMessage);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS9F9(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(9, 9, false)
                {
                    SecsItem = Item.B(0x00)

                };
                await DeviceWorker.secsGem.SendAsync(replyMessage);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }

        public async void OnS9F11(CancellationToken cancellation, string tmp)
        {
            try
            {
                using var replyMessage = new SecsMessage(9, 11, false)
                {
                    SecsItem = Item.B(0x00)

                };

                systemByte = new Random(Guid.NewGuid().GetHashCode()).Next();
                await Worker.test_Connect.SendDataMessageAsync(replyMessage, systemByte);
            }
            catch (Exception ex)
            {
                Utility.Log(1, ex.Message);
            }
        }
        */
    }

}
