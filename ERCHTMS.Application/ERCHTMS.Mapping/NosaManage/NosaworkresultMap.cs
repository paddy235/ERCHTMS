using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// �� ���������ɹ�
    /// </summary>
    public class NosaworkresultMap : EntityTypeConfiguration<NosaworkresultEntity>
    {
        public NosaworkresultMap()
        {
            #region ������
            //��
            this.ToTable("HRS_NOSAWORKRESULT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
