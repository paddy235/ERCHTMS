using ERCHTMS.Entity.HseToolMange;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.HseToolManage
{
    /// <summary>
    /// 安全观察内容标准
    /// </summary>
    public class HseObserveNormMap : EntityTypeConfiguration<HseObserveNormEntity>
    {
        public HseObserveNormMap()
        {
            #region 表、主键
            //表
            this.ToTable("HSE_SECURITYOBSERVENORM");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
