using ERCHTMS.Entity.Observerecord;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.Observerecord
{
   public class ObsTaskFBMap : EntityTypeConfiguration<ObsTaskFBEntity>
    {
        public ObsTaskFBMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OBSTASK_FB");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
