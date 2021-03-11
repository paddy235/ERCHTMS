using ERCHTMS.Entity.AssessmentManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AssessmentManage
{
    /// <summary>
    /// 描 述：不适宜项筛选
    /// </summary>
    public class NoSuitableDetailMap : EntityTypeConfiguration<NoSuitableDetailEntity>
    {
        public NoSuitableDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_NOSUITABLEDETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
