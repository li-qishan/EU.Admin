using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.System.WorkFlow
{
    //流程节点
    public class SmNode : PersistPoco
    {
        public virtual SmProjectFlow SmProjectFlow { get; set; }

        public Guid? SmProjectFlowId { get; set; }

        [Display(Name = "驳回类型")]
        [Column(TypeName = "nvarchar(50)")]
        public string RejectionType { get; set; }

        [NotMapped]
        public List<string> Roles { get; set; }

        [Display(Name = "指定角色")]
        public string role { get; set; }

        public string color { get; set; }

        public string nodeid { get; set; }

        public string index { get; set; }

        public string label { get; set; }

        public string shape { get; set; }

        public string size { get; set; }

        public string type { get; set; }

        public string x { get; set; }

        public string y { get; set; }
    }
}
