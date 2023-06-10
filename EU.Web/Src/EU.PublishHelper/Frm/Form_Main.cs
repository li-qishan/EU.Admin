using JianLian.HDIS.PublishHelper.Frm;
using JianLian.HDIS.PublishHelper.Src;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace JianLian.HDIS.PublishHelper
{
    public partial class Form_Main : Form
    {
        #region 初始化
        public Form_Main()
        {
            InitializeComponent();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            Utility.Init();
            Init();
        }
        Thread t_RefreshServer;
        /// <summary>
        /// 重要：规约要求开发环境必装dev目录下，运维测试必装root目录下
        /// </summary>
        private string SelectEnv = string.Empty;
        /// <summary>
        /// 冗余多开发环境docker地址后缀
        /// </summary>
        private string SelectCmdSuffix = string.Empty;

        private void Init()
        {
            lb_Logger.ItemHeight = 16;
            treeView_Main.ItemHeight = 20;
            Utility.SendLogHandle += SendLog;
            Utility.SetStatus += SetStatus;
            Utility.SetProgressBarValue += SetProgressBar;
            Utility.SetProgressBarVisible += SetProgressBarVisible;
            tssl_ServerStatus.Visible = false;
            tssl_Split3.Visible = false;
            RefreshTree();
            cb_webapi.Checked = Utility.m_OprateLog.Webapi;
            cb_hfs.Checked = Utility.m_OprateLog.Hfs;
            cb_job.Checked = Utility.m_OprateLog.Job;
            cb_rtm.Checked = Utility.m_OprateLog.Rtm;
            cb_remove.Checked = Utility.m_OprateLog.Remove;
            cb_build.Checked = Utility.m_OprateLog.Build;
            cb_web.Checked = Utility.m_OprateLog.Web;
            cb_pad.Checked = Utility.m_OprateLog.Pad;
            timer_Main.Start();
            if (!string.IsNullOrEmpty(Utility.m_OprateLog.ServerName) && !string.IsNullOrEmpty(Utility.m_OprateLog.HospitalName))
            {
                foreach (TreeNode node in treeView_Main.Nodes)
                {
                    if (node.Name == Utility.m_OprateLog.ServerName)
                    {
                        foreach (TreeNode nodeItem in node.Nodes)
                        {
                            if (nodeItem.Name == Utility.m_OprateLog.HospitalName)
                            {
                                treeView_Main.SelectedNode = nodeItem;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            t_RefreshServer = new Thread(new ThreadStart(delegate
            {
                RefreshServer();
            }));
            t_RefreshServer.Start();
            //SetFont(this);
        }

        Font defaultFont = new Font("宋体", 11, FontStyle.Regular);
        void SetFont(Control control)
        {
            control.Font = defaultFont;
            if (control.HasChildren)
            {
                foreach (Control item in control.Controls)
                {
                    if (item is GroupBox)
                        continue;
                    SetFont(item);
                }
            }
        }

        //private double m_CpuMax = 0;
        private double m_MemMax = 0;
        private double m_DiskMax = 0;
        private void RefreshServer()
        {
            while (!b_Closed)
            {
                if (DateTime.Now.Second % 5 == 0)
                {
                    try
                    {
                        BeginInvoke(new EventHandler(delegate
                        {
                            (bool Success, double Cpu, double MemTotal, double MemUsed, double Disk) res = (false, 0.0, 0.0, 0.0, 0.0);
                            DevServer server = null;
                            var node = treeView_Main.SelectedNode;
                            if (node != null)
                            {
                                if (node.Tag.ToString() != "hospital")
                                {
                                    server = Utility.m_DevServers.Where(o => o.Name == node.Name).FirstOrDefault();
                                }
                                else
                                {
                                    server = Utility.m_DevServers.Where(o => o.Name == node.Parent.Name).FirstOrDefault();
                                }

                                if (server != null)
                                {
                                    res = Utility.GetServerStatus(server);
                                }
                            }
                            Color color = System.Drawing.SystemColors.ControlText;
                            if (server != null && res.Success)
                            {
                                var mused = Math.Round(100.0 * res.MemUsed / res.MemTotal, 2);
                                tssl_ServerStatus.Visible = true;
                                tssl_Split3.Visible = true;
                                tssl_ServerStatus.Text = $"{server.Ip}(已用): Cpu {res.Cpu}% Mem {mused}% Disk {res.Disk}%";
                                //if (res.Cpu > 90 && res.Cpu < 100 && m_CpuMax != res.Cpu)
                                //{
                                //    m_CpuMax = res.Cpu;
                                //    Utility.SendLog($"{server.Ip} Cpu使用率过高 {res.Cpu}%");
                                //}
                                if (mused > 90 && m_MemMax != mused)
                                {
                                    m_MemMax = mused;
                                    //Utility.SendLog($"{server.Ip} 物理内存使用率过高 {mused}%");
                                    color = Color.Red;
                                }
                                if (res.Disk > 90 && m_DiskMax != res.Disk)
                                {
                                    m_DiskMax = res.Disk;
                                    //Utility.SendLog($"{server.Ip} 磁盘使用率过高 {res.Disk}%");
                                    color = Color.Red;
                                }
                                tssl_ServerStatus.ForeColor = color;
                            }
                            else
                            {
                                tssl_ServerStatus.ForeColor = color;
                                tssl_ServerStatus.Visible = false;
                                tssl_Split3.Visible = false;
                            }
                        }));
                    }
                    catch { }
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

        bool b_Closed = false;
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (b_Closed)
                return;

            if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.None)
            {
                if (MessageBox.Show("确定关闭发布工具？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    b_Closed = true;
                    ExitSystem();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                b_Closed = true;
                ExitSystem();
            }
        }

        private void ExitSystem()
        {
            try
            {
                var node = treeView_Main.SelectedNode;
                if (node != null)
                {
                    if (node.Tag.ToString() == "hospital")
                    {
                        Utility.m_OprateLog.ServerName = Utility.m_DevServers.Where(o => o.Name == node.Parent?.Name).FirstOrDefault()?.Name;
                        Utility.m_OprateLog.HospitalName = Utility.m_DevServers.Where(o => o.Name == node.Parent?.Name).FirstOrDefault()?.Hospitals?.Where(o => o.Name == node.Name).FirstOrDefault()?.Name;
                    }
                }
                timer_Main.Stop();
                t_RefreshServer.Abort();
                Utility.m_OprateLog.Webapi = cb_webapi.Checked;
                Utility.m_OprateLog.Hfs = cb_hfs.Checked;
                Utility.m_OprateLog.Job = cb_job.Checked;
                Utility.m_OprateLog.Rtm = cb_rtm.Checked;
                Utility.m_OprateLog.Remove = cb_remove.Checked;
                Utility.m_OprateLog.Build = cb_build.Checked;
                Utility.m_OprateLog.Web = cb_web.Checked;
                Utility.m_OprateLog.Pad = cb_pad.Checked;
                Utility.SaveOprateLog();
                //Environment.Exit(0);
            }
            catch { }
        }

        private void RefreshTree()
        {
            tssl_Dev.Text = $"Dev：Server - {Utility.m_DevServers.Count} Hospital - {Utility.m_DevServers.Sum(o => o.Hospitals.Count)}";
            tssl_Publish.Text = $"Pulish：Server - {Utility.m_PublishServers.Count} File - {Utility.m_PublishServers.Sum(o => o.Folders.Sum(f => f.Files.Count))}";

            treeView_Main.Nodes.Clear();
            Utility.m_DevServers.ForEach(server =>
            {
                TreeNode tnServer = new TreeNode()
                {
                    Tag = "server",
                    Name = server.Name,
                    Text = $"{server.Name}"
                };
                server?.Hospitals?.ForEach(hospital =>
                {
                    tnServer.Nodes.Add(new TreeNode()
                    {
                        Tag = "hospital",
                        Name = hospital.Name,
                        Text = $"{hospital.Name}({hospital.DefaultFort})"
                    });
                });
                treeView_Main.Nodes.Add(tnServer);
            });
            treeView_Main.ExpandAll();
        }
        #endregion

        #region Tree操作
        private bool HasSelectdServer(out DevServer server)
        {
            server = null;

            var node = treeView_Main.SelectedNode;
            if (node is null || node.Tag.ToString() != "server")
                return false;

            server = Utility.m_DevServers.Where(o => o.Name == node.Name).FirstOrDefault();
            if (server is null)
                return false;

            return true;
        }
        private bool HasSelectdHospital(out DevServer server, out Hospital hospital)
        {
            server = null;
            hospital = null;

            var node = treeView_Main.SelectedNode;
            if (node is null || node.Tag.ToString() != "hospital")
                return false;

            server = Utility.m_DevServers.Where(o => o.Name == node.Parent?.Name).FirstOrDefault();
            if (server is null)
                return false;

            hospital = server?.Hospitals?.Where(o => o.Name == node.Name).FirstOrDefault();
            if (hospital is null)
                return false;

            return true;
        }

        private void contextMenuStrip_Tree_Opening(object sender, CancelEventArgs e)
        {
            if (treeView_Main.SelectedNode == null)
                return;
            //选中服务器
            if (treeView_Main.SelectedNode.Tag.ToString() == "server")
            {
                var server = Utility.m_DevServers.Where(o => o.Name == treeView_Main.SelectedNode.Name).FirstOrDefault();
                if (server?.Ip == Utility.m_TestServerIp)
                {
                    foreach (ToolStripItem item in contextMenuStrip_Tree.Items)
                    {
                        item.Visible = false;
                    }
                }
                else
                {
                    foreach (ToolStripItem item in contextMenuStrip_Tree.Items)
                    {
                        if (item.Name == "tsmi_RemoveLocker" || item.Name == "tsmi_HospitalCreate")
                        {
                            item.Visible = true;
                        }
                        else
                        {
                            item.Visible = false;
                        }
                    }
                }
            }
            else
            {
                //选中医院
                var server = Utility.m_DevServers.Where(o => o.Name == treeView_Main.SelectedNode.Parent.Name).FirstOrDefault();
                if (server?.Ip == Utility.m_TestServerIp)
                {
                    foreach (ToolStripItem item in contextMenuStrip_Tree.Items)
                    {
                        item.Visible = false;
                    }
                }
                else
                {
                    foreach (ToolStripItem item in contextMenuStrip_Tree.Items)
                    {
                        if (item.Name == "tsmi_RemoveLocker")
                        {
                            item.Visible = false;
                        }
                        else
                        {
                            item.Visible = true;
                        }
                    }
                }
            }
        }


        private void treeView_Main_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Thread t = new Thread(new ThreadStart(delegate
            {
                RefreshDocker();
            }));
            t.Start();
        }

        private void RefreshDocker(TreeNode node = null)
        {
            try
            {
                BeginInvoke(new EventHandler(delegate
                {
                    if (node == null)
                        node = treeView_Main.SelectedNode;

                    if (node?.Tag.ToString() != "hospital")
                    {
                        return;
                    }

                    var server = Utility.m_DevServers.Where(o => o.Name == node.Parent?.Name).FirstOrDefault();
                    var hospital = server?.Hospitals?.Where(o => o.Name == node.Name).FirstOrDefault();
                    if (server is null || hospital is null)
                        return;
                    //获取docker信息
                    var flag = $"{server.Name} - {hospital.Name} - {hospital.DefaultFort}";
                    this.gb_Docker.Text = flag;
                    var (Success, Result) = SshHelper.ExcuteCmd(server, DockerCommand.GetDockerStatus(hospital.FileName));
                    listView_App.Items.Clear();
                    if (Success)
                    {
                        if (!string.IsNullOrEmpty(Result))
                        {
                            string[] empties = Result.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < empties.Length; ++i)
                            {
                                string[] items = empties[i].Split(new string[] { "||" }, StringSplitOptions.None);
                                ListViewItem item = new ListViewItem();
                                item.SubItems[0].Text = items[0];
                                for (int j = 1; j < items.Length; ++j)
                                    item.SubItems.Add(items[j]);
                                listView_App.Items.Add(item);
                            }
                            Utility.SendLog(flag, $"刷新成功");

                            SelectEnv = hospital.DefaultFort == "600" ? "dev" : "root";
                            SelectCmdSuffix = SelectEnv == "dev" ? "/compose" : string.Empty;
                        }
                    }
                    else
                    {
                        if (SftpHelper.Exists(server, $"/home/{server.UserName}/ihdis/compose/{hospital.FileName}"))
                        {
                            if (string.IsNullOrEmpty(Result))
                            {
                                Utility.SendLog(flag, "项目已经停止运行");
                            }
                            else
                            {
                                Utility.SendLog(flag, Result);
                            }
                        }
                        else
                        {
                            Utility.SendLog(flag, $"医院不存在");
                        }
                    }
                }));
            }
            catch { }

        }

        private void tsmi_HospitalCreate_Click(object sender, EventArgs e)
        {
            var node = treeView_Main.SelectedNode;
            if (node is null)
                return;

            DevServer server = null;
            Hospital hospital = null;

            //服务器操作
            if (node.Tag.ToString() == "server")
            {
                server = Utility.m_DevServers.Where(o => o.Name == node.Name).FirstOrDefault();
                using (Form_Hospital_Item f = new Form_Hospital_Item(server.Name, "新增"))
                {
                    f.ShowDialog();
                    hospital = f.m_Hoipital;
                }
                if (!(hospital is null))
                {
                    Utility.SaveDevServer();
                    RefreshTree();
                }
            }
            else
            {

                //医院操作
                server = Utility.m_DevServers.Where(o => o.Name == node.Parent?.Name).FirstOrDefault();
                hospital = server?.Hospitals?.Where(o => o.Name == node.Name).FirstOrDefault();
                if (server is null || hospital is null)
                    return;
                if (SftpHelper.Exists(server, $"/home/{server.UserName}/ihdis/compose/{hospital.FileName}"))
                {
                    MessageBox.Show("医院已经存在，不允许重复创建", "提示");
                    return;
                }
                if (SshHelper.InRtmNow())
                {
                    MessageBox.Show("正在执行其他命令，请等待！", "提示");
                    return;
                }

                Thread t = new Thread(new ThreadStart(delegate
                {
                    SshHelper.ExcuteCmdRtm(server, $"cd /home/{server.UserName}/ihdis;sudo bash create-hospital.sh {hospital.FileName} {hospital.DefaultFort} {GetMask(server)};");
                    RefreshDocker();
                }));
                t.Start();
            }
        }
        private string GetMask(DevServer server)
        {
            var res = SshHelper.ExcuteCmd(server, "sudo ip route");
            if (res.Success)
            {
                List<int> lst = new List<int>();
                res.Result
                    .Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Where(o => o.StartsWith("172."))
                    .ToList()
                    .ForEach(s =>
                    {
                        lst.Add(Convert.ToInt32(s.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[2]));
                    });
                while (true)
                {
                    var r = new Random().Next(1, 255);
                    if (!lst.Contains(r))
                    {
                        return r.ToString();
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            else
            {
                return new Random().Next(1, 255).ToString();
            }
        }

        private void tsmi_HospitalRemove_Click(object sender, EventArgs e)
        {
            if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
            {
                return;
            }
            if (!SftpHelper.Exists(server, $"/home/{server.UserName}/ihdis/compose/{hospital.FileName}"))
            {
                MessageBox.Show("医院不存在", "提示");
                return;
            }

            if (MessageBox.Show("是否确定删除医院？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (SshHelper.InRtmNow())
            {
                MessageBox.Show("正在执行其他命令，请等待！", "提示");
                return;
            }

            Thread t = new Thread(new ThreadStart(delegate
            {
                SshHelper.ExcuteCmdRtm(server, $"cd /home/{server.UserName}/ihdis;sudo bash remove-hospital.sh {hospital.FileName};");
                RefreshDocker();
            }));
            t.Start();
        }

        private void tsmi_RemoveLocker_Click(object sender, EventArgs e)
        {
            if (!HasSelectdServer(out DevServer server))
                return;
            Utility.RemoveDevFiles(server);
        }

        private void tsmi_Stop_Click(object sender, EventArgs e)
        {
            if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
            {
                return;
            }
            SetStatus("停止docker-compose...");
            SshHelper.ExcuteCmd(server, DockerCommand.DockerComposeDown(server.UserName, hospital.FileName, SelectCmdSuffix));
            RefreshDocker(treeView_Main.SelectedNode);
            SetStatus("执行完毕...");
        }

        private void tsmi_Start_Click(object sender, EventArgs e)
        {
            if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
            {
                return;
            }
            SetStatus("启动docker-compose...");
            SshHelper.ExcuteCmd(server, DockerCommand.DockerComposeUp(server.UserName, hospital.FileName, SelectCmdSuffix));
            RefreshDocker(treeView_Main.SelectedNode);
            SetStatus("执行完毕...");
        }

        private void treeView_Main_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.State == TreeNodeStates.Selected)//状态判定
            {
                // 背景色（原本的背景色，蓝色）
                Color bgColor = Color.FromArgb(0, 120, 215);
                Brush brush = new SolidBrush(bgColor);
                e.Graphics.FillRectangle(brush, new Rectangle(e.Node.Bounds.Left, e.Node.Bounds.Top, e.Node.Bounds.Width, e.Node.Bounds.Height));//背景色为蓝色
                                                                                                                                                 // 绘制文本为蓝底白字
                TextRenderer.DrawText(e.Graphics,
                                      e.Node.Text,
                                      e.Node.TreeView.Font,
                                      new Rectangle(e.Node.Bounds.Left + 1, e.Node.Bounds.Top, e.Node.Bounds.Width, e.Node.Bounds.Height),
                                      Color.White);
            }
            else
            {
                e.DrawDefault = true;
            }
        }
        #endregion

        #region 开发服务器配置
        private void tsmi_DevServer_Click(object sender, EventArgs e)
        {
            using (Form_DevServer f = new Form_DevServer())
            {
                f.ShowDialog();
            }
            RefreshTree();
        }
        #endregion

        #region 发布服务器配置
        private void tsmi_PulishServer_Click(object sender, EventArgs e)
        {
            using (Form_PublishServer f = new Form_PublishServer())
            {
                f.ShowDialog();
            }
            RefreshTree();
        }
        #endregion

        #region ListView操作

        private void tsmi_OpenCode_Click(object sender, EventArgs e)
        {
            if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
            {
                return;
            }
            if (string.IsNullOrEmpty(hospital.SCPath))
                return;

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", "/select," + hospital.SCPath);
            }
            catch { }
        }

        private void tsmi_HospitalRestart_Click(object sender, EventArgs e)
        {
            if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
            {
                return;
            }
            SetStatus("重启docker-compose...");
            SshHelper.ExcuteCmd(server, DockerCommand.DockerComposeRestart(server.UserName, hospital.FileName, SelectCmdSuffix));
            RefreshDocker(treeView_Main.SelectedNode);
            SetStatus("执行完毕...");
        }

        private void tsmi_HospitalTester_Click(object sender, EventArgs e)
        {
            if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
            {
                return;
            }
            System.Diagnostics.Process.Start($"http://{server.Ip}:{hospital.DefaultFort}00");
        }
        private void tsmi_Import_Click(object sender, EventArgs e)
        {
            if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
            {
                return;
            }
            if (server.Ip == Utility.m_TestServerIp)
            {
                return;
            }
            if (!SftpHelper.Exists(server, $"/home/{server.UserName}/ihdis/compose/{hospital.FileName}"))
            {
                MessageBox.Show("医院不存在", "提示");
                return;
            }
            if (MessageBox.Show($"是否确定导入数据至[{server.Ip}]-[{hospital.FileName}]，该操作将清空现有的IHDIS数据库？", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            string fname = string.Empty;
            using (Form_ImportDb f = new Form_ImportDb())
            {
                f.ShowDialog();
                fname = f.m_FileName;
                if (f.m_DialogResult != DialogResult.OK)
                    return;
            }
            Thread t = new Thread(new ThreadStart(delegate
            {
                ImportSql(server, hospital, fname);
            }));
            t.Start();
        }

        object m_InImportSql = new object();
        bool b_InImportSql = false;
        private void ImportSql(DevServer server, Hospital hospital, string fname)
        {
            lock (m_InImportSql)
            {
                if (b_InImportSql)
                {
                    return;
                }

                b_InImportSql = true;
            }
            string remoteFile = $"/tmp/{Guid.NewGuid():N}.sql";
            try
            {
                SetProgressBarVisible(true);
                //step 1 上传文件
                Utility.SendLog("开始上传文件");
                SftpHelper.UploadFile(server, fname, remoteFile, SetProgressBar);

                //step 2 解析文件
                string db = "ihdis";
                int tables = 0;
                bool b_use = false;
                using (StreamReader sr = new StreamReader(fname))
                {
                    var line = sr.ReadLine();
                    while (line != null)
                    {
                        if (!b_use && line.Contains("USE `ihdis`;"))
                        {
                            b_use = true;
                            db = "mysql";
                        }

                        if (line.Trim().StartsWith("CREATE TABLE"))
                        {
                            tables++;
                        }
                        line = sr.ReadLine();
                    }
                }

                //step 3  执行导入
                Utility.SendLog($"开始导入sql文件,共有表 {tables}");
                SshHelper.ExcuteCmd(server, $"cd /home/{server.UserName}/ihdis/compose/{hospital.FileName};sudo docker-compose exec -T mysql mysql -uroot -hlocalhost -pjlmed#123 -e \"DROP DATABASE IF EXISTS ihdis;\";", true);
                SshHelper.ExcuteCmd(server, $"cd /home/{server.UserName}/ihdis/compose/{hospital.FileName};sudo docker-compose exec -T mysql mysql -uroot -hlocalhost -pjlmed#123 -e \"CREATE DATABASE IF NOT EXISTS ihdis;\";", true);
                Thread t = new Thread(new ThreadStart(delegate
                {
                    QueryTables(server, hospital, tables);
                }));
                t.Start();
                SshHelper.ExcuteCmd(server, $"cd /home/{server.UserName}/ihdis/compose/{hospital.FileName};sudo docker-compose exec -T mysql mysql -uroot -hlocalhost -pjlmed#123 {db} < {remoteFile}", true);
                Utility.SendLog("导入sql文件完毕");
                SshHelper.ExcuteCmd(server, DockerCommand.DockerComposeContainerRestart(server.UserName, hospital.FileName, "webapi", SelectCmdSuffix));
            }
            catch (Exception ex)
            {
                Utility.SendLog($"导入sql失败：{ex}");
            }
            finally
            {
                SetProgressBarVisible(false);
                Utility.SendLog($"删除垃圾文件 {remoteFile}");
                SftpHelper.Delete(server, remoteFile);
                lock (m_InImportSql)
                {
                    b_InImportSql = false;
                }
            }
        }

        private void QueryTables(DevServer server, Hospital hospital, int sum)
        {
            string last = string.Empty;
            string sql = $"SELECT TABLE_NAME,TABLE_ROWS FROM information_schema.TABLES WHERE TABLE_SCHEMA='ihdis' order by CREATE_TIME DESC,TABLE_NAME DESC;";
            string connStr = $"server = {server.Ip}; port = {hospital.DefaultFort}90; userid = root; password = jlmed#123;database=ihdis;";
            while (true)
            {
                lock (m_InImportSql)
                {
                    if (!b_InImportSql)
                    {
                        break;
                    }
                }
                try
                {
                    //查询数据库表
                    DataTable dt = MySqlHelper.ExecuteDataset(connStr, sql).Tables[0];
                    SetProgressBar(sum, dt.Rows.Count);
                    string cur = $"{dt.Rows[0]["TABLE_NAME"]} {dt.Rows[0]["TABLE_ROWS"]} ...";
                    if (last != cur)
                    {
                        Utility.SendLog($"[{dt.Rows.Count}/{sum}] {cur}");
                        last = cur;
                    }
                }
                catch { }

                System.Threading.Thread.Sleep(2000);
            }
        }

        private void listView_App_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            QueryLog();
        }

        private void QueryLog()
        {
            try
            {
                if (listView_App.SelectedItems.Count <= 0)
                    return;
                var items = listView_App.SelectedItems[0].SubItems[1].Text.Split('_');
                if (items.Length < 2)
                    return;
                if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
                {
                    return;
                }
                using (Form_Logger f = new Form_Logger(server, hospital, items[1]))
                {
                    f.ShowDialog();
                }
            }
            catch { }
        }
        private void tsmi_Log_Click(object sender, EventArgs e)
        {
            QueryLog();
        }

        private void tsmi_Restart_Click(object sender, EventArgs e)
        {
            if (listView_App.SelectedItems.Count <= 0)
                return;
            if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
            {
                return;
            }
            SetStatus("重启容器中...");
            SshHelper.ExcuteCmd(server, DockerCommand.DockeContainerRestart(listView_App.SelectedItems[0].Text));
            RefreshDocker(treeView_Main.SelectedNode);
            SetStatus("执行完毕...");
        }
        #endregion

        #region 日志操作
        private static List<string> m_LogsTemp = new List<string>();
        private void timer_Main_Tick(object sender, EventArgs e)
        {
            try
            {
                lock (m_Logs)
                {
                    m_LogsTemp.Clear();
                    m_Logs.ForEach(log => m_LogsTemp.Add(log));
                    m_Logs.Clear();
                }

                if (m_LogsTemp.Count > 0)
                {
                    if (lb_Logger.Items.Count > 2000)
                        lb_Logger.Items.Clear();
                    m_LogsTemp.ForEach(log => lb_Logger.Items.Add(log));
                    lb_Logger.TopIndex = lb_Logger.Items.Count - 1;
                }
            }
            catch { }
        }

        private static List<string> m_Logs = new List<string>();
        private void SendLog(string oprator, string msg)
        {
            lock (m_Logs)
            {
                if (string.IsNullOrEmpty(oprator))
                    m_Logs.Add($"{DateTime.Now:HH:mm:ss} {msg}");
                else
                    m_Logs.Add($"{DateTime.Now:HH:mm:ss} [{oprator}] {msg}");
            }
        }

        private void tsmi_Copy_Click(object sender, EventArgs e)
        {
            if (lb_Logger.SelectedIndex <= 0)
                return;

            try
            {
                Clipboard.SetDataObject(lb_Logger.Items[lb_Logger.SelectedIndex].ToString(), true);
                //MessageBox.Show("已经拷贝至系统剪贴板，请执行CTRL+V粘贴！", "提示");
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void tsmi_Clear_Click(object sender, EventArgs e)
        {
            lb_Logger.Items.Clear();
        }
        private void tsmi_ClearCache_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
                    .ToList()
                    .ForEach(f =>
                    {
                        if (f.EndsWith(".dat"))
                            File.Delete(f);
                    });
            }
            catch { }
        }

        private void tsmi_OpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                string fname = AppDomain.CurrentDomain.BaseDirectory + "Log\\" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                System.Diagnostics.Process.Start("explorer.exe", "/select," + fname);
            }
            catch { }
        }

        private void lb_Logger_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();//绘制背景 
            Brush myBrush = Brushes.White;
            if (e.Index >= 0)
            {
                var item = lb_Logger.Items[e.Index].ToString();
                if (item.Contains("失败") || item.Contains("异常") || item.Contains("错误") || item.Contains("ERR!") || item.Contains("ERROR") || item.Contains("error") || item.Contains("fatal") || item.Contains("failed"))
                {
                    if (!item.Contains("  0 个错误"))
                        myBrush = Brushes.Red;
                }
                else if (item.Contains("完成") || item.Contains("success") || item.Contains("complete") || item.Contains("Already up"))
                {
                    myBrush = Brushes.Green;
                }
                e.Graphics.FillRectangle(myBrush, e.Bounds);
                e.DrawFocusRectangle();//焦点框 
                //文本 
                e.Graphics.DrawString(lb_Logger.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            }
        }
        #endregion

        #region 状态变更
        private void SetStatus(string str)
        {
            try
            {
                BeginInvoke(new EventHandler(delegate
                {
                    tssl_Oprate.Text = str;
                }));
            }
            catch { }
        }
        #endregion

        #region 项目发布
        bool m_InPublish = false;
        object m_LockPublish = new object();
        bool m_RemoveFile = false;
        bool m_BuildFisrt = false;
        DevServer m_SelectServer = null;
        Hospital m_SelectHospital = null;
        List<string> m_SelectProjects = new List<string>();
        private void btn_Publish_Click(object sender, EventArgs e)
        {
            lock (m_LockPublish)
            {
                if (m_InPublish)
                    return;
            }

            #region 初始化选择
            var node = treeView_Main.SelectedNode;
            if (node is null || node.Tag.ToString() != "hospital")
            {
                MessageBox.Show("请选择医院！", "提示");
                return;
            }

            m_SelectServer = Utility.m_DevServers.Where(o => o.Name == node.Parent?.Name).FirstOrDefault();
            m_SelectHospital = m_SelectServer?.Hospitals?.Where(o => o.Name == node.Name).FirstOrDefault();
            if (m_SelectServer is null || m_SelectHospital is null)
                return;

            if (string.IsNullOrEmpty(m_SelectHospital.SCPath) || string.IsNullOrEmpty(m_SelectHospital.WWWPath))
            {
                var h = Utility.m_DevServers.Where(o => o.Ip == "192.168.8.72").FirstOrDefault()?.Hospitals?.Where(o => o.FileName == "develop")?.FirstOrDefault();
                if (h != null)
                {
                    m_SelectHospital.SCPath = h?.SCPath;
                    m_SelectHospital.WWWPath = h?.WWWPath;
                    Utility.SendLog("检测到未配置代码路径，默认使用72开发环境配置");
                }
                else
                {
                    MessageBox.Show("请先完善医院信息：前后端目录！", "提示");
                    return;
                }
            }
            m_SelectProjects.Clear();
            bool b_back = false;
            m_RemoveFile = cb_remove.Checked;
            m_BuildFisrt = cb_build.Checked;
            if (cb_hfs.Checked)
            {
                b_back = true;
                m_SelectProjects.Add("hfs");
            }
            if (cb_job.Checked)
            {
                b_back = true;
                m_SelectProjects.Add("job");
            }
            if (cb_iot.Checked)
            {
                b_back = true;
                m_SelectProjects.Add("iot");
            }
            if (cb_ts.Checked)
            {
                b_back = true;
                m_SelectProjects.Add("ts");
            }
            if (cb_rtm.Checked)
            {
                b_back = true;
                m_SelectProjects.Add("rtm");
            }
            if (cb_web.Checked)
            {
                m_SelectProjects.Add("web");
            }
            if (cb_pad.Checked)
            {
                m_SelectProjects.Add("pad");
            }
            if (cb_webapi.Checked)
            {
                b_back = true;
                m_SelectProjects.Add("webapi");
            }
            if (m_SelectHospital.FileName == "produse" && b_back)
            {
                MessageBox.Show("后端代码不允许发布至生产服务器！", "提示");
                return;
            }

            if (m_SelectProjects.Count <= 0)
            {
                return;
            }
            else
            {
                if (MessageBox.Show($"确定发布以下项目？\r\n{string.Join("\r\n", m_SelectProjects)}\r\n至\r\n{m_SelectServer.Ip}\r\n{m_SelectHospital.FileName}({m_SelectHospital.DefaultFort})", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            #endregion

            #region 执行发布
            new Thread(new ThreadStart(() =>
            {
                if (m_SelectHospital.FileName != "produse" && Utility.LockDevFiles(m_SelectServer, $"发布[{m_SelectHospital.FileName}]"))
                {
                    return;
                }
                Publish();
                RefreshDocker(node);
                if (m_SelectHospital.FileName != "produse")
                {
                    Utility.RemoveDevFiles(m_SelectServer);
                }
            })).Start();
            #endregion
        }


        private bool Publish()
        {
            Utility.m_StopPublish = false;
            lock (m_LockPublish)
            {
                if (m_InPublish)
                    return false;
                else
                    m_InPublish = true;
            }
            Utility.SendLog("开始执行发布", true);
            if (m_BuildFisrt && Utility.m_OprateLog.PackGit)
            {
                Utility.SendLog("开始拉取最新代码");
                if (m_SelectProjects.Any(p => p == "webapi")
                        || m_SelectProjects.Any(p => p == "hfs")
                        || m_SelectProjects.Any(p => p == "rtm")
                        || m_SelectProjects.Any(p => p == "job")
                        || m_SelectProjects.Any(p => p == "iot")
                        || m_SelectProjects.Any(p => p == "ts"))
                {
                    CmdHelper.ExecCmd($"cd {m_SelectHospital.SCPath}\r\n"
                    + $"{m_SelectHospital.SCPath.Split(':')[0].Trim()}:\r\n"
                    + $"git pull\r\n"
                    + $"exit\r\n");
                }
                else
                {
                    CmdHelper.ExecCmd($"cd {m_SelectHospital.WWWPath}\r\n"
                    + $"{m_SelectHospital.WWWPath.Split(':')[0].Trim()}:\r\n"
                    + $"git pull\r\n"
                    + $"exit\r\n");
                }
                Utility.SendLog("拉取最新代码完成");
            }
            bool b_suc = true;
            try
            {
                Utility.SetProgressBarVisible(true);
                int index = 0;
                Utility.SetProgressBarValue(m_SelectProjects.Count, (ulong)index);
                m_SelectProjects.ForEach(project =>
                {
                    if (Utility.m_StopPublish)
                        return;
                    if (!b_suc)
                    {
                        return;
                    }
                    try
                    {
                        Utility.SendLog($"{project}", $"开始发布");
                        ++index;

                        #region 是否执行编译
                        b_suc = ProjectBuild(project);
                        if (!b_suc)
                        {
                            return;
                        }
                        else
                        {
                            if (project == "web")
                            {
                                //文件不存在，并且目录也不存在，标识未编译
                                if (!Directory.Exists($"{m_SelectHospital.WWWPath}\\dist"))
                                {
                                    Directory.CreateDirectory($"{m_SelectHospital.WWWPath}\\dist");
                                    Utility.SendLog($"{project}", "检测到文件目录dist不存在，已自动创建，继续执行");
                                }
                            }
                            else if (project == "pad")
                            {
                                //文件不存在，并且目录也不存在，标识未编译
                                if (!Directory.Exists($"{m_SelectHospital.WWWPath}\\pad-h5\\dist"))
                                {
                                    Directory.CreateDirectory($"{m_SelectHospital.WWWPath}\\dist");
                                    Utility.SendLog($"{project}", "检测到文件目录dist不存在，已自动创建，继续执行");
                                }
                            }
                        }
                        #endregion

                        #region 是否删除远程文件
                        ProjectRemove(project);
                        #endregion

                        #region 上传文件
                        b_suc = ProjectUpload(project);
                        if (!b_suc)
                        {
                            return;
                        }
                        #endregion

                        #region 执行pulish行指令
                        ProjectPublish(project);
                        #endregion

                        Utility.SetProgressBarValue(m_SelectProjects.Count, (ulong)index);
                        Utility.SendLog($"{project}", $"发布 {project} 完毕");
                    }
                    catch (Exception ex)
                    {
                        Utility.SendLog($"{project}", $"执行发布失败：{ex}");
                    }
                });

                if (b_suc)
                {
                    //重启容器(只发布web、pad不需要重启)
                    if (m_SelectProjects.Any(p => p == "webapi")
                        || m_SelectProjects.Any(p => p == "hfs")
                        || m_SelectProjects.Any(p => p == "rtm")
                        || m_SelectProjects.Any(p => p == "job")
                        || m_SelectProjects.Any(p => p == "iot")
                        || m_SelectProjects.Any(p => p == "ts"))
                    {

                        m_SelectProjects.Where(p => p != "web" && p != "pad").ToList().ForEach(p =>
                         {
                             Utility.SendLog($"正在重新启动容器 {p}", true);

                             SshHelper.ExcuteCmd(m_SelectServer, DockerCommand.DockerComposeRemove(SelectEnv == "dev"
                                 ? m_SelectServer.UserName : SelectEnv, m_SelectHospital.FileName, p, SelectCmdSuffix));
                             //SshHelper.ExcuteCmd(m_SelectServer, DockerCommand.DockerComposeRemove(m_SelectServer.UserName, m_SelectHospital.FileName, p));
                         });

                        SshHelper.ExcuteCmd(m_SelectServer, DockerCommand.DockerComposeUp(SelectEnv == "dev"
                             ? m_SelectServer.UserName : SelectEnv, m_SelectHospital.FileName, SelectCmdSuffix));
                        //SshHelper.ExcuteCmd(m_SelectServer, DockerCommand.DockerComposeUp(m_SelectServer.UserName, m_SelectHospital.FileName));
                    }
                }
                else
                {
                    Utility.SendLog($"发布", $"发生错误，执行发布失败，停止发布...");
                }
            }
            catch (Exception ex)
            {
                Utility.SendLog($"执行发布失败：{ex}");
                b_suc = false;
            }
            finally
            {
                Utility.SetProgressBarVisible(false);
                lock (m_LockPublish)
                {
                    m_InPublish = false;
                }
            }

            Utility.SendLog("执行发布完毕", true);
            return b_suc;
        }

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="project"></param>
        private bool ProjectBuild(string project)
        {
            if (Utility.m_StopPublish)
                return false;
            if (!m_BuildFisrt)
                return true;

            Utility.SendLog("编译生成", $"[{project}]开始编译生成");
            string cmd;
            switch (project)
            {
                case "webapi":
                    {
                        cmd = $"cd {m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HttpApi.Hosting\\\r\n{m_SelectHospital.SCPath.Split(':')[0].Trim()}:\r\ndotnet build\r\ndotnet publish -c Release -r linux-x64 --self-contained false -o .\\bin\\Release\\net5.0\\publish\r\nexit\r\n";
                        break;
                    }
                case "hfs":
                    {
                        cmd = $"cd {m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HFS\\\r\n{m_SelectHospital.SCPath.Split(':')[0].Trim()}:\r\ndotnet build\r\ndotnet publish -r linux-x64 --self-contained false -o .\\bin\\Release\\net5.0\\publish\r\nexit\r\n";
                        break;
                    }
                case "rtm":
                    {
                        cmd = $"cd {m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.SignalR\\\r\n{m_SelectHospital.SCPath.Split(':')[0].Trim()}:\r\ndotnet build\r\ndotnet publish -r linux-x64 --self-contained false -o .\\bin\\Release\\net5.0\\publish\r\nexit\r\n";
                        break;
                    }
                case "job":
                    {
                        cmd = $"cd {m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.BackgroundJobs\\\r\n{m_SelectHospital.SCPath.Split(':')[0].Trim()}:\r\ndotnet build\r\ndotnet publish -r linux-x64 --self-contained false -o .\\bin\\Release\\net5.0\\publish\r\nexit\r\n";
                        break;
                    }
                case "iot":
                    {
                        cmd = $"cd {m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.IoT\\\r\n{m_SelectHospital.SCPath.Split(':')[0].Trim()}:\r\ndotnet build\r\ndotnet publish -r linux-x64 --self-contained false -o .\\bin\\Release\\net5.0\\publish\r\nexit\r\n";
                        break;
                    }
                case "ts":
                    {
                        var tspath = $"{m_SelectHospital.SCPath.Substring(0, m_SelectHospital.SCPath.LastIndexOf('\\'))}\\adapter\\JianLian.Adapter.Center\\Host\\JianLian.Adapter.Hosting";
                        cmd = $"cd {tspath}\\\r\n{tspath.Split(':')[0].Trim()}:\r\ndotnet build\r\ndotnet publish -r linux-x64 --self-contained false -o .\\bin\\Release\\net5.0\\publish\r\nexit\r\n";
                        break;
                    }
                case "pad":
                    {
                        cmd = $"cd {m_SelectHospital.WWWPath}\\pad-h5\\\r\n{m_SelectHospital.WWWPath.Split(':')[0].Trim()}:\r\nnpm install\r\nnpm run build\r\nexit\r\n";
                        break;
                    }
                case "web":
                    {
                        var path = $"cd {m_SelectHospital.WWWPath}\\\r\n{m_SelectHospital.WWWPath.Split(':')[0].Trim()}:\r\n";

                        cmd = $"{path}npm install -g cnpm --registry=https://registry.npm.taobao.org \r\n cnpm i \r\n npm run build\r\nexit\r\n";
                        break;
                    }
                default:
                    return false;
            }
            var cmdResult = CmdHelper.ExecCmd(cmd);
            if (cmdResult.Item1)
            {
                Utility.SendLog("编译生成", $"[{project}]编译生成发生错误，终止执行");
                Utility.SendLog("编译生成", $"[{project}] {cmdResult.Item2}");
                return false;
            }
            else
            {
                Utility.SendLog("编译生成", $"[{project}]编译生成完毕");
                //等待IO操作
                System.Threading.Thread.Sleep(2000);

                return true;
            }
        }
        /// <summary>
        /// 删除远程文件
        /// </summary>
        /// <param name="project"></param>
        private void ProjectRemove(string project)
        {
            if (Utility.m_StopPublish)
                return;
            if (!m_RemoveFile)
                return;
            if (project != "web" && project != "pad")
            {
                //SshHelper.ExcuteCmd(m_SelectServer, $"sudo rm -rf /home/{m_SelectServer.UserName}/ihdis/dev/app/server/{project}/*", true); 
                if (SelectEnv == "dev")
                {
                    SshHelper.ExcuteCmd(m_SelectServer, $"sudo rm -rf /home/{m_SelectServer.UserName}/ihdis/dev/app/server/{project}/*", true);
                }
                else
                {
                    SshHelper.ExcuteCmd(m_SelectServer, $"sudo rm -rf /home/{SelectEnv}/ihdis/{project}/code/*", true);
                }
            }
            else
            {
                if (SelectEnv == "dev")
                {
                    SshHelper.ExcuteCmd(m_SelectServer, $"sudo rm -rf /home/{m_SelectServer.UserName}/ihdis/{(m_SelectHospital.FileName == "produse" ? "produse" : $"compose/{m_SelectHospital.FileName}")}/app/www/{project}/*", true);
                }
                else
                {
                    SshHelper.ExcuteCmd(m_SelectServer, $"sudo rm -rf /home/{SelectEnv}/ihdis/{m_SelectHospital.FileName}/app/www/{project}/*", true);
                }
            }
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="project"></param>
        private bool ProjectUpload(string project)
        {
            if (Utility.m_StopPublish)
                return false;

            bool b_suc = true;
            SetProgressBarVisible(true);
            try
            {
                Utility.SendLog("文件上传", $"[{project}]开始上传文件");
                List<string> files = new List<string>();

                var toRemotePath = SelectEnv == "dev"
                    ? $"/home/{m_SelectServer.UserName}/ihdis/dev/app/server/{project}"
                    : $"/home/{SelectEnv}/ihdis/{m_SelectHospital.FileName}/code/{project}";
                switch (project)
                {
                    case "webapi":
                        {
                            //更新日志
                            string llogpath = $"{m_SelectHospital.SCPath}\\src\\Document\\Upgrade.md";
                            //string rlogpath = $"/home/{m_SelectServer.UserName}/ihdis/compose/{m_SelectHospital.FileName}/data/hfs/wwwroot/files/upgradelogs.md";
                            var rlogpath = string.Empty;
                            rlogpath = SelectEnv == "dev"
                                ? $"/home/{m_SelectServer.UserName}/ihdis/compose/{m_SelectHospital.FileName}/data/hfs/wwwroot/files/upgradelogs.md"
                                : $"/home/{SelectEnv}/ihdis/{m_SelectHospital.FileName}/data/hfs/wwwroot/files/upgradelogs.md";

                            if (File.Exists(llogpath))
                            {
                                SftpHelper.UploadFile(m_SelectServer, llogpath, rlogpath);
                            }

                            //xml文件
                            string[] fs = Directory.GetFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HttpApi.Hosting\\bin\\Debug\\net5.0", "*.xml");
                            fs.ToList().ForEach(fname =>
                            {
                                FileInfo file = new FileInfo(fname);
                                files.Add(file.Name);
                            });
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HttpApi.Hosting\\bin\\Debug\\net5.0", toRemotePath, files, SetProgressBar);

                            //发布文件
                            files.Clear();
                            GetUploadFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HttpApi.Hosting\\bin\\Release\\net5.0\\publish", $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HttpApi.Hosting\\bin\\Release\\net5.0\\publish", files);
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HttpApi.Hosting\\bin\\Release\\net5.0\\publish", toRemotePath, files, SetProgressBar);
                            break;
                        }
                    case "hfs":
                        {
                            //xml文件
                            string[] fs = Directory.GetFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HFS\\bin\\Debug\\net5.0", "*.xml");
                            fs.ToList().ForEach(fname =>
                            {
                                FileInfo file = new FileInfo(fname);
                                files.Add(file.Name);
                            });
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HFS\\bin\\Debug\\net5.0", toRemotePath, files, SetProgressBar);

                            //发布文件
                            files.Clear();
                            GetUploadFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HFS\\bin\\Release\\net5.0\\publish", $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HFS\\bin\\Release\\net5.0\\publish", files);
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.HFS\\bin\\Release\\net5.0\\publish", toRemotePath, files, SetProgressBar);
                            break;
                        }
                    case "rtm":
                        {
                            //xml文件
                            string[] fs = Directory.GetFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.SignalR\\bin\\Debug\\net5.0", "*.xml");
                            fs.ToList().ForEach(fname =>
                            {
                                FileInfo file = new FileInfo(fname);
                                files.Add(file.Name);
                            });
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.SignalR\\bin\\Debug\\net5.0", toRemotePath, files, SetProgressBar);

                            //发布文件
                            files.Clear();
                            GetUploadFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.SignalR\\bin\\Release\\net5.0\\publish", $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.SignalR\\bin\\Release\\net5.0\\publish", files);
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.SignalR\\bin\\Release\\net5.0\\publish", toRemotePath, files, SetProgressBar);
                            break;
                        }
                    case "job":
                        {
                            //xml文件
                            string[] fs = Directory.GetFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.BackgroundJobs\\bin\\Debug\\net5.0", "*.xml");
                            fs.ToList().ForEach(fname =>
                            {
                                FileInfo file = new FileInfo(fname);
                                files.Add(file.Name);
                            });
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.BackgroundJobs\\bin\\Debug\\net5.0", toRemotePath, files, SetProgressBar);

                            //发布文件
                            files.Clear();
                            GetUploadFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.BackgroundJobs\\bin\\Release\\net5.0\\publish", $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.BackgroundJobs\\bin\\Release\\net5.0\\publish", files);
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.BackgroundJobs\\bin\\Release\\net5.0\\publish", toRemotePath, files, SetProgressBar);
                            break;
                        }
                    case "iot":
                        {
                            //xml文件
                            string[] fs = Directory.GetFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.IoT\\bin\\Debug\\net5.0", "*.xml");
                            fs.ToList().ForEach(fname =>
                            {
                                FileInfo file = new FileInfo(fname);
                                files.Add(file.Name);
                            });
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.IoT\\bin\\Debug\\net5.0", toRemotePath, files, SetProgressBar);

                            //发布文件
                            files.Clear();
                            GetUploadFiles($"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.IoT\\bin\\Release\\net5.0\\publish", $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.IoT\\bin\\Release\\net5.0\\publish", files);
                            SftpHelper.UploadFile(m_SelectServer, $"{m_SelectHospital.SCPath}\\src\\Host\\JianLian.HDIS.IoT\\bin\\Release\\net5.0\\publish", toRemotePath, files, SetProgressBar);
                            break;
                        }
                    case "ts":
                        {
                            var tspath = $"{m_SelectHospital.SCPath.Substring(0, m_SelectHospital.SCPath.LastIndexOf('\\'))}\\adapter\\JianLian.Adapter.Center\\Host\\JianLian.Adapter.Hosting";
                            //xml文件
                            string[] fs = Directory.GetFiles($"{tspath}\\bin\\Debug\\net5.0", "*.xml");
                            fs.ToList().ForEach(fname =>
                            {
                                FileInfo file = new FileInfo(fname);
                                files.Add(file.Name);
                            });
                            SftpHelper.UploadFile(m_SelectServer, $"{tspath}\\bin\\Debug\\net5.0", toRemotePath, files, SetProgressBar);

                            //发布文件
                            files.Clear();
                            GetUploadFiles($"{tspath}\\bin\\Release\\net5.0\\publish", $"{tspath}\\bin\\Release\\net5.0\\publish", files);
                            SftpHelper.UploadFile(m_SelectServer, $"{tspath}\\bin\\Release\\net5.0\\publish", toRemotePath, files, SetProgressBar);
                            break;
                        }
                    case "pad":
                        {
                            var fname = $"{AppDomain.CurrentDomain.BaseDirectory}\\dist.zip";
                            //var rname = $"/home/{m_SelectServer.UserName}/ihdis/{(m_SelectHospital.FileName == "produse" ? "produse" : $"compose/{m_SelectHospital.FileName}")}/app/www/{project}/dist.zip";
                            var rname = SelectEnv == "dev"
                               ? $"/home/{m_SelectServer.UserName}/ihdis/{(m_SelectHospital.FileName == "produse" ? "produse" : $"compose/{m_SelectHospital.FileName}")}/app/www/{project}/dist.zip"
                               : $"/home/{SelectEnv}/ihdis/{m_SelectHospital.FileName}/app/www/{project}/dist.zip";

                            //文件不存在，并且目录也不存在，标识未编译
                            if (!Directory.Exists($"{m_SelectHospital.WWWPath}\\pad-h5\\dist"))
                            {
                                Utility.SendLog($"{project}", "项目未编译，发布失败");
                                return false;
                            }

                            //执行压缩
                            Utility.SendLog($"{project}", "正在执行压缩");
                            ZipHelper.ZipFiles(fname, $"{m_SelectHospital.WWWPath}\\pad-h5\\dist");

                            files.Add(fname);
                            SftpHelper.UploadFile(m_SelectServer, fname, rname, SetProgressBar);
                            break;
                        }
                    case "web":
                        {
                            var fname = $"{AppDomain.CurrentDomain.BaseDirectory}\\dist.zip";
                            //var rname = $"/home/{m_SelectServer.UserName}/ihdis/{(m_SelectHospital.FileName == "produse" ? "produse" : $"compose/{m_SelectHospital.FileName}")}/app/www/{project}/dist.zip";
                            var rname = SelectEnv == "dev"
                             ? $"/home/{m_SelectServer.UserName}/ihdis/{(m_SelectHospital.FileName == "produse" ? "produse" : $"compose/{m_SelectHospital.FileName}")}/app/www/{project}/dist.zip"
                             : $"/home/{SelectEnv}/ihdis/{m_SelectHospital.FileName}/app/www/{project}/dist.zip";

                            //文件不存在，并且目录也不存在，标识未编译
                            if (!Directory.Exists($"{m_SelectHospital.WWWPath}\\dist"))
                            {
                                Utility.SendLog($"{project}", "项目未编译，发布失败");
                                return false;
                            }

                            //执行压缩
                            Utility.SendLog($"{project}", "正在执行压缩");
                            ZipHelper.ZipFiles(fname, $"{m_SelectHospital.WWWPath}\\dist");

                            files.Add(fname);
                            SftpHelper.UploadFile(m_SelectServer, fname, rname, SetProgressBar);
                            break;
                        }
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                b_suc = false;
                throw new Exception(ex.Message);
            }
            finally
            {
                SetProgressBarVisible(false);
            }
            Utility.SendLog("文件上传", $"[{project}]上传文件完毕");
            return b_suc;
        }
        private void SetProgressBarVisible(bool visible)
        {
            try
            {
                BeginInvoke(new EventHandler(delegate
                {
                    tssl_Split2.Visible = visible;
                    tspb_Upload.Visible = visible;
                    tssl_Upload.Visible = visible;
                }));
            }
            catch { }
        }
        private void SetProgressBar(long max, ulong value)
        {
            SetProgressBar((int)max, (int)value);
        }
        private void SetProgressBar(int max, int value)
        {
            try
            {
                BeginInvoke(new EventHandler(delegate
                {
                    tspb_Upload.Minimum = 0;
                    tspb_Upload.Maximum = max;
                    tspb_Upload.Value = value;
                    tssl_Upload.Text = $"{value}/{max}";
                }));
            }
            catch { }
        }
        private void GetUploadFiles(string basePath, string path, List<string> files)
        {
            string[] fs = Directory.GetFiles(path);
            fs.ToList().ForEach(fname =>
            {
                FileInfo file = new FileInfo(fname);
                files.Add(fname.Replace(basePath + "\\", ""));
            });

            string[] ds = Directory.GetDirectories(path);
            ds.ToList().ForEach(spath =>
            {
                if (!spath.EndsWith("Logs"))
                    GetUploadFiles(basePath, spath, files);
            });
        }
        /// <summary>
        /// 执行指令
        /// </summary>
        /// <param name="project"></param>
        private void ProjectPublish(string project)
        {
            if (Utility.m_StopPublish)
                return;
            if (project != "web" && project != "pad")
            {
                //SshHelper.ExcuteCmd(m_SelectServer, $"sudo rm -rf /home/{m_SelectServer.UserName}/ihdis/compose/{m_SelectHospital.FileName}/data/{project}/*;sudo cp -r /home/{m_SelectServer.UserName}/ihdis/dev/app/server/{project}/* /home/{m_SelectServer.UserName}/ihdis/compose/{m_SelectHospital.FileName}/code/{project}");
                //SshHelper.ExcuteCmdRtm(m_SelectServer, $"cd /home/{m_SelectServer.UserName}/ihdis/dev;sudo bash publish-{project}.sh {m_SelectHospital.FileName};");
                //SshHelper.ExcuteCmdRtm(m_SelectServer, $"cd /home/{m_SelectServer.UserName}/ihdis/dev;sudo bash publish-copycode.sh {m_SelectHospital.FileName} {project};");

                if (SelectEnv == "dev")
                {
                    SshHelper.ExcuteCmdRtm(m_SelectServer, $"cd /home/{m_SelectServer.UserName}/ihdis/dev;sudo bash publish-copycode.sh {m_SelectHospital.FileName} {project};");
                }
                else
                {
                    //发布测试环境代码直接发到指定目录了，不需要copy
                }
            }
            else
            {
                if (SelectEnv == "dev")
                {
                    if (m_SelectHospital.FileName == "produse")
                    {
                        SshHelper.ExcuteCmdRtm(m_SelectServer, $"unzip -o -d /home/{m_SelectServer.UserName}/ihdis/produse/app/www/{project} /home/{m_SelectServer.UserName}/ihdis/produse/app/www/{project}/dist.zip");
                    }
                    else
                    {
                        SshHelper.ExcuteCmdRtm(m_SelectServer, $"cd /home/{m_SelectServer.UserName}/ihdis;sudo bash unzip-www.sh {m_SelectHospital.FileName} {project};");
                    }
                }
                else
                {
                    SshHelper.ExcuteCmdRtm(m_SelectServer, $"unzip -o -d /home/{SelectEnv}/ihdis/{m_SelectHospital.FileName}/app/www/{project} /home/{SelectEnv}/ihdis/{m_SelectHospital.FileName}/app/www/{project}/dist.zip");
                }
            }
        }

        private void btn_Stop_Click(object sender, EventArgs e)
        {
            Utility.m_StopPublish = true;
        }
        #endregion

        #region 项目打包
        bool b_InPublishNew = false;
        private void tsmi_Pack_Click(object sender, EventArgs e)
        {
            var node = treeView_Main.SelectedNode;
            var server = Utility.m_DevServers.Where(o => o.Name == node?.Parent?.Name).FirstOrDefault();
            var hospital = server?.Hospitals?.Where(o => o.Name == node?.Name).FirstOrDefault();
            if (b_InPublishNew)
            {
                return;
            }
            b_InPublishNew = true;
            try
            {
                #region 初始化
                using (Form_Publish f = new Form_Publish(server, hospital))
                {
                    f.ShowDialog();
                    if (f.m_DialogResult != DialogResult.Yes)
                    {
                        return;
                    }
                }

                string version = Utility.m_OprateLog.PackVersion;
                DevServer devServer = Utility.m_DevServers.Where(o => o.Ip == Utility.m_OprateLog.PackDevIp).FirstOrDefault();
                hospital = devServer?.Hospitals.Where(o => o.FileName == Utility.m_OprateLog.PackHospital).FirstOrDefault();
                PublishServer publishServer = Utility.m_PublishServers.Where(o => o.Ip == Utility.m_OprateLog.PackPublishIp).FirstOrDefault();
                if (devServer is null)
                {
                    Utility.SendLog("打包", $"开发服务器不存在 {devServer.Ip}");
                    return;
                }
                if (hospital is null)
                {
                    Utility.SendLog("打包", $"医院不存在 {hospital.FileName}");
                    return;
                }
                if (publishServer is null)
                {
                    Utility.SendLog("打包", $"发布服务器不存在 {publishServer.Ip}");
                    return;
                }

                if (string.IsNullOrEmpty(hospital.SCPath) || string.IsNullOrEmpty(hospital.WWWPath))
                {
                    var h = Utility.m_DevServers.Where(o => o.Ip == "192.168.8.72").FirstOrDefault()?.Hospitals?.Where(o => o.FileName == "develop")?.FirstOrDefault();
                    if (h != null)
                    {
                        hospital.SCPath = h?.SCPath;
                        hospital.WWWPath = h?.WWWPath;
                        Utility.SendLog("检测到未配置代码路径，默认使用72开发环境配置");
                    }
                    else
                    {
                        MessageBox.Show("请先完善医院信息：前后端目录！", "提示");
                        return;
                    }
                }
                server = devServer;
                #endregion

                #region 是否先执行发布、或者是升级包
                m_SelectServer = devServer;
                m_SelectHospital = hospital;
                if (m_SelectServer is null || m_SelectHospital is null)
                    return;

                m_SelectProjects.Clear();
                m_RemoveFile = cb_remove.Checked;
                m_BuildFisrt = cb_build.Checked;
                if (cb_hfs.Checked)
                {
                    m_SelectProjects.Add("hfs");
                }
                if (cb_job.Checked)
                {
                    m_SelectProjects.Add("job");
                }
                if (cb_rtm.Checked)
                {
                    m_SelectProjects.Add("rtm");
                }
                if (cb_iot.Checked)
                {
                    m_SelectProjects.Add("iot");
                }
                if (cb_ts.Checked)
                {
                    m_SelectProjects.Add("ts");
                }
                if (cb_web.Checked)
                {
                    if (!Directory.Exists($"{m_SelectHospital.WWWPath}\\node_modules"))
                    {
                        MessageBox.Show("Web项目未执行npm install，直接发布将会报错！");
                        return;
                    }
                    m_SelectProjects.Add("web");
                }
                if (cb_pad.Checked)
                {
                    if (!Directory.Exists($"{m_SelectHospital.WWWPath}\\pad-h5\\node_modules"))
                    {
                        MessageBox.Show("Pad项目未执行npm install，直接发布将会报错！");
                        return;
                    }
                    m_SelectProjects.Add("pad");
                }
                if (cb_webapi.Checked)
                {
                    m_SelectProjects.Add("webapi");
                }
                if (Utility.m_OprateLog.PackUpgrade)
                {
                    if (!m_SelectProjects.Any() && !cb_andriod.Checked)
                    {
                        MessageBox.Show("升级包未选择任何模块，不再执行！");
                        return;
                    }
                }
                #endregion

                #region 指令生成
                List<string> cmdlist = new List<string>();

                //生成基础包文件
                cmdlist.Add($"sudo rm -rf /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}");
                cmdlist.Add($"sudo mkdir -p  /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}");
                cmdlist.Add($"sudo cp -r /home/{publishServer.UserName}/ihdis/packages/sample/*  /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}");

                #region 安装包需要打包镜像
                if (!Utility.m_OprateLog.PackUpgrade)
                {
                    //镜像版本文件的环境变量
                    string env = $"\"IHDIS_PORT=600\n" +
                                $"mask=60\n" +
                                $"redis_TAG=5.0.5\n" +
                                $"mysql_TAG=8.0\n" +
                                $"nginx_TAG=1.17\n" +
                                $"rabbitmq_TAG=3.7.17-management\n" +
                                $"adminer_TAG=4.7.1\n" +
                                $"phpredisadmin_TAG=v1.11.4\n" +
                                $"portainer_TAG=1.22.0\n" +
                                $"webapi_TAG={version}\n" +
                                $"hfs_TAG={version}\n" +
                                $"rtm_TAG={version}\n" +
                                $"iot_TAG={version}\n" +
                                $"ts_TAG={version}\n" +
                                $"job_TAG={version}\"";

                    cmdlist.Add($"sudo echo -e {env} > /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/.env");

                    //APP镜像生成
                    SetAppCmd(hospital, devServer, version, cmdlist, cb_webapi);
                    SetAppCmd(hospital, devServer, version, cmdlist, cb_hfs);
                    SetAppCmd(hospital, devServer, version, cmdlist, cb_rtm);
                    SetAppCmd(hospital, devServer, version, cmdlist, cb_job);
                    SetAppCmd(hospital, devServer, version, cmdlist, cb_iot);
                    SetAppCmd(hospital, devServer, version, cmdlist, cb_ts);

                }
                #endregion

                bool b_ts = Utility.m_OprateLog.PackUpgrade && Utility.m_OprateLog.PackUpgradeTS;
                #region TS插件包
                if (b_ts)
                {
                    //ts升级包，只拷贝ts文件
                    cmdlist.Add($"sudo cp -r /home/{devServer.UserName}/ihdis/dev/app/server/ts /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/code");
                }
                #endregion

                #region 安装包/升级包
                else
                {
                    //后端代码映射
                    cmdlist.Add($"sudo cp -r /home/{devServer.UserName}/ihdis/dev/app/server/* /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/code");

                    //升级包移除ts文件
                    if (Utility.m_OprateLog.PackUpgrade)
                        cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/code/ts/*");

                    //版本升级日志
                    cmdlist.Add($"sudo cp /home/{devServer.UserName}/ihdis/compose/{hospital.FileName}/data/hfs/wwwroot/files/upgradelogs.md /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/hfs/wwwroot/files/upgradelogs.md");
                    cmdlist.Add($"sudo echo {version} > /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/Version");

                    //PC代码
                    var cname = cb_web.Text;
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/app/www/{cname}/*");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/compose/{hospital.FileName}/app/www/{cname}/dist.zip");
                    cmdlist.Add($"sudo cp -r /home/{devServer.UserName}/ihdis/compose/{hospital.FileName}/app/www/{cname}  /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/app/www/");

                    //PAD-H5代码：只有是升级包并且未勾选，才会不打包该模块
                    cname = cb_pad.Text;
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/app/www/{cname}/*");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/compose/{hospital.FileName}/app/www/{cname}/dist.zip");
                    cmdlist.Add($"sudo cp -r /home/{devServer.UserName}/ihdis/compose/{hospital.FileName}/app/www/{cname}  /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/app/www/");

                    //PAD-Andriod文件
                    var cmdStr = CopyPadCmd(devServer, hospital, publishServer, version, $"hdis");
                    if (!string.IsNullOrWhiteSpace(cmdStr))
                        cmdlist.Add(cmdStr);

                    //基础数据库文件(包含TS)：安装包、升级包必有
                    cmdlist.Add($"sudo cp /home/{devServer.UserName}/ihdis/packages/tempdb/{hospital.FileName}/{version}/hdis_table.sql  /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/conf/mysql/");
                    cmdlist.Add($"sudo echo {version} > /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/Version");


                }

                if (Utility.m_OprateLog.PackUpgrade)
                {
                    //升级包删除多余的文件
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/conf/*");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/iot");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/mysql");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/oracle");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/portainer");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/rabbitmq");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/redis");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/ts");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/webapi");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/install/*");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/log/*");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/load_images.cmd");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/load_images.sh");
                    cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/docker-compose.yml");
                }
                #endregion

                //包注释文件
                cmdlist.Add($"sudo cp /tmp/v{version}_log.dat /home/{devServer.UserName}/ihdis/packages/versions/{hospital.FileName}/v{version}_log.dat");

                //压缩文件
                cmdlist.Add($"sudo mkdir /home/{devServer.UserName}/ihdis/packages/versions/{hospital.FileName}/");
                cmdlist.Add($"sudo rm -rf  /home/{devServer.UserName}/ihdis/packages/versions/{hospital.FileName}/{version}.zip");
                cmdlist.Add($"cd /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/ && sudo zip /home/{devServer.UserName}/ihdis/packages/versions/{hospital.FileName}/{version}.zip  -r  ./");
                cmdlist.Add($"sudo chmod 777 /home/{devServer.UserName}/ihdis/packages/versions/version_list");
                cmdlist.Add($"sudo echo {version} >> /home/{devServer.UserName}/ihdis/packages/versions/version_list");

                //清理缓存文件
                cmdlist.Add($"sudo rm -rf /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}");
                cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/packages/tempdb/{hospital.FileName}/*");
                #endregion

                #region 开始执行脚本
                Thread t = new Thread(new ThreadStart(delegate
                {
                    var flag = Utility.m_OprateLog.PackUpgrade ? (Utility.m_OprateLog.PackUpgradeTS ? "[TS插件包]" : "[升级包]") : "[基础包]";
                    if (Utility.LockDevFiles(server, $"{flag}打包[{hospital.FileName}]"))
                    {
                        return;
                    }
                    try
                    {
                        Utility.SetStatus($"{flag}开始打包");

                        #region TS打包

                        if (Utility.m_OprateLog.PackUpgrade && Utility.m_OprateLog.PackUpgradeTS)
                        {
                            //ts升级包，单独处理
                            var tspath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"assistant\JianLian.Assistant\JianLian.HDIS.PublishHelper\bin\Debug\", "") + @"adapter\JianLian.Adapter.Center";

                            Utility.SendLog(flag, $"编译项目");
                            CmdHelper.ExecCmd($"cd {tspath}\\Host\\JianLian.Adapter.Hosting\r\n"
                            + $"{tspath.Split(':')[0].Trim()}:\r\n"
                            + $"dotnet publish  -r linux-x64 --self-contained false -o .\\bin\\Release\\net5.0\\publish\r\n"
                            + $"exit\r\n");

                            //编译TS组件
                            Utility.SendLog(flag, $"编译组件 {hospital}");
                            CmdHelper.ExecCmd($"cd {tspath}\\Custom\\JianLian.Adapter.{Utility.m_OprateLog.PackUpgradeTSCustom}\r\n"
                                + $"{tspath.Split(':')[0].Trim()}:\r\n"
                                + $"dotnet build\r\n"
                                + $"exit\r\n");
                            //清除文件
                            SshHelper.ExcuteCmd(devServer, $"rm -rf /home/{m_SelectServer.UserName}/ihdis/dev/app/server/ts/*", true);
                            //上传文件
                            List<string> files = new List<string>();
                            string[] fs = Directory.GetFiles($"{tspath}\\Host\\JianLian.Adapter.Hosting\\bin\\Debug\\net5.0", "*.xml");
                            fs.ToList().ForEach(fname =>
                            {
                                FileInfo file = new FileInfo(fname);
                                files.Add(file.Name);
                            });
                            SftpHelper.UploadFile(m_SelectServer, $"{tspath}\\Host\\JianLian.Adapter.Hosting\\bin\\Debug\\net5.0", $"/home/{m_SelectServer.UserName}/ihdis/dev/app/server/ts", files, SetProgressBar);

                            files.Clear();
                            GetUploadFiles($"{tspath}\\Host\\JianLian.Adapter.Hosting\\bin\\Release\\net5.0\\publish", $"{tspath}\\Host\\JianLian.Adapter.Hosting\\bin\\Release\\net5.0\\publish", files);
                            SftpHelper.UploadFile(m_SelectServer, $"{tspath}\\Host\\JianLian.Adapter.Hosting\\bin\\Release\\net5.0\\publish", $"/home/{m_SelectServer.UserName}/ihdis/dev/app/server/ts", files, SetProgressBar);

                            files.Clear();
                            GetUploadFiles($"{tspath}\\Custom\\JianLian.Adapter.{Utility.m_OprateLog.PackUpgradeTSCustom}\\bin\\Debug\\net5.0", $"{tspath}\\Custom\\JianLian.Adapter.{Utility.m_OprateLog.PackUpgradeTSCustom}\\bin\\Debug\\net5.0", files);
                            SftpHelper.UploadFile(m_SelectServer, $"{tspath}\\Custom\\JianLian.Adapter.{Utility.m_OprateLog.PackUpgradeTSCustom}\\bin\\Debug\\net5.0", $"/home/{m_SelectServer.UserName}/ihdis/dev/app/server/ts", files, SetProgressBar);

                        }
                        #endregion

                        #region 安装包/升级包
                        else
                        {
                            #region 发布
                            bool b_suc = true;
                            if (Utility.m_OprateLog.PackPublish)
                            {
                                if (m_SelectProjects.Count > 0)
                                {
                                    b_suc = Publish();
                                }
                            }
                            else
                            {
                                Utility.SendLog($"{flag}不需要执行发布流程，默认使用服务端文件");
                            }
                            if (!b_suc)
                            {
                                Utility.SendLog($"{flag}由于发布失败，打包不继续执行");
                                return;
                            }
                            #endregion

                            #region 生成数据库脚本文件
                            var newdb = $"hdis_{version.Replace(".", "_")}";
                            if (Utility.m_OprateLog.PackDbFile)
                            {
                                Utility.SendLog($"{flag}等待程序初始化 10 秒");
                                System.Threading.Thread.Sleep(10000);
                                //需要先设置数据库密码为空，再设置回来
                                //生成最新版本数据库文件
                                SshHelper.ExcuteCmd(devServer, $"cd /home/{devServer.UserName}/ihdis/compose/{hospital.FileName};sudo docker-compose exec -T mysql mysql -uroot -hlocalhost -e \"ALTER USER 'root'@'%' IDENTIFIED WITH mysql_native_password BY ''; flush privileges; \"", true);
                                SshHelper.ExcuteCmd(devServer, $"cd /home/{devServer.UserName}/ihdis/compose/{hospital.FileName};sudo docker-compose exec -T mysql mysql -uroot -hlocalhost -e \"CREATE DATABASE IF NOT EXISTS {newdb};\";", true);
                                SshHelper.ExcuteCmd(devServer, $"cd /home/{devServer.UserName}/ihdis/compose/{hospital.FileName};sudo docker-compose exec -T mysql /bin/bash -c \"mysqldump hdis -uroot  | mysql {newdb} -uroot\"", true);

                                //上传reset.sql并执行
                                string resetsql = File.ReadAllText($"{hospital.SCPath}/src/Document/build/reset.sql", Encoding.UTF8).Replace("use hdis_compare;", $"use {newdb};");
                                var rsfname = Utility.GetTempFileName("reset.sql");
                                File.WriteAllText(rsfname, resetsql, Encoding.UTF8);
                                SftpHelper.UploadFile(devServer, rsfname, "/tmp/reset.sql");
                                SshHelper.ExcuteCmd(devServer, $"cd /home/{devServer.UserName}/ihdis/compose/{hospital.FileName};sudo docker-compose exec -T mysql mysql -uroot -hlocalhost {newdb} < /tmp/reset.sql");

                                SshHelper.ExcuteCmd(devServer, $"cd /home/{devServer.UserName}/ihdis/compose/{hospital.FileName};sudo docker-compose exec -T mysql mysql -uroot -hlocalhost -e \"ALTER USER 'root'@'%' IDENTIFIED WITH mysql_native_password BY 'jlmed#123'; flush privileges; \"", true);
                                //下载数据库
                                DownLoadDb(devServer, hospital, newdb);

                                string allPath = $"{hospital.SCPath}\\src\\Document\\script\\hdis.sql";
                                // 转换编码
                                string allText = File.ReadAllText(allPath);

                                File.WriteAllText(allPath, allText);
                            }
                            else
                            {
                                Utility.SendLog($"{flag}不需要打包最新数据库，使用本地文件 {hospital.SCPath}\\src\\Document\\script\\hdis.sql");
                                Utility.SendLog($"{flag}{hospital.SCPath}\\src\\Document\\script\\hdis.sql");
                                Utility.SendLog($"{flag}{hospital.SCPath}\\src\\Document\\script\\hdis_ts.sql");
                            }

                            //合并ts数据库
                            string ihdis_sql = File.ReadAllText($"{hospital.SCPath}\\src\\Document\\script\\hdis.sql");
                            string ihdis_ts_sql = File.ReadAllText($"{hospital.SCPath}\\src\\Document\\script\\hdis_ts.sql");
                            string sql = $" use `hdis`; \n {ihdis_sql} \n use `hdis_ts`; \n {ihdis_ts_sql}";
                            var sql_fname = Utility.GetTempFileName("hdis_table.sql");
                            File.WriteAllText(sql_fname, $"{sql}");

                            //上传数据库文件
                            SftpHelper.CreateDirectory(publishServer, $"/home/{publishServer.UserName}/ihdis/packages/tempdb/{hospital.FileName}/");
                            SftpHelper.CreateDirectory(publishServer, $"/home/{publishServer.UserName}/ihdis/packages/tempdb/{hospital.FileName}/{version}/");
                            System.Threading.Thread.Sleep(1000);
                            SftpHelper.UploadFile(publishServer, sql_fname, $"/home/{publishServer.UserName}/ihdis/packages/tempdb/{hospital.FileName}/{version}/hdis_table.sql", null, true);

                            //开始生成数据库脚本
                            var localFile = Utility.GetTempFileName($"v{Utility.m_OprateLog.PackVersion}_sql.zip");
                            SftpHelper.CreateDirectory(publishServer, $"/home/{publishServer.UserName}/ihdis/packages/versions/{hospital.FileName}/");
                            var remoteFile = $"/home/{publishServer.UserName}/ihdis/packages/versions/{hospital.FileName}/v{Utility.m_OprateLog.PackVersion}_sql.zip";
                            CreateDbFile(localFile, hospital);
                            System.Threading.Thread.Sleep(1000);
                            SftpHelper.UploadFile(publishServer, localFile, remoteFile);
                            #endregion
                        }
                        #endregion

                        #region 包注释文件
                        StringBuilder sb_comment = new StringBuilder();
                        sb_comment.AppendLine(Utility.m_OprateLog.PackUpgrade ? "Upgrade Package" : "Installation Package");
                        sb_comment.AppendLine($"Version: {version}");
                        sb_comment.AppendLine($"PackageTime: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        sb_comment.AppendLine($"PackageEnvironment: {devServer.Ip} {hospital.FileName}");
                        sb_comment.AppendLine($"Operator: {Environment.MachineName}");
                        sb_comment.AppendLine($"Moudule: {((Utility.m_OprateLog.PackUpgrade && Utility.m_OprateLog.PackUpgradeTS) ? "ts" : string.Join($"; ", this.gb_Oprate.Controls.Cast<Control>().Where(o => o is CheckBox c && (Utility.m_OprateLog.PackUpgrade ? c.Name != "cb_ts" : true) && c.Name != "cb_remove" && c.Name != "cb_build").Select(o => o.Text).OrderBy(o => o)))}");
                        string lfname = Utility.GetTempFileName($"v{version}_log.dat");
                        string rfname = $"/tmp/v{version}_log.dat";
                        File.WriteAllText(lfname, sb_comment.ToString(), Encoding.UTF8);
                        SftpHelper.UploadFile(server, lfname, rfname);
                        #endregion

                        #region 执行指令
                        SshHelper.ExcuteCmds(devServer, cmdlist, $"{flag}版本打包");
                        #endregion

                        if (Utility.m_OprateLog.PackDownload)
                        {
                            var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                            Utility.SendLog("下载", "开始执行下载", true);
                            Utility.SetProgressBarVisible(true);
                            SftpHelper.DownloadFile(server, $"/home/{server.UserName}/ihdis/packages/versions/{hospital.FileName}/{version}.zip", $"{path}\\{version}.zip", true, progress: Utility.SetProgressBarValue);
                            SftpHelper.DownloadFile(server, $"/home/{server.UserName}/ihdis/packages/versions/{hospital.FileName}/v{version}_log.dat", $"{path}\\v{version}_log.dat", true, progress: Utility.SetProgressBarValue);
                            SftpHelper.DownloadFile(server, $"/home/{server.UserName}/ihdis/packages/versions/{hospital.FileName}/v{version}_sql.zip", $"{path}\\v{version}_sql.zip", true, progress: Utility.SetProgressBarValue, completedAction: () =>
                            {
                                Utility.SendLog("下载", "文件全部下载完毕", true);
                                var cfile = $"{path}\\v{version}_log.dat";
                                if (File.Exists(cfile))
                                {
                                    Utility.SendLog("下载", "开始添加压缩包版本注释", true);
                                    var comment = File.ReadAllText(cfile, Encoding.UTF8);
                                    if (!b_ts)
                                    {
                                        ZipHelper.SetComment($"{path}\\v{version}_sql.zip", comment);
                                    }
                                    ZipHelper.SetComment($"{path}\\{version}.zip", comment);
                                    try
                                    {
                                        Directory.GetFiles(path, "*zip.*.tmp").ToList().ForEach(f =>
                                        {
                                            File.Delete(f);
                                        });
                                        File.Delete(cfile);
                                    }
                                    catch { }
                                    Utility.SendLog("下载", "完成", true);
                                }
                            });
                            Utility.SetProgressBarVisible(false);
                        }
                        else
                        {
                            Utility.SetStatus($"{flag}打包完毕，请使用本工具下载安装包，否则会现场安装时导致安装包校验不通过。");
                        }
                    }
                    catch (Exception ex)
                    {
                        Utility.SendLog($"{flag}打包异常：{ex.Message}");
                    }
                    finally
                    {
                        Utility.RemoveDevFiles(server);
                    }
                }));
                t.Start();
                #endregion
            }
            finally
            {
                b_InPublishNew = false;
            }

        }
        private string CopyPadCmd(DevServer devServer, Hospital hospital, PublishServer publishServer, string version, string db)
        {
            string sql = $"SELECT Version FROM SystemPadVersion ORDER BY ABS(NOW() - CreationTime) ASC LIMIT 0, 1;";
            string connStr = $"server = {devServer.Ip}; port = {hospital.DefaultFort}90; userid = root; password = jlmed#123;database={db};";
            DataTable dt = MySqlHelper.ExecuteDataset(connStr, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return $"sudo cp /home/{devServer.UserName}/ihdis/compose/{hospital.FileName}/data/hfs/wwwroot/files/pad/{dt.Rows[0][0].ToString()}.apk /home/{publishServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data/hfs/wwwroot/files/pad/{dt.Rows[0][0].ToString()}.apk";
            }
            else
            {
                return string.Empty;
            }
        }

        private void DownLoadDb(DevServer devServer, Hospital hospital, string db)
        {
            Utility.SetProgressBarVisible(true);

            //查询数据库表
            string sql = $"SELECT * FROM INFORMATION_SCHEMA.`COLUMNS` C WHERE  C.TABLE_SCHEMA = '{db}';";
            string connStr = $"server = {devServer.Ip}; port = {hospital.DefaultFort}90; userid = root; password = jlmed#123;database={db};";
            DataTable dt = MySqlHelper.ExecuteDataset(connStr, sql).Tables[0];
            Dictionary<string, List<string>> tables = new Dictionary<string, List<string>>();
            foreach (DataRow dr in dt.Rows)
            {
                if (!tables.ContainsKey(dr["TABLE_NAME"].ToString()))
                {
                    tables.Add(dr["TABLE_NAME"].ToString(), new List<string>
                        {
                            dr["COLUMN_NAME"].ToString()
                        });
                }
                else
                {
                    tables[dr["TABLE_NAME"].ToString()].Add(dr["COLUMN_NAME"].ToString());
                }
            }

            //导出数据
            try
            {
                Utility.SendLog("导出数据库", "共有表 " + tables.Count);
                string fname = $"{hospital.SCPath}\\src\\Document\\script\\hdis.sql";
                if (File.Exists(fname))
                {
                    File.Delete(fname);
                }
                int tcount = 0;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SET FOREIGN_KEY_CHECKS = 0;");
                tables.Keys.ToList().ForEach(tabelname =>
                {
                    Utility.SetProgressBarValue(tables.Keys.Count, (ulong)++tcount);
                    sb.AppendLine($"-- 正在导出  表 {db}.{tabelname} 结构");
                    //Utility.SendLog("导出数据库", $"-- 正在导出  表 ihdis.{tabelname} 结构");
                    //删除原表
                    //sb.AppendLine($"DROP TABLE IF EXISTS `{tabelname}`;");

                    //导出表结构
                    sb.AppendLine($"{MySqlHelper.ExecuteDataset(connStr, $"SHOW CREATE TABLE `{db}`.`{tabelname}`;").Tables[0].Rows[0]["Create Table"]};");
                    sb.AppendLine();

                    //导出数据
                    sb.AppendLine($"-- 正在导出  表 {db}.{tabelname} 数据");
                    int rows = int.Parse(MySqlHelper.ExecuteScalar(connStr, $"SELECT COUNT(0) FROM `{db}`.`{tabelname}`;").ToString());
                    //Utility.SendLog("导出数据库", $"-- 正在导出  表 ihdis.{tabelname} 数据，共有数据 {rows}");

                    int sum = 0;
                    using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connStr, $"SELECT * FROM `{db}`.`{tabelname}`;"))
                    {
                        if (reader.HasRows)
                        {
                            //字段类型
                            Dictionary<int, string> dic = new Dictionary<int, string>();

                            //添加字段
                            //sb.AppendLine($"DELETE FROM `{tabelname}`;");

                            StringBuilder sb_insert = new StringBuilder();
                            sb_insert.Append($"INSERT INTO `{tabelname}` (");
                            for (int i = 0; i < reader.FieldCount; ++i)
                            {
                                sb_insert.Append($"`{reader.GetName(i)}`");
                                if (i != reader.FieldCount - 1)
                                    sb_insert.Append(", ");

                                dic.Add(i, reader.GetDataTypeName(i));
                            }
                            sb_insert.Append(") VALUES");

                            sb_insert.AppendLine();
                            //表开始
                            WriteToFile(fname, $"{sb}");
                            sb.Clear();

                            int index = 0;
                            //添加数据
                            while (reader.Read())
                            {
                                sum++;
                                sb.Append("\t(");
                                for (int i = 0; i < reader.FieldCount; ++i)
                                {
                                    sb.Append($"{ConvertData(reader[i], dic[i])}");
                                    if (i != reader.FieldCount - 1)
                                        sb.Append(", ");
                                }
                                sb.Append(")");
                                //500行数据入库一次
                                if (index++ >= 500)
                                {
                                    sb.Append(";");

                                    sb.AppendLine();
                                    sb.AppendLine();
                                    WriteToFile(fname, $"{sb_insert}{sb}");
                                    sb.Clear();
                                    index = 0;
                                    continue;
                                }
                                //1024KB为一组插入语句
                                if (sb.Length >= 100 * 1024)
                                {
                                    sb.Append(";");
                                    sb.AppendLine();
                                    sb.AppendLine();
                                    WriteToFile(fname, $"{sb_insert}{sb}");
                                    sb.Clear();
                                    index = 0;
                                    continue;
                                }
                                if (sum != rows)
                                    sb.Append(",");
                                else
                                    sb.Append(";");

                                sb.AppendLine();
                            }

                            //表结束
                            if (sb.Length > 0)
                            {
                                sb.AppendLine();
                                WriteToFile(fname, $"{sb_insert}{sb}");
                                sb.Clear();
                                index = 0;
                            }
                        }
                    }

                    sb.AppendLine();
                });
                sb.AppendLine("SET FOREIGN_KEY_CHECKS = 1;");
                if (sb.Length > 0)
                    WriteToFile(fname, sb.ToString());
                Utility.SendLog("导出数据库", $"导出数据完毕");
            }
            catch (Exception ex)
            {
                Utility.SendLog("导出数据库", "导出数据异常" + ex.ToString());
            }
            Utility.SetProgressBarVisible(false);
        }
        private string ConvertData(object data, string type)
        {
            if (data.GetType().FullName == "System.DBNull")
                return "NULL";
            string value;
            if (type.Contains("CHAR"))  //varchar ,char
            {
                value = data.ToString();
                if (value.IndexOf("\r") > 0)
                    value = value.Replace("\r", "\\r");
                if (value.IndexOf("\n") > 0)
                    value = value.Replace("\n", "\\n");
                if (value.IndexOf("'") > 0)
                    value = value.Replace("'", "''");
                if (value.IndexOf("\\") > 0)
                    value = value.Replace("\\", "\\\\");
                value = $"'{value}'";
            }
            else if (type.Contains("JSON"))
            {
                value = data.ToString().Trim() == "" ? "NULL" : $"'{data}'";
            }
            else if (type.Contains("TIMESTAMP") || type.Contains("DATETIME"))
            {
                value = data.ToString().Trim() == "" ? "NULL" : $"'{Convert.ToDateTime(data.ToString()):yyyy-MM-dd HH:mm:ss}'";
            }
            else if (type.Contains("DATE"))
            {
                value = data.ToString().Trim() == "" ? "NULL" : $"'{Convert.ToDateTime(data.ToString()):yyyy-MM-dd}'";
            }
            else
            {
                value = data.ToString().Trim() == "" ? "NULL" : data.ToString();
            }
            return value;
        }

        private void WriteToFile(string fname, string content)
        {
            if (!File.Exists(fname))
            {
                using (StreamWriter sw = File.CreateText(fname))
                {
                    sw.Write(content);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(fname))
                {
                    sw.Write(content);
                }
            }
        }
        /// <summary>
        /// 创建并复镜像文件
        /// </summary>
        /// <param name="hospital"></param>
        /// <param name="devServer"></param>
        /// <param name="version"></param>
        /// <param name="cmdlist"></param>
        /// <param name="cb"></param>
        private void SetAppCmd(Hospital hospital, DevServer devServer, string version, List<string> cmdlist, CheckBox cb)
        {
            var cname = cb.Text;
            cmdlist.Add($"rm -rf /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/app/images/{cname}.*");
            cmdlist.Add($"sh /home/{devServer.UserName}/ihdis/dev/build-one.sh {version} {cname} {devServer.UserName} {hospital.FileName}");
            cmdlist.Add($"cp /home/{devServer.UserName}/ihdis/dev/app/images/{cname}.{version}.tar.gz  /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/app/images/");
            //cmdlist.Add($"ssh {publishServer.Ip} sh  /home/{publishServer.UserName}/ihdis/script/publish.sh {version} {cname}");
            //cmdlist.Add($"sudo sed -i \"s/${version}_TAG.*/${version}_TAG=${cname}/\"   /home/{devServer.UserName}/ihdis/packages/{version}/.env");
            cmdlist.Add($"sudo sed -i \"s/images\\/{cname}\\..*.tar.gz/images\\/{cname}.{version}.tar.gz/\" /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/load_images.cmd");
            cmdlist.Add($"sudo sed -i \"s/images\\/{cname}\\..*.tar.gz/images\\/{cname}.{version}.tar.gz/\" /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/load_images.sh");
            cmdlist.Add($"sudo rm -rf /home/{devServer.UserName}/ihdis/dev/app/images/{cname}.{version}*");
            cmdlist.Add($"sudo cp -r /home/{devServer.UserName}/ihdis/dev/app/server/ts /home/{devServer.UserName}/ihdis/packages/{hospital.FileName}/{version}/data");
        }

        private bool CreateDbFile(string fname, Hospital hospital)
        {
            bool b_suc = false;
            //创建文件夹
            string path = fname.Substring(0, fname.LastIndexOf("."));
            DirectoryInfo dir;
            FileSystemInfo[] fileinfo;
            if (Directory.Exists(path))
            {
                dir = new DirectoryInfo(path);
                fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
                Directory.Delete(path);
            }
            Directory.CreateDirectory(path);
            Utility.SendLog("版本打包", $"创建临时目录 {path}");

            //比对脚本  \tools\HH.iHDIS.OPS.Tool\HH.iHDIS.OPS.Tool\Config\CompareConfig.json
            string toolsPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\JianLian.HDIS.PublishHelper\\bin\\Debug\\", "");
            if (File.Exists($"{toolsPath}\\JianLian.HDIS.OPS.Tool\\Config\\CompareConfig.json"))
            {
                if (File.Exists($"{path}\\CompareConfig.json"))
                {
                    File.Delete($"{path}\\CompareConfig.json");
                }
                File.Copy($"{toolsPath}\\JianLian.HDIS.OPS.Tool\\Config\\CompareConfig.json", $"{path}\\CompareConfig.json");
                Utility.SendLog("版本打包", $"复制文件 {$"{toolsPath}\\JL.HDIS.OPS.Tool\\Config\\CompareConfig.json"} =>> {path}\\CompareConfig.json");
            }
            else
            {
                Utility.SendLog("版本打包", $"未能找到比对脚本：{toolsPath}\\JL.HDIS.OPS.Tool\\Config\\CompareConfig.json");
                return b_suc;
            }
            //数据库脚本 \sc\script\ihdis.sql
            if (File.Exists($"{hospital.SCPath}\\src\\Document\\script\\hdis.sql"))
            {
                if (File.Exists($"{path}\\hdis.sql"))
                {
                    File.Delete($"{path}\\hdis.sql");
                }
                File.Copy($"{hospital.SCPath}\\src\\Document\\script\\hdis.sql", $"{path}\\hdis.sql");
                Utility.SendLog("版本打包", $"复制文件 {hospital.SCPath}\\src\\Document\\script\\hdis.sql =>> {path}\\hdis.sql");
            }
            else
            {
                Utility.SendLog("版本打包", $"未能找到数据库脚本：{$"{hospital.SCPath}\\src\\Document\\script\\hdis.sql"}");
                return b_suc;
            }

            //压缩文件
            Utility.SendLog("版本打包", $"压缩文件");
            ZipHelper.ZipFiles(fname, $"{path}");

            //清空缓存文件
            Utility.SendLog("版本打包", $"清空缓存文件与文件夹");
            dir = new DirectoryInfo(path);
            fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
            foreach (FileSystemInfo i in fileinfo)
            {
                if (i is DirectoryInfo)            //判断是否文件夹
                {
                    DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                    subdir.Delete(true);          //删除子目录和文件
                }
                else
                {
                    File.Delete(i.FullName);      //删除指定文件
                }
            }
            Directory.Delete(path);
            b_suc = true;
            Utility.SendLog("版本打包", $"生成数据库脚本完毕 {fname}");
            return b_suc;
        }
        #endregion

        #region 采集插件打包
        object m_InPulishTS = new object();
        bool b_InPulishTS = false;
        private void tsmi_TSRelease_Click(object sender, EventArgs e)
        {
            lock (m_InPulishTS)
            {
                if (b_InPulishTS)
                {
                    return;
                }
            }

            string flag = "采集插件";
            bool b_build = false;
            string path = string.Empty;
            string tspath = string.Empty;
            string version = string.Empty;
            bool custom = false;
            string customHospitals = string.Empty;
            //选择路径和版本
            using (Form_Plugin f = new Form_Plugin())
            {
                f.ShowDialog();
                if (f.m_DialogResult != DialogResult.Yes)
                {
                    return;
                }
                b_build = f.m_Build;
                path = f.m_Path;
                version = f.m_Version;
                custom = f.m_Custom;
                customHospitals = f.m_CustomHospitals;
                tspath = f.m_TSPath;
                Utility.SendLog(flag, $"项目路径 {tspath}");
                Utility.SendLog(flag, $"选择的路径 {path}");
                Utility.SendLog(flag, $"输入的版本 {version}");
                Utility.SendLog(flag, $"是否编译项目 {b_build}");
                Utility.SendLog(flag, $"是否自定义医院 {custom} {(custom ? customHospitals : "")}");
            }
            Utility.SendLog("开始生成采集插件", true);
            Thread t = new Thread(new ThreadStart(delegate
             {
                 CreateTSPlugin(flag, b_build, path, tspath, version, custom, customHospitals);
             }));
            t.Start();
        }

        void CreateTSPlugin(string flag, bool b_build, string path, string tspath, string version, bool custom, string customHospitals)
        {
            lock (m_InPulishTS)
            {
                b_InPulishTS = true;
            }
            try
            {
                //采集插件基础文件路径
                string tsBasePath = $"{AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\", "")}\\ts";

                //编译项目
                if (b_build)
                {
                    Utility.SendLog(flag, $"编译项目");
                    CmdHelper.ExecCmd($"cd {tspath}\\JianLian.Adapter.Center\\Host\\JianLian.Adapter.Hosting\r\n"
                    + $"{tspath.Split(':')[0].Trim()}:\r\n"
                    + $"dotnet publish  -r linux-x64 --self-contained false -o .\\bin\\Release\\net5.0\\publish\r\n"
                    + $"exit\r\n");
                }

                List<string> customs = custom ? customHospitals.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();
                //根据Custom\JianLian.Adapter.Hospital，创建医院文件夹
                string customHospitalPath = $"{tspath}\\JianLian.Adapter.Center\\Custom";
                System.IO.Directory.GetDirectories(customHospitalPath)
                    .ToList()
                    .ForEach(o =>
                    {
                        //医院名称
                        var hospital = o.Substring(o.LastIndexOf('.') + 1);

                        if (custom)
                        {
                            if (!customs.Contains(hospital))
                            {
                                Utility.SendLog(flag, $"自定义生成，医院 {hospital} 丢弃");
                                return;
                            }
                        }
                        var hospitalPath = $"{path}\\{hospital}\\{version}";
                        if (!System.IO.Directory.Exists(hospitalPath))
                        {
                            System.IO.Directory.CreateDirectory(hospitalPath);
                        }
                        else
                        {
                            DeleteFilesItem(hospitalPath, new List<string> { });
                        }
                        //编译
                        if (b_build)
                        {
                            Utility.SendLog(flag, $"编译项目 {hospital}");
                            CmdHelper.ExecCmd($"cd {o}\r\n"
                                + $"{tspath.Split(':')[0].Trim()}:\r\n"
                                + $"dotnet build\r\n"
                                + $"exit\r\n");
                        }

                        Utility.SendLog(flag, $"正在生成医院 {hospital}");
                        if (!File.Exists($"{o}\\bin\\Debug\\net5.0\\Adapter.json"))
                        {
                            SendLog(flag, $"医院[{hospital}]配置文件 Adapter.json 不存在，不继续打包该医院");
                            return;
                        }
                        if (!File.Exists($"{o}\\bin\\Debug\\net5.0\\OtherDb.json"))
                        {
                            SendLog(flag, $"医院[{hospital}]配置文件 OtherDb.json 不存在，不继续打包该医院");
                            return;
                        }
                        if (!File.Exists($"{o}\\bin\\Debug\\net5.0\\appsettings.json"))
                        {
                            SendLog(flag, $"医院[{hospital}]配置文件 appsettings.json 不存在，不继续打包该医院");
                            return;
                        }
                        //拷贝插件基础文件
                        CopyDirectory(tsBasePath, hospitalPath, true);

                        ////拷贝发布的文件到Code文件夹
                        //CopyDirectory($"{tspath}\\JianLian.Adapter.Center\\Host\\JianLian.Adapter.Hosting\\bin\\Release\\net5.0\\publish", $"{hospitalPath}\\code", true);
                        //System.Threading.Thread.Sleep(1000);

                        //拷贝本医院最新的dll文件和配置文件到Code文件夹
                        CopyDirectory($"{o}\\bin\\Debug\\net5.0", $"{hospitalPath}", true);
                        System.Threading.Thread.Sleep(1000);

                        //修改Adapter.json
                        //var adaptercontent = File.ReadAllText($"{hospitalPath}\\code\\Adapter.json", Encoding.UTF8);
                        //adaptercontent = adaptercontent.Replace($"JianLian.Adapter.Sample", $"JianLian.Adapter.{hospital}");
                        //File.WriteAllText($"{hospitalPath}\\code\\Adapter.json", adaptercontent, Encoding.UTF8);

                        ////修改版本号 .env init.sh README.ihdis
                        //var envcontent = File.ReadAllText($"{hospitalPath}\\.env", Encoding.UTF8);
                        //envcontent = envcontent.Replace($"ts_TAG=base", $"ts_TAG={version}");
                        //File.WriteAllText($"{hospitalPath}\\.env", envcontent, Encoding.UTF8);

                        //StringBuilder sb_init = new StringBuilder();
                        //File.ReadAllText($"{hospitalPath}\\init.sh", Encoding.UTF8)
                        //.Replace("sudo docker tag hh/ts:base hh/ts:{$version}", $"sudo docker tag hh/ts:base hh/ts:{version}")
                        //.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        //.ToList()
                        //.ForEach(str =>
                        //{
                        //    sb_init.Append($"{str}\n");
                        //});
                        //File.WriteAllText($"{hospitalPath}\\init.sh", sb_init.ToString(), Encoding.ASCII);

                        var readmecontent = File.ReadAllText($"{hospitalPath}\\README.ihdis", Encoding.UTF8);
                        readmecontent = readmecontent.Replace($"\"version\": \"base\",", $"\"version\": \"{version}\",");

                        File.WriteAllText($"{hospitalPath}\\README.ihdis", readmecontent, Encoding.UTF8);

                        //打包压缩文件
                        Utility.SendLog(flag, $"医院 {hospital} 开始压缩文件");
                        var zippath = hospitalPath.Substring(0, hospitalPath.LastIndexOf('\\'));
                        ZipHelper.ZipFiles($"{zippath}\\ts.zip", hospitalPath);
                        File.Copy($"{zippath}\\ts.zip", $"{hospitalPath}\\ts.zip");
                        File.Delete($"{zippath}\\ts.zip");
                        Utility.SendLog(flag, $"医院 {hospital} 开始清理缓存文件");
                        DeleteFilesItem(hospitalPath, new List<string> { $"{hospitalPath}\\ts.zip" });
                        Utility.SendLog(flag, $"医院 {hospital} 生成完毕");
                    });

                Utility.SendLog(flag, $"生成完毕", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成失败：{ex}");
            }
            finally
            {
                lock (m_InPulishTS)
                    b_InPulishTS = false;
            }
        }

        /// <summary>
        /// 删除全部文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="pathExceptions">例外的目录</param>
        /// <param name="fileExceptions">例外的文件</param>
        private void DeleteFilesItem(string path, List<string> fileExceptions)
        {
            var fs = Directory.GetFiles(path);
            fs.ToList().ForEach(filename =>
            {
                if (fileExceptions.Contains(filename))
                    return;
                File.Delete(filename);
            });
            var ds = Directory.GetDirectories(path);
            ds.ToList().ForEach(o =>
            {
                DeleteFilesItem(o, fileExceptions);
            });

            //清除空目录
            fs = Directory.GetFiles(path);
            if (fs.Length == 0)
            {
                Directory.Delete(path);
            }
        }
        private static bool CopyDirectory(string sourcePath, string destinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try
            {
                sourcePath = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\";
                destinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";

                if (Directory.Exists(sourcePath))
                {
                    if (Directory.Exists(destinationPath) == false)
                        Directory.CreateDirectory(destinationPath);

                    foreach (string fls in Directory.GetFiles(sourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(destinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(sourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, destinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch
            {
                ret = false;
            }
            return ret;
        }
        #endregion

        #region 升级日志
        private void tsmi_UpgradeLog_Click(object sender, EventArgs e)
        {
            using (Form_UpgradeLog f = new Form_UpgradeLog())
            {
                f.ShowDialog();
            }
        }
        #endregion

        #region 数据库管理
        private void tsmi_HeidiSQL_Click(object sender, EventArgs e)
        {
            if (!HasSelectdHospital(out DevServer server, out Hospital hospital))
            {
                return;
            }
            if (!SftpHelper.Exists(server, $"/home/{server.UserName}/ihdis/compose/{hospital.FileName}") && !SftpHelper.Exists(server, $"/home/{server.UserName}/ihdis/{hospital.FileName}"))
            {
                MessageBox.Show("医院不存在", "提示");
                return;
            }
            string heidisql = @"C:\Program Files\HeidiSQL\heidisql.exe";

            if (!File.Exists(heidisql))
            {
                MessageBox.Show($"未能检测到 [HeidiSQL]，请先安装！\r\n默认检测路径为：{heidisql}", "提示");
            }
            else
            {
                if (hospital.FileName == "produse")
                {
                    System.Diagnostics.Process.Start(heidisql, $" -d {server.Ip}({hospital.FileName}) -n {0} -h {server.Ip} -u root -p jlmed#123 -P {hospital.DefaultFort}90");
                }
                else
                {
                    System.Diagnostics.Process.Start(heidisql, $" -d {server.Ip}({hospital.FileName}) -n {0} -h {server.Ip} -u root -p jlmed#123 -P {hospital.DefaultFort}90");
                }
            }
        }
        #endregion

        #region 关于
        private void tsmi_About_Click(object sender, EventArgs e)
        {
            using (Form_About f = new Form_About())
            {
                f.ShowDialog();
            }
        }
        #endregion

        #region 前端代码生成
        private void tsmi_CodeGeneRATOR_Click(object sender, EventArgs e)
        {
            using (Form_CodeGenerator f = new Form_CodeGenerator())
            {
                f.ShowDialog();
            }
        }
        #endregion
    }
}
