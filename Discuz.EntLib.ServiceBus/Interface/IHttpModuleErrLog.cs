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
    /// <summary>
    /// IHttpModuleErrLogService接口类
    /// </summary>  
    [ServiceContract]
    public interface IHttpModuleErrLogService
    {
        /// <summary>
        /// 添加httpModuleErrLogData日志信息
        /// </summary>
        /// <param name="httpModuleErrLogData"></param>
        [OperationContract]
        void AddLog(HttpModuleErrLogData httpModuleErrLogData);
    }

    /// <summary>
    /// IHttpModuleErrlogClient客户端接口类，用于反射实例化绑定
    /// </summary>
    public interface IHttpModuleErrlogClient
    {
        void AddLog(HttpModuleErrLogData httpModuleErrLogData);

        void AsyncAddLog(HttpModuleErrLogData httpModuleErrLogData);
    }

    /// <summary>
    /// RabbitMQ
    /// </summary>
    public class HttpModuleErrLogClientHelper
    {
        static IHttpModuleErrlogClient ihttpModuleErrLogClient;

        private static object lockHelper = new object();

        public static IHttpModuleErrlogClient GetHttpModuleErrLogClient()
        {
            if (ihttpModuleErrLogClient == null)
            {
                lock (lockHelper)
                {
                    if (ihttpModuleErrLogClient == null)
                    {
                        try
                        {
                            if (RabbitMQConfigs.GetConfig().HttpModuleErrLog.Enable)
                            {
                                ihttpModuleErrLogClient = (IHttpModuleErrlogClient)Activator.CreateInstance(Type.GetType(
                                      "Discuz.EntLib.RabbitMQ.Client.HttpModuleErrLogClient, Discuz.EntLib.RabbitMQ.Client", false, true));
                            }
                        }
                        catch
                        {
                            throw new Exception("请检查 Discuz.EntLib.RabbitMQ.Client.dll 文件是否被放置到了bin目录下!");
                        }
                    }
                }
            }
            return ihttpModuleErrLogClient;
        }
    }
}