/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdReissueDetail.cs
*
*功 能： N / A
* 类 名： PdReissueDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/7/20 14:45:23 SimonHsiao 初版
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
    /// PdReissueDetail
    /// </summary>
    [Entity(TableCnName = "", TableName = "PdReissueDetail")]
    public class PdReissueDetail : Base.PersistPoco
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
        /// 来源单ID
        /// </summary>
        [Display(Name = "SourceOrderId")]
        public Guid? SourceOrderId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 补发数量
        /// </summary>
        [Display(Name = "QTY"), Column(TypeName = "decimal(20,8)")]
        public decimal QTY { get; set; }

        /// <summary>
        /// 工艺卡号
        /// </summary>
        [Display(Name = "ProcessCardNo")]
        public string ProcessCardNo { get; set; }
    }
}