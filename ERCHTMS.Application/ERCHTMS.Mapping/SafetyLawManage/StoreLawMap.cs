using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：收藏法律法规
    /// </summary>
    public class StoreLawMap : EntityTypeConfiguration<StoreLawEntity>
    {
        public StoreLawMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_STORELAW");
            //主键
            this.HasKey(t => t.storeId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
