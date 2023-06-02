namespace JianLian.HDIS.PublishHelper
{
    partial class Form_Hospital_Item
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
            this.gb_main = new System.Windows.Forms.GroupBox();
            this.lbl_www_default = new System.Windows.Forms.Label();
            this.lbl_sc_default = new System.Windows.Forms.Label();
            this.txt_Port = new System.Windows.Forms.TextBox();
            this.lbl_Port = new System.Windows.Forms.Label();
            this.txt_WWWPath = new System.Windows.Forms.TextBox();
            this.lbl_WWWPath = new System.Windows.Forms.Label();
            this.txt_SCPath = new System.Windows.Forms.TextBox();
            this.lbl_SCPath = new System.Windows.Forms.Label();
            this.txt_FileName = new System.Windows.Forms.TextBox();
            this.lbl_FileName = new System.Windows.Forms.Label();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.lbl_Name = new System.Windows.Forms.Label();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.gb_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_main
            // 
            this.gb_main.Controls.Add(this.lbl_www_default);
            this.gb_main.Controls.Add(this.lbl_sc_default);
            this.gb_main.Controls.Add(this.txt_Port);
            this.gb_main.Controls.Add(this.lbl_Port);
            this.gb_main.Controls.Add(this.txt_WWWPath);
            this.gb_main.Controls.Add(this.lbl_WWWPath);
            this.gb_main.Controls.Add(this.txt_SCPath);
            this.gb_main.Controls.Add(this.lbl_SCPath);
            this.gb_main.Controls.Add(this.txt_FileName);
            this.gb_main.Controls.Add(this.lbl_FileName);
            this.gb_main.Controls.Add(this.txt_Name);
            this.gb_main.Controls.Add(this.lbl_Name);
            this.gb_main.Location = new System.Drawing.Point(12, 12);
            this.gb_main.Name = "gb_main";
            this.gb_main.Size = new System.Drawing.Size(270, 190);
            this.gb_main.TabIndex = 0;
            this.gb_main.TabStop = false;
            // 
            // lbl_www_default
            // 
            this.lbl_www_default.AutoSize = true;
            this.lbl_www_default.ForeColor = System.Drawing.Color.Red;
            this.lbl_www_default.Location = new System.Drawing.Point(22, 171);
            this.lbl_www_default.Name = "lbl_www_default";
            this.lbl_www_default.Size = new System.Drawing.Size(59, 12);
            this.lbl_www_default.TabIndex = 19;
            this.lbl_www_default.Text = "(...\\www)";
            // 
            // lbl_sc_default
            // 
            this.lbl_sc_default.AutoSize = true;
            this.lbl_sc_default.ForeColor = System.Drawing.Color.Red;
            this.lbl_sc_default.Location = new System.Drawing.Point(22, 137);
            this.lbl_sc_default.Name = "lbl_sc_default";
            this.lbl_sc_default.Size = new System.Drawing.Size(53, 12);
            this.lbl_sc_default.TabIndex = 18;
            this.lbl_sc_default.Text = "(...\\sc)";
            // 
            // txt_Port
            // 
            this.txt_Port.Location = new System.Drawing.Point(105, 86);
            this.txt_Port.Name = "txt_Port";
            this.txt_Port.Size = new System.Drawing.Size(144, 21);
            this.txt_Port.TabIndex = 16;
            this.txt_Port.Text = "600";
            // 
            // lbl_Port
            // 
            this.lbl_Port.AutoSize = true;
            this.lbl_Port.Location = new System.Drawing.Point(22, 89);
            this.lbl_Port.Name = "lbl_Port";
            this.lbl_Port.Size = new System.Drawing.Size(59, 12);
            this.lbl_Port.TabIndex = 17;
            this.lbl_Port.Text = "默认端口:";
            this.lbl_Port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_WWWPath
            // 
            this.txt_WWWPath.Location = new System.Drawing.Point(105, 152);
            this.txt_WWWPath.Name = "txt_WWWPath";
            this.txt_WWWPath.Size = new System.Drawing.Size(144, 21);
            this.txt_WWWPath.TabIndex = 12;
            this.txt_WWWPath.Text = "E:\\code\\huhang\\ui\\www";
            // 
            // lbl_WWWPath
            // 
            this.lbl_WWWPath.AutoSize = true;
            this.lbl_WWWPath.Location = new System.Drawing.Point(22, 155);
            this.lbl_WWWPath.Name = "lbl_WWWPath";
            this.lbl_WWWPath.Size = new System.Drawing.Size(59, 12);
            this.lbl_WWWPath.TabIndex = 13;
            this.lbl_WWWPath.Text = "前端路径:";
            this.lbl_WWWPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_SCPath
            // 
            this.txt_SCPath.Location = new System.Drawing.Point(105, 119);
            this.txt_SCPath.Name = "txt_SCPath";
            this.txt_SCPath.Size = new System.Drawing.Size(144, 21);
            this.txt_SCPath.TabIndex = 10;
            this.txt_SCPath.Text = "E:\\code\\huhang\\sc";
            // 
            // lbl_SCPath
            // 
            this.lbl_SCPath.AutoSize = true;
            this.lbl_SCPath.Location = new System.Drawing.Point(22, 122);
            this.lbl_SCPath.Name = "lbl_SCPath";
            this.lbl_SCPath.Size = new System.Drawing.Size(59, 12);
            this.lbl_SCPath.TabIndex = 11;
            this.lbl_SCPath.Text = "后端路径:";
            this.lbl_SCPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_FileName
            // 
            this.txt_FileName.Location = new System.Drawing.Point(105, 53);
            this.txt_FileName.Name = "txt_FileName";
            this.txt_FileName.Size = new System.Drawing.Size(144, 21);
            this.txt_FileName.TabIndex = 8;
            this.txt_FileName.Text = "yiyou";
            // 
            // lbl_FileName
            // 
            this.lbl_FileName.AutoSize = true;
            this.lbl_FileName.Location = new System.Drawing.Point(22, 56);
            this.lbl_FileName.Name = "lbl_FileName";
            this.lbl_FileName.Size = new System.Drawing.Size(71, 12);
            this.lbl_FileName.TabIndex = 9;
            this.lbl_FileName.Text = "文件夹名称:";
            this.lbl_FileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_Name
            // 
            this.txt_Name.Location = new System.Drawing.Point(105, 20);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(144, 21);
            this.txt_Name.TabIndex = 6;
            this.txt_Name.Text = "优智云";
            // 
            // lbl_Name
            // 
            this.lbl_Name.AutoSize = true;
            this.lbl_Name.Location = new System.Drawing.Point(22, 23);
            this.lbl_Name.Name = "lbl_Name";
            this.lbl_Name.Size = new System.Drawing.Size(59, 12);
            this.lbl_Name.TabIndex = 7;
            this.lbl_Name.Text = "平台名称:";
            this.lbl_Name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_Ok
            // 
            this.btn_Ok.Location = new System.Drawing.Point(30, 218);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(75, 23);
            this.btn_Ok.TabIndex = 1;
            this.btn_Ok.Text = "确定(&O)";
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(177, 218);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "取消(&C)";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // Form_Hospital_Item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 256);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.gb_main);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(312, 295);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(312, 295);
            this.Name = "Form_Hospital_Item";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增";
            this.Load += new System.EventHandler(this.Form_Hospital_Item_Load);
            this.gb_main.ResumeLayout(false);
            this.gb_main.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_main;
        private System.Windows.Forms.TextBox txt_WWWPath;
        private System.Windows.Forms.Label lbl_WWWPath;
        private System.Windows.Forms.TextBox txt_SCPath;
        private System.Windows.Forms.Label lbl_SCPath;
        private System.Windows.Forms.TextBox txt_FileName;
        private System.Windows.Forms.Label lbl_FileName;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.Button btn_Ok;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.TextBox txt_Port;
        private System.Windows.Forms.Label lbl_Port;
        private System.Windows.Forms.Label lbl_www_default;
        private System.Windows.Forms.Label lbl_sc_default;
    }
}