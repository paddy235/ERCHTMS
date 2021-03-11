using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：反违章奖励表
    /// </summary>
    public class LllegalAwardDetailMap : EntityTypeConfiguration<LllegalAwardDetailEntity>
    {
        public LllegalAwardDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALAWARDDETAIL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}