using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ������
    /// </summary>
    public class TeamMap : EntityTypeConfiguration<TeamEntity>
    {
        public TeamMap()
        {
            #region ������
            //��
            this.ToTable("MAE_TEAM");
            //����
            this.HasKey(t => t.TEAMID);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
