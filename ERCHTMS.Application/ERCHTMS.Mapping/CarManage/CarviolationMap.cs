using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：违章信息类
    /// </summary>
    public class CarviolationMap : EntityTypeConfiguration<CarviolationEntity>
    {
        public CarviolationMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CARVIOLATION");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
