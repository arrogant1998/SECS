using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SECS_Code
{
    public static class ExtensionHelper
    {
        public static Item BytesDecode(this SecsFormat format)
        {
            switch (format)
            {
                case SecsFormat.A: return Item.A();
                case SecsFormat.JIS8: return Item.J();
                case SecsFormat.B: return Item.TF();
                case SecsFormat.TF: return Item.B();
                case SecsFormat.U1: return Item.U1();
                case SecsFormat.U2: return Item.U2();
                case SecsFormat.U4: return Item.U4();
                case SecsFormat.U8: return Item.U8();
                case SecsFormat.I1: return Item.I1();
                case SecsFormat.I2: return Item.I2();
                case SecsFormat.I4: return Item.I4();
                case SecsFormat.I8: return Item.I8();
                case SecsFormat.F4: return Item.F4();
                case SecsFormat.F8: return Item.F8();
            }
            throw new ArgumentException("Invalid format:" + format, nameof(format));
        }

        public static Item BytesDecode(this SecsFormat format, byte[] bytes, int index, int length)
        {
            switch (format)
            {
                case SecsFormat.A: return Item.A(Encoding.ASCII.GetString(bytes, index, length));
                case SecsFormat.JIS8: return Item.J(Item.Jis8Encoding.GetString(bytes, index, length));
                case SecsFormat.TF: return Item.TF(Decode<bool>(sizeof(bool), bytes, index, length));
                case SecsFormat.B: return Item.B(Decode<byte>(sizeof(byte), bytes, index, length));
                case SecsFormat.U1: return Item.U1(Decode<byte>(sizeof(byte), bytes, index, length));
                case SecsFormat.U2: return Item.U2(Decode<ushort>(sizeof(ushort), bytes, index, length));
                case SecsFormat.U4: return Item.U4(Decode<uint>(sizeof(uint), bytes, index, length));
                case SecsFormat.U8: return Item.U8(Decode<ulong>(sizeof(ulong), bytes, index, length));
                case SecsFormat.I1: return Item.I1(Decode<sbyte>(sizeof(sbyte), bytes, index, length));
                case SecsFormat.I2: return Item.I2(Decode<short>(sizeof(short), bytes, index, length));
                case SecsFormat.I4: return Item.I4(Decode<int>(sizeof(int), bytes, index, length));
                case SecsFormat.I8: return Item.I8(Decode<long>(sizeof(long), bytes, index, length));
                case SecsFormat.F4: return Item.F4(Decode<float>(sizeof(float), bytes, index, length));
                case SecsFormat.F8: return Item.F8(Decode<double>(sizeof(double), bytes, index, length));
            }
            throw new ArgumentException("Invalid format", nameof(format));
        }

        static T[] Decode<T>(int elmSize, byte[] bytes, int index, int length) where T : struct
        {
            bytes.Reverse(index, index + length, elmSize);
            var values = new T[length / elmSize];
            Buffer.BlockCopy(bytes, index, values, 0, length);
            return values;
        }



        public static void Reverse(this byte[] bytes, int begin, int end, int offSet)
        {
            if(offSet > 1)
            {
                for(int i = begin; i < end; i+= offSet)
                {
                    Array.Reverse(bytes, i, offSet);
                }
            }
        }

        public static Tuple<byte[], int>  EncodeItem(SecsFormat format, int valueCount)
        {
            byte[] Bytes_Length = BitConverter.GetBytes(valueCount);
            int dataLength;
            if(format == SecsFormat.L)
            {
                dataLength = 0;
            }
            else
            {
                dataLength = valueCount;
            }

            if(valueCount <= 0xFF)
            {
                //資料量 只有 1 byte 以下 
                /*
                 * MessageType + 資料數量 只需要兩格
                 * ex: L 3
                 * bytes => |01|03|
                 * A 5 "hello"
                 * bytes => |41|05|68|65|6C|6C|6F|
                 * 
                 * 所以總格數為 資料數量 + 2
                 */
                var result = new byte[dataLength + 2];
                result[0] = (byte)((byte)format | 1); // --> 相當於+1
                result[1] = Bytes_Length[0];
                return new Tuple<byte[], int>(result, 2);
            }
            if (valueCount <= 0xFFFF)
            {// 2byte
                var result = new byte[dataLength + 3];
                result[0] = (byte)((byte)format | 2); // --> 相當於+ 2 (資料量)
                result[1] = Bytes_Length[1];
                result[2] = Bytes_Length[0];
                return new Tuple<byte[], int>(result, 3);
            }
            if (valueCount <= 0xFFFFFF)
            {// 3byte
                var result = new byte[dataLength + 4];
                result[0] = (byte)((byte)format | 3); // --> 相當於+ 3 (資料量)
                result[1] = Bytes_Length[2];
                result[2] = Bytes_Length[1];
                result[3] = Bytes_Length[0];
                return new Tuple<byte[], int>(result, 4);
            }

            throw new ArgumentOutOfRangeException(nameof(valueCount), valueCount, $"Item data length({valueCount}) is overflow");
        }

        public static Item Decode(byte[] bytes, ref int index)
        {
            var format = (SecsFormat)(bytes[index] & 0xFC);
            var lengthBits = (byte)(bytes[index] & 3);
            index++;

            var itemLengthBytes = new byte[4];
            Array.Copy(bytes, index, itemLengthBytes, 0, lengthBits);
            Array.Reverse(itemLengthBytes, 0, lengthBits);
            int length = BitConverter.ToInt32(itemLengthBytes, 0);
            index += lengthBits;

            if(format == SecsFormat.L)
            {
                if(length == 0)
                {
                    return Item.L();
                }
                var list = new List<Item>(length);
                for(int i = 0; i < length; i++)
                {
                    list.Add(Decode(bytes, ref index));
                }
                return Item.L(list);
            }
            var item = length == 0? format.BytesDecode() : format.BytesDecode(bytes, index, length);
            index += length;
            return item;
        }


        public static Tuple<MessageHeader, SecsMessage> DecodeTo(byte[] bytes)
        {
            byte[] Message_header = new byte[10];
            Array.Copy(bytes, 4, Message_header, 0, 10);
            MessageHeader message_header = null;
            SecsMessage secsMessage = null;
            // 需加入判斷Message Data 是否錯誤 若有錯誤即發出S9Fy
            try
            {
                if (bytes.Length <= 14)
                {
                    message_header = MessageHeader.decode(Message_header);
                    secsMessage = new SecsMessage(message_header.S, message_header.F, message_header.ReplyExpection)
                    {
                    };
                }
                else
                {

                    int index = 14;
                    Item item = Decode(bytes, ref index);
                    message_header = MessageHeader.decode(Message_header);
                    secsMessage = new SecsMessage(message_header.S, message_header.F, message_header.ReplyExpection)
                    {
                        SecsItem = item
                    };
                }
            }
            catch(Exception ex)
            {
                Utility.LogException(ex);
            }
            

            return new Tuple<MessageHeader, SecsMessage>(message_header, secsMessage);
        }

        public static byte[] ToCombine(byte[] header, byte[] message)
        {
            try
            {
                byte[] Combine_Message = new byte[header.Length + message.Length + 4];
                byte[] Bytes_Length = BitConverter.GetBytes(header.Length + message.Length);
                Combine_Message[0] = Bytes_Length[3];
                Combine_Message[1] = Bytes_Length[2];
                Combine_Message[2] = Bytes_Length[1];
                Combine_Message[3] = Bytes_Length[0];
                Array.Copy(header, 0, Combine_Message, 4, header.Length);
                Array.Copy(message, 0, Combine_Message, (header.Length + 4), message.Length);

                return Combine_Message;
            }
            catch(Exception ex)
            {
                Utility.LogException(ex);
                return null;
            }
        }

        public static byte[] EncodeTo(Item? items)
        {
            List<byte> result = new();

            if(items != null)
            {
                if (items.Format == SecsFormat.L)
                {
                    foreach (var item in items._rawData)
                    {
                        result.Add(item);
                    }
                    foreach (Item item in items.Value)
                    {
                        if (item.Format == SecsFormat.L)
                        {
                            foreach (var item1 in item._rawData)
                            {
                                result.Add(item1);
                            }
                            foreach (Item item2 in item.Value)
                            {
                                if (item2.Format == SecsFormat.L)
                                {
                                    ThirdList(result, item2);
                                }
                                else
                                {
                                    if (item2._rawData != null)
                                    {
                                        foreach (var bytes in item2._rawData)
                                        {
                                            result.Add(bytes);
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (item._rawData != null)
                            {
                                foreach (var bytes in item._rawData)
                                {
                                    result.Add(bytes);
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (items._rawData != null)
                    {
                        foreach (var bytes in items._rawData)
                        {
                            result.Add(bytes);
                        }
                    }

                }
            }

            return result.ToArray();
        }

        public static List<byte> ThirdList(List<byte> result, Item item)
        {
            foreach (var item1 in item._rawData)
            {
                result.Add(item1);
            }
            foreach (Item item2 in item.Value)
            {
                if (item2.Format == SecsFormat.L)
                {
                    ThirdList(result, item2);
                }
                else
                {
                    if (item2._rawData != null)
                    {
                        foreach (var bytes in item2._rawData)
                        {
                            result.Add(bytes);
                        }
                    }
                }

            }


            return result;
        }
        
    }
}
