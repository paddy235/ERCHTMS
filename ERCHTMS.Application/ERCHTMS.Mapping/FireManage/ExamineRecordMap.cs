using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ��������¼
    /// </summary>
    public class ExamineRecordMap : EntityTypeConfiguration<ExamineRecordEntity>
    {
        public ExamineRecordMap()
        {
            #region ������
            //��
            this.ToTable("HRS_EXAMINERECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
