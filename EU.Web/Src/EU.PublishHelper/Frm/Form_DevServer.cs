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
    public partial class Form_DevServer : Form
    {
        public Form_DevServer()
        {
            InitializeComponent();
        }

        private void Form_DevServer_Load(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void tsmi_Insert_Click(object sender, EventArgs e)
        {
            using (Form_DevServer_Item f = new Form_DevServer_Item("新增"))
            {
                f.ShowDialog();
            }
            Utility.SaveDevServer();
            RefreshView();
        }

        private void tsmi_Edit_Click(object sender, EventArgs e)
        {
            if (this.listView_Dev.SelectedItems.Count <= 0)
                return;
            using (Form_DevServer_Item f = new Form_DevServer_Item($"编辑 - {this.listView_Dev.SelectedItems[0].Text}"))
            {
                f.ShowDialog();
            }
            Utility.SaveDevServer();
            RefreshView();
        }

        private void tsmi_Delete_Click(object sender, EventArgs e)
        {
            if (this.listView_Dev.SelectedItems.Count <= 0)
            {
                return;
            }
            if (MessageBox.Show("是否确认删除？", "提示", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            var item = Utility.m_DevServers.Where(o => o.Name == this.listView_Dev.SelectedItems[0].Text).FirstOrDefault();
            if (!(item is null))
            {
                Utility.m_DevServers.Remove(item);
                Utility.SaveDevServer();
                RefreshView();
            }
        }

        private void RefreshView()
        {
            listView_Dev.Items.Clear();
            Utility.m_DevServers.ForEach(o =>
            {
                ListViewItem item = new ListViewItem();
                item.SubItems[0].Text = o.Name;
                item.SubItems.Add(o.Ip);
                item.SubItems.Add(o.UserName);
                item.SubItems.Add(o.Password);
                item.SubItems.Add(o.SuPassword);
                item.SubItems.Add(o.Port.ToString());
                listView_Dev.Items.Add(item);
            });
        }

        private void tsmi_Hospital_Click(object sender, EventArgs e)
        {
            if (this.listView_Dev.SelectedItems.Count <= 0)
            {
                return;
            }
            using (Form_Hospital f = new Form_Hospital(this.listView_Dev.SelectedItems[0].Text))
            {
                f.ShowDialog();
            }
            Utility.SaveDevServer();
        }

        private void listView_Dev_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView_Dev.SelectedItems.Count <= 0)
            {
                return;
            }
            using (Form_Hospital f = new Form_Hospital(this.listView_Dev.SelectedItems[0].Text))
            {
                f.ShowDialog();
            }
            Utility.SaveDevServer();
        }
    }
}
