using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// �� �����۲�ƻ�
    /// </summary>
    public class ObsplanTZMap : EntityTypeConfiguration<ObsplanTZEntity>
    {
        public ObsplanTZMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OBSPLAN_TZ");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
