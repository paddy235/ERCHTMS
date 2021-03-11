using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ϣ��
    /// </summary>
    public class OutsouringengineerMap : EntityTypeConfiguration<OutsouringengineerEntity>
    {
        public OutsouringengineerMap()
        {
            #region ������
            //��
            this.ToTable("EPG_OUTSOURINGENGINEER");
            //����
            this.HasKey(t => t.ID);
            this.Ignore(t => t.ENGINEERAREANAME);
            this.Ignore(t => t.ENGINEERLEVELNAME);
            this.Ignore(t => t.ENGINEERTYPENAME);
            this.Ignore(t => t.OUTPROJECTCODE);
            this.Ignore(t => t.OUTPROJECTNAME);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
