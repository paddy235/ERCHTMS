using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// �� ������׼�ƶȷ����
    /// </summary>
    public class StdsysTypeMap : EntityTypeConfiguration<StdsysTypeEntity>
    {
        public StdsysTypeMap()
        {
            #region ������
            //��
            this.ToTable("HRS_STDSYSTYPE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
