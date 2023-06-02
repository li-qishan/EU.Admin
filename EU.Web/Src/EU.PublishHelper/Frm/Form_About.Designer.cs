namespace JianLian.HDIS.PublishHelper
{
    partial class Form_About
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
            this.gb_About = new System.Windows.Forms.GroupBox();
            this.lbl_Right = new System.Windows.Forms.Label();
            this.lbl_Url = new System.Windows.Forms.Label();
            this.lbl_System = new System.Windows.Forms.Label();
            this.lbl_Company = new System.Windows.Forms.Label();
            this.pb_Left = new System.Windows.Forms.PictureBox();
            this.gb_About.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Left)).BeginInit();
            this.SuspendLayout();
            // 
            // gb_About
            // 
            this.gb_About.Controls.Add(this.lbl_Right);
            this.gb_About.Controls.Add(this.lbl_Url);
            this.gb_About.Controls.Add(this.lbl_System);
            this.gb_About.Controls.Add(this.lbl_Company);
            this.gb_About.Location = new System.Drawing.Point(173, 1);
            this.gb_About.Name = "gb_About";
            this.gb_About.Size = new System.Drawing.Size(284, 153);
            this.gb_About.TabIndex = 1;
            this.gb_About.TabStop = false;
            // 
            // lbl_Right
            // 
            this.lbl_Right.AutoSize = true;
            this.lbl_Right.Location = new System.Drawing.Point(16, 120);
            this.lbl_Right.Name = "lbl_Right";
            this.lbl_Right.Size = new System.Drawing.Size(263, 12);
            this.lbl_Right.TabIndex = 4;
            this.lbl_Right.Text = "Copyright(c) 2020-2030 All Rights Reserved.";
            // 
            // lbl_Url
            // 
            this.lbl_Url.AutoSize = true;
            this.lbl_Url.Location = new System.Drawing.Point(16, 90);
            this.lbl_Url.Name = "lbl_Url";
            this.lbl_Url.Size = new System.Drawing.Size(155, 12);
            this.lbl_Url.TabIndex = 3;
            this.lbl_Url.Text = "http://cloud.eu-keji.com/";
            // 
            // lbl_System
            // 
            this.lbl_System.AutoSize = true;
            this.lbl_System.Location = new System.Drawing.Point(16, 30);
            this.lbl_System.Name = "lbl_System";
            this.lbl_System.Size = new System.Drawing.Size(113, 12);
            this.lbl_System.TabIndex = 2;
            this.lbl_System.Text = "PublishHelper v1.0";
            // 
            // lbl_Company
            // 
            this.lbl_Company.AutoSize = true;
            this.lbl_Company.Location = new System.Drawing.Point(16, 60);
            this.lbl_Company.Name = "lbl_Company";
            this.lbl_Company.Size = new System.Drawing.Size(149, 12);
            this.lbl_Company.TabIndex = 1;
            this.lbl_Company.Text = "苏州一优信息技术有限公司";
            // 
            // pb_Left
            // 
            this.pb_Left.Image = global::EU.PublishHelper.Properties.Resources.logo_7c906606;
            this.pb_Left.Location = new System.Drawing.Point(12, 10);
            this.pb_Left.Name = "pb_Left";
            this.pb_Left.Size = new System.Drawing.Size(155, 142);
            this.pb_Left.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_Left.TabIndex = 5;
            this.pb_Left.TabStop = false;
            // 
            // Form_About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 159);
            this.Controls.Add(this.pb_Left);
            this.Controls.Add(this.gb_About);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(478, 198);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(478, 198);
            this.Name = "Form_About";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "关于";
            this.gb_About.ResumeLayout(false);
            this.gb_About.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Left)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gb_About;
        private System.Windows.Forms.Label lbl_Right;
        private System.Windows.Forms.Label lbl_Url;
        private System.Windows.Forms.Label lbl_System;
        private System.Windows.Forms.Label lbl_Company;
        private System.Windows.Forms.PictureBox pb_Left;
    }
}