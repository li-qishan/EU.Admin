using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JianLian.HDIS.PublishHelper
{
    public partial class Form_UpgradeLog : Form
    {
        #region 初始化
        public Form_UpgradeLog()
        {
            InitializeComponent();
            this.webBrowser_Upgrade.ScriptErrorsSuppressed = true;
            this.webBrowser_Upgrade.DocumentCompleted += WebBrowser_Upgrade_DocumentCompleted;
            this.webBrowser_Upgrade.StatusTextChanged += WebBrowser_Upgrade_StatusTextChanged;
            this.webBrowser_Upgrade.NewWindow += WebBrowser_Upgrade_NewWindow;
        }

        private void WebBrowser_Upgrade_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            try
            {
                this.webBrowser_Upgrade.Navigate(this.webBrowser_Upgrade.Document.ActiveElement.GetAttribute("href"));
            }
            catch { }
        }

        private void WebBrowser_Upgrade_StatusTextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(webBrowser_Upgrade.StatusText))
                return;
            this.tssl_status.Text = webBrowser_Upgrade.StatusText;
        }

        public static Dictionary<string, string> ParseQueryString(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException("url");
            }
            var uri = new Uri(url);
            if (string.IsNullOrWhiteSpace(uri.Query))
            {
                return new Dictionary<string, string>();
            }
            //1.去除第一个前导?字符
            var dic = uri.Query.Substring(1)
                    //2.通过&划分各个参数
                    .Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                    //3.通过=划分参数key和value,且保证只分割第一个=字符
                    .Select(param => param.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries))
                    //4.通过相同的参数key进行分组
                    .GroupBy(part => part[0], part => part.Length > 1 ? part[1] : string.Empty)
                    //5.将相同key的value以,拼接
                    .ToDictionary(group => group.Key, group => string.Join(",", group));

            return dic;
        }

        private void WebBrowser_Upgrade_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //if (webBrowser_Upgrade.DocumentTitle == "登录-TAPD")
            //{
            //    foreach (HtmlElement element in webBrowser_Upgrade.Document.Links)
            //    {
            //        if (element != null && !string.IsNullOrEmpty(element.OuterHtml) && element.OuterHtml.Contains("wx-login-btn"))
            //        {
            //            element.InvokeMember("click");
            //            return;
            //        }
            //    }
            //}
            //else
            {
                var ver = string.Empty;
                StringBuilder sb = new StringBuilder();
                foreach (HtmlElement element in webBrowser_Upgrade.Document.All)
                {
                    try
                    {
                        var content = element.InnerHtml;
                        if (string.IsNullOrEmpty(ver) && !string.IsNullOrEmpty(content) && content.Contains("<span class=\"inner-name\">"))
                        {
                            int verStart = content.IndexOf("<span class=\"inner-name\">");
                            string verContent = content.Substring(verStart + 25);
                            ver = verContent.Substring(0, verContent.IndexOf("<"));
                            sb.AppendLine($"## {ver}");
                            sb.AppendLine();
                            sb.AppendLine($"`{DateTime.Now:yyyy-MM-dd}`");
                            sb.AppendLine();
                        }
                        if (string.IsNullOrEmpty(ver) && !string.IsNullOrEmpty(content) && content.Contains("<span class=\"j-iteration-detail__name iteration-detail__name\"><a title=\""))
                        {
                            int verStart = content.IndexOf("<span class=\"j-iteration-detail__name iteration-detail__name\"><a title=\"");
                            string verContent = content.Substring(verStart + 72);
                            ver = verContent.Substring(0, verContent.IndexOf("\""));
                            sb.AppendLine($"## {ver}");
                            sb.AppendLine();
                            sb.AppendLine($"`{DateTime.Now:yyyy-MM-dd}`");
                            sb.AppendLine();
                        }
                        int sumEnd = 0;
                        while (!string.IsNullOrEmpty(content))
                        {
                            if (content.Trim().StartsWith("</table>"))
                                content = content.Substring(9);

                            if (content.IndexOf("<TABLE") > -1 && webBrowser_Upgrade.Document.Url.ToString().Contains("releases/get_story_list/"))
                            {
                                int start = content.IndexOf("<TABLE");
                                int end = content.IndexOf("</TABLE>");
                                if (start >= 0 && end >= 0)
                                {
                                    var str = content.Trim().Substring(start, end - start + 8);

                                    Regex reg = new Regex(@"(?is)<a[^>]*?href=(['""\s]?)(?<href>[^'""\s]*)\1[^>]*?>");
                                    MatchCollection match = reg.Matches(str);
                                    foreach (Match m in match)
                                    {
                                        string mValue = m.Value;
                                        int start1 = mValue.IndexOf("title=");
                                        int end1 = mValue.IndexOf("class=\"j-story-title-link");
                                        if (start1 >= 0 && end1 >= 0)
                                        {
                                            var name = mValue.Trim().Substring(start1, end1);
                                            name = name.Replace("title=", null);
                                            name = name.Replace("class=\"j-story-title-link ", null);
                                            name = name.Replace("nameco", null);

                                            string url = m.Groups["href"].Value;
                                            if (url.Length > 7)
                                            {
                                                start1 = url.IndexOf("href=\"");
                                                end1 = url.IndexOf("?url_cache_key");
                                                url = url.Substring(0, end1);

                                                Uri uri = new Uri(url);
                                                string id = uri.Segments[uri.Segments.Length - 1];

                                                id = id.Substring(id.Length - 7);

                                                //sb.AppendLine($"- :{(url.Contains("bugs") ? "bug" : "sparkles")}: {name} ({url})");
                                                sb.AppendLine($"- :{(url.Contains("bugs") ? "bug" : "sparkles")}: {name} [#{id}]({url})");
                                                sb.AppendLine();
                                            }

                                            //Response.Write(m.Groups["href"].Value + "<br/>");
                                        }

                                    }

                                    sumEnd += end;
                                    content = element.InnerHtml.Substring(sumEnd + 8);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else if (content.IndexOf("<TBODY id=bugs-list-tbody>") > -1 && webBrowser_Upgrade.Document.Url.ToString().Contains("releases/release_tab_bugs_list"))
                            {
                                int start = content.IndexOf("<TBODY id=bugs-list-tbody>");
                                int end = content.IndexOf("</TBODY>");
                                if (start >= 0 && end >= 0)
                                {
                                    var str = content.Trim().Substring(start, end - start + 8);

                                    //Regex reg = new Regex("<TR.*?>(.*?)</TR>");
                                    Regex reg = new Regex("<TR.*?>[\\s\\S]*?</TR>");
                                    MatchCollection match = reg.Matches(str);
                                    foreach (Match m in match)
                                    {
                                        string mValue = m.Value;
                                        Regex reg1 = new Regex("<TD data-editable=\"text\".*?>[\\s\\S]*?</TD>");
                                        MatchCollection match1 = reg1.Matches(mValue);
                                        foreach (Match m1 in match1)
                                        {
                                            string m1Value = m.Value;
                                            var name = "";
                                            int start1 = m1Value.IndexOf("growing-title-inner>");
                                            int end1 = m1Value.IndexOf("data-editable-field=\"version_report\"");
                                            if (start1 >= 0 && end1 >= 0)
                                            {
                                                name = m1Value.Trim().Substring(start1, end1);
                                                name = name.Replace("growing-title-inner>", null);
                                                name = name.Replace("data-editable-field=\"version_report\"", null);
                                            }

                                            reg = new Regex(@"(?is)<a[^>]*?href=(['""\s]?)(?<href>[^'""\s]*)\1[^>]*?>");
                                            match = reg.Matches(name);
                                            foreach (Match m2 in match)
                                            {

                                                start1 = m2.Value.IndexOf("<A title=");
                                                end1 = m2.Value.IndexOf("class=editable-value");
                                                if (start1 >= 0 && end1 >= 0)
                                                {
                                                    name = m2.Value.Trim().Substring(start1, end1);
                                                    name = name.Replace("<A title=", null);
                                                    name = name.Replace("class=editable-value", null);

                                                    start1 = m2.Value.IndexOf("href=\"");
                                                    end1 = m2.Value.IndexOf("&amp;url_cache_key=");
                                                    if (start1 >= 0 && end1 >= 0)
                                                    {
                                                        string url = m2.Value.Substring(start1, end1 - start1);
                                                        url = url.Replace("href=\"", null);
                                                        url = url.Replace("target=\"\"", null);

                                                        Uri uri = new Uri(url);
                                                        string queryString = uri.Query;
                                                        string id = queryString.Replace("?bug_id=", null);

                                                        id = id.Substring(id.Length - 7);

                                                        //sb.AppendLine($"- :{(url.Contains("bugs") ? "bug" : "sparkles")}: {name} ({url})");
                                                        sb.AppendLine($"- :{(url.Contains("bugs") ? "bug" : "sparkles")}: {name} [#{id}]({url})");
                                                        sb.AppendLine();
                                                    }
                                                }
                                            }

                                            //    start1 = m1Value.IndexOf("href=\"http");
                                            //end1 = m1Value.IndexOf("target=");
                                            //if (start1 >= 0 && end1 >= 0)
                                            //{
                                            //    url = m1Value.Trim().Substring(start1, end1);
                                            //    url = name.Replace("\"", null);
                                            //    url = name.Replace("\"", null);
                                            //}
                                        }
                                    }

                                    //Regex reg = new Regex(@"(?is)<a[^>]*?href=(['""\s]?)(?<href>[^'""\s]*)\1[^>]*?>[^'""\s]*</a>");
                                    reg = new Regex(@"(?is)<a[^>]*?href=(['""\s]?)(?<href>[^'""\s]*)\1[^>]*?>");
                                    match = reg.Matches(str);
                                    foreach (Match m in match)
                                    {
                                        string mValue = m.Value;
                                        int start1 = mValue.IndexOf("data-editable-value=");
                                        int end1 = mValue.IndexOf("data-editable-field=");
                                        if (start1 >= 0 && end1 >= 0)
                                        {
                                            var name = mValue.Trim().Substring(start1, end1);
                                            name = name.Replace("\"", null);
                                            name = name.Replace("\"", null);

                                            string url = m.Groups["href"].Value;
                                            if (url.Length > 7)
                                            {
                                                //string id = url.Substring(url.Length - 7);

                                                sb.AppendLine($"- :{(url.Contains("bugs") ? "bug" : "sparkles")}: {name} ({url})");
                                                //sb.AppendLine($"- :{(url.Contains("bugs") ? "bug" : "sparkles")}: {name} [#{id}]({url})");
                                                sb.AppendLine();
                                            }

                                            //Response.Write(m.Groups["href"].Value + "<br/>");
                                        }

                                    }

                                    sumEnd += end;
                                    content = element.InnerHtml.Substring(sumEnd + 8);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                int start = content.IndexOf("<table");
                                int end = content.IndexOf("</table>");
                                if (start >= 0 && end >= 0)
                                {
                                    var str = content.Trim().Substring(start, end - start + 8);
                                    string[] empties = str.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                    for (int i = 0; i < empties.Length; ++i)
                                    {
                                        if (!empties[i].Trim().StartsWith("<a title=\"") || !empties[i].Contains("editable"))
                                            continue;
                                        string[] datas = empties[i].Contains("bugs") ? empties[i].Split(new string[] { "\"", "&" }, StringSplitOptions.RemoveEmptyEntries) : empties[i].Split(new string[] { "\"", "&", "?" }, StringSplitOptions.RemoveEmptyEntries);
                                        string url = datas.ToList().Where(d => d.StartsWith("https://www.tapd.cn/")).FirstOrDefault();

                                        if (string.IsNullOrEmpty(url) || (!url.Contains("stories/view") && !url.Contains("bugs/view")))
                                        {
                                            continue;
                                        }
                                        string name = datas[1].Trim();
                                        string id = url.Substring(url.Length - 7);
                                        sb.AppendLine($"- :{(url.Contains("bugs") ? "bug" : "sparkles")}: {name} [#{id}]({url})");
                                        sb.AppendLine();
                                    }
                                    sumEnd += end;
                                    content = element.InnerHtml.Substring(sumEnd + 8);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        if (sb.Length > 0)
                        {
                            this.txt_content.Text = sb.ToString();
                            return;
                        }
                    }
                    catch (Exception ex) { }
                }
            }
        }
        #endregion

        #region 拷贝与刷新
        private void ll_Copy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txt_content.Text))
                return;
            try
            {
                Clipboard.SetDataObject(this.txt_content.Text, true);
            }
            catch
            {
            }
        }

        private void ll_Refresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.webBrowser_Upgrade.Refresh();
        }
        #endregion

        #region 清除IE缓存
        private enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [DllImport("shell32.dll")]
        private static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, ShowCommands nShowCmd);

        private void ll_Clear_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShellExecute(IntPtr.Zero, "open", "rundll32.exe", " InetCpl.cpl,ClearMyTracksByProcess 255", "", ShowCommands.SW_HIDE);
        }
        #endregion

        bool b_autorefresh = false;
        private void cb_Refresh_CheckedChanged(object sender, EventArgs e)
        {
            b_autorefresh = true;
        }
    }
}
