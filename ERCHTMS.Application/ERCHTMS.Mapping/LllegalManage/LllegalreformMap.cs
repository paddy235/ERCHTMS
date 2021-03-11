using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：违章整改信息
    /// </summary>
    public class LllegalReformMap : EntityTypeConfiguration<LllegalReformEntity>
    {
        public LllegalReformMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALREFORM");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}