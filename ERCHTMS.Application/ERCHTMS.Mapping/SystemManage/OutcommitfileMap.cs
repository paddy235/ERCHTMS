using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：外包电厂提交资料说明表
    /// </summary>
    public class OutcommitfileMap : EntityTypeConfiguration<OutcommitfileEntity>
    {
        public OutcommitfileMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_OUTCOMMITFILE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
