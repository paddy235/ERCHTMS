using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// �� ��������Ԥ��
    /// </summary>
    public class WeatherMap : EntityTypeConfiguration<WeatherEntity>
    {
        public WeatherMap()
        {
            #region ������
            //��
            this.ToTable("BIS_WEATHER");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
