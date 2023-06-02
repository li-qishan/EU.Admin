using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.System.CompanyStructure
{
    public class SmDepartment : PersistPoco
    {
        public virtual SmCompany Company { get; set; }

        public Guid? CompanyId { get; set; }

        public virtual SmDepartment Department { get; set; }

        public Guid? DepartmentId { get; set; }

        [Display(Name = "部门编码")]
        [Column(TypeName = "nvarchar(50)")]
        public string DepartmentCode { get; set; }

        [Display(Name = "部门名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string DepartmentName { get; set; }
    }
}
