using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// �� ������Ա����
    /// </summary>
    public class UserScoreMap : EntityTypeConfiguration<UserScoreEntity>
    {
        public UserScoreMap()
        {
            #region ������
            //��
            this.ToTable("BIS_USERSCORE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
