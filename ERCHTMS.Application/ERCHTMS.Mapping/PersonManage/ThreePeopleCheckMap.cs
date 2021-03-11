using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// �� ��������������ҵ���
    /// </summary>
    public class ThreePeopleCheckMap : EntityTypeConfiguration<ThreePeopleCheckEntity>
    {
        public ThreePeopleCheckMap()
        {
            #region ������
            //��
            this.ToTable("BIS_THREEPEOPLECHECK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
