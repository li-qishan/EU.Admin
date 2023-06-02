/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* PdOrderMould.cs
*
*功 能： N / A
* 类 名： PdOrderMould
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2021/7/14 14:18:50 SimonHsiao 初版
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

    /// <summary>
    /// 生产工单工模治具
    /// </summary>
    [Entity(TableCnName = "生产工单工模治具", TableName = "PdOrderMould")]
    public class PdOrderMould : Base.PersistPoco
    {

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "OrderId")]
        public Guid? OrderId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "SerialNumber")]
        public int SerialNumber { get; set; }

        /// <summary>
        /// BOMID
        /// </summary>
        [Display(Name = "BOMId")]
        public Guid? BOMId { get; set; }

        /// <summary>
        /// 模具ID
        /// </summary>
        [Display(Name = "MouldId")]
        public Guid? MouldId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        [Display(Name = "ProcessId")]
        public Guid? ProcessId { get; set; }
    }
}