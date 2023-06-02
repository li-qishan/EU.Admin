/* 代码由框架生成,任何更改都可能导致被代码生成器覆盖，可自行修改。
* SmModuleSql.cs
*
*功 能： N / A
* 类 名： SmModuleSql
*
* Ver 变更日期 负责人 变更内容
* ───────────────────────────────────
*V0.01 2020/11/23 11:25:20 SimonHsiao 初版
*
* Copyright(c) 2020 SUZHOU EU Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：苏州一优信息技术有限公司                                │
*└──────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EU.Model.System
{
    public class SmModuleSql : Base.PersistPoco
    {
       

        /// <summary>
        /// 模块ID
        /// </summary>
        [Display(Name = "模块ID")]
        public Guid? ModuleId { get; set; }

        /// <summary>
        /// 主表名
        /// </summary>
        [Display(Name = "主表名")]
        public string PrimaryTableName { get; set; }

        /// <summary>
        /// 全部表名
        /// </summary>
        [Display(Name = "全部表名")]
        public string TableNames { get; set; }

        /// <summary>
        /// 全部表别名
        /// </summary>
        [Display(Name = "全部表别名")]
        public string TableAliasNames { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [Display(Name = "主键")]
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Select语句
        /// </summary>
        [Display(Name = "Select语句")]
        public string SqlSelect { get; set; }

        /// <summary>
        /// 首页Select语句
        /// </summary>
        [Display(Name = "首页Select语句")]
        public string SqlSelectBrw { get; set; }

        /// <summary>
        /// 关联类型
        /// </summary>
        [Display(Name = "关联类型")]
        public string JoinType { get; set; }

        /// <summary>
        /// 关联表
        /// </summary>
        [Display(Name = "关联表")]
        public string SqlJoinTable { get; set; }

        /// <summary>
        /// 关联表别名
        /// </summary>
        [Display(Name = "关联表别名")]
        public string SqlJoinTableAlias { get; set; }

        /// <summary>
        /// 关联条件
        /// </summary>
        [Display(Name = "关联条件")]
        public string SqlJoinCondition { get; set; }

        /// <summary>
        /// 默认条件
        /// </summary>
        [Display(Name = "默认条件")]
        public string SqlDefaultCondition { get; set; }

        /// <summary>
        /// 回收站条件
        /// </summary>
        [Display(Name = "回收站条件")]
        public string SqlRecycleCondition { get; set; }

        /// <summary>
        /// 初始查询条件
        /// </summary>
        [Display(Name = "初始查询条件")]
        public string SqlQueryCondition { get; set; }

        /// <summary>
        /// 主表默认排序列名
        /// </summary>
        [Display(Name = "主表默认排序列名")]
        public string DefaultSortField { get; set; }

        /// <summary>
        /// 主表默认排序方向
        /// </summary>
        [Display(Name = "主表默认排序方向")]
        public string DefaultSortDirection { get; set; }

        /// <summary>
        /// GROUP_BY
        /// </summary>
        [Display(Name = "GroupBy")]
        public string GroupBy { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }
        
        /// <summary>
        /// 完整SQL
        /// </summary>
        [Display(Name = "完整SQL")]
        public string FullSql { get; set; }

        //public SmModuleSql()
        //{
        //   TableAliasNames=""
        //}
    }
}