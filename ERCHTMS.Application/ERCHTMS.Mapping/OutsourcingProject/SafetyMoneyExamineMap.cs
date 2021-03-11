using ERCHTMS.Entity.OutsourcingProject;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    public class SafetyMoneyExamineMap : EntityTypeConfiguration<SafetyMoneyExamineEntity>
    {
        public SafetyMoneyExamineMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SAFETYMONEYEXAMINE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
