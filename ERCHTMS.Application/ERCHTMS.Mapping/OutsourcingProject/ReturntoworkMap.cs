using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：复工申请表
    /// </summary>
    public class ReturntoworkMap : EntityTypeConfiguration<ReturntoworkEntity>
    {
        public ReturntoworkMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_RETURNTOWORK");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
