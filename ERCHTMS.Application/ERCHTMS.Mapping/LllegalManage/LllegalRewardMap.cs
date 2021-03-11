using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：违章奖励表
    /// </summary>
    public class LllegalRewardMap : EntityTypeConfiguration<LllegalRewardEntity>
    {
        public LllegalRewardMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALREWARD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}