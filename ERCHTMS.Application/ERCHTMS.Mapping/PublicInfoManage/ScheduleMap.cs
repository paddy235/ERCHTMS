using ERCHTMS.Entity.PublicInfoManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PublicInfoManage
{
    /// <summary>
    /// �� �����ճ̹���
    /// </summary>
    public class ScheduleMap : EntityTypeConfiguration<ScheduleEntity>
    {
        public ScheduleMap()
        {
            #region ������
            //��
            this.ToTable("BASE_SCHEDULE");
            //����
            this.HasKey(t => t.ScheduleId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
