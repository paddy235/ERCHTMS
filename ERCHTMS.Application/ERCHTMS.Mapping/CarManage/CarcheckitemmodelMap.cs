using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：配置危化品检查项目表模板详情
    /// </summary>
    public class CarcheckitemmodelMap : EntityTypeConfiguration<CarcheckitemmodelEntity>
    {
        public CarcheckitemmodelMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARCHECKITEMMODEL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
