using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� ������׼����֯����������
    /// </summary>
    public class StandardoriganzedescMap : EntityTypeConfiguration<StandardoriganzedescEntity>
    {
        public StandardoriganzedescMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STANDARDORIGANZEDESC");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
