using ERCHTMS.Entity.PowerPlantInside;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PowerPlantInside
{
    /// <summary>
    /// �� �����¹��¼�������Ϣ
    /// </summary>
    public class PowerplanthandledetailMap : EntityTypeConfiguration<PowerplanthandledetailEntity>
    {
        public PowerplanthandledetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_POWERPLANTHANDLEDETAIL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
