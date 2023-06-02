using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianLian.HDIS.PublishHelper
{
    /// <summary>
    /// SSHHelper
    /// </summary>
    public class SshHelper
    {
        #region 执行命令
        /// <summary>
        /// SSH执行命令返回结果
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="command">命令</param>
        /// <param name="result">执行结果</param>
        /// <param name="b_log">打印日志</param>
        /// <returns></returns>
        public static (bool Success, string Result) ExcuteCmd(Server server, string command, bool b_log = false)
        {
            bool b_suc = false;
            string result = string.Empty;
            try
            {
                //if (!Utility.Ping(server.Ip, b_log))
                //{
                //    return (b_suc, result);
                //}

                using (SshClient ssh = new SshClient(server.Ip, server.Port, "root", server.SuPassword))
                {
                    ssh.Connect();
                    if (b_log)
                    {
                        Utility.SendLog(command);
                    }
                    var cmd = ssh.RunCommand(command);
                    if (cmd.ExitStatus != 0)
                    {
                        result = cmd.Error;
                    }
                    else
                    {
                        b_suc = true;
                        result = $"{cmd.Result}{cmd.Error}";
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            if (b_log)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result
                          .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                          .ToList()
                          .ForEach(
                          s =>
                          {
                              if (!string.IsNullOrEmpty(s.Trim()))
                                  Utility.SendLog(s);
                          });
                }
                else
                {
                    Utility.SendLog("success");
                }
            }
            return (b_suc, result);
        }

        /// <summary>
        /// SSH执行批量命令
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="commands">命令</param>
        /// <param name="logFlag">日志标识</param>
        /// <param name="action">完成以后的操作</param>
        public static void ExcuteCmds(Server server, List<string> commands, string logFlag, Action completedAction = null)
        {
            string result = string.Empty;
            try
            {
                Utility.SetProgressBarVisible(true);
                Utility.SendLog(logFlag, "开始执行批量指令", true);
                if (!Utility.Ping(server.Ip))
                {
                    return;
                }

                using (SshClient ssh = new SshClient(server.Ip, server.Port, "root", server.SuPassword))
                {
                    ssh.Connect();
                    int index = 0;
                    if (commands != null)
                    {
                        Utility.SetProgressBarValue(commands.Count, 0);
                        commands.ForEach(command =>
                        {
                            Utility.SendLog(logFlag, $"{command}");
                            var cmd = ssh.RunCommand(command);
                            if (cmd.ExitStatus != 0)
                            {
                                result = cmd.Error;
                            }
                            else
                            {
                                result = $"{cmd.Result}{cmd.Error}";
                            }
                            if (!string.IsNullOrEmpty(result))
                            {
                                result
                                      .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                      .ToList()
                                      .ForEach(
                                      s =>
                                      {
                                          if (!string.IsNullOrEmpty(s.Trim()))
                                              Utility.SendLog(logFlag, s);
                                      });
                            }
                            else
                            {
                                Utility.SendLog(logFlag, "success");
                            }
                            Utility.SetProgressBarValue(commands.Count, (ulong)++index);
                        });
                    }
                }
                completedAction?.Invoke();
            }
            catch (Exception ex)
            {
                Utility.SendLog(logFlag, $"{ex.Message}");
            }
            Utility.SetProgressBarVisible(false);
            Utility.SendLog(logFlag, "执行批量指令完毕", true);
        }
        #endregion

        #region 执行命令实时返回结果
        public static bool m_InRtm = false;
        private static object m_LockRtm = new object();
        /// <summary>
        /// 执行命令实时返回结果
        /// </summary>
        /// <param name="server"></param>
        /// <param name="command"></param>
        public static void ExcuteCmdRtm(Server server, string command)
        {
            lock (m_LockRtm)
            {
                if (m_InRtm)
                {
                    Utility.SendLog("执行脚本", $"正在执行其他命令，请等待");
                    return;
                }
                else
                {
                    m_InRtm = true;
                }
            }
            try
            {
                if (!Utility.Ping(server.Ip))
                {
                    return;
                }
                using (var client = new SshClient(server.Ip, server.Port, "root", server.SuPassword))
                {
                    client.Connect();
                    Utility.SendLog("登录", $"[{server.Ip}]成功");
                    using (var stream = client.CreateShellStream("anything", 80, 24, 800, 600, 4096))
                    {
                        byte[] buffer = new byte[1000];
                        stream.WriteLine("pwd");

                        stream.BeginRead(buffer, 0, buffer.Length, null, null);

                        bool end = false;
                        stream.DataReceived += new EventHandler<Renci.SshNet.Common.ShellDataEventArgs>(
                            (o, ex) =>
                            {
                                var str = stream.Read();
                                if (str.Contains("\u001b"))
                                {
                                    foreach (System.Text.RegularExpressions.Match item in Utility.m_RegexUb.Matches(str))
                                    {
                                        str = str.Replace(item.Value, "");
                                    }
                                }
                                str
                                .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .ToList()
                                .ForEach(
                                s =>
                                {
                                    if (!string.IsNullOrEmpty(s.Trim()))
                                        Utility.SendLog("", s);
                                });
                                end = str.Trim().EndsWith($"ihdis]#");
                            }
                        );
                        command = $"{command};cd /home/{server.UserName}/ihdis;".Replace(";;", ";");
                        Utility.SendLog("执行脚本", $"{command}");
                        stream.WriteLine(command);
                        int timecout = 5 * 60;
                        while (!end)
                        {
                            System.Threading.Thread.Sleep(1000);
                            if (--timecout < 0)
                                break;
                        }
                        Utility.SendLog("执行脚本", $"[{server.Ip}]完毕");
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.SendLog("执行脚本", $"{ex}");
            }
            finally
            {
                lock (m_LockRtm)
                {
                    m_InRtm = false;
                }
            }
        }

        /// <summary>
        /// 是否繁忙
        /// </summary>
        /// <returns></returns>
        public static bool InRtmNow()
        {
            return m_InRtm;
        }
        #endregion
    }
}
