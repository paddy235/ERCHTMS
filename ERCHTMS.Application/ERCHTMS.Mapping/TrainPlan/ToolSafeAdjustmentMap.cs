using ERCHTMS.Entity.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.TrainPlan
{
   public class ToolSafeAdjustmentMap : EntityTypeConfiguration<SafeAdjustmentEntity>
    {
        public ToolSafeAdjustmentMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEMEASURE_ADJUSTMENT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
