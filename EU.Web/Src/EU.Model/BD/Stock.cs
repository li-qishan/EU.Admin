/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdStock.cs
*
*功 能： N / A
* 类 名： BdStock
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/1 13:59:18 SimonHsiao 初版
*
* Copyright(c) 2020 SUZHOU EU Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{
    /// <summary>
    /// 仓库
    /// </summary>
    public class Stock : Base.PersistPoco
    {
        /// <summary>
        /// 仓库编号
        /// </summary>
        [Display(Name = "仓库编号")]
        public string StockNo { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        [Display(Name = "仓库名称")]
        public string StockNames { get; set; }

        /// <summary>
        /// 仓库类别
        /// </summary>
        [Display(Name = "仓库类别")]
        public string StockCategory { get; set; }

        /// <summary>
        /// 仓管员ID
        /// </summary>
        [Display(Name = "仓管员ID")]
        public Guid? StockKeeperId { get; set; }

        /// <summary>
        /// 是否虚拟
        /// </summary>
        [Display(Name = "IsVirtual")]
        public bool? IsVirtual { get; set; }
    }
}