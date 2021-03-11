﻿using ERCHTMS.Entity.FlowManage;
using System.Data.Entity.ModelConfiguration;
namespace ERCHTMS.Mapping.FlowManage
{
    /// <summary>
    /// 描 述：工作流委托规则与工作流模板对应表
    /// </summary>
    public class WFDelegateRuleSchemeInfoMap : EntityTypeConfiguration<WFDelegateRuleSchemeInfoEntity>
    {
        public WFDelegateRuleSchemeInfoMap()
        {
            #region 表、主键
            //表
            this.ToTable("WF_DELEGATERULESCHEMEINFO");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}
