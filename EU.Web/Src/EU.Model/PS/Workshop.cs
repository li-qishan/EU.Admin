﻿/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PsWorkshop.cs
*
*功 能： N / A
* 类 名： PsWorkshop
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/5/18 17:42:28 SimonHsiao 初版
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
    [Entity(TableCnName = "制造车间", TableName = "PsWorkShop")]
    public class WorkShop : Base.PersistPoco
    {
        /// <summary>
        /// 车间编号
        /// </summary>
        [Display(Name = "WorkShopNo")]
        public string WorkShopNo { get; set; }

        /// <summary>
        /// 车间名称
        /// </summary>
        [Display(Name = "WorkShopName")]
        public string WorkShopName { get; set; }

        /// <summary>
        /// 负责人ID
        /// </summary>
        [Display(Name = "ChargeId")]
        public Guid? ChargeId { get; set; }
    }
}