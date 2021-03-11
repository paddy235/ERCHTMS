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
    /// 计量管理详情映射
    /// </summary>
    public class CalculateDetailedMap : EntityTypeConfiguration<CalculateDetailedEntity>
    {
        public CalculateDetailedMap()
        {
            #region 表、主键
            //表
            this.ToTable("WL_CALCULATEDETAILED");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
