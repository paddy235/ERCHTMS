using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// �� ������ȫ�����ƶ�
    /// </summary>
    public class SafeInstitutionMap : EntityTypeConfiguration<SafeInstitutionEntity>
    {
        public SafeInstitutionMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFEINSTITUTION");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
