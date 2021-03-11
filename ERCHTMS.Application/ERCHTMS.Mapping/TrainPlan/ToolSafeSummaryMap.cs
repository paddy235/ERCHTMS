using ERCHTMS.Entity.TrainPlan;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.TrainPlan
{
   public class ToolSafeSummaryMap : EntityTypeConfiguration<SafeSummaryEntity>
    {
        public ToolSafeSummaryMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEMEASURE_SUMMARY");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
