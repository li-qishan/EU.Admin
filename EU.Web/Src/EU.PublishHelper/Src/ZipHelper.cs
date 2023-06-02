using ICSharpCode.SharpZipLib.Zip;
using System;

namespace JianLian.HDIS.PublishHelper
{
    /// <summary>
    /// 压缩文件帮助类
    /// </summary>
    public static class ZipHelper
    {
        /// <summary>  
        /// zip压缩文件  
        /// </summary>  
        /// <param name="fname">filename生成的文件的名称，如：C\123\123.zip</param>  
        /// <param name="path">directory要压缩的文件夹路径</param>  
        /// <returns></returns>  
        public static void ZipFiles(string fname, string path)
        {
            try
            {
                FastZip fz = new FastZip
                {
                    CreateEmptyDirectories = true
                };
                fz.CreateZip(fname, path, true, "");
            }
            catch (Exception ex)
            {
                Utility.SendLog("压缩", $"压缩文件失败：{ex}");
            }
        }

        /// <summary>
        /// 添加压缩文件注释
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="comment"></param>
        public static void SetComment(string fname, string comment)
        {
            try
            {
                using (ZipFile entry = new ZipFile(fname))
                {
                    entry.BeginUpdate();
                    entry.SetComment(comment);
                    entry.CommitUpdate();
                    Utility.SendLog("压缩", $"添加注释成功 {fname}");
                }
            }
            catch (Exception ex)
            {
                Utility.SendLog("压缩", $"添加注释失败：{ex}");
            }
        }
    }
}