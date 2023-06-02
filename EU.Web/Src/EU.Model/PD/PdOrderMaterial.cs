/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdOrderMaterial.cs
*
*功 能： N / A
* 类 名： PdOrderMaterial
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/7/9 16:09:54 SimonHsiao 初版
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
    /// 生产工单-材料明细
    /// </summary>
    [Entity(TableCnName = "生产工单-材料明细", TableName = "PdOrderMaterial")]
    public class PdOrderMaterial : Base.PersistPoco
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
        /// 物料ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 单位用量
        /// </summary>
        [Display(Name = "Dosage")]
        public decimal Dosage { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        [Display(Name = "WastageRate")]
        public decimal WastageRate { get; set; }

        /// <summary>
        /// 应发数量
        /// </summary>
        [Display(Name = "ShouldQTY")]
        public decimal ShouldQTY { get; set; }

        /// <summary>
        /// 实发数量
        /// </summary>
        [Display(Name = "ActualQTY")]
        public decimal ActualQTY { get; set; }

        /// <summary>
        /// 状态，状态 未发料/部分发料/已发料
        /// </summary>
        [Display(Name = "PdOrderMaterialStatus")]
        public string PdOrderMaterialStatus { get; set; }
    }
}