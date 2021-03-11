using ERCHTMS.Entity.FileListManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FileListManage
{
    /// <summary>
    /// 描 述：体系建设文件清单
    /// </summary>
    public class FileListMap : EntityTypeConfiguration<FileListEntity>
    {
        public FileListMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_FILELIST");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
