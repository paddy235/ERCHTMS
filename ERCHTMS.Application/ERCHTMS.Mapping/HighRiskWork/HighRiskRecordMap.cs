using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.HighRiskWork;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：高风险作业安全分析
    /// </summary>
    public class HighRiskRecordMap : EntityTypeConfiguration<HighRiskRecordEntity>
    {
        public HighRiskRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIGHRISKRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
