using ERCHTMS.Entity.RiskDatabase;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDatabase
{
    /// <summary>
    /// �� ������ҵ����豸��ʩ�嵥
    /// </summary>
    public class BaseListingMap : EntityTypeConfiguration<BaseListingEntity>
    {
        public BaseListingMap()
        {
            #region ������
            //��
            this.ToTable("BIS_BASELISTING");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
