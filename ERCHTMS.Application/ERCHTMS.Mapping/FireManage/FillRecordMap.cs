using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// �� ������װ/������¼
    /// </summary>
    public class FillRecordMap : EntityTypeConfiguration<FillRecordEntity>
    {
        public FillRecordMap()
        {
            #region ������
            //��
            this.ToTable("HRS_FILLRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
