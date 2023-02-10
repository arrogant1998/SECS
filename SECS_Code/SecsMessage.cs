namespace SECS_Code
{
    public class SecsMessage : IDisposable
    {
        public string? Name { get; set; }
        public byte S { get; set; }
        public byte F { get; set; }
        public bool ReplyExpected { get; set; }

        internal readonly Lazy<List<ArraySegment<byte>>> RawDatas;

        public IList<ArraySegment<byte>> RawBytes => RawDatas.Value.AsReadOnly();

        private static readonly List<ArraySegment<byte>> EmptyMsgDatas = new List<ArraySegment<byte>>
        {
            new ArraySegment<byte>(new byte[]{0, 0, 0, 10}),// total length: 10
            new ArraySegment<byte>(new byte[]{ })// header
            // item
        };

        //自訂
        public Item? SecsItem { get; set; }

        public SecsMessage(byte s, byte f, bool replyExpected = true)
        {
            try
            {
                if (s > 0x7F)
                {
                    Utility.Log(1, "SecsMessage StreamNumber Must Less Than 127");
                    return;
                }

                S = s;
                F = f;
                ReplyExpected = replyExpected;

                RawDatas = new Lazy<List<ArraySegment<byte>>>(() =>
                {
                    if (SecsItem is null)
                    {
                        return EmptyMsgDatas;
                    }
                    else
                    {
                        var result = new List<ArraySegment<byte>>
                {
                    default(ArraySegment<byte>),// total length
                    new ArraySegment<byte>(new byte[]{})// header
                    //item

                };
                        // 計算總個數並將 SecsItem 編碼後加入ArraySegment
                        var length = 10 + SecsItem.EncodeTo(result, SecsItem);// total length = item + header;

                        byte[] msgLengthByte = BitConverter.GetBytes(length);
                        Array.Reverse(msgLengthByte);
                        result[0] = new ArraySegment<byte>(msgLengthByte);
                        /*
                        foreach (var item in result)
                        {
                            for (int i = item.Offset; i < (item.Offset + item.Count); i++)
                            {
                                Console.WriteLine("   [{0}] : {1}", i, item.Array[i]);
                            }
                            Console.WriteLine("---------------------------------------------");
                        }
                        */
                        return result;
                    }

                });
            }
            catch(Exception ex)
            {
                Utility.Log(1, $"SecsMessage Create Error: {ex.Message}");
            }
            
        }
        public override string ToString() => $"{Name ?? String.Empty}'S{S}F{F}' {(ReplyExpected ? "W" : string.Empty)}";
        public void Dispose()
        {
            if(SecsItem is not null)
            {
                SecsItem.Dispose();
            }
            GC.Collect();
        }
    }
}
