using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：违章责任人处罚信息
    /// </summary>
    public class LllegalPunishMap : EntityTypeConfiguration<LllegalPunishEntity>
    {
        public LllegalPunishMap() 
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALPUNISH");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}