using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� ���������λ��������
    /// </summary>
    public class OutprojectblacklistMap : EntityTypeConfiguration<OutprojectblacklistEntity>
    {
        public OutprojectblacklistMap()
        {
            #region ������
            //��
            this.ToTable("EPG_OUTPROJECTBLACKLIST");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
