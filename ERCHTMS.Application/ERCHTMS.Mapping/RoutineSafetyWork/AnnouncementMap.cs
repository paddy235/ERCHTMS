using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// �� ����֪ͨ����
    /// </summary>
    public class AnnouncementMap : EntityTypeConfiguration<AnnouncementEntity>
    {
        public AnnouncementMap()
        {
            #region ������
            //��
            this.ToTable("BIS_ANNOUNCEMENT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
