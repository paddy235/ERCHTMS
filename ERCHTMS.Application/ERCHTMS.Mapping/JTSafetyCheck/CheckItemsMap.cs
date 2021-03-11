using ERCHTMS.Entity.JTSafetyCheck;
using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.JTSafetyCheck
{
    /// <summary>
    /// 描 述：区域定位表
    /// </summary>
    public class CheckItemsMap : EntityTypeConfiguration<CheckItemsEntity>
    {
        public CheckItemsMap()
        {
            #region 表、主键
            //表
            this.ToTable("JT_CHECKITEMS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
