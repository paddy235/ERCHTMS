using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� �����ݷó�����
    /// </summary>
    public class VisitcarMap : EntityTypeConfiguration<VisitcarEntity>
    {
        public VisitcarMap()
        {
            #region ������
            //��
            this.ToTable("BIS_VISITCAR");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
