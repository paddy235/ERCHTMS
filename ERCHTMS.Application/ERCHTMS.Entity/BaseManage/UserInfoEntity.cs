using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERCHTMS.Entity;
using ERCHTMS.Code;
using System.ComponentModel;

namespace ERCHTMS.Entity.BaseManage 
{
    /// <summary>
    /// 描 述：用户基本信息(视图)
    /// </summary>
    [Table("V_USERINFO")]
    public class UserInfoEntity : BaseEntity
    {
        #region 实体成员
        /// <summary>
        /// 用户主键
        /// </summary>	
        [Key]
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
        /// 登陆密码
        /// </summary>
          [Column("PASSWORD")]
        public string Password { get; set; }
        /// <summary>
        /// 密钥
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
        /// 角色名称
        /// </summary>		
        [Column("ROLENAME")]
        public string RoleName { get; set; }
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
        /// 主管
        /// </summary>	
        [Column("MANAGER")]
        public string Manager { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>		
        [Column("DUTYNAME")]
        public string DutyName { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>		
        [Column("POSTNAME")]
        public string PostName { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>		
        [Column("ORGANIZENAME")]
        public string OrganizeName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>		
        [Column("DEPTNAME")]
        public string DeptName { get; set; }
        /// <summary>
        /// 单位新编码
        /// </summary>
        [Column("DEPTCODE")]
        public string DeptCode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>		
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        /// <summary>
        /// 所属机构ID
        /// </summary>		
        [Column("ORGANIZEID")]
        public string OrganizeId { get; set; }
        /// <summary>
        /// 所属机构Code
        /// </summary>		
        [Column("ORGANIZECODE")]
        public string OrganizeCode { get; set; }
        /// <summary>
        ///职务ID
        /// </summary>		
        [Column("DUTYID")]
        public string DutyId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>		
        [Column("ROLEID")]
        public string RoleId { get; set; }
        /// <summary>
        /// 岗位ID
        /// </summary>		
        [Column("POSTID")]
        public string PostId { get; set; }
        /// <summary>
        /// 主管ID
        /// </summary>		
        [Column("MANAGERID")]
        public string ManagerId { get; set; }
        /// <summary>
        /// 所属部门ID
        /// </summary>		
        [Column("DEPARTMENTID")]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 所属部门编码
        /// </summary>		
        [Column("DEPARTMENTCODE")]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// 有效标志
        /// </summary>		
        [Column("ENABLEDMARK")]
        public int? EnabledMark { get; set; }
        /// <summary>
        /// 发包部门
        /// </summary>		
        [Column("SENDDEPTID")]
        public string SendDeptID { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [Column("IDENTIFYID")]
        public string IdentifyID { get; set; }
        /// <summary>
        /// 上级部门id
        /// </summary>		
        [Column("PARENTID")]
        public string ParentId { get; set; }

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
        /// 入场时间
        /// </summary>		
        [Column("ENTERTIME")]
        public DateTime? EnterTime { get; set; }
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
        /// 是否外包
        /// </summary>		
        [Column("ISEPIBOLY")]
        public string isEpiboly { get; set; }
        /// <summary>
        /// 是否在职
        /// </summary>		
        [Column("ISPRESENCE")]
        public string isPresence { get; set; }
        /// <summary>
        /// 工程Id
        /// </summary>		
        [Column("PROJECTID")]
        public string ProjectId { get; set; }
        /// <summary>
        /// 是否黑名单人员
        /// </summary>		
        [Column("ISBLACK")]
        public int? IsBlack { get; set; }
        /// <summary>
        /// 离场时间
        /// </summary>		
        [Column("DEPARTURETIME")]
        public DateTime? DepartureTime { get; set; }
        /// <summary>
        /// 单点登陆标记
        /// </summary>		
        [Column("OPENID")]
        public int? OpenId { get; set; } 

        /// <summary>
        /// 是否为四种人
        /// </summary>
        [Column("ISFOURPERSON")]
        public string IsFourPerson { get; set; }

        /// <summary>
        /// 四种人类别
        /// </summary>
        [Column("FOURPERSONTYPE")]
        public string FourPersonType { get; set; }
        /// <summary>
        /// 签名图片
        /// </summary>
        [Column("SIGNIMG")]
        public string SignImg { get; set; }
        /// <summary>
        /// 部门性质
        /// </summary>
        [Column("NATURE")]
        public string Nature { get; set; }
        /// <summary>
        /// 是否对接培训云平台
        /// </summary>		
        [Column("ISTRAIN")]
        public int? IsTrain { get; set; }

        /// <summary>
        /// 允许登录时间开始
        /// </summary>		
        [Column("ALLOWSTARTTIME")]
        public DateTime? AllowStartTime { get; set; }
        /// <summary>
        /// 允许登录时间结束
        /// </summary>		
        [Column("ALLOWENDTIME")]
        public DateTime? AllowEndTime { get; set; }

        /// <summary>
        /// 专业类别
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
        /// 最后访问时间
        /// </summary>		
        [Column("LASTVISIT")]
        public DateTime? LastVisit { get; set; }
        /// <summary>
        /// 上级部门id
        /// </summary>		
        [Column("PARENTNAME")]
        public string ParentName { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Column("AGE")]
        public string Age { get; set; }
        #endregion

        #region 扩展操作
        /// <summary>
        /// 新增调用
        /// </summary>
        public override void Create()
        {
        }
        /// <summary>
        /// 编辑调用
        /// </summary>
        /// <param name="keyValue"></param>
        public override void Modify(string keyValue)
        {
        }
        #endregion
    }

}