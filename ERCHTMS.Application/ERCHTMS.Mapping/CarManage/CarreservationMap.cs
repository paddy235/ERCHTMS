using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� �����೵ԤԼ��¼
    /// </summary>
    public class CarreservationMap : EntityTypeConfiguration<CarreservationEntity>
    {
        public CarreservationMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARRESERVATION");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
