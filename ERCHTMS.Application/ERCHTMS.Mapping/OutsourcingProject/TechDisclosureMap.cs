using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public class TechDisclosureMap : EntityTypeConfiguration<TechDisclosureEntity>
    {
        public TechDisclosureMap()
        {
            #region ������
            //��
            this.ToTable("EPG_TECHDISCLOSURE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
