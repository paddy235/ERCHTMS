﻿using ERCHTMS.Entity.FlowManage;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.FlowManage
{
    public class WFProcessTransitionHistoryMap : EntityTypeConfiguration<WFProcessTransitionHistoryEntity>
    {
        /// <summary>
        /// 描 述：工作流实例节点转化记录表
        /// </summary>
        public WFProcessTransitionHistoryMap()
        {
            #region 表、主键
            //表
            this.ToTable("WF_PROCESSTRANSITIONHISTORY");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
