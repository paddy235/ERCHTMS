using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ��������·��������
    /// </summary>
    public class RouteconfigMap : EntityTypeConfiguration<RouteconfigEntity>
    {
        public RouteconfigMap()
        {
            #region ������
            //��
            this.ToTable("BIS_ROUTECONFIG");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
