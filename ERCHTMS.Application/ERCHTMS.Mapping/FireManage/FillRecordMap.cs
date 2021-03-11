using ERCHTMS.Entity.FireManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FireManage
{
    /// <summary>
    /// 描 述：充装/更换记录
    /// </summary>
    public class FillRecordMap : EntityTypeConfiguration<FillRecordEntity>
    {
        public FillRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_FILLRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
