namespace JianLian.HDIS.PublishHelper
{
    partial class Form_DevServer
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
            this.gb_Main = new System.Windows.Forms.GroupBox();
            this.listView_Dev = new System.Windows.Forms.ListView();
            this.DevName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Password = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuPassword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Port = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_ListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Hospital = new System.Windows.Forms.ToolStripMenuItem();
            this.gb_Main.SuspendLayout();
            this.contextMenuStrip_ListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_Main
            // 
            this.gb_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main.Controls.Add(this.listView_Dev);
            this.gb_Main.Location = new System.Drawing.Point(12, 12);
            this.gb_Main.Name = "gb_Main";
            this.gb_Main.Size = new System.Drawing.Size(776, 426);
            this.gb_Main.TabIndex = 0;
            this.gb_Main.TabStop = false;
            // 
            // listView_Dev
            // 
            this.listView_Dev.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DevName,
            this.IP,
            this.UserName,
            this.Password,
            this.SuPassword,
            this.Port});
            this.listView_Dev.ContextMenuStrip = this.contextMenuStrip_ListView;
            this.listView_Dev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Dev.FullRowSelect = true;
            this.listView_Dev.GridLines = true;
            this.listView_Dev.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_Dev.HideSelection = false;
            this.listView_Dev.Location = new System.Drawing.Point(3, 17);
            this.listView_Dev.MultiSelect = false;
            this.listView_Dev.Name = "listView_Dev";
            this.listView_Dev.Size = new System.Drawing.Size(770, 406);
            this.listView_Dev.TabIndex = 0;
            this.listView_Dev.UseCompatibleStateImageBehavior = false;
            this.listView_Dev.View = System.Windows.Forms.View.Details;
            this.listView_Dev.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_Dev_MouseDoubleClick);
            // 
            // DevName
            // 
            this.DevName.Text = "名称";
            this.DevName.Width = 100;
            // 
            // IP
            // 
            this.IP.Text = "Ip";
            this.IP.Width = 150;
            // 
            // UserName
            // 
            this.UserName.Text = "用户名";
            this.UserName.Width = 150;
            // 
            // Password
            // 
            this.Password.Text = "密码";
            this.Password.Width = 150;
            // 
            // SuPassword
            // 
            this.SuPassword.Text = "Su密码";
            this.SuPassword.Width = 150;
            // 
            // Port
            // 
            this.Port.Text = "端口";
            this.Port.Width = 50;
            // 
            // contextMenuStrip_ListView
            // 
            this.contextMenuStrip_ListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Insert,
            this.tsmi_Edit,
            this.tsmi_Delete,
            this.tsmi_Hospital});
            this.contextMenuStrip_ListView.Name = "contextMenuStrip_Main";
            this.contextMenuStrip_ListView.Size = new System.Drawing.Size(142, 92);
            // 
            // tsmi_Insert
            // 
            this.tsmi_Insert.Name = "tsmi_Insert";
            this.tsmi_Insert.Size = new System.Drawing.Size(141, 22);
            this.tsmi_Insert.Text = "新增(&N)";
            this.tsmi_Insert.Click += new System.EventHandler(this.tsmi_Insert_Click);
            // 
            // tsmi_Edit
            // 
            this.tsmi_Edit.Name = "tsmi_Edit";
            this.tsmi_Edit.Size = new System.Drawing.Size(141, 22);
            this.tsmi_Edit.Text = "编辑(&E)";
            this.tsmi_Edit.Click += new System.EventHandler(this.tsmi_Edit_Click);
            // 
            // tsmi_Delete
            // 
            this.tsmi_Delete.Name = "tsmi_Delete";
            this.tsmi_Delete.Size = new System.Drawing.Size(141, 22);
            this.tsmi_Delete.Text = "删除(&D)";
            this.tsmi_Delete.Click += new System.EventHandler(this.tsmi_Delete_Click);
            // 
            // tsmi_Hospital
            // 
            this.tsmi_Hospital.Name = "tsmi_Hospital";
            this.tsmi_Hospital.Size = new System.Drawing.Size(141, 22);
            this.tsmi_Hospital.Text = "平台管理(&H)";
            this.tsmi_Hospital.Click += new System.EventHandler(this.tsmi_Hospital_Click);
            // 
            // Form_DevServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gb_Main);
            this.Name = "Form_DevServer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "开发服务器";
            this.Load += new System.EventHandler(this.Form_DevServer_Load);
            this.gb_Main.ResumeLayout(false);
            this.contextMenuStrip_ListView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Main;
        private System.Windows.Forms.ListView listView_Dev;
        private System.Windows.Forms.ColumnHeader DevName;
        private System.Windows.Forms.ColumnHeader IP;
        private System.Windows.Forms.ColumnHeader UserName;
        private System.Windows.Forms.ColumnHeader Password;
        private System.Windows.Forms.ColumnHeader SuPassword;
        private System.Windows.Forms.ColumnHeader Port;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ListView;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Insert;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Edit;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Delete;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Hospital;
    }
}