using ERCHTMS.Entity.SafeReward;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafeReward
{
    /// <summary>
    /// 描 述：安全奖励详细
    /// </summary>
    public class SaferewarddetailMap : EntityTypeConfiguration<SaferewarddetailEntity>
    {
        public SaferewarddetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEREWARDDETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
