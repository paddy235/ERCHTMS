using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：起吊证
    /// </summary>
    public class LifthoistcertMap : EntityTypeConfiguration<LifthoistcertEntity>
    {
        public LifthoistcertMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LIFTHOISTCERT");
            //主键
            this.HasKey(t => t.ID);
            this.Ignore(t => t.safetys);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
