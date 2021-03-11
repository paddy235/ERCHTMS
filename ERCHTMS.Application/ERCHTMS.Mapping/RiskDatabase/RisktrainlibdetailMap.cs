using ERCHTMS.Entity.RiskDatabase;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// 描 述：风险预知训练库管控措施
    /// </summary>
    public class RisktrainlibdetailMap : EntityTypeConfiguration<RisktrainlibdetailEntity>
    {
        public RisktrainlibdetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_RISKTRAINLIBDETAIL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
