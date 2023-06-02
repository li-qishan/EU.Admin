using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JianLian.HDIS.PublishHelper
{
    public partial class Form_Plugin : Form
    {
        public DialogResult m_DialogResult = DialogResult.No;
        public string m_TSPath = string.Empty;
        public string m_Version = string.Empty;
        public string m_Path = string.Empty;
        public bool m_Custom = false;
        public bool m_Build = false;
        public string m_CustomHospitals = string.Empty;
        public Form_Plugin()
        {
            InitializeComponent();
            m_DialogResult = DialogResult.No;
            var p = System.Environment.CurrentDirectory.Replace(@"\backend\src\Assistant\JianLian.HDIS.PublishHelper\bin\Debug", "");
            txt_tsproject.Text = $"{p}\\adapter";
            txt_path.Text = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\ts";
        }

        private void Form_Plugin_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Utility.m_OprateLog.TSPath))
            {
                this.txt_custom.Text = Utility.m_OprateLog.TSCustomHospitals;
                this.txt_path.Text = Utility.m_OprateLog.TSSavePath;
                this.txt_tsproject.Text = Utility.m_OprateLog.TSPath;
                this.txt_version.Text = Utility.m_OprateLog.TSVersion;
                this.cb_build.Checked = Utility.m_OprateLog.TSBuild;
                this.cb_custom.Checked = Utility.m_OprateLog.TSCustom;
            }
        }

        private void txt_path_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = @"%HOMEDRIVE%%HOMEPATH%";
                fbd.Description = "请选择保存路径";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    this.txt_path.Text = fbd.SelectedPath;

                    if (!System.IO.Directory.Exists(fbd.SelectedPath))
                    {
                        System.IO.Directory.CreateDirectory(fbd.SelectedPath);
                    }
                }
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_tsproject.Text) || string.IsNullOrEmpty(this.txt_custom.Text) || string.IsNullOrEmpty(this.txt_version.Text))
            {
                return;
            }
            m_TSPath = this.txt_tsproject.Text;
            m_Custom = this.cb_custom.Checked;
            m_CustomHospitals = this.txt_custom.Text;
            m_Path = this.txt_path.Text.Trim();
            m_Version = this.txt_version.Text.Trim();
            m_Build = this.cb_build.Checked;

            Utility.m_OprateLog.TSCustomHospitals = this.txt_custom.Text;
            Utility.m_OprateLog.TSSavePath = this.txt_path.Text;
            Utility.m_OprateLog.TSPath = this.txt_tsproject.Text;
            Utility.m_OprateLog.TSVersion = this.txt_version.Text;
            Utility.m_OprateLog.TSBuild = this.cb_build.Checked;
            Utility.m_OprateLog.TSCustom = this.cb_custom.Checked;
            Utility.SaveOprateLog();
            m_DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void cb_custom_CheckedChanged(object sender, EventArgs e)
        {
            this.txt_custom.Visible = this.cb_custom.Checked;
        }
    }
}
