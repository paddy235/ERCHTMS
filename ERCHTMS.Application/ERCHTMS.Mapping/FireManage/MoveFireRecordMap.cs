using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ���������¼���ص����λ�ӱ�
    /// </summary>
    public class MoveFireRecordMap : EntityTypeConfiguration<MoveFireRecordEntity>
    {
        public MoveFireRecordMap()
        {
            #region ������
            //��
            this.ToTable("HRS_MOVEFIRERECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
