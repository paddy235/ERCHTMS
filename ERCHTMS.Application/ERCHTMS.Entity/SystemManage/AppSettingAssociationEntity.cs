using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Entity.SystemManage
{
    [Table("BASE_APPSETTINGASSOCIATION")]
    public class AppSettingAssociationEntity : BaseEntity
    {

        [Column("ID")]
        public string Id { get; set; }
        [Column("DEPTID")]
        public string DeptId { get; set; }
        [Column("COLUMNID")]
        public string ColumnId { get; set; }
        [Column("MODULEID")]
        public string ModuleId { get; set; }
        [Column("SORT")]
        public int? Sort { get; set; }
        [Column("COLUMNNAME")]
        public string ColumnName { get; set; }

        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }

        public AppSettingAssociationEntity Clone(string deptid)
        {
            AppSettingAssociationEntity association = new AppSettingAssociationEntity()
            {
                Id = Guid.NewGuid().ToString(),
                ColumnId = this.ColumnId,
                ColumnName = this.ColumnName,
                DeptId = deptid,
                ModuleId = this.ModuleId,
                Sort = this.Sort
            };
            return association;
        }

     
    }
}
