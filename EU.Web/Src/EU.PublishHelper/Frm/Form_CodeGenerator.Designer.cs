namespace JianLian.HDIS.PublishHelper.Frm
{
    partial class Form_CodeGenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_CodeGenerator));
            this.txt_value = new System.Windows.Forms.TextBox();
            this.cmb_action = new System.Windows.Forms.ComboBox();
            this.cmb_models = new System.Windows.Forms.ComboBox();
            this.lbl_model = new System.Windows.Forms.Label();
            this.lbl_api = new System.Windows.Forms.Label();
            this.txt_tname = new System.Windows.Forms.TextBox();
            this.gb_bottom = new System.Windows.Forms.GroupBox();
            this.ll_Copy = new System.Windows.Forms.LinkLabel();
            this.gb_top = new System.Windows.Forms.GroupBox();
            this.lbl_type = new System.Windows.Forms.Label();
            this.cmb_type = new System.Windows.Forms.ComboBox();
            this.gb_bottom.SuspendLayout();
            this.gb_top.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_value
            // 
            this.txt_value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_value.Location = new System.Drawing.Point(3, 20);
            this.txt_value.MaxLength = 0;
            this.txt_value.Multiline = true;
            this.txt_value.Name = "txt_value";
            this.txt_value.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_value.Size = new System.Drawing.Size(611, 302);
            this.txt_value.TabIndex = 8;
            // 
            // cmb_action
            // 
            this.cmb_action.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_action.FormattingEnabled = true;
            this.cmb_action.Items.AddRange(new object[] {
            "pc_columns",
            "pc_form",
            "pc_model",
            "android_model"});
            this.cmb_action.Location = new System.Drawing.Point(434, 67);
            this.cmb_action.Name = "cmb_action";
            this.cmb_action.Size = new System.Drawing.Size(136, 23);
            this.cmb_action.TabIndex = 17;
            this.cmb_action.SelectedIndexChanged += new System.EventHandler(this.cmb_action_SelectedIndexChanged);
            // 
            // cmb_models
            // 
            this.cmb_models.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_models.FormattingEnabled = true;
            this.cmb_models.Location = new System.Drawing.Point(101, 67);
            this.cmb_models.Name = "cmb_models";
            this.cmb_models.Size = new System.Drawing.Size(303, 23);
            this.cmb_models.TabIndex = 4;
            this.cmb_models.SelectedIndexChanged += new System.EventHandler(this.cmb_models_SelectedIndexChanged);
            // 
            // lbl_model
            // 
            this.lbl_model.AutoSize = true;
            this.lbl_model.Location = new System.Drawing.Point(23, 70);
            this.lbl_model.Name = "lbl_model";
            this.lbl_model.Size = new System.Drawing.Size(67, 15);
            this.lbl_model.TabIndex = 9;
            this.lbl_model.Text = "选择模板";
            // 
            // lbl_api
            // 
            this.lbl_api.AutoSize = true;
            this.lbl_api.Location = new System.Drawing.Point(23, 44);
            this.lbl_api.Name = "lbl_api";
            this.lbl_api.Size = new System.Drawing.Size(67, 15);
            this.lbl_api.TabIndex = 2;
            this.lbl_api.Text = "快速搜索";
            // 
            // txt_tname
            // 
            this.txt_tname.Location = new System.Drawing.Point(101, 40);
            this.txt_tname.Name = "txt_tname";
            this.txt_tname.Size = new System.Drawing.Size(303, 24);
            this.txt_tname.TabIndex = 2;
            this.txt_tname.TextChanged += new System.EventHandler(this.txt_tname_TextChanged);
            // 
            // gb_bottom
            // 
            this.gb_bottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_bottom.Controls.Add(this.ll_Copy);
            this.gb_bottom.Controls.Add(this.txt_value);
            this.gb_bottom.Font = new System.Drawing.Font("宋体", 11F);
            this.gb_bottom.Location = new System.Drawing.Point(12, 113);
            this.gb_bottom.Name = "gb_bottom";
            this.gb_bottom.Size = new System.Drawing.Size(617, 325);
            this.gb_bottom.TabIndex = 3;
            this.gb_bottom.TabStop = false;
            // 
            // ll_Copy
            // 
            this.ll_Copy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ll_Copy.AutoSize = true;
            this.ll_Copy.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.ll_Copy.Location = new System.Drawing.Point(544, 0);
            this.ll_Copy.Name = "ll_Copy";
            this.ll_Copy.Size = new System.Drawing.Size(67, 15);
            this.ll_Copy.TabIndex = 9;
            this.ll_Copy.TabStop = true;
            this.ll_Copy.Text = "复制代码";
            this.ll_Copy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_Copy_LinkClicked);
            // 
            // gb_top
            // 
            this.gb_top.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_top.Controls.Add(this.lbl_type);
            this.gb_top.Controls.Add(this.cmb_type);
            this.gb_top.Controls.Add(this.cmb_action);
            this.gb_top.Controls.Add(this.cmb_models);
            this.gb_top.Controls.Add(this.lbl_model);
            this.gb_top.Controls.Add(this.lbl_api);
            this.gb_top.Controls.Add(this.txt_tname);
            this.gb_top.Font = new System.Drawing.Font("宋体", 11F);
            this.gb_top.Location = new System.Drawing.Point(12, 8);
            this.gb_top.Name = "gb_top";
            this.gb_top.Size = new System.Drawing.Size(617, 99);
            this.gb_top.TabIndex = 2;
            this.gb_top.TabStop = false;
            // 
            // lbl_type
            // 
            this.lbl_type.AutoSize = true;
            this.lbl_type.Location = new System.Drawing.Point(23, 17);
            this.lbl_type.Name = "lbl_type";
            this.lbl_type.Size = new System.Drawing.Size(67, 15);
            this.lbl_type.TabIndex = 20;
            this.lbl_type.Text = "选择模块";
            // 
            // cmb_type
            // 
            this.cmb_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_type.FormattingEnabled = true;
            this.cmb_type.Items.AddRange(new object[] {
            "认证授权(auth)",
            "系统模块(system)",
            "科室模块(dept)",
            "透析模块(dialysis)",
            "电子病历(emr)",
            "排床模块(schedule)",
            "模板模块(tmpl)",
            "流程模块(procedure)"});
            this.cmb_type.Location = new System.Drawing.Point(101, 14);
            this.cmb_type.Name = "cmb_type";
            this.cmb_type.Size = new System.Drawing.Size(303, 23);
            this.cmb_type.TabIndex = 19;
            this.cmb_type.SelectedIndexChanged += new System.EventHandler(this.cmb_type_SelectedIndexChanged);
            // 
            // Form_CodeGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 450);
            this.Controls.Add(this.gb_bottom);
            this.Controls.Add(this.gb_top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_CodeGenerator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码生成";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_CodeGenerator_FormClosing);
            this.gb_bottom.ResumeLayout(false);
            this.gb_bottom.PerformLayout();
            this.gb_top.ResumeLayout(false);
            this.gb_top.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txt_value;
        private System.Windows.Forms.ComboBox cmb_action;
        private System.Windows.Forms.ComboBox cmb_models;
        private System.Windows.Forms.Label lbl_model;
        private System.Windows.Forms.Label lbl_api;
        private System.Windows.Forms.TextBox txt_tname;
        private System.Windows.Forms.GroupBox gb_bottom;
        private System.Windows.Forms.GroupBox gb_top;
        private System.Windows.Forms.Label lbl_type;
        private System.Windows.Forms.ComboBox cmb_type;
        private System.Windows.Forms.LinkLabel ll_Copy;
    }
}