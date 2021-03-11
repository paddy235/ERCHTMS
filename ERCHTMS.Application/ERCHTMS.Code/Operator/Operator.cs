using System;
using System.Collections.Generic;

namespace ERCHTMS.Code
{
    /// <summary>
    /// 描 述：当前操作者信息类
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdentifyID { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登陆账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LogTime { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string Secretkey { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 机构Id
        /// </summary>
        public string OrganizeId { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public string DeptId { get; set; }
        /// <summary>
        /// 部门编码
        /// </summary>
        public string DeptCode { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 机构编码
        /// </summary>
        public string OrganizeCode { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string OrganizeName { get; set; }
        /// <summary>
        /// 对象用户关系Id,对象分类:1-部门2-角色3-岗位4-群组
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// 登录IP地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 登录IP地址所在地址
        /// </summary>
        public string IPAddressName { get; set; }
        /// <summary>
        /// 是否系统账户；拥有所以权限
        /// </summary>
        public bool IsSystem { get; set; }
        /// <summary>
        /// 登录Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 拥有角色ID（多个用英文逗号分隔）
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 所属角色名称（多个用英文逗号分隔）
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 职务名称（多个用英文逗号分隔）
        /// </summary>
        public string PostName { get; set; }
        /// <summary>
        /// 职务Id（多个用英文逗号分隔）
        /// </summary>
        public string PostId { get; set; }
        /// <summary>
        ///岗位名称
        /// </summary>
        public string DutyName { get; set; }
        /// <summary>
        ///岗位id
        /// </summary>
        public string DutyId { get; set; }
        /// <summary>
        ///用户照片
        /// </summary>
        public string Photo { get; set; }

        ///// <summary>
        ///// 发包部门ID
        ///// </summary>
        public string SendDeptID { get; set; }

        /// <summary>
        /// 流程下用户模式
        /// </summary>
        public string wfMode { get; set; }
        /// <summary>
        /// 隐患ID|隐患名称
        /// </summary>
        public string rankArgs { get; set; }
        /// <summary>
        /// 是否负责人
        /// </summary>
        public string isPrincipal { get; set; }

        /// <summary>
        /// 是否公司及厂级
        /// </summary>
        public string isPlanLevel { get; set; }
        /// <summary>
        /// 用户数据权限
        /// </summary>
        public AuthorizeDataModel DataAuthorize { get; set; }

        /// <summary>
        /// 上级部门id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 用户标记
        /// </summary>
        public string uMark { get; set; }

        /// <summary>
        /// 所属工程
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// 是否为承包商
        /// </summary>
        public bool isEpiboly { get; set; }
        /// <summary>
        ///所属机构新的编码（对应部门表中新加的编码字段deptcode）
        /// </summary>
        public string NewDeptCode { get; set; }
        /// <summary>
        ///签名图片
        /// </summary>
        public string SignImg { get; set; }
        /// <summary>
        /// 是否对接培训云平台
        /// </summary>		
        public int? IsTrain { get; set; }

        /// <summary>
        /// 是否国电新疆红雁池
        /// </summary>
        public int IsGdxjUser { get; set; }
        /// <summary>
        /// 单位所属行业
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// 所属专业类别
        /// </summary>
        public string SpecialtyType { get; set; }
        /// <summary>
        ///是否培训管理员
        /// </summary>
        public int? IsTrainAdmin { get; set; }

        /// <summary>
        /// 账号来源（0:本地，1:LDAP）
        /// </summary>
        public string AccountType { get; set; }

    }
}
