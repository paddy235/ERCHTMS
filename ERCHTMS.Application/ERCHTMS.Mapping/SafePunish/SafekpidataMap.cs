using ERCHTMS.Entity.SafePunish;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafePunish
{
    /// <summary>
    /// �� ������ȫ�ͷ�
    /// </summary>
    public class SafekpidataMap : EntityTypeConfiguration<SafekpidataEntity>
    {
        public SafekpidataMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFEKPIDATA");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
