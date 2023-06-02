/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* IvCheck.cs
*
*功 能： N / A
* 类 名： IvCheck
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/17 23:19:07 SimonHsiao 初版
*
* Copyright(c) 2021 SUZHOU EU Corporation. All Rights Reserved.
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
    [Entity(TableCnName = "库存盘点单", TableName = "IvCheck")]
    public class IvCheck : Base.PersistPoco
    {
        /// <summary>
        /// 单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 盘点名称
        /// </summary>
        [Display(Name = "CheckName")]
        public string CheckName { get; set; }

        /// <summary>
        /// 盘点日期
        /// </summary>
        [Display(Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 盘点人ID
        /// </summary>
        [Display(Name = "UserId")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 盘点仓库ID
        /// </summary>
        [Display(Name = "StockId")]
        public Guid? StockId { get; set; }

        /// <summary>
        /// 盘点货位ID
        /// </summary>
        [Display(Name = "GoodsLocationId")]
        public Guid? GoodsLocationId { get; set; }

        /// <summary>
        /// 货品ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 货品名称
        /// </summary>
        [Display(Name = "MaterialName")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 包含0库存
        /// </summary>
        [Display(Name = "IsZero")]
        public bool IsZero { get; set; }
    }
}