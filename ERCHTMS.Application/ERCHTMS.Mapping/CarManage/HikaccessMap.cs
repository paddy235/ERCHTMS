using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ���������Ž����豸����
    /// </summary>
    public class HikaccessMap : EntityTypeConfiguration<HikaccessEntity>
    {
        public HikaccessMap()
        {
            #region ������
            //��
            this.ToTable("BIS_HIKACCESS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
