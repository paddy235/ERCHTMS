using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人员表
    /// </summary>
    public class OccupatioalstaffMap : EntityTypeConfiguration<OccupatioalstaffEntity>
    {
        public OccupatioalstaffMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OCCUPATIOALSTAFF");
            //主键
            this.HasKey(t => t.OccId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
