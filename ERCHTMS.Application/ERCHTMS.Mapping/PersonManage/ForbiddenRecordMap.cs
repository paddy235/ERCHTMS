using ERCHTMS.Entity.PersonManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.PersonManage
{
    public class ForbiddenRecordMap : EntityTypeConfiguration<ForbiddenRecordEntity>
    {
        /// <summary>
        /// 禁入记录映射表
        /// </summary>
        public ForbiddenRecordMap()
        {
            #region 表、主键
            //表
            this.ToTable("BIS_FORBIDDENRECORD");
            //主键
            this.HasKey(t => t.ID);
            #endregion

        }
    }
}
