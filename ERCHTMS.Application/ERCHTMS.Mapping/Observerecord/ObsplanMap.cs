using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// �� �����۲�ƻ�
    /// </summary>
    public class ObsplanMap : EntityTypeConfiguration<ObsplanEntity>
    {
        public ObsplanMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OBSPLAN");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
