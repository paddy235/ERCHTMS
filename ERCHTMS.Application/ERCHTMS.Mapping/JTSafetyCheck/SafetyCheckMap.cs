using ERCHTMS.Entity.JTSafetyCheck;
using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.JTSafetyCheck
{
    /// <summary>
    /// �� ��������λ��
    /// </summary>
    public class SafetyCheckMap : EntityTypeConfiguration<SafetyCheckEntity>
    {
        public SafetyCheckMap()
        {
            #region ������
            //��
            this.ToTable("JT_SAFETYCHECK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
