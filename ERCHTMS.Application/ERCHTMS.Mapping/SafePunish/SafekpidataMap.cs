using ERCHTMS.Entity.SafePunish;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafePunish
{
    /// <summary>
    /// 描 述：安全惩罚
    /// </summary>
    public class SafekpidataMap : EntityTypeConfiguration<SafekpidataEntity>
    {
        public SafekpidataMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEKPIDATA");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
