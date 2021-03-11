using ERCHTMS.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.BaseManage
{
    /// <summary>
    /// 描 述：区域设置
    /// </summary>
    public class DistrictMap : EntityTypeConfiguration<DistrictEntity>
    {
        public DistrictMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_DISTRICT");
            //主键
            this.HasKey(t => t.DistrictID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
