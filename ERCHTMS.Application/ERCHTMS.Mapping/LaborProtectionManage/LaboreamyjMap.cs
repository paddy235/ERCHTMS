using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ�����Ԥ����
    /// </summary>
    public class LaboreamyjMap : EntityTypeConfiguration<LaboreamyjEntity>
    {
        public LaboreamyjMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LABOREAMYJ");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
