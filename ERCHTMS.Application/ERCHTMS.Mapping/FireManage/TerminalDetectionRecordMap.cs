using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ����������ʩ-���ڼ���¼
    /// </summary>
    public class TerminalDetectionRecordMap : EntityTypeConfiguration<TerminalDetectionRecordEntity>
    {
        public TerminalDetectionRecordMap()
        {
            #region ������
            //��
            this.ToTable("HRS_TERMINALDETECTIONRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
