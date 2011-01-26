using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.ServiceModel;

using RabbitMQ.ServiceModel;
using Discuz.EntLib.RabbitMQ.Server.Logs;
using Discuz.Config;
using Discuz.EntLib.ServiceBus;

namespace Discuz.EntLib.RabbitMQ.Server
{
    public partial class RabbitMQ_Server : Form
    {
        public RabbitMQ_Server()
        {
            InitializeComponent();
            this.Closing += new CancelEventHandler(Form_Closing);

            MsgRichBox.ContextMenuStrip = CreateMsgRichBoxMenuStrip();

            RabbitMQConfigInfo rabbitMQConfigInfo = RabbitMQConfigs.GetConfig();
            LogServerTxt.Text = rabbitMQConfigInfo.HttpModuleErrLog.RabbitMQAddress;
            ShortMsgServer.Text = rabbitMQConfigInfo.SendShortMsg.RabbitMQAddress;
        }

        public void Form_Closing(object sender, CancelEventArgs cArgs)
        {
            if (sender == this)
            {
                DialogResult dialogResult = MessageBox.Show("当前程序运行RabbitMQ服务,真的要关闭吗?", "系统提示:", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)//当点击确定后再关闭窗体
                    this.Dispose();
                else
                    cArgs.Cancel = true;
            }
        }

        #region 鼠标右键菜单事件代码 
        /// <summary>
        /// 鼠标右键菜单事件代码 
        /// </summary>
        /// <returns></returns>
        private ContextMenuStrip CreateMsgRichBoxMenuStrip()
        {
            ContextMenuStrip cms = new ContextMenuStrip() { ImageList = imageList1};
     
            ToolStripMenuItem queryItem = new ToolStripMenuItem() { Text = "清空", Name = "Clearup", ImageKey = "new.ico" }; 
            queryItem.Click += new EventHandler(ClearUpClick);
            cms.Items.Add(queryItem);

            ToolStripMenuItem indexItem = new ToolStripMenuItem() { Text = "保存", Name = "Save", ImageKey = "save.ico" };
            indexItem.Click += new EventHandler(SaveClick);
            cms.Items.Add(indexItem);

            return cms;
        }

        private void ClearUpClick(object sender, EventArgs e)
        {
            MsgRichBox.Clear();
        }

        private void SaveClick(object sender, EventArgs e)
        {
            saveMsgFileDialog.Title = "保存";
            saveMsgFileDialog.Filter = "文本文档(*.txt)|*.txt";
            saveMsgFileDialog.RestoreDirectory = true;
            if (saveMsgFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveMsgFileDialog.FileName))
            {
                using (System.IO.FileStream fs = System.IO.File.Open(saveMsgFileDialog.FileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                {
                    byte[] b = System.Text.Encoding.UTF8.GetBytes(MsgRichBox.Text);
                    fs.Write(b, 0, b.Length);
                    fs.Close();
                }
            }
        }
        #endregion

      
        #region 日志服务功能
        private void StartLogServer_Click(object sender, EventArgs e)
        {
            if (!m_logserverStarted)
            {
                StartService(new RabbitMQBinding(LogServerTxt.Text));
                StartLogServer.Text = "停止";
                MsgRichBox.Text += DateTime.Now + " => 日志服务启动\r";
            }
            else
            {
                StopService();
                StartLogServer.Text = "开始";
                MsgRichBox.Text += DateTime.Now + " => 日志服务停止\r";
            }
            HttpModuleErrLogService.Initial(MsgRichBox, Discuz.Common.TypeConverter.StrToInt(ErrorTimer.Text));
        }
        
        private ServiceHost m_logServerHost;
        private bool m_logserverStarted = false;

        public void StartService(System.ServiceModel.Channels.Binding binding)
        {
            m_logServerHost = new ServiceHost(typeof(HttpModuleErrLogService), new Uri("soap.amqp:///"));
            //((RabbitMQBinding)binding).OneWayOnly = true;
            m_logServerHost.AddServiceEndpoint(typeof(IHttpModuleErrLogService), binding, "HttpModuleErrLogService");
            m_logServerHost.Open();
            m_logserverStarted = true;            
        }

        public void StopService()
        {
            if (m_logserverStarted)
            {
                m_logServerHost.Close();
                m_logserverStarted = false;
            }
        }
  
        private void ErrorDequeue_Click(object sender, EventArgs e)
        {
            HttpModuleErrLogService.Dequeue();
        }
        #endregion

#region 短消息发送功能
        private void StartShortMsgServer_Click(object sender, EventArgs e)
        {
            if (!m_shortMsgServerStarted)
            {
                StartShortMsgService(new RabbitMQBinding(LogServerTxt.Text));
                StartShortMsgServer.Text = "停止";
                MsgRichBox.Text += DateTime.Now + " => 短消息批量发送服务启动\r";
            }
            else
            {
                StopShortMsgService();
                StartShortMsgServer.Text = "开始";
                MsgRichBox.Text += DateTime.Now + " => 短消息批量发送服务停止\r";
            }
            SendShortMsgService.Initial(MsgRichBox, Discuz.Common.TypeConverter.StrToInt(SuspendTime.Text));
            //new SendShortMsgService().SendShortMsgByUserGroup("1,2,3,4,5,6,8,9,10,11,12,13,14,15,16", new Discuz.Entity.PrivateMessageInfo() { Msgfromid = 1, Msgfrom = "admin", Message = "hello", Subject = "hello", Folder = 1, Postdatetime = "2010-8-8" });
        }


        private ServiceHost m_shortMsgServerHost;
        private bool m_shortMsgServerStarted = false;

        public void StartShortMsgService(System.ServiceModel.Channels.Binding binding)
        {
            m_shortMsgServerHost = new ServiceHost(typeof(SendShortMsgService), new Uri("soap.amqp:///"));
            //((RabbitMQBinding)binding).OneWayOnly = true;
            m_shortMsgServerHost.AddServiceEndpoint(typeof(ISendShortMsgService), binding, "SendShortMsgService");
            m_shortMsgServerHost.Open();
            m_shortMsgServerStarted = true;
        }

        public void StopShortMsgService()
        {
            if (m_shortMsgServerStarted)
            {
                m_shortMsgServerHost.Close();
                m_shortMsgServerStarted = false;
            }
        }
#endregion

    }
}
