using ERCHTMS.Entity.SafeReward;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafeReward
{
    /// <summary>
    /// �� ������ȫ������ϸ
    /// </summary>
    public class SaferewarddetailMap : EntityTypeConfiguration<SaferewarddetailEntity>
    {
        public SaferewarddetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFEREWARDDETAIL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
