using ERCHTMS.Entity.SafePunish;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafePunish
{
    /// <summary>
    /// �� ������ȫ������ϸ
    /// </summary>
    public class SafepunishdetailMap : EntityTypeConfiguration<SafepunishdetailEntity>
    {
        public SafepunishdetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFEPUNISHDETAIL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
