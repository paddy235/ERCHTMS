using ERCHTMS.Entity.PowerPlantInside;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼�����
    /// </summary>
    public class PowerplanthandleMap : EntityTypeConfiguration<PowerplanthandleEntity>
    {
        public PowerplanthandleMap()
        {
            #region ������
            //��
            this.ToTable("BIS_POWERPLANTHANDLE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
