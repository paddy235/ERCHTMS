using ERCHTMS.Entity.PersonManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.PersonManage
{
    public class WorkRecordMap : EntityTypeConfiguration<WorkRecordEntity>
    {
        public WorkRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_WORKRECORD");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
