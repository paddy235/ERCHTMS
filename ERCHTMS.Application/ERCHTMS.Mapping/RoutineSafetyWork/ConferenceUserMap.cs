using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// �� ������ȫ����λ���Ա��
    /// </summary>
    public class ConferenceUserMap : EntityTypeConfiguration<ConferenceUserEntity>
    {
        public ConferenceUserMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CONFERENCEUSER");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
