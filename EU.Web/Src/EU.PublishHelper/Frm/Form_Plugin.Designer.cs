namespace JianLian.HDIS.PublishHelper
{
    partial class Form_Plugin
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_build = new System.Windows.Forms.CheckBox();
            this.txt_tsproject = new System.Windows.Forms.TextBox();
            this.lbl_ts = new System.Windows.Forms.Label();
            this.cb_custom = new System.Windows.Forms.CheckBox();
            this.txt_custom = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.txt_path = new System.Windows.Forms.TextBox();
            this.lbl_path = new System.Windows.Forms.Label();
            this.txt_version = new System.Windows.Forms.TextBox();
            this.lbl_version = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_build);
            this.groupBox1.Controls.Add(this.txt_tsproject);
            this.groupBox1.Controls.Add(this.lbl_ts);
            this.groupBox1.Controls.Add(this.cb_custom);
            this.groupBox1.Controls.Add(this.txt_custom);
            this.groupBox1.Controls.Add(this.btn_ok);
            this.groupBox1.Controls.Add(this.txt_path);
            this.groupBox1.Controls.Add(this.lbl_path);
            this.groupBox1.Controls.Add(this.txt_version);
            this.groupBox1.Controls.Add(this.lbl_version);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 199);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cb_build
            // 
            this.cb_build.AutoSize = true;
            this.cb_build.Checked = true;
            this.cb_build.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_build.Location = new System.Drawing.Point(30, 164);
            this.cb_build.Name = "cb_build";
            this.cb_build.Size = new System.Drawing.Size(72, 16);
            this.cb_build.TabIndex = 11;
            this.cb_build.Text = "编译项目";
            this.cb_build.UseVisualStyleBackColor = true;
            // 
            // txt_tsproject
            // 
            this.txt_tsproject.Location = new System.Drawing.Point(97, 21);
            this.txt_tsproject.Name = "txt_tsproject";
            this.txt_tsproject.Size = new System.Drawing.Size(247, 21);
            this.txt_tsproject.TabIndex = 10;
            this.txt_tsproject.Text = "E:\\code\\huhang\\customs";
            // 
            // lbl_ts
            // 
            this.lbl_ts.AutoSize = true;
            this.lbl_ts.Location = new System.Drawing.Point(28, 24);
            this.lbl_ts.Name = "lbl_ts";
            this.lbl_ts.Size = new System.Drawing.Size(65, 12);
            this.lbl_ts.TabIndex = 9;
            this.lbl_ts.Text = "项目路径：";
            // 
            // cb_custom
            // 
            this.cb_custom.AutoSize = true;
            this.cb_custom.Location = new System.Drawing.Point(30, 129);
            this.cb_custom.Name = "cb_custom";
            this.cb_custom.Size = new System.Drawing.Size(60, 16);
            this.cb_custom.TabIndex = 8;
            this.cb_custom.Text = "自定义";
            this.cb_custom.UseVisualStyleBackColor = true;
            this.cb_custom.CheckedChanged += new System.EventHandler(this.cb_custom_CheckedChanged);
            // 
            // txt_custom
            // 
            this.txt_custom.Location = new System.Drawing.Point(96, 127);
            this.txt_custom.Name = "txt_custom";
            this.txt_custom.Size = new System.Drawing.Size(247, 21);
            this.txt_custom.TabIndex = 7;
            this.txt_custom.Text = "Nanchong;Yongding";
            this.txt_custom.Visible = false;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(269, 160);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 4;
            this.btn_ok.Text = "确定(&Y)";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // txt_path
            // 
            this.txt_path.Location = new System.Drawing.Point(97, 57);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(247, 21);
            this.txt_path.TabIndex = 3;
            this.txt_path.Text = "C:\\Users\\Administrator\\Desktop\\ts";
            this.txt_path.Click += new System.EventHandler(this.txt_path_Click);
            // 
            // lbl_path
            // 
            this.lbl_path.AutoSize = true;
            this.lbl_path.Location = new System.Drawing.Point(28, 59);
            this.lbl_path.Name = "lbl_path";
            this.lbl_path.Size = new System.Drawing.Size(65, 12);
            this.lbl_path.TabIndex = 2;
            this.lbl_path.Text = "保存路径：";
            // 
            // txt_version
            // 
            this.txt_version.Location = new System.Drawing.Point(97, 93);
            this.txt_version.Name = "txt_version";
            this.txt_version.Size = new System.Drawing.Size(247, 21);
            this.txt_version.TabIndex = 1;
            this.txt_version.Text = "1.0.1";
            // 
            // lbl_version
            // 
            this.lbl_version.AutoSize = true;
            this.lbl_version.Location = new System.Drawing.Point(28, 94);
            this.lbl_version.Name = "lbl_version";
            this.lbl_version.Size = new System.Drawing.Size(53, 12);
            this.lbl_version.TabIndex = 0;
            this.lbl_version.Text = "版本号：";
            // 
            // Form_Plugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 223);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(409, 262);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(409, 262);
            this.Name = "Form_Plugin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成插件";
            this.Load += new System.EventHandler(this.Form_Plugin_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.TextBox txt_path;
        private System.Windows.Forms.Label lbl_path;
        private System.Windows.Forms.TextBox txt_version;
        private System.Windows.Forms.Label lbl_version;
        private System.Windows.Forms.CheckBox cb_custom;
        private System.Windows.Forms.TextBox txt_custom;
        private System.Windows.Forms.TextBox txt_tsproject;
        private System.Windows.Forms.Label lbl_ts;
        private System.Windows.Forms.CheckBox cb_build;
    }
}