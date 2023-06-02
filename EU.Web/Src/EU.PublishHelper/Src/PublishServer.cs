using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianLian.HDIS.PublishHelper
{

    /// <summary>
    /// 发布服务器
    /// </summary>
    [Serializable]
    public class PublishServer : Server
    {
        private List<VersionFolder> folders;
        /// <summary>
        /// 版本文件夹
        /// </summary>
        public List<VersionFolder> Folders
        {
            get { return folders; }
            set
            {
                if (folders == null)
                    value = new List<VersionFolder>();
                folders = value;
            }
        }
    }

    /// <summary>
    /// 版本文件夹
    /// </summary>
    [Serializable]
    public class VersionFolder
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FolderName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }

        private List<VersionFile> files;
        /// <summary>
        /// 版本文件
        /// </summary>
        public List<VersionFile> Files
        {
            get { return files; }
            set
            {
                if (files == null)
                    value = new List<VersionFile>();
                files = value;
            }
        }
    }
    /// <summary>
    /// 版本文件
    /// </summary>
    [Serializable]
    public class VersionFile
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
    }
}
