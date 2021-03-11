using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;
using System.ComponentModel;

namespace ERCHTMS.Entity.BaseManage 
{
    /// <summary>
    /// 描 述：用户管理
    /// </summary>
    public class UserEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 用户主键
        /// </summary>		
        [Column("USERID")]
        public string UserId { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>	
        [Column("ENCODE")]
        public string EnCode { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>	
        [Column("ACCOUNT")]
        public string Account { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>	
        [Column("PASSWORD")]
        public string Password { get; set; }
        /// <summary>
        /// 密码秘钥
        /// </summary>		
        [Column("SECRETKEY")]
        public string Secretkey { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>		
        [Column("REALNAME")]
        public string RealName { get; set; }
        /// <summary>
        /// 呢称
        /// </summary>		
        [Column("NICKNAME")]
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>		
        [Column("HEADICON")]
        public string HeadIcon { get; set; }
        /// <summary>
        ///省份
        /// </summary>		
        [Column("QUICKQUERY")]
        public string QuickQuery { get; set; }
        /// <summary>
        /// 省份编码
        /// </summary>		
        [Column("SIMPLESPELLING")]
        public string SimpleSpelling { get; set; }
        /// <summary>
        /// 性别
        /// </summary>		
        [Column("GENDER")]
        public string Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>		
        [Column("BIRTHDAY")]
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 手机
        /// </summary>		
        [Column("MOBILE")]
        public string Mobile { get; set; }
        /// <summary>
        /// 电话
        /// </summary>		
        [Column("TELEPHONE")]
        public string Telephone { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>		
        [Column("EMAIL")]
        public string Email { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>		
        [Column("OICQ")]
        public string OICQ { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>		
        [Column("WECHAT")]
        public string WeChat { get; set; }
        /// <summary>
        /// MSN
        /// </summary>		
        [Column("MSN")]
        public string MSN { get; set; }
        /// <summary>
        /// 市编码
        /// </summary>		
        [Column("MANAGERID")]
        public string ManagerId { get; set; }
        /// <summary>
        /// 市
        /// </summary>	
        [Column("MANAGER")]
        public string Manager { get; set; }
        /// <summary>
        /// 机构主键
        /// </summary>		
        [Column("ORGANIZEID")]
        public string OrganizeId { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>		
        [Column("ORGANIZECODE")]
        public string OrganizeCode { get; set; }
        /// <summary>
        /// 部门主键
        /// </summary>		
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 角色主键
        /// </summary>		
        [Column("ROLEID")]
        public string RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>		
        [Column("ROLENAME")]
        public string RoleName { get; set; }
        /// <summary>
        /// 岗位主键
        /// </summary>		
        [Column("DUTYID")]
        public string DutyId { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>		
        [Column("DUTYNAME")]
        public string DutyName { get; set; }
        /// <summary>
        /// 职位主键
        /// </summary>		
        [Column("POSTID")]
        public string PostId { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>		
        [Column("POSTNAME")]
        public string PostName { get; set; }
        /// <summary>
        /// 工作组主键
        /// </summary>		
        [Column("WORKGROUPID")]
        public string WorkGroupId { get; set; }
        /// <summary>
        /// 安全级别
        /// </summary>		
        [Column("SECURITYLEVEL")]
        public int?  SecurityLevel { get; set; }
        /// <summary>
        /// 在线状态
        /// </summary>		
        [Column("USERONLINE")]
        [DefaultValue(0)]
        public int?  UserOnLine { get; set; }
        /// <summary>
        /// 单点登录标识
        /// </summary>		
        [Column("OPENID")]
        public int?  OpenId { get; set; }
        /// <summary>
        /// 密码提示问题
        /// </summary>		
        [Column("QUESTION")]
        public string Question { get; set; }
        /// <summary>
        /// 密码提示答案
        /// </summary>		
        [Column("ANSWERQUESTION")]
        public string AnswerQuestion { get; set; }
        /// <summary>
        /// 允许多用户同时登录
        /// </summary>		
        [Column("CHECKONLINE")]
        public int? CheckOnLine { get; set; }
        /// <summary>
        /// 允许登录时间开始
        /// </summary>		
        [Column("ALLOWSTARTTIME")]
        public DateTime?  AllowStartTime { get; set; }
        /// <summary>
        /// 允许登录时间结束
        /// </summary>		
        [Column("ALLOWENDTIME")]
        public DateTime?  AllowEndTime { get; set; }
        /// <summary>
        /// 暂停用户开始日期
        /// </summary>		
        [Column("LOCKSTARTDATE")]
        public DateTime?  LockStartDate { get; set; }
        /// <summary>
        /// 暂停用户结束日期
        /// </summary>		
        [Column("LOCKENDDATE")]
        public DateTime?  LockEndDate { get; set; }
        /// <summary>
        /// 第一次访问时间
        /// </summary>		
        [Column("FIRSTVISIT")]
        public DateTime?  FirstVisit { get; set; }
        /// <summary>
        /// 上一次访问时间
        /// </summary>		
        [Column("PREVIOUSVISIT")]
        public DateTime? PreviousVisit { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>		
       [Column("LASTVISIT")]
        public DateTime?  LastVisit { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>		
        [Column("LOGONCOUNT")]
        public int?  LogOnCount { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>		
        [Column("SORTCODE")]
        public int?  SortCode { get; set; }
        /// <summary>
        /// 删除标记
        /// </summary>		
        [Column("DELETEMARK")]
        public int?  DeleteMark { get; set; }
        /// <summary>
        /// 有效标志
        /// </summary>		
        [Column("ENABLEDMARK")]
        public int? EnabledMark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>		
        [Column("CREATEDATE")]
        public DateTime?  CreateDate { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>		
        [Column("CREATEUSERID")]
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>		
        [Column("CREATEUSERNAME")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary>		
        [Column("MODIFYDATE")]
        public DateTime?  ModifyDate { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>		
        [Column("MODIFYUSERID")]
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改用户
        /// </summary>		
        [Column("MODIFYUSERNAME")]
        public string ModifyUserName { get; set; }

        /// 所属部门编码
        /// </summary>		
        [Column("DEPARTMENTCODE")]
        public string DepartmentCode { get; set; }
        /// <summary>
        /// 创建用户所属部门编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERDEPTCODE")]
        public string CreateUserDeptCode { get; set; }
        /// <summary>
        /// 创建用户所属机构编码
        /// </summary>
        /// <returns></returns>
        [Column("CREATEUSERORGCODE")]
        public string CreateUserOrgCode { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>	
        /// <returns></returns>
        [Column("IDENTIFYID")]
        public string IdentifyID { get; set; }

        /// <summary>
        /// 学历
        /// </summary>	

        [Column("DEGREES")]
        public string Degrees { get; set; }

        /// <summary>
        /// 学历主键
        /// </summary>		
        [Column("DEGREESID")]
        public string DegreesID { get; set; }


        /// <summary>
        /// 是否外包工程
        /// </summary>		
        [Column("ISEPIBOLY")]
        public string IsEpiboly { get; set; }

        /// <summary>
        /// 工种
        /// </summary>		
        [Column("CRAFT")]
        public string Craft { get; set; }

        /// <summary>
        /// 是否在场
        /// </summary>		
        [Column("ISPRESENCE")]
        public string IsPresence{ get; set; }

        /// <summary>
        /// 离场时间
        /// </summary>		
        [Column("DEPARTURETIME")]
        public DateTime? DepartureTime { get; set; }

        /// <summary>
        /// 籍贯
        /// </summary>		
        [Column("NATIVE")]
        public string Native { get; set; }
        /// <summary>
        /// 人员类型
        /// </summary>		
        [Column("USERTYPE")]
        public string UserType { get; set; }
        /// <summary>
        /// 民族
        /// </summary>		
        [Column("NATION")]
        public string Nation { get; set; }

        /// <summary>
        /// 是否特种作业人员
        /// </summary>		
        [Column("ISSPECIAL")]
        public string IsSpecial { get; set; }
        /// <summary>
        /// 是否特种设备操作人员
        /// </summary>		
        [Column("ISSPECIALEQU")]
        public string IsSpecialEqu { get; set; }
        /// <summary>
        /// 入场/入职时间
        /// </summary>		
        [Column("ENTERTIME")]
        public DateTime? EnterTime { get; set; }
        /// <summary>
        ///外包工程Id
        /// </summary>		
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        /// <summary>
        /// 是否为四种人
        /// </summary>
        [Column("ISFOURPERSON")]
        public string ISFOURPERSON { get; set; }
        /// <summary>
        /// 四种人类别
        /// </summary>
        [Column("FOURPERSONTYPE")]
        public string FOURPERSONTYPE { get; set; }
        /// <summary>
        /// 签名图片
        /// </summary>
        [Column("SIGNIMG")]
        public string SignImg { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Column("NEWPASSWORD")]
        public string NewPassword { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Column("AGE")]
        public string Age { get; set; }

        /// <summary>
        /// 政治面貌
        /// </summary>
        [Column("POLITICAL")]
        public string Political { get; set; }

        /// <summary>
        /// 工种工龄
        /// </summary>
        [Column("CRAFTAGE")]
        public string CraftAge { get; set; }

        /// <summary>
        /// 后期学历
        /// </summary>
        [Column("LATEDEGREES")]
        public string LateDegrees { get; set; }
        
        /// <summary>
        /// 后期学历
        /// </summary>
        [Column("LATEDEGREESID")]
        public string LateDegreesID { get; set; }

        /// <summary>
        /// 健康状况
        /// </summary>
        [Column("HEALTHSTATUS")]
        public string HealthStatus { get; set; }

        /// <summary>
        /// 技术等级
        /// </summary>
        [Column("TECHNICALGRADE")]
        public string TechnicalGrade { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        [Column("JOBTITLE")]
        public string JobTitle { get; set; }

        /// <summary>
        /// 是否转岗中 0为否 1为是
        /// </summary>
        [Column("ISTRANSFER")]
        public int IsTransfer { get; set; }

        /// <summary>
        /// 是否黑名单用户
        /// </summary>
        [Column("ISBLACK")]
        public int? IsBlack { get; set; }

        /// <summary>
        /// 离场原因 
        /// </summary>
        [Column("DEPARTUREREASON")]
        public string DepartureReason { get; set; }

        /// <summary>
        /// 职务Code 
        /// </summary>
        [Column("POSTCODE")]
        public string PostCode { get; set; }


        ///// <summary>
        ///// 工作状态(01:值班,02:休息)
        ///// </summary>
        //[Column("WORKSTATUS")]
        //public int? WorkStatus { get; set; }
        /// <summary>
        ///专业类别
        /// </summary>
        [Column("SPECIALTYTYPE")]
        public string SpecialtyType { get; set; }


        
        /// <summary>
        ///账号来源
        /// </summary>
        [Column("ACCOUNTTYPE")]
        public string AccountType { get; set; }

        /// <summary>
        ///是否申请ldap账号
        /// </summary>
        [Column("ISAPPLICATIONLDAP")]
        public string IsapplicationLdap { get; set; }

        /// <summary>
        ///是否培训管理员
        /// </summary>
        [Column("ISTRAINADMIN")]
        public int? IsTrainAdmin { get; set; }
        /// <summary>
        /// 是否修改过密码
        /// </summary>		
        [Column("ISBZ")]
        public int? IsBz { get; set; }
        /// <summary>
        /// 区
        /// </summary>		
        [Column("DISTRICT")]
        public string District { get; set; }
        /// <summary>
        /// 区编码
        /// </summary>		
        [Column("DISTRICTCODE")]
        public string DistrictCode { get; set; }
        /// <summary>
        /// 街道
        /// </summary>		
        [Column("STREET")]
        public string Street { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>		
        [Column("ADDRESS")]
        public string Address { get; set; }
        /// <summary>
        /// 受训角色ID(多个用英文逗号分隔)
        /// </summary>		
        [Column("TRAINROLEID")]
        public string TrainRoleId { get; set; }
        /// <summary>
        /// 受训角色名称(多个用英文逗号分隔)
        /// </summary>		
        [Column("TRAINROLENAME")]
        public string TrainRoleName { get; set; }

        /// <summary>
        /// 密码连续错误次数
        /// </summary>		
        [Column("PWDERRORCOUNT")]
        public int? PwdErrorCount { get; set; }


        /// <summary>
        /// 关联java培训平台账号(若有值则单点登录及获取人员档案相关接口要转换传此账号)
        /// </summary>	
        [Column("NEWACCOUNT")]
        public string NewAccount { get; set; }

        /// <summary>
        /// 是否离场审批中 0为否 1为是
        /// </summary>
        [Column("ISLEAVING")]
        public int? IsLeaving { get; set; } = 0;

        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
            var user = OperatorProvider.Provider.Current();
            this.UserId =string.IsNullOrEmpty(UserId)? Guid.NewGuid().ToString():UserId;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = user==null?"":user.UserId;
            this.CreateUserName = user==null?"":user.UserName;
            this.DepartmentId = string.IsNullOrEmpty(DepartmentId) ? this.OrganizeId : DepartmentId;
            this.DepartmentCode = string.IsNullOrEmpty(DepartmentCode) ? this.OrganizeCode : DepartmentCode;
            this.DeleteMark = 0;
            this.EnabledMark = 1;
            
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
            var user = OperatorProvider.Provider.Current();
            this.UserId = keyValue;
            this.ModifyDate = DateTime.Now;
            this.ModifyUserId = user == null ? "" : user.UserId;
            this.ModifyUserName = user == null ? "" : user.UserName;
        }
        #endregion
    }

}