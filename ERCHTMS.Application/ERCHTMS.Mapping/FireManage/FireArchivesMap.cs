using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class FireArchivesMap : EntityTypeConfiguration<FireArchivesEntity>
    {
        public FireArchivesMap()
        {
            #region ������
            //��
            this.ToTable("HRS_FIREARCHIVES");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
