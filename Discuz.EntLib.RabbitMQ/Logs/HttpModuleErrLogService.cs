using System;
using System.Collections;
using System.ServiceModel;

using Discuz.Common;
using MongoDB;
using Discuz.EntLib.ServiceBus;
using Discuz.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;

namespace Discuz.EntLib.RabbitMQ.Server.Logs
{
  
    // 注意: 如果更改此处的类名 "HttpModuleErrLogService"，也必须更新 Web.config 中对 "HttpModuleErrLogService" 的引用。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class HttpModuleErrLogService : IHttpModuleErrLogService
    {
        /// <summary>
        /// 传递的显示操作结果的RichTextBox控件实例
        /// </summary>
        private static System.Windows.Forms.RichTextBox _msgBox;
        /// <summary>
        /// 获取HttpModuleErrLogInfo配置文件对象实例
        /// </summary>
        private static HttpModuleErrLogInfo httpModuleErrorLogInfo = RabbitMQConfigs.GetConfig().HttpModuleErrLog;
        /// <summary>
        /// 定时器对象
        /// </summary>
        private static System.Timers.Timer _timer;
        /// <summary>
        /// 定时器的时间
        /// </summary>
        private static int _elapsed = 0;

        public static void Initial(System.Windows.Forms.RichTextBox msgBox, int elapsed)
        {
            _msgBox = msgBox;
            _elapsed = elapsed;

            //初始定时器
            if (_elapsed > 0)
            {
                _timer = new System.Timers.Timer() { Interval = elapsed * 1000,  Enabled = true, AutoReset = true };            
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
                _timer.Start();
            }
        }

        /// <summary>
        /// 时间到时执行出队操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {    
            Dequeue();    
        }
 
        /// <summary>
        /// 添加httpModuleErrLogData日志信息
        /// </summary>
        /// <param name="httpModuleErrLogData"></param>
        public void AddLog(HttpModuleErrLogData httpModuleErrLogData)
        {
            Enqueue(httpModuleErrLogData);

            if (_elapsed <=0)
                Dequeue();
        }

        /// <summary>
        /// 声明委托 
        /// </summary>
        /// <param name="message"></param>
        public delegate void ShowMsg(string message);
        /// <summary>
        /// 绑定到上面delegate的方法
        /// </summary>
        /// <param name="outPut"></param>
        public static void SetMsgRichBox(string outPut)
        {
            _msgBox.Text += "\r==================================\r下列错误信息出队时间=>" + DateTime.Now + outPut + "\r";
        }

        /// <summary>
        /// 日志出队
        /// </summary>
        public static void Dequeue()
        {       
            string serverAddress = httpModuleErrorLogInfo.RabbitMQAddress.Replace("amqp://", "").TrimEnd('/'); //"10.0.4.85:5672";
            ConnectionFactory cf = new ConnectionFactory()
            {
                UserName = httpModuleErrorLogInfo.UserName,
                Password = httpModuleErrorLogInfo.PassWord,
                VirtualHost = "dnt_mq",
                RequestedHeartbeat = 0,
                Address = serverAddress
            };
      
            using (IConnection conn = cf.CreateConnection())
            {
                using (IModel ch = conn.CreateModel())
                {
                    while (true)
                    {
                        BasicGetResult res = ch.BasicGet(httpModuleErrorLogInfo.QueueName, false);
                        if (res != null)
                        {
                            try
                            {
                                string objstr = System.Text.UTF8Encoding.UTF8.GetString(res.Body).Replace("\0\0\0body\0\n", "");//去掉头部信息
                                object obj = SerializationHelper.DeSerialize(typeof(HttpModuleErrLogData), objstr);
                                HttpModuleErrLogData httpModuleErrLogData = obj as HttpModuleErrLogData;
                                if (httpModuleErrLogData != null)
                                {
                                    MongoDbHelper.Insert(new Mongo(httpModuleErrorLogInfo.MongoDB), "dnt_httpmoduleerrlog", LoadAttachment(httpModuleErrLogData));
                                    _msgBox.BeginInvoke(new ShowMsg(SetMsgRichBox), "\r发生时间:" + httpModuleErrLogData.TimeStamp + "\r错误等级:" + httpModuleErrLogData.Level + "\r详细信息:" + httpModuleErrLogData.Message);
                                    ch.BasicAck(res.DeliveryTag, false);
                                }
                            }
                            catch { }
                        }
                        else
                            break;
                    }
                }
            }           
        }   

        /// <summary>
        /// 交换机名称
        /// </summary>
        private const string EXCHANGE = "ex1";
        /// <summary>
        /// 交换方法,更多内容参见:http://melin.javaeye.com/blog/691265
        /// </summary>
        private const string EXCHANGE_TYPE = "direct";
        /// <summary>
        /// 路由key,更多内容参见:http://sunjun041640.blog.163.com/blog/static/256268322010328102029919/
        /// </summary>
        private const string ROUTING_KEY = "m1";

        /// <summary>
        /// 日志入队
        /// </summary>
        /// <param name="httpModuleErrLogData"></param>
        public static void Enqueue(HttpModuleErrLogData httpModuleErrLogData)
        {
            Uri uri = new Uri(httpModuleErrorLogInfo.RabbitMQAddress);         
            ConnectionFactory cf = new ConnectionFactory() 
            {
                UserName = httpModuleErrorLogInfo.UserName,
                Password = httpModuleErrorLogInfo.PassWord,
                VirtualHost = "dnt_mq",
                RequestedHeartbeat = 0,
                Endpoint = new AmqpTcpEndpoint(uri)
            };
            using (IConnection conn = cf.CreateConnection())
            {
                using (IModel ch = conn.CreateModel())
                {
                    if (EXCHANGE_TYPE != null)
                    {
                        ch.ExchangeDeclare(EXCHANGE, EXCHANGE_TYPE);//,true,true,false,false, true,null);
                        ch.QueueDeclare(httpModuleErrorLogInfo.QueueName, true);//true, true, true, false, false, null);
                        ch.QueueBind(httpModuleErrorLogInfo.QueueName, EXCHANGE, ROUTING_KEY, false, null);
                    }
                    IMapMessageBuilder b = new MapMessageBuilder(ch);
                    IDictionary target = b.Headers;
                    target["header"] = "HttpErrLog";
                    IDictionary targetBody = b.Body;
                    targetBody["body"] = SerializationHelper.Serialize(httpModuleErrLogData);
                    ((IBasicProperties)b.GetContentHeader()).DeliveryMode = 2;//persistMode                   
                    ch.BasicPublish(EXCHANGE, ROUTING_KEY,
                                               (IBasicProperties)b.GetContentHeader(),
                                               b.GetContentBody());
                }
            }
        }

        /// <summary>
        /// 将HttpModuleErrLogData转换成Document类型
        /// </summary>
        /// <param name="httpModuleErrLogData"></param>
        /// <returns></returns>
        public static Document LoadAttachment(HttpModuleErrLogData httpModuleErrLogData)
        {
           Document doc = new Document();
            doc["_id"] = httpModuleErrLogData.Oid;
            doc["level"] = httpModuleErrLogData.Level;
            doc["message"] = httpModuleErrLogData.Message;
            doc["timestamp"] = httpModuleErrLogData.TimeStamp;
            return doc;
        }      
    }
}
