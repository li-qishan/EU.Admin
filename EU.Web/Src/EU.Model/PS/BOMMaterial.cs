/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsBOMMaterial.cs
*
*功 能： N / A
* 类 名： PsBOMMaterial
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/29 23:06:55 SimonHsiao 初版
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
    [Entity(TableCnName = "BOM物料", TableName = "PsBOMMaterial")]
    public class BOMMaterial : Base.PersistPoco
    {
        /// <summary>
        /// BOMID
        /// </summary>
        [Display(Name = "BOMId")]
        public Guid? BOMId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "SerialNumber")]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 货品编号
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        [Display(Name = "Dosage"), Column(TypeName = "decimal(20,4)")]
        public decimal Dosage { get; set; }

        /// <summary>
        /// 用量基数，默认1，用量基数为分母，用量为分子（比如用量基数为1000，用量为1，则单个用量则为千分之一）
        /// </summary>
        [Display(Name = "DosageBase"), Column(TypeName = "decimal(20,2)")]
        public decimal DosageBase { get; set; }

        /// <summary>
        /// 损耗率，最好可设置阶梯损耗，比如0～1000损耗率为3%，1001～10000损耗率为2%，后续计算物料用量需要加上损耗
        /// </summary>
        [Display(Name = "WastageRate"), Column(TypeName = "decimal(20,2)")]
        public decimal WastageRate { get; set; }
    }
}