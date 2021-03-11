using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：出入库记录
    /// </summary>
    public class InoroutrecordMap : EntityTypeConfiguration<InoroutrecordEntity>
    {
        public InoroutrecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_INOROUTRECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
