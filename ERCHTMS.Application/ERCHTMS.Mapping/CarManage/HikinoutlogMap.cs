using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� �����豸��¼��Ա������־
    /// </summary>
    public class HikinoutlogMap : EntityTypeConfiguration<HikinoutlogEntity>
    {
        public HikinoutlogMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIKINOUTLOG");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
