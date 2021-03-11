using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල����
    /// </summary>
    public class SuperviseTaskMap : EntityTypeConfiguration<SuperviseTaskEntity>
    {
        public SuperviseTaskMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SUPERVISETASK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
