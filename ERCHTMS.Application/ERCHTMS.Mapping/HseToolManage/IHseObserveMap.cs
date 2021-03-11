using ERCHTMS.Entity.HseToolMange;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.HseToolManage
{
    public class IHseObserveMap : EntityTypeConfiguration<HseObserveEntity>
    {

        public IHseObserveMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_SECURITYOBSERVE");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
