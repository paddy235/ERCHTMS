using ERCHTMS.Entity.SaftyCheck;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SaftyCheck
{
    /// <summary>
    /// �� ������ȫ����
    /// </summary>
    public class SaftyCheckDataMap : EntityTypeConfiguration<SaftyCheckDataEntity>
    {
        public SaftyCheckDataMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SAFTYCHECKDATA");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
