using ERCHTMS.Entity.LllegalStandard;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalStandard
{
    /// <summary>
    /// �� ����Υ�±�׼��
    /// </summary>
    public class LllegalstandardMap : EntityTypeConfiguration<LllegalstandardEntity>
    {
        public LllegalstandardMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LLLEGALSTANDARD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
