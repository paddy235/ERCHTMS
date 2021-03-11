using ERCHTMS.Entity.DangerousJob;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.DangerousJob
{
    public class DangerousJobFlowDetailMap : EntityTypeConfiguration<DangerousJobFlowDetailEntity>
    {
        public DangerousJobFlowDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_DANGEROUSJOBFLOWDETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
