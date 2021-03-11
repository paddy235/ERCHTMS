using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using ERCHTMS.Entity.CarManage;

namespace ERCHTMS.Mapping.CarManage
{

    /// <summary>
    /// 访客管理-随行人员附件表
    /// </summary>
    public class UserCarFileMultipleMap : EntityTypeConfiguration<UserCarFileMultipleEntity>
    {
        public UserCarFileMultipleMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_USERCARFILE_MULTIPLE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
