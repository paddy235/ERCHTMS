using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// �� ������ԱGPS������
    /// </summary>
    public class PersongpsMap : EntityTypeConfiguration<PersongpsEntity>
    {
        public PersongpsMap()
        {
            #region ������
            //��
            this.ToTable("BIS_PERSONGPS");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
