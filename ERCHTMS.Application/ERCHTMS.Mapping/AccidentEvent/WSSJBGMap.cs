using ERCHTMS.Entity.AccidentEvent;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AccidentEvent
{
    /// <summary>
    /// �� ����WSSJBG
    /// </summary>
    public class WSSJBGMap : EntityTypeConfiguration<WSSJBGEntity>
    {
        public WSSJBGMap()
        {
            #region ������
            //��
            this.ToTable("AEM_WSSJBG");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
