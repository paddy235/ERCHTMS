using ERCHTMS.Entity.EvaluateManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EvaluateManage
{
    /// <summary>
    /// �� �����Ϲ�������
    /// </summary>
    public class EvaluateMap : EntityTypeConfiguration<EvaluateEntity>
    {
        public EvaluateMap()
        {
            #region ������
            //��
            this.ToTable("HRS_EVALUATE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
