using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ��������GPS������
    /// </summary>
    public class CargpsMap : EntityTypeConfiguration<CargpsEntity>
    {
        public CargpsMap()
        {
            #region ������
            //��
            this.ToTable("BIS_CARGPS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
