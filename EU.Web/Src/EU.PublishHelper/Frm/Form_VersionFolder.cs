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
    public partial class Form_VersionFolder : Form
    {
        string publishName = string.Empty;
        public Form_VersionFolder(string name)
        {
            InitializeComponent();
            publishName = name;
        }

        private void Form_VersionFile_Load(object sender, EventArgs e)
        {
            this.Text = $"版本文件夹管理 - {publishName}";
            RefreshView();
        }

        private void RefreshView()
        {
            listView_VersionFolder.Items.Clear();
            Utility.m_PublishServers.Where(o => o.Name == publishName).FirstOrDefault()
                ?.Folders
                .OrderByDescending(o => o.CreateTime)
                .ToList()
                ?.ForEach(o =>
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems[0].Text = o.FolderName;
                    item.SubItems.Add(o.Size.ToString());
                    item.SubItems.Add(o.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(o.Files.Count.ToString());
                    listView_VersionFolder.Items.Add(item);
                });
        }

        private void tsmi_Delete_Click(object sender, EventArgs e)
        {
            if (this.listView_VersionFolder.SelectedItems.Count <= 0)
            {
                return;
            }
            if (MessageBox.Show("本操作将同步操作服务端文件,是否确认删除？", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            var foldername = this.listView_VersionFolder.SelectedItems[0].Text;
            var server = Utility.m_PublishServers.Where(o => o.Name == publishName).FirstOrDefault();
            if (!(server is null))
            {
                if (!Utility.Ping(server.Ip))
                {
                    MessageBox.Show("网络连接失败！", "提示");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    //移除远端文件
                    SshHelper.ExcuteCmd(server, $"sudo rm -rf /home/{server.UserName}/ihdis/packages/versions/{foldername}", true);

                    //移除配置
                    var item = server.Folders.Where(o => o.FolderName == this.listView_VersionFolder.SelectedItems[0].Text).FirstOrDefault();
                    if (!(item is null))
                    {
                        server.Folders.Remove(item);
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
        }

        private void ll_Sync_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var server = Utility.m_PublishServers.Where(o => o.Name == publishName).FirstOrDefault();
            if (server is null)
                return;
            if (!Utility.Ping(server.Ip))
            {
                MessageBox.Show("网络连接失败！", "提示");
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var folders = SftpHelper.ListDirectory(server, $"/home/{server.UserName}/ihdis/packages/versions/");
                folders.ForEach(f =>
                {
                    var folder = new VersionFolder
                    {
                        FolderName = f.Name,
                        Size = f.Length,
                        CreateTime = f.LastWriteTime,
                        Files = new List<VersionFile>()
                    };
                    var item = server.Folders.Where(o => o.FolderName == f.Name).FirstOrDefault();

                    if (item is null)
                    {
                        server.Folders.Add(folder);
                        Utility.SendLog("同步", $"发现新文件夹 {folder.FolderName}");
                    }
                    else
                    {
                        //已有文件大小与时间是否一致
                        if (item.Size != folder.Size)
                        {
                            Utility.SendLog("同步", $"文件夹 {folder.FolderName} 文件大小不一致 {item.Size}，更新为实际大小 {folder.Size}");
                            item.Size = folder.Size;
                        }
                        if (item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss") != folder.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"))
                        {
                            Utility.SendLog("同步", $"文件夹 {folder.FolderName} 创建时间不一致 {item.CreateTime:yyyy-MM-dd HH:mm:ss}，更新为实际创建时间 {folder.CreateTime:yyyy-MM-dd HH:mm:ss}");
                            item.CreateTime = folder.CreateTime;
                        }
                    }
                });

                var notExsit = new List<VersionFolder>();
                var fs = folders.Select(o => o.Name).ToList();
                server.Folders.ForEach(f =>
                {
                    if (!fs.Contains(f.FolderName))
                    {
                        notExsit.Add(f);
                    }
                });
                notExsit.ForEach(f =>
                {
                    server.Folders.Remove(f);
                    //文件不存在
                    Utility.SendLog("同步", $"文件夹已经不存在 {f.FolderName}");
                });
                server.Folders?.ForEach(f => SyscFiles(server, f));
            }
            catch (Exception ex)
            {
                Utility.SendLog("同步", $"同步失败：{ex}");
            }
            RefreshView();
            Utility.SendLog("同步", $"同步完毕");
            this.Cursor = Cursors.Default;
        }

        private void SyscFiles(PublishServer server, VersionFolder folder)
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

        private void listView_VersionFolder_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView_VersionFolder.SelectedItems.Count <= 0)
            {
                return;
            }
            var server = Utility.m_PublishServers.Where(o => o.Name == publishName).FirstOrDefault();
            var item = server.Folders.Where(o => o.FolderName == this.listView_VersionFolder.SelectedItems[0].Text).FirstOrDefault();

            if (server is null || item is null)
                return;

            using (Form_VersionFile f = new Form_VersionFile(server, item))
            {
                f.ShowDialog();
            }
            Utility.SavePublishServer();
            RefreshView();
        }
    }
}
