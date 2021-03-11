using ERCHTMS.Entity.EvaluateManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EvaluateManage
{
    /// <summary>
    /// 描 述：文件识别
    /// </summary>
    public class FileSpotMap : EntityTypeConfiguration<FileSpotEntity>
    {
        public FileSpotMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_FILESPOT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
