namespace JianLian.HDIS.PublishHelper
{
    partial class Form_UpgradeLog
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
            this.gb_middle = new System.Windows.Forms.GroupBox();
            this.ll_Clear = new System.Windows.Forms.LinkLabel();
            this.ll_Refresh = new System.Windows.Forms.LinkLabel();
            this.webBrowser_Upgrade = new System.Windows.Forms.WebBrowser();
            this.gb_2 = new System.Windows.Forms.GroupBox();
            this.ll_Copy = new System.Windows.Forms.LinkLabel();
            this.txt_content = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssl_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.cb_Refresh = new System.Windows.Forms.CheckBox();
            this.gb_middle.SuspendLayout();
            this.gb_2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_middle
            // 
            this.gb_middle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_middle.Controls.Add(this.cb_Refresh);
            this.gb_middle.Controls.Add(this.ll_Clear);
            this.gb_middle.Controls.Add(this.ll_Refresh);
            this.gb_middle.Controls.Add(this.webBrowser_Upgrade);
            this.gb_middle.Location = new System.Drawing.Point(12, 2);
            this.gb_middle.Name = "gb_middle";
            this.gb_middle.Size = new System.Drawing.Size(776, 176);
            this.gb_middle.TabIndex = 0;
            this.gb_middle.TabStop = false;
            this.gb_middle.Text = "WebBrowser";
            // 
            // ll_Clear
            // 
            this.ll_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ll_Clear.AutoSize = true;
            this.ll_Clear.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.ll_Clear.Location = new System.Drawing.Point(654, 2);
            this.ll_Clear.Name = "ll_Clear";
            this.ll_Clear.Size = new System.Drawing.Size(35, 12);
            this.ll_Clear.TabIndex = 3;
            this.ll_Clear.TabStop = true;
            this.ll_Clear.Text = "Clear";
            this.ll_Clear.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_Clear_LinkClicked);
            // 
            // ll_Refresh
            // 
            this.ll_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ll_Refresh.AutoSize = true;
            this.ll_Refresh.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.ll_Refresh.Location = new System.Drawing.Point(711, 2);
            this.ll_Refresh.Name = "ll_Refresh";
            this.ll_Refresh.Size = new System.Drawing.Size(47, 12);
            this.ll_Refresh.TabIndex = 2;
            this.ll_Refresh.TabStop = true;
            this.ll_Refresh.Text = "Refresh";
            this.ll_Refresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_Refresh_LinkClicked);
            // 
            // webBrowser_Upgrade
            // 
            this.webBrowser_Upgrade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser_Upgrade.Location = new System.Drawing.Point(3, 17);
            this.webBrowser_Upgrade.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser_Upgrade.Name = "webBrowser_Upgrade";
            this.webBrowser_Upgrade.Size = new System.Drawing.Size(770, 156);
            this.webBrowser_Upgrade.TabIndex = 2;
            this.webBrowser_Upgrade.Url = new System.Uri("https://www.tapd.cn/cloud_logins/login", System.UriKind.Absolute);
            // 
            // gb_2
            // 
            this.gb_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_2.Controls.Add(this.ll_Copy);
            this.gb_2.Controls.Add(this.txt_content);
            this.gb_2.Location = new System.Drawing.Point(12, 181);
            this.gb_2.Name = "gb_2";
            this.gb_2.Size = new System.Drawing.Size(776, 244);
            this.gb_2.TabIndex = 1;
            this.gb_2.TabStop = false;
            this.gb_2.Text = "Logger";
            // 
            // ll_Copy
            // 
            this.ll_Copy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ll_Copy.AutoSize = true;
            this.ll_Copy.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.ll_Copy.Location = new System.Drawing.Point(720, 2);
            this.ll_Copy.Name = "ll_Copy";
            this.ll_Copy.Size = new System.Drawing.Size(29, 12);
            this.ll_Copy.TabIndex = 1;
            this.ll_Copy.TabStop = true;
            this.ll_Copy.Text = "Copy";
            this.ll_Copy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_Copy_LinkClicked);
            // 
            // txt_content
            // 
            this.txt_content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_content.Location = new System.Drawing.Point(3, 17);
            this.txt_content.Multiline = true;
            this.txt_content.Name = "txt_content";
            this.txt_content.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_content.Size = new System.Drawing.Size(770, 224);
            this.txt_content.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssl_status
            // 
            this.tssl_status.Name = "tssl_status";
            this.tssl_status.Size = new System.Drawing.Size(0, 17);
            // 
            // cb_Refresh
            // 
            this.cb_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_Refresh.AutoSize = true;
            this.cb_Refresh.Location = new System.Drawing.Point(545, 1);
            this.cb_Refresh.Name = "cb_Refresh";
            this.cb_Refresh.Size = new System.Drawing.Size(90, 16);
            this.cb_Refresh.TabIndex = 4;
            this.cb_Refresh.Text = "AutoRefresh";
            this.cb_Refresh.UseVisualStyleBackColor = true;
            this.cb_Refresh.CheckedChanged += new System.EventHandler(this.cb_Refresh_CheckedChanged);
            // 
            // Form_UpgradeLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.gb_2);
            this.Controls.Add(this.gb_middle);
            this.Name = "Form_UpgradeLog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "升级日志抓取";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.gb_middle.ResumeLayout(false);
            this.gb_middle.PerformLayout();
            this.gb_2.ResumeLayout(false);
            this.gb_2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_middle;
        private System.Windows.Forms.WebBrowser webBrowser_Upgrade;
        private System.Windows.Forms.GroupBox gb_2;
        private System.Windows.Forms.TextBox txt_content;
        private System.Windows.Forms.LinkLabel ll_Copy;
        private System.Windows.Forms.LinkLabel ll_Refresh;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssl_status;
        private System.Windows.Forms.LinkLabel ll_Clear;
        private System.Windows.Forms.CheckBox cb_Refresh;
    }
}