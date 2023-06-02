namespace JianLian.HDIS.PublishHelper
{
    partial class Form_Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.menuStrip_Main = new System.Windows.Forms.MenuStrip();
            this.tsmi_Manager = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_DevServer = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_PulishServer = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Tools = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Pack = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_UpgradeLog = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_CodeGeneRATOR = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_About = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip_Main = new System.Windows.Forms.StatusStrip();
            this.tssl_Oprate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_Split2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspb_Upload = new System.Windows.Forms.ToolStripProgressBar();
            this.tssl_Upload = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_Fill = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_ServerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_Split3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_Dev = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_Split1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssl_Publish = new System.Windows.Forms.ToolStripStatusLabel();
            this.gb_Server = new System.Windows.Forms.GroupBox();
            this.treeView_Main = new System.Windows.Forms.TreeView();
            this.contextMenuStrip_Tree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_HospitalCreate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_RemoveLocker = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_HospitalRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Stop = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Start = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip_ListBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_ClearCache = new System.Windows.Forms.ToolStripMenuItem();
            this.gb_Docker = new System.Windows.Forms.GroupBox();
            this.listView_App = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Names = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Image = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Command = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CreatedAt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RunningFor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Ports = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Networks = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip_ListView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_HospitalTester = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Log = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Restart = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Import = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_OpenCode = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_HeidiSQL = new System.Windows.Forms.ToolStripMenuItem();
            this.gb_Log = new System.Windows.Forms.GroupBox();
            this.lb_Logger = new System.Windows.Forms.ListBox();
            this.timer_Main = new System.Windows.Forms.Timer(this.components);
            this.gb_Oprate = new System.Windows.Forms.GroupBox();
            this.cb_andriod = new System.Windows.Forms.CheckBox();
            this.cb_ts = new System.Windows.Forms.CheckBox();
            this.cb_iot = new System.Windows.Forms.CheckBox();
            this.cb_pad = new System.Windows.Forms.CheckBox();
            this.cb_build = new System.Windows.Forms.CheckBox();
            this.cb_remove = new System.Windows.Forms.CheckBox();
            this.btn_Stop = new System.Windows.Forms.Button();
            this.btn_Publish = new System.Windows.Forms.Button();
            this.cb_web = new System.Windows.Forms.CheckBox();
            this.cb_job = new System.Windows.Forms.CheckBox();
            this.cb_rtm = new System.Windows.Forms.CheckBox();
            this.cb_hfs = new System.Windows.Forms.CheckBox();
            this.cb_webapi = new System.Windows.Forms.CheckBox();
            this.menuStrip_Main.SuspendLayout();
            this.statusStrip_Main.SuspendLayout();
            this.gb_Server.SuspendLayout();
            this.contextMenuStrip_Tree.SuspendLayout();
            this.contextMenuStrip_ListBox.SuspendLayout();
            this.gb_Docker.SuspendLayout();
            this.contextMenuStrip_ListView.SuspendLayout();
            this.gb_Log.SuspendLayout();
            this.gb_Oprate.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip_Main
            // 
            this.menuStrip_Main.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.menuStrip_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Manager,
            this.tsmi_Tools,
            this.tsmi_About});
            this.menuStrip_Main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_Main.Name = "menuStrip_Main";
            this.menuStrip_Main.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip_Main.Size = new System.Drawing.Size(1067, 28);
            this.menuStrip_Main.TabIndex = 0;
            this.menuStrip_Main.Text = "menuStrip1";
            // 
            // tsmi_Manager
            // 
            this.tsmi_Manager.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_DevServer,
            this.tsmi_PulishServer});
            this.tsmi_Manager.Name = "tsmi_Manager";
            this.tsmi_Manager.Size = new System.Drawing.Size(76, 24);
            this.tsmi_Manager.Text = "管理(&M)";
            // 
            // tsmi_DevServer
            // 
            this.tsmi_DevServer.Name = "tsmi_DevServer";
            this.tsmi_DevServer.Size = new System.Drawing.Size(174, 24);
            this.tsmi_DevServer.Text = "开发服务器(&D)";
            this.tsmi_DevServer.Click += new System.EventHandler(this.tsmi_DevServer_Click);
            // 
            // tsmi_PulishServer
            // 
            this.tsmi_PulishServer.Name = "tsmi_PulishServer";
            this.tsmi_PulishServer.Size = new System.Drawing.Size(174, 24);
            this.tsmi_PulishServer.Text = "发布服务器(&P)";
            this.tsmi_PulishServer.Click += new System.EventHandler(this.tsmi_PulishServer_Click);
            // 
            // tsmi_Tools
            // 
            this.tsmi_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Pack,
            this.tsmi_UpgradeLog,
            this.tsmi_CodeGeneRATOR});
            this.tsmi_Tools.Name = "tsmi_Tools";
            this.tsmi_Tools.Size = new System.Drawing.Size(70, 24);
            this.tsmi_Tools.Text = "工具(&T)";
            // 
            // tsmi_Pack
            // 
            this.tsmi_Pack.Name = "tsmi_Pack";
            this.tsmi_Pack.Size = new System.Drawing.Size(189, 24);
            this.tsmi_Pack.Text = "版本打包(&R)";
            this.tsmi_Pack.Click += new System.EventHandler(this.tsmi_Pack_Click);
            // 
            // tsmi_UpgradeLog
            // 
            this.tsmi_UpgradeLog.Name = "tsmi_UpgradeLog";
            this.tsmi_UpgradeLog.Size = new System.Drawing.Size(189, 24);
            this.tsmi_UpgradeLog.Text = "升级日志生成(&U)";
            this.tsmi_UpgradeLog.Click += new System.EventHandler(this.tsmi_UpgradeLog_Click);
            // 
            // tsmi_CodeGeneRATOR
            // 
            this.tsmi_CodeGeneRATOR.Name = "tsmi_CodeGeneRATOR";
            this.tsmi_CodeGeneRATOR.Size = new System.Drawing.Size(189, 24);
            this.tsmi_CodeGeneRATOR.Text = "前端代码生成(&G)";
            this.tsmi_CodeGeneRATOR.Click += new System.EventHandler(this.tsmi_CodeGeneRATOR_Click);
            // 
            // tsmi_About
            // 
            this.tsmi_About.Name = "tsmi_About";
            this.tsmi_About.Size = new System.Drawing.Size(72, 24);
            this.tsmi_About.Text = "关于(&A)";
            this.tsmi_About.Click += new System.EventHandler(this.tsmi_About_Click);
            // 
            // statusStrip_Main
            // 
            this.statusStrip_Main.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.statusStrip_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssl_Oprate,
            this.tssl_Split2,
            this.tspb_Upload,
            this.tssl_Upload,
            this.tssl_Fill,
            this.tssl_ServerStatus,
            this.tssl_Split3,
            this.tssl_Dev,
            this.tssl_Split1,
            this.tssl_Publish});
            this.statusStrip_Main.Location = new System.Drawing.Point(0, 606);
            this.statusStrip_Main.Name = "statusStrip_Main";
            this.statusStrip_Main.Size = new System.Drawing.Size(1067, 25);
            this.statusStrip_Main.TabIndex = 1;
            this.statusStrip_Main.Text = "statusStrip1";
            // 
            // tssl_Oprate
            // 
            this.tssl_Oprate.Name = "tssl_Oprate";
            this.tssl_Oprate.Size = new System.Drawing.Size(54, 20);
            this.tssl_Oprate.Text = "Ready";
            // 
            // tssl_Split2
            // 
            this.tssl_Split2.Name = "tssl_Split2";
            this.tssl_Split2.Size = new System.Drawing.Size(13, 21);
            this.tssl_Split2.Text = "|";
            this.tssl_Split2.Visible = false;
            // 
            // tspb_Upload
            // 
            this.tspb_Upload.Name = "tspb_Upload";
            this.tspb_Upload.Size = new System.Drawing.Size(100, 20);
            this.tspb_Upload.Visible = false;
            // 
            // tssl_Upload
            // 
            this.tssl_Upload.Name = "tssl_Upload";
            this.tssl_Upload.Size = new System.Drawing.Size(41, 20);
            this.tssl_Upload.Text = "0 / 0";
            this.tssl_Upload.Visible = false;
            // 
            // tssl_Fill
            // 
            this.tssl_Fill.Name = "tssl_Fill";
            this.tssl_Fill.Size = new System.Drawing.Size(449, 20);
            this.tssl_Fill.Spring = true;
            // 
            // tssl_ServerStatus
            // 
            this.tssl_ServerStatus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tssl_ServerStatus.Name = "tssl_ServerStatus";
            this.tssl_ServerStatus.Size = new System.Drawing.Size(92, 20);
            this.tssl_ServerStatus.Text = "ServerStats";
            // 
            // tssl_Split3
            // 
            this.tssl_Split3.Name = "tssl_Split3";
            this.tssl_Split3.Size = new System.Drawing.Size(13, 20);
            this.tssl_Split3.Text = "|";
            // 
            // tssl_Dev
            // 
            this.tssl_Dev.Name = "tssl_Dev";
            this.tssl_Dev.Size = new System.Drawing.Size(210, 20);
            this.tssl_Dev.Text = "Dev：Server - 2 Hospital - 7";
            // 
            // tssl_Split1
            // 
            this.tssl_Split1.Name = "tssl_Split1";
            this.tssl_Split1.Size = new System.Drawing.Size(13, 20);
            this.tssl_Split1.Text = "|";
            // 
            // tssl_Publish
            // 
            this.tssl_Publish.Name = "tssl_Publish";
            this.tssl_Publish.Size = new System.Drawing.Size(221, 20);
            this.tssl_Publish.Text = "Publish: Server - 1 Version - 5";
            // 
            // gb_Server
            // 
            this.gb_Server.BackColor = System.Drawing.SystemColors.Control;
            this.gb_Server.Controls.Add(this.treeView_Main);
            this.gb_Server.Font = new System.Drawing.Font("宋体", 11F);
            this.gb_Server.Location = new System.Drawing.Point(12, 28);
            this.gb_Server.Name = "gb_Server";
            this.gb_Server.Size = new System.Drawing.Size(276, 416);
            this.gb_Server.TabIndex = 2;
            this.gb_Server.TabStop = false;
            this.gb_Server.Text = "Server";
            // 
            // treeView_Main
            // 
            this.treeView_Main.ContextMenuStrip = this.contextMenuStrip_Tree;
            this.treeView_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Main.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeView_Main.ForeColor = System.Drawing.SystemColors.WindowText;
            this.treeView_Main.HideSelection = false;
            this.treeView_Main.Location = new System.Drawing.Point(3, 20);
            this.treeView_Main.Name = "treeView_Main";
            this.treeView_Main.Size = new System.Drawing.Size(270, 393);
            this.treeView_Main.TabIndex = 0;
            this.treeView_Main.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_Main_DrawNode);
            this.treeView_Main.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Main_AfterSelect);
            // 
            // contextMenuStrip_Tree
            // 
            this.contextMenuStrip_Tree.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.contextMenuStrip_Tree.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_Tree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_HospitalCreate,
            this.tsmi_RemoveLocker,
            this.tsmi_HospitalRemove,
            this.tsmi_Stop,
            this.tsmi_Start});
            this.contextMenuStrip_Tree.Name = "contextMenuStrip_Tree";
            this.contextMenuStrip_Tree.Size = new System.Drawing.Size(188, 124);
            this.contextMenuStrip_Tree.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Tree_Opening);
            // 
            // tsmi_HospitalCreate
            // 
            this.tsmi_HospitalCreate.Name = "tsmi_HospitalCreate";
            this.tsmi_HospitalCreate.Size = new System.Drawing.Size(187, 24);
            this.tsmi_HospitalCreate.Text = "新建医院(&C)";
            this.tsmi_HospitalCreate.Click += new System.EventHandler(this.tsmi_HospitalCreate_Click);
            // 
            // tsmi_RemoveLocker
            // 
            this.tsmi_RemoveLocker.Name = "tsmi_RemoveLocker";
            this.tsmi_RemoveLocker.Size = new System.Drawing.Size(187, 24);
            this.tsmi_RemoveLocker.Text = "解除锁定(&L)";
            this.tsmi_RemoveLocker.Click += new System.EventHandler(this.tsmi_RemoveLocker_Click);
            // 
            // tsmi_HospitalRemove
            // 
            this.tsmi_HospitalRemove.Name = "tsmi_HospitalRemove";
            this.tsmi_HospitalRemove.Size = new System.Drawing.Size(187, 24);
            this.tsmi_HospitalRemove.Text = "移除医院(&Q)";
            this.tsmi_HospitalRemove.Click += new System.EventHandler(this.tsmi_HospitalRemove_Click);
            // 
            // tsmi_Stop
            // 
            this.tsmi_Stop.Name = "tsmi_Stop";
            this.tsmi_Stop.Size = new System.Drawing.Size(187, 24);
            this.tsmi_Stop.Text = "停止所有容器(&T)";
            this.tsmi_Stop.Click += new System.EventHandler(this.tsmi_Stop_Click);
            // 
            // tsmi_Start
            // 
            this.tsmi_Start.Name = "tsmi_Start";
            this.tsmi_Start.Size = new System.Drawing.Size(187, 24);
            this.tsmi_Start.Text = "启动所有容器(&S)";
            this.tsmi_Start.Click += new System.EventHandler(this.tsmi_Start_Click);
            // 
            // contextMenuStrip_ListBox
            // 
            this.contextMenuStrip_ListBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.contextMenuStrip_ListBox.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_ListBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_Copy,
            this.tsmi_Clear,
            this.tsmi_OpenFile,
            this.tsmi_ClearCache});
            this.contextMenuStrip_ListBox.Name = "contextMenuStripListBox";
            this.contextMenuStrip_ListBox.Size = new System.Drawing.Size(188, 100);
            // 
            // tsmi_Copy
            // 
            this.tsmi_Copy.Name = "tsmi_Copy";
            this.tsmi_Copy.Size = new System.Drawing.Size(187, 24);
            this.tsmi_Copy.Text = "复制(&C)";
            this.tsmi_Copy.Click += new System.EventHandler(this.tsmi_Copy_Click);
            // 
            // tsmi_Clear
            // 
            this.tsmi_Clear.Name = "tsmi_Clear";
            this.tsmi_Clear.Size = new System.Drawing.Size(187, 24);
            this.tsmi_Clear.Text = "清空(&Q)";
            this.tsmi_Clear.Click += new System.EventHandler(this.tsmi_Clear_Click);
            // 
            // tsmi_OpenFile
            // 
            this.tsmi_OpenFile.Name = "tsmi_OpenFile";
            this.tsmi_OpenFile.Size = new System.Drawing.Size(187, 24);
            this.tsmi_OpenFile.Text = "定位日志文件(&F)";
            this.tsmi_OpenFile.Click += new System.EventHandler(this.tsmi_OpenFile_Click);
            // 
            // tsmi_ClearCache
            // 
            this.tsmi_ClearCache.Name = "tsmi_ClearCache";
            this.tsmi_ClearCache.Size = new System.Drawing.Size(187, 24);
            this.tsmi_ClearCache.Text = "清除缓存配置(&P)";
            this.tsmi_ClearCache.Click += new System.EventHandler(this.tsmi_ClearCache_Click);
            // 
            // gb_Docker
            // 
            this.gb_Docker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Docker.BackColor = System.Drawing.SystemColors.Control;
            this.gb_Docker.Controls.Add(this.listView_App);
            this.gb_Docker.Font = new System.Drawing.Font("宋体", 11F);
            this.gb_Docker.ForeColor = System.Drawing.Color.IndianRed;
            this.gb_Docker.Location = new System.Drawing.Point(294, 123);
            this.gb_Docker.Name = "gb_Docker";
            this.gb_Docker.Size = new System.Drawing.Size(761, 318);
            this.gb_Docker.TabIndex = 3;
            this.gb_Docker.TabStop = false;
            this.gb_Docker.Text = "Docker";
            // 
            // listView_App
            // 
            this.listView_App.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.Names,
            this.Image,
            this.Command,
            this.CreatedAt,
            this.RunningFor,
            this.Ports,
            this.Status,
            this.Networks});
            this.listView_App.ContextMenuStrip = this.contextMenuStrip_ListView;
            this.listView_App.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_App.FullRowSelect = true;
            this.listView_App.GridLines = true;
            this.listView_App.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_App.HideSelection = false;
            this.listView_App.Location = new System.Drawing.Point(3, 20);
            this.listView_App.MultiSelect = false;
            this.listView_App.Name = "listView_App";
            this.listView_App.Size = new System.Drawing.Size(755, 295);
            this.listView_App.TabIndex = 0;
            this.listView_App.UseCompatibleStateImageBehavior = false;
            this.listView_App.View = System.Windows.Forms.View.Details;
            this.listView_App.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView_App_MouseDoubleClick);
            // 
            // ID
            // 
            this.ID.Text = "容器ID";
            this.ID.Width = 120;
            // 
            // Names
            // 
            this.Names.Text = "容器名称";
            this.Names.Width = 200;
            // 
            // Image
            // 
            this.Image.Text = "镜像";
            this.Image.Width = 200;
            // 
            // Command
            // 
            this.Command.Text = "执行的命令";
            this.Command.Width = 120;
            // 
            // CreatedAt
            // 
            this.CreatedAt.Text = "容器创建时间";
            this.CreatedAt.Width = 200;
            // 
            // RunningFor
            // 
            this.RunningFor.Text = "运行时长";
            this.RunningFor.Width = 200;
            // 
            // Ports
            // 
            this.Ports.Text = "暴露的端口";
            this.Ports.Width = 200;
            // 
            // Status
            // 
            this.Status.Text = "容器状态";
            this.Status.Width = 120;
            // 
            // Networks
            // 
            this.Networks.Text = "网络";
            this.Networks.Width = 120;
            // 
            // contextMenuStrip_ListView
            // 
            this.contextMenuStrip_ListView.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.contextMenuStrip_ListView.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_ListView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_HospitalTester,
            this.tsmi_Log,
            this.tsmi_Restart,
            this.tsmi_Import,
            this.tsmi_OpenCode,
            this.tsmi_HeidiSQL});
            this.contextMenuStrip_ListView.Name = "contextMenuStrip_ListView";
            this.contextMenuStrip_ListView.Size = new System.Drawing.Size(236, 148);
            // 
            // tsmi_HospitalTester
            // 
            this.tsmi_HospitalTester.Name = "tsmi_HospitalTester";
            this.tsmi_HospitalTester.Size = new System.Drawing.Size(235, 24);
            this.tsmi_HospitalTester.Text = "前端测试(&W)";
            this.tsmi_HospitalTester.Click += new System.EventHandler(this.tsmi_HospitalTester_Click);
            // 
            // tsmi_Log
            // 
            this.tsmi_Log.Name = "tsmi_Log";
            this.tsmi_Log.Size = new System.Drawing.Size(235, 24);
            this.tsmi_Log.Text = "查看当前日志(&L)";
            this.tsmi_Log.Click += new System.EventHandler(this.tsmi_Log_Click);
            // 
            // tsmi_Restart
            // 
            this.tsmi_Restart.Name = "tsmi_Restart";
            this.tsmi_Restart.Size = new System.Drawing.Size(235, 24);
            this.tsmi_Restart.Text = "重启当前容器(&R)";
            this.tsmi_Restart.Click += new System.EventHandler(this.tsmi_Restart_Click);
            // 
            // tsmi_Import
            // 
            this.tsmi_Import.Name = "tsmi_Import";
            this.tsmi_Import.Size = new System.Drawing.Size(235, 24);
            this.tsmi_Import.Text = "导入数据库文件(&I)";
            this.tsmi_Import.Click += new System.EventHandler(this.tsmi_Import_Click);
            // 
            // tsmi_OpenCode
            // 
            this.tsmi_OpenCode.Name = "tsmi_OpenCode";
            this.tsmi_OpenCode.Size = new System.Drawing.Size(235, 24);
            this.tsmi_OpenCode.Text = "打开后端代码位置(&O)";
            this.tsmi_OpenCode.Click += new System.EventHandler(this.tsmi_OpenCode_Click);
            // 
            // tsmi_HeidiSQL
            // 
            this.tsmi_HeidiSQL.Name = "tsmi_HeidiSQL";
            this.tsmi_HeidiSQL.Size = new System.Drawing.Size(235, 24);
            this.tsmi_HeidiSQL.Text = "打开数据库管理工具(&H)";
            this.tsmi_HeidiSQL.Click += new System.EventHandler(this.tsmi_HeidiSQL_Click);
            // 
            // gb_Log
            // 
            this.gb_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Log.BackColor = System.Drawing.SystemColors.Control;
            this.gb_Log.Controls.Add(this.lb_Logger);
            this.gb_Log.Font = new System.Drawing.Font("宋体", 11F);
            this.gb_Log.Location = new System.Drawing.Point(15, 450);
            this.gb_Log.Name = "gb_Log";
            this.gb_Log.Size = new System.Drawing.Size(1040, 156);
            this.gb_Log.TabIndex = 4;
            this.gb_Log.TabStop = false;
            this.gb_Log.Text = "Logger";
            // 
            // lb_Logger
            // 
            this.lb_Logger.ContextMenuStrip = this.contextMenuStrip_ListBox;
            this.lb_Logger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_Logger.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lb_Logger.FormattingEnabled = true;
            this.lb_Logger.ItemHeight = 12;
            this.lb_Logger.Location = new System.Drawing.Point(3, 20);
            this.lb_Logger.Name = "lb_Logger";
            this.lb_Logger.Size = new System.Drawing.Size(1034, 133);
            this.lb_Logger.TabIndex = 0;
            this.lb_Logger.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lb_Logger_DrawItem);
            // 
            // timer_Main
            // 
            this.timer_Main.Tick += new System.EventHandler(this.timer_Main_Tick);
            // 
            // gb_Oprate
            // 
            this.gb_Oprate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Oprate.BackColor = System.Drawing.SystemColors.Control;
            this.gb_Oprate.Controls.Add(this.cb_andriod);
            this.gb_Oprate.Controls.Add(this.cb_ts);
            this.gb_Oprate.Controls.Add(this.cb_iot);
            this.gb_Oprate.Controls.Add(this.cb_pad);
            this.gb_Oprate.Controls.Add(this.cb_build);
            this.gb_Oprate.Controls.Add(this.cb_remove);
            this.gb_Oprate.Controls.Add(this.btn_Stop);
            this.gb_Oprate.Controls.Add(this.btn_Publish);
            this.gb_Oprate.Controls.Add(this.cb_web);
            this.gb_Oprate.Controls.Add(this.cb_job);
            this.gb_Oprate.Controls.Add(this.cb_rtm);
            this.gb_Oprate.Controls.Add(this.cb_hfs);
            this.gb_Oprate.Controls.Add(this.cb_webapi);
            this.gb_Oprate.Font = new System.Drawing.Font("宋体", 11F);
            this.gb_Oprate.Location = new System.Drawing.Point(294, 28);
            this.gb_Oprate.Name = "gb_Oprate";
            this.gb_Oprate.Size = new System.Drawing.Size(761, 89);
            this.gb_Oprate.TabIndex = 5;
            this.gb_Oprate.TabStop = false;
            this.gb_Oprate.Text = "Oprate";
            // 
            // cb_andriod
            // 
            this.cb_andriod.AutoSize = true;
            this.cb_andriod.Location = new System.Drawing.Point(597, 25);
            this.cb_andriod.Name = "cb_andriod";
            this.cb_andriod.Size = new System.Drawing.Size(82, 19);
            this.cb_andriod.TabIndex = 24;
            this.cb_andriod.Text = "andriod";
            this.cb_andriod.UseVisualStyleBackColor = true;
            this.cb_andriod.Visible = false;
            // 
            // cb_ts
            // 
            this.cb_ts.AutoSize = true;
            this.cb_ts.Location = new System.Drawing.Point(398, 25);
            this.cb_ts.Name = "cb_ts";
            this.cb_ts.Size = new System.Drawing.Size(42, 19);
            this.cb_ts.TabIndex = 23;
            this.cb_ts.Text = "ts";
            this.cb_ts.UseVisualStyleBackColor = true;
            this.cb_ts.Visible = false;
            // 
            // cb_iot
            // 
            this.cb_iot.AutoSize = true;
            this.cb_iot.Location = new System.Drawing.Point(329, 25);
            this.cb_iot.Name = "cb_iot";
            this.cb_iot.Size = new System.Drawing.Size(50, 19);
            this.cb_iot.TabIndex = 22;
            this.cb_iot.Text = "iot";
            this.cb_iot.UseVisualStyleBackColor = true;
            this.cb_iot.Visible = false;
            // 
            // cb_pad
            // 
            this.cb_pad.AutoSize = true;
            this.cb_pad.Location = new System.Drawing.Point(528, 25);
            this.cb_pad.Name = "cb_pad";
            this.cb_pad.Size = new System.Drawing.Size(50, 19);
            this.cb_pad.TabIndex = 21;
            this.cb_pad.Text = "pad";
            this.cb_pad.UseVisualStyleBackColor = true;
            this.cb_pad.Visible = false;
            // 
            // cb_build
            // 
            this.cb_build.AutoSize = true;
            this.cb_build.Checked = true;
            this.cb_build.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_build.Location = new System.Drawing.Point(29, 57);
            this.cb_build.Name = "cb_build";
            this.cb_build.Size = new System.Drawing.Size(101, 19);
            this.cb_build.TabIndex = 20;
            this.cb_build.Text = "发布前编译";
            this.cb_build.UseVisualStyleBackColor = true;
            // 
            // cb_remove
            // 
            this.cb_remove.AutoSize = true;
            this.cb_remove.Location = new System.Drawing.Point(191, 57);
            this.cb_remove.Name = "cb_remove";
            this.cb_remove.Size = new System.Drawing.Size(176, 19);
            this.cb_remove.TabIndex = 19;
            this.cb_remove.Text = "发布前清空远程文件夹";
            this.cb_remove.UseVisualStyleBackColor = true;
            this.cb_remove.Visible = false;
            // 
            // btn_Stop
            // 
            this.btn_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Stop.Location = new System.Drawing.Point(579, 52);
            this.btn_Stop.Name = "btn_Stop";
            this.btn_Stop.Size = new System.Drawing.Size(85, 24);
            this.btn_Stop.TabIndex = 18;
            this.btn_Stop.Text = "停止(&S)";
            this.btn_Stop.UseVisualStyleBackColor = true;
            this.btn_Stop.Click += new System.EventHandler(this.btn_Stop_Click);
            // 
            // btn_Publish
            // 
            this.btn_Publish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Publish.Location = new System.Drawing.Point(670, 52);
            this.btn_Publish.Name = "btn_Publish";
            this.btn_Publish.Size = new System.Drawing.Size(85, 24);
            this.btn_Publish.TabIndex = 17;
            this.btn_Publish.Text = "发布(&P)";
            this.btn_Publish.UseVisualStyleBackColor = true;
            this.btn_Publish.Click += new System.EventHandler(this.btn_Publish_Click);
            // 
            // cb_web
            // 
            this.cb_web.AutoSize = true;
            this.cb_web.Location = new System.Drawing.Point(122, 25);
            this.cb_web.Name = "cb_web";
            this.cb_web.Size = new System.Drawing.Size(50, 19);
            this.cb_web.TabIndex = 15;
            this.cb_web.Text = "web";
            this.cb_web.UseVisualStyleBackColor = true;
            // 
            // cb_job
            // 
            this.cb_job.AutoSize = true;
            this.cb_job.Location = new System.Drawing.Point(191, 25);
            this.cb_job.Name = "cb_job";
            this.cb_job.Size = new System.Drawing.Size(50, 19);
            this.cb_job.TabIndex = 14;
            this.cb_job.Text = "job";
            this.cb_job.UseVisualStyleBackColor = true;
            this.cb_job.Visible = false;
            // 
            // cb_rtm
            // 
            this.cb_rtm.AutoSize = true;
            this.cb_rtm.Location = new System.Drawing.Point(260, 25);
            this.cb_rtm.Name = "cb_rtm";
            this.cb_rtm.Size = new System.Drawing.Size(50, 19);
            this.cb_rtm.TabIndex = 13;
            this.cb_rtm.Text = "rtm";
            this.cb_rtm.UseVisualStyleBackColor = true;
            this.cb_rtm.Visible = false;
            // 
            // cb_hfs
            // 
            this.cb_hfs.AutoSize = true;
            this.cb_hfs.Location = new System.Drawing.Point(455, 25);
            this.cb_hfs.Name = "cb_hfs";
            this.cb_hfs.Size = new System.Drawing.Size(50, 19);
            this.cb_hfs.TabIndex = 12;
            this.cb_hfs.Text = "hfs";
            this.cb_hfs.UseVisualStyleBackColor = true;
            this.cb_hfs.Visible = false;
            // 
            // cb_webapi
            // 
            this.cb_webapi.AutoSize = true;
            this.cb_webapi.Location = new System.Drawing.Point(29, 25);
            this.cb_webapi.Name = "cb_webapi";
            this.cb_webapi.Size = new System.Drawing.Size(74, 19);
            this.cb_webapi.TabIndex = 11;
            this.cb_webapi.Text = "webapi";
            this.cb_webapi.UseVisualStyleBackColor = true;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1067, 631);
            this.Controls.Add(this.gb_Oprate);
            this.Controls.Add(this.gb_Log);
            this.Controls.Add(this.gb_Docker);
            this.Controls.Add(this.gb_Server);
            this.Controls.Add(this.statusStrip_Main);
            this.Controls.Add(this.menuStrip_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip_Main;
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "优质云企业发布";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Main_FormClosing);
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.menuStrip_Main.ResumeLayout(false);
            this.menuStrip_Main.PerformLayout();
            this.statusStrip_Main.ResumeLayout(false);
            this.statusStrip_Main.PerformLayout();
            this.gb_Server.ResumeLayout(false);
            this.contextMenuStrip_Tree.ResumeLayout(false);
            this.contextMenuStrip_ListBox.ResumeLayout(false);
            this.gb_Docker.ResumeLayout(false);
            this.contextMenuStrip_ListView.ResumeLayout(false);
            this.gb_Log.ResumeLayout(false);
            this.gb_Oprate.ResumeLayout(false);
            this.gb_Oprate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip_Main;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Manager;
        private System.Windows.Forms.ToolStripMenuItem tsmi_DevServer;
        private System.Windows.Forms.ToolStripMenuItem tsmi_PulishServer;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Tools;
        private System.Windows.Forms.StatusStrip statusStrip_Main;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Publish;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Split1;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Dev;
        private System.Windows.Forms.GroupBox gb_Server;
        private System.Windows.Forms.GroupBox gb_Docker;
        private System.Windows.Forms.GroupBox gb_Log;
        private System.Windows.Forms.TreeView treeView_Main;
        private System.Windows.Forms.ListBox lb_Logger;
        private System.Windows.Forms.ToolStripMenuItem tsmi_About;
        private System.Windows.Forms.Timer timer_Main;
        private System.Windows.Forms.ListView listView_App;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader Image;
        private System.Windows.Forms.ColumnHeader Command;
        private System.Windows.Forms.ColumnHeader CreatedAt;
        private System.Windows.Forms.ColumnHeader RunningFor;
        private System.Windows.Forms.ColumnHeader Ports;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.ColumnHeader Names;
        private System.Windows.Forms.ColumnHeader Networks;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Fill;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Oprate;
        private System.Windows.Forms.GroupBox gb_Oprate;
        private System.Windows.Forms.CheckBox cb_build;
        private System.Windows.Forms.CheckBox cb_remove;
        private System.Windows.Forms.Button btn_Stop;
        private System.Windows.Forms.Button btn_Publish;
        private System.Windows.Forms.CheckBox cb_web;
        private System.Windows.Forms.CheckBox cb_job;
        private System.Windows.Forms.CheckBox cb_rtm;
        private System.Windows.Forms.CheckBox cb_hfs;
        private System.Windows.Forms.CheckBox cb_webapi;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ListBox;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Copy;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Clear;
        private System.Windows.Forms.ToolStripMenuItem tsmi_ClearCache;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_ListView;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Log;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Restart;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_Tree;
        private System.Windows.Forms.ToolStripMenuItem tsmi_HospitalCreate;
        private System.Windows.Forms.ToolStripMenuItem tsmi_HospitalRemove;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Split2;
        private System.Windows.Forms.ToolStripProgressBar tspb_Upload;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Upload;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Pack;
        private System.Windows.Forms.ToolStripMenuItem tsmi_UpgradeLog;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Import;
        private System.Windows.Forms.ToolStripMenuItem tsmi_HeidiSQL;
        private System.Windows.Forms.ToolStripMenuItem tsmi_HospitalTester;
        private System.Windows.Forms.ToolStripStatusLabel tssl_ServerStatus;
        private System.Windows.Forms.ToolStripStatusLabel tssl_Split3;
        private System.Windows.Forms.ToolStripMenuItem tsmi_OpenFile;
        private System.Windows.Forms.ToolStripMenuItem tsmi_RemoveLocker;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Stop;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Start;
        private System.Windows.Forms.ToolStripMenuItem tsmi_CodeGeneRATOR;
        private System.Windows.Forms.CheckBox cb_pad;
        private System.Windows.Forms.CheckBox cb_ts;
        private System.Windows.Forms.CheckBox cb_iot;
        private System.Windows.Forms.CheckBox cb_andriod;
        private System.Windows.Forms.ToolStripMenuItem tsmi_OpenCode;
    }
}

