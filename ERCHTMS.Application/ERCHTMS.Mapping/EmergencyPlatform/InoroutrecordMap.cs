using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����������¼
    /// </summary>
    public class InoroutrecordMap : EntityTypeConfiguration<InoroutrecordEntity>
    {
        public InoroutrecordMap()
        {
            #region ������
            //��
            this.ToTable("MAE_INOROUTRECORD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
