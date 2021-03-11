using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：起吊作业
    /// </summary>
    public class LifthoistauditrecordMap : EntityTypeConfiguration<LifthoistauditrecordEntity>
    {
        public LifthoistauditrecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LIFTHOISTAUDITRECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
