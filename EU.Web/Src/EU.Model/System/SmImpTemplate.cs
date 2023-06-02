/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmImpTemplate.cs
*
*功 能： N / A
* 类 名： SmImpTemplate
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/2/20 15:47:27 SimonHsiao 初版
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
    [Entity(TableCnName = "导入模板定义", TableName = "SmImpTemplate")]
    public class SmImpTemplate : Base.PersistPoco
    {


        /// <summary>
        /// 模板代码
        /// </summary>
        [Display(Name = "TemplateCode")]
        public string TemplateCode { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        [Display(Name = "TemplateName")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 表代码
        /// </summary>
        [Display(Name = "TableCode")]
        public string TableCode { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        [Display(Name = "ModuleId")]
        public Guid? ModuleId { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        [Display(Name = "ModuleCode")]
        public string ModuleCode { get; set; }

        /// <summary>
        /// Sheet名
        /// </summary>
        [Display(Name = "SheetName")]
        public string SheetName { get; set; }

        /// <summary>
        /// 数据起始行
        /// </summary>
        [Display(Name = "StartRow")]
        public int StartRow { get; set; }

        /// <summary>
        /// 标签名
        /// </summary>
        [Display(Name = "Label")]
        public string Label { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "TaxisNo")]
        public int TaxisNo { get; set; }

        /// <summary>
        /// 转验证完全正确，允许存在错误
        /// </summary>
        [Display(Name = "TransferMode")]
        public string TransferMode { get; set; }

        /// <summary>
        /// 显示进度条
        /// </summary>
        [Display(Name = "IsDisplayProgress")]
        public bool IsDisplayProgress { get; set; }

        /// <summary>
        /// 加载数据
        /// </summary>
        [Display(Name = "IsLoadData")]
        public bool IsLoadData { get; set; }

        /// <summary>
        /// 显示
        /// </summary>
        [Display(Name = "IsDisplay")]
        public bool IsDisplay { get; set; }

        /// <summary>
        /// 是否允许覆盖导入
        /// </summary>
        [Display(Name = "IsAllowOverride")]
        public bool IsAllowOverride { get; set; }

        /// <summary>
        /// 排除最后行数
        /// </summary>
        [Display(Name = "ExcludeLastRow")]
        public int ExcludeLastRow { get; set; }
    }
}