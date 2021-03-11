using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// 描 述：观察计划
    /// </summary>
    public class ObsplanMap : EntityTypeConfiguration<ObsplanEntity>
    {
        public ObsplanMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OBSPLAN");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
