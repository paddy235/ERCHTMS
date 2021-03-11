using ERCHTMS.Entity.PersonManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// 描 述：三种人审批业务表
    /// </summary>
    public class ThreePeopleInfoMap : EntityTypeConfiguration<ThreePeopleInfoEntity>
    {
        public ThreePeopleInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_THREEPEOPLEINFO");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
