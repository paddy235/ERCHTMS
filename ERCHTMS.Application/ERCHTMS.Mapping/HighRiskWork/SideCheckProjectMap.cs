using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �����ල��������Ŀ
    /// </summary>
    public class SideCheckProjectMap : EntityTypeConfiguration<SideCheckProjectEntity>
    {
        public SideCheckProjectMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SIDECHECKPROJECT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
