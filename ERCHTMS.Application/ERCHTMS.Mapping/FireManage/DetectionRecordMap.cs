using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ������⡢ά����¼
    /// </summary>
    public class DetectionRecordMap : EntityTypeConfiguration<DetectionRecordEntity>
    {
        public DetectionRecordMap()
        {
            #region ������
            //��
            this.ToTable("HRS_DETECTIONRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
