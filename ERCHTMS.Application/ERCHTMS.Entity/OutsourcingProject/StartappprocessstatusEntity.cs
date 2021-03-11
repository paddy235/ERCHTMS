using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：工程流程状态表
    /// </summary>
    [Table("EPG_STARTAPPPROCESSSTATUS")]
    public class StartappprocessstatusEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 开工申请Id
        /// </summary>
        /// <returns></returns>
        [Column("STARTAPPLYID")]
        public string STARTAPPLYID { get; set; }
        /// <summary>
        /// 设备工器具验收状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("EQUIPMENTTOOLSTATUS")]
        public string EQUIPMENTTOOLSTATUS { get; set; }
        /// <summary>
        /// 合同与协议状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("PACTSTATUS")]
        public string PACTSTATUS { get; set; }
        /// <summary>
        /// 安全技术交底状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("TECHNICALSTATUS")]
        public string TECHNICALSTATUS { get; set; }
        /// <summary>
        /// 三措两案状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("THREETWOSTATUS")]
        public string THREETWOSTATUS { get; set; }
        /// <summary>
        /// 外包工程Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTENGINEERID")]
        public string OUTENGINEERID { get; set; }
        /// <summary>
        /// 保证金状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("SECURITYSTATUS")]
        public string SECURITYSTATUS { get; set; }
        /// <summary>
        /// 资质审查状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("EXAMSTATUS")]
        public string EXAMSTATUS { get; set; }
        /// <summary>
        /// 备注字段状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string REMARK { get; set; }
        /// <summary>
        /// 外包单位Id
        /// </summary>
        /// <returns></returns>
        [Column("OUTPROJECTID")]
        public string OUTPROJECTID { get; set; }

        /// <summary>
        /// 人员审查状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("PEOPLESTATUS")]
        public string PEOPLESTATUS { get; set; }
        
        /// <summary>
        /// 合同审查状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("COMPACTSTATUS")]
        public string COMPACTSTATUS { get; set; }
        
        /// <summary>
        /// 特种设备审查状态(0:未完成1:完成)
        /// </summary>
        /// <returns></returns>
        [Column("SPTOOLSSTATUS")]
        public string SPTOOLSSTATUS { get; set; }
        
            
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;

        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
        }
        #endregion
    }
}