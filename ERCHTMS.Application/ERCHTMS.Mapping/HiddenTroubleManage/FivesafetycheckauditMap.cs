using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� �����嶨��ȫ��� �������ձ�
    /// </summary>
    public class FivesafetycheckauditMap : EntityTypeConfiguration<FivesafetycheckauditEntity>
    {
        public FivesafetycheckauditMap()
        {
            #region ������
            //��
            this.ToTable("BIS_FIVESAFETYCHECKAUDIT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
