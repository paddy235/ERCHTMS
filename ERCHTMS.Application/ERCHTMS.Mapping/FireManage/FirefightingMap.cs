using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ����������ʩ
    /// </summary>
    public class FirefightingMap : EntityTypeConfiguration<FirefightingEntity>
    {
        public FirefightingMap()
        {
            #region ������
            //��
            this.ToTable("HRS_FIREFIGHTING");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
