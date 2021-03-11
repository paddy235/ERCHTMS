using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.SystemManage
{
    [Table("BASE_REGISTMANAGE")]
   public class RegistManageEntity : BaseEntity
    {
        [Column("ID")]
        public string Id { get; set; }
        [Column("REGISTCODE")]
        public string RegistCode { get; set; }
        [Column("APIURL")]
        public string ApiUrl { get; set; }
        [Column("REMARK")]
        public string Remark { get; set; }
        [Column("BZAPIURL")]
        public string BZApiUrl { get; set; }
        [Column("SKAPIURL")]
        public string SKApiUrl { get; set; }
        [Column("PXAPIURL")]
        public string PXApiUrl { get; set; }

        public override void Create()
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = Guid.NewGuid().ToString();
            }
        }
    }
}
