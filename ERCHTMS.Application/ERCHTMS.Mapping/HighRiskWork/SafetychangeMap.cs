using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ������ȫ��ʩ�䶯�����
    /// </summary>
    public class SafetychangeMap : EntityTypeConfiguration<SafetychangeEntity>
    {
        public SafetychangeMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFETYCHANGE");
            //����
            this.HasKey(t => t.ID);
            this.Ignore(t => t.ACCSPECIALTYTYPENAME);
            this.Ignore(t => t.SPECIALTYTYPENAME);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
