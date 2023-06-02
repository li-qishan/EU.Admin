using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EU.Domain;
using EU.Entity;

namespace EU.Model.System
{
    [Entity(TableCnName = "", TableName = "FileAttachment")]
    public class FileAttachment : PersistPoco
    {
        [Display(Name = "主表ID")]
        public Guid? MasterId { get; set; }

        [Display(Name = "OriginalFileName")]
        public string OriginalFileName { get; set; }

        [Display(Name = "FileName")]
        public string FileName { get; set; }

        [Display(Name = "FileExt")]
        [StringLength(10)]
        public string FileExt { get; set; }

        [Display(Name = "Path")]
        public string Path { get; set; }

        [Display(Name = "Length")]
        public long Length { get; set; }

        public byte[] FileData { get; set; }

        [Display(Name = "ImageType")]
        public string ImageType { get; set; }
    }
}
