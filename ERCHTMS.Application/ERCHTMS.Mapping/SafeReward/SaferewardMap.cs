using ERCHTMS.Entity.SafeReward;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafeReward
{
    /// <summary>
    /// 描 述：安全奖励
    /// </summary>
    public class SaferewardMap : EntityTypeConfiguration<SaferewardEntity>
    {
        public SaferewardMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEREWARD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
