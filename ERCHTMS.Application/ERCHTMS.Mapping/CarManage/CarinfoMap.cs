using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class CarinfoMap : EntityTypeConfiguration<CarinfoEntity>
    {
        public CarinfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARINFO");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
