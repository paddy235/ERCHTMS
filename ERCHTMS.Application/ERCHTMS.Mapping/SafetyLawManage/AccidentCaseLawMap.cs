using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// �� �����¹ʰ�����
    /// </summary>
    public class AccidentCaseLawMap : EntityTypeConfiguration<AccidentCaseLawEntity>
    {
        public AccidentCaseLawMap()
        {
            #region ������
            //��
            this.ToTable("BIS_ACCIDENTCASELAW");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
