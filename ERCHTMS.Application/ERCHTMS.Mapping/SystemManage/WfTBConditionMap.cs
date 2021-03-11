using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：流程跳转
    /// </summary>
    public class WfTBConditionMap : EntityTypeConfiguration<WfTBConditionEntity>
    {
        public WfTBConditionMap()
        {
            #region 表、主键
            //表
            this.ToTable("SYS_WFTBCONDITION");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
