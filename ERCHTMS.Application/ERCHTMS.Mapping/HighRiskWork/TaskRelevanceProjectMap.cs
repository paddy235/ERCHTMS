using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �����Ѽ��ļ����Ŀ
    /// </summary>
    public class TaskRelevanceProjectMap : EntityTypeConfiguration<TaskRelevanceProjectEntity>
    {
        public TaskRelevanceProjectMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TASKRELEVANCEPROJECT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
