using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JianLian.HDIS.PublishHelper.Src
{
    /// <summary>
    /// CmdHelper
    /// </summary>
    public class CmdHelper
    {
        /// <summary>
        /// 指令Cmd命令
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static (bool, string) ExecCmd(string str)
        {
            string msg = string.Empty;
            bool hasError = false;
            bool completed = false;
            string cmdCur = string.Empty;
            using (Process m_Cmd = new Process())
            {
                m_Cmd.StartInfo.FileName = "cmd.exe";
                m_Cmd.StartInfo.WorkingDirectory = ".";
                m_Cmd.StartInfo.UseShellExecute = false;
                m_Cmd.StartInfo.RedirectStandardInput = true;
                m_Cmd.StartInfo.RedirectStandardOutput = true;
                m_Cmd.StartInfo.CreateNoWindow = true;
                m_Cmd.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        var res = e.Data;
                        if (res.Contains("\u001b"))
                        {
                            foreach (System.Text.RegularExpressions.Match item in Utility.m_RegexUb.Matches(res))
                            {
                                res = res.Replace(item.Value, "");
                            }
                        }
                        Utility.SendLog("", res);
                        if (res.Trim() != "Unable to use package assets cache due to I/O error. This can occur when the same project is built more than once in parallel. Performance may be degraded, but the build result will not be impacted."
                            && (res.Contains("ERR!") || res.Contains("ERROR") || res.Contains("error")))
                        {
                            msg = res;
                            hasError = true;
                        }

                        if (res.EndsWith("exit"))
                        {
                            completed = true;
                        }
                    }
                });
                m_Cmd.Start();
                m_Cmd.BeginOutputReadLine();

                str.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList()
                    .ForEach(s =>
                    {
                        if (hasError)
                            return;
                        cmdCur = s;
                        m_Cmd.StandardInput.WriteLine(cmdCur);
                    });

                int timecout = 60 * 15;
                while (!completed && !hasError)
                {
                    System.Threading.Thread.Sleep(1000);
                    if (--timecout < 0)
                    {
                        msg = $"指令执行超时：{str}";
                        hasError = true;
                        break;
                    }
                }
            }
            return (hasError, msg);
        }

    }
}
