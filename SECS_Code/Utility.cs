using System.Reflection;

namespace SECS_Code
{
    public class Utility
    {
        private static string? pub_text = null;
        private static int pub_count = 0;
        public static string[]? log_tmp = new string[5];
        public static int log_tmp_count = 0;
        public static string[]? log_tmp_LogException = new string[5];
        public static int log_tmp_LogException_count = 0;
        public static string? error_code = "";
        public static string GetDateTimeStamp()
        {
            //return DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK");
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffffff");
        }

        public async static void Log(int verbose, string msg)
        {
            MethodBase m = MethodBase.GetCurrentMethod();
            string sCurrentMethod = $"{m.ReflectedType.Name}.{m.Name}";
            try
            {
                string sVerbose = "";
                switch (verbose)
                {
                    case 1:
                        sVerbose = "Error  ";
                        break;
                    case 2:
                        sVerbose = "Startup";
                        break;
                    case 3:
                        sVerbose = "Closure";
                        break;
                    case 4:
                        sVerbose = "Trace  ";
                        break;
                    case 5:
                        sVerbose = "Info   ";
                        break;
                    case 6:
                        sVerbose = "Test   ";
                        break;
                    case 7:
                        sVerbose = "Debug  ";
                        break;
                    default:
                        sVerbose = "Undefined";
                        break;
                }
                if (log_tmp_count == log_tmp.Length)
                {
                    log_tmp_count = 0;
                }
                if (log_tmp_count == 0 && log_tmp[0] == null)
                {
                    log_tmp[log_tmp_count] = msg;
                    log_tmp_count++;

                }
                else
                {
                    bool flag = false;
                    foreach (var item in log_tmp)
                    {
                        if (msg.Equals(item))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        log_tmp[log_tmp_count] = msg;
                        log_tmp_count++;
                        Console.WriteLine($"[{GetDateTimeStamp()}][{sVerbose}] {msg}");
                    }
                }
            }
            catch (Exception ex)
            {
                string error_message = $"{sCurrentMethod} : {ex.Message} : {ex.StackTrace} : {ex.ToString()} ";
                Console.WriteLine($"[{GetDateTimeStamp()}][Error] {error_message}");
            }
        }


        public static void LogException(Exception ex)
        {
            MethodBase m = MethodBase.GetCurrentMethod();
            string sCurrentMethod = $"{m.ReflectedType.Name}.{m.Name}";
            string error_message = $"{sCurrentMethod} : {ex.Message} : {ex.StackTrace} : {ex} ";
            Log(1, error_message);
        }
        public static void LogException(Exception ex, string insertMessage)
        {
            MethodBase m = MethodBase.GetCurrentMethod();
            string sCurrentMethod = $"{m.ReflectedType.Name}.{m.Name}";
            string error_message = $"{insertMessage} {sCurrentMethod} : {ex.Message} : {ex.StackTrace} : {ex} ";
            Log(1, error_message);
        }
    }
}
