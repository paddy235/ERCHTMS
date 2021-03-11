using ERCHTMS.Entity.PersonManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.PersonManage
{
    public class TemporaryUserMap : EntityTypeConfiguration<TemporaryUserEntity>
    {
        public TemporaryUserMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TEMPORARYUSER");
            //主键
            this.HasKey(t => t.USERID);
            #endregion

        }
    }
}
