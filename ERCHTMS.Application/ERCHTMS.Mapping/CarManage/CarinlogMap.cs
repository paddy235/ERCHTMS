using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ��������������¼��
    /// </summary>
    public class CarinlogMap : EntityTypeConfiguration<CarinlogEntity>
    {
        public CarinlogMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARINLOG");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
