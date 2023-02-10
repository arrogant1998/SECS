namespace SECS_Code
{
    public class MessageHeader : IDisposable
    {
        public ushort DeviceId;
        public bool ReplyExpection;
        public byte S;
        public byte F;
        public MessageType messagetype;
        public int id;


        public MessageHeader(ushort _DeviceId = default, bool _ReplyExpection = default, byte s = default, byte f = default, MessageType message = default, int _id = default)
        {
            DeviceId= _DeviceId;
            ReplyExpection= _ReplyExpection;
            S = s;
            F = f;
            messagetype = message;
            id = _id;
        }





        public byte[] EncodeTo()
        {
            byte[] header= new byte[10];
            byte[] values = BitConverter.GetBytes(DeviceId);
            header[0] = values[1];
            header[1] = values[0];
            if (ReplyExpection)
            {
                header[2] = (byte)(S | 128);
            }
            else
            {
                header[2] = S;
            }
            header[3] = F;
            header[4] = 0;
            header[5] = (byte)messagetype;
            byte[] id_value = BitConverter.GetBytes(id);
            header[6] = id_value[3];
            header[7] = id_value[2];
            header[8] = id_value[1];
            header[9] = id_value[0];

            return header;

        }


        //需判斷Device ID, S, F, 是否正常若有錯誤則送出 SxFy
        public static MessageHeader decode(byte[] data)
        {
            MessageHeader tmp = new();
            byte[] deviceid = new byte[2] { data[1], data[0] };
            byte[] id = new byte[4] { data[9], data[8], data[7], data[6] };
            tmp.DeviceId = BitConverter.ToUInt16(deviceid);
            tmp.ReplyExpection = (data[2] & 0x80) != 0;
            tmp.S = (byte)(data[2] & 63);
            tmp.F = data[3];
            tmp.messagetype = (MessageType)data[5];
            tmp.id = BitConverter.ToInt32(id);
            return tmp;
        }

        public void Dispose()
        {
        }
    }
}
