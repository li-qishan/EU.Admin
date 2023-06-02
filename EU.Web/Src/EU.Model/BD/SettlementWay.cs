/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdSettlementWay.cs
*
*功 能： N / A
* 类 名： BdSettlementWay
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/11 0:43:27 SimonHsiao 初版
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
    public class SettlementWay : Base.PersistPoco
    {


        /// <summary>
        /// 结算编号
        /// </summary>
        [Display(Name = "结算编号")]
        public string SettlementNo { get; set; }

        /// <summary>
        /// 账款类型, 现付-ImmediatePay;月结-MonthlyPay;次月结-NextMonthlyPay; 货到付款-PayOnDelivery;其他-Other
        /// </summary>
        [Display(Name = "账款类型, 现付-ImmediatePay;月结-MonthlyPay;次月结-NextMonthlyPay; 货到付款-PayOnDelivery;其他-Other")]
        public string SettlementAccountType { get; set; }

        /// <summary>
        /// 账期天数
        /// </summary>
        [Display(Name = "账期天数")]
        public int Days { get; set; }

        /// <summary>
        /// 收付款（收-Get、付-Out选择）
        /// </summary>
        [Display(Name = "收付款（收-Get、付-Out选择）")]
        public string SettlementBillType { get; set; }

        /// <summary>
        /// 结算名称
        /// </summary>
        public string SettlementName { get; set; }
    }
}