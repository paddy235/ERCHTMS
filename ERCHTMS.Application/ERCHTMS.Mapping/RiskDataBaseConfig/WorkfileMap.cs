using ERCHTMS.Entity.RiskDataBaseConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RiskDataBaseConfig
{
    /// <summary>
    /// �� �������������嵥˵����
    /// </summary>
    public class WorkfileMap : EntityTypeConfiguration<WorkfileEntity>
    {
        public WorkfileMap()
        {
            #region ������
            //��
            this.ToTable("BIS_WORKFILE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
