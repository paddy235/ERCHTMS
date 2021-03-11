using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：工程流程状态表
    /// </summary>
    public class StartappprocessstatusMap : EntityTypeConfiguration<StartappprocessstatusEntity>
    {
        public StartappprocessstatusMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_STARTAPPPROCESSSTATUS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
