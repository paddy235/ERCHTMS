using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// �� ������������ɸѡ
    /// </summary>
    public class NoSuitableDetailMap : EntityTypeConfiguration<NoSuitableDetailEntity>
    {
        public NoSuitableDetailMap()
        {
            #region ������
            //��
            this.ToTable("BIS_NOSUITABLEDETAIL");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
