using ERCHTMS.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.BaseManage
{
    public class ManyPowerCheckMap : EntityTypeConfiguration<ManyPowerCheckEntity>
    {
        public ManyPowerCheckMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_MANYPOWERCHECK");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
