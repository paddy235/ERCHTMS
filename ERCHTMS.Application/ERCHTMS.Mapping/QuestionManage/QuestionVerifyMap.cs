using ERCHTMS.Entity.QuestionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.QuestionManage
{
    /// <summary>
    /// 描 述：问题验证信息表
    /// </summary>
    public class QuestionVerifyMap : EntityTypeConfiguration<QuestionVerifyEntity>
    {
        public QuestionVerifyMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_QUESTIONVERIFY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}