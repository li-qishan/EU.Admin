using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.System.Setup
{
    public enum DateTypeEnum
    {
        YYYYMMDDHHMM,
        YYYYMMDDHH,
        YYYYMMDD,
        YYYYMM,
        YYYY
    }

    public class SmAutoCode : PersistPoco
    {
        [Display(Name = "编号代码")]
        [Column(TypeName = "nvarchar(50)")]
        public string NumberCode { get; set; }

        [Display(Name = "前缀")]
        [Column(TypeName = "nvarchar(50)")]
        public string Prefix { get; set; }

        [Display(Name = "日期格式")]
        public string DateFormatType { get; set; }

        [Display(Name = "长度")]
        [Column(TypeName = "int")]
        public int NumberLength { get; set; }

        [Display(Name = "表名")]
        [Column(TypeName = "nvarchar(50)")]
        public string TableName { get; set; }

        [Display(Name = "列名")]
        [Column(TypeName = "nvarchar(50)")]
        public string ColumnName { get; set; }
    }
}
