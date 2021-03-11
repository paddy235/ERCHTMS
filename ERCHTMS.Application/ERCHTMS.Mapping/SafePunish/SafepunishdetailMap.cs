using ERCHTMS.Entity.SafePunish;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafePunish
{
    /// <summary>
    /// 描 述：安全考核详细
    /// </summary>
    public class SafepunishdetailMap : EntityTypeConfiguration<SafepunishdetailEntity>
    {
        public SafepunishdetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SAFEPUNISHDETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
