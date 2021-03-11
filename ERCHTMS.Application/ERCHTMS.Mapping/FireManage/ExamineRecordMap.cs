using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：检查记录
    /// </summary>
    public class ExamineRecordMap : EntityTypeConfiguration<ExamineRecordEntity>
    {
        public ExamineRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_EXAMINERECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
