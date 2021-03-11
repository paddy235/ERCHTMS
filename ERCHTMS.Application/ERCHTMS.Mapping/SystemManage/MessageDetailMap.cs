﻿using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Mapping.SystemManage
{
    public class MessageDetailMap : EntityTypeConfiguration<MessageDetail>
    {
        public MessageDetailMap()
        {
            #region 表、主键
            //表
            this.ToTable("BASE_MESSAGEDETAIL");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
