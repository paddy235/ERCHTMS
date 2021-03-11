using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� ������׼��ϵ
    /// </summary>
    public class StandardsystemMap : EntityTypeConfiguration<StandardsystemEntity>
    {
        public StandardsystemMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STANDARDSYSTEM");
            //����
            this.HasKey(t => t.ID);
            this.Ignore(t=>t.CATEGORYNAME);
            this.Ignore(t => t.CREATEUSERDEPTNAME);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
