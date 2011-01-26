namespace Discuz.EntLib.RabbitMQ.Server
{
    partial class RabbitMQ_Server
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RabbitMQ_Server));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MsgRichBox = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ErrorTimer = new System.Windows.Forms.TextBox();
            this.ErrorDequeue = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LogServerTxt = new System.Windows.Forms.TextBox();
            this.StartLogServer = new System.Windows.Forms.Button();
            this.saveMsgFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.skinUI1 = new DotNetSkin.SkinUI();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ShortMsgServer = new System.Windows.Forms.TextBox();
            this.StartShortMsgServer = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MsgRichBox);
            this.groupBox1.Location = new System.Drawing.Point(1, 285);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(685, 233);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // MsgRichBox
            // 
            this.MsgRichBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.MsgRichBox.Location = new System.Drawing.Point(10, 20);
            this.MsgRichBox.Name = "MsgRichBox";
            this.MsgRichBox.ReadOnly = true;
            this.MsgRichBox.Size = new System.Drawing.Size(667, 205);
            this.MsgRichBox.TabIndex = 4;
            this.MsgRichBox.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.ErrorTimer);
            this.groupBox2.Controls.Add(this.ErrorDequeue);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.LogServerTxt);
            this.groupBox2.Controls.Add(this.StartLogServer);
            this.groupBox2.Location = new System.Drawing.Point(1, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(685, 75);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "    日志队列服务配置:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(543, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "秒";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(386, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "定时出队时间:";
            // 
            // ErrorTimer
            // 
            this.ErrorTimer.Location = new System.Drawing.Point(475, 22);
            this.ErrorTimer.Name = "ErrorTimer";
            this.ErrorTimer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ErrorTimer.Size = new System.Drawing.Size(43, 21);
            this.ErrorTimer.TabIndex = 9;
            this.ErrorTimer.Text = "0";
            // 
            // ErrorDequeue
            // 
            this.ErrorDequeue.Location = new System.Drawing.Point(335, 47);
            this.ErrorDequeue.Name = "ErrorDequeue";
            this.ErrorDequeue.Size = new System.Drawing.Size(75, 23);
            this.ErrorDequeue.TabIndex = 8;
            this.ErrorDequeue.Text = "日志出队";
            this.ErrorDequeue.UseVisualStyleBackColor = true;
            this.ErrorDequeue.Click += new System.EventHandler(this.ErrorDequeue_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "RabbitMQ 服务地址:";
            // 
            // LogServerTxt
            // 
            this.LogServerTxt.Location = new System.Drawing.Point(132, 21);
            this.LogServerTxt.Name = "LogServerTxt";
            this.LogServerTxt.ReadOnly = true;
            this.LogServerTxt.Size = new System.Drawing.Size(202, 21);
            this.LogServerTxt.TabIndex = 4;
            this.LogServerTxt.Text = "amqp://10.0.4.107:5672/";
            // 
            // StartLogServer
            // 
            this.StartLogServer.Location = new System.Drawing.Point(244, 47);
            this.StartLogServer.Name = "StartLogServer";
            this.StartLogServer.Size = new System.Drawing.Size(62, 23);
            this.StartLogServer.TabIndex = 3;
            this.StartLogServer.Text = "启动";
            this.StartLogServer.UseVisualStyleBackColor = true;
            this.StartLogServer.Click += new System.EventHandler(this.StartLogServer_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "new.ico");
            this.imageList1.Images.SetKeyName(1, "save.ico");
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(8, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 23);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 273);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // skinUI1
            // 
            this.skinUI1.Active = true;
            this.skinUI1.Button = true;
            this.skinUI1.Caption = true;
            this.skinUI1.CheckBox = true;
            this.skinUI1.ComboBox = true;
            this.skinUI1.ContextMenu = true;
            this.skinUI1.DisableTag = 999;
            this.skinUI1.Edit = true;
            this.skinUI1.GroupBox = true;
            this.skinUI1.ImageList = null;
            this.skinUI1.MaiMenu = true;
            this.skinUI1.Panel = true;
            this.skinUI1.Progress = true;
            this.skinUI1.RadioButton = true;
            this.skinUI1.ScrollBar = true;
            this.skinUI1.SkinFile = "D:\\dnt\\3.0\\Discuz.EntLib.RabbitMQ\\Skin\\wmpx-XMPX3.skn";
            this.skinUI1.SkinSteam = null;
            this.skinUI1.Spin = true;
            this.skinUI1.StatusBar = true;
            this.skinUI1.SystemMenu = true;
            this.skinUI1.TabControl = true;
            this.skinUI1.Text = "Mycontrol1=edit\r\nMycontrol2=edit\r\n";
            this.skinUI1.ToolBar = true;
            this.skinUI1.TrackBar = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.SuspendTime);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.ShortMsgServer);
            this.groupBox3.Controls.Add(this.StartShortMsgServer);
            this.groupBox3.Location = new System.Drawing.Point(1, 100);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(685, 79);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "    短消息队列服务配置:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(543, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "秒";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(365, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "批量发送间隔时间:";
            // 
            // SuspendTime
            // 
            this.SuspendTime.Location = new System.Drawing.Point(475, 22);
            this.SuspendTime.Name = "SuspendTime";
            this.SuspendTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SuspendTime.Size = new System.Drawing.Size(43, 21);
            this.SuspendTime.TabIndex = 9;
            this.SuspendTime.Text = "5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "RabbitMQ 服务地址:";
            // 
            // ShortMsgServer
            // 
            this.ShortMsgServer.Location = new System.Drawing.Point(132, 22);
            this.ShortMsgServer.Name = "ShortMsgServer";
            this.ShortMsgServer.ReadOnly = true;
            this.ShortMsgServer.Size = new System.Drawing.Size(202, 21);
            this.ShortMsgServer.TabIndex = 4;
            this.ShortMsgServer.Text = "amqp://10.0.4.107:5672/";
            // 
            // StartShortMsgServer
            // 
            this.StartShortMsgServer.Location = new System.Drawing.Point(244, 50);
            this.StartShortMsgServer.Name = "StartShortMsgServer";
            this.StartShortMsgServer.Size = new System.Drawing.Size(62, 23);
            this.StartShortMsgServer.TabIndex = 3;
            this.StartShortMsgServer.Text = "启动";
            this.StartShortMsgServer.UseVisualStyleBackColor = true;
            this.StartShortMsgServer.Click += new System.EventHandler(this.StartShortMsgServer_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(8, 96);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(24, 23);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 9;
            this.pictureBox3.TabStop = false;
            // 
            // RabbitMQ_Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 520);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RabbitMQ_Server";
            this.Text = "RabbitMQ配置管理器";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox MsgRichBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox LogServerTxt;
        private System.Windows.Forms.Button StartLogServer;
        private System.Windows.Forms.SaveFileDialog saveMsgFileDialog;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private DotNetSkin.SkinUI skinUI1;
        private System.Windows.Forms.Button ErrorDequeue;
        private System.Windows.Forms.TextBox ErrorTimer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ShortMsgServer;
        private System.Windows.Forms.Button StartShortMsgServer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SuspendTime;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}