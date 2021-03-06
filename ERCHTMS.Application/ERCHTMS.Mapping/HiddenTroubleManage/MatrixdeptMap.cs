using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：矩阵检查部门维护表
    /// </summary>
    public class MatrixdeptMap : EntityTypeConfiguration<MatrixdeptEntity>
    {
        public MatrixdeptMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_MATRIXDEPT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
