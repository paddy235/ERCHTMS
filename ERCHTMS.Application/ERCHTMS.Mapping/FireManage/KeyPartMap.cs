using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� �����ص����λ
    /// </summary>
    public class KeyPartMap : EntityTypeConfiguration<KeyPartEntity>
    {
        public KeyPartMap()
        {
            #region ������
            //��
            this.ToTable("HRS_KEYPART");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
