using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：动火记录（重点防火部位子表）
    /// </summary>
    public class MoveFireRecordMap : EntityTypeConfiguration<MoveFireRecordEntity>
    {
        public MoveFireRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_MOVEFIRERECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
