using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    public class SafetyCreditEvaluateMap: EntityTypeConfiguration<SafetyCreditEvaluateEntity>
    {
        public SafetyCreditEvaluateMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SAFETYCREDITEVALUATE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
