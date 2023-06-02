/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdCustomerInvoice.cs
*
*功 能： N / A
* 类 名： BdCustomerInvoice
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/14 0:10:36 SimonHsiao 初版
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
    public class CustomerInvoice : Base.PersistPoco
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        [Display(Name = "客户ID")]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 抬头
        /// </summary>
        [Display(Name = "抬头")]
        public string InvoiceTitle { get; set; }

        /// <summary>
        /// 税号
        /// </summary>
        [Display(Name = "税号")]
        public string TaxNo { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        [Display(Name = "开户银行")]
        public string BankName { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        [Display(Name = "银行账号")]
        public string BankAccount { get; set; }
    }
}