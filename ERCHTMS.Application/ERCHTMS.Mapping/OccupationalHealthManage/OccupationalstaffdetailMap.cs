using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ���������
    /// </summary>
    public class OccupationalstaffdetailMap : EntityTypeConfiguration<OccupationalstaffdetailEntity>
    {
        public OccupationalstaffdetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OCCUPATIONALSTAFFDETAIL");
            //����
            this.HasKey(t => t.OccDetailId);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
