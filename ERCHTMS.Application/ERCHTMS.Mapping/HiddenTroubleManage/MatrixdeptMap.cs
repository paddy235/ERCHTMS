using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ���������鲿��ά����
    /// </summary>
    public class MatrixdeptMap : EntityTypeConfiguration<MatrixdeptEntity>
    {
        public MatrixdeptMap()
        {
            #region ������
            //��
            this.ToTable("BIS_MATRIXDEPT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
