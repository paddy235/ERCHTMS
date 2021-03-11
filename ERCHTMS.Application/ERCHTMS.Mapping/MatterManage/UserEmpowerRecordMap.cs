using ERCHTMS.Entity.MatterManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.MatterManage
{
    /// <summary>
    /// 用户授权记录
    /// </summary>
    public class UserEmpowerRecordMap : EntityTypeConfiguration<UserEmpowerRecordEntity>
    {
       public UserEmpowerRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("WL_USEREMPOWERRECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
