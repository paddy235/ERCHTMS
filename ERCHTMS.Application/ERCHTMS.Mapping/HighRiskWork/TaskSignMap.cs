using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �����ල����ǩ��
    /// </summary>
    public class TaskSignMap : EntityTypeConfiguration<TaskSignEntity>
    {
        public TaskSignMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TASKSIGN");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
