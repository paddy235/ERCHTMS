using ERCHTMS.Entity.EvaluateManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EvaluateManage
{
    /// <summary>
    /// �� �����Ϲ���������ϸ
    /// </summary>
    public class EvaluateDetailsMap : EntityTypeConfiguration<EvaluateDetailsEntity>
    {
        public EvaluateDetailsMap()
        {
            #region ������
            //��
            this.ToTable("HRS_EVALUATEDETAILS");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
