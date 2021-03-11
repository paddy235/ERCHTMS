using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using System.Data.Entity.ModelConfiguration;
using ERCHTMS.Entity.BreakRulesManage;


namespace ERCHTMS.Mapping.BreakRulesManage
{
    /// <summary>
    /// 描 述：违章验收管理
    /// </summary>
    public class BrAcceptMap : EntityTypeConfiguration<BrAcceptEntity>
    {

        public BrAcceptMap() 
        {

            #region 表、主键
            //表
            this.ToTable("BIS_BRACCEPT");
            //主键
            this.HasKey(t => t.Id);
            #endregion

            #region 配置关系
            #endregion
        }
        
    }
}