using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� ������׼������
    /// </summary>
    public class StandardCheckEntityMap : EntityTypeConfiguration<StandardCheckEntity>
    {
        public StandardCheckEntityMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STANDARDCHECK");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
