using ERCHTMS.Entity.NosaManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.NosaManage
{
    /// <summary>
    /// 描 述：工作成果
    /// </summary>
    public class NosaworkresultMap : EntityTypeConfiguration<NosaworkresultEntity>
    {
        public NosaworkresultMap()
        {
            #region 表、主键
            //表
            this.ToTable("HRS_NOSAWORKRESULT");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
