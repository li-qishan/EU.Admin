using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JianLian.HDIS.PublishHelper
{
    public partial class Form_PublishServer : Form
    {
        public Form_PublishServer()
        {
            InitializeComponent();
        }

        private void Form_PublishServer_Load(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void tsmi_Insert_Click(object sender, EventArgs e)
        {
            using (Form_PublishServer_Item f = new Form_PublishServer_Item("新增"))
            {
                f.ShowDialog();
            }
            Utility.SavePublishServer();
            RefreshView();
        }

        private void tsmi_Edit_Click(object sender, EventArgs e)
        {
            if (this.listView_Publish.SelectedItems.Count <= 0)
                return;
            using (Form_PublishServer_Item f = new Form_PublishServer_Item($"编辑 - {this.listView_Publish.SelectedItems[0].Text}"))
            {
                f.ShowDialog();
            }
            Utility.SavePublishServer();
            RefreshView();
        }

        private void tsmi_Delete_Click(object sender, EventArgs e)
        {
            if (this.listView_Publish.SelectedItems.Count <= 0)
            {
                return;
            }
            if (MessageBox.Show("是否确认删除？", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            var item = Utility.m_PublishServers.Where(o => o.Name == this.listView_Publish.SelectedItems[0].Text).FirstOrDefault();
            if (!(item is null))
            {
                Utility.m_PublishServers.Remove(item);
                Utility.SavePublishServer();
                RefreshView();
            }
        }

        private void RefreshView()
        {
            listView_Publish.Items.Clear();
            Utility.m_PublishServers.ForEach(o =>
            {
                ListViewItem item = new ListViewItem();
                item.SubItems[0].Text = o.Name;
                item.SubItems.Add(o.Ip);
                item.SubItems.Add(o.UserName);
                item.SubItems.Add(o.Password);
                item.SubItems.Add(o.SuPassword);
                item.SubItems.Add(o.Port.ToString());
                listView_Publish.Items.Add(item);
            });
        }

        private void tsmi_VersionFile_Click(object sender, EventArgs e)
        {
            if (this.listView_Publish.SelectedItems.Count <= 0)
            {
                return;
            }
            using (Form_VersionFolder f = new Form_VersionFolder(this.listView_Publish.SelectedItems[0].Text))
            {
                f.ShowDialog();
            }
            Utility.SavePublishServer();
        }

        private void listView_Pulish_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView_Publish.SelectedItems.Count <= 0)
            {
                return;
            }
            using (Form_VersionFolder f = new Form_VersionFolder(this.listView_Publish.SelectedItems[0].Text))
            {
                f.ShowDialog();
            }
            Utility.SavePublishServer();
        }
    }
}
