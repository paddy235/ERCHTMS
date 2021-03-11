using ERCHTMS.Entity.CarManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.CarManage
{
    public class PersonSetMap : EntityTypeConfiguration<PersonSetEntity>
    {
        public PersonSetMap()
        {
            #region 表、主键
            //表
            this.ToTable("HJB_PERSONSET");
            //主键
            this.HasKey(t => t.PersonSetId);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
