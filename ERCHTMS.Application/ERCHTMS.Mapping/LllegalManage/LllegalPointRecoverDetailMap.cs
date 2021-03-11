using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：违章积分恢复人员明细
    /// </summary>
    public class LllegalPointRecoverDetailMap : EntityTypeConfiguration<LllegalPointRecoverDetailEntity>
    {
        public LllegalPointRecoverDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALPOINTRECOVERDETAIL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}