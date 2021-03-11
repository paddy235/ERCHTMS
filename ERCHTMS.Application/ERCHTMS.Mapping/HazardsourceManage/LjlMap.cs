using ERCHTMS.Entity.HazardsourceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HazardsourceManage
{
    /// <summary>
    /// 描 述：危险化学品值信息
    /// </summary>
    public class LjlMap : EntityTypeConfiguration<LjlEntity>
    {
        public LjlMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSD_LJL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
