using ERCHTMS.Entity.ComprehensiveManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.ComprehensiveManage
{
    /// <summary>
    /// 描 述：信息报送明细表
    /// </summary>
    public class InfoSubmitDetailsEntityMap : EntityTypeConfiguration<InfoSubmitDetailsEntity>
    {
        public InfoSubmitDetailsEntityMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_INFOSUBMITDETAILS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
