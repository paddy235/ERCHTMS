using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：检测、维护记录
    /// </summary>
    public class DetectionRecordMap : EntityTypeConfiguration<DetectionRecordEntity>
    {
        public DetectionRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_DETECTIONRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
