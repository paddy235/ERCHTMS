using ERCHTMS.Entity.EnvironmentalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EnvironmentalManage
{
    /// <summary>
    /// �� ����ˮ�ʷ�����¼
    /// </summary>
    public class WaterrecordMap : EntityTypeConfiguration<WaterrecordEntity>
    {
        public WaterrecordMap()
        {
            #region ������
            //��
            this.ToTable("BIS_WATERRECORD");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
