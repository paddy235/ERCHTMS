using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：标准体系查阅详细表
    /// </summary>
    public class StandardreadrecordMap : EntityTypeConfiguration<StandardreadrecordEntity>
    {
        public StandardreadrecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_STANDARDREADRECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
