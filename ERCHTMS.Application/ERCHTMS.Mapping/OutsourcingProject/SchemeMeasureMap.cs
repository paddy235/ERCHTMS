using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ����������ʩ����
    /// </summary>
    public class SchemeMeasureMap : EntityTypeConfiguration<SchemeMeasureEntity>
    {
        public SchemeMeasureMap()
        {
            #region ������
            //��
            this.ToTable("EPG_SCHEMEMEASURE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
