using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.HseManage
{
    [Table("HSE_WARNINGCARD")]
    public class WarningCardEntity
    {
        [Key]
        [Column("CARDID")]
        public string CardId { get; set; }
        [Column("CARDNAME")]
        public string CardName { get; set; }
        [Column("CATEGORY")]
        public string Category { get; set; }
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        [Column("CREATETIME")]
        public DateTime CreateTime { get; set; }
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        [Column("MODIFYTIME")]
        public DateTime ModifyTime { get; set; }
        [Column("SUBMITUSER")]
        public string SubmitUser { get; set; }
        [Column("SUBMITTIME")]
        public DateTime SubmitTime { get; set; }
        [Column("DEPTID")]
        public string DeptId { get; set; }
        [NotMapped]
        public List<CheckContentEntity> CheckContents { get; set; }
    }

    [Table("HSE_CHECKCONTENT")]
    public class CheckContentEntity
    {
        [Key]
        [Column("CHECKCONTENTID")]
        public string CheckContentId { get; set; }
        [Column("CONTENT")]
        public string Content { get; set; }
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        [Column("CREATETIME")]
        public DateTime CreateTime { get; set; }
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        [Column("MODIFYTIME")]
        public DateTime ModifyTime { get; set; }
        [Column("CARDID")]
        public string CardId { get; set; }
    }
}
