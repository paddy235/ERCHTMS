using ERCHTMS.Entity.AccidentEvent;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AccidentEvent
{
    /// <summary>
    /// �� ����δ���¼���������鴦��
    /// </summary>
    public class Wssjbg_dealMap : EntityTypeConfiguration<Wssjbg_dealEntity>
    {
        public Wssjbg_dealMap()
        {
            #region ������
            //��
            this.ToTable("AEM_WSSJBG_DEAL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
