using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置实例表
    /// </summary>
    public class WfInstanceMap : EntityTypeConfiguration<WfInstanceEntity>
    {
        public WfInstanceMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WFINSTANCE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}