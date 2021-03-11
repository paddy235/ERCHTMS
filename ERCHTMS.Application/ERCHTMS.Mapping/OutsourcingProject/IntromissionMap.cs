using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：入厂许可申请
    /// </summary>
    public class IntromissionMap : EntityTypeConfiguration<IntromissionEntity>
    {
        public IntromissionMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_INTROMISSION");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}