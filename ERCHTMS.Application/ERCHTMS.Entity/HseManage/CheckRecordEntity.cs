using ERCHTMS.Entity.PublicInfoManage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HseManage
{
    [Table("HSE_CHECKRECORD")]
    public class CheckRecordEntity
    {
        [Key]
        [Column("CHECKRECORDID")]
        public string CheckRecordId { get; set; }
        [Column("CARDID")]
        public string CardId { get; set; }
        [Column("CARDNAME")]
        public string CardName { get; set; }
        [Column("CATEGORY")]
        public string Category { get; set; }
        [Column("CHECKPLACE")]
        public string CheckPlace { get; set; }
        [Column("CHECKUSER")]
        public string CheckUser { get; set; }
        [Column("CHECKTIME")]
        public DateTime CheckTime { get; set; }
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        [Column("CREATETIME")]
        public DateTime CreateTime { get; set; }
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        [Column("MODIFYTIME")]
        public DateTime ModifyTime { get; set; }
        [Column("DEPTID")]
        public string DeptId { get; set; }
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        [NotMapped]
        public int? Num1 { get; set; }
        [NotMapped]
        public int? Num2 { get; set; }
        [NotMapped]
        public List<CheckItemEntity> CheckItems { get; set; }
        [NotMapped]
        public List<FileInfoEntity> Files { get; set; }
    }

    [Table("HSE_CHECKITEM")]
    public class CheckItemEntity
    {
        [Key]
        [Column("CHECKITEMID")]
        public string CheckItemId { get; set; }
        [Column("CHECKRECORDID")]
        public string CheckRecordId { get; set; }
        [Column("CHECKCONTENTID")]
        public string CheckContentId { get; set; }
        [Column("CHECKCONTENT")]
        public string CheckContent { get; set; }
        [Column("NUM1")]
        public int Num1 { get; set; }
        [Column("NUM2")]
        public int Num2 { get; set; }
        [Column("NUM3")]
        public int Num3 { get; set; }
        [Column("NUM4")]
        public int Num4 { get; set; }
        [Column("DANGEROUS")]
        public string Dangerous { get; set; }
        [Column("MEASURES")]
        public string Measures { get; set; }
        [Column("DANGEROUS2")]
        public string Dangerous2 { get; set; }
        [Column("MEASURES2")]
        public string Measures2 { get; set; }
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        [Column("CREATETIME")]
        public DateTime CreateTime { get; set; }
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        [Column("MODIFYTIME")]
        public DateTime ModifyTime { get; set; }
        [NotMapped]
        public string clientid { get; set; }
        [NotMapped]
        public List<FileInfoEntity> Files { get; set; }
    }
}
