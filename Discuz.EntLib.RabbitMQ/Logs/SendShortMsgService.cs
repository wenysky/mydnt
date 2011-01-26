using System;
using System.Data;
using System.Collections;
using System.ServiceModel;

using Discuz.Entity;
using Discuz.EntLib.ServiceBus;
using Discuz.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Content;

namespace Discuz.EntLib.RabbitMQ.Server.Logs
{
   
    // 注意: 如果更改此处的类名 "SendShortMsgService"，也必须更新 App.config 中对 "SendShortMsgService" 的引用。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SendShortMsgService : ISendShortMsgService
    {
        /// <summary>
        /// 获取SendShortMsg配置文件对象实例
        /// </summary>
        private static RabbitMQBaseInfo sendShortMsg = RabbitMQConfigs.GetConfig().SendShortMsg;
        /// <summary>
        /// 传递的显示操作结果的RichTextBox控件实例
        /// </summary>
        private static System.Windows.Forms.RichTextBox _msgBox;
        /// <summary>
        /// 批量发送100条短消息之后线程暂停时间
        /// </summary>
        private static int _suspendTime;

        public static void Initial(System.Windows.Forms.RichTextBox msgBox, int suspendTime)
        {
            _msgBox = msgBox;
            _suspendTime = suspendTime;
        }

        public void SendShortMsgByUserGroup(string groupIdList, PrivateMessageInfo privateMesageInfo)
        {
            Enqueue(groupIdList);
            AsyncDequeue(privateMesageInfo);
        }

        private delegate void delegateDequeue(PrivateMessageInfo privateMesageInfo);

        public void AsyncDequeue(PrivateMessageInfo privateMesageInfo)
        {
            delegateDequeue Dequeue_aysncallback = new delegateDequeue(Dequeue);
            Dequeue_aysncallback.BeginInvoke(privateMesageInfo, null, null);
        }
    
        /// <summary>
        /// 日志出队
        /// </summary>
        public static void Dequeue(PrivateMessageInfo privateMesageInfo)
        {
            string serverAddress = sendShortMsg.RabbitMQAddress.Replace("amqp://", "").TrimEnd('/'); 
            ConnectionFactory cf = new ConnectionFactory()
            {
                UserName = sendShortMsg.UserName,
                Password = sendShortMsg.PassWord,
                VirtualHost = "dnt_mq",
                RequestedHeartbeat = 0,
                Address = serverAddress
            };

            _msgBox.BeginInvoke(new ShowMsg(SetMsgRichBox), "开始批量发送短消息===>时间:" + DateTime.Now + "\r==================================\r下列用户组短消息已发送完毕=>");
            using (IConnection conn = cf.CreateConnection())
            {
                using (IModel ch = conn.CreateModel())
                {
                    while (true)
                    {
                        BasicGetResult res = ch.BasicGet(sendShortMsg.QueueName, false);
                        if (res != null)
                        {
                            try
                            {
                                string groupId = System.Text.UTF8Encoding.UTF8.GetString(res.Body).Replace("\0\0\0body\0\n", "").Replace("\0", "");//去掉头部信息
                                int startUid = 0;//要查询的起始用户uid(大于该uid的信息)
                                int count = 0;
                                while(true)
                                {
                                    DataTable dt = SqlServerDbHelper.GetUserData("dnt_", groupId, startUid.ToString());
                                    if (dt.Rows.Count == 0) break;//如无数据则获取下一条队列信息

                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        PrivateMessageInfo pm = new PrivateMessageInfo();
                                        pm.Msgfrom = privateMesageInfo.Msgfrom.Replace("'", "''");
                                        pm.Msgfromid = privateMesageInfo.Msgfromid;
                                        pm.Msgto = dr["username"].ToString().Replace("'", "''");
                                        pm.Msgtoid = Convert.ToInt32(dr["uid"].ToString());
                                        pm.Folder = privateMesageInfo.Folder;
                                        pm.Subject = privateMesageInfo.Subject;
                                        pm.Postdatetime = privateMesageInfo.Postdatetime;
                                        pm.Message = privateMesageInfo.Message;
                                        pm.New = 1;//标记为未读
                                        SqlServerDbHelper.CreatePrivateMessage(pm);
                                        count++;
                                        startUid = pm.Msgtoid;//更新
                                    }
                                }
                                string output = "用户组名: " + SqlServerDbHelper.GetUserGroupName(groupId) + "\r用户组ID: " + groupId + "\r成员数量: " + count + "\r结束时间: " + DateTime.Now + "\r";
                                _msgBox.BeginInvoke(new ShowMsg(SetMsgRichBox), output);
                                 ch.BasicAck(res.DeliveryTag, false);//清除队列中该用户组信息
                                 System.Threading.Thread.Sleep(_suspendTime * 1000);//暂停指定秒数之后，让数据库处理别的用户请求                                                          
                            }
                            catch { }
                        }
                        else
                            break;
                    }
                }
            }
            _msgBox.BeginInvoke(new ShowMsg(SetMsgRichBox), "批量发送短消息结束===>时间:" + DateTime.Now);
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
            _msgBox.Text += "\r" + outPut + "\r";
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
        public static void Enqueue(string groupIdList)
        {
            Uri uri = new Uri(sendShortMsg.RabbitMQAddress);
            ConnectionFactory cf = new ConnectionFactory()
            {
                UserName = sendShortMsg.UserName,
                Password = sendShortMsg.PassWord,
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
                        ch.QueueDeclare(sendShortMsg.QueueName, true);//true, true, true, false, false, null);
                        ch.QueueBind(sendShortMsg.QueueName, EXCHANGE, ROUTING_KEY, false, null);
                    }
                    //对用户组元素进行分割
                    foreach (string groupId in groupIdList.Split(','))
                    {
                        IMapMessageBuilder b = new MapMessageBuilder(ch);
                        IDictionary target = b.Headers;
                        target["header"] = "SendShortMsgGroupId";
                        IDictionary targetBody = b.Body;
                        targetBody["body"] = groupId;
                        ((IBasicProperties)b.GetContentHeader()).DeliveryMode = 2;//persistMode                   
                        ch.BasicPublish(EXCHANGE, ROUTING_KEY,
                                                   (IBasicProperties)b.GetContentHeader(),
                                                   b.GetContentBody());
                    }
                }
            }
        }
    }
}
