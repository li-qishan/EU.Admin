using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EU.Domain;
using EU.Model.System;
using Microsoft.AspNetCore.Components;

namespace EU.Model.BFProject
{
    //发票登记
    public class InvoiceRegist : PersistPoco
    {
        public virtual BFContract Contract { get; set; }
        public Guid? ContractId { get; set; }


        [Display(Name = "登记日期")]
        [Column(TypeName = "datetime")]
        public DateTime RegistDate { get; set; }

        [Display(Name = "发票金额")]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal InvoiceAmount { get; set; }

        [Display(Name = "是否报销")]
        public bool IsRefund { get; set; }
    }
}
