using ERCHTMS.Entity.EquipmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EquipmentManage
{
    /// <summary>
    /// �� �����ճ�ʹ��״����¼��
    /// </summary>
    public class DailyUseRecordMap : EntityTypeConfiguration<DailyUseRecordEntity>
    {
        public DailyUseRecordMap()
        {
            #region ������
            //��
            this.ToTable("BIS_DAILYUSERECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
