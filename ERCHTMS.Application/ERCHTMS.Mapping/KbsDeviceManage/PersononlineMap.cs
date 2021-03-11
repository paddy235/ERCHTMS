using ERCHTMS.Entity.KbsDeviceManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.KbsDeviceManage
{
    /// <summary>
    /// 描 述：人员每小时在场记录
    /// </summary>
    public class PersononlineMap : EntityTypeConfiguration<PersononlineEntity>
    {
        public PersononlineMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_PERSONONLINE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
