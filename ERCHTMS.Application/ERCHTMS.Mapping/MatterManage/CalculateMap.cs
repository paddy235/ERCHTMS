using ERCHTMS.Entity.MatterManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.MatterManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class CalculateMap : EntityTypeConfiguration<CalculateEntity>
    {
        public CalculateMap()
        {
            #region ������
            //��
            this.ToTable("WL_CALCULATE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
