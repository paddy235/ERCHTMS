using ERCHTMS.Entity.LllegalManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.LllegalManage
{
    /// <summary>
    /// 描 述：违章积分恢复单
    /// </summary>
    public class LllegalPointRecoverMap : EntityTypeConfiguration<LllegalPointRecoverEntity>
    {
        public LllegalPointRecoverMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_LLLEGALPOINTRECOVER");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}