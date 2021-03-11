using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// 描 述：自评扣分明细
    /// </summary>
    public class KScoreDetailMap : EntityTypeConfiguration<KScoreDetailEntity>
    {
        public KScoreDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_KSCOREDETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
