using ERCHTMS.Entity.AccidentEvent;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AccidentEvent
{
    /// <summary>
    /// �� �����¹��¼����鴦��
    /// </summary>
    public class Bulletin_dealMap : EntityTypeConfiguration<Bulletin_dealEntity>
    {
        public Bulletin_dealMap()
        {
            #region ������
            //��
            this.ToTable("AEM_BULLETIN_DEAL");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
