using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：康巴什摄像头管理
    /// </summary>
    public class KbscameramanageMap : EntityTypeConfiguration<KbscameramanageEntity>
    {
        public KbscameramanageMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_KBSCAMERAMANAGE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
