using ERCHTMS.Entity.EnvironmentalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EnvironmentalManage
{
    /// <summary>
    /// �� ����ˮ�ʷ���
    /// </summary>
    public class WaterqualityMap : EntityTypeConfiguration<WaterqualityEntity>
    {
        public WaterqualityMap()
        {
            #region ������
            //��
            this.ToTable("BIS_WATERQUALITY");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
