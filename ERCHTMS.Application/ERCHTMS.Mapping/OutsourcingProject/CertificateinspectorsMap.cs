using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ա֤����
    /// </summary>
    public class CertificateinspectorsMap : EntityTypeConfiguration<CertificateinspectorsEntity>
    {
        public CertificateinspectorsMap()
        {
            #region ������
            //��
            this.ToTable("EPG_CERTIFICATEINSPECTORS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
