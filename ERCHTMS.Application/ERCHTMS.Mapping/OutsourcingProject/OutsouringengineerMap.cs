using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：外包工程信息表
    /// </summary>
    public class OutsouringengineerMap : EntityTypeConfiguration<OutsouringengineerEntity>
    {
        public OutsouringengineerMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_OUTSOURINGENGINEER");
            //主键
            this.HasKey(t => t.ID);
            this.Ignore(t => t.ENGINEERAREANAME);
            this.Ignore(t => t.ENGINEERLEVELNAME);
            this.Ignore(t => t.ENGINEERTYPENAME);
            this.Ignore(t => t.OUTPROJECTCODE);
            this.Ignore(t => t.OUTPROJECTNAME);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
