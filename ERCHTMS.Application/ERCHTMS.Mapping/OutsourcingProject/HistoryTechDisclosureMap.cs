﻿using ERCHTMS.Entity.OutsourcingProject;
using System.Data.Entity.ModelConfiguration;

namespace ERCHTMS.Mapping.OutsourcingProject
{
    /// <summary>
    /// 描 述：历史方案措施管理
    /// </summary>
    public class HistoryTechDisclosureMap : EntityTypeConfiguration<HistoryTechDisclosureEntity>
    {
        public HistoryTechDisclosureMap()
        {
            #region 表、主键
            //表
            this.ToTable("EPG_HISTORYTECHDISCLOSURE");
            //主键
            this.HasKey(t => t.ID);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}