using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �����ճ����˱�
    /// </summary>
    public class DailyexamineMap : EntityTypeConfiguration<DailyexamineEntity>
    {
        public DailyexamineMap()
        {
            #region ������
            //��
            this.ToTable("EPG_DAILYEXAMINE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
