using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// 描 述：积分设置
    /// </summary>
    public class ScoreSetMap : EntityTypeConfiguration<ScoreSetEntity>
    {
        public ScoreSetMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SCORESET");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
