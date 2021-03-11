using ERCHTMS.Entity.ComprehensiveManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    public class HonoursMap : EntityTypeConfiguration<HonoursEntity>
    {
        public HonoursMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_HONOURS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
