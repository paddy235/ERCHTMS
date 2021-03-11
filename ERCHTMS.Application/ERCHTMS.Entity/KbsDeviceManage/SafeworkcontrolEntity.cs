using System;
using ERCHTMS.Code;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ERCHTMS.Entity.KbsDeviceManage
{
    /// <summary>
    /// 描 述：作业现场安全管控
    /// </summary>
    [Table("BIS_SAFEWORKCONTROL")]
    public class SafeworkcontrolEntity : BaseEntity
    {
        #region 实体成员

        /// <summary>
        /// 创建人所属机构
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CREATEUSERORGCODE { get; set; }
        /// <summary>
        /// 创建人所属部门
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CREATEUSERDEPTCODE { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYDATE")]
        public DateTime? MODIFYDATE { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        /// <returns></returns>
        [Column("MODIFYUSERID")]
        public string MODIFYUSERID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        [Column("CREATEDATE")]
        public DateTime? CREATEDATE { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERID")]
        public string CREATEUSERID { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <returns></returns>
        [Column("ID")]
        public string ID { get; set; }


        /// <summary>
        /// 备用
        /// </summary>
        /// <returns></returns>
        [Column("REMARK")]
        public string Remark { get; set; }
        /// <summary>
        /// 半径
        /// </summary>
        /// <returns></returns>
        [Column("RADIUS")]
        public int? Radius { get; set; }
        /// <summary>
        /// 区域坐标点
        /// </summary>
        /// <returns></returns>
        [Column("AREACODE")]
        public string Areacode { get; set; }
        /// <summary>
        /// 区域状态0正方形1圆形2手绘
        /// </summary>
        /// <returns></returns>
        [Column("AREASTATE")]
        public int? Areastate { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        /// <returns></returns>
        [Column("INVALIDTIME")]
        public DateTime? Invalidtime { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        /// <returns></returns>
        [Column("TAKEEFFECTTIME")]
        public DateTime? Takeeffecttime { get; set; }
        /// <summary>
        /// 隔离区域id
        /// </summary>
        /// <returns></returns>
        [Column("QUARANTINEID")]
        public string Quarantineid { get; set; }
        /// <summary>
        /// 隔离区域code
        /// </summary>
        /// <returns></returns>
        [Column("QUARANTINECODE")]
        public string Quarantinecode { get; set; }
        /// <summary>
        /// 隔离区域名称
        /// </summary>
        /// <returns></returns>
        [Column("QUARANTINENAME")]
        public string Quarantinename { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        /// <returns></returns>
        [Column("PLANENSTARTTIME")]
        public DateTime? PlanenStarttime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        /// <returns></returns>
        [Column("PLANENDTIME")]
        public DateTime? Planendtime { get; set; }
        /// <summary>
        /// 实际开始时间
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALSTARTTIME")]
        public DateTime? Actualstarttime { get; set; }
        /// <summary>
        /// 实际结束时间
        /// </summary>
        /// <returns></returns>
        [Column("ACTUALENDTIME")]
        public DateTime? ActualEndTime { get; set; }
      
        /// <summary>
        /// 作业成员Id
        /// </summary>
        /// <returns></returns>
        [Column("TASKMEMBERID")]
        public string Taskmemberid { get; set; }
        /// <summary>
        /// 作业成员名称
        /// </summary>
        /// <returns></returns>
        [Column("TASKMEMBERNAME")]
        public string Taskmembername { get; set; }
        /// <summary>
        /// 作业负责人Id
        /// </summary>
        /// <returns></returns>
        [Column("TASKMANAGEID")]
        public string Taskmanageid { get; set; }
        /// <summary>
        /// 作业负责人名称
        /// </summary>
        /// <returns></returns>
        [Column("TASKMANAGENAME")]
        public string Taskmanagename { get; set; }
        /// <summary>
        /// 部门/班组Id
        /// </summary>
        /// <returns></returns>
        [Column("DEPTID")]
        public string Deptid { get; set; }
        /// <summary>
        /// 部门/班组编码
        /// </summary>
        /// <returns></returns>
        [Column("DEPTCODE")]
        public string Deptcode { get; set; }
        /// <summary>
        /// 部门/班组名称
        /// </summary>
        /// <returns></returns>
        [Column("DEPTNAME")]
        public string Deptname { get; set; }
        /// <summary>
        /// 作业内容
        /// </summary>
        /// <returns></returns>
        [Column("TASKCONTENT")]
        public string Taskcontent { get; set; }
        /// <summary>
        /// 作业区域ID
        /// </summary>
        /// <returns></returns>
        [Column("TASKREGIONID")]
        public string Taskregionid { get; set; }
        /// <summary>
        /// 作业区域编码
        /// </summary>
        /// <returns></returns>
        [Column("TASKREGIONCODE")]
        public string Taskregioncode { get; set; }
        /// <summary>
        /// 作业区域名称
        /// </summary>
        /// <returns></returns>
        [Column("TASKREGIONNAME")]
        public string Taskregionname { get; set; }
        /// <summary>
        /// 作业类型
        /// </summary>
        /// <returns></returns>
        [Column("TASKTYPE")]
        public string Tasktype { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        /// <returns></returns>
        [Column("TASKNAME")]
        public string Taskname { get; set; }
        /// <summary>
        /// 工作票编号
        /// </summary>
        /// <returns></returns>
        [Column("WORKNO")]
        public string Workno { get; set; }

        /// <summary>
        /// 状态 0保存任务 1提交任务开始 2任务结束
        /// </summary>
        [Column("STATE")]
        public int State { get; set; }

        /// <summary>
        /// 风险等级
        /// </summary>
        [Column("DANGERLEVEL")]
        public string DangerLevel { get; set; }

        /// <summary>
        /// 电子围栏内摄像头ID
        /// </summary>
        [Column("COMERID")]
        public string comerid { get; set; }

       
        /// <summary>
        /// /工作签发人
        /// </summary>
        [Column("ISSUENAME")]
        public string IssueName { get; set; }
        /// <summary>
        /// /工作签发人Id
        /// </summary>
        [Column("ISSUEUSERID")]
        public string IssueUserid { get; set; }

        /// <summary>
        /// /工作许可人
        /// </summary>
        [Column("PERMITNAME")]
        public string PermitName { get; set; }
        
        /// <summary>
        /// /工作许可人
        /// </summary>
        [Column("PERMITUSERID")]
        public string PermitUserid { get; set; }

        /// <summary>
        /// 监护人（施工单位）
        /// </summary>
        /// <returns></returns>
        [Column("GUARDIANID")]
        public string Guardianid { get; set; }
        /// <summary>
        /// 监护人（施工单位）
        /// </summary>
        /// <returns></returns>
        [Column("GUARDIANNAME")]
        public string Guardianname { get; set; }

        /// <summary>
        /// 监护人（主管部门）
        /// </summary>
        [Column("EXECUTIVENAMES")]
        public string ExecutiveNames { get; set; }
        /// <summary>
        /// 监护人（主管部门）
        /// </summary>
        [Column("EXECUTIVEIDS")]
        public string ExecutiveIds { get; set; }

        /// <summary>
        /// 监护人（监察部门）
        /// </summary>
        [Column("SUPERVISIONNAMES")]
        public string SupervisionNames { get; set; }
        /// <summary>
        /// 监护人（监察部门）
        /// </summary>
        [Column("SUPERVISIONIDS")]
        public string SupervisionIds { get; set; }
         

        #endregion

        #region 扩展操作
        /// <summary>
        /// 作业监护人
        /// </summary>
        [NotMapped]
        public List<SafeworkUserEntity> MonitorUsers { get; set; }
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            this.ID = string.IsNullOrEmpty(ID) ? Guid.NewGuid().ToString() : ID;
            this.CREATEDATE = DateTime.Now;
            this.CREATEUSERID = OperatorProvider.Provider.Current().UserId;
            this.CREATEUSERDEPTCODE = OperatorProvider.Provider.Current().DeptCode;
            this.CREATEUSERORGCODE = OperatorProvider.Provider.Current().OrganizeCode;
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            this.ID = keyValue;
            this.MODIFYDATE = DateTime.Now;
            this.MODIFYUSERID = OperatorProvider.Provider.Current().UserId;
        }
        #endregion
    }

    /// <summary>
    /// 手机接口序列化实体
    /// </summary>
    public class Safeworkcontro
    {
        [NotMapped]
        public string userId { get; set; }
        public SafeworkcontrolEntity data { get; set; }
    }

}