using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������
    /// </summary>
    public class HTEstimateMap : EntityTypeConfiguration<HTEstimateEntity>
    {
        public HTEstimateMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HTESTIMATE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
