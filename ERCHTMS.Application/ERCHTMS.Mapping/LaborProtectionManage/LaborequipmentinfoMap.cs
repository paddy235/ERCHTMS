using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�������Ϣ������
    /// </summary>
    public class LaborequipmentinfoMap : EntityTypeConfiguration<LaborequipmentinfoEntity>
    {
        public LaborequipmentinfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LABOREQUIPMENTINFO");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
