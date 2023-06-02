namespace JianLian.HDIS.PublishHelper
{
    partial class Form_VersionFile
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
            this.listView_VersionFile = new System.Windows.Forms.ListView();
            this.FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FileSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CreateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_ListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_Download = new System.Windows.Forms.ToolStripMenuItem();
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
            this.gb_Main.Controls.Add(this.listView_VersionFile);
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
            this.ll_Sync.Text = "同步数据";
            this.ll_Sync.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll_Sync_LinkClicked);
            // 
            // listView_VersionFile
            // 
            this.listView_VersionFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.FileName,
            this.FileSize,
            this.CreateTime});
            this.listView_VersionFile.ContextMenuStrip = this.contextMenuStrip_ListView;
            this.listView_VersionFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_VersionFile.FullRowSelect = true;
            this.listView_VersionFile.GridLines = true;
            this.listView_VersionFile.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_VersionFile.HideSelection = false;
            this.listView_VersionFile.Location = new System.Drawing.Point(3, 17);
            this.listView_VersionFile.MultiSelect = false;
            this.listView_VersionFile.Name = "listView_VersionFile";
            this.listView_VersionFile.Size = new System.Drawing.Size(770, 406);
            this.listView_VersionFile.TabIndex = 0;
            this.listView_VersionFile.UseCompatibleStateImageBehavior = false;
            this.listView_VersionFile.View = System.Windows.Forms.View.Details;
            // 
            // FileName
            // 
            this.FileName.Text = "文件名称";
            this.FileName.Width = 200;
            // 
            // FileSize
            // 
            this.FileSize.Text = "文件大小";
            this.FileSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.FileSize.Width = 150;
            // 
            // CreateTime
            // 
            this.CreateTime.Text = "创建时间";
            this.CreateTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CreateTime.Width = 200;
            // 
            // contextMenuStrip_ListView
            // 
            this.contextMenuStrip_ListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Download,
            this.tsmi_Delete});
            this.contextMenuStrip_ListView.Name = "contextMenuStrip_Main";
            this.contextMenuStrip_ListView.Size = new System.Drawing.Size(118, 48);
            // 
            // tsmi_Download
            // 
            this.tsmi_Download.Name = "tsmi_Download";
            this.tsmi_Download.Size = new System.Drawing.Size(117, 22);
            this.tsmi_Download.Text = "下载(&D)";
            this.tsmi_Download.Click += new System.EventHandler(this.tsmi_Download_Click);
            // 
            // tsmi_Delete
            // 
            this.tsmi_Delete.Name = "tsmi_Delete";
            this.tsmi_Delete.Size = new System.Drawing.Size(117, 22);
            this.tsmi_Delete.Text = "删除(&R)";
            this.tsmi_Delete.Click += new System.EventHandler(this.tsmi_Delete_Click);
            // 
            // Form_VersionFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gb_Main);
            this.Name = "Form_VersionFile";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "版本文件管理";
            this.Load += new System.EventHandler(this.Form_VersionFile_Load);
            this.gb_Main.ResumeLayout(false);
            this.gb_Main.PerformLayout();
            this.contextMenuStrip_ListView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_Main;
        private System.Windows.Forms.ListView listView_VersionFile;
        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.ColumnHeader CreateTime;
        private System.Windows.Forms.ColumnHeader FileSize;
        private System.Windows.Forms.LinkLabel ll_Sync;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ListView;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Delete;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Download;
    }
}