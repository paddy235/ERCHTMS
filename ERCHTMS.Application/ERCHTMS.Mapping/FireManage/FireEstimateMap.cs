using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ������������ָ��
    /// </summary>
    public class FireEstimateMap : EntityTypeConfiguration<FireEstimateEntity>
    {
        public FireEstimateMap()
        {
            #region ������
            //��
            this.ToTable("HRS_FIREESTIMATE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
