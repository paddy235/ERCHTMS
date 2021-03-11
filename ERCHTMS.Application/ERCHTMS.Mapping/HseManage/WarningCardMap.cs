using ERCHTMS.Entity.HseManage;
using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�����Ԥ����
    /// </summary>
    public class WarningCardMap : EntityTypeConfiguration<WarningCardEntity>
    {
        public WarningCardMap()
        {
            #region ������
            //��
            this.ToTable("HSE_WARNINGCARD");
            //����
            this.HasKey(t => t.CardId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }

    public class CheckContentMap : EntityTypeConfiguration<CheckContentEntity>
    {
        public CheckContentMap()
        {
            this.ToTable("HSE_CHECKCONTENT");
            //����
            this.HasKey(t => t.CheckContentId);
        }
    }
}
