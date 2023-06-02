/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* MfMouldType.cs
*
*功 能： N / A
* 类 名： MfMouldType
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/6/7 22:26:55 SimonHsiao 初版
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
    [Entity(TableCnName = "工模治具类别", TableName = "MfMouldType")]
    public class MouldType : Base.PersistPoco
    {

        /// <summary>
        /// 类型编号
        /// </summary>
        [Display(Name = "TypeNo")]
        public string TypeNo { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Display(Name = "TypeName")]
        public string TypeName { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        [Display(Name = "StockId")]
        public Guid? StockId { get; set; }
    }
}