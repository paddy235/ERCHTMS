using ERCHTMS.Entity.MatterManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.MatterManage
{
    /// <summary>
    /// 描 述：计量管理
    /// </summary>
    public class CalculateMap : EntityTypeConfiguration<CalculateEntity>
    {
        public CalculateMap()
        {
            #region 表、主键
            //表
            this.ToTable("WL_CALCULATE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
