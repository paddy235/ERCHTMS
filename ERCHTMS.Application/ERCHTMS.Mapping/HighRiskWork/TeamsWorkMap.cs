using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �����������������ҵ��Ϣ
    /// </summary>
    public class TeamsWorkMap : EntityTypeConfiguration<TeamsWorkEntity>
    {
        public TeamsWorkMap()
        {
            #region ������
            //��
            this.ToTable("BIS_TEAMSWORK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
