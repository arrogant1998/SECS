namespace SECS_Code
{
    public class PrintMessage
    {
        private static string temp = "    ";
        private static bool isList = false;
        public static void MessageOut(SecsMessage secsMessage, int id)
        {
            uint u_id = (uint)id;
            try
            {
                Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] Send {secsMessage.ToString()} id:({u_id})");
                Console.WriteLine($"{secsMessage.ToString()}");
                if (secsMessage.SecsItem != null)
                {
                    if (secsMessage.SecsItem.Format == SecsFormat.L)
                    {
                        Console.WriteLine($"<L [{secsMessage.SecsItem.Count}] ");
                        foreach (Item item in secsMessage.SecsItem.Value)
                        {
                            if (item.Format == SecsFormat.L)
                            {
                                Console.WriteLine($"    <L [{item.Count}] ");
                                foreach (Item item2 in item.Value)
                                {
                                    if (item2.Format == SecsFormat.L)
                                    {
                                        ThirdList(item2, 3);
                                    }
                                    else
                                    {
                                        if (item2.Format == SecsFormat.A || item2.Format == SecsFormat.JIS8)
                                        {
                                            Console.WriteLine($"        <{item2.Format} [{item2.Count}] \"{item2.Value}\">");
                                        }
                                        else
                                        {

                                            Console.Write($"        <{item2.Format} [{item2.Count}] ");
                                            if (item2.Format == SecsFormat.B)
                                            {
                                                foreach (byte bytes in item2.Value)
                                                {
                                                    Console.Write("0x" + bytes.ToString("x2") + " ");
                                                }
                                                Console.WriteLine(">");
                                            }
                                            else if (item2.Format == SecsFormat.TF)
                                            {
                                                foreach (var bytes in item2.Value)
                                                {
                                                    if ((bool)bytes)
                                                    {
                                                        Console.Write("0x01" + " ");
                                                    }
                                                    else
                                                    {
                                                        Console.Write("0x00" + " ");
                                                    }

                                                }
                                                Console.WriteLine(">");
                                            }
                                            else
                                            {
                                                foreach (var bytes in item2.Value)
                                                {
                                                    Console.Write(bytes + " ");
                                                }
                                                Console.WriteLine(">");
                                            }

                                        }
                                    }

                                }
                                Console.WriteLine($"    >");
                            }
                            else
                            {

                                if (item.Format == SecsFormat.A || item.Format == SecsFormat.JIS8)
                                {
                                    Console.WriteLine($"    <{item.Format} [{item.Count}] \"{item.Value}\">");
                                }
                                else
                                {
                                    Console.Write($"    <{item.Format} [{item.Count}] ");
                                    if (item.Format == SecsFormat.B)
                                    {
                                        foreach (byte bytes in item.Value)
                                        {
                                            Console.Write("0x" + bytes.ToString("x2") + " ");
                                        }
                                        Console.WriteLine(">");
                                    }
                                    else if (item.Format == SecsFormat.TF)
                                    {
                                        foreach (var bytes in item.Value)
                                        {
                                            if ((bool)bytes)
                                            {
                                                Console.Write("0x01" + " ");
                                            }
                                            else
                                            {
                                                Console.Write("0x00" + " ");
                                            }

                                        }
                                        Console.WriteLine(">");
                                    }
                                    else
                                    {
                                        foreach (var bytes in item.Value)
                                        {
                                            Console.Write(bytes + " ");
                                        }
                                        Console.WriteLine(">");
                                    }
                                }
                            }
                        }
                        Console.WriteLine(">");
                    }
                    else
                    {
                        //第一層非List
                        if (secsMessage.SecsItem.Format == SecsFormat.A || secsMessage.SecsItem.Format == SecsFormat.JIS8)
                        {
                            Console.WriteLine($"<{secsMessage.SecsItem.Format} [{secsMessage.SecsItem.Count}] \"{secsMessage.SecsItem.Value}\">");
                        }
                        else
                        {
                            Console.Write($"<{secsMessage.SecsItem.Format} [{secsMessage.SecsItem.Count}] ");
                            if (secsMessage.SecsItem.Format == SecsFormat.B)
                            {
                                foreach (byte bytes in secsMessage.SecsItem.Value)
                                {
                                    Console.Write("0x" + bytes.ToString("x2") + " ");
                                }
                                Console.WriteLine(">");
                            }
                            else if (secsMessage.SecsItem.Format == SecsFormat.TF)
                            {
                                foreach (var bytes in secsMessage.SecsItem.Value)
                                {
                                    if ((bool)bytes)
                                    {
                                        Console.Write("0x01" + " ");
                                    }
                                    else
                                    {
                                        Console.Write("0x00" + " ");
                                    }

                                }
                                Console.WriteLine(">");
                            }
                            else
                            {
                                foreach (var bytes in secsMessage.SecsItem.Value)
                                {
                                    Console.Write(bytes + " ");
                                }
                                Console.WriteLine(">");
                            }
                        }
                    }
                }


                Console.WriteLine(".");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"MessageOut:");
                Utility.LogException(ex);
            }
        }
        public static void MessageIn(SecsMessage secsMessage, int id)
        {
            uint u_id = (uint)id;
            Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss.fff")}] Receive {secsMessage.ToString()} id:({u_id})");
            Console.WriteLine($"{secsMessage.ToString()}");
            if(secsMessage.SecsItem != null)
            {
                if (secsMessage.SecsItem.Format == SecsFormat.L)
                {
                    Console.WriteLine($"<L [{secsMessage.SecsItem.Count}] ");
                    foreach (Item item in secsMessage.SecsItem.Value)
                    {
                        if (item.Format == SecsFormat.L)
                        {
                            Console.WriteLine($"    <L [{item.Count}] ");
                            foreach (Item item2 in item.Value)
                            {
                                if (item2.Format == SecsFormat.L)
                                {
                                    ThirdList(item2, 3);
                                }
                                else
                                {
                                    if (item2.Format == SecsFormat.A || item2.Format == SecsFormat.JIS8)
                                    {
                                        Console.WriteLine($"        <{item2.Format} [{item2.Count}] \"{item2.Value}\">");
                                    }
                                    else
                                    {

                                        Console.Write($"        <{item2.Format} [{item2.Count}] ");
                                        if (item2.Format == SecsFormat.B)
                                        {
                                            foreach (byte bytes in item2.Value)
                                            {
                                                Console.Write("0x" + bytes.ToString("x2") + " ");
                                            }
                                            Console.WriteLine(">");
                                        }
                                        else if(item2.Format == SecsFormat.TF)
                                        {
                                            foreach (var bytes in item2.Value)
                                            {
                                                if ((bool)bytes)
                                                {
                                                    Console.Write("0x01" + " ");
                                                }
                                                else
                                                {
                                                    Console.Write("0x00" + " ");
                                                }
                                                
                                            }
                                            Console.WriteLine(">");
                                        }
                                        else
                                        {
                                            foreach (var bytes in item2.Value)
                                            {
                                                Console.Write(bytes + " ");
                                            }
                                            Console.WriteLine(">");
                                        }

                                    }
                                }

                            }
                            Console.WriteLine($"    >");
                        }
                        else
                        {
                            if (item.Format == SecsFormat.A || item.Format == SecsFormat.JIS8)
                            {
                                Console.WriteLine($"    <{item.Format} [{item.Count}] \"{item.Value}\">");
                            }
                            else
                            {
                                Console.Write($"    <{item.Format} [{item.Count}] ");
                                if (item.Format == SecsFormat.B)
                                {
                                    foreach (byte bytes in item.Value)
                                    {
                                        Console.Write("0x" + bytes.ToString("x2") + " ");
                                    }
                                    Console.WriteLine(">");
                                }
                                else if (item.Format == SecsFormat.TF)
                                {
                                    foreach (var bytes in item.Value)
                                    {
                                        if ((bool)bytes)
                                        {
                                            Console.Write("0x01" + " ");
                                        }
                                        else
                                        {
                                            Console.Write("0x00" + " ");
                                        }

                                    }
                                    Console.WriteLine(">");
                                }
                                else
                                {
                                    foreach (var bytes in item.Value)
                                    {
                                        Console.Write(bytes.ToString() + " ");
                                    }
                                    Console.WriteLine(">");
                                }
                            }
                            /*
                            foreach (var bytes in item._rawData)
                            {
                                Console.Write(bytes.ToString("x2") + " ");
                            }
                            Console.WriteLine();
                            */
                        }
                    }
                    Console.WriteLine(">");
                }
                else
                {
                    if (secsMessage.SecsItem.Format == SecsFormat.A || secsMessage.SecsItem.Format == SecsFormat.JIS8)
                    {
                        Console.WriteLine($"<{secsMessage.SecsItem.Format} [{secsMessage.SecsItem.Count}] \"{secsMessage.SecsItem.Value}\">");
                    }
                    else
                    {
                        Console.Write($"<{secsMessage.SecsItem.Format} [{secsMessage.SecsItem.Count}] ");
                        if (secsMessage.SecsItem.Format == SecsFormat.B)
                        {
                            foreach (byte bytes in secsMessage.SecsItem.Value)
                            {
                                Console.Write("0x" + bytes.ToString("x2") + " ");
                            }
                            Console.WriteLine(">");
                        }
                        else if (secsMessage.SecsItem.Format == SecsFormat.TF)
                        {
                            foreach (var bytes in secsMessage.SecsItem.Value)
                            {
                                if ((bool)bytes)
                                {
                                    Console.Write("0x01" + " ");
                                }
                                else
                                {
                                    Console.Write("0x00" + " ");
                                }

                            }
                            Console.WriteLine(">");
                        }
                        else
                        {
                            foreach (var bytes in secsMessage.SecsItem.Value)
                            {
                                Console.Write(bytes.ToString() + " ");
                            }
                            Console.WriteLine(">");
                        }
                    }
                }
            }
            
            
            Console.WriteLine(".");
        }

        public static void ThirdList(Item item, int count)
        {
            PrintTemp(count);
            Console.WriteLine($"<L [{item.Count}] ");
            foreach (Item item1 in item.Value)
            {
                if (item1.Format == SecsFormat.L)
                {
                    ThirdList(item1, count+1);
                }
                else
                {
                    if (item1.Format == SecsFormat.A || item1.Format == SecsFormat.JIS8)
                    {
                        PrintTemp(count);
                        Console.WriteLine($"    <{item1.Format} [{item1.Count}] \"{item1.Value}\">");
                    }
                    else
                    {
                        PrintTemp(count);
                        Console.Write($"    <{item1.Format} [{item1.Count}] ");
                        if (item1.Format == SecsFormat.B)
                        {
                            foreach (byte bytes in item1.Value)
                            {
                                Console.Write("0x" + bytes.ToString("x2") + " ");
                            }
                            Console.WriteLine(">");
                        }
                        else if (item1.Format == SecsFormat.TF)
                        {
                            foreach (var bytes in item1.Value)
                            {
                                if ((bool)bytes)
                                {
                                    Console.Write("0x01" + " ");
                                }
                                else
                                {
                                    Console.Write("0x00" + " ");
                                }

                            }
                            Console.WriteLine(">");
                        }
                        else
                        {
                            foreach (var bytes in item1.Value)
                            {
                                Console.Write(bytes.ToString() + " ");
                            }
                            Console.WriteLine(">");
                        }

                    }
                }

            }
            PrintTemp(count);
            Console.WriteLine(">");

        }

        public static void PrintTemp(int count)
        {
            for(int i = 0; i < count; i++)
            {
                Console.Write(temp);
            }
        }
    }
}
