using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：整改计划明细表
    /// </summary>
    public class ChangePlanDetailMap : EntityTypeConfiguration<ChangePlanDetailEntity>
    {
        public ChangePlanDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CHANGEPLANDETAIL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}