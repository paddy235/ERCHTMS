using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� ������׼�����
    /// </summary>
    public class StandardApplyEntityMap : EntityTypeConfiguration<StandardApplyEntity>
    {
        public StandardApplyEntityMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STANDARDAPPLY");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
