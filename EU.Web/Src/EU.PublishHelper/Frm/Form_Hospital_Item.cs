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
    public partial class Form_Hospital_Item : Form
    {
        bool b_edit = false;
        string devName = string.Empty;
        Hospital hoipitalOld;
        public Hospital m_Hoipital;
        public Form_Hospital_Item(string devname, string name)
        {
            InitializeComponent();
            this.Text = name;
            devName = devname;
            b_edit = this.Text != "新增";
            var path = System.Environment.CurrentDirectory.Replace(@"\backend\src\Assistant\JianLian.HDIS.PublishHelper\bin\Debug", "");
            this.txt_SCPath.Text = $"{path}\\backend";
            this.txt_WWWPath.Text = $"{path}\\web";
        }

        private void Form_Hospital_Item_Load(object sender, EventArgs e)
        {
            if (b_edit)
            {
                hoipitalOld = Utility.m_DevServers
                    .Where(o => o.Name == devName)
                    .FirstOrDefault()
                    ?.Hospitals
                    ?.Where(o => o.Name == this.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1])
                    .FirstOrDefault();
                if (!(hoipitalOld is null))
                {
                    this.txt_Name.Text = hoipitalOld.Name;
                    this.txt_FileName.Text = hoipitalOld.FileName;
                    this.txt_Port.Text = hoipitalOld.DefaultFort;
                    this.txt_SCPath.Text = hoipitalOld.SCPath;
                    this.txt_WWWPath.Text = hoipitalOld.WWWPath;
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

            int.TryParse(this.txt_Port.Text.Trim(), out int port);
            if (port < 100 || port > 655)
            {
                MessageBox.Show($"端口配置不正确，端口取值范围[100,655]！", "提示");
                return;
            }
            var server = Utility.m_DevServers.Where(o => o.Name == devName).FirstOrDefault();
            if (server is null)
                return;
            m_Hoipital = new Hospital
            {
                Name = this.txt_Name.Text.Trim(),
                FileName = this.txt_FileName.Text.Trim(),
                DefaultFort = this.txt_Port.Text.Trim(),
                SCPath = this.txt_SCPath.Text.Trim(),
                WWWPath = this.txt_WWWPath.Text.Trim(),
            };
            if (b_edit)
            {
                server.Hospitals.Remove(hoipitalOld);
            }
            bool b_suc = false;
            try
            {
                if (server.Hospitals.Any(o => o.Name == this.txt_Name.Text.Trim()))
                {
                    MessageBox.Show($"医院名称已存在！", "提示");
                    return;
                }
                if (server.Hospitals.Any(o => o.FileName == this.txt_FileName.Text.Trim()))
                {
                    MessageBox.Show($"文件夹已存在！", "提示");
                    return;
                }
                if (server.Hospitals.Any(o => o.DefaultFort == this.txt_Port.Text.Trim()))
                {
                    MessageBox.Show($"端口已存在！", "提示");
                    return;
                }
                b_suc = true;
            }
            finally
            {
                if (!b_suc)
                {
                    m_Hoipital = null;
                    if (b_edit)
                    {
                        server.Hospitals.Add(hoipitalOld);
                    }
                }
            }
            server.Hospitals.Add(m_Hoipital);
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
