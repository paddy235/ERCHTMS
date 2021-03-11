using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：模板页面管理表
    /// </summary>
    public class PageTemplateMap : EntityTypeConfiguration<PageTemplateEntity>
    {
        public PageTemplateMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_PAGETEMPLATE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}