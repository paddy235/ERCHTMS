using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ��������������
    /// </summary>
    public class TeamsInfoMap : EntityTypeConfiguration<TeamsInfoEntity>
    {
        public TeamsInfoMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TEAMSINFO");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
