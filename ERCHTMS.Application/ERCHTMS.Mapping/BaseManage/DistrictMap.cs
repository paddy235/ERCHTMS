using ERCHTMS.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.BaseManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class DistrictMap : EntityTypeConfiguration<DistrictEntity>
    {
        public DistrictMap()
        {
            #region ������
            //��
            this.ToTable("BIS_DISTRICT");
            //����
            this.HasKey(t => t.DistrictID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
