using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������������״̬��
    /// </summary>
    public class StartappprocessstatusMap : EntityTypeConfiguration<StartappprocessstatusEntity>
    {
        public StartappprocessstatusMap()
        {
            #region ������
            //��
            this.ToTable("EPG_STARTAPPPROCESSSTATUS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
