using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护回收报废表详情
    /// </summary>
    public class LaborrecyclingMap : EntityTypeConfiguration<LaborrecyclingEntity>
    {
        public LaborrecyclingMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LABORRECYCLING");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
