using ERCHTMS.Entity.SafePunish;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafePunish
{
    /// <summary>
    /// �� ������ȫ�ͷ�
    /// </summary>
    public class SafepunishMap : EntityTypeConfiguration<SafepunishEntity>
    {
        public SafepunishMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFEPUNISH");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
