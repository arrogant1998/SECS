namespace SECS_Code
{
    public class Worker
    {
        public static IConfiguration? configuration;
        public static HsmsConnection test_Connect;
        private static Test TestSendMessage;

        public Worker(IConfiguration _configuration)
        {
            configuration = _configuration;
            test_Connect = new HsmsConnection(configuration);
            Thread.Sleep(1000);
            test_Connect.Start();
            //TestSendMessage = new Test();

        }
    }
}
