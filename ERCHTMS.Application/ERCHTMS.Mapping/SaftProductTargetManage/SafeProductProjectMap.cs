using ERCHTMS.Entity.SaftProductTargetManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftProductTargetManage
{
    /// <summary>
    /// �� ������ȫ����Ŀ����Ŀ
    /// </summary>
    public class SafeProductProjectMap : EntityTypeConfiguration<SafeProductProjectEntity>
    {
        public SafeProductProjectMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFEPRODUCTPROJECT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
