using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ����Ա��
    /// </summary>
    public class OccupatioalstaffMap : EntityTypeConfiguration<OccupatioalstaffEntity>
    {
        public OccupatioalstaffMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OCCUPATIOALSTAFF");
            //����
            this.HasKey(t => t.OccId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
