/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsProcessTemplate.cs
*
*功 能： N / A
* 类 名： PsProcessTemplate
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/25 14:24:58 SimonHsiao 初版
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
    [Entity(TableCnName = "工序模板", TableName = "PsProcessTemplate")]
    public class ProcessTemplate : Base.PersistPoco
    {
        /// <summary>
        /// 模版单号
        /// </summary>
        [Display(Name = "TemplateNo")]
        public string TemplateNo { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        [Display(Name = "TemplateName")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 工艺流程
        /// </summary>
        [Display(Name = "FlowList")]
        public string FlowList { get; set; }
    }
}