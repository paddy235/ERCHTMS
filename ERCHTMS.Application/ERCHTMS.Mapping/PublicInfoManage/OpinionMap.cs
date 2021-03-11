using ERCHTMS.Entity.PublicInfoManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PublicInfoManage
{
    /// <summary>
    /// 描 述：意见反馈
    /// </summary>
    public class OpinionMap : EntityTypeConfiguration<OpinionEntity>
    {
        public OpinionMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_OPINION");
            //主键
            this.HasKey(t => t.OpinionId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
