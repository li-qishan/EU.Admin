/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdCustomerContact.cs
*
*功 能： N / A
* 类 名： BdCustomerContact
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/12/14 14:07:31 SimonHsiao 初版
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
    public class CustomerContact : Base.PersistPoco
    {


        /// <summary>
        /// 客户ID
        /// </summary>
        [Display(Name = "CustomerId")]
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "Sex")]
        public string Sex { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [Display(Name = "Position")]
        public string Position { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "Telphone")]
        public string Telphone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "Mail")]
        public string Mail { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        [Display(Name = "QQ")]
        public string QQ { get; set; }

        /// <summary>
        /// 微信
        /// </summary>
        [Display(Name = "WechatId")]
        public string WechatId { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Display(Name = "Birthday")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>
        [Display(Name = "NativePlace")]
        public string NativePlace { get; set; }

        /// <summary>
        /// MSN
        /// </summary>
        [Display(Name = "MSN")]
        public string MSN { get; set; }
    }
}