using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

using MongoDB;
using RabbitMQ.ServiceModel;
using Discuz.EntLib.RabbitMQ.Logs;
using Discuz.Config;
using Discuz.Cache;

namespace Discuz.EntLib.RabbitMQ
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
            if (!m_serviceStarted)
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
            //new HttpModuleErrLogService().AddLog(new HttpModuleErrLogData(LogLevel.High, "daizhj"));
        }
        
        private ServiceHost m_host;
        private bool m_serviceStarted = false;

        public void StartService(System.ServiceModel.Channels.Binding binding)
        {
            m_host = new ServiceHost(typeof(HttpModuleErrLogService), new Uri("soap.amqp:///"));
            //((RabbitMQBinding)binding).OneWayOnly = true;
            m_host.AddServiceEndpoint(typeof(IHttpModuleErrLogService), binding, "HttpModuleErrLogService");
            m_host.Open();
            m_serviceStarted = true;            
        }

        public void StopService()
        {
            if (m_serviceStarted)
            {
                m_host.Close();
                m_serviceStarted = false;
            }
        }
  
        private void ErrorDequeue_Click(object sender, EventArgs e)
        {
            HttpModuleErrLogService.Dequeue();
        }
        #endregion

    }
}
