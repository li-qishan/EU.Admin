/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* WxUser.cs
*
*功 能： N / A
* 类 名： WxUser
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2022/5/23 23:55:41 SimonHsiao 初版
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
    /// 微信用户
    /// </summary>
    [Entity(TableCnName = "微信用户", TableName = "WxUser")]
    public class WxUser : Base.PersistPoco
    {

        /// <summary>
        /// 表代码
        /// </summary>
        [Display(Name = "OpenId"), Description("表代码")]
        public string OpenId { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        [Display(Name = "OriginId"), Description("表名")]
        public string OriginId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "UserId"), Description("用户ID")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 场景值ID
        /// </summary>
        [Display(Name = "SceneId"), Description("场景值ID")]
        public string SceneId { get; set; }

        /// <summary>
        /// 场景字符串
        /// </summary>
        [Display(Name = "SceneStr"), Description("场景字符串")]
        public string SceneStr { get; set; }
    }
}