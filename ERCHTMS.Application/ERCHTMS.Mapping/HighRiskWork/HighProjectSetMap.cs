using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �����߷�������Ŀ����
    /// </summary>
    public class HighProjectSetMap : EntityTypeConfiguration<HighProjectSetEntity>
    {
        public HighProjectSetMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIGHPROJECTSET");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
