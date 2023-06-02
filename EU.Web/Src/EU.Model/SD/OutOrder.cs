/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SdOutOrder.cs
*
*功 能： N / A
* 类 名： SdOutOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/1 20:52:27 SimonHsiao 初版
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
    [Entity(TableCnName = "出库单", TableName = "SdOutOrder")]
    public class OutOrder : Base.PersistPoco
    {


        /// <summary>
        /// 订单号
        /// </summary>
        [Display(Name = "OrderNo")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 作业日期
        /// </summary>
        [Display(Name = "OrderDate")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [Display(Name = "CustomerId")]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        [Display(Name = "StockId")]
        public Guid? StockId { get; set; }

        /// <summary>
        /// 货位ID
        /// </summary>
        [Display(Name = "GoodsLocationId")]
        public Guid? GoodsLocationId { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        [Display(Name = "Contact")]
        public string Contact { get; set; }

        /// <summary>
        /// 收货地址
        /// </summary>
        [Display(Name = "Address")]
        public string Address { get; set; }

        /// <summary>
        /// 收货电话
        /// </summary>
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 出库日期
        /// </summary>
        [Display(Name = "OutDate")]
        public DateTime? OutDate { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        [Display(Name = "OutTime")]
        public DateTime? OutTime { get; set; }
    }
}