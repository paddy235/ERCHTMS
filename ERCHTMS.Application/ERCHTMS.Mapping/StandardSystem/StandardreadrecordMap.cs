using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// �� ������׼��ϵ������ϸ��
    /// </summary>
    public class StandardreadrecordMap : EntityTypeConfiguration<StandardreadrecordEntity>
    {
        public StandardreadrecordMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STANDARDREADRECORD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
