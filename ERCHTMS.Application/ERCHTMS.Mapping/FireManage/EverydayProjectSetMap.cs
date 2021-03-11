using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HighRiskWork
{
    /// <summary>
    /// 描 述：日常巡查项目配置
    /// </summary>
    public class EverydayProjectSetMap : EntityTypeConfiguration<EverydayProjectSetEntity>
    {
        public EverydayProjectSetMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_EVERYDAYPROJECTSET");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
