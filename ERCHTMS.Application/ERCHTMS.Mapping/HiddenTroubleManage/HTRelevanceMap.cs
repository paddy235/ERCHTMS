using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：隐患及其他应用关联表
    /// </summary>
    public class HTRelevanceMap : EntityTypeConfiguration<HTRelevanceEntity>
    {
        public HTRelevanceMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_HTRELEVANCE");
            //主键
            this.HasKey(t => t.ID);
            //忽略字段
            //this.Ignore(t=>t.CHANGEMEASURE);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
