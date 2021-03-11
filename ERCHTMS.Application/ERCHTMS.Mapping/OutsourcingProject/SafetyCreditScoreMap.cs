using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;


namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：安全信用评价分数表
    /// </summary>
    public class SafetyCreditScoreMap : EntityTypeConfiguration<SafetyCreditScoreEntity>
    {
        public SafetyCreditScoreMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_SAFETYCREDITSCORE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
