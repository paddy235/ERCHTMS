using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ��Σ�����ؼ��
    /// </summary>
    public class HazarddetectionMap : EntityTypeConfiguration<HazarddetectionEntity>
    {
        public HazarddetectionMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HAZARDDETECTION");
            //����
            this.HasKey(t => t.HId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
