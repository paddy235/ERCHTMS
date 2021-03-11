using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.SafeNote;

namespace ERCHTMS.Mapping.SafeNote
{
    public class SafeNoteMap : EntityTypeConfiguration<SafeNoteEntity>
    {
        public SafeNoteMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFENOTE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
