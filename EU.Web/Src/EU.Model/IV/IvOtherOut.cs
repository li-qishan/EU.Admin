/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* IvOtherOut.cs
*
*功 能： N / A
* 类 名： IvOtherOut
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/18 23:27:56 SimonHsiao 初版
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
    [Entity(TableCnName = "其他出库单", TableName = "IvOtherOut")]
    public class IvOtherOut : Base.PersistPoco
    {
        /// <summary>
        /// 单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 入库类型
        /// </summary>
        [Display(Name = "OutType")]
        public string OutType { get; set; }

        /// <summary>
        /// 作业日期
        /// </summary>
        [Display(Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 建立人ID
        /// </summary>
        [Display(Name = "UserId")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 入库仓库ID
        /// </summary>
        [Display(Name = "StockId")]
        public Guid? StockId { get; set; }

        /// <summary>
        /// 入库货位ID
        /// </summary>
        [Display(Name = "GoodsLocationId")]
        public Guid? GoodsLocationId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Display(Name = "DepartmentId")]
        public Guid? DepartmentId { get; set; }
    }
}