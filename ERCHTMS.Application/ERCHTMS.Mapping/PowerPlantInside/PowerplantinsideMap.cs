using ERCHTMS.Entity.PowerPlantInside;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PowerPlantInside
{
    /// <summary>
    /// �� ������λ�ڲ��챨
    /// </summary>
    public class PowerplantinsideMap : EntityTypeConfiguration<PowerplantinsideEntity>
    {
        public PowerplantinsideMap()
        {
            #region ������
            //��
            this.ToTable("BIS_POWERPLANTINSIDE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
