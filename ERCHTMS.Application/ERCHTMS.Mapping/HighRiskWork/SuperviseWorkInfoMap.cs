using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：旁站监督作业信息
    /// </summary>
    public class SuperviseWorkInfoMap : EntityTypeConfiguration<SuperviseWorkInfoEntity>
    {
        public SuperviseWorkInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SUPERVISEWORKINFO");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
