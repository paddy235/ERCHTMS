using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ��������Ѳ��ȷ�ϼ�¼
    /// </summary>
    public class AffirmRecordMap : EntityTypeConfiguration<AffirmRecordEntity>
    {
        public AffirmRecordMap()
        {
            #region ������
            //��
            this.ToTable("HRS_AFFIRMRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
