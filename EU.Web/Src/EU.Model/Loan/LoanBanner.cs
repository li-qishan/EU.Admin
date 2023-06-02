using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EU.Domain;

namespace EU.Model.Loan
{
    public class LoanBanner : PersistPoco
    {
        [Display(Name = "ID")]
        public Guid? ID { get; set; }


        [Display(Name = "横幅名称")]
        [Column(TypeName = "varchar(32)")]
        public string BannerName { get; set; }

        [Display(Name = "图片URL")]
        [Column(TypeName = "varchar(32)")]
        public string ImageUrl { get; set; }

        [Display(Name = "排序号")]
        public int TaxisNo { get; set; }
    }
}
