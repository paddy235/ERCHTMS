using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ����Υ����Ϣ��
    /// </summary>
    public class CarviolationMap : EntityTypeConfiguration<CarviolationEntity>
    {
        public CarviolationMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARVIOLATION");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
