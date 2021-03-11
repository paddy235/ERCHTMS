using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// �� ��������������ҵ���
    /// </summary>
    public class ThreePeopleInfoMap : EntityTypeConfiguration<ThreePeopleInfoEntity>
    {
        public ThreePeopleInfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_THREEPEOPLEINFO");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
