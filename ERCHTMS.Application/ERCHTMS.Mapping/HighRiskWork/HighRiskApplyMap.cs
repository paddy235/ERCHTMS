using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �����߷�����ҵ�������
    /// </summary>
    public class HighRiskApplyMap : EntityTypeConfiguration<HighRiskApplyEntity>
    {
        public HighRiskApplyMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIGHRISKAPPLY");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
