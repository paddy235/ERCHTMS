using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� �����೵�ϳ���¼��
    /// </summary>
    public class CarrideMap : EntityTypeConfiguration<CarrideEntity>
    {
        public CarrideMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARRIDE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
