using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ����������������Ϣ��
    /// </summary>
    public class AptitudeinvestigateinfoMap : EntityTypeConfiguration<AptitudeinvestigateinfoEntity>
    {
        public AptitudeinvestigateinfoMap()
        {
            #region ������
            //��
            this.ToTable("EPG_APTITUDEINVESTIGATEINFO");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
