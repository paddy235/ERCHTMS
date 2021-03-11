using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// 描 述：观察记录表
    /// </summary>
    public class ObserverecordMap : EntityTypeConfiguration<ObserverecordEntity>
    {
        public ObserverecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OBSERVERECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
