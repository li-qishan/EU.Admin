/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PoDeductOrder.cs
*
*功 能： N / A
* 类 名： PoDeductOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/9/12 14:02:30 SimonHsiao 初版
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

    /// <summary>
    /// 采购费用单
    /// </summary>
    [Entity(TableCnName = "采购扣款单", TableName = "PoDeductOrder")]
    public class PoFeeOrder : Base.PersistPoco
    {

        /// <summary>
        /// 单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 作业日期
        /// </summary>
        [Display(Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 采购员ID
        /// </summary>
        [Display(Name = "UserId")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [Display(Name = "SupplierId")]
        public Guid? SupplierId { get; set; }
    }
}