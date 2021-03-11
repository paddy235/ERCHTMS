using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.MessageManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using BSFramework.Cache.Factory;
using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.SignalR;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Linq.Expressions;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using System.Data.Common;
using ERCHTMS.Busines.OutsourcingProject;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.IO;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Entity.PersonManage;
using System.Net;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Busines.BaseManage
{
    /// <summary>
    /// 描 述：用户管理
    /// </summary>
    public class UserBLL
    {
        private IUserService service = new UserService();
        private IUserInfoService service1 = new UserInfoService();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL(); //部门操作对象
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL(); //外包工程管理
        /// <summary>
        /// 缓存key
        /// </summary>  
        public string cacheKey = BSFramework.Util.Config.GetValue("SoftName") + "_userCache";

        #region 获取数据
        /// <summary>
        ///获取培训人员记录(来自工具箱同步过来的数据)
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetTrainUsersPageList(Pagination pagination, string queryJson)
        {
            return service.GetTrainUsersPageList(pagination, queryJson);
        }
        /// <summary>
        /// 获取人员签名照并转换成base64字符串
        /// </summary>
        /// <returns></returns>
        public string GetSignContent(string signUrl)
        {
            if(string.IsNullOrWhiteSpace(signUrl))
            {
                return "";
            }
            string imgPath=HttpContext.Current.Server.MapPath("~/"+signUrl.Replace("~/", ""));
            if(System.IO.File.Exists(imgPath))
            {
                byte []bytes=System.IO.File.ReadAllBytes(imgPath);
                return Convert.ToBase64String(bytes);
            }
            else
            {
                return "";
            }
            
        }
        public IEnumerable<UserEntity> GetListForCon(Expression<Func<UserEntity, bool>> condition)
        {

            return service.GetListForCon(condition);
        }
        /// <summary>
        /// 上传个人签名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="signImg">图片名称</param>
        /// <returns></returns>
        public int UploadSignImg(string userId, string signImg,int mode=0)
        {
            return service.UploadSignImg(userId, signImg, mode);
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable(string deptId = "")
        {
            return service.GetTable();
        }
        #endregion

        #region 根据用户id获取用户列表
        /// <summary>
        /// 根据用户id获取用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetUserTable(string[] userids)
        {
            return service.GetUserTable(userids);
        }
        #endregion

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <returns></returns>
        public IList<UserInfoEntity> GetAllUserInfoList(string userids="") 
        {
            try
            {
                return service1.GetAllUserInfoList(userids);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 获取特殊用户集合
        /// </summary>
        /// <param name="accounts"></param>
        /// <returns></returns>
        public IList<UserInfoEntity> GetCurLevelAndHigherLevelUserByArgs(string accounts, string rolename) 
        {
            try
            {
                return service1.GetCurLevelAndHigherLevelUserByArgs(accounts, rolename);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 获取用户
        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="deptmentid"></param>
        /// <param name="natrue"></param>
        /// <param name="roleid"></param>
        /// <param name="useraccounts"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public IList<UserInfoEntity> GetWFUserListByDeptRoleOrg(string orgid, string deptmentid, string natrue, string roleid, string useraccounts, string rolename = "",string specialtytype = "")
        {
            try
            {
                return service1.GetWFUserListByDeptRoleOrg(orgid, deptmentid, natrue, roleid, useraccounts, rolename, specialtytype);
            }
            catch (Exception)
            {
                
                throw;
            }
        } 
        #endregion

        public DataTable GetMembers(string deptId)
        {
            return service.GetMembers(deptId);
        }

        /// <summary>
        /// 根据当前用户角色获取用户所属单位信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public DepartmentEntity GetUserOrgInfo(string userId)
        {
            return service.GetUserOrgInfo(userId);
        }
        public int ExcuteUser(string sql, DbParameter[] dbparams)
        {
            return service.ExcuteUser(sql, dbparams);
        }
        public DepartmentEntity GetUserOrganizeInfo(UserInfoEntity userEntity)
        {
            return service.GetUserOrganizeInfo(userEntity);
        }
        public int ExcuteBySql(string sql)
        {
            return service.ExcuteBySql(sql);
        }

        public IList<UserInfoEntity> GetUserListByAnyCondition(string orgid, string deptmentcode, string rolecode, string majorclassify = "") 
        {
            return service1.GetUserListByAnyCondition(orgid, deptmentcode, rolecode, majorclassify);
        }

        #region 用户列表
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserEntity> GetList()
        {
            return service.GetList();
        }
        #endregion
        /// <summary>
        /// 根据角色和部门获取用户账号和姓名
        /// 多个部门 多个角色 用,拼接
        /// </summary>
        /// <param name="orgid">厂级Id</param>
        /// <param name="deptid">部门Id</param>
        /// <param name="rolename">角色名称</param>
        /// <returns></returns>
        public DataTable GetUserAccountByRoleAndDept(string orgid, string deptid, string rolename)
        {
            return service.GetUserAccountByRoleAndDept(orgid, deptid, rolename);
        }
        public IList<UserEntity> GetUserListByRole(string deptmentid, string roleCode, string orgid)
        {
            return service.GetUserListByRole(deptmentid, roleCode, orgid);
        }


        public IList<UserEntity> GetUserListByRoleName(string deptId, string roleName, bool isSplit, string orgid)
        {
            return service.GetUserListByRoleName(deptId, roleName, isSplit, orgid);
        }

        public IList<UserEntity> GetUserListByDeptId(string deptId, string roleId, bool isSplit, string orgid)
        {
            return service.GetUserListByDeptId(deptId, roleId, isSplit, orgid);
        }
        #region 用户列表
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="pagination">分页</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        #endregion
        public IEnumerable<UserInfoEntity> GetUserList(Pagination pagination, string queryJson)
        {
            return service1.GetPageList(pagination, queryJson);
        }

        public IEnumerable<UserEntity> GetPagerUserList()
        {
            return service.GetUserList();
        }

        #region 用户列表（ALL）
        /// <summary>
        /// 用户列表（ALL）
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllTable()
        {
            return service.GetAllTable();
        }
        #endregion

        #region 用户实体
        /// <summary>
        /// 用户实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        #endregion

        #region 用户基本信息
        /// <summary
        /// 用户基本信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public UserInfoEntity GetUserInfoEntity(string keyValue)
        {
            return service1.GetUserInfoEntity(keyValue);
        }
        #endregion

        #region 获取用户是否存在某个角色
        /// <summary>
        /// 获取用户是否存在某个角色
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public DataTable HaveRoleListByKey(string keyValue, string rolename)
        {
            return service1.HaveRoleListByKey(keyValue, rolename);
        }
        #endregion

        public DataTable GetAllTableByArgs(string username, string deptid, string organizeid, string sjorgid, string reqmark, string threeperson = "")
        {
            return service.GetAllTableByArgs(username, deptid, organizeid, sjorgid, reqmark, threeperson);
        }

        #region 通过当前用户获取上级部门的安全管理员
        /// <summary>
        /// 通过当前用户获取上级部门的安全管理员
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<UserEntity> GetParentUserByCurrent(string userID, string userRoleCode)
        {
            return service.GetParentUserByCurrent(userID, userRoleCode);
        }
        #endregion


        #region 通过部门编码获取用户列表
        /// <summary>
        /// 通过部门编码获取用户列表
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="roleCode"></param>
        /// <param name="isSplit"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public IList<UserEntity> GetUserListByDeptCode(string deptCode, string roleCode, bool isSplit, string orgid)
        {
            return service.GetUserListByDeptCode(deptCode, roleCode, isSplit, orgid);
        }
        #endregion

        public DataTable GetAllTableByArgs(string argValue, string organizeid)
        {
            return service.GetAllTableByArgs(argValue, organizeid);
        }

        public IList<UserInfoEntity> GetUserInfoByDeptCode(string deptCode)
        {
            return service1.GetUserInfoByDeptCode(deptCode);
        }
        /// <summary>
        /// 根据部门编码或角色编码获取用户信息
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public IList<UserInfoEntity> GetUserListByCodeAndRole(string deptCode, string roleCode)
        {
            return service1.GetUserListByCodeAndRole(deptCode, roleCode);
        }

        public List<UserEntity> GetList(string[] users, int pageSize, int pageIndex, out int total)
        {
            return service.GetList(users, pageSize, pageIndex, out total);
        }

        /// <summary>
        /// 人员统计（集团）
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="newCode"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public List<string> GetStatByDeptCodeForGroup(string deptCode, string newCode, string deptType = "0")
        {
            return service.GetStatByDeptCodeForGroup(deptCode, newCode, deptType);
        }
        /// <summary>
        /// 根据人员持证率得分（特种作业人员和安全管理人员持证率）
        /// </summary>
        /// <param name="user"></param>
        /// <param name="time">月份，如2017-10-01</param>
        /// <returns></returns>
        public decimal GetIndexScoreByTime(ERCHTMS.Code.Operator user, string time = "")
        {
            return service1.GetIndexScoreByTime(user, time);
        }

  

        #region 通过电厂机构id获取对应的安全部门
        /// <summary>
        /// 通过电厂机构id获取对应的安全部门
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<DepartmentEntity> GetSafetyPersonByOrgId(string orgId)
        {
            List<DepartmentEntity> list = new List<DepartmentEntity>();

            List<string> ids = new List<string>();

            //安全部门
            DataItemModel safetymodel = dataitemdetailbll.GetDataItemListByItemCode("'SafetyDept'").Where(p => p.ItemName == orgId).ToList().FirstOrDefault();

            if (null != safetymodel)
            {
                string[] spitDept = safetymodel.ItemValue.Split('#');

                foreach (string s in spitDept)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        string tempDeptId = s.Split('|')[0].ToString(); //安全部门

                        ids.Add(tempDeptId);
                    }
                }
            }

            list = departmentbll.GetList().Where(p => ids.Contains(p.DepartmentId)).ToList();

            return list;
        }
        #endregion

        #region 根据用户ID和类别获取用户拥有的资源名称，多个用英文逗号分隔
        /// <summary>
        /// 根据用户ID和类别获取用户拥有的资源名称，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="category">数据类别，1:部门名称,2:角色名称,3:岗位名称,4:职位名称,5:工作组</param>
        /// <returns></returns>
        public string GetObjectName(string userId, int category)
        {
            return service.GetObjectName(userId, category);
        }
        #endregion

        #region 根据用户ID和类别获取用户拥有的资源Id，多个用英文逗号分隔
        /// <summary>
        /// 根据用户ID和类别获取用户拥有的资源Id，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="category">数据类别，1:部门名称,2:角色名称,3:岗位名称,4:职位名称,5:工作组</param>
        /// <returns></returns>
        public string GetObjectId(string userId, int category)
        {
            return service.GetObjectId(userId, category);
        }
        #endregion

        #region 获取用户拥有的角色名称，多个用英文逗号分隔
        /// <summary>
        /// 获取用户拥有的角色名称，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public string GetRoleName(string userId)
        {
            return service.GetRoleName(userId);
        }
        #endregion

        #region  获取用户拥有的角色Id，多个用英文逗号分隔
        /// <summary>
        /// 获取用户拥有的角色Id，多个用英文逗号分隔
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public string GetRoleId(string userId)
        {
            return service.GetRoleId(userId);
        }
        #endregion

        #region 获取用户账号获取用户信息
        /// <summary>
        /// 获取用户账号获取用户信息
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <returns></returns>
        /// <summary>
        public UserInfoEntity GetUserInfoByAccount(string account)
        {
            return service1.GetUserInfoByAccount(account);
        }
        /// <summary>
        /// 根据部门、用户名称获取用户信息
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <param name="userName">用户姓名</param>
        /// <returns></returns>
        public UserInfoEntity GetUserInfoByName(string deptName, string userName)
        {
            return service1.GetUserInfoByName(deptName, userName);
        }
        #endregion

        #region 身份证不能重复
        /// 身份证不能重复
        /// </summary>
        /// <param name="IdentifyID">身份证号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistIdentifyID(string IdentifyID, string keyValue)
        {
            return service.ExistIdentifyID(IdentifyID, keyValue);
        }
        #endregion

        #region 通过身份证号获取用户信息
        /// <summary>
        /// 通过身份证号获取用户信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public UserEntity GetUserByIdCard(string idCard)
        {
            return service.GetUserByIdCard(idCard);
        }
        /// <summary>
        /// 根据单位编码统计单位人员信息
        /// </summary>
        /// <param name="deptCode">单位编码</param>
        /// <returns></returns>
        public List<string> GetStatByDeptCode(string deptCode, string deptType = "")
        {
            return service.GetStatByDeptCode(deptCode, deptType);
        }
        #endregion

        #region 获取安全管理部门及装置部门的标记属性 (主要用于判断当前人是否能添加立即整改违章)
        /// <summary>  
        /// 获取安全管理部门及装置部门的标记属性  
        /// 注：装置部门 能新增 立即整改违章装置类违章 ，
        /// 安全部门 能新增 立即整改违章非装置类违章  其他用户无法新增   
        /// </summary>
        /// <param name="operators"></param>  
        /// <returns></returns> 1  安全管理部门用户  2 装置部门用户
        public string GetSafetyAndDeviceDept(Operator operators)
        {
            string userMark = string.Empty;
            string tempDeptID = string.Empty;


            //安全管理部门  存在多个安全部门 ，用"#"分隔  "|" 分隔部门相关的内容,  即结构是 "部门ID|部门编号|部门名称|部门人员 "
            DataItemModel safetymodel = dataitemdetailbll.GetDataItemListByItemCode("'SafetyDept'").Where(p => p.ItemName == operators.OrganizeId).ToList().FirstOrDefault();

            if (null != safetymodel)
            {
                string[] spitDept = safetymodel.ItemValue.Split('#');

                foreach (string s in spitDept)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        tempDeptID = s.Split('|')[0].ToString(); //当前部门

                        //比较当前人的部门
                        if (tempDeptID == operators.DeptId)
                        {
                            userMark += "1,";
                            break;
                        }
                    }
                }

            }
            //装置部门  只有一个
            DataItemModel devicemodel = dataitemdetailbll.GetDataItemListByItemCode("'DeviceDept'").Where(p => p.ItemName == operators.OrganizeId).ToList().FirstOrDefault();

            if (null != devicemodel)
            {
                //tempDeptID = devicemodel.ItemValue.Split('|')[0].ToString();

                ////比较当前人的部门
                //if (tempDeptID == operators.DeptId)
                //{
                //    if (!string.IsNullOrEmpty(userMark))
                //    {
                //        userMark += ",";
                //    }
                //    userMark += "2";
                //}
                string[] spitDept = devicemodel.ItemValue.Split('#');

                foreach (string s in spitDept)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        tempDeptID = s.Split('|')[0].ToString(); //当前部门

                        //比较当前人的部门
                        if (tempDeptID == operators.DeptId)
                        {
                            userMark += "2,";
                            break;
                        }
                    }
                }
            }

            ////获取发包部门
            //List<DepartmentEntity> dlist = departmentbll.GetList().Where(p =>p.SendDeptID == operators.DeptId).ToList();
            //if (dlist.Count() > 0) 
            //{
            //    userMark += "5";
            //}

            if (!string.IsNullOrEmpty(userMark))
            {
                userMark = userMark.Substring(0, userMark.Length - 1);
            }

            return userMark;
        }
        #endregion

        #region 获取违章流程中需要的对应用户账户
        /// <summary>
        /// 获取流程中需要的对应用户账户
        /// </summary>
        /// <param name="mark"></param>
        /// <param name="operators"></param>
        /// <returns></returns>
        public string GetSafetyDeviceDeptUser(string mark, Operator operators)
        {
            string result = string.Empty;

            string tempDept = string.Empty;

            List<UserEntity> ulist = new List<UserEntity>();

            try
            {
                switch (mark)
                {
                    //取安全管理部门
                    case "0":
                        DataItemModel safetymodel = dataitemdetailbll.GetDataItemListByItemCode("'SafetyDept'").Where(p => p.ItemName == operators.OrganizeId).ToList().FirstOrDefault();

                        if (null != safetymodel)
                        {
                            string[] spitDept = safetymodel.ItemValue.Split('#');

                            foreach (string s in spitDept)
                            {
                                if (!string.IsNullOrEmpty(s))
                                {
                                    tempDept += "'" + s.Split('|')[1].ToString() + "',"; //获取安全部门编码
                                }
                            }
                            if (!string.IsNullOrEmpty(tempDept))
                            {
                                tempDept = tempDept.Substring(0, tempDept.Length - 1); //安全部门编码
                            }

                            ulist = GetUserListByDeptCode(tempDept, null, false, operators.OrganizeId).ToList();

                            foreach (UserEntity entity in ulist)
                            {
                                result += entity.Account + ",";
                            }
                        }
                        break;
                    //取装置部门
                    case "1":
                        DataItemModel devicemodel = dataitemdetailbll.GetDataItemListByItemCode("'DeviceDept'").Where(p => p.ItemName == operators.OrganizeId).ToList().FirstOrDefault();

                        if (null != devicemodel)
                        {
                            tempDept = "'" + devicemodel.ItemValue.Split('|')[1].ToString() + "'"; //装置部门编码

                            ulist = GetUserListByDeptCode(tempDept, null, false, operators.OrganizeId).ToList();

                            foreach (UserEntity entity in ulist)
                            {
                                result += entity.Account + ",";
                            }
                        }
                        break;
                    //取当前人部门负责人
                    case "2":

                        tempDept = "'" + operators.DeptCode + "'";  //当前部门的编码

                        //获取负责人的对应值
                        string roleCode = dataitemdetailbll.GetItemValue("PrincipalUser");

                        ulist = GetUserListByDeptCode(tempDept, roleCode, false, operators.OrganizeId).ToList();

                        foreach (UserEntity entity in ulist)
                        {
                            result += entity.Account + ",";
                        }
                        break;
                    //取发包单位人员
                    case "3":

                        string fbRole = dataitemdetailbll.GetItemValue("DeptSafetyUser"); //安全管理员
                        //取发包部门
                        if (!string.IsNullOrEmpty(operators.ProjectID))
                        {
                            string engineerletdeptid = outsouringengineerbll.GetEntity(operators.ProjectID).ENGINEERLETDEPTID; //发包部门责任人 
                            if (!string.IsNullOrEmpty(engineerletdeptid))
                            {
                                string SendDeptCode = "'" + departmentbll.GetEntity(engineerletdeptid).EnCode + "'";

                                ulist = GetUserListByDeptCode(SendDeptCode, fbRole, false, operators.OrganizeId).ToList();

                                foreach (UserEntity entity in ulist)
                                {
                                    result += entity.Account + ",";
                                }
                            }
                        }
                        else
                        {
                            List<string> eperson = new List<string>();
                            //获取当前人下的所有外包工程
                            DataTable engineerdt = outsouringengineerbll.GetEngineerByCurrDept();
                            foreach (DataRow erow in engineerdt.Rows)
                            {
                                string engineerletdeptid = erow["engineerletdeptid"].ToString();
                                if (!string.IsNullOrEmpty(engineerletdeptid))
                                {
                                    string SendDeptCode = "'" + departmentbll.GetEntity(engineerletdeptid).EnCode + "'";

                                    ulist = GetUserListByDeptCode(SendDeptCode, fbRole, false, operators.OrganizeId).ToList();

                                    foreach (UserEntity entity in ulist)
                                    {
                                        if (!("," + result).Contains("," + entity.Account + ","))
                                        {
                                            result += entity.Account + ",";
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    //取当前的上一级部门级,安全员
                    case "4":
                        var superior = departmentbll.GetParentDeptBySpecialArgs(operators.ParentId, "部门"); //获取上一级部门级部门

                        if (null != superior)
                        {
                            tempDept = "'" + superior.EnCode + "'";  //当前部门的编码

                            //获取负责人的对应值
                            string DeptSafetyCode = dataitemdetailbll.GetItemValue("DeptSafetyUser");

                            ulist = GetUserListByDeptCode(tempDept, DeptSafetyCode, false, operators.OrganizeId).ToList();

                            foreach (UserEntity entity in ulist)
                            {
                                result += entity.Account + ",";
                            }
                        }
                        break;
                }
                // 结果
                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 获取流程中需要的对应用户账户
        /// </summary>
        /// <param name="mark"></param>
        /// <param name="OrganizeId">电厂编号</param>
        /// <returns></returns>
        public string GetSafetyDeviceDeptUser(string mark, string OrganizeId)
        {
            string result = string.Empty;

            string tempDept = string.Empty;

            List<UserEntity> ulist = new List<UserEntity>();

            try
            {
                switch (mark)
                {
                    //取安全管理部门
                    case "0":
                        DataItemModel safetymodel = dataitemdetailbll.GetDataItemListByItemCode("'SafetyDept'").Where(p => p.ItemName == OrganizeId).ToList().FirstOrDefault();

                        if (null != safetymodel)
                        {
                            string[] spitDept = safetymodel.ItemValue.Split('#');

                            foreach (string s in spitDept)
                            {
                                if (!string.IsNullOrEmpty(s))
                                {
                                    tempDept += "'" + s.Split('|')[1].ToString() + "',"; //获取安全部门编码
                                }
                            }
                            if (!string.IsNullOrEmpty(tempDept))
                            {
                                tempDept = tempDept.Substring(0, tempDept.Length - 1); //安全部门编码
                            }

                            ulist = GetUserListByDeptCode(tempDept, null, false, OrganizeId).ToList();

                            foreach (UserEntity entity in ulist)
                            {
                                result += entity.Account + ",";
                            }
                        }
                        break;
                    //取装置部门
                    case "1":
                        DataItemModel devicemodel = dataitemdetailbll.GetDataItemListByItemCode("'DeviceDept'").Where(p => p.ItemName == OrganizeId).ToList().FirstOrDefault();

                        if (null != devicemodel)
                        {
                            tempDept = "'" + devicemodel.ItemValue.Split('|')[1].ToString() + "'"; //装置部门编码

                            ulist = GetUserListByDeptCode(tempDept, null, false, OrganizeId).ToList();

                            foreach (UserEntity entity in ulist)
                            {
                                result += entity.Account + ",";
                            }
                        }
                        break;
                }
                // 结果
                if (!string.IsNullOrEmpty(result))
                {
                    result = result.Substring(0, result.Length - 1);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 账户不能重复
        /// </summary>
        /// <param name="account">账户值</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistAccount(string account,  string keyValue = "", string encode = "", string mobile = "")
        {
            return service.ExistAccount(account, encode, mobile, keyValue);
        }
        /// <summary>
        /// 校验手机号是否重复，没重复返回true
        /// </summary>
        /// <param name="mobile">要校验的手机号</param>
        /// <param name="keyValue">主键</param>
        /// <returns></returns>
        public bool ExistMoblie(string mobile, string keyValue = "")
        {
            return service.ExistMoblie(mobile, keyValue);
        }
        /// <summary>
        /// 验证是否属于黑名单用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string IsBalckUser(string userId,string idCard)
        {
            string res = "";
            UserEntity user=null;
            if (string.IsNullOrWhiteSpace(userId))
            {
                user = GetUserByIdCard(idCard);
                if (user != null)
                {
                    var list = new BlacklistBLL().GetList(user.UserId);
                    if (list.Count() > 0)
                    {
                        list = list.Where(t => t.EnableMark == 0).OrderByDescending(t => t.JoinTime);
                        res = list.FirstOrDefault().JoinTime.ToString();
                    }
                }
            }
            else
            {
                user = GetEntity(userId);
                if (user == null)
                {
                    user = GetUserByIdCard(idCard);
                    if (user != null)
                    {
                        var list = new BlacklistBLL().GetList(user.UserId);
                        if (list.Count() > 0)
                        {
                            list = list.Where(t => t.EnableMark == 0).OrderByDescending(t => t.JoinTime);
                            res = list.FirstOrDefault().JoinTime.ToString();
                        }
                    }
                }
            }
           
            return res;
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
                CacheFactory.Cache().RemoveCache(cacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 验证用户手机号和工号是否存在
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="user"></param>
        public bool ValidateUser(string keyValue, UserEntity user)
        {
            string sql = "1=1";
            if(!string.IsNullOrWhiteSpace(keyValue))
            {
                sql += " and userid!='"+keyValue+"'";  
            }
            if (!string.IsNullOrWhiteSpace(user.UserId))
            {
                UserEntity ue = GetEntity(user.UserId);
                if(ue!=null)
                {
                    sql += " and userid!='" + keyValue + "'";
                }
            }
            if (!string.IsNullOrWhiteSpace(user.Account))
            {
                DataTable dtUser = departmentbll.GetDataTable(string.Format("select count(1) from base_user where ({1}) and (encode='{0}'  or account='{0}' or mobile='{0}')", user.Account.Trim(), sql));
                if (dtUser.Rows[0][0].ToString() != "0")
                {
                    return false;
                    //throw new Exception("人员编号/工号已存在!");
                }
            }
            if (!string.IsNullOrWhiteSpace(user.EnCode))
            {
                DataTable dtUser = departmentbll.GetDataTable(string.Format("select count(1) from base_user where ({1}) and (encode='{0}'  or account='{0}' or mobile='{0}')", user.EnCode.Trim(),sql));
                if(dtUser.Rows[0][0].ToString()!="0")
                {
                    return false ;
                    //throw new Exception("人员编号/工号已存在!");
                }
            }
            if (!string.IsNullOrWhiteSpace(user.Mobile))
            {
                DataTable dtUser = departmentbll.GetDataTable(string.Format("select count(1) from base_user where ({1})  and (mobile='{0}' or account='{0}' or encode='{0}')", user.Mobile.Trim(), sql));
                if (dtUser.Rows[0][0].ToString() != "0")
                {
                    return false;
                    //throw new Exception("该手机号已存在!");
                }
            }
            return true;
        }
        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        public string SaveForm(string keyValue, UserEntity userEntity,int mode=0)
        {
            try
            {
                bool result=ValidateUser(keyValue, userEntity);
                if(result)
                {
                    keyValue = service.SaveForm(keyValue, userEntity, mode);
                    return keyValue;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>
        /// 修改用户基本信息
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        public string UpdateUserInfo(string keyValue, UserEntity userEntity)
        {
            try
            {
                service.UpdateEntity(userEntity);
                return keyValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="Password">新密码（MD5 小写）</param>
        public bool RevisePassword(string keyValue, string Password)
        {
           return service.RevisePassword(keyValue, Password);
        }
        /// <summary>
        /// 密码修改记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool RecordPassword(string userId,string password)
        {
            try
            {
                string sql = string.Format("insert into BASE_PASSWORDHISTORY(id,userid,password,time) values('{0}','{1}','{2}',sysdate)", Guid.NewGuid().ToString(), userId, password);
                int count = departmentbll.ExecuteSql(sql);
                return count > 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="State">状态：1-启动；0-禁用</param>
        public void UpdateState(string keyValue, int State)
        {
            try
            {
                service.UpdateState(keyValue, State);
                CacheFactory.Cache().RemoveCache(cacheKey);
                if (State == 0)
                {
                    UpdateIMUserList(keyValue, false, null);
                }
                else
                {
                    UserEntity entity = service.GetEntity(keyValue);
                    UpdateIMUserList(keyValue, true, entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public UserInfoEntity CheckUserLogin(string username)
        {
            return service.CheckUserLogin(username);
        }

        public bool ValidateUser(string username, string password)
        {
            UserInfoEntity userEntity = service.CheckUserLogin(username);
            if (userEntity.EnabledMark == 1)
            {
                string dbPassword = Md5Helper.MD5(DESEncrypt.Encrypt(password.ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();
                if (dbPassword == userEntity.Password)
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
            else
            {

                return true;
            }    
        }
        #region sha256加密
        /// <summary>
        /// sha256
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string sha256(string data)
        {
            //string digest = SHA256Encrypt(data);
            //byte[] b = System.Text.Encoding.Default.GetBytes(digest);
            //return Convert.ToBase64String(b);


            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }
            return builder.ToString();
        }

        public string SHA256Encrypt(string strIN)
        {
            //string strIN = getstrIN(strIN);
            byte[] tmpByte;
            SHA256 sha256 = new SHA256Managed();

            tmpByte = sha256.ComputeHash(GetKeyByteArray(strIN));
            sha256.Clear();

            return GetStringValue(tmpByte);

        }

        private string GetStringValue(byte[] Byte)
        {
            string tmpString = "";
            ASCIIEncoding Asc = new ASCIIEncoding();
            tmpString = Asc.GetString(Byte);
            return tmpString;
        }

        private byte[] GetKeyByteArray(string strKey)
        {
            ASCIIEncoding Asc = new ASCIIEncoding();

            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];

            tmpByte = Asc.GetBytes(strKey);

            return tmpByte;

        }


        #endregion
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UserInfoEntity CheckLogin(string username, string password, string shapassword="")
        {
            UserInfoEntity userEntity = service.CheckUserLogin(username);
            if (userEntity != null)
            {
                if (userEntity.EnabledMark == 1)
                {
                    if (!string.IsNullOrWhiteSpace(shapassword))
                    {
                        shapassword = sha256("CRCLDAP+" + username.ToUpper() + "+" + shapassword.ToUpper());
                    }
                    string dbPassword = Md5Helper.MD5(DESEncrypt.Encrypt(password.ToLower(), userEntity.Secretkey).ToLower(), 32).ToLower();

                    if (dbPassword == userEntity.Password || shapassword==userEntity.Password)
                    {
                        Operator operUser = new Operator
                        {
                            OrganizeCode = userEntity.OrganizeCode
                        };
                        ERCHTMS.Busines.SystemManage.PasswordSetBLL psBll = new PasswordSetBLL();
                        List<string> lst = psBll.IsPasswordRuleStatus(operUser);
                        System.Threading.Tasks.Task.Factory.StartNew(()=>{
                    
                         DateTime LastVisit = DateTime.Now;
                         UserEntity user =  GetEntity(userEntity.UserId);
                         int LogOnCount = (user.LogOnCount).ToInt() + 1;
                         if (user.LastVisit != null)
                         {
                            user.PreviousVisit = user.LastVisit.ToDate();
                           
                         }
                         if (lst[0] == "true" && user.LastVisit!=null)
                         {
                             user.LastVisit = LastVisit;
                         }
                         user.PwdErrorCount = 0;
                         user.LogOnCount = LogOnCount;
                         user.UserOnLine = 1;
                         service.UpdateEntity(user);
                       });
                        
                        return userEntity;
                    }
                    else
                    {
                        PasswordSetEntity ps = new PasswordSetBLL().GetList(userEntity.OrganizeCode).FirstOrDefault() ;
                        if (ps!=null)
                        {
                            if (userEntity.Account.ToLower() != "system" && ps.Num>0)
                            {
                                UserEntity user = GetEntity(userEntity.UserId);
                                user.PwdErrorCount += 1;
                                if (user.PwdErrorCount >= ps.Num)
                                {
                                    user.EnabledMark = 0;
                                }
                                service.UpdateEntity(user);
                            }
                        }
                        throw new Exception("账号或密码错误");
                    }
                }
                else
                {
                    
                    throw new Exception("账号已被禁用,请联系管理员");
                }
            }
            else
            {
                throw new Exception("账号或密码错误");
            }
        }
        /// <summary>
        /// 更新实时通信用户列表
        /// </summary>
        private void UpdateIMUserList(string keyValue, bool isAdd, UserEntity userEntity)
        {
            try
            {
                IMUserModel entity = new IMUserModel();
                OrganizeBLL bll = new OrganizeBLL();
                DepartmentBLL dbll = new DepartmentBLL();
                entity.UserId = keyValue;
                if (userEntity != null)
                {
                    entity.RealName = userEntity.RealName;
                    entity.DepartmentId = dbll.GetEntity(userEntity.DepartmentId).FullName;
                    entity.Gender = userEntity.Gender;
                    entity.HeadIcon = userEntity.HeadIcon;
                    entity.OrganizeId = bll.GetEntity(userEntity.OrganizeId).FullName; ;
                }
                SendHubs.callMethod("upDateUserList", entity, isAdd);
            }
            catch
            {

            }
        }
        #endregion

        #region 处理数据

        /// <summary>
        /// 同步外包工程人员到双控平台用户
        /// </summary>
        /// <param name="projectId">工程Id</param>
        /// <param name="deptId">外包单位Id</param>
        /// <returns></returns>
        public bool SyncUsers(string projectId, string deptId)
        {
            return service.SyncUsers(projectId, deptId);
        }
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        public void GetExportList(string condition, string keyword, string code)
        {
            //取出数据源
            DataTable exportTable = service.GetExportList(condition, keyword, code);
            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "用户信息";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "用户导出.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "account", ExcelColumn = "账户" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "realname", ExcelColumn = "姓名" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "gender", ExcelColumn = "性别" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "birthday", ExcelColumn = "生日" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "mobile", ExcelColumn = "手机", Background = Color.Red });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "telephone", ExcelColumn = "电话", Background = Color.Red });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "wechat", ExcelColumn = "微信" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "manager", ExcelColumn = "主管" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "organize", ExcelColumn = "公司" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "department", ExcelColumn = "部门" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "description", ExcelColumn = "说明" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "创建日期" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createusername", ExcelColumn = "创建人" });
            //调用导出方法
            ExcelHelper.ExcelDownload(exportTable, excelconfig);
            //从泛型Lis导出
            //TExcelHelper<DepartmentEntity>.ExcelDownload(department.GetList().ToList(), excelconfig);
        }

        /// <summary>
        /// 设置用户黑名单状态
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="status">状态值（0:不是黑名单，1：是黑名单）</param>
        /// <returns></returns>
        public int SetBlack(string userId, int status = 0)
        {
            return service.SetBlack(userId, status);
        }
        /// <summary>
        /// 人员离场
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="leaveTime">离场时间</param>
        /// <returns></returns>
        public int SetLeave(string userId, string leaveTime, string DepartureReason)
        {
            return service.SetLeave(userId, leaveTime, DepartureReason);
        }

        /// <summary>
        /// 同步用户信息到培训平台
        /// </summary>
        /// <param name="user">用户基本信息</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public string SyncUser(UserEntity user, string pwd, ERCHTMS.Code.Operator currUser = null)
        {
            return service1.SyncUser(user, pwd, currUser);
        }
        #endregion

        /// <summary>
        /// 获取人员来自培训平台培训档案
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public string GetTrainRecord(string userId,string userAccount, string deptId,string idCard="")
        {
            return service1.GetTrainRecord(userId,userAccount, deptId, idCard);
        }

        public string GetTrainRecord(string userId,out string error)
        {
            var user = GetEntity(userId);
            DataItemDetailBLL di = new DataItemDetailBLL();
            string val = di.GetItemValue("TrainSyncWay");//对接方式，0：账号，1：身份证,不配置默认为账号
            string way = di.GetItemValue("WhatWay");//对接平台 0：.net培训平台 1:java培训平台
            if (way == "1")
            {
                string account = string.IsNullOrWhiteSpace(user.NewAccount)?user.Account:user.NewAccount;
                string fileName = "Train_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                try
                {
                    WebClient wc = new WebClient();
                    wc.Credentials = CredentialCache.DefaultCredentials;
                    wc.Headers.Add("Content-Type", "application/json; charset=utf-8");
                    //发送请求到web api并获取返回值，默认为post方式
                    string url = new DataItemDetailBLL().GetItemValue("TrainServiceUrl");
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(new { user_account = account });
                    string result = wc.UploadString(new Uri(url), json);
                    System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ：获取人员培训档案,远程服务器返回信息：" + result + "\r\n");
                    dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Dynamic.ExpandoObject>(result);
                    error = "";
                    return Newtonsoft.Json.JsonConvert.SerializeObject(dy.data);
                }
                catch (Exception ex)
                {
                    System.IO.File.AppendAllText(System.Web.HttpContext.Current.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ：获取人员培训档案异常,信息：" + ex.Message + "\r\n");
                    error = ex.Message;
                    return "" ;
                }


            }
            else
            {
                string idCard = "";
                string account = user.Account;

                if (!string.IsNullOrWhiteSpace(val))
                {
                    if (val == "0")
                    {
                        idCard = "";
                    }
                    else
                    {
                        account = "";
                        idCard = user.IdentifyID;
                    }
                }
                var json = GetTrainRecord(userId, account, "", idCard);
                if (json.Length > 1)
                {
                    var dt = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json.Replace("{\"Traindata\":", "").TrimEnd('}'));
                    dt.Columns.Add("ISEXAM");
                    dt.Columns.Add("LINE");
                    dt.Columns.Add("exam_score");
                    json = GetExamRecord(userId, account, "", idCard);
                    if (json.Length > 30)
                    {
                        var dtExams = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(json.Replace("{\"Examdata\":", "").TrimEnd('}'));
                        foreach (DataRow dr in dt.Rows)
                        {
                            var rows = dtExams.Select("TRAINPLANCODE='" + dr["TRAINPRJID"].ToString() + "'");
                            if (rows.Count() > 0)
                            {
                                if (string.IsNullOrEmpty(rows[0]["POINT"].ToString()))
                                {
                                    dr["ISEXAM"] = "否";
                                }
                                else
                                {
                                    dr["ISEXAM"] = "是";
                                    dr["exam_score"] = rows[0]["POINT"];
                                }
                                dr["LINE"] = rows[0]["PASSLINE"];
                            }
                            else
                            {
                                dr["ISEXAM"] = "否";
                            }
                        }
                    }
                    error ="";
                    return dt.ToJson();
                }
                else
                {
                    error = "获取数据发生错误";
                    return "";
                }
            }
        }
        /// <summary>
        /// 获取人员来自培训平台的考试记录
        /// </summary>
        /// <param name="userAccount"></param>
        /// <returns></returns>
        public string GetExamRecord(string userId, string userAccount, string deptId, string idCard = "")
        {
            return service1.GetExamRecord(userId,userAccount, deptId, idCard);
        }
        /// <summary>
        /// 人员转岗
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="newDeptId">新部门Id</param>
        /// <param name="newPostId">新的岗位Id</param>
        /// <param name="newPostName">新的岗位名称</param>
        /// <param name="newDutyId">新的职务Id</param>
        /// <param name="newDutyName">新的职务名称</param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int LeavePost(string userId, string newDeptId, string newPostId, string newPostName, string newDutyId, string newDutyName, string time)
        {
            return service.LeavePost(userId, newDeptId, newPostId, newPostName, newDutyId, newDutyName, time);
        }


        /// <summary>
        /// 保存用户表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="userEntity">用户实体</param>
        /// <returns></returns>
        public string SaveOnlyForm(string keyValue, UserEntity userEntity)
        {
            return service.SaveOnlyForm(keyValue, userEntity);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="deptcode"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public DataTable GetUserByDeptCodeAndRoleName(string userid, string deptcode, string rolename) 
        {
            return service.GetUserByDeptCodeAndRoleName(userid, deptcode,rolename);
        }

        public IEnumerable<UserEntity> FindList(string strSql, DbParameter[] dbParameter)
        {
            return service.FindList(strSql, dbParameter);
        }
        /// <summary>
        /// 批量同步用户到班组
        /// </summary>
        /// <param name="userList"></param>
        /// <param name="mode">0:同步,1:异步</param>
        public string SyncUsersToBZ(IList<UserEntity> userList,int mode=0)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Type", "application/json");
            //发送请求到web api并获取返回值，默认为post方式
            try
            {
                DataItemDetailBLL dd = new DataItemDetailBLL();
                string imgUrl = dd.GetItemValue("imgUrl");
                foreach (UserEntity item in userList)
                {
                    //用户信息
                    item.Gender = item.Gender == "男" ? "1" : "0";
                    if (item.RoleName.Contains("班组级用户"))
                    {
                        if (item.RoleName.Contains("负责人"))
                        {
                            item.RoleId = "a1b68f78-ec97-47e0-b433-2ec4a5368f72";
                            item.RoleName = "班组长";
                        }
                        else
                        {
                            item.RoleId = "e503d929-daa6-472d-bb03-42533a11f9c6";
                            item.RoleName = "班组成员";
                        }
                    }
                    if (item.RoleName.Contains("部门级用户"))
                    {
                        if (item.RoleName.Contains("负责人"))
                        {
                            item.RoleId = "1266af38-9c0a-4eca-a04a-9829bc2ee92d";
                            item.RoleName = "部门管理员";
                        }
                        else
                        {
                            item.RoleId = "3a4b56ac-6207-429d-ac07-28ab49dca4a6";
                            item.RoleName = "部门级用户";
                        }
                    }
                    if (item.RoleName.Contains("公司级用户"))
                    {
                        //if (user.RoleName.Contains("负责人"))
                        //{
                        item.RoleId = "97869267-e5eb-4f20-89bd-61e7202c4ecd";
                        item.RoleName = "厂级管理员";
                        // }

                    }
                    if (item.EnterTime == null)
                    {
                        item.EnterTime = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(item.SignImg))
                    {
                        item.SignImg = imgUrl + item.SignImg;
                    }
                    if (!string.IsNullOrEmpty(item.HeadIcon))
                    {
                        item.HeadIcon = imgUrl + item.HeadIcon;
                    }
                    if (!string.IsNullOrWhiteSpace(item.Password))
                    {
                        if (item.Password.Contains("***"))
                        {
                            item.Password = null;
                        }

                    }
                    item.Gender = item.Gender == "男" ? "1" : "0";
                }
                if(mode==0)
                {
                    wc.UploadStringCompleted += wc_UploadStringCompleted;
                    wc.UploadStringAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "PostEmployees"), "post", userList.ToJson());
                }
                else
                {
                    wc.UploadString(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "PostEmployees?NeedEncrypt=false"), "post", userList.ToJson());
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                //将同步结果写入日志文
                string logPath = AppDomain.CurrentDomain.BaseDirectory + "logs\\syncbz\\";
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(logPath+fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户失败，同步信息" + Newtonsoft.Json.JsonConvert.SerializeObject(userList) + ",异常信息：" + ex.Message + "\r\n");
                return ex.Message;
            }
        }
        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var error = e.Error;
            //将同步结果写入日志文件
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "logs\\syncbz\\";
            string fileName = "user_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!System.IO.Directory.Exists(logPath))
            {
                System.IO.Directory.CreateDirectory(logPath);
            }
            try
            {

                System.IO.File.AppendAllText(logPath + fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户结果>" + e.Result + "\r\n");
            }
            catch (Exception ex)
            {
                string msg = error == null ? ex.Message : error.Message;
                System.IO.File.AppendAllText(logPath+fileName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：同步用户结果>" + msg + "\r\n");
            }

        }

        public UserEntity GetUserInfoByUserName(string username)
        {
            return service.GetUserInfoByUserName(username);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public IEnumerable<UserInfoEntity> GetUserListBySql(string strSql)
        {
           return service1.GetUserListBySql(strSql);
        }
    }

}
