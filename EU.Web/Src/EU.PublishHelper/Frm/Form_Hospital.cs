using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JianLian.HDIS.PublishHelper
{
    public partial class Form_Hospital : Form
    {
        string devName = string.Empty;
        public Form_Hospital(string name)
        {
            InitializeComponent();
            devName = name;
        }

        private void Form_Hospital_Load(object sender, EventArgs e)
        {
            this.Text = $"医院管理 - {devName}";
            RefreshView();
        }

        private void RefreshView()
        {
            listView_Hospital.Items.Clear();
            Utility.m_DevServers.Where(o => o.Name == devName).FirstOrDefault()
                ?.Hospitals
                ?.ForEach(o =>
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems[0].Text = o.Name;
                    item.SubItems.Add(o.FileName);
                    item.SubItems.Add(o.DefaultFort);
                    item.SubItems.Add(o.SCPath);
                    item.SubItems.Add(o.WWWPath);
                    listView_Hospital.Items.Add(item);
                });
        }


        private void tsmi_Insert_Click(object sender, EventArgs e)
        {
            using (Form_Hospital_Item f = new Form_Hospital_Item(devName, "新增"))
            {
                f.ShowDialog();
            }
            Utility.SaveDevServer();
            RefreshView();
        }

        private void tsmi_Edit_Click(object sender, EventArgs e)
        {
            if (this.listView_Hospital.SelectedItems.Count <= 0)
                return;
            using (Form_Hospital_Item f = new Form_Hospital_Item(devName, $"编辑 - {this.listView_Hospital.SelectedItems[0].Text}"))
            {
                f.ShowDialog();
            }
            Utility.SaveDevServer();
            RefreshView();
        }

        private void tsmi_Delete_Click(object sender, EventArgs e)
        {
            if (this.listView_Hospital.SelectedItems.Count <= 0)
            {
                return;
            }
            if (MessageBox.Show("是否确认删除？", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            var server = Utility.m_DevServers.Where(o => o.Name == devName).FirstOrDefault();
            if (!(server is null))
            {
                var item = server.Hospitals.Where(o => o.Name == this.listView_Hospital.SelectedItems[0].Text).FirstOrDefault();
                if (!(item is null))
                {
                    server.Hospitals.Remove(item);
                    Utility.SaveDevServer();
                    RefreshView();
                }
            }
        }

        private void ll_Sync_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var server = Utility.m_DevServers.Where(o => o.Name == devName).FirstOrDefault();
            if (server is null)
                return;
            if (!Utility.Ping(server.Ip))
            {
                MessageBox.Show("网络连接失败！", "提示");
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            bool b_add = false;
            try
            {
                bool b_dev = SftpHelper.Exists(server, $"/home/{server.UserName}/compose");
                var hospitals = SftpHelper.ListDirectory(server, b_dev ? $"/home/{server.UserName}/compose" : $"/home/{server.UserName}").Select(o => o.Name).ToList();
                hospitals.ForEach(hospitalName =>
                {
                    if (hospitalName == (b_dev ? "sample" : "tools"))
                        return;
                    if (!b_dev)
                    {
                        if (!SftpHelper.Exists(server, $"/home/{server.UserName}/{hospitalName}/.env"))
                        {
                            Utility.SendLog("同步", $"发现 文件夹 {hospitalName} 不存在 .env 文件，跳过同步");
                            return;
                        }
                    }
                    var item = server.Hospitals.Where(o => o.FileName == hospitalName).FirstOrDefault();
                    var path = System.Environment.CurrentDirectory.Replace(@"\backend\src\Assistant\JianLian.HDIS.PublishHelper\bin\Debug", "");
                    //获取医院信息
                    var fname = Utility.GetTempFileName(".env");
                    bool b_suc = SftpHelper.DownloadFile(server, b_dev ? $"/home/{server.UserName}/compose/{hospitalName}/.env" : $"/home/{server.UserName}/{hospitalName}/.env", fname);
                    var content = File.ReadAllText(fname);
                    var datas = content.Split(new string[] { "\r", "\n", "=" }, StringSplitOptions.RemoveEmptyEntries);
                    var hospital = new Hospital
                    {
                        Name = b_dev ? datas[3] : hospitalName,
                        FileName = b_dev ? datas[3] : hospitalName,
                        DefaultFort = datas[1],
                        SCPath = $"{path}\\backend",
                        WWWPath = $""
                    };

                    if (item is null)
                    {
                        if (b_suc)
                        {
                            server.Hospitals.Add(hospital);
                            //新医院
                            Utility.SendLog("同步", $"发现新医院 {hospitalName} 端口 {hospital.DefaultFort}");
                            b_add = true;
                        }
                    }
                    else
                    {
                        //已有医院查看端口是否一致
                        if (item.DefaultFort != hospital.DefaultFort)
                        {
                            Utility.SendLog("同步", $"医院 {hospitalName} 端口号不一致 {item.DefaultFort}，更新为实际端口号 {hospital.DefaultFort}");
                            item.DefaultFort = hospital.DefaultFort;
                        }
                        else
                        {
                            Utility.SendLog("同步", $"医院 {hospitalName} 端口号一致 {hospital.DefaultFort}");
                        }
                    }
                });

                var notExsit = new List<Hospital>();
                server.Hospitals.ForEach(hospital =>
                {
                    if (!hospitals.Contains(hospital.FileName))
                    {
                        notExsit.Add(hospital);
                    }
                });
                notExsit.ForEach(hospital =>
                {
                    server.Hospitals.Remove(hospital);
                    //医院不存在
                    Utility.SendLog("同步", $"医院已经不存在 {hospital.FileName} 端口 {hospital.DefaultFort}");
                });
            }
            catch (Exception ex)
            {
                Utility.SendLog("同步", $"同步失败：{ex}");
            }
            RefreshView();
            Utility.SendLog("同步", $"同步完毕" + (b_add ? "请完善医院信息" : ""));
            this.Cursor = Cursors.Default;
        }

        private void listView_Hospital_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView_Hospital.SelectedItems.Count <= 0)
                return;
            using (Form_Hospital_Item f = new Form_Hospital_Item(devName, $"编辑 - {this.listView_Hospital.SelectedItems[0].Text}"))
            {
                f.ShowDialog();
            }
            Utility.SaveDevServer();
            RefreshView();
        }
    }
}
