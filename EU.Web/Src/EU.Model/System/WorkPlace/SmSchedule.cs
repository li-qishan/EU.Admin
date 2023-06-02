using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.System
{
    public class SmSchedule : PersistPoco
    {
        [Display(Name = "标题")]
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }

        [Display(Name = "内容")]
        [Column(TypeName = "nvarchar(50)")]
        public string Content { get; set; }

        [Display(Name = "状态")]
        [Column(TypeName = "nvarchar(50)")]
        public string Status { get; set; }

        public string Path { get; set; }

        [Display(Name = "数据ID")]
        public Guid? MasterId { get; set; }

        [Display(Name = "模块ID")]
        public Guid? ModuleId { get; set; }

        [Display(Name = "用户")]
        public Guid? UserId { get; set; }
    }
}
