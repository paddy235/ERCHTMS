using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� ����Ա�����
    /// </summary>
    public class StaffMienMap : EntityTypeConfiguration<StaffMienEntity>
    {
        public StaffMienMap()
        {
            #region ������
            //��
            this.ToTable("BIS_STAFFMIEN");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
