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
    public partial class Form_DevServer_Item : Form
    {
        bool b_edit = false;
        public Form_DevServer_Item(string name)
        {
            InitializeComponent();
            this.Text = name;
            b_edit = this.Text != "新增";
        }

        private void Form_DevServer_Item_Load(object sender, EventArgs e)
        {
            if (b_edit)
            {
                var item = Utility.m_DevServers.Where(o => o.Name == this.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1]).FirstOrDefault();
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
            var devServer = new DevServer
            {
                Name = this.txt_Name.Text.Trim(),
                Ip = this.txt_Ip.Text.Trim(),
                UserName = this.txt_UserName.Text.Trim(),
                Password = this.txt_Password.Text.Trim(),
                SuPassword = this.txt_SuPassword.Text.Trim(),
                Port = int.Parse(this.txt_Port.Text),
                Hospitals = new List<Hospital>()
            };
            if (b_edit)
            {
                var nameOld = this.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (nameOld != this.txt_Name.Text.Trim())
                {
                    if (Utility.m_DevServers.Any(o => o.Name == this.txt_Name.Text.Trim()))
                    {
                        MessageBox.Show($"服务器名称已存在！", "提示");
                        return;
                    }
                    Utility.m_DevServers.Add(devServer);
                }
                else
                {
                    var item = Utility.m_DevServers.Where(o => o.Name == this.txt_Name.Text.Trim()).FirstOrDefault();
                    if (!(item is null))
                    {
                        devServer.Hospitals = item.Hospitals;
                        Utility.m_DevServers.Remove(item);
                        Utility.m_DevServers.Add(devServer);
                    }
                }
            }
            else
            {
                if (Utility.m_DevServers.Any(o => o.Name == this.txt_Name.Text.Trim()))
                {
                    MessageBox.Show($"服务器名称已存在！", "提示");
                    return;
                }
                Utility.m_DevServers.Add(devServer);
            }
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
