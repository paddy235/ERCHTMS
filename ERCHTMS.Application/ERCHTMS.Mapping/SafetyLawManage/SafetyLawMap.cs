using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// �� ������ȫ�������ɷ���
    /// </summary>
    public class SafetyLawMap : EntityTypeConfiguration<SafetyLawEntity>
    {
        public SafetyLawMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFETYLAW");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
