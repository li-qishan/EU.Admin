using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JianLian.HDIS.PublishHelper
{
    /// <summary>
    /// 操作记录
    /// </summary>
    [Serializable]
    public class OprateLog
    {
        /// <summary>
        /// 上一次选择的服务器
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 上一次选择的医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// webapi
        /// </summary>
        public bool Webapi { get; set; }
        /// <summary>
        /// hfs
        /// </summary>
        public bool Hfs { get; set; }
        /// <summary>
        /// rtm
        /// </summary>
        public bool Rtm { get; set; }
        /// <summary>
        /// job
        /// </summary>
        public bool Job { get; set; }
        /// <summary>
        /// web
        /// </summary>
        public bool Web { get; set; }
        /// <summary>
        /// pad
        /// </summary>
        public bool Pad { get; set; }
        /// <summary>
        /// remove
        /// </summary>
        public bool Remove { get; set; }
        /// <summary>
        /// build
        /// </summary>
        public bool Build { get; set; }

        /// <summary>
        /// 版本打包-开发服务器
        /// </summary>
        public string PackDevIp { get; set; }
        /// <summary>
        /// 版本打包-医院
        /// </summary>
        public string PackHospital { get; set; }
        /// <summary>
        /// 版本打包-发布服务器
        /// </summary>
        public string PackPublishIp { get; set; }
        /// <summary>
        /// 版本打包-版本
        /// </summary>
        public string PackVersion { get; set; }
        /// <summary>
        /// 版本打包-数据库脚本
        /// </summary>
        public bool PackDbFile { get; set; }
        /// <summary>
        /// 版本打包-打包前先执行发布
        /// </summary>
        public bool PackPublish { get; set; }
        /// <summary>
        /// 版本打包-Git
        /// </summary>
        public bool PackGit { get; set; }
        /// <summary>
        /// 版本打包-是否自动下载
        /// </summary>
        public bool PackDownload { get; set; }
        /// <summary>
        /// 版本打包-是否为升级包
        /// </summary>
        public bool PackUpgrade { get; set; }
        /// <summary>
        /// 版本打包-TS升级包
        /// </summary>
        public bool PackUpgradeTS { get; set; }
        /// <summary>
        /// 版本打包-TS升级包项目名称
        /// </summary>
        public string PackUpgradeTSCustom { get; set; }
        /// <summary>
        /// TS插件打包-项目地址
        /// </summary>
        public string TSPath { get; set; }
        /// <summary>
        /// TS插件打包-版本号
        /// </summary>
        public string TSVersion { get; set; }
        /// <summary>
        /// TS插件打包-保存位置
        /// </summary>
        public string TSSavePath { get; set; }
        /// <summary>
        /// TS插件打包-是否自定义
        /// </summary>
        public bool TSCustom { get; set; }
        /// <summary>
        /// TS插件打包-是否编译
        /// </summary>
        public bool TSBuild { get; set; }
        /// <summary>
        /// TS插件打包-自定义医院
        /// </summary>
        public string TSCustomHospitals { get; set; }
    }
}
