using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class TaskShareMap : EntityTypeConfiguration<TaskShareEntity>
    {
        public TaskShareMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TASKSHARE");
            //����
            this.HasKey(t => t.Id);
            this.Ignore(t => t.WorkSpecs);
            this.Ignore(t => t.TeamSpec);
            this.Ignore(t => t.StaffSpec);
            this.Ignore(t => t.DelIds);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
