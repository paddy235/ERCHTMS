using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// �� ������ȫ�������
    /// </summary>
    public class SafeStandardsMap : EntityTypeConfiguration<SafeStandardsEntity>
    {
        public SafeStandardsMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFESTANDARDS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
