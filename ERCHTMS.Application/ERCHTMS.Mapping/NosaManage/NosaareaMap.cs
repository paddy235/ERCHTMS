using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// �� ����Nosa�����
    /// </summary>
    public class NosaareaMap : EntityTypeConfiguration<NosaareaEntity>
    {
        public NosaareaMap()
        {
            #region ������
            //��
            this.ToTable("HRS_NOSAAREA");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
