using ERCHTMS.Entity.SaftProductTargetManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftProductTargetManage
{
    /// <summary>
    /// �� ������ȫ����������
    /// </summary>
    public class SafeProductDutyBookMap : EntityTypeConfiguration<SafeProductDutyBookEntity>
    {
        public SafeProductDutyBookMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFEPRODUCTDUTYBOOK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
