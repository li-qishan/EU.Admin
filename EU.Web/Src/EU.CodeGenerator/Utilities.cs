using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.CodeGenerator
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public class Utilities
    {
        #region 参数
        /// <summary>
        /// 后台代码基础路径
        /// </summary>
        public static string BasePath = string.Empty;
        public static string FileName = string.Empty;

        #endregion



        #region 生成文件
        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="fname">文件名称</param>
        /// <param name="content">内容</param>
        /// <param name="modify">是否更新</param>
        public static void CreateFile(string fname, string content, bool modify = true)
        {
            Encoding encoding = Encoding.UTF8;
            if (File.Exists(fname))
            {
                if (!modify)
                    return;
                string fcontent = "";
                using (StreamReader sr = new StreamReader(fname, encoding))
                {
                    fcontent = sr.ReadToEnd();
                }
                if (fcontent == content)
                {
                    return;
                }
                File.WriteAllText(fname, content, encoding);
                Console.WriteLine("修改文件 " + fname);
            }
            else
            {
                File.WriteAllText(fname, content, encoding);
                Console.WriteLine("新增文件 " + fname);
            }
        }
        #endregion
    }
}
