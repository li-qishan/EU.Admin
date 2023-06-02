/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* WxConfig.cs
*
*功 能： N / A
* 类 名： WxConfig
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2022/6/24 10:45:18 SimonHsiao 初版
*
* Copyright(c) 2022 SUZHOU EU Corporation. All Rights Reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using EU.Entity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{

    /// <summary>
    /// 微信配置
    /// </summary>
    [Entity(TableCnName = "微信配置", TableName = "WxConfig")]
    public class WxConfig : Base.PersistPoco
    {

        /// <summary>
        /// 接口类型
        /// </summary>
        [Display(Name = "InterfaceType"), Description("接口类型")]
        public string InterfaceType { get; set; }

        /// <summary>
        /// 令牌
        /// </summary>
        [Display(Name = "Token"), Description("令牌")]
        public string Token { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        [Display(Name = "AppId"), Description("AppId")]
        public string AppId { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        [Display(Name = "AppSecret"), Description("AppSecret")]
        public string AppSecret { get; set; }

        /// <summary>
        /// 原始ID
        /// </summary>
        [Display(Name = "OriginId"), Description("原始ID")]
        public string OriginId { get; set; }

        /// <summary>
        /// 微信ID
        /// </summary>
        [Display(Name = "WeixinId"), Description("微信ID")]
        public string WeixinId { get; set; }

        /// <summary>
        /// 微信名
        /// </summary>
        [Display(Name = "WeixinName"), Description("微信名")]
        public string WeixinName { get; set; }

        /// <summary>
        /// EncodingAESKey
        /// </summary>
        [Display(Name = "AESKey"), Description("EncodingAESKey")]
        public string AESKey { get; set; }

        /// <summary>
        /// 关注提醒内容
        /// </summary>
        [Display(Name = "SubscribeContent"), Description("关注提醒内容")]
        public string SubscribeContent { get; set; }

        /// <summary>
        /// 自动回复内容
        /// </summary>
        [Display(Name = "AutoReplyContent"), Description("自动回复内容")]
        public string AutoReplyContent { get; set; }
    }
}