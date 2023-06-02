using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JianLian.HDIS.PublishHelper
{
    public partial class Form_VersionFile : Form
    {
        PublishServer server = null;
        VersionFolder folder = null;
        public Form_VersionFile(PublishServer publishServer, VersionFolder versionFolder)
        {
            InitializeComponent();
            server = publishServer;
            folder = versionFolder;
        }

        private void Form_VersionFile_Load(object sender, EventArgs e)
        {
            this.Text = $"版本文件管理 -{server.Name} - {folder.FolderName}";
            RefreshView();
        }

        private void RefreshView()
        {
            listView_VersionFile.Items.Clear();
            folder.Files
                .OrderByDescending(o => o.CreateTime)
                .ToList()
                ?.ForEach(o =>
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems[0].Text = o.FileName;
                    item.SubItems.Add(o.Size.ToString());
                    item.SubItems.Add(o.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    listView_VersionFile.Items.Add(item);
                });
        }

        private void tsmi_Delete_Click(object sender, EventArgs e)
        {
            if (this.listView_VersionFile.SelectedItems.Count <= 0)
            {
                return;
            }
            if (MessageBox.Show("本操作将同步操作服务端文件,是否确认删除？", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            var filename = this.listView_VersionFile.SelectedItems[0].Text;
            if (!Utility.Ping(server.Ip))
            {
                MessageBox.Show("网络连接失败！", "提示");
                return;
            }
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //移除远端文件
                SshHelper.ExcuteCmd(server, $"sudo rm -rf /home/{server.UserName}/ihdis/packages/versions/{folder.FolderName}/{filename.Replace(".zip", "")}*", true);

                //更新版本文件
                //var replacename = filename.Replace(".zip", "");
                //EditFile(server, $"/home/{server.UserName}/ihdis/version_list", Utility.GetTempFileName("version_list"), replacename);
                //EditFile(server, $"/home/{server.UserName}/ihdis/version_liste", Utility.GetTempFileName("version_liste"), replacename);
                //EditFile(server, $"/home/{server.UserName}/ihdis/version/version_list", Utility.GetTempFileName("version_list"), replacename);
                //EditFile(server, $"/home/{server.UserName}/ihdis/version/version_liste", Utility.GetTempFileName("version_liste"), replacename);

                //移除配置
                var item = folder.Files.Where(o => o.FileName == this.listView_VersionFile.SelectedItems[0].Text).FirstOrDefault();
                if (!(item is null))
                {
                    folder.Files.Remove(item);
                    Utility.SaveDevServer();
                    RefreshView();
                }
            }
            catch (Exception ex)
            {
                Utility.SendLog("", $"删除失败：{ex}");
            }
            Utility.SendLog("", $"删除成功");
            this.Cursor = Cursors.Default;
        }

        private void EditFile(Server server, string remoteFile, string localFile, string fname)
        {
            bool b_suc = SftpHelper.DownloadFile(server, remoteFile, localFile);
            if (b_suc)
            {
                if (File.Exists(localFile))
                {
                    List<string> list = new List<string>();
                    string[] vs = File.ReadAllText(localFile).Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    vs.ToList().ForEach(str =>
                    {
                        str = str.Trim();
                        if (str == fname)
                            return;
                        if (!list.Contains(str))
                            list.Add(str);
                    });
                    File.Delete(localFile);
                    File.WriteAllLines(localFile, list);
                    SftpHelper.UploadFile(server, localFile, remoteFile);
                    Utility.SendLog("", $"文件修改完毕 {remoteFile}");
                }
            }
        }

        private void ll_Sync_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!Utility.Ping(server.Ip))
            {
                MessageBox.Show("网络连接失败！", "提示");
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var files = SftpHelper.ListFiles(server, $"/home/{server.UserName}/ihdis/packages/versions/{folder.FolderName}");
                files.ForEach(ftpFile =>
                {
                    if (ftpFile.Name == "version_list" || ftpFile.Name == "version_liste" || ftpFile.Name.EndsWith("_sql.zip") || !ftpFile.Name.EndsWith(".zip"))
                    {
                        return;
                    }
                    var file = new VersionFile
                    {
                        FileName = ftpFile.Name,
                        Size = ftpFile.Length,
                        CreateTime = ftpFile.LastWriteTime
                    };
                    var item = folder.Files.Where(o => o.FileName == ftpFile.Name).FirstOrDefault();

                    if (item is null)
                    {
                        folder.Files.Add(file);
                        Utility.SendLog("同步", $"发现新文件 {file.FileName}");
                    }
                    else
                    {
                        //已有文件大小与时间是否一致
                        if (item.Size != file.Size)
                        {
                            Utility.SendLog("同步", $"文件 {file.FileName} 文件大小不一致 {item.Size}，更新为实际大小 {file.Size}");
                            item.Size = file.Size;
                        }
                        if (item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss") != file.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"))
                        {
                            Utility.SendLog("同步", $"文件 {file.FileName} 创建时间不一致 {item.CreateTime:yyyy-MM-dd HH:mm:ss}，更新为实际创建时间 {file.CreateTime:yyyy-MM-dd HH:mm:ss}");
                            item.CreateTime = file.CreateTime;
                        }
                    }
                });

                var notExsit = new List<VersionFile>();
                var fnames = files.Select(o => o.Name).ToList();
                folder.Files.ForEach(file =>
                {
                    if (!fnames.Contains(file.FileName))
                    {
                        notExsit.Add(file);
                    }
                });
                notExsit.ForEach(file =>
                {
                    folder.Files.Remove(file);
                    //文件不存在
                    Utility.SendLog("同步", $"文件已经不存在 {file.FileName}");
                });
            }
            catch (Exception ex)
            {
                Utility.SendLog("同步", $"同步失败：{ex}");
            }
            RefreshView();
            Utility.SendLog("同步", $"同步完毕");
            this.Cursor = Cursors.Default;
        }

        bool b_in = false;
        private void tsmi_Download_Click(object sender, EventArgs e)
        {
            if (b_in)
                return;
            if (this.listView_VersionFile.SelectedItems.Count <= 0)
            {
                return;
            }
            if (MessageBox.Show("版本文件与数据库脚本文件将会下载到桌面，是否确定？", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            b_in = true;
            var version = this.listView_VersionFile.SelectedItems[0].Text.Replace(".zip", "");
            var path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            Utility.SendLog("下载", "开始执行下载", true);
            Thread t = new Thread(new ThreadStart(delegate
            {
                Utility.SetProgressBarVisible(true);
                SftpHelper.DownloadFile(server, $"/home/{server.UserName}/ihdis/packages/versions/{folder.FolderName}/{version}.zip", $"{path}\\{version}.zip", true, progress: Utility.SetProgressBarValue);
                SftpHelper.DownloadFile(server, $"/home/{server.UserName}/ihdis/packages/versions/{folder.FolderName}/v{version}_log.dat", $"{path}\\v{version}_log.dat", true, progress: Utility.SetProgressBarValue);
                SftpHelper.DownloadFile(server, $"/home/{server.UserName}/ihdis/packages/versions/{folder.FolderName}/v{version}_sql.zip", $"{path}\\v{version}_sql.zip", true, progress: Utility.SetProgressBarValue, completedAction: () =>
                {
                    b_in = false;
                    Utility.SendLog("下载", "文件全部下载完毕", true);
                    var cfile = $"{path}\\v{version}_log.dat";
                    if (System.IO.File.Exists(cfile))
                    {
                        Utility.SendLog("下载", "开始添加压缩包版本注释", true);
                        var comment = File.ReadAllText(cfile, Encoding.UTF8);
                        ZipHelper.SetComment($"{path}\\v{version}_sql.zip", comment);
                        ZipHelper.SetComment($"{path}\\{version}.zip", comment);
                        try
                        {
                            Directory.GetFiles(path, "*zip.*.tmp").ToList().ForEach(f =>
                            {
                                System.IO.File.Delete(f);
                            });
                            System.IO.File.Delete(cfile);
                        } catch { }
                        Utility.SendLog("下载", "完成", true);
                    }
                });
                Utility.SetProgressBarVisible(false);
                b_in = false;
            }));
            t.Start();
        }
    }
}
