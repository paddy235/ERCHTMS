using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ���������λ������Ϣ��
    /// </summary>
    public class OutsourcingprojectMap : EntityTypeConfiguration<OutsourcingprojectEntity>
    {
        public OutsourcingprojectMap()
        {
            #region ������
            //��
            this.ToTable("EPG_OUTSOURCINGPROJECT");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
