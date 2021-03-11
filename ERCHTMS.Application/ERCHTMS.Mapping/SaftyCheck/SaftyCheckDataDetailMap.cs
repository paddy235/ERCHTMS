using ERCHTMS.Entity.SaftyCheck;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public class SaftyCheckDataDetailMap : EntityTypeConfiguration<SaftyCheckDataDetailEntity>
    {
        public SaftyCheckDataDetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFTYCHECKDATADETAILED");
            //����
            this.HasKey(t => t.ID);
            this.Ignore(t => t.Content);
            //this.Ignore(t => t.IsSure);
            //this.Ignore(t => t.Remark);
            this.Ignore(t => t.WzCount);
            this.Ignore(t => t.WtCount);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
