using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;

namespace Discuz.EntLib.ServiceBus
{
    #region HttpModule错误日志数据类
    /// <summary>
    /// HttpModule错误日志数据类
    /// </summary>
    [DataContract]
    public class HttpModuleErrLogData
    {
        private String m_oid;
        private LogLevel m_level;
        private String m_message;
        private DateTime m_timeStamp;

        public HttpModuleErrLogData() { }

        public HttpModuleErrLogData(LogLevel level, String message)
        {
            m_level = level;
            m_message = message;
            m_timeStamp = DateTime.Now;
        }

        [DataMember]
        public string Oid
        {
            get { return m_oid; }
            set { m_oid = value; }
        }

        [DataMember]
        public LogLevel Level
        {
            get { return m_level; }
            set { m_level = value; }
        }

        [DataMember]
        public String Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        [DataMember]
        public DateTime TimeStamp
        {
            get { return m_timeStamp; }
            set { m_timeStamp = value; }
        }
    }

    [DataContract]
    public enum LogLevel
    {
        [EnumMember]
        High,
        [EnumMember]
        Medium,
        [EnumMember]
        Low,
    }
    #endregion
}
