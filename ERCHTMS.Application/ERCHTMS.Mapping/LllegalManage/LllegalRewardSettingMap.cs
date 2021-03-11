using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：奖励设置信息表
    /// </summary>
    public class LllegalRewardSettingMap : EntityTypeConfiguration<LllegalRewardSettingEntity>
    {
        public LllegalRewardSettingMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALREWARDSETTING");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}