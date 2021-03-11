using ERCHTMS.Entity.ComprehensiveManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    public class AnnouncementMap : EntityTypeConfiguration<AnnouncementsEntity>
    {
        public AnnouncementMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_ANNOUNCEMENT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
