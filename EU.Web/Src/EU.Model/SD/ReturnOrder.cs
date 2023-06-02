/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SdReturnOrder.cs
*
*功 能： N / A
* 类 名： SdReturnOrder
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/3 14:37:51 SimonHsiao 初版
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
    [Entity(TableCnName = "退货单", TableName = "SdReturnOrder")]
    public class ReturnOrder : Base.PersistPoco
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
        /// 退回原因
        /// </summary>
        [Display(Name = "ReturnReason")]
        public string ReturnReason { get; set; }

        /// <summary>
        /// 退货日期
        /// </summary>
        [Display(Name = "ReturnDate")]
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// 退货状态--待退回、已退回（保存后，单据状态为待退回，增加退回入库按钮，点击确认后，单据状态为已退回）
        /// </summary>
        [Display(Name = "ReturnStatus")]
        public string ReturnStatus { get; set; }
    }
}