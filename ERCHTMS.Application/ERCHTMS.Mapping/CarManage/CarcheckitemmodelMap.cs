using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ��������Σ��Ʒ�����Ŀ��ģ������
    /// </summary>
    public class CarcheckitemmodelMap : EntityTypeConfiguration<CarcheckitemmodelEntity>
    {
        public CarcheckitemmodelMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARCHECKITEMMODEL");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
