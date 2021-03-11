using ERCHTMS.Entity.EmergencyPlatform;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    public class DrillplanrecordHistoryMap: EntityTypeConfiguration<DrillplanrecordHistoryEntity>
    {
        public DrillplanrecordHistoryMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_DRILLPLANRECORDHISTORY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
