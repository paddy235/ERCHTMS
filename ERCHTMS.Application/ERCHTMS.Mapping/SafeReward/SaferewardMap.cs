using ERCHTMS.Entity.SafeReward;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafeReward
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SaferewardMap : EntityTypeConfiguration<SaferewardEntity>
    {
        public SaferewardMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFEREWARD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
