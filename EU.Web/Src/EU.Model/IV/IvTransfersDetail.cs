﻿/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* IvTransfersDetail.cs
*
*功 能： N / A
* 类 名： IvTransfersDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/17 17:36:47 SimonHsiao 初版
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
    /// 库存调拨单明细
    /// </summary>
    [Entity(TableCnName = "库存调拨单明细", TableName = "IvTransfersDetail")]
    public class IvTransfersDetail : Base.PersistPoco
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
        /// 货品ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }



        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "QTY"), Column(TypeName = "decimal(20,8)")]
        public decimal? QTY { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "Price"), Column(TypeName = "decimal(20,2)")]
        public decimal? Price { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [Display(Name = "BatchNo")]
        public string BatchNo { get; set; }

        /// <summary>
        /// 移出仓库ID
        /// </summary>
        [Display(Name = "OutStockId")]
        public Guid? OutStockId { get; set; }

        /// <summary>
        /// 移出货位ID
        /// </summary>
        [Display(Name = "OutGoodsLocationId")]
        public Guid? OutGoodsLocationId { get; set; }

        /// <summary>
        /// 移入仓库ID
        /// </summary>
        [Display(Name = "InStockId")]
        public Guid? InStockId { get; set; }

        /// <summary>
        /// 移入货位ID
        /// </summary>
        [Display(Name = "InGoodsLocationId")]
        public Guid? InGoodsLocationId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark1")]
        public string ExtRemark1 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark2")]
        public string ExtRemark2 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark3")]
        public string ExtRemark3 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "ExtRemark4")]
        public string ExtRemark4 { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IvTransfersDetailExtend : IvTransfersDetail
    {

        public string OutStockName { get; set; }

        public string OutGoodsLocationName { get; set; }

        public string InStockName { get; set; }

        public string InGoodsLocationName { get; set; }

        /// <summary>
        /// 货品名称
        /// </summary>
        [Display(Name = "MaterialName")]
        public string MaterialName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Display(Name = "Specifications")]
        public string Specifications { get; set; }

        /// <summary>
        /// 单位ID
        /// </summary>
        [Display(Name = "UnitId")]
        public Guid? UnitId { get; set; }
    }
}