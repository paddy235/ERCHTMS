using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּܷ����ϴ������ϴ�����
    /// </summary>
    public class ScaffoldfileMap : EntityTypeConfiguration<ScaffoldfileEntity>
    {
        public ScaffoldfileMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SCAFFOLDFILE");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
