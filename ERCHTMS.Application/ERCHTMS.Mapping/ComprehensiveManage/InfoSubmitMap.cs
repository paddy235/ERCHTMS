using ERCHTMS.Entity.ComprehensiveManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    /// <summary>
    /// 描 述：信息报送表
    /// </summary>
    public class InfoSubmitEntityMap : EntityTypeConfiguration<InfoSubmitEntity>
    {
        public InfoSubmitEntityMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_INFOSUBMIT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
