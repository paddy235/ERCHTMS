using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using System.Data.Entity.ModelConfiguration;
using ERCHTMS.Entity.BreakRulesManage;

namespace ERCHTMS.Mapping.BreakRulesManage
{

    public class BrAssessMap : EntityTypeConfiguration<BrAssessEntity>
    {
        public BrAssessMap() 
        {

            #region 表、主键
            //表
            this.ToTable("BIS_BRASSESS");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
    }
}