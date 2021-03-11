using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督人员(高风险作业)
    /// </summary>
    public class SidePersonMap : EntityTypeConfiguration<SidePersonEntity>
    {
        public SidePersonMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SIDEPERSON");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
