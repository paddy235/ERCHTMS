using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// �� �����嶨��ȫ���
    /// </summary>
    public class FivesafetycheckMap : EntityTypeConfiguration<FivesafetycheckEntity>
    {
        public FivesafetycheckMap()
        {
            #region ������
            //��
            this.ToTable("BIS_FIVESAFETYCHECK");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
