using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class ConferenceMap : EntityTypeConfiguration<ConferenceEntity>
    {
        public ConferenceMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CONFERENCE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
