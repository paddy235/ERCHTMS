using ERCHTMS.Entity.RoutineSafetyWork;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：文件管理
    /// </summary>
    public class FileManageMap : EntityTypeConfiguration<FileManageEntity>
    {
        public FileManageMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_FILEMANAGE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
