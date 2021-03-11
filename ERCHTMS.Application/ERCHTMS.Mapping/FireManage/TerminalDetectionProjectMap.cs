using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：消防设施-定期检测记录-定期检测项目
    /// </summary>
    public class TerminalDetectionProjectMap : EntityTypeConfiguration<TerminalDetectionProjectEntity>
    {
        public TerminalDetectionProjectMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_TERMINALDETECTIONPROJECT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
