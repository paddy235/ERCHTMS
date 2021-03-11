using ERCHTMS.Entity.DangerousJob;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJob
{
    /// <summary>
    /// 描 述：盲板作业盲板规格
    /// </summary>
    public class BlindPlateWallSpecMap : EntityTypeConfiguration<BlindPlateWallSpecEntity>
    {
        public BlindPlateWallSpecMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_BLINDPLATEWALLSPEC");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
