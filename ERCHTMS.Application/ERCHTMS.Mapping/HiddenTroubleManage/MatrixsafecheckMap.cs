using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������ȫ���ƻ�
    /// </summary>
    public class MatrixsafecheckMap : EntityTypeConfiguration<MatrixsafecheckEntity>
    {
        public MatrixsafecheckMap()
        {
            #region ������
            //��
            this.ToTable("BIS_MATRIXSAFECHECK");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
