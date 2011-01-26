using System.ServiceModel;
using System.ServiceModel.Channels;
using RabbitMQ.ServiceModel;

using Discuz.EntLib.ServiceBus;
using Discuz.Config;

namespace Discuz.EntLib.RabbitMQ.Client
{
    public class HttpModuleErrLogClient : IHttpModuleErrlogClient
    {
        public void AddLog(HttpModuleErrLogData httpModuleErrLogData)
        {
            try
            {
                //((RabbitMQBinding)binding).OneWayOnly = true;
                ChannelFactory<IHttpModuleErrLogService> m_factory = new ChannelFactory<IHttpModuleErrLogService>(GetBinding(), "soap.amqp:///HttpModuleErrLogService");
                m_factory.Open();
                IHttpModuleErrLogService m_client = m_factory.CreateChannel();
                m_client.AddLog(httpModuleErrLogData);
                ((IClientChannel)m_client).Close();
                m_factory.Close();
            }
            catch (System.Exception e)
            { 
                string msg = e.Message; 
            }
        }

        private delegate void delegateAddLog(HttpModuleErrLogData httpModuleErrLogData);

        public void AsyncAddLog(HttpModuleErrLogData httpModuleErrLogData)
        {
            delegateAddLog AddLog_aysncallback = new delegateAddLog(AddLog);
            AddLog_aysncallback.BeginInvoke(httpModuleErrLogData, null, null);
        }

        public Binding GetBinding()
        {
            return new RabbitMQBinding(RabbitMQConfigs.GetConfig().HttpModuleErrLog.RabbitMQAddress);
        }
    }
}
