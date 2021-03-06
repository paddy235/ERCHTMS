using ERCHTMS.Entity.EmergencyPlatform;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EmergencyPlatform
{
    /// <summary>
    /// 描 述：应急预案
    /// </summary>
    public class ReserverplanMap : EntityTypeConfiguration<ReserverplanEntity>
    {
        public ReserverplanMap()
        {
            #region 表、主键
            //表
            this.ToTable("MAE_RESERVERPLAN");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
