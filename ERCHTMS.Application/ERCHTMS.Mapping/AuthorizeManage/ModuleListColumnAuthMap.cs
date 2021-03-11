using ERCHTMS.Entity.AuthorizeManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.AuthorizeManage
{
    /// <summary>
    /// 描 述：应用模块列表的列查看权限设置表
    /// </summary>
    public class ModuleListColumnAuthMap : EntityTypeConfiguration<ModuleListColumnAuthEntity>
    {
        public ModuleListColumnAuthMap()
        {
            #region 表、主键
            //表
            this.ToTable("BASE_MODULELISTCOLUMNAUTH");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}