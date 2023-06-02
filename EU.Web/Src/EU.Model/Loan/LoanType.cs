using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EU.Domain;

namespace EU.Model.Loan
{
    public class LoanType : PersistPoco
    {
        [Display(Name = "ID")]
        public Guid? ID { get; set; }

        [Display(Name = "贷款类型")]
        [Column(TypeName = "varchar(32)")]
        public string TypeName { get; set; }
    }
}
