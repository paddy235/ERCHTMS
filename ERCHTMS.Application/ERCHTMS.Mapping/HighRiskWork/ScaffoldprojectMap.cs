using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּ�������Ŀ
    /// </summary>
    public class ScaffoldprojectMap : EntityTypeConfiguration<ScaffoldprojectEntity>
    {
        public ScaffoldprojectMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SCAFFOLDPROJECT");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
