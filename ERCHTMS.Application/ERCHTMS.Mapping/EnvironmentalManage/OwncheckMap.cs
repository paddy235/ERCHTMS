using ERCHTMS.Entity.EnvironmentalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EnvironmentalManage
{
    /// <summary>
    /// �� �������м��
    /// </summary>
    public class OwncheckMap : EntityTypeConfiguration<OwncheckEntity>
    {
        public OwncheckMap()
        {
            #region ������
            //��
            this.ToTable("BIS_OWNCHECK");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
