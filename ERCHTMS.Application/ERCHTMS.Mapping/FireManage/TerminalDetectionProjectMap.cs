using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ����������ʩ-���ڼ���¼-���ڼ����Ŀ
    /// </summary>
    public class TerminalDetectionProjectMap : EntityTypeConfiguration<TerminalDetectionProjectEntity>
    {
        public TerminalDetectionProjectMap()
        {
            #region ������
            //��
            this.ToTable("HRS_TERMINALDETECTIONPROJECT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
