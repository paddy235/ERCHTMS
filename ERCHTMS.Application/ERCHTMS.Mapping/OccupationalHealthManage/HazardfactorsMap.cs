using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：危险因素清单
    /// </summary>
    public class HazardfactorsMap : EntityTypeConfiguration<HazardfactorsEntity>
    {
        public HazardfactorsMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HAZARDFACTORS");
            //主键
            this.HasKey(t => t.Hid);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
