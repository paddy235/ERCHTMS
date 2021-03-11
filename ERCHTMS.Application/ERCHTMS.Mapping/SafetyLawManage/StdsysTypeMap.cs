using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：标准制度分类表
    /// </summary>
    public class StdsysTypeMap : EntityTypeConfiguration<StdsysTypeEntity>
    {
        public StdsysTypeMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_STDSYSTYPE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
