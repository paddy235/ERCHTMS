using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ����Ѳ���¼���ص����λ�ӱ�
    /// </summary>
    public class PatrolRecordMap : EntityTypeConfiguration<PatrolRecordEntity>
    {
        public PatrolRecordMap()
        {
            #region ������
            //��
            this.ToTable("HRS_PATROLRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
