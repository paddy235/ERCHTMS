using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：危险点数据库
    /// </summary>
    public class DangerdataMap : EntityTypeConfiguration<DangerdataEntity>
    {
        public DangerdataMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_DANGERDATA");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
