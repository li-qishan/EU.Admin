/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PoReturnOrder.cs
*
*功 能： N / A
* 类 名： PoReturnOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/15 0:19:01 SimonHsiao 初版
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
    [Entity(TableCnName = "采购退货单", TableName = "PoReturnOrder")]
    public class POReturnOrder : Base.PersistPoco
    {


        /// <summary>
        /// 销售单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 作业日期
        /// </summary>
        [Display(Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 作业人ID
        /// </summary>
        [Display(Name = "UserId")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Display(Name = "DepartmentId")]
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// 供应商ID
        /// </summary>
        [Display(Name = "SupplierId")]
        public Guid? SupplierId { get; set; }

        /// <summary>
        /// 供应商名字
        /// </summary>
        [Display(Name = "SupplierName")]
        public string SupplierName { get; set; }
    }
}