using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：日常巡查
    /// </summary>
    public class EverydaypPatrolMap : EntityTypeConfiguration<EverydayPatrolEntity>
    {
        public EverydaypPatrolMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_EVERYDAYPATROL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
