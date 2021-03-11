using ERCHTMS.Entity.QuestionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.QuestionManage
{
    /// <summary>
    /// 描 述：发现问题基本信息表
    /// </summary>
    public class FindQuestionInfoMap : EntityTypeConfiguration<FindQuestionInfoEntity>
    {
        public FindQuestionInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_FINDQUESTIONINFO");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}