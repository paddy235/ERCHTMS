using ERCHTMS.Entity.QuestionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.QuestionManage
{
    /// <summary>
    /// 描 述：发现问题处理记录表
    /// </summary>
    public class FindQuestionHandleMap : EntityTypeConfiguration<FindQuestionHandleEntity>
    {
        public FindQuestionHandleMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_FINDQUESTIONHANDLE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}