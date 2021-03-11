using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：入厂许可申请历史记录
    /// </summary>
    public class IntromissionHistoryMap : EntityTypeConfiguration<IntromissionHistoryEntity>  
    {
        public IntromissionHistoryMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_INTROMISSIONHISTORY");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}