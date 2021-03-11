using ERCHTMS.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.BaseManage
{
    /// <summary>
    /// 描 述：黑名单条件设置
    /// </summary>
    public class BlackSetMap : EntityTypeConfiguration<BlackSetEntity>
    {
        public BlackSetMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_BLACKSET");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
