using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල��Ա(�߷�����ҵ)
    /// </summary>
    public class SidePersonMap : EntityTypeConfiguration<SidePersonEntity>
    {
        public SidePersonMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SIDEPERSON");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
