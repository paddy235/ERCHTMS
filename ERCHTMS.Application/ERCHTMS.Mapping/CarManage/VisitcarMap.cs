using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：拜访车辆表
    /// </summary>
    public class VisitcarMap : EntityTypeConfiguration<VisitcarEntity>
    {
        public VisitcarMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_VISITCAR");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
