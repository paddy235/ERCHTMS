using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �����߷���ͨ����ҵ����
    /// </summary>
    public class HighRiskCommonApplyMap : EntityTypeConfiguration<HighRiskCommonApplyEntity>
    {
        public HighRiskCommonApplyMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIGHRISKCOMMONAPPLY");
            //����
            this.HasKey(t => t.Id);
            this.Ignore(t => t.DeleteFileIds);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
