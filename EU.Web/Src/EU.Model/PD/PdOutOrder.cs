/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdOutOrder.cs
*
*功 能： N / A
* 类 名： PdOutOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/6/14 14:05:55 SimonHsiao 初版
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
    /// 材料出库工单
    /// </summary>
    [Entity(TableCnName = "材料出库工单", TableName = "PdOutOrder")]
    public class PdOutOrder : Base.PersistPoco
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
        /// 车间ID
        /// </summary>
        [Display(Name = "WorkShopId")]
        public Guid? WorkShopId { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        [Display(Name = "OutTime")]
        public DateTime? OutTime { get; set; }
    }
}