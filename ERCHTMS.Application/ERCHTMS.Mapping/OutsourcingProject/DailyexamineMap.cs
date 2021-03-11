using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：日常考核表
    /// </summary>
    public class DailyexamineMap : EntityTypeConfiguration<DailyexamineEntity>
    {
        public DailyexamineMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_DAILYEXAMINE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
