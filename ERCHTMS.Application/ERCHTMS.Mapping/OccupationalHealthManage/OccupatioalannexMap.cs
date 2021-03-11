using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// 描 述：职业病人附件表
    /// </summary>
    public class OccupatioalannexMap : EntityTypeConfiguration<OccupatioalannexEntity>
    {
        public OccupatioalannexMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OCCUPATIOALANNEX");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
