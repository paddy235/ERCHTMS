using ERCHTMS.Entity.DangerousJob;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJob
{
    /// <summary>
    /// �� ����Σ����ҵ�嵥
    /// </summary>
    public class DangerjoblistMap : EntityTypeConfiguration<DangerjoblistEntity>
    {
        public DangerjoblistMap()
        {
            #region ������
            //��
            this.ToTable("BIS_DANGERJOBLIST");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
