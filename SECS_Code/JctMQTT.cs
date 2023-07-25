using MQTTnet;
using MQTTnet.Client;
using System.Timers;
using System.Threading.Tasks;
using System.Reflection;
using MQTTnet.Formatter;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MQTTnet.Server;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SECS_Code
{
    public class JctMQTT
    {
        public struct DataType
        {
            public int? no;
            public string? ip;
            public string? port;
            public string? macid;
            public int[]? data;
            public DateTime time;
        }

        public static string? text = null;
        public static List<DataType> datas = new();
        public static List<int> send_data= new();
        public static bool send_flag = false;
        public static List<string> Insert_messages = new();
        public static MqttFactory mqttFactory = new MqttFactory();
        public static IMqttClient? mqttClient = mqttFactory.CreateMqttClient();
        public static List<int> data_tmp = new();
        public static DataType data_t = new DataType();
        public static JArray json;
        public static MqttClientOptions? mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(HsmsConnection.configuration["MQTT_Address"], 1883)
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                    .WithClientId(Guid.NewGuid().ToString().Substring(0, 5))
                    .Build();
        public static MqttClientSubscribeOptions mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f =>
                {
                    f.WithTopic(HsmsConnection.configuration["MQTTtopic"]);
                }).Build();

        public static async Task Handle_Received_Application_Message()
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (!mqttClient.IsConnected)
                        {

                            mqttClient.ApplicationMessageReceivedAsync += e =>
                            {

                                text = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                                if (text != null)
                                {
                                    
                                    datas.Clear();
                                    GetData(text);
                                    
                                }

                                text = null;
                                return Task.CompletedTask;

                            };
                            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                            await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
                            Console.WriteLine("MQTT client subscribed to topic.");
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Handle_Received_Application_Message : " + ex.Message);
                    }
                    finally
                    {
                        await Task.Delay(TimeSpan.FromSeconds(60));
                    }
                }
            });
        }
         /*GetData()*/ 
    }
}
