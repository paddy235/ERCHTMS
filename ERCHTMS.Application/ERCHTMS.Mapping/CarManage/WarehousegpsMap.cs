using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� �����ֿⷶΧ������
    /// </summary>
    public class WarehousegpsMap : EntityTypeConfiguration<WarehousegpsEntity>
    {
        public WarehousegpsMap()
        {
            #region ������
            //��
            this.ToTable("BIS_WAREHOUSEGPS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
