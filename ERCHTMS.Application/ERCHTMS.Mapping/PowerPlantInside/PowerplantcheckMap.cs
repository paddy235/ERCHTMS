using ERCHTMS.Entity.PowerPlantInside;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼�����
    /// </summary>
    public class PowerplantcheckMap : EntityTypeConfiguration<PowerplantcheckEntity>
    {
        public PowerplantcheckMap()
        {
            #region ������
            //��
            this.ToTable("BIS_POWERPLANTCHECK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
