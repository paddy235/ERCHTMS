using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ����ͣ���������
    /// </summary>
    public class StopreturnworkMap : EntityTypeConfiguration<StopreturnworkEntity>
    {
        public StopreturnworkMap()
        {
            #region ������
            //��
            this.ToTable("EPG_STOPRETURNWORK");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
