using ERCHTMS.Entity.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.RiskDatabase
{
    public class RiskEvaluateMap : EntityTypeConfiguration<RiskEvaluate>
    {
        public RiskEvaluateMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_EVALUATE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
