/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdOutDetail.cs
*
*功 能： N / A
* 类 名： PdOutDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/7/21 16:11:04 SimonHsiao 初版
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
using System.ComponentModel.DataAnnotations.Schema;

namespace EU.Model
{

    /// <summary>
    /// 材料出库工单明细
    /// </summary>
    [Entity(TableCnName = "材料出库工单明细", TableName = "PdOutDetail")]
    public class PdOutDetail : Base.PersistPoco
    {

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "OrderId")]
        public Guid? OrderId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "SerialNumber")]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 项次
        /// </summary>
        [Display(Name = "SourceSerialNumber")]
        public int SourceSerialNumber { get; set; }

        /// <summary>
        /// 来源单ID
        /// </summary>
        [Display(Name = "SourceOrderId")]
        public Guid? SourceOrderId { get; set; }

        /// <summary>
        /// 来源单明细ID
        /// </summary>
        [Display(Name = "SourceOrderDetailId")]
        public Guid? SourceOrderDetailId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 应发物料ID
        /// </summary>
        [Display(Name = "ShouldMaterialId")]
        public Guid? ShouldMaterialId { get; set; }

        /// <summary>
        /// 应发数量
        /// </summary>
        [Display(Name = "ShouldQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal ShouldQTY { get; set; }

        /// <summary>
        /// 实发数量
        /// </summary>
        [Display(Name = "ActualQTY"), Column(TypeName = "decimal(20,8)")]
        public decimal ActualQTY { get; set; }

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
        /// 批号
        /// </summary>
        [Display(Name = "BatchNo")]
        public string BatchNo { get; set; }
    }
}