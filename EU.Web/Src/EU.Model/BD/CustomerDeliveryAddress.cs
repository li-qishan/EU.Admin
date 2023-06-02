/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdCustomerDeliveryAddress.cs
*
*功 能： N / A
* 类 名： BdCustomerDeliveryAddress
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/13 23:55:57 SimonHsiao 初版
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
    public class CustomerDeliveryAddress : Base.PersistPoco
    {


        /// <summary>
        /// 客户ID
        /// </summary>
        [Display(Name = "客户ID")]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [Display(Name = "负责人")]
        public string Contact { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "手机")]
        public string Phone { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        public string Telephone { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        [Display(Name = "是否默认")]
        public bool IsDefault { get; set; }
    }
}