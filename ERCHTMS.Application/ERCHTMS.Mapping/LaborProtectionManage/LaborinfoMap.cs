using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�������Ʒ��
    /// </summary>
    public class LaborinfoMap : EntityTypeConfiguration<LaborinfoEntity>
    {
        public LaborinfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LABORINFO");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
