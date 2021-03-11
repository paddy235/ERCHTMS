using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：标准制度收藏文件
    /// </summary>
    public class StdsysFilesStoreEntityMap : EntityTypeConfiguration<StdsysFilesStoreEntity>
    {
        public StdsysFilesStoreEntityMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_STDSYSSTOREFILES");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
