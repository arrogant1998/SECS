using System;
using System.Runtime.InteropServices;

namespace SECS_Code
{
    public class WinAPI
    {
        public WinAPI()
        {
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        public static extern bool SetLocalTime(ref WinAPI.SystemTime dtTime);

        public struct SystemTime
        {
            public ushort wYear;

            public ushort wMonth;

            public ushort wDayOfWeek;

            public ushort wDay;

            public ushort wHour;

            public ushort wMinute;

            public ushort wSecond;

            public ushort wMiliseconds;
        }
    }
}
