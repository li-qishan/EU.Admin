/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmQuartzJob.cs
*
*功 能： N / A
* 类 名： SmQuartzJob
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2023/4/6 16:30:21 SimonHsiao 初版
*
* Copyright(c) 2023 SUZHOU EU Corporation. All Rights Reserved.
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
    /// 任务管理
    /// </summary>
    [Entity(TableCnName = "任务管理", TableName = "SmQuartzJob")]
    public class SmQuartzJob : Base.PersistPoco
    {

        /// <summary>
        /// 任务代码
        /// </summary>
        [Display(Name = "JobCode"), Description("任务代码")]
        public string JobCode { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [Display(Name = "JobName"), Description("任务名称")]
        public string JobName { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        [Display(Name = "ClassName"), Description("类名")]
        public string ClassName { get; set; }

        /// <summary>
        /// 执行规则
        /// </summary>
        [Display(Name = "ScheduleRule"), Description("执行规则")]
        public string ScheduleRule { get; set; }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        [Display(Name = "LastExecuteTime"), Description("上次执行时间")]
        public DateTime? LastExecuteTime { get; set; }

        /// <summary>
        /// 是否更新
        /// </summary>
        [Display(Name = "IsUpdate"), Description("是否更新")]
        public string IsUpdate { get; set; }
    }
}