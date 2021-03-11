using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：摄像头管理
    /// </summary>
    public class CameramanageMap : EntityTypeConfiguration<CameramanageEntity>
    {
        public CameramanageMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CAMERAMANAGE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
