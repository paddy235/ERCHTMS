using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.OutsourcingProject
{
    /// <summary>
    /// 描 述：资质审查人员证件表
    /// </summary>
    [Table("EPG_CERTIFICATEINSPECTORS")]
    public class CertificateinspectorsEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 发证机关
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSORG")]
        public string CREDENTIALSORG { get; set; }
        /// <summary>
        /// 证件照片地址
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSFILE")]
        public string CREDENTIALSFILE { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }
        /// <summary>
        /// 资质审查人员Id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string USERID { get; set; }
        /// <summary>
        /// 有效期(年)
        /// </summary>
        /// <returns></returns>
        [Column("VALIDTTIME")]
        public string VALIDTTIME { get; set; }
        /// <summary>
        /// 证件名称
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSNAME")]
        public string CREDENTIALSNAME { get; set; }
        /// <summary>
        /// 证件编号
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSCODE")]
        public string CREDENTIALSCODE { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        /// <returns></returns>
        [Column("CREDENTIALSTIME")]
        public DateTime? CREDENTIALSTIME { get; set; }
        /// <summary>
        /// 证件类型
        /// </summary>
        /// <returns></returns>
        [Column("CERTTYPE")]
        public string CertType { get; set; }

        /// <summary>
        /// 种类/作业类别
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public string WorkType { get; set; }
        /// <summary>
        /// 作业项目/准操项目
        /// </summary>
        /// <returns></returns>
        [Column("WORKITEM")]
        public string WorkItem { get; set; }
        /// <summary>
        /// 项目代号
        /// </summary>
        /// <returns></returns>
        [Column("ITEMNUM")]
        public string ItemNum { get; set; }
        
        /// <summary>
        /// 复审日期
        /// </summary>
        /// <returns></returns>
        [Column("APPLYDATE")]
        public DateTime? ApplyDate { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        /// <returns></returns>
        [Column("STARTDATE")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        ///等级
        /// </summary>
        /// <returns></returns>
        [Column("GRADE")]
        public string Grade { get; set; }

        /// <summary>
        ///行业
        /// </summary>
        /// <returns></returns>
        [Column("INDUSTRY")]
        public string Industry { get; set; }

        /// <summary>
        ///人员类型
        /// </summary>
        /// <returns></returns>
        [Column("USERTYPE")]
        public string UserType { get; set; }
        /// <summary>
        ///工种
        /// </summary>
        /// <returns></returns>
        [Column("CRAFT")]
        public string Craft { get; set; }
        /// <summary>
        ///资格名称
        /// </summary>
        /// <returns></returns>
        [Column("ZGNAME")]
        public string ZGName { get; set; }

        /// <summary>
        /// 证书失效日期
        /// </summary>
        /// <returns></returns>
        [Column("ENDDATE")]
        public DateTime? EndDate { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID)?Guid.NewGuid().ToString():ID;

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