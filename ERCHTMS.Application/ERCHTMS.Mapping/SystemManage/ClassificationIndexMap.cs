using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：分项指标项目表
    /// </summary>
    public class ClassificationIndexMap : EntityTypeConfiguration<ClassificationIndexEntity>
    {
        public ClassificationIndexMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_CLASSIFICATIONINDEX");
            //主键
            this.HasKey(t => t.Id); 
            #endregion

            #region 配置关系
            #endregion
        }
    }
}