using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class ScoreSetMap : EntityTypeConfiguration<ScoreSetEntity>
    {
        public ScoreSetMap()
        {
            #region ������
            //��
            this.ToTable("BIS_SCORESET");
            //����
            this.HasKey(t => t.Id);
            #endregion

            #region ���ù�ϵ
            #endregion
        }
    }
}
