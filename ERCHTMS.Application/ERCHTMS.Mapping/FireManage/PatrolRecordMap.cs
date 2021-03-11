using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：巡查记录（重点防火部位子表）
    /// </summary>
    public class PatrolRecordMap : EntityTypeConfiguration<PatrolRecordEntity>
    {
        public PatrolRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_PATROLRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
