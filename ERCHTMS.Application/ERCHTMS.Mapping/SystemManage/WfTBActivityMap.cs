using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：流程活动
    /// </summary>
    public class WfTBActivityMap : EntityTypeConfiguration<WfTBActivityEntity>
    {
        public WfTBActivityMap()
        {
            #region 表、主键
            //表
            this.ToTable("SYS_WFTBACTIVITY");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
