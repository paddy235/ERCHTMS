using ERCHTMS.Entity.BaseManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.BaseManage
{
    /// <summary>
    /// 描 述：数据设置
    /// </summary>
    public class DataSetMap : EntityTypeConfiguration<DataSetEntity>
    {
        public DataSetMap()
        {
            #region 表、主键
            //表
            this.ToTable("BASE_DATASET");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
