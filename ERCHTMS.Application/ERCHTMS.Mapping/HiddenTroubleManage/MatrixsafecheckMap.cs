using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：矩阵安全检查计划
    /// </summary>
    public class MatrixsafecheckMap : EntityTypeConfiguration<MatrixsafecheckEntity>
    {
        public MatrixsafecheckMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_MATRIXSAFECHECK");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
