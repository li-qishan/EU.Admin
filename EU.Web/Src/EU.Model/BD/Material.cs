/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdMaterial.cs
*
*功 能： N / A
* 类 名： BdMaterial
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/1/14 17:51:19 SimonHsiao 初版
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
using System.ComponentModel.DataAnnotations.Schema;

namespace EU.Model
{
    [Entity(TableCnName = "物料管理", TableName = "BdMaterial")]
    public class Material : Base.PersistPoco
    {


        /// <summary>
        /// 物料编号
        /// </summary>
        [Display(Name = "MaterialNo")]
        public string MaterialNo { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        [Display(Name = "MaterialNames")]
        public string MaterialNames { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Display(Name = "Specifications")]
        public string Specifications { get; set; }

        /// <summary>
        /// 材质ID
        /// </summary>
        [Display(Name = "TextureId")]
        public Guid? TextureId { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        [Display(Name = "MaterialTypeId")]
        public Guid? MaterialTypeId { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        [Display(Name = "MaterialTypeIds")]
        public string MaterialTypeIds { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// 存货计价
        /// </summary>
        [Display(Name = "InventoryValuation"), Column(TypeName = "decimal(20,2)")]
        public decimal InventoryValuation { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        [Display(Name = "Length"), Column(TypeName = "decimal(20,6)")]
        public decimal Length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        [Display(Name = "Width"), Column(TypeName = "decimal(20,6)")]
        public decimal Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        [Display(Name = "Height"), Column(TypeName = "decimal(20,6)")]
        public decimal Height { get; set; }

        /// <summary>
        /// 颜色ID
        /// </summary>
        [Display(Name = "ColorId")]
        public Guid? ColorId { get; set; }

        /// <summary>
        /// 单重
        /// </summary>
        [Display(Name = "SingleWeight"), Column(TypeName = "decimal(20,6)")]
        public decimal SingleWeight { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [Display(Name = "UnitId")]
        public Guid? UnitId { get; set; }

        /// <summary>
        /// 图号
        /// </summary>
        [Display(Name = "DrawingNo")]
        public string DrawingNo { get; set; }

        /// <summary>
        /// 最小起订量
        /// </summary>
        [Display(Name = "MinOrder"), Column(TypeName = "decimal(20,8)")]
        public decimal MinOrder { get; set; }

        /// <summary>
        /// 安全库存量
        /// </summary>
        [Display(Name = "SafetStock"), Column(TypeName = "decimal(20,8)")]
        public decimal SafetStock { get; set; }

        /// <summary>
        /// 最小采购量
        /// </summary>
        [Display(Name = "MinPurchase"), Column(TypeName = "decimal(20,8)")]
        public decimal MinPurchase { get; set; }

        /// <summary>
        /// 保质期
        /// </summary>
        [Display(Name = "ExpirationDate")]
        public int ExpirationDate { get; set; }

        /// <summary>
        /// 是否批号管
        /// </summary>
        [Display(Name = "IsBatchControl")]
        public bool IsBatchControl { get; set; }

        /// <summary>
        /// 检验方式（免检、抽检、全检）
        /// </summary>
        [Display(Name = "CheckMethod")]
        public string CheckMethod { get; set; }

        /// <summary>
        /// 采购价
        /// </summary>
        [Display(Name = "PurchasePrice"), Column(TypeName = "decimal(20,2)")]
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// 最低销售价
        /// </summary>
        [Display(Name = "MinSalesPrice"), Column(TypeName = "decimal(20,2)")]
        public decimal MinSalesPrice { get; set; }

        /// <summary>
        /// 生产采购前置天数
        /// </summary>
        [Display(Name = "ProductionPurchasePreDays")]
        public int ProductionPurchasePreDays { get; set; }

        /// <summary>
        /// 生产采购周期
        /// </summary>
        [Display(Name = "ProductionPurchasePeriod")]
        public int ProductionPurchasePeriod { get; set; }

        /// <summary>
        /// 图片URL
        /// </summary>
        [Display(Name = "图片URL")]
        public string ImageUrl { get; set; }
    }
}