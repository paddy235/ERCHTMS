using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּܴ��衢���ա��������2.���ּܴ��衢���ա��������
    /// </summary>
    public class ScaffoldMap : EntityTypeConfiguration<ScaffoldEntity>
    {
        public ScaffoldMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SCAFFOLD");
            //����
            this.HasKey(t => t.Id);
            this.Ignore(t => t.SpecialtyTypeName);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
