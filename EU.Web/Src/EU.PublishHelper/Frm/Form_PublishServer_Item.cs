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
    public partial class Form_PublishServer_Item : Form
    {
        bool b_edit = false;
        public Form_PublishServer_Item(string name)
        {
            InitializeComponent();
            this.Text = name;
            b_edit = this.Text != "新增";
        }

        private void Form_PublishServer_Item_Load(object sender, EventArgs e)
        {
            if (b_edit)
            {
                var item = Utility.m_PublishServers.Where(o => o.Name == this.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1]).FirstOrDefault();
                if (!(item is null))
                {
                    this.txt_Name.Text = item.Name;
                    this.txt_Ip.Text = item.Ip;
                    this.txt_UserName.Text = item.UserName;
                    this.txt_Password.Text = item.Password;
                    this.txt_SuPassword.Text = item.SuPassword;
                    this.txt_Port.Text = item.Port.ToString();
                }
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            foreach (var control in gb_main.Controls)
            {
                if (control is TextBox)
                {
                    var textBox = control as TextBox;
                    if (string.IsNullOrEmpty(textBox.Text.Trim()))
                    {
                        MessageBox.Show($"{textBox.Name}，不允许为空！", "提示");
                        return;
                    }
                }
            }
            var publishServer = new PublishServer
            {
                Name = this.txt_Name.Text.Trim(),
                Ip = this.txt_Ip.Text.Trim(),
                UserName = this.txt_UserName.Text.Trim(),
                Password = this.txt_Password.Text.Trim(),
                SuPassword = this.txt_SuPassword.Text.Trim(),
                Port = int.Parse(this.txt_Port.Text),
                Folders = new List<VersionFolder>()
            };
            if (b_edit)
            {
                var nameOld = this.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (nameOld != this.txt_Name.Text.Trim())
                {
                    if (Utility.m_PublishServers.Any(o => o.Name == this.txt_Name.Text.Trim()))
                    {
                        MessageBox.Show($"服务器名称已存在！", "提示");
                        return;
                    }
                    Utility.m_PublishServers.Add(publishServer);
                }
                else
                {
                    var item = Utility.m_PublishServers.Where(o => o.Name == this.txt_Name.Text.Trim()).FirstOrDefault();
                    if (!(item is null))
                    {
                        publishServer.Folders = item.Folders;
                        Utility.m_PublishServers.Remove(item);
                        Utility.m_PublishServers.Add(publishServer);
                    }
                }
            }
            else
            {
                if (Utility.m_PublishServers.Any(o => o.Name == this.txt_Name.Text.Trim()))
                {
                    MessageBox.Show($"服务器名称已存在！", "提示");
                    return;
                }
                Utility.m_PublishServers.Add(publishServer);
            }
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
