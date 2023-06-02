/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmProvince.cs
*
*功 能： N / A
* 类 名： SmProvince
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2022/5/8 22:01:42 SimonHsiao 初版
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
    /// 省份
    /// </summary>
    [Entity(TableCnName = "省份", TableName = "SmProvince")]
    public class SmProvince : Base.PersistPoco
    {

        /// <summary>
        /// 省份代码
        /// </summary>
        [Display(Name = "ProvinceCode"), Description("省份代码")]
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [Display(Name = "ProvinceNameZh"), Description("省份")]
        public string ProvinceNameZh { get; set; }

        /// <summary>
        /// 省份(英文)
        /// </summary>
        [Display(Name = "ProvinceNameEn"), Description("省份(英文)")]
        public string ProvinceNameEn { get; set; }

        /// <summary>
        /// 省份编号
        /// </summary>
        [Display(Name = "ProvinceNo"), Description("省份编号")]
        public string ProvinceNo { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "TaxisNo"), Description("排序号")]
        public int? TaxisNo { get; set; }
    }
}