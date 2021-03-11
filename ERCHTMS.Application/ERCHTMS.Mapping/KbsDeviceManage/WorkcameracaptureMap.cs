using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：闯入人员抓拍记录表
    /// </summary>
    public class WorkcameracaptureMap : EntityTypeConfiguration<WorkcameracaptureEntity>
    {
        public WorkcameracaptureMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WORKCAMERACAPTURE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
