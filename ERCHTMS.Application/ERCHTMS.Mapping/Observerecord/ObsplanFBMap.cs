using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// �� �����۲�ƻ�
    /// </summary>
    public class ObsplanFBMap : EntityTypeConfiguration<ObsplanFBEntity>
    {
        public ObsplanFBMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OBSPLAN_FB");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
