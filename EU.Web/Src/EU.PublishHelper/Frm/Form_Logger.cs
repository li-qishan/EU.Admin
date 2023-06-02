using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JianLian.HDIS.PublishHelper
{
    public partial class Form_Logger : Form
    {
        DevServer _server = null;
        Hospital _hospital = null;
        string _model = "";
        string form_name = "";
        int count = 1;
        int countlast = 1;
        public Form_Logger(DevServer server, Hospital hospital, string model)
        {
            InitializeComponent();
            _server = server;
            _hospital = hospital;
            _model = model;
            form_name = $"日志 - {server.Ip} - {hospital.FileName}({hospital.DefaultFort}) - {model}";
            Text = form_name;
        }
        private void Form_Logger_Load(object sender, EventArgs e)
        {
            new Thread(new ThreadStart(() =>
            {
                while (!b_close)
                {
                    SetLogger();
                    while (--count > 0 && !b_close)
                    {
                        BeginInvoke(new EventHandler(delegate
                        {
                            this.Text = $"{form_name} - 下一次刷新剩余 {count} 秒...";
                        }));
                        System.Threading.Thread.Sleep(1000);
                    }
                    while (count < 0)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            })).Start();
        }

        private void linkLabel_Refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            count = -1;
            SetLogger();
        }

        object m_Lock = new object();
        bool m_In = false;
        private void SetLogger()
        {
            lock (m_Lock)
            {
                if (m_In)
                {
                    return;
                }
                m_In = true;
            }
            try
            {
                BeginInvoke(new EventHandler(delegate
                {
                    this.Text = $"{form_name} - 正在获取日志...";
                    var res = SshHelper.ExcuteCmd(_server, DockerCommand.GetDockerContainerName(_hospital.FileName, _model));
                    if (res.Success)
                    {
                        res = SshHelper.ExcuteCmd(_server, DockerCommand.GetDockerLogs(res.Result.Trim('\n')));
                        if (res.Success)
                        {
                            var log = res.Result;
                            if (log.Contains("\u001b"))
                            {
                                foreach (System.Text.RegularExpressions.Match item in Utility.m_RegexUb.Matches(log))
                                {
                                    log = log.Replace(item.Value, string.Empty);
                                }
                            }
                            if (this.txt_Log.Text != log)
                                this.txt_Log.Text = log;//.Replace("\n", "\r\n");
                        }
                    }
                    count = countlast + 2;
                    if (count >= 60)
                    {
                        count = 60;
                    }
                    countlast = count;
                }));
            }
            catch { }
            finally
            {
                m_In = false;
            }
        }

        private void txt_Log_TextChanged(object sender, EventArgs e)
        {
            if (txt_Log.Text.Length <= 0)
                return;
            txt_Log.SelectionStart = txt_Log.Text.Length;
            txt_Log.ScrollToCaret();
        }

        bool b_close = false;
        private void Form_Logger_FormClosing(object sender, FormClosingEventArgs e)
        {
            b_close = true;
        }

    }
}
