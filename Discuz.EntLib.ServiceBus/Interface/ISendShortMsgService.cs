using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;

using Discuz.Entity;
using Discuz.Config;

namespace Discuz.EntLib.ServiceBus
{
    // 注意: 如果更改此处的接口名称 "ISendShortMsgService"，也必须更新 App.config 中对 "ISendShortMsgService" 的引用。
    [ServiceContract]
    public interface ISendShortMsgService
    {
        [OperationContract]
        void SendShortMsgByUserGroup(string groupIdList, PrivateMessageInfo privateMesageInfo);
    }

    public interface ISendShortMsgClient
    {
        void SendShortMsgByUserGroup(string groupIdList, PrivateMessageInfo privateMesageInfo);

        void AsyncSendShortMsgByUserGroup(string groupIdList, PrivateMessageInfo privateMesageInfo);
    }

    /// <summary>
    /// RabbitMQ
    /// </summary>
    public class SendShortMsgClientHelper
    {
        static ISendShortMsgClient isendShortMsgClient;

        private static object lockHelper = new object();

        public static ISendShortMsgClient GetSendShortMsgClient()
        {
            if (isendShortMsgClient == null)
            {
                lock (lockHelper)
                {
                    if (isendShortMsgClient == null)
                    {
                        try
                        {
                            if (RabbitMQConfigs.GetConfig().SendShortMsg.Enable)
                            {
                                isendShortMsgClient = (ISendShortMsgClient)Activator.CreateInstance(Type.GetType(
                                      "Discuz.EntLib.RabbitMQ.Client.SendShortMsgClient, Discuz.EntLib.RabbitMQ.Client", false, true));
                            }
                        }
                        catch
                        {
                            throw new Exception("请检查 Discuz.EntLib.RabbitMQ.Client.dll 文件是否被放置到了bin目录下!");
                        }
                    }
                }
            }
            return isendShortMsgClient;
        }
    }
}