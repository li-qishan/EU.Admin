﻿/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdRequireAnalysis.cs
*
*功 能： N / A
* 类 名： PdRequireAnalysis
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/7/6 14:59:35 SimonHsiao 初版
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

namespace EU.Model.PD.Extend
{
    /// <summary>
    /// 
    /// </summary>
    public class OutSourceList : PdOutDetail
    {
        public decimal OrderQTY { get; set; }

        public string SourceOrderNo { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialNo { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specifications { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TextureName { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 配方
        /// </summary>
        public string Formula { get; set; }

        public string ShouldMaterialNo { get; set; }
        public string ShouldMaterialName { get; set; }
        public string ShouldSpecifications { get; set; }
        public string ShouldUnitName { get; set; }

    }
}