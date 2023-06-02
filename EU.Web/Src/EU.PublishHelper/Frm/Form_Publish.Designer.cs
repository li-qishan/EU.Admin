namespace JianLian.HDIS.PublishHelper
{
    partial class Form_Publish
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
            this.groupBox_Top = new System.Windows.Forms.GroupBox();
            this.lbl_hopital = new System.Windows.Forms.Label();
            this.cmb_Hospital = new System.Windows.Forms.ComboBox();
            this.lbl_publish = new System.Windows.Forms.Label();
            this.lbl_dev = new System.Windows.Forms.Label();
            this.cmb_Publish = new System.Windows.Forms.ComboBox();
            this.cmb_Dev = new System.Windows.Forms.ComboBox();
            this.groupBox_Down = new System.Windows.Forms.GroupBox();
            this.lbl_desc = new System.Windows.Forms.Label();
            this.cb_Download = new System.Windows.Forms.CheckBox();
            this.cb_Git = new System.Windows.Forms.CheckBox();
            this.cmb_TS = new System.Windows.Forms.ComboBox();
            this.cb_TS = new System.Windows.Forms.CheckBox();
            this.cb_Upgrade = new System.Windows.Forms.CheckBox();
            this.cb_Publish = new System.Windows.Forms.CheckBox();
            this.cb_CreateDbFile = new System.Windows.Forms.CheckBox();
            this.txt_version = new System.Windows.Forms.TextBox();
            this.lbl_version = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox_Top.SuspendLayout();
            this.groupBox_Down.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Top
            // 
            this.groupBox_Top.Controls.Add(this.lbl_hopital);
            this.groupBox_Top.Controls.Add(this.cmb_Hospital);
            this.groupBox_Top.Controls.Add(this.lbl_publish);
            this.groupBox_Top.Controls.Add(this.lbl_dev);
            this.groupBox_Top.Controls.Add(this.cmb_Publish);
            this.groupBox_Top.Controls.Add(this.cmb_Dev);
            this.groupBox_Top.Location = new System.Drawing.Point(12, 12);
            this.groupBox_Top.Name = "groupBox_Top";
            this.groupBox_Top.Size = new System.Drawing.Size(371, 142);
            this.groupBox_Top.TabIndex = 0;
            this.groupBox_Top.TabStop = false;
            this.groupBox_Top.Text = "服务器";
            // 
            // lbl_hopital
            // 
            this.lbl_hopital.AutoSize = true;
            this.lbl_hopital.Location = new System.Drawing.Point(20, 74);
            this.lbl_hopital.Name = "lbl_hopital";
            this.lbl_hopital.Size = new System.Drawing.Size(65, 12);
            this.lbl_hopital.TabIndex = 13;
            this.lbl_hopital.Text = "医院名称：";
            // 
            // cmb_Hospital
            // 
            this.cmb_Hospital.FormattingEnabled = true;
            this.cmb_Hospital.Location = new System.Drawing.Point(120, 71);
            this.cmb_Hospital.Name = "cmb_Hospital";
            this.cmb_Hospital.Size = new System.Drawing.Size(224, 20);
            this.cmb_Hospital.TabIndex = 12;
            this.cmb_Hospital.SelectedIndexChanged += new System.EventHandler(this.cmb_Hospital_SelectedIndexChanged);
            // 
            // lbl_publish
            // 
            this.lbl_publish.AutoSize = true;
            this.lbl_publish.Location = new System.Drawing.Point(19, 108);
            this.lbl_publish.Name = "lbl_publish";
            this.lbl_publish.Size = new System.Drawing.Size(77, 12);
            this.lbl_publish.TabIndex = 11;
            this.lbl_publish.Text = "发布服务器：";
            // 
            // lbl_dev
            // 
            this.lbl_dev.AutoSize = true;
            this.lbl_dev.Location = new System.Drawing.Point(19, 37);
            this.lbl_dev.Name = "lbl_dev";
            this.lbl_dev.Size = new System.Drawing.Size(77, 12);
            this.lbl_dev.TabIndex = 10;
            this.lbl_dev.Text = "开发服务器：";
            // 
            // cmb_Publish
            // 
            this.cmb_Publish.FormattingEnabled = true;
            this.cmb_Publish.Location = new System.Drawing.Point(119, 105);
            this.cmb_Publish.Name = "cmb_Publish";
            this.cmb_Publish.Size = new System.Drawing.Size(225, 20);
            this.cmb_Publish.TabIndex = 1;
            this.cmb_Publish.SelectedIndexChanged += new System.EventHandler(this.cmb_Publish_SelectedIndexChanged);
            // 
            // cmb_Dev
            // 
            this.cmb_Dev.FormattingEnabled = true;
            this.cmb_Dev.Location = new System.Drawing.Point(119, 34);
            this.cmb_Dev.Name = "cmb_Dev";
            this.cmb_Dev.Size = new System.Drawing.Size(225, 20);
            this.cmb_Dev.TabIndex = 0;
            this.cmb_Dev.SelectedIndexChanged += new System.EventHandler(this.cmb_Dev_SelectedIndexChanged);
            // 
            // groupBox_Down
            // 
            this.groupBox_Down.Controls.Add(this.lbl_desc);
            this.groupBox_Down.Controls.Add(this.cb_Download);
            this.groupBox_Down.Controls.Add(this.cb_Git);
            this.groupBox_Down.Controls.Add(this.cmb_TS);
            this.groupBox_Down.Controls.Add(this.cb_TS);
            this.groupBox_Down.Controls.Add(this.cb_Upgrade);
            this.groupBox_Down.Controls.Add(this.cb_Publish);
            this.groupBox_Down.Controls.Add(this.cb_CreateDbFile);
            this.groupBox_Down.Controls.Add(this.txt_version);
            this.groupBox_Down.Controls.Add(this.lbl_version);
            this.groupBox_Down.Location = new System.Drawing.Point(12, 160);
            this.groupBox_Down.Name = "groupBox_Down";
            this.groupBox_Down.Size = new System.Drawing.Size(371, 221);
            this.groupBox_Down.TabIndex = 1;
            this.groupBox_Down.TabStop = false;
            this.groupBox_Down.Text = "发布选项";
            // 
            // lbl_desc
            // 
            this.lbl_desc.AutoSize = true;
            this.lbl_desc.ForeColor = System.Drawing.Color.Red;
            this.lbl_desc.Location = new System.Drawing.Point(6, 173);
            this.lbl_desc.Name = "lbl_desc";
            this.lbl_desc.Size = new System.Drawing.Size(359, 36);
            this.lbl_desc.TabIndex = 16;
            this.lbl_desc.Text = "1、不勾选升级包则为完整安装包\r\n2、勾选升级包，不勾选TS插件包，则为除TS组件外的模块包\r\n3、勾选升级包，再勾选TS插件包，选择指定医院，则打包TS插件包";
            // 
            // cb_Download
            // 
            this.cb_Download.AutoSize = true;
            this.cb_Download.Checked = true;
            this.cb_Download.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Download.Location = new System.Drawing.Point(204, 92);
            this.cb_Download.Name = "cb_Download";
            this.cb_Download.Size = new System.Drawing.Size(120, 16);
            this.cb_Download.TabIndex = 16;
            this.cb_Download.Text = "打包完成自动下载";
            this.cb_Download.UseVisualStyleBackColor = true;
            // 
            // cb_Git
            // 
            this.cb_Git.AutoSize = true;
            this.cb_Git.Checked = true;
            this.cb_Git.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Git.Location = new System.Drawing.Point(22, 92);
            this.cb_Git.Name = "cb_Git";
            this.cb_Git.Size = new System.Drawing.Size(120, 16);
            this.cb_Git.TabIndex = 15;
            this.cb_Git.Text = "自动拉取最新代码";
            this.cb_Git.UseVisualStyleBackColor = true;
            // 
            // cmb_TS
            // 
            this.cmb_TS.FormattingEnabled = true;
            this.cmb_TS.Location = new System.Drawing.Point(204, 150);
            this.cmb_TS.Name = "cmb_TS";
            this.cmb_TS.Size = new System.Drawing.Size(121, 20);
            this.cmb_TS.TabIndex = 14;
            // 
            // cb_TS
            // 
            this.cb_TS.AutoSize = true;
            this.cb_TS.Location = new System.Drawing.Point(204, 121);
            this.cb_TS.Name = "cb_TS";
            this.cb_TS.Size = new System.Drawing.Size(72, 16);
            this.cb_TS.TabIndex = 13;
            this.cb_TS.Text = "TS插件包";
            this.cb_TS.UseVisualStyleBackColor = true;
            this.cb_TS.CheckedChanged += new System.EventHandler(this.cb_TS_CheckedChanged);
            // 
            // cb_Upgrade
            // 
            this.cb_Upgrade.AutoSize = true;
            this.cb_Upgrade.Checked = true;
            this.cb_Upgrade.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Upgrade.Location = new System.Drawing.Point(21, 121);
            this.cb_Upgrade.Name = "cb_Upgrade";
            this.cb_Upgrade.Size = new System.Drawing.Size(60, 16);
            this.cb_Upgrade.TabIndex = 12;
            this.cb_Upgrade.Text = "升级包";
            this.cb_Upgrade.UseVisualStyleBackColor = true;
            this.cb_Upgrade.CheckedChanged += new System.EventHandler(this.cb_Upgrade_CheckedChanged);
            // 
            // cb_Publish
            // 
            this.cb_Publish.AutoSize = true;
            this.cb_Publish.Checked = true;
            this.cb_Publish.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Publish.Location = new System.Drawing.Point(204, 63);
            this.cb_Publish.Name = "cb_Publish";
            this.cb_Publish.Size = new System.Drawing.Size(132, 16);
            this.cb_Publish.TabIndex = 11;
            this.cb_Publish.Text = "打包前执行发布流程";
            this.cb_Publish.UseVisualStyleBackColor = true;
            // 
            // cb_CreateDbFile
            // 
            this.cb_CreateDbFile.AutoSize = true;
            this.cb_CreateDbFile.Checked = true;
            this.cb_CreateDbFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_CreateDbFile.Location = new System.Drawing.Point(21, 63);
            this.cb_CreateDbFile.Name = "cb_CreateDbFile";
            this.cb_CreateDbFile.Size = new System.Drawing.Size(132, 16);
            this.cb_CreateDbFile.TabIndex = 10;
            this.cb_CreateDbFile.Text = "同时生成数据库脚本";
            this.cb_CreateDbFile.UseVisualStyleBackColor = true;
            // 
            // txt_version
            // 
            this.txt_version.Location = new System.Drawing.Point(119, 30);
            this.txt_version.Name = "txt_version";
            this.txt_version.Size = new System.Drawing.Size(225, 21);
            this.txt_version.TabIndex = 6;
            this.txt_version.Text = "3.0.201027.develop";
            // 
            // lbl_version
            // 
            this.lbl_version.AutoSize = true;
            this.lbl_version.Location = new System.Drawing.Point(19, 33);
            this.lbl_version.Name = "lbl_version";
            this.lbl_version.Size = new System.Drawing.Size(77, 12);
            this.lbl_version.TabIndex = 9;
            this.lbl_version.Text = "打包版本号：";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(286, 399);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(70, 23);
            this.btn_cancel.TabIndex = 15;
            this.btn_cancel.Text = "取消(&C)";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(167, 399);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "确定(&Y)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Form_Publish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 434);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox_Down);
            this.Controls.Add(this.groupBox_Top);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(409, 473);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(409, 473);
            this.Name = "Form_Publish";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发布新版本";
            this.Load += new System.EventHandler(this.Form_Publish_Load);
            this.groupBox_Top.ResumeLayout(false);
            this.groupBox_Top.PerformLayout();
            this.groupBox_Down.ResumeLayout(false);
            this.groupBox_Down.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_Top;
        private System.Windows.Forms.GroupBox groupBox_Down;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmb_Publish;
        private System.Windows.Forms.ComboBox cmb_Dev;
        private System.Windows.Forms.TextBox txt_version;
        private System.Windows.Forms.Label lbl_version;
        private System.Windows.Forms.Label lbl_publish;
        private System.Windows.Forms.Label lbl_dev;
        private System.Windows.Forms.Label lbl_hopital;
        private System.Windows.Forms.ComboBox cmb_Hospital;
        private System.Windows.Forms.CheckBox cb_CreateDbFile;
        private System.Windows.Forms.CheckBox cb_Publish;
        private System.Windows.Forms.CheckBox cb_Upgrade;
        private System.Windows.Forms.CheckBox cb_TS;
        private System.Windows.Forms.ComboBox cmb_TS;
        private System.Windows.Forms.CheckBox cb_Git;
        private System.Windows.Forms.CheckBox cb_Download;
        private System.Windows.Forms.Label lbl_desc;
    }
}