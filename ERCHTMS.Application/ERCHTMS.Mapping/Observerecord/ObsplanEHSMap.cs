using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// �� �����۲�ƻ�
    /// </summary>
    public class ObsplanEHSMap : EntityTypeConfiguration<ObsplanEHSEntity>
    {
        public ObsplanEHSMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OBSPLAN_COMMITEHS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
