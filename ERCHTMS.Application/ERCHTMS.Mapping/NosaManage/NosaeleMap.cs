using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// �� ����Ԫ�ر�
    /// </summary>
    public class NosaeleMap : EntityTypeConfiguration<NosaeleEntity>
    {
        public NosaeleMap()
        {
            #region ������
            //��
            this.ToTable("HRS_NOSAELE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
