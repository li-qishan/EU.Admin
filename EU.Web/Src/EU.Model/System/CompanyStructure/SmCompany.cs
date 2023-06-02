using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;

namespace EU.Model.System.CompanyStructure
{
    public class SmCompany : PersistPoco
    {
        [Display(Name = "组织编码")]
        [Column(TypeName = "nvarchar(50)")]
        public string CompanyCode { get; set; }

        [Display(Name = "组织简称")]
        [Column(TypeName = "nvarchar(50)")]
        public string CompanyShortName { get; set; }

        [Display(Name = "组织名称")]
        [Column(TypeName = "nvarchar(50)")]
        public string CompanyName { get; set; }
    }
}
