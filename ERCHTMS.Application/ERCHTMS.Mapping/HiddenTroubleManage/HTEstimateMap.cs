using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患评估表
    /// </summary>
    public class HTEstimateMap : EntityTypeConfiguration<HTEstimateEntity>
    {
        public HTEstimateMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HTESTIMATE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
