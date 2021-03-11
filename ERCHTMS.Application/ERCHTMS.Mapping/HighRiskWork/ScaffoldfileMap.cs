using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架方案上传，可上传多张
    /// </summary>
    public class ScaffoldfileMap : EntityTypeConfiguration<ScaffoldfileEntity>
    {
        public ScaffoldfileMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SCAFFOLDFILE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
