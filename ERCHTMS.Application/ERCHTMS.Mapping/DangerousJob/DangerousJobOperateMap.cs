using ERCHTMS.Entity.DangerousJob;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.DangerousJob
{
    /// <summary>
    /// 描 述：高危作业操作表
    /// </summary>
    public class DangerousJobOperateMap : EntityTypeConfiguration<DangerousJobOperateEntity>
    {
        public DangerousJobOperateMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_DANGEROUSJOBOPERATE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
