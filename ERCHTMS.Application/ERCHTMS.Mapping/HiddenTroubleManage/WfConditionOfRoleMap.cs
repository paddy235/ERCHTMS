using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置条件角色附加表
    /// </summary>
    public class WfConditionOfRoleMap : EntityTypeConfiguration<WfConditionOfRoleEntity>
    {
        public WfConditionOfRoleMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WFCONDITIONOFROLE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
