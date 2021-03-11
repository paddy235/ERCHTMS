using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    /// <summary>
    /// 描 述：人员GPS关联表
    /// </summary>
    public class PersongpsMap : EntityTypeConfiguration<PersongpsEntity>
    {
        public PersongpsMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_PERSONGPS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
