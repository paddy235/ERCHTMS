using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// �� �������ص�װ��ҵ������Ա��
    /// </summary>
    public class LifthoistpersonMap : EntityTypeConfiguration<LifthoistpersonEntity>
    {
        public LifthoistpersonMap()
        {
            #region ������
            //��
            this.ToTable("BIS_LIFTHOISTPERSON");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
