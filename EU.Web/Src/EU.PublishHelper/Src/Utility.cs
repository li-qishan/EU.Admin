using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace JianLian.HDIS.PublishHelper
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public class Utility
    {
        #region 参数
        /// <summary>
        /// 停止发布
        /// </summary>
        public static bool m_StopPublish = false;
        /// <summary>
        /// 替换控制指令
        /// </summary>
        public static System.Text.RegularExpressions.Regex m_RegexUb = new System.Text.RegularExpressions.Regex(@"\u001b\[\d+[A-z]");
        /// <summary>
        /// 测试服务器
        /// </summary>
        public static string m_TestServerIp = "192.168.8.60";
        #endregion

        #region 事件
        /// <summary>
        /// 记录日志
        /// </summary>
        public static Action<string, string> SendLogHandle;
        public static void SendLog(string msg, bool status = false)
        {
            SendLogHandle("", msg);
            if (status)
            {
                SetStatus(msg);
            }
            lock (m_Logs)
                m_Logs.Add(msg);
        }
        public static void SendLog(string oprate, string msg, bool status = false)
        {
            SendLogHandle(oprate, msg);
            if (status)
            {
                SetStatus(msg);
            }
            lock (m_Logs)
            {
                if (string.IsNullOrEmpty(oprate))
                    m_Logs.Add($"{msg}");
                else
                    m_Logs.Add($"[{oprate}] {msg}");
            }
        }
        /// <summary>
        /// 设置进度条显示/隐藏
        /// </summary>
        public static Action<bool> SetProgressBarVisible;
        /// <summary>
        /// 设置进度条状态
        /// </summary>
        public static Action<long, ulong> SetProgressBarValue;
        /// <summary>
        /// 设置当前状态
        /// </summary>
        public static Action<string> SetStatus;
        #endregion

        #region 初始化
        public static void Init()
        {
            ReadDevServer();
            ReadPublishServer();
            ReadOprateLog();
            Thread log = new Thread(WriteLogger);
            log.Start();
        }
        #endregion

        #region 服务器管理
        /// <summary>
        /// 开发服务器列表
        /// </summary>
        public static List<DevServer> m_DevServers = new List<DevServer>();
        /// <summary>
        /// 保存开发服务器配置
        /// </summary>
        public static void SaveDevServer()
        {
            try
            {
                string fname = AppDomain.CurrentDomain.BaseDirectory + "\\DevServers.dat";
                if (File.Exists(fname))
                    File.Delete(fname);
                using (FileStream fs = new FileStream(fname, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    //将list序列化到文件中
                    bf.Serialize(fs, m_DevServers);
                }
            }
            catch { }
        }

        /// <summary>
        /// 读取开发服务器配置
        /// </summary>
        public static void ReadDevServer()
        {
            try
            {
                string fname = AppDomain.CurrentDomain.BaseDirectory + "\\DevServers.dat";
                if (File.Exists(fname))
                {
                    using (FileStream fs = new FileStream(fname, FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        //将list序列化到文件中
                        m_DevServers = bf.Deserialize(fs) as List<DevServer>;
                    }
                }
            }
            catch { }

            if (m_DevServers is null)
            {
                m_DevServers = new List<DevServer>();
            }

            m_DevServers.ForEach(s =>
            {
                if (s.Hospitals is null)
                {
                    s.Hospitals = new List<Hospital>();
                }
            });
        }
        /// <summary>
        /// 发布服务器列表
        /// </summary>
        public static List<PublishServer> m_PublishServers = new List<PublishServer>();
        /// <summary>
        /// 保存发布服务器配置
        /// </summary>
        public static void SavePublishServer()
        {
            try
            {
                string fname = AppDomain.CurrentDomain.BaseDirectory + "\\PublishServers.dat";
                if (File.Exists(fname))
                    File.Delete(fname);
                using (FileStream fs = new FileStream(fname, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    //将list序列化到文件中
                    bf.Serialize(fs, m_PublishServers);
                }
            }
            catch { }
        }

        /// <summary>
        /// 读取发布服务器配置
        /// </summary>
        public static void ReadPublishServer()
        {
            try
            {
                string fname = AppDomain.CurrentDomain.BaseDirectory + "\\PublishServers.dat";
                if (File.Exists(fname))
                {
                    using (FileStream fs = new FileStream(fname, FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        //将list序列化到文件中
                        m_PublishServers = bf.Deserialize(fs) as List<PublishServer>;
                    }
                }
            }
            catch { }

            if (m_PublishServers is null)
            {
                m_PublishServers = new List<PublishServer>();
            }

            m_PublishServers.ForEach(s =>
            {
                if (s.Folders is null)
                    s.Folders = new List<VersionFolder>();

                s.Folders.ForEach(f =>
                {
                    if (f.Files is null)
                        f.Files = new List<VersionFile>();
                });
            });
        }
        #endregion

        #region 操作记录
        /// <summary>
        /// 操作记录
        /// </summary>
        public static OprateLog m_OprateLog = new OprateLog();
        /// <summary>
        /// 保存操作记录
        /// </summary>
        public static void SaveOprateLog()
        {
            try
            {
                string fname = AppDomain.CurrentDomain.BaseDirectory + "\\OprateLog.dat";
                if (File.Exists(fname))
                    File.Delete(fname);
                using (FileStream fs = new FileStream(fname, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    //将list序列化到文件中
                    bf.Serialize(fs, m_OprateLog);
                }
            }
            catch { }
        }

        /// <summary>
        /// 读取操作记录
        /// </summary>
        public static void ReadOprateLog()
        {
            try
            {
                string fname = AppDomain.CurrentDomain.BaseDirectory + "\\OprateLog.dat";
                if (File.Exists(fname))
                {
                    using (FileStream fs = new FileStream(fname, FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        //将list序列化到文件中
                        m_OprateLog = bf.Deserialize(fs) as OprateLog;
                    }
                }
            }
            catch { }

            if (m_OprateLog is null)
            {
                m_OprateLog = new OprateLog();
            }
        }
        #endregion

        #region Ping测试
        /// <summary>
        /// Ping测试
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="b_log"></param>
        /// <returns></returns>
        public static bool Ping(string ip, bool b_log = true)
        {
            bool b_suc = false;
            try
            {
                using (Ping ping = new Ping())
                {
                    b_suc = ping.Send(ip, 2000).Status == IPStatus.Success;
                }
            }
            catch
            {
                b_suc = false;
            }
            if (!b_suc && b_log)
            {
                SendLog(ip, "ping失败，不继续执行其他远程操作指令");
            }
            return b_suc;
        }
        #endregion

        #region 获取临时文件名
        /// <summary>
        /// 获取临时文件名
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static string GetTempFileName(string fname, bool random = true)
        {
            return $"{System.Environment.GetEnvironmentVariable("TEMP")}\\{fname}" + (random ? $"_{DateTime.Now.Ticks}" : "");
        }
        #endregion

        #region 获取服务器状态
        /// <summary>
        /// 获取服务器状态
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public static (bool Success, double Cpu, double MemTotal, double MemUsed, double Disk) GetServerStatus(Server server)
        {
            var res = (false, 0, 0, 0, 0);
            try
            {
                var (Success, Result) = SshHelper.ExcuteCmd(server, "cpu=`top -b -n 1 | awk 'NR>7{sum+=$9} END {print sum}'`\n mem=`free -m | awk 'NR==2{print $2,$3}'` \n disk=`df -h | grep \"home\" |awk '{{print $5}}'` \n echo \"$cpu,$mem,$disk\"", false);
                if (Success)
                {
                    //87,7981 4369,96%
                    var empties = Result.Trim('\n').Trim().Split(new string[] { "%", ",", " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (empties.Length == 4)
                    {
                        return (true, double.Parse(empties[0]), double.Parse(empties[1]), double.Parse(empties[2]), double.Parse(empties[3]));
                    }
                }
            }
            catch
            { }
            return res;
        }
        #endregion

        #region 日志
        /// <summary>
        /// 停止记录日志
        /// </summary>
        public static bool m_LogRun = true;
        private static List<string> m_Logs = new List<string>();
        /// <summary>
        /// 日志线程
        /// </summary>
        private static void WriteLogger()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Log\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fname = string.Empty;
            var logs = new List<string>();
            while (m_LogRun)
            {
                try
                {
                    lock (m_Logs)
                    {
                        if (m_Logs.Count > 0)
                        {
                            m_Logs.ForEach(s => logs.Add(s));
                            fname = path + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                            m_Logs.Clear();
                        }
                    }
                    if (logs.Count > 0)
                    {
                        WriteLoggerToFile(fname, logs);
                        logs.Clear();
                    }
                }
                catch
                {
                    // ignored
                }
                finally
                {
                    System.Threading.Thread.Sleep(3000);
                }
            }
        }

        /// <summary>
        /// 日志写入文件
        /// </summary>
        /// <param name="fname">文件名</param>
        /// <param name="logs">日志</param>
        private static void WriteLoggerToFile(string fname, List<string> logs)
        {
            StringBuilder sb = new StringBuilder();
            logs.ForEach(s => sb.AppendLine(DateTime.Now.ToString("HH:mm:ss") + "\t" + s));
            if (!File.Exists(fname))
            {
                using (StreamWriter sw = File.CreateText(fname))
                {
                    sw.Write(sb.ToString());
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(fname))
                {
                    sw.Write(sb.ToString());
                }
            }
        }
        #endregion

        #region 发布与打包锁
        /// <summary>
        /// 查询锁定文件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="hospital"></param>
        /// <param name="oprate"></param>
        /// <returns></returns>
        public static bool LockDevFiles(Server server, string oprate)
        {
            bool b_in = false;
            string lfname = GetTempFileName("lockfile");
            string rfname = $"/tmp/lockfile";
            if (SftpHelper.DownloadFile(server, rfname, lfname))
            {
                if (File.Exists(lfname))
                {
                    SendLog(File.ReadAllText(lfname, Encoding.UTF8));
                    b_in = true;
                }
            }
            else
            {
                File.WriteAllText(lfname, $"当前服务器 {server.Ip} 繁忙。用户 [{Environment.MachineName}] 正在 {oprate}，请等待 [{Environment.MachineName}] 操作完毕...", Encoding.UTF8);
                SftpHelper.UploadFile(server, lfname, rfname);
            }
            return b_in;
        }

        /// <summary>
        /// 删除锁定文件
        /// </summary>
        /// <param name="server"></param>
        public static void RemoveDevFiles(Server server)
        {
            SftpHelper.Delete(server, $"/tmp/lockfile");
        }
        #endregion

        #region HTTP模板
        public static CookieContainer m_Cookies = new CookieContainer();
        public enum Compression
        {
            GZip,
            Deflate,
            None,
        }

        /// <summary>
        /// 获取HttpWebRequest模板
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postdata">POST</param>
        /// <param name="cookies">Cookies</param>
        /// <returns></returns>
        public static HttpWebRequest GetHttpRequest(string url, string postdata, CookieContainer cookies, string header = null)
        {
            HttpWebRequest request = WebRequest.Create(new Uri(url)) as HttpWebRequest;

            request.CookieContainer = cookies;
            request.ContentType = !string.IsNullOrEmpty(header) ? "application/json" : "application/x-www-form-urlencoded";
            request.ServicePoint.ConnectionLimit = 300;
            ServicePointManager.Expect100Continue = false;
            request.Referer = url;
            request.Accept = "*/*";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
            request.AllowAutoRedirect = true;
            if (!string.IsNullOrEmpty(header))
                request.Headers.Add("Authorization", header);
            if (postdata != null && postdata != "")
            {
                request.Method = "POST";
                byte[] byte_post = Encoding.Default.GetBytes(postdata);
                request.ContentLength = byte_post.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(byte_post, 0, byte_post.Length);
                }
            }
            else
            {
                request.Method = "GET";
            }
            return request;
        }

        /// <summary>
        /// 提取HttpWebResponse文本内容
        /// </summary>
        /// <param name="resp">HttpWebResponse响应包</param>
        /// <returns></returns>
        public static string GetResponseContent(HttpWebResponse resp)
        {

            if (resp.StatusCode != HttpStatusCode.OK)
                throw new Exception("远程服务器返回状态码: " + resp.StatusCode);

            Encoding enc = Encoding.UTF8;
            if (resp.CharacterSet != null && resp.CharacterSet != "")
                enc = Encoding.GetEncoding(resp.CharacterSet);

            Compression comp = Compression.None;
            if (resp.ContentEncoding != null && resp.ContentEncoding.Trim().ToUpper() == "GZIP")
                comp = Compression.GZip;
            else if (resp.ContentEncoding != null && resp.ContentEncoding.Trim().ToUpper() == "DEFLATE")
                comp = Compression.Deflate;

            MemoryStream ms = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(ms, enc))
            {
                StreamReader sr;
                switch (comp)
                {
                    case Compression.GZip:
                        sr = new StreamReader(new System.IO.Compression.GZipStream(resp.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress), enc);
                        break;
                    case Compression.Deflate:
                        sr = new StreamReader(new System.IO.Compression.DeflateStream(resp.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress), enc);
                        break;
                    default:
                        sr = new StreamReader(resp.GetResponseStream(), enc);
                        break;
                }

                while (!sr.EndOfStream)
                {
                    char[] buf = new char[16000];
                    int read = sr.ReadBlock(buf, 0, 16000);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(buf, 0, read);
                    sw.Write(buf, 0, read);
                }
                sr.Close();
            }

            byte[] mbuf = ms.GetBuffer();
            string sbuf = enc.GetString(mbuf);
            return sbuf;
        }

        /// <summary>
        /// 获取HttpWebRequest返回值
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postdata">PostData</param>
        /// <returns></returns>
        public static string GetHttpResult(string url, string postdata, string header = null)
        {
            try
            {
                HttpWebRequest request = GetHttpRequest(url, postdata, m_Cookies, header);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                m_Cookies.Add(response.Cookies);

                return GetResponseContent(response);
            }
            catch
            {
                //Utilities.sendLog("连接 " + url + " 失败" + ex.Message);
                return "";
            }
        }


        /// <summary>
        /// 获取HttpWebRequest返回值
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="postdata">PostData</param>
        /// <returns></returns>
        public static Stream GetHttpResponse(string url, string postdata)
        {
            try
            {
                HttpWebRequest request = GetHttpRequest(url, postdata, m_Cookies);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                return response.GetResponseStream();
            }
            catch 
            {
                //Utilities.sendLog("连接 " + url + " 失败" + ex.Message);
                return null;
            }
        }

        ///   <summary>   
        ///   去除HTML标记   
        ///   </summary>   
        ///   <param   name="NoHTML">包括HTML的源码 </param>   
        ///   <returns>已经去除后的文字</returns>   
        public static string RemoveHTMLFlag(string input)
        {
            input = Regex.Replace(input, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            input = regex.Replace(input, "");
            input = Regex.Replace(input, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"-->", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"<!--.*", "", RegexOptions.IgnoreCase);

            input = Regex.Replace(input, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            input.Replace("<", "");
            input.Replace(">", "");
            input.Replace("\r\n", "");

            return input;
        }
        #endregion
    }
}
