using ERCHTMS.Entity.EvaluateManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EvaluateManage
{
    /// <summary>
    /// �� �����Ϲ������ۼƻ�
    /// </summary>
    public class EvaluatePlanMap : EntityTypeConfiguration<EvaluatePlanEntity>
    {
        public EvaluatePlanMap()
        {
            #region ������
            //��
            this.ToTable("HRS_EVALUATEPLAN");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
