using BSFramework.Util;
using ERCHTMS.Busines;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// Api基类对象，用于获取用户对象，以甄别当前登陆用户是否失效
    /// </summary>
    public class BaseApiController : ApiController
    {
        public Operator curUser = new Operator();
        public string AppSign { get; set; }

        private AccountBLL accountBLL = new AccountBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private UserBLL userbll = new UserBLL();

        public BaseApiController()
        {
            string userId = string.Empty;
            string json = HttpContext.Current.Request.Form["json"];
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(json);
                    if (json.Contains("\"userid\":") || json.Contains("\'userid\':")) 
                    {
                        userId = dy.userid.ToString();
                    }
                    if (json.Contains("\"userId\":") || json.Contains("\'userId\':"))
                    {
                        userId = dy.userId.ToString();
                    }
                    AppSign = "ios"; //默认指定ios
                    if (json.Contains("global_apptype"))
                    {
                        if (null != dy.global_apptype)
                        {
                            AppSign = dy.global_apptype.ToString();
                        }
                    }
                    OperatorProvider.AppUserId = userId;  //设置当前用户
                }
                catch (Exception ex)
                {
                    AppSign = "ios";
                }
            }
            curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                //用户id不为空
                if (!string.IsNullOrEmpty(userId))
                {
                    curUser = GetOperator(userId);
                }
            }
        }


        public Operator GetOperator(string userId)
        {
            AuthorizeBLL authorizeBLL = new AuthorizeBLL();
            UserBLL userBLL = new UserBLL();
            UserInfoEntity userEntity = userbll.GetUserInfoEntity(userId);
            if (userEntity == null)
            {
                return null;
            }
            Operator operators = new Operator();
            operators.UserId = userEntity.UserId;
            operators.Code = userEntity.EnCode;
            operators.Account = userEntity.Account;
            operators.UserName = userEntity.RealName;
            operators.Password = userEntity.Password;
            operators.Secretkey = userEntity.Secretkey;
            operators.OrganizeId = userEntity.OrganizeId;
            operators.DeptId = userEntity.DepartmentId;
            operators.ParentId = userEntity.ParentId;
            operators.DeptCode = userEntity.DepartmentCode;
            operators.OrganizeCode = userEntity.OrganizeCode;
            operators.DeptName = userEntity.DeptName;
            operators.OrganizeName = userEntity.OrganizeName;
            operators.SpecialtyType = userEntity.SpecialtyType;
            DepartmentEntity dept = userBLL.GetUserOrgInfo(userEntity.UserId);//获取当前用户所属的机构
            operators.OrganizeId = dept.DepartmentId; //所属机构ID
            operators.OrganizeCode = dept.EnCode;//所属机构编码
            operators.NewDeptCode = dept.DeptCode;//所属机构新的编码（对应部门表中新加的编码字段deptcode）
            operators.OrganizeName = dept.FullName;//所属机构名称
            //公司级用户
            if (new UserBLL().HaveRoleListByKey(userEntity.UserId, dataitemdetailbll.GetItemValue("HidOrganize")).Rows.Count > 0)
            {
                operators.DeptId = userEntity.OrganizeId;
                operators.DeptCode = userEntity.OrganizeCode;
                operators.DeptName = userEntity.OrganizeName;
            }
            operators.PostName = userEntity.DutyName;
            operators.RoleName = userEntity.RoleName;
            operators.RoleId = userEntity.RoleId;
            operators.DutyName = userEntity.PostName;
            operators.DutyId = userEntity.DutyId; //岗位id
            operators.IPAddress = Net.Ip;
            operators.Photo = dataitemdetailbll.GetItemValue("imgUrl") + userEntity.HeadIcon;  //头像
            operators.IdentifyID = userEntity.IdentifyID; //身份证号码
            //operators.SendDeptID = userEntity.SendDeptID;
            //operators.IPAddressName = IPLocation.GetLocation(Net.Ip);
            operators.ObjectId = new PermissionBLL().GetObjectStr(userEntity.UserId);
            operators.LogTime = DateTime.Now;
            operators.Token = DESEncrypt.Encrypt(Guid.NewGuid().ToString());
            operators.IsTrainAdmin = userEntity.IsTrainAdmin;
            //写入当前用户数据权限
            AuthorizeDataModel dataAuthorize = new AuthorizeDataModel();
            dataAuthorize.ReadAutorize = authorizeBLL.GetDataAuthor(operators);
            dataAuthorize.ReadAutorizeUserId = authorizeBLL.GetDataAuthorUserId(operators);
            dataAuthorize.WriteAutorize = authorizeBLL.GetDataAuthor(operators, true);
            dataAuthorize.WriteAutorizeUserId = authorizeBLL.GetDataAuthorUserId(operators, true);
            operators.DataAuthorize = dataAuthorize;
            //判断是否系统管理员
            if (userEntity.Account == "System")
            {
                operators.IsSystem = true;
            }
            else
            {
                operators.IsSystem = false;
            }

            string userMode = "";

            string roleCode = dataitemdetailbll.GetItemValue("HidApprovalSetting");

            string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

            string[] pstr = HidApproval.Split('#');  //分隔机构组

            foreach (string strArgs in pstr)
            {
                string[] str = strArgs.Split('|');

                //当前机构相同，且为本部门安全管理员验证  第一种
                if (str[0].ToString() == userEntity.OrganizeId && str[1].ToString() == "0")
                {
                    int count = new UserBLL().GetUserListByRole(userEntity.DepartmentCode, roleCode, userEntity.OrganizeId).ToList().Where(p => p.UserId == userEntity.UserId).Count();
                    if (count > 0)
                    {
                        userMode = "0";
                    }
                    else
                    {
                        userMode = "1";
                    }

                    break;
                }
                if (str[0].ToString() == userEntity.OrganizeId && str[1].ToString() == "1")
                {
                    //获取指定部门的所有人员
                    int count = new UserBLL().GetUserListByDeptCode(str[2].ToString(), null, false, userEntity.OrganizeId).ToList().Where(p => p.UserId == userEntity.UserId).Count();
                    if (count > 0)
                    {
                        userMode = "2";
                    }
                    else
                    {
                        userMode = "3";
                    }
                    break;
                }
            }
            string rankArgs = dataitemdetailbll.GetItemValue("GeneralHid"); //一般隐患
            operators.rankArgs = rankArgs;
            operators.wfMode = userMode;

            //string hidPlantLevel = dataitemdetailbll.GetItemValue("HidPlantLevel");

            //string hidOrganize = dataitemdetailbll.GetItemValue("HidOrganize");

            //string CompanyRole = hidPlantLevel + "," + hidOrganize;

            //var userList = userBLL.GetUserListByDeptCode(userEntity.DepartmentCode, CompanyRole, false, userEntity.OrganizeId).Where(p => p.UserId == userEntity.UserId).ToList();

            //string isPlanLevel = "";
            ////当前用户是公司级及厂级用户
            //if (userList.Count() > 0)
            //{
            //    isPlanLevel = "1"; //厂级用户
            //}
            //else
            //{
            //    isPlanLevel = "0";  //非公司及厂级
            //}
            operators.isPlanLevel = "0";
            if (operators.RoleName.Contains("公司级") || operators.RoleName.Contains("厂级")) { operators.isPlanLevel = "1"; }

            string pricipalCode = dataitemdetailbll.GetItemValue("HidPrincipalSetting");
            IList<UserEntity> ulist = new UserBLL().GetUserListByRole(userEntity.DepartmentCode, pricipalCode, userEntity.OrganizeId).ToList();
            //返回的记录数,大于0，标识当前用户拥有部门负责人身份，反之则无
            int uModel = ulist.Where(p => p.UserId == userEntity.UserId).Count();
            operators.isPrincipal = uModel > 0 ? "1" : "0";
            var deptEntity = new DepartmentBLL().GetEntity(userEntity.DepartmentId);
            if (null != deptEntity)
            {
                operators.SendDeptID = deptEntity.SendDeptID;
            }
            else
            {
                operators.SendDeptID = "";
            }
            //用于违章的用户标记
            string mark = string.Empty;

            mark = userbll.GetSafetyAndDeviceDept(operators); //1 安全管理部门， 2 装置部门   5.发包部门

            string isPrincipal = userbll.HaveRoleListByKey(operators.UserId, dataitemdetailbll.GetItemValue("PrincipalUser")).Rows.Count > 0 ? "3" : ""; //第一级核准人
            if (!string.IsNullOrEmpty(isPrincipal))
            {
                if (!string.IsNullOrEmpty(mark))
                {
                    mark = mark + "," + isPrincipal;
                }
                else
                {
                    mark = isPrincipal;
                }
            }
            string isEpiboly = userbll.HaveRoleListByKey(operators.UserId, dataitemdetailbll.GetItemValue("EpibolyUser")).Rows.Count > 0 ? "4" : "";  //承包商

            if (!string.IsNullOrEmpty(isEpiboly))
            {
                if (!string.IsNullOrEmpty(mark))
                {
                    mark = mark + "," + isEpiboly;
                }
                else
                {
                    mark = isEpiboly;
                }
            }
            operators.uMark = mark;
            OperatorProvider.Provider.AddCurrent(operators);

            return operators;
        }
    }
}
