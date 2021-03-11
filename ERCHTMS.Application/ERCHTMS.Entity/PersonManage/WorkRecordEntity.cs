using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ERCHTMS.Entity.PersonManage
{
    /// <summary>
    /// 描 述：人员工作记录
    /// </summary>
    [Table("bis_workrecord")]
    public class WorkRecordEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 所属部门Id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string DeptId { get; set; }
        /// <summary>
        /// 所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        /// <returns></returns>
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        [Column("USERNAME")]
        public string UserName { get; set; }
        /// <summary>
        /// 入场（职）时间
        /// </summary>
        /// <returns></returns>
        [Column("ENTERDATE")]
        public DateTime EnterDate { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        ///岗位
        /// </summary>
        /// <returns></returns>
        [Column("POSTNAME")]
        public string PostName { get; set; }
        /// <summary>
        ///离场（职）时间
        /// </summary>
        /// <returns></returns>
        [Column("LEAVETIME")]
        public DateTime LeaveTime { get; set; }
        /// <summary>
        /// 备用
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 所属机构名称
        /// </summary>
        /// <returns></returns>
        [Column("ORGANIZENAME")]
        public string OrganizeName { get; set; }
        /// <summary>
        /// 职务名称
        /// </summary>
        /// <returns></returns>
        [Column("JOBNAME")]
        public string JobName { get; set; }

        /// <summary>
        /// 记录类型 0为手动添加 1为系统自动生成
        /// </summary>
        /// <returns></returns>
        [Column("WORKTYPE")]
        public int WorkType { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.Id = string.IsNullOrEmpty(Id)?Guid.NewGuid().ToString():Id;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = OperatorProvider.Provider.Current().UserId;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.Id = keyValue;
        }
        #endregion
    }
}