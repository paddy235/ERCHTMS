using ERCHTMS.Entity.StandardSystem;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.StandardSystem
{
    /// <summary>
    /// 描 述：标准化组织机构描述表
    /// </summary>
    public class StandardoriganzedescMap : EntityTypeConfiguration<StandardoriganzedescEntity>
    {
        public StandardoriganzedescMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_STANDARDORIGANZEDESC");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
