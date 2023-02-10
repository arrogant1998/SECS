using System.Diagnostics;
using System.Threading;
using System.Timers;
using static System.Net.Mime.MediaTypeNames;

namespace SECS_Code
{
    public class SendData
    {
        public static System.Timers.Timer timer = new System.Timers.Timer();
        Thread? thread;
        public static bool send_flag
        {
            get { return timer.Enabled; }
            set
            {
                if (timer.Enabled != value)
                {
                    
                    if(time != 0)
                    {
                        timer.Interval = time;
                    }
                    timer.Enabled = value;
                    Console.WriteLine($"timer.Enabled: {timer.Enabled}");
                }

            }
        }
        public static int time = 1000;
        public int count;
        public int i = 0;
        public SendData()
        {
            timer.Elapsed += InitTimer;
            timer.Enabled = false;
        }
        public void InitTimer(object source, EventArgs e)
        {
            if (send_flag)
            {
                SendS6F1();
            }
        }
        public async void SendS6F1()
        {
            try
            {
                if (i >= count)
                {
                    i = 0;
                    Console.WriteLine("Stop SendS6F1");
                    send_flag = false;
                    GC.Collect();
                }
                if (send_flag)
                {
                    using var s6f1 = new SecsMessage(6, 1)
                    {
                        SecsItem = Item.L(
                                        Item.U4(1),
                                        Item.U4((uint) i + 1),
                                        Item.A(DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                        Item.L(
                                            from slotInfo in JctMQTT.send_data
                                            select
                                                       Item.F4(slotInfo)
                                            )
                                        )
                    };
                    i++;
                    await Worker.test_Connect.SendDataMessageAsync(s6f1, i);
                    
                    
                }

            }
            catch(Exception ex)
            {
                send_flag = false;
                Console.WriteLine("[SendData]" + ex.ToString());
            }
            
        }
    }
}
