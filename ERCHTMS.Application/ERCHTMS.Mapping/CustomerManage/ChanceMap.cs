using ERCHTMS.Entity.CustomerManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CustomerManage
{
    /// <summary>
    /// �� �����̻���Ϣ
    /// </summary>
    public class ChanceMap : EntityTypeConfiguration<ChanceEntity>
    {
        public ChanceMap()
        {
            #region ������
            //��
            this.ToTable("CLIENT_CHANCE");
            //����
            this.HasKey(t => t.ChanceId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
