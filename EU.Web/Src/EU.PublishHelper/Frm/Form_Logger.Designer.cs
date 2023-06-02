namespace JianLian.HDIS.PublishHelper
{
    partial class Form_Logger
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
            this.groupBox_Log = new System.Windows.Forms.GroupBox();
            this.txt_Log = new System.Windows.Forms.RichTextBox();
            this.linkLabel_Refresh = new System.Windows.Forms.LinkLabel();
            this.groupBox_Log.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Log
            // 
            this.groupBox_Log.Controls.Add(this.txt_Log);
            this.groupBox_Log.Controls.Add(this.linkLabel_Refresh);
            this.groupBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Log.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Log.Name = "groupBox_Log";
            this.groupBox_Log.Size = new System.Drawing.Size(802, 523);
            this.groupBox_Log.TabIndex = 0;
            this.groupBox_Log.TabStop = false;
            // 
            // txt_Log
            // 
            this.txt_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Log.Location = new System.Drawing.Point(3, 17);
            this.txt_Log.Name = "txt_Log";
            this.txt_Log.Size = new System.Drawing.Size(796, 503);
            this.txt_Log.TabIndex = 3;
            this.txt_Log.Text = "";
            this.txt_Log.TextChanged += new System.EventHandler(this.txt_Log_TextChanged);
            // 
            // linkLabel_Refresh
            // 
            this.linkLabel_Refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel_Refresh.AutoSize = true;
            this.linkLabel_Refresh.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel_Refresh.Location = new System.Drawing.Point(743, 2);
            this.linkLabel_Refresh.Name = "linkLabel_Refresh";
            this.linkLabel_Refresh.Size = new System.Drawing.Size(47, 12);
            this.linkLabel_Refresh.TabIndex = 2;
            this.linkLabel_Refresh.TabStop = true;
            this.linkLabel_Refresh.Text = "Refresh";
            this.linkLabel_Refresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_Refresh_LinkClicked);
            // 
            // Form_Logger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 523);
            this.Controls.Add(this.groupBox_Log);
            this.Name = "Form_Logger";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "日志";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Logger_FormClosing);
            this.Load += new System.EventHandler(this.Form_Logger_Load);
            this.groupBox_Log.ResumeLayout(false);
            this.groupBox_Log.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_Log;
        private System.Windows.Forms.LinkLabel linkLabel_Refresh;
        private System.Windows.Forms.RichTextBox txt_Log;
    }
}