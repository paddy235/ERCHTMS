using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�������Ʒ
    /// </summary>
    public class LaborprotectionMap : EntityTypeConfiguration<LaborprotectionEntity>
    {
        public LaborprotectionMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LABORPROTECTION");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
