using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：标准申请表
    /// </summary>
    public class StandardApplyEntityMap : EntityTypeConfiguration<StandardApplyEntity>
    {
        public StandardApplyEntityMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_STANDARDAPPLY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
