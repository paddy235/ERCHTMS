using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：配置危化品检查项目表
    /// </summary>
    public class CarcheckitemMap : EntityTypeConfiguration<CarcheckitemEntity>
    {
        public CarcheckitemMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARCHECKITEM");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
