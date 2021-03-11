using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// 描 述：用户门禁数据类
    /// </summary>
    public class AcesscontrolinfoMap : EntityTypeConfiguration<AcesscontrolinfoEntity>
    {
        public AcesscontrolinfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_ACESSCONTROLINFO");
            //主键
            this.HasKey(t => t.TID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
