using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// �� ������ѵ�ļ������
    /// </summary>
    public class NosatratypeMap : EntityTypeConfiguration<NosatratypeEntity>
    {
        public NosatratypeMap()
        {
            #region ������
            //��
            this.ToTable("HRS_NOSATRATYPE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
