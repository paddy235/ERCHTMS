using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：外包资料说明用户设置表
    /// </summary>
    public class OutcommitfilesettingMap : EntityTypeConfiguration<OutcommitfilesettingEntity>
    {
        public OutcommitfilesettingMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_OUTCOMMITFILESETTING");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
