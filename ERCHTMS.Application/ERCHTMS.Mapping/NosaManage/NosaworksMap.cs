using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class NosaworksMap : EntityTypeConfiguration<NosaworksEntity>
    {
        public NosaworksMap()
        {
            #region ������
            //��
            this.ToTable("HRS_NOSAWORKS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
