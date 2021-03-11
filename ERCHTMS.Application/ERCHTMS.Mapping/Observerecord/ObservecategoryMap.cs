using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// 描 述：观察类别表
    /// </summary>
    public class ObservecategoryMap : EntityTypeConfiguration<ObservecategoryEntity>
    {
        public ObservecategoryMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OBSERVECATEGORY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}