using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ����Σ�յ�Ԥ�ش�ʩ
    /// </summary>
    public class MeasuresMap : EntityTypeConfiguration<OutMeasuresEntity>
    {
        public MeasuresMap()
        {
            #region ������
            //��
            this.ToTable("EPG_MEASURES");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
