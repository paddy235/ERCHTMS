using ERCHTMS.Entity.PowerPlantInside;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼���������
    /// </summary>
    public class PowerplantreformMap : EntityTypeConfiguration<PowerplantreformEntity>
    {
        public PowerplantreformMap()
        {
            #region ������
            //��
            this.ToTable("BIS_POWERPLANTREFORM");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
