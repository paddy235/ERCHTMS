using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：动火区域维护
    /// </summary>
    public class MoveFireAreaMap : EntityTypeConfiguration<MoveFireAreaEntity>
    {
        public MoveFireAreaMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_MOVEFIREAREA");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
