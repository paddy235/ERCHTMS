using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������ά����
    /// </summary>
    public class MatrixcontentMap : EntityTypeConfiguration<MatrixcontentEntity>
    {
        public MatrixcontentMap()
        {
            #region ������
            //��
            this.ToTable("BIS_MATRIXCONTENT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
