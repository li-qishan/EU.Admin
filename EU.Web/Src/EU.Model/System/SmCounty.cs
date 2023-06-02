/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmCounty.cs
*
*功 能： N / A
* 类 名： SmCounty
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2022/5/8 22:13:42 SimonHsiao 初版
*
* Copyright(c) 2022 SUZHOU EU Corporation. All Rights Reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using EU.Entity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EU.Model
{

    /// <summary>
    /// 区县
    /// </summary>
    [Entity(TableCnName = "区县", TableName = "SmCounty")]
    public class SmCounty : Base.PersistPoco
    {

        /// <summary>
        /// 省份ID
        /// </summary>
        [Display(Name = "ProvinceId"), Description("省份ID")]
        public Guid? ProvinceId { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        [Display(Name = "CityId"), Description("城市ID")]
        public Guid? CityId { get; set; }

        /// <summary>
        /// 区县代码
        /// </summary>
        [Display(Name = "CountyCode"), Description("区县代码")]
        public string CountyCode { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        [Display(Name = "CountyNameZh"), Description("区县")]
        public string CountyNameZh { get; set; }

        /// <summary>
        /// 区县(英文)
        /// </summary>
        [Display(Name = "CountyNameEn"), Description("区县(英文)")]
        public string CountyNameEn { get; set; }

        /// <summary>
        /// 区县编号
        /// </summary>
        [Display(Name = "CountyNo"), Description("区县编号")]
        public string CountyNo { get; set; }

        /// <summary>
        /// 区县邮编
        /// </summary>
        [Display(Name = "CountyZipCode"), Description("区县邮编")]
        public string CountyZipCode { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "TaxisNo"), Description("排序号")]
        public int? TaxisNo { get; set; }
    }
}