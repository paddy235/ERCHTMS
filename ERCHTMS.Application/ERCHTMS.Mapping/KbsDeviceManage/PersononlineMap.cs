using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// �� ������ԱÿСʱ�ڳ���¼
    /// </summary>
    public class PersononlineMap : EntityTypeConfiguration<PersononlineEntity>
    {
        public PersononlineMap()
        {
            #region ������
            //��
            this.ToTable("BIS_PERSONONLINE");
            //����
            this.HasKey(t => t.ID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
