using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ������վ�������
    /// </summary>
    public class TaskUrgeMap : EntityTypeConfiguration<TaskUrgeEntity>
    {
        public TaskUrgeMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TASKURGE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
