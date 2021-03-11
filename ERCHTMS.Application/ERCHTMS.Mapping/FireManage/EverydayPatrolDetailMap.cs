using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：日常巡查（内容）
    /// </summary>
    public class EverydayPatrolDetailMap : EntityTypeConfiguration<EverydayPatrolDetailEntity>
    {
        public EverydayPatrolDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_EVERYDAYPATROLDETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
