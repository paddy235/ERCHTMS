using ERCHTMS.Entity.DangerousJob;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJob
{
    /// <summary>
    /// 描 述：危险作业清单
    /// </summary>
    public class DangerjoblistMap : EntityTypeConfiguration<DangerjoblistEntity>
    {
        public DangerjoblistMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_DANGERJOBLIST");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
