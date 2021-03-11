using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：复工申请历史记录表
    /// </summary>
    public class HisReturnWorkMap : EntityTypeConfiguration<HisReturnWorkEntity>
    {
        public HisReturnWorkMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_HISTORYRETURNTOWORK");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
