using ERCHTMS.Entity.DangerousJob;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJob
{
    /// <summary>
    /// �� ����ä����ҵä����
    /// </summary>
    public class BlindPlateWallSpecMap : EntityTypeConfiguration<BlindPlateWallSpecEntity>
    {
        public BlindPlateWallSpecMap()
        {
            #region ������
            //��
            this.ToTable("BIS_BLINDPLATEWALLSPEC");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
