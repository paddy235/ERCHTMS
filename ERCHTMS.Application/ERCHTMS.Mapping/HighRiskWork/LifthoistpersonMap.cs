using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：起重吊装作业操作人员表
    /// </summary>
    public class LifthoistpersonMap : EntityTypeConfiguration<LifthoistpersonEntity>
    {
        public LifthoistpersonMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LIFTHOISTPERSON");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
