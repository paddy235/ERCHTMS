using ERCHTMS.Entity.DangerousJobConfig;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJobConfig
{
    /// <summary>
    /// 描 述：危险作业分级标准配置
    /// </summary>
    public class ClassStandardConfigMap : EntityTypeConfiguration<ClassStandardConfigEntity>
    {
        public ClassStandardConfigMap()
        {
            #region 表、主键
            //表
            this.ToTable("DJ_CLASSSTANDARDCONFIG");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
