using ERCHTMS.Entity.Observerecord;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.Observerecord
{
    /// <summary>
    /// �� �����۲��¼��
    /// </summary>
    public class ObserverecordMap : EntityTypeConfiguration<ObserverecordEntity>
    {
        public ObserverecordMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OBSERVERECORD");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
