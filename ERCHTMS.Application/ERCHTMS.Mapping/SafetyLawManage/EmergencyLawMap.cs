using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// �� ����Ӧ��Ԥ������
    /// </summary>
    public class EmergencyLawMap : EntityTypeConfiguration<EmergencyLawEntity>
    {
        public EmergencyLawMap()
        {
            #region ������
            //��
            this.ToTable("BIS_EMERGENCYLAW");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
