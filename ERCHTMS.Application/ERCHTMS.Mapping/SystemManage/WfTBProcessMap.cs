using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：流程实例
    /// </summary>
    public class WfTBProcessMap : EntityTypeConfiguration<WfTBProcessEntity>
    {
        public WfTBProcessMap()
        {
            #region 表、主键
            //表
            this.ToTable("SYS_WFTBPROCESS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
