using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// �� ������Ա֤��
    /// </summary>
    public class CertAuditMap : EntityTypeConfiguration<CertAuditEntity>
    {
        public CertAuditMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CERTAUDIT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
