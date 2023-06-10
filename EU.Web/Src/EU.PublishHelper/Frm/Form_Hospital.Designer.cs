namespace JianLian.HDIS.PublishHelper
{
    partial class Form_Hospital
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
            this.ll_Sync = new System.Windows.Forms.LinkLabel();
            this.listView_Hospital = new System.Windows.Forms.ListView();
            this.HospitalName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DefaultPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SCPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.WWWPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_ListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Edit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.gb_Main.SuspendLayout();
            this.contextMenuStrip_ListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_Main
            // 
            this.gb_Main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Main.Controls.Add(this.ll_Sync);
            this.gb_Main.Controls.Add(this.listView_Hospital);
            this.gb_Main.Location = new System.Drawing.Point(12, 12);
            this.gb_Main.Name = "gb_Main";
            this.gb_Main.Size = new System.Drawing.Size(776, 426);
            this.gb_Main.TabIndex = 1;
            this.gb_Main.TabStop = false;
            // 
            // ll_Sync
            // 
            this.ll_Sync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ll_Sync.AutoSize = true;
            this.ll_Sync.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.ll_Sync.Location = new System.Drawing.Point(707, 2);
            this.ll_Sync.Name = "ll_Sync";
            this.ll_Sync.Size = new System.Drawing.Size(53, 12);
            this.ll_Sync.TabIndex = 1;
            this.ll_Sync.TabStop = true;
            this.ll_Sync.Text = "同步项目";
            this.ll_Sync.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_Sync_LinkClicked);
            // 
            // listView_Hospital
            // 
            this.listView_Hospital.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.HospitalName,
            this.FileName,
            this.DefaultPort,
            this.SCPath,
            this.WWWPath});
            this.listView_Hospital.ContextMenuStrip = this.contextMenuStrip_ListView;
            this.listView_Hospital.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Hospital.FullRowSelect = true;
            this.listView_Hospital.GridLines = true;
            this.listView_Hospital.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_Hospital.HideSelection = false;
            this.listView_Hospital.Location = new System.Drawing.Point(3, 17);
            this.listView_Hospital.MultiSelect = false;
            this.listView_Hospital.Name = "listView_Hospital";
            this.listView_Hospital.Size = new System.Drawing.Size(770, 406);
            this.listView_Hospital.TabIndex = 0;
            this.listView_Hospital.UseCompatibleStateImageBehavior = false;
            this.listView_Hospital.View = System.Windows.Forms.View.Details;
            this.listView_Hospital.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_Hospital_MouseDoubleClick);
            // 
            // HospitalName
            // 
            this.HospitalName.Text = "项目名称";
            this.HospitalName.Width = 175;
            // 
            // FileName
            // 
            this.FileName.Text = "文件夹名称";
            this.FileName.Width = 87;
            // 
            // DefaultPort
            // 
            this.DefaultPort.Text = "默认端口(前三位)";
            this.DefaultPort.Width = 120;
            // 
            // SCPath
            // 
            this.SCPath.Text = "本地后端代码路径";
            this.SCPath.Width = 205;
            // 
            // WWWPath
            // 
            this.WWWPath.Text = "本地前端代码路径";
            this.WWWPath.Width = 176;
            // 
            // contextMenuStrip_ListView
            // 
            this.contextMenuStrip_ListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Insert,
            this.tsmi_Edit,
            this.tsmi_Delete});
            this.contextMenuStrip_ListView.Name = "contextMenuStrip_Main";
            this.contextMenuStrip_ListView.Size = new System.Drawing.Size(119, 70);
            // 
            // tsmi_Insert
            // 
            this.tsmi_Insert.Name = "tsmi_Insert";
            this.tsmi_Insert.Size = new System.Drawing.Size(118, 22);
            this.tsmi_Insert.Text = "新增(&N)";
            this.tsmi_Insert.Click += new System.EventHandler(this.tsmi_Insert_Click);
            // 
            // tsmi_Edit
            // 
            this.tsmi_Edit.Name = "tsmi_Edit";
            this.tsmi_Edit.Size = new System.Drawing.Size(118, 22);
            this.tsmi_Edit.Text = "编辑(&E)";
            this.tsmi_Edit.Click += new System.EventHandler(this.tsmi_Edit_Click);
            // 
            // tsmi_Delete
            // 
            this.tsmi_Delete.Name = "tsmi_Delete";
            this.tsmi_Delete.Size = new System.Drawing.Size(118, 22);
            this.tsmi_Delete.Text = "删除(&D)";
            this.tsmi_Delete.Click += new System.EventHandler(this.tsmi_Delete_Click);
            // 
            // Form_Hospital
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gb_Main);
            this.Name = "Form_Hospital";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "项目配置";
            this.Load += new System.EventHandler(this.Form_Hospital_Load);
            this.gb_Main.ResumeLayout(false);
            this.gb_Main.PerformLayout();
            this.contextMenuStrip_ListView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Main;
        private System.Windows.Forms.ListView listView_Hospital;
        private System.Windows.Forms.ColumnHeader HospitalName;
        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.ColumnHeader DefaultPort;
        private System.Windows.Forms.ColumnHeader SCPath;
        private System.Windows.Forms.ColumnHeader WWWPath;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ListView;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Insert;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Edit;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Delete;
        private System.Windows.Forms.LinkLabel ll_Sync;
    }
}