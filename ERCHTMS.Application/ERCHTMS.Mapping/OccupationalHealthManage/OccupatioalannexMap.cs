using ERCHTMS.Entity.OccupationalHealthManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OccupationalHealthManage
{
    /// <summary>
    /// �� ����ְҵ���˸�����
    /// </summary>
    public class OccupatioalannexMap : EntityTypeConfiguration<OccupatioalannexEntity>
    {
        public OccupatioalannexMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OCCUPATIOALANNEX");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
