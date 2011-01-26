using System.ServiceModel;
using System.ServiceModel.Channels;
using RabbitMQ.ServiceModel;

using Discuz.EntLib.ServiceBus;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.EntLib.RabbitMQ.Client
{  
    public class SendShortMsgClient : ISendShortMsgClient
    {
        public void SendShortMsgByUserGroup(string groupIdList, PrivateMessageInfo privateMesageInfo)
        {
            try
            {
                //((RabbitMQBinding)binding).OneWayOnly = true;
                ChannelFactory<ISendShortMsgService> m_factory = new ChannelFactory<ISendShortMsgService>(GetBinding(), "soap.amqp:///SendShortMsgService");
                m_factory.Open();
                ISendShortMsgService m_client = m_factory.CreateChannel();
                m_client.SendShortMsgByUserGroup(groupIdList, privateMesageInfo);
                ((IClientChannel)m_client).Close();
                m_factory.Close();
            }
            catch (System.Exception e)
            {
                string msg = e.Message;
            }
        }

        private delegate void delegateSendShortMsgByUserGroup(string groupIdList, PrivateMessageInfo privateMesageInfo);

        public void AsyncSendShortMsgByUserGroup(string groupIdList, PrivateMessageInfo privateMesageInfo)
        {
            delegateSendShortMsgByUserGroup SendShortMsgByUserGroup_aysncallback = new delegateSendShortMsgByUserGroup(SendShortMsgByUserGroup);
            SendShortMsgByUserGroup_aysncallback.BeginInvoke(groupIdList, privateMesageInfo, null, null);
        }

        public Binding GetBinding()
        {
            return new RabbitMQBinding(RabbitMQConfigs.GetConfig().SendShortMsg.RabbitMQAddress);
        }
    }

}
