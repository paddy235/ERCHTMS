using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：消防设施
    /// </summary>
    public class FirefightingMap : EntityTypeConfiguration<FirefightingEntity>
    {
        public FirefightingMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_FIREFIGHTING");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
