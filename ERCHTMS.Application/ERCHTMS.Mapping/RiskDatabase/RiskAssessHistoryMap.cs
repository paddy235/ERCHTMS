using ERCHTMS.Entity.HazardsourceManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.RiskDatabase
{
    public class RiskAssessHistoryMap : EntityTypeConfiguration<RiskAssessHistoryEntity>
    {
        public RiskAssessHistoryMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKASSESSHISTORY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
