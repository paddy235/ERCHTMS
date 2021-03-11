using ERCHTMS.Entity.HseManage;
using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护预警表
    /// </summary>
    public class CheckRecordMap : EntityTypeConfiguration<CheckRecordEntity>
    {
        public CheckRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_CHECKRECORD");
            //主键
            this.HasKey(t => t.CheckRecordId);
            #endregion

            #region 配置关系
            #endregion
        }
    }

    public class CheckItemMap : EntityTypeConfiguration<CheckItemEntity>
    {
        public CheckItemMap()
        {
            this.ToTable("HSE_CHECKITEM");
            //主键
            this.HasKey(t => t.CheckItemId);
        }
    }
}
