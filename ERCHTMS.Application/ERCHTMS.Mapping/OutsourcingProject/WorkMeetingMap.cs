using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �������չ���
    /// </summary>
    public class WorkMeetingEntityMap : EntityTypeConfiguration<WorkMeetingEntity>
    {
        public WorkMeetingEntityMap()
        {
            #region ������
            //��
            this.ToTable("BIS_WORKMEETING");
            //����
            this.HasKey(t => t.ID);
            this.Ignore(t => t.FILES);
            this.Ignore(t => t.DELETEFILEID);
            this.Ignore(t => t.OUTPROJECTNAME);
            this.Ignore(t => t.OUTPROJECTCODE);
            this.Ignore(t => t.MeasuresList);
            this.Ignore(t => t.ids);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
