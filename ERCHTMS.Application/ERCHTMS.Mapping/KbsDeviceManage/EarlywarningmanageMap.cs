using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：预警类型管理
    /// </summary>
    public class EarlywarningmanageMap : EntityTypeConfiguration<EarlywarningmanageEntity>
    {
        public EarlywarningmanageMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_EARLYWARNINGMANAGE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
