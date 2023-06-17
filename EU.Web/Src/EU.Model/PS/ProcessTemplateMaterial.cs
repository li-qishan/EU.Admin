/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsProcessTemplateMaterial.cs
*
*功 能： N / A
* 类 名： PsProcessTemplateMaterial
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/25 17:47:26 SimonHsiao 初版
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
    [Entity(TableCnName = "工序模板物料", TableName = "PsProcessTemplateMaterial")]
    public class ProcessTemplateMaterial : Base.PersistPoco
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        [Display(Name = "TemplateId")]
        public Guid? TemplateId { get; set; }

        /// <summary>
        /// 物料ID
        /// </summary>
        [Display(Name = "MaterialId")]
        public Guid? MaterialId { get; set; }

        /// <summary>
        /// 批量
        /// </summary>
        [Display(Name = "BulkQty"), Column(TypeName = "decimal(20,6)")]
        public decimal? BulkQty { get; set; }
    }
}