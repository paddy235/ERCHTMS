using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：配置危化品检查项目表关联危化品
    /// </summary>
    public class CarcheckitemhazardousMap : EntityTypeConfiguration<CarcheckitemhazardousEntity>
    {
        public CarcheckitemhazardousMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARCHECKITEMHAZARDOUS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
