using ERCHTMS.Entity.AccidentEvent;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AccidentEvent
{
    /// <summary>
    /// �� �����¹��¼��챨
    /// </summary>
    public class BulletinMap : EntityTypeConfiguration<BulletinEntity>
    {
        public BulletinMap()
        {
            #region ������
            //��
            this.ToTable("AEM_BULLETIN");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
