using ERCHTMS.Entity.QuestionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.QuestionManage
{
    /// <summary>
    /// 描 述：问题整改信息
    /// </summary>
    public class QuestionReformMap : EntityTypeConfiguration<QuestionReformEntity>
    {
        public QuestionReformMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_QUESTIONREFORM");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}