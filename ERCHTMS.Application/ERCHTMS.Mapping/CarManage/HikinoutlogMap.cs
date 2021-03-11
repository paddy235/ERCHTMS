using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：设备记录人员进出日志
    /// </summary>
    public class HikinoutlogMap : EntityTypeConfiguration<HikinoutlogEntity>
    {
        public HikinoutlogMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HIKINOUTLOG");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
