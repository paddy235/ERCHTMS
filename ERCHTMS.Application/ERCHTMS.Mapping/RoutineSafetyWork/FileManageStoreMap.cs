using ERCHTMS.Entity.RoutineSafetyWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.RoutineSafetyWork
{
    public class FileManageStoreMap : EntityTypeConfiguration<FileManageStoreEntity>
    {
        public FileManageStoreMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_FILEMANAGESTORE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
