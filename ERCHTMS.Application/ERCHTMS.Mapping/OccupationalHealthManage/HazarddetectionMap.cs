using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病危害因素监测
    /// </summary>
    public class HazarddetectionMap : EntityTypeConfiguration<HazarddetectionEntity>
    {
        public HazarddetectionMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HAZARDDETECTION");
            //主键
            this.HasKey(t => t.HId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
