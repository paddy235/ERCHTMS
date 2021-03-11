using ERCHTMS.Entity.PersonManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.PersonManage
{
    /// <summary>
    /// 临时分组管理映射
    /// </summary>
    public class TemporaryGroupsMap : EntityTypeConfiguration<TemporaryGroupsEntity>
    {
        public TemporaryGroupsMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_TEMPORARYGROUPS");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
