using ERCHTMS.Entity.LaborProtectionManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LaborProtectionManage
{
    /// <summary>
    /// 描 述：劳动防护发放表详情
    /// </summary>
    public class LaborissuedetailMap : EntityTypeConfiguration<LaborissuedetailEntity>
    {
        public LaborissuedetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LABORISSUEDETAIL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
