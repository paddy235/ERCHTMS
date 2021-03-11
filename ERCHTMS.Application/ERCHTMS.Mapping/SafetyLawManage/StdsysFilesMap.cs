using ERCHTMS.Entity.SafetyLawManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SafetyLawManage
{
    /// <summary>
    /// 描 述：标准制度文件
    /// </summary>
    public class StdsysFilesEntityMap : EntityTypeConfiguration<StdsysFilesEntity>
    {
        public StdsysFilesEntityMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_STDSYSFILES");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
