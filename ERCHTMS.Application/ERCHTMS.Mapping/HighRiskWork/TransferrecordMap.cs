using ERCHTMS.Entity.HighRiskWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：转交记录表
    /// </summary>
    public class TransferrecordMap : EntityTypeConfiguration<TransferrecordEntity>
    {
        public TransferrecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TRANSFERRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
