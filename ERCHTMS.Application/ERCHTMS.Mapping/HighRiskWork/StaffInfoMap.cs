using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �������������Ա
    /// </summary>
    public class StaffInfoMap : EntityTypeConfiguration<StaffInfoEntity>
    {
        public StaffInfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_STAFFINFO");
            //����
            this.HasKey(t => t.Id);
            this.Ignore(t => t.SpecialtyTypeName);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
