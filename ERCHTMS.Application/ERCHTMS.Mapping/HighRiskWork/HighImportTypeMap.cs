using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ������ʾ
    /// </summary>
    public class HighImportTypeMap : EntityTypeConfiguration<HighImportTypeEntity>
    {
        public HighImportTypeMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIGHIMPORTTYPE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
