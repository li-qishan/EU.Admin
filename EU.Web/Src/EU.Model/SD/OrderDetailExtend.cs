/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SdOrderDetail.cs
*
*功 能： N / A
* 类 名： SdOrderDetail
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/27 21:41:17 SimonHsiao 初版
*
* Copyright(c) 2020 SUZHOU EU Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{
    public class OrderDetailExtend : OrderDetail
    {
        /// <summary>
        /// 材质编号
        /// </summary>
        [Display(Name = "MaterialNo")]
        public string MaterialNo { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Display(Name = "类型名称")]
        public string UnitName { get; set; }

        /// <summary>
        /// 订单占用数量
        /// </summary>
        [Display(Name = "OccupyQTY")]
        public decimal OccupyQTY { get; set; }

    }
}