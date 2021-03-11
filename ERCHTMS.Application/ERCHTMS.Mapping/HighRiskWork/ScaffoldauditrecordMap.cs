using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架审核记录表
    /// </summary>
    public class ScaffoldauditrecordMap : EntityTypeConfiguration<ScaffoldauditrecordEntity>
    {
        public ScaffoldauditrecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_SCAFFOLDAUDITRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
