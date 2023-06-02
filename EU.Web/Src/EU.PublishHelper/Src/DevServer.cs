using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianLian.HDIS.PublishHelper
{
    /// <summary>
    /// 服务器
    /// </summary>
    [Serializable]
    public class Server
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Su密码
        /// </summary>
        public string SuPassword { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }
    }
    /// <summary>
    /// 开发服务器
    /// </summary>
    [Serializable]
    public class DevServer: Server
    {
        /// <summary>
        /// 医院信息
        /// </summary>
        public List<Hospital> Hospitals { get; set; }
    }

    /// <summary>
    /// 医院
    /// </summary>
    [Serializable]
    public class Hospital
    {
        /// <summary>
        /// 医院名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件夹名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 默认端口前三位
        /// </summary>
        public string DefaultFort { get; set; }
        /// <summary>
        /// 本地后端代码路径
        /// </summary>
        public string SCPath { get; set; }
        /// <summary>
        /// 本地前端代码路径
        /// </summary>
        public string WWWPath { get; set; }
    }
}
