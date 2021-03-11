using ERCHTMS.Entity.HseManage;
using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�����Ԥ����
    /// </summary>
    public class CheckRecordMap : EntityTypeConfiguration<CheckRecordEntity>
    {
        public CheckRecordMap()
        {
            #region ������
            //��
            this.ToTable("HSE_CHECKRECORD");
            //����
            this.HasKey(t => t.CheckRecordId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }

    public class CheckItemMap : EntityTypeConfiguration<CheckItemEntity>
    {
        public CheckItemMap()
        {
            this.ToTable("HSE_CHECKITEM");
            //����
            this.HasKey(t => t.CheckItemId);
        }
    }
}
