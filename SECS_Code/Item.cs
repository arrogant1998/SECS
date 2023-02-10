using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
namespace SECS_Code
{
    public class Item : IDisposable 
    {
        public SecsFormat Format { get; set; }
        public int Count { get; set; }
        public IEnumerable Value { get; set; }

        public IList<Item> Items { get; set; }

        public byte[] _rawData;
        
        public static readonly Encoding Jis8Encoding = Encoding.GetEncoding(50222);
        private static readonly Item Empty_L = new Item(SecsFormat.L, Enumerable.Empty<Item>());
        private static readonly Item Empty_A = new Item(SecsFormat.A, string.Empty);
        private static readonly Item Empty_J = new Item(SecsFormat.JIS8, string.Empty);
        private static readonly Item Empty_B = new Item(SecsFormat.B, new byte[0]);
        private static readonly Item Empty_TF = new Item(SecsFormat.TF, new bool[0]);
        private static readonly Item Empty_U1 = new Item(SecsFormat.U1, new byte[0]);
        private static readonly Item Empty_U2 = new Item(SecsFormat.U2, new ushort[0]);
        private static readonly Item Empty_U4 = new Item(SecsFormat.U4, new uint[0]);
        private static readonly Item Empty_U8 = new Item(SecsFormat.U8, new ulong[0]);
        private static readonly Item Empty_I1 = new Item(SecsFormat.I1, new sbyte[0]);
        private static readonly Item Empty_I2 = new Item(SecsFormat.I2, new short[0]);
        private static readonly Item Empty_I4 = new Item(SecsFormat.I4, new int[0]);
        private static readonly Item Empty_I8 = new Item(SecsFormat.I8, new long[0]);
        private static readonly Item Empty_F4 = new Item(SecsFormat.F4, new float[0]);
        private static readonly Item Empty_F8 = new Item(SecsFormat.F8, new double[0]);

        //List
        private Item(IList<Item> items)
        {
            if(items.Count > 255)
            {
                throw new ArgumentException("List items length out of range, max length: 255");
            }
            Format = SecsFormat.L;
            Count = items.Count;
            Value = items;
            _rawData = new byte[] { (byte)((byte)SecsFormat.L | 1), (byte)Count };
        }

        //ASCII JIS8
        private Item(SecsFormat format, string value)
        {
            Format = format;
            Value = value;
            Count = Value.ToString().Length;
            var str = (string)Value;
            var result = ExtensionHelper.EncodeItem(Format, str.Length);
            var encoder = Format == SecsFormat.A? Encoding.ASCII : Jis8Encoding;
            encoder.GetBytes(str, 0, str.Length, result.Item1, result.Item2);
            _rawData = result.Item1;
        }


        /// <summary>
        /// U1, U2, U4, U8
        /// I1, I2, I4, I8
        /// F4, F8
        /// Boolean,
        /// Binary
        /// </summary>
        private Item(SecsFormat format, Array value)
        {
            Format = format;
            Value = value;
            Count = value.Length;
            var arr = (Array)Value;
            if (arr.Length != 0)
            {
                var byteLength = Buffer.ByteLength(arr);
                var result = ExtensionHelper.EncodeItem(Format, byteLength);
                Buffer.BlockCopy(arr, 0, result.Item1, result.Item2, byteLength);
                result.Item1.Reverse(result.Item2, byteLength + result.Item2, byteLength / arr.Length);
                _rawData = result.Item1;
            }
            else
            {
                var byteLength = 0;
                var result = ExtensionHelper.EncodeItem(Format, byteLength);
                Buffer.BlockCopy(arr, 0, result.Item1, result.Item2, byteLength);
                result.Item1.Reverse(result.Item2, byteLength + result.Item2, byteLength);
                _rawData = result.Item1;
            }
                
            
            
        }
        //Empty List
        private Item(SecsFormat format, IEnumerable value)
        {
            Format= format;
            Value = value;
            Count = 0;
            _rawData = new byte[] {(byte)((byte)format | 1), 0 };
        }

        public T GetValue<T>()
        {
            if (Value == null)
                throw new InvalidOperationException("Item format is List");

            if (Value is T)
                return (T)((ICloneable)Value).Clone();

            if (Value is T[])
                return ((T[])Value)[0];

            Type valueType = Nullable.GetUnderlyingType(typeof(T));
            if (valueType != null && Value.GetType().GetElementType() == valueType)
                return ((IEnumerable)Value).Cast<T>().FirstOrDefault();

            throw new InvalidOperationException("Item value type is incompatible");
        }


        public static Item L() => Empty_L;
        public static Item A() => Empty_A;
        public static Item B() => Empty_B;
        public static Item TF() => Empty_TF;
        public static Item U1() => Empty_U1;
        public static Item U2() => Empty_U2;
        public static Item U4() => Empty_U4;
        public static Item U8() => Empty_U8;
        public static Item I1() => Empty_I1;
        public static Item I2() => Empty_I2;
        public static Item I4() => Empty_I4;
        public static Item I8() => Empty_I8;
        public static Item F4() => Empty_F4;
        public static Item F8() => Empty_F8;
        public static Item J() => Empty_J;


        public static Item L(IList<Item> items) => items.Count > 0 ? new Item(items) : L();
        public static Item L(IEnumerable<Item> items) => L(items.ToList());
        public static Item L(params Item[] items) => L(items.ToList());
        public static Item A(string value) => new Item(SecsFormat.A, value);
        public static Item J(string value) => new Item(SecsFormat.JIS8, value);
        public static Item B(params byte[] value) => new Item(SecsFormat.B, value);
        public static Item TF(params bool[] value) => new Item(SecsFormat.TF, value);
        public static Item U1(params byte[] value) => new Item(SecsFormat.U1, value);
        public static Item U2(params ushort[] value) => new Item(SecsFormat.U2, value);
        public static Item U4(params uint[] value) => new Item(SecsFormat.U4, value);
        public static Item U8(params ulong[] value) => new Item(SecsFormat.U8, value);
        public static Item I1(params sbyte[] value) => new Item(SecsFormat.I1, value);
        public static Item I2(params short[] value) => new Item(SecsFormat.I2, value);
        public static Item I4(params int[] value) => new Item(SecsFormat.I4, value);
        public static Item I8(params long[] value) => new Item(SecsFormat.I8, value);
        public static Item F4(params float[] value) => new Item(SecsFormat.F4, value);
        public static Item F8(params double[] value) => new Item(SecsFormat.F8, value);

        public void Dispose()
        {
            GC.Collect();
        }

        internal uint EncodeTo(List<ArraySegment<byte>> buffer, Item secsMessage)
        {
            byte[] bytes = new byte[0];
            uint length = 0;
            try
            {
                if (secsMessage is not null)
                {
                    bytes = ExtensionHelper.EncodeTo(secsMessage);
                    length = unchecked((uint)bytes.Length);
                    buffer.Add(new ArraySegment<byte>(bytes));
                }
                return length;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"EncodeTo: {ex.Message}");
                return 0;
            }
        }
    }
}
