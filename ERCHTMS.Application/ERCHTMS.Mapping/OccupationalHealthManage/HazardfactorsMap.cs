using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// �� ����Σ�������嵥
    /// </summary>
    public class HazardfactorsMap : EntityTypeConfiguration<HazardfactorsEntity>
    {
        public HazardfactorsMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HAZARDFACTORS");
            //����
            this.HasKey(t => t.Hid);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
