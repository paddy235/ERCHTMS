using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：消防设施-定期检测记录
    /// </summary>
    public class TerminalDetectionRecordMap : EntityTypeConfiguration<TerminalDetectionRecordEntity>
    {
        public TerminalDetectionRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_TERMINALDETECTIONRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
