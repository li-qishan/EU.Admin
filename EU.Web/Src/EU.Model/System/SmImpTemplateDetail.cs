/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmImpTemplateDetail.cs
*
*功 能： N / A
* 类 名： SmImpTemplateDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/22 11:51:21 SimonHsiao 初版
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
    [Entity(TableCnName = "导入模板定义明细", TableName = "SmImpTemplateDetail")]
    public class SmImpTemplateDetail : Base.PersistPoco
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        [Display(Name = "ImpTemplateId")]
        public Guid? ImpTemplateId { get; set; }

        /// <summary>
        /// Execl列号
        /// </summary>
        [Display(Name = "ColumnNo")]
        public int ColumnNo { get; set; }

        /// <summary>
        /// 列名称
        /// </summary>
        [Display(Name = "ColumnCode")]
        public string ColumnCode { get; set; }

        /// <summary>
        /// 是否唯一
        /// </summary>
        [Display(Name = "IsUnique")]
        public bool IsUnique { get; set; }

        /// <summary>
        /// 是否插入
        /// </summary>
        [Display(Name = "IsInsert")]
        public bool IsInsert { get; set; }

        /// <summary>
        /// 格式
        /// </summary>
        [Display(Name = "DateFormate")]
        public string DateFormate { get; set; }

        /// <summary>
        /// 最大长度
        /// </summary>
        [Display(Name = "MaxLength")]
        public int MaxLength { get; set; }

        /// <summary>
        /// 允许为空
        /// </summary>
        [Display(Name = "IsAllowNull")]
        public bool IsAllowNull { get; set; }

        /// <summary>
        /// 加密
        /// </summary>
        [Display(Name = "IsEncrypt")]
        public bool IsEncrypt { get; set; }

        /// <summary>
        /// 参数代码
        /// </summary>
        [Display(Name = "LovCode")]
        public string LovCode { get; set; }

        /// <summary>
        /// 映射表
        /// </summary>
        [Display(Name = "CorresTableCode")]
        public string CorresTableCode { get; set; }

        /// <summary>
        /// 映射字段
        /// </summary>
        [Display(Name = "CorresColumnCode")]
        public string CorresColumnCode { get; set; }

        /// <summary>
        /// 转换字段
        /// </summary>
        [Display(Name = "TransColumnCode")]
        public string TransColumnCode { get; set; }
    }
}