using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包单位基础信息表
    /// </summary>
    public class OutsourcingprojectMap : EntityTypeConfiguration<OutsourcingprojectEntity>
    {
        public OutsourcingprojectMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_OUTSOURCINGPROJECT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
