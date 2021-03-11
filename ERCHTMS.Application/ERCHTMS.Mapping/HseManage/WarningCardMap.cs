using ERCHTMS.Entity.HseManage;
using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护预警表
    /// </summary>
    public class WarningCardMap : EntityTypeConfiguration<WarningCardEntity>
    {
        public WarningCardMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_WARNINGCARD");
            //主键
            this.HasKey(t => t.CardId);
            #endregion

            #region 配置关系
            #endregion
        }
    }

    public class CheckContentMap : EntityTypeConfiguration<CheckContentEntity>
    {
        public CheckContentMap()
        {
            this.ToTable("HSE_CHECKCONTENT");
            //主键
            this.HasKey(t => t.CheckContentId);
        }
    }
}
