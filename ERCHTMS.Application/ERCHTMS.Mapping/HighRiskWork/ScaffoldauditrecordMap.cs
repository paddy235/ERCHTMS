using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּ���˼�¼��
    /// </summary>
    public class ScaffoldauditrecordMap : EntityTypeConfiguration<ScaffoldauditrecordEntity>
    {
        public ScaffoldauditrecordMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SCAFFOLDAUDITRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
