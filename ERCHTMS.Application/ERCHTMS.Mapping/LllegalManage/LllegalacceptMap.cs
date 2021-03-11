using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：违章验收信息
    /// </summary>
    public class LllegalAcceptMap : EntityTypeConfiguration<LllegalAcceptEntity>
    {
        public LllegalAcceptMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALACCEPT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}