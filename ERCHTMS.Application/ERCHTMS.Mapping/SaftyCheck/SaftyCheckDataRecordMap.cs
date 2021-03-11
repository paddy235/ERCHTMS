using ERCHTMS.Entity.SaftyCheck;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ����¼
    /// </summary>
    public class SaftyCheckDataRecordMap : EntityTypeConfiguration<SaftyCheckDataRecordEntity>
    {
        public SaftyCheckDataRecordMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFTYCHECKDATARECORD");
            //����
            this.HasKey(t => t.ID);
            this.Ignore(t => t.WzCount);
            this.Ignore(t => t.WtCount);
            this.Ignore(t => t.Count1);
            this.Ignore(t => t.WzCount1);
            this.Ignore(t => t.WtCount1);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
