using ERCHTMS.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.SystemManage
{
    /// <summary>
    /// 描 述：流程业务明细
    /// </summary>
    public class WfTBInstanceDetailMap : EntityTypeConfiguration<WfTBInstanceDetailEntity>
    {
        public WfTBInstanceDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("SYS_WFTBINSTANCEDETAIL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}