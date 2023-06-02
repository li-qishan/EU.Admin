/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdGoodsLocation.cs
*
*功 能： N / A
* 类 名： BdGoodsLocation
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/1 16:10:02 SimonHsiao 初版
*
* Copyright(c) 2020 SUZHOU EU Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using EU.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{
    /// <summary>
    /// 货位
    /// </summary>
    [Entity(TableCnName = "货位", TableName = "BdGoodsLocation")]
    public class GoodsLocation : Base.PersistPoco
    {

        /// <summary>
        /// 仓库ID
        /// </summary>
        [Display(Name = "仓库ID")]
        public Guid? StockId { get; set; }

        /// <summary>
        /// 货位编号
        /// </summary>
        [Display(Name = "货位编号")]
        public string LocationNo { get; set; }

        /// <summary>
        /// 货位名称
        /// </summary>
        [Display(Name = "货位名称")]
        public string LocationNames { get; set; }

        /// <summary>
        /// 货位容量
        /// </summary>
        [Display(Name = "货位容量")]
        public decimal? LocationCapacity { get; set; }
    }
}