using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �������������
    /// </summary>
    public class StartapplyforMap : EntityTypeConfiguration<StartapplyforEntity>
    {
        public StartapplyforMap()
        {
            #region ������
            //��
            this.ToTable("EPG_STARTAPPLYFOR");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
