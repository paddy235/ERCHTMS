using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ʷ
    /// </summary>
    public class HistorydailyexamineMap : EntityTypeConfiguration<HistorydailyexamineEntity>
    {
        public HistorydailyexamineMap()
        {
            #region ������
            //��
            this.ToTable("EPG_HISTORYDAILYEXAMINE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
