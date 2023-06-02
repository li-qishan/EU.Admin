/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* BdTexture.cs
*
*功 能： N / A
* 类 名： BdTexture
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/11/30 14:47:00 SimonHsiao 初版
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
    public class Texture : Base.PersistPoco
    {
        /// <summary>
        /// 材质编号
        /// </summary>
        [Display(Name = "材质编号")]
        public string TextureNo { get; set; }

        /// <summary>
        /// 材质名称
        /// </summary>
        [Display(Name = "材质名称")]
        public string TextureNames { get; set; }

        /// <summary>
        /// 比重
        /// </summary>
        [Display(Name = "比重")]
        public int Proportion { get; set; }

        /// <summary>
        /// 基数
        /// </summary>
        [Display(Name = "基数")]
        public int BaseAmount { get; set; }
    }
}