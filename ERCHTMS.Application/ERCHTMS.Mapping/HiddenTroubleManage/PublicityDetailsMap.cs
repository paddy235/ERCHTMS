using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.HiddenTroubleManage
{
    /// <summary>
    /// 描 述：流程配置条件表
    /// </summary>
    public class PublicityDetailsMap : EntityTypeConfiguration<PublicityDetailsEntity>
    {
        public PublicityDetailsMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_PUBLICITYDETAILS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}