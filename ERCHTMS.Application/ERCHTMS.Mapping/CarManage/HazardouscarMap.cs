using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ����Σ�����س�����
    /// </summary>
    public class HazardouscarMap : EntityTypeConfiguration<HazardouscarEntity>
    {
        public HazardouscarMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HAZARDOUSCAR");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
