using ERCHTMS.Entity.EmergencyPlatform;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：演练记录评价表
    /// </summary>
    public class DrillrecordevaluateMap : EntityTypeConfiguration<DrillrecordevaluateEntity>
    {
        public DrillrecordevaluateMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_DRILLRECORDEVALUATE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
