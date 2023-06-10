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
    public partial class Form_Publish : Form
    {
        #region 初始化
        DevServer m_DevServer = null;
        Hospital m_Hospital = null;
        Hospital m_SelectHospital = null;
        public DialogResult m_DialogResult = DialogResult.No;
        public Form_Publish(DevServer devServer, Hospital hospital)
        {
            InitializeComponent();
            m_DevServer = devServer;
            m_SelectHospital = hospital;
        }

        private void Form_Publish_Load(object sender, EventArgs e)
        {
            this.cmb_Dev.Items.AddRange(Utility.m_DevServers.Where(o => o.Ip != Utility.m_TestServerIp).Select(o => o.Ip).ToArray());
            this.cmb_Publish.Items.AddRange(Utility.m_PublishServers.Select(o => o.Ip).ToArray());
            if (cmb_Publish.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(Utility.m_OprateLog.PackPublishIp) && Utility.m_PublishServers.Any(o => o.Ip == Utility.m_OprateLog.PackPublishIp))
                {
                    cmb_Publish.Text = Utility.m_OprateLog.PackPublishIp;
                }
                else
                {
                    cmb_Publish.Text = cmb_Publish.Items[0].ToString();
                }
            }
            if (cmb_Dev.Items.Count > 0)
            {
                if (!(m_DevServer is null))
                {
                    cmb_Dev.Text = m_DevServer.Ip;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Utility.m_OprateLog.PackDevIp) && Utility.m_DevServers.Any(o => o.Ip == Utility.m_OprateLog.PackDevIp))
                    {
                        cmb_Dev.Text = Utility.m_OprateLog.PackDevIp;
                    }
                    else
                    {
                        cmb_Dev.Text = cmb_Dev.Items[0].ToString();
                    }
                }
            }
            this.cb_Upgrade.Checked = Utility.m_OprateLog.PackUpgrade;
            this.cb_TS.Checked = Utility.m_OprateLog.PackUpgradeTS;
            this.cb_Git.Checked = Utility.m_OprateLog.PackGit;
            this.cb_Download.Checked = Utility.m_OprateLog.PackDownload;

            var tspath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"assistant\JianLian.Assistant\JianLian.HDIS.PublishHelper\bin\Debug\", "") + @"adapter\JianLian.Adapter.Center";
            this.cmb_TS.Items.AddRange(System.IO.Directory.GetDirectories($"{tspath}\\Custom").Select(d => d.Substring(d.LastIndexOf('.') + 1)).ToArray());
            this.cmb_TS.Text = this.cmb_TS.Items[0].ToString();

            if (!string.IsNullOrEmpty(Utility.m_OprateLog.PackUpgradeTSCustom))
            {
                this.cmb_TS.Text = Utility.m_OprateLog.PackUpgradeTSCustom;
            }
            this.cmb_TS.Visible = cb_TS.Checked;
        }

        private void cb_TS_CheckedChanged(object sender, EventArgs e)
        {
            this.cmb_TS.Visible = cb_TS.Checked;
        }
        #endregion

        #region 开发服务器、医院、发布服务器变更
        bool b_first = true;
        private void cmb_Dev_SelectedIndexChanged(object sender, EventArgs e)
        {

            //医院
            var hospitals = Utility.m_DevServers.Where(o => o.Ip == cmb_Dev.Text).FirstOrDefault()?.Hospitals;
            cmb_Hospital.Items.Clear();
            if (!(hospitals is null))
            {
                cmb_Hospital.Items.AddRange(hospitals.Select(o => o.FileName).ToArray());
            }
            if (cmb_Hospital.Items.Count > 0)
            {
                cmb_Hospital.Text = cmb_Hospital.Items[0].ToString();
            }
            if (b_first)
            {
                b_first = false;

                if (!string.IsNullOrEmpty(Utility.m_OprateLog.PackVersion))
                {
                    this.txt_version.Text = Utility.m_OprateLog.PackVersion;
                }

                if (!string.IsNullOrEmpty(Utility.m_OprateLog.PackHospital) && !(hospitals is null) && hospitals.Any(o => o.FileName == Utility.m_OprateLog.PackHospital))
                {
                    cmb_Hospital.Text = Utility.m_OprateLog.PackHospital;
                }

                if (!(m_SelectHospital is null) && !(hospitals is null) && hospitals.Any(o => o.FileName == m_SelectHospital.FileName))
                {
                    cmb_Hospital.Text = m_SelectHospital.FileName;
                }
                this.cb_Publish.Checked = Utility.m_OprateLog.PackPublish;
                this.cb_CreateDbFile.Checked = Utility.m_OprateLog.PackDbFile;
            }

            var mask = this.cmb_Dev.Text.Substring(0, this.cmb_Dev.Text.LastIndexOf('.') + 1);

            //发布服务器
            foreach (var item in cmb_Publish.Items)
            {
                if (item.ToString().StartsWith(mask))
                {
                    cmb_Publish.Text = item.ToString();
                    break;
                }
            }
        }

        private void cmb_Hospital_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeVersionText();
        }

        private void cmb_Publish_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeVersionText();
        }

        private void ChangeVersionText()
        {
            m_Hospital = Utility.m_DevServers.Where(o => o.Ip == cmb_Dev.Text).FirstOrDefault()?.Hospitals?.Where(o => o.FileName == cmb_Hospital.Text).FirstOrDefault();
            if (m_Hospital is null)
            {
                return;
            }
            var lastVersion = Utility.m_PublishServers
              .Where(o => o.Ip == this.cmb_Publish.Text)
              .FirstOrDefault()
              ?.Folders
              ?.Where(o => o.FolderName == m_Hospital?.FileName)
              .FirstOrDefault()
              ?.Files
              .OrderByDescending(o => o.CreateTime)
              .FirstOrDefault()
              ?.FileName
              .Replace(".zip", "");
            if (!string.IsNullOrEmpty(lastVersion))
            {
                string[] empites = lastVersion.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (empites.Length >= 4)
                {
                    var ver = "1";
                    if (empites[3].Contains("_"))
                    {
                        try
                        {
                            ver = empites[3].Substring(0, empites[3].IndexOf("_"));
                            if (DateTime.Now.AddDays(-1).ToString("yyMMdd") == empites[2])
                            {
                                ver = (Convert.ToInt32(empites[3].Substring(0, empites[3].IndexOf("_"))) + 1).ToString();
                            }
                        }
                        catch
                        {

                        }
                    }
                    this.txt_version.Text = $"{empites[0]}.{empites[1]}.{DateTime.Now:yyMMdd}.{ver}_R";
                }
            }
            else
            {
                this.txt_version.Text = $"3.0.{DateTime.Now:yyMMdd}.1_R";
            }
        }
        #endregion

        #region 确定/取消
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.cmb_Dev.Text)
                || string.IsNullOrEmpty(this.cmb_Hospital.Text)
                || string.IsNullOrEmpty(this.cmb_Publish.Text)
                || string.IsNullOrEmpty(this.txt_version.Text))
            {
                return;
            }
            if (this.cmb_Dev.Text.Substring(0, this.cmb_Dev.Text.LastIndexOf('.') + 1) != this.cmb_Publish.Text.Substring(0, this.cmb_Publish.Text.LastIndexOf('.') + 1))
            {
                if (MessageBox.Show("开发服务器与发布服务器不是一个网段，是否确定？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            if (!Utility.m_DevServers.Any(o => o.Ip == cmb_Dev.Text))
            {
                MessageBox.Show("开发服务器不存在，请先配置", "提示");
                return;
            }
            if (!Utility.m_PublishServers.Any(o => o.Ip == cmb_Publish.Text))
            {
                MessageBox.Show("发布服务器不存在，请先配置", "提示");
                return;
            }
            if (Utility.m_OprateLog.PackUpgrade && Utility.m_OprateLog.PackUpgradeTS && string.IsNullOrEmpty(Utility.m_OprateLog.PackUpgradeTSCustom))
            {
                MessageBox.Show("TS升级包，必须包含指定组件", "提示");
                return;
            }

            Utility.m_OprateLog.PackDevIp = this.cmb_Dev.Text;
            Utility.m_OprateLog.PackHospital = this.cmb_Hospital.Text;
            Utility.m_OprateLog.PackPublishIp = this.cmb_Publish.Text;
            Utility.m_OprateLog.PackVersion = this.txt_version.Text;
            Utility.m_OprateLog.PackDbFile = this.cb_CreateDbFile.Checked;
            Utility.m_OprateLog.PackPublish = this.cb_Publish.Checked;
            Utility.m_OprateLog.PackUpgrade = this.cb_Upgrade.Checked;
            Utility.m_OprateLog.PackUpgradeTS = this.cb_TS.Checked;
            Utility.m_OprateLog.PackUpgradeTSCustom = this.cmb_TS.Text;
            Utility.m_OprateLog.PackGit = this.cb_Git.Checked;
            Utility.m_OprateLog.PackDownload = this.cb_Download.Checked;
            Utility.SaveOprateLog();
            m_DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 升级包
        private void cb_Upgrade_CheckedChanged(object sender, EventArgs e)
        {
            this.cb_TS.Visible = this.cb_Upgrade.Checked;
            this.cmb_TS.Visible = this.cb_Upgrade.Checked;
        }
        #endregion
    }
}
