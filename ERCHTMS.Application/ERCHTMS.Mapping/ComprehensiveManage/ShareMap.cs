using ERCHTMS.Entity.ComprehensiveManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    /// <summary>
    /// �� �����������
    /// </summary>
    public class ShareMap : EntityTypeConfiguration<ShareEntity>
    {
        public ShareMap()
        {
            #region ������
            //��
            this.ToTable("HRS_SHARE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
