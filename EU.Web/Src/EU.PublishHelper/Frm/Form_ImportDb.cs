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
    public partial class Form_ImportDb : Form
    {
        public int m_Tables = 0;
        public string m_Db = "mysql";
        public string m_FileName = string.Empty;
        public DialogResult m_DialogResult = DialogResult.No;
        public Form_ImportDb()
        {
            InitializeComponent();
        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;//该值确定是否可以选择多个文件
                ofd.Title = "请选择文件";
                ofd.Filter = "sql文件|*.sql";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.txt_fname.Text = ofd.FileName;
                }
            }
        }

        private void btn_Import_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_fname.Text) || !File.Exists(this.txt_fname.Text))
                return;
            m_FileName = this.txt_fname.Text;
            m_DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
