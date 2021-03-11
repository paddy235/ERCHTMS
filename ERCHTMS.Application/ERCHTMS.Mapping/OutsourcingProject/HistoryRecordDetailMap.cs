﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    public class HistoryRecordDetailMap : EntityTypeConfiguration<HistoryRecordDetail>
    {
        public HistoryRecordDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_HISTORYRECORDDETAIL");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
