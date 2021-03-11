using ERCHTMS.Entity.EvaluateManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.EvaluateManage
{
    /// <summary>
    /// 描 述：合规性评价
    /// </summary>
    public class EvaluateMap : EntityTypeConfiguration<EvaluateEntity>
    {
        public EvaluateMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_EVALUATE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
