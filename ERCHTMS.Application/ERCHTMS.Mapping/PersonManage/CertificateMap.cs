using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public class CertificateMap : EntityTypeConfiguration<CertificateEntity>
    {
        public CertificateMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CERTIFICATE");
            //����
            this.HasKey(t => t.Id);
            this.Ignore(t=>t.Result);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
