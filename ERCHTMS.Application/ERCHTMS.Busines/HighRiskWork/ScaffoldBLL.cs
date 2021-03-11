using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// 描 述：1.脚手架搭设、验收、拆除申请2.脚手架搭设、验收、拆除审批
    /// </summary>
    public class ScaffoldBLL
    {
        private ScaffoldIService service = new ScaffoldService();
        private PeopleReviewIService peopleReviwservice = new PeopleReviewService();
        private IRoleService rolesevice = new RoleService();
        private ScaffoldauditrecordIService scaffoldauditrecordservice = new ScaffoldauditrecordService();
        private IDepartmentService departmentservice = new DepartmentService();
        private IManyPowerCheckService powerCheck = new ManyPowerCheckService();
        private IUserService userservice = new UserService();
        private IDataItemDetailService dataitemdetailservice = new DataItemDetailService();
        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private SafetychangeBLL safetychangebll = new SafetychangeBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private FireWaterBLL firewaterbll = new FireWaterBLL();

        #region 获取数据

        /// <summary>
        /// 得到当前最大编号
        /// 编号规则：类型首字母+年份+3位数（如J2018001、J2018002）
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            return service.GetMaxCode();

        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetList(Pagination page, string queryJson)
        {
            return service.GetList(page, queryJson);
        }

        /// <summary>
        /// 台账列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLedgerList(Pagination page, string queryJson, string authType)
        {
            return service.GetLedgerList(page, queryJson, authType);
        }

        /// <summary>
        /// 获取部门下的专业类别
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public List<itemClass> GetSpecialtyToJson(string deptid, string specialtytype, string workdepttype)
        {
            List<itemClass> list = new List<itemClass>();
            if (!string.IsNullOrEmpty(specialtytype))
            {
                itemClass itementity = new itemClass();
                var strtype = getName(!string.IsNullOrEmpty(specialtytype) ? specialtytype : "", "SpecialtyType");
                itementity.itemvalue = specialtytype;
                itementity.itemname = strtype;
                list.Add(itementity);
            }
            else
            {
                IList<DepartmentEntity> dept = new List<DepartmentEntity>();
                if (workdepttype == "1" && departmentbll.GetEntity(deptid) == null)//外包单位
                {
                    dept.Add(departmentbll.GetEntity(new OutsouringengineerBLL().GetEntity(deptid).ENGINEERLETDEPTID));
                }
                else
                {

                    DepartmentEntity department = departmentbll.GetEntity(deptid);
                    dept.Add(department);
                    while (department.Nature != "部门")
                    {
                        department = departmentbll.GetEntity(department.ParentId);
                        dept.Add(department);
                    }
                }
                if (dept.Count>0)
                {
                    string departmentid = "";
                    foreach (var item in dept)
                    {
                        departmentid += item.DepartmentId + ",";
                    }
                    if (!string.IsNullOrEmpty(departmentid))
                    {
                        departmentid = departmentid.Substring(0, departmentid.Length - 1);
                    }
                    IList<UserEntity> users = userservice.GetUserListByDeptId("'" + departmentid.Replace(",","','") + "'", "", false, string.Empty).OrderBy(t => t.RealName).ToList();
                    if (users != null && users.Count > 0)
                    {
                        foreach (var item in users)
                        {
                            if (!string.IsNullOrEmpty(item.SpecialtyType) && item.SpecialtyType != "null")
                            {
                                string[] str = item.SpecialtyType.Split(',');
                                for (int i = 0; i < str.Length; i++)
                                {
                                    var slist = list.Where(x => x.itemvalue.Equals(str[i])).ToList();
                                    if (slist.Count <= 0)
                                    {
                                        itemClass itementity = new itemClass();
                                        var strtype =getName(str[i], "SpecialtyType");
                                        if (!string.IsNullOrEmpty(strtype))
                                        {
                                            itementity.itemvalue = str[i];
                                            itementity.itemname = strtype;
                                            if (!list.Contains(itementity))
                                            {
                                                list.Add(itementity);
                                            } 
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 得到流程图
        /// </summary>
        /// <param name="keyValue">业务表ID</param>
        /// <param name="modulename">逐级审核模块名</param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue, string modulename)
        {
            return service.GetFlow(keyValue, modulename);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ScaffoldEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// 获取脚手架搭设和拆除
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetSelectPageList(Pagination pagination, string queryJson)
        {
            return service.GetSelectPageList(pagination, queryJson);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ScaffoldEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// 适用于保存及修改时，处理业务数据及初始状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="model">实体对象</param>
        /// <param name="auditEntity">审核实体对象</param>
        /// <returns></returns> 
        public void SaveForm(string keyValue, ScaffoldModel model)
        {
            try
            {
                List<RoleEntity> roles = (List<RoleEntity>)rolesevice.GetList();
                Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                ManyPowerCheckEntity mpcEntity = null;
                //没有审核节点时，默认审核的角色部门为当前用户
                model.FlowDeptId = currUser.DeptId;
                model.FlowDeptName = currUser.DeptName;
                model.FlowRoleId = currUser.RoleId;
                model.FlowRoleName = currUser.RoleName;
                string moduleName = "";
                switch (model.ScaffoldType)
                {
                    case 0:

                        #region 搭设申请审核
                        //审核同意，只处理6米以上的
                        if (model.SetupCompanyType == 0)
                        {
                            moduleName = "(搭设申请-内部-" + model.SetupTypeName + ")审核";
                        }
                        else
                        {
                            moduleName = "(搭设申请-外包-" + model.SetupTypeName + ")审核";
                        }
                        //if (model.SetupType == 1)
                        //{
                        //    if (model.SetupCompanyType == 0)
                        //    {
                        //        moduleName = "(搭设申请-内部-6米以上)审核";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(搭设申请-外包-6米以上)审核";
                        //    }
                        //}
                        //else
                        //{
                        //    if (model.SetupCompanyType == 0)
                        //    {
                        //        moduleName = "(搭设申请-内部-6米以下)审核";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(搭设申请-外包-6米以下)审核";
                        //    }
                        //}
                        mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, model.FlowId);
                        if (model.AuditState == 0)
                        {
                            model.FlowName = "申请中";
                            model.InvestigateState = 0;
                        }
                        if (model.AuditState == 1)
                        {
                            //如果审核步骤不为空，处理流程信息及状态
                            if (mpcEntity != null)
                            {
                                model.FlowDeptId = mpcEntity.CHECKDEPTID;
                                model.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                                model.FlowRoleId = mpcEntity.CHECKROLEID;
                                model.FlowRoleName = mpcEntity.CHECKROLENAME;
                                model.FlowId = mpcEntity.ID;
                                model.FlowName = mpcEntity.FLOWNAME;
                                model.AuditState = 1;
                                model.InvestigateState = 2;
                                model.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                                

                            }
                            else
                            {
                                model.FlowRemark = "";
                                model.FlowId = "";
                                model.FlowDeptId = "";
                                model.FlowDeptName = "";
                                model.FlowRoleId = "";
                                model.FlowRoleName = "";
                                model.AuditState = 3;
                                model.FlowName = "已完结";
                                model.InvestigateState = 3;
                                
                                string Content = "脚手架搭设类型：" + model.SetupTypeName + "&#10;搭设时间：" + model.SetupStartDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + model.SetupEndDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;搭设地点：" + model.SetupAddress;
                                UserEntity userEntity = userservice.GetEntity(model.ApplyUserId);
                                string[] workuserlist = (model.SetupChargePerson + "," + model.SetupPersons).Split(',');
                                DataTable dutyuserDt = new DataTable();
                                dutyuserDt = userservice.GetUserTable(workuserlist);
                                //推送给作业申请人
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY021", "脚手架搭设申请已通过，请及时处理", Content, keyValue);
                                }
                                //推送给作业负责人/作业人
                                if (dutyuserDt.Rows.Count > 0)
                                {
                                    string Account = "";
                                    string RealName = "";
                                    foreach (DataRow item in dutyuserDt.Rows)
                                    {
                                        Account += item["account"].ToString() + ",";
                                        RealName += item["realname"].ToString() + ",";
                                    }
                                    if (!string.IsNullOrEmpty(Account))
                                    {
                                        Account = Account.Substring(0, Account.Length - 1);
                                        RealName = RealName.Substring(0, RealName.Length - 1);
                                    }
                                    JPushApi.PushMessage(Account, RealName, "ZY021", "您有一条新的脚手架搭设作业任务，请及时处理", Content, keyValue);
                                }
                            }
                        }
                        #endregion

                        break;
                    case 1:

                        #region 验收申请审核
                        if (model.SetupCompanyType == 0)
                        {

                            moduleName = "(搭设验收-内部-" + model.SetupTypeName + ")审核";
                        }
                        else
                        {
                            moduleName = "(搭设验收-外包-" + model.SetupTypeName + ")审核";
                        }
                        //if (model.SetupCompanyType == 0)
                        //{
                        //    if (model.SetupType == 0)
                        //    {

                        //        moduleName = "(搭设验收-内部-6米以下)审核";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(搭设验收-内部-6米以上)审核";
                        //    }
                        //}
                        //else
                        //{
                        //    if (model.SetupType == 0)
                        //    {
                        //        moduleName = "(搭设验收-外包-6米以下)审核";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(搭设验收-外包-6米以上)审核";
                        //    }

                        //}
                        mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, model.FlowId);
                        if (model.AuditState == 0)
                        {
                            model.FlowName = "申请中";
                            model.InvestigateState = 0;
                        }
                        if (model.AuditState == 1)
                        {
                            //单位内部的话，第一步就是验收，所以状态改为验收中
                            
                            model.InvestigateState = 2;

                            if (mpcEntity != null)
                            {
                                model.FlowDeptId = mpcEntity.CHECKDEPTID;
                                model.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                                model.FlowRoleId = mpcEntity.CHECKROLEID;
                                model.FlowRoleName = mpcEntity.CHECKROLENAME;
                                model.FlowId = mpcEntity.ID;
                                model.FlowName = mpcEntity.FLOWNAME;
                                model.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                                if (model.FlowName.Contains("验收确认"))
                                {
                                    model.AuditState = 4;
                                }
                                else
                                {
                                    model.AuditState = 1;
                                }
                            }
                            else
                            {
                                model.FlowRemark = "";
                                model.AuditState = 3;
                                model.FlowName = "已完结";
                                model.FlowDeptId = "";
                                model.FlowDeptName = "";
                                model.FlowRoleId = "";
                                model.FlowRoleName = "";
                                model.FlowId = "";
                                model.InvestigateState = 3;
                                //更改实际搭设时间
                                var buildentity = this.GetEntity(model.SetupInfoId);
                                buildentity.ActSetupStartDate = model.ActSetupStartDate;
                                buildentity.ActSetupEndDate = model.ActSetupEndDate;
                                service.SaveForm(model.SetupInfoId, buildentity);
                                
                                string Content = "脚手架验收类型：" + model.SetupTypeName + "&#10;验收时间：" + model.SetupStartDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + model.SetupEndDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;验收地点：" + model.SetupAddress;
                                UserEntity userEntity = userservice.GetEntity(model.ApplyUserId);
                                //推送给作业申请人
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY022", "脚手架搭设验收申请已通过，请您及时处理", Content, keyValue);
                                }
                            }
                        }

                        #endregion

                        break;
                    case 2:

                        #region 拆除申请审核
                        if (model.SetupCompanyType == 0)
                        {
                            moduleName = "(搭设拆除-内部-" + model.SetupTypeName + ")审核";
                        }
                        else
                        {
                            moduleName = "(搭设拆除-外包-" + model.SetupTypeName + ")审核";
                        }
                        //if (model.SetupType == 1)
                        //{
                        //    if (model.SetupCompanyType == 0)
                        //    {
                        //        moduleName = "(搭设拆除-内部-6米以上)审核";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(搭设拆除-外包-6米以上)审核";
                        //    }
                        //}
                        //else
                        //{
                        //    if (model.SetupCompanyType == 0)
                        //    {
                        //        moduleName = "(搭设拆除-内部-6米以下)审核";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(搭设拆除-外包-6米以下)审核";
                        //    }
                        //}
                        mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, model.FlowId);
                        if (model.AuditState == 0)
                        {
                            model.FlowName = "申请中";
                            model.InvestigateState = 0;
                        }
                        if (model.AuditState == 1)
                        {
                            //如果审核步骤不为空，处理流程信息及状态
                            if (mpcEntity != null)
                            {
                                model.FlowDeptId = mpcEntity.CHECKDEPTID;
                                model.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                                model.FlowRoleId = mpcEntity.CHECKROLEID;
                                model.FlowRoleName = mpcEntity.CHECKROLENAME;
                                model.FlowId = mpcEntity.ID;
                                model.FlowName = mpcEntity.FLOWNAME;
                                model.AuditState = 1;
                                model.InvestigateState = 2;
                                model.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                                
                            }
                            else
                            {
                                model.FlowRemark = "";
                                model.AuditState = 3;
                                model.FlowName = "已完结";
                                model.FlowDeptId = "";
                                model.FlowDeptName = "";
                                model.FlowId = "";
                                model.FlowRoleId = "";
                                model.FlowRoleName = "";
                                model.InvestigateState = 3;

                                string Content = "脚手架拆除类型：" + model.SetupTypeName + "&#10;拆除时间：" + model.DismentleStartDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + model.DismentleEndDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;拆除地点：" + model.SetupAddress;
                                UserEntity userEntity = userservice.GetEntity(model.ApplyUserId);
                                string[] workuserlist = (model.SetupChargePersonIds + "," + model.DismentlePersonsIds).Split(',');
                                DataTable dutyuserDt = new DataTable();
                                dutyuserDt = userservice.GetUserTable(workuserlist);
                                //推送给作业申请人
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY023", "脚手架拆除申请已通过，请及时处理", Content, keyValue);
                                }
                                //推送给作业负责人/作业人
                                if (dutyuserDt.Rows.Count > 0)
                                {
                                    string Account = "";
                                    string RealName = "";
                                    foreach (DataRow item in dutyuserDt.Rows)
                                    {
                                        Account += item["account"].ToString() + ",";
                                        RealName += item["realname"].ToString() + ",";
                                    }
                                    if (!string.IsNullOrEmpty(Account))
                                    {
                                        Account = Account.Substring(0, Account.Length - 1);
                                        RealName = RealName.Substring(0, RealName.Length - 1);
                                    }
                                    JPushApi.PushMessage(Account, RealName, "ZY023", "您有一条新的脚手架拆除作业任务，请及时处理", Content, keyValue);
                                }
                            }
                        }
                        #endregion

                        break;
                    default:
                        break;
                }

                service.SaveForm(keyValue, model);

                var entity = GetEntity(keyValue);
                if (entity.AuditState == 1)
                {
                    string executedept = string.Empty;
                    highriskcommonapplybll.GetExecutedept(entity.SetupCompanyType.ToString(), entity.SetupCompanyId, entity.OutProjectId, out executedept);//获取执行部门
                    string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                    string outsouringengineerdept = string.Empty;
                    highriskcommonapplybll.GetOutsouringengineerDept(entity.SetupCompanyId, out outsouringengineerdept);
                    string accountstr = powerCheck.GetApproveUserAccount(entity.FlowId, entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //获取审核人账号
                                                                                                                                                                                         //推送消息到有审批权限的人
                    DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
                    string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                    switch (entity.ScaffoldType)
                    {
                        case 0:
                            JPushApi.PushMessage(accountstr, string.Join(",",usernames), "ZY005", "", "", entity.Id);
                            break;
                        case 1:
                            if (entity.AuditState == 4)
                            {
                                JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY007", "", "", entity.Id);
                            }
                            else
                            {
                                JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY008", "", "", entity.Id);
                            }
                            break;
                        case 2:
                            JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY010", "", "", entity.Id);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 推送系统短消息
        /// </summary>
        /// <param name="model"></param>
        public void SendMessage(string flowdeptid, string flowroleid, string code, string entityid, string title = "", string content = "", string type = "", string specialtytype = "")
        {
            string names = "";
            string accounts = "";
            string flowdeptids = "'" + flowdeptid.Replace(",", "','") + "'";
            string flowroleids = "'" + flowroleid.Replace(",", "','") + "'";
            IList<UserEntity> users = userservice.GetUserListByDeptId(flowdeptids, flowroleids, true, string.Empty).OrderBy(t => t.RealName).ToList();
            if (users != null && users.Count > 0)
            {
                if (!string.IsNullOrEmpty(specialtytype) && type == "1")
                {
                    foreach (var item in users)
                    {
                        if (item.RoleName.Contains("专工"))
                        {
                            if (!string.IsNullOrEmpty(item.SpecialtyType) && item.SpecialtyType != "null")
                            {
                                string[] str = item.SpecialtyType.Split(',');
                                for (int i = 0; i < str.Length; i++)
                                {
                                    if (str[i] == specialtytype)
                                    {
                                        names += item.RealName + ",";
                                        accounts += item.Account + ",";
                                    }
                                }
                            }
                        }
                        else
                        {
                            names += item.RealName + ",";
                            accounts += item.Account + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(names))
                    {
                        names = names.TrimEnd(',');
                    }
                    if (!string.IsNullOrEmpty(accounts))
                    {
                        accounts = accounts.TrimEnd(',');
                    }
                }
                else
                {
                    names = string.Join(",", users.Select(x => x.RealName).ToArray());
                    accounts = string.Join(",", users.Select(x => x.Account).ToArray());
                }
                if (!string.IsNullOrEmpty(content))
                {
                    JPushApi.PushMessage(accounts, names, code, title, content, entityid);
                }
                else
                {
                    JPushApi.PushMessage(accounts, names, code, entityid);
                }
            }
        }

        /// <summary>
        /// 申请审核
        /// 适用于审核时，修改业务状态信息
        /// </summary>
        /// <param name="key">申请信息主键ID</param>
        /// <param name="auditEntity">审核记录</param>
        /// <param name="checktype">为区别是否是项目验收确认，1为项目验收确认 其他则为正常审批流程</param>
        public void ApplyCheck(string keyValue, ScaffoldauditrecordEntity auditEntity, List<ScaffoldprojectEntity> projects, int checktype)
        {
            List<RoleEntity> roles = (List<RoleEntity>)rolesevice.GetList();
            ScaffoldEntity scaffoldEntity = service.GetEntity(keyValue);
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ManyPowerCheckEntity mpcEntity = null;
            string moduleName = "";

            //把当前业务流程节点赋值到审核记录中
            auditEntity.FlowId = scaffoldEntity.FlowId;

            //没有审核节点时，默认审核的角色部门为当前用户
            scaffoldEntity.FlowDeptId = currUser.DeptId;
            scaffoldEntity.FlowDeptName = currUser.DeptName;
            scaffoldEntity.FlowRoleId = currUser.RoleId;
            scaffoldEntity.FlowRoleName = currUser.RoleName;
            switch (scaffoldEntity.ScaffoldType)
            {
                case 0:

                    #region 搭设申请审核
                    //如果审核记录不为空，且为不同意，流程结束
                    if (auditEntity != null && auditEntity.AuditState == 0)
                    {
                        scaffoldEntity.AuditState = 2;
                        scaffoldEntity.FlowName = "已完结";
                        scaffoldEntity.InvestigateState = 3;
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";

                        //审批不通过,推消息到申请人 2019.01.10 sx 暂不开放
                        UserEntity userEntity = userservice.GetEntity(scaffoldEntity.CreateUserId);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY004", scaffoldEntity.Id);
                        }

                        break;
                    }

                    //审核同意，只处理6米以上的
                    if (scaffoldEntity.SetupCompanyType == 0)
                    {
                        moduleName = "(搭设申请-内部-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                    else
                    {
                        moduleName = "(搭设申请-外包-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                    //if (scaffoldEntity.SetupType == 1)
                    //{
                    //    if (scaffoldEntity.SetupCompanyType == 0)
                    //    {
                    //        moduleName = "(搭设申请-内部-6米以上)审核";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(搭设申请-外包-6米以上)审核";
                    //    }
                    //}
                    //else
                    //{
                    //    if (scaffoldEntity.SetupCompanyType == 0)
                    //    {
                    //        moduleName = "(搭设申请-内部-6米以下)审核";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(搭设申请-外包-6米以下)审核";
                    //    }
                    //}
                    mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, scaffoldEntity.FlowId);
                    //如果审核步骤不为空，处理流程信息及状态
                    if (mpcEntity != null)
                    {
                        scaffoldEntity.FlowDeptId = mpcEntity.CHECKDEPTID;
                        scaffoldEntity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        scaffoldEntity.FlowRoleId = mpcEntity.CHECKROLEID;
                        scaffoldEntity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        scaffoldEntity.FlowId = mpcEntity.ID;
                        scaffoldEntity.FlowName = mpcEntity.FLOWNAME;
                        scaffoldEntity.AuditState = 1;
                        scaffoldEntity.InvestigateState = 2;
                        scaffoldEntity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                        ////推送消息到有审批权限的人
                        //string type = scaffoldEntity.FlowRemark != "1" ? "0" : "1";
                        //this.SendMessage(scaffoldEntity.FlowDeptId, scaffoldEntity.FlowRoleId, "ZY005", scaffoldEntity.Id, "", "", type, !string.IsNullOrEmpty(scaffoldEntity.SpecialtyType) ? scaffoldEntity.SpecialtyType : "");

                    }
                    else
                    {
                        scaffoldEntity.FlowRemark = "";
                        scaffoldEntity.AuditState = 3;
                        scaffoldEntity.FlowName = "已完结";
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";
                        scaffoldEntity.InvestigateState = 3;

                        var high = GetEntity(scaffoldEntity.Id);
                        if (high != null)
                        {
                            string Content = "脚手架搭设类型：" + (high.SetupType == 0 ? "6米以下脚手架搭设申请" : "6米以上脚手架搭设申请") + "&#10;搭设时间：" + high.SetupStartDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + high.SetupEndDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;搭设地点：" + high.SetupAddress;
                            UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                            string[] workuserlist = (high.SetupChargePersonIds + "," + high.SetupPersonIds).Split(',');
                            DataTable dutyuserDt = new DataTable();
                            dutyuserDt = userservice.GetUserTable(workuserlist);
                            //推送给作业申请人
                            if (userEntity != null)
                            {
                                JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY021", "脚手架搭设申请已通过，请及时处理", Content, scaffoldEntity.Id);
                            }
                            //推送给作业负责人/作业人
                            if (dutyuserDt.Rows.Count > 0)
                            {
                                string Account = "";
                                string RealName = "";
                                foreach (DataRow item in dutyuserDt.Rows)
                                {
                                    Account += item["account"].ToString() + ",";
                                    RealName += item["realname"].ToString() + ",";
                                }
                                if (!string.IsNullOrEmpty(Account))
                                {
                                    Account = Account.Substring(0, Account.Length - 1);
                                    RealName = RealName.Substring(0, RealName.Length - 1);
                                }
                                JPushApi.PushMessage(Account, RealName, "ZY021", "您有一条新的脚手架搭设作业任务，请及时处理", Content, scaffoldEntity.Id);
                            }
                        }
                    }
                    #endregion

                    break;
                case 1:

                    #region 验收申请审核

                    //如果审核记录不为空，且为不同意，流程结束
                    if (auditEntity != null && auditEntity.AuditState == 0)
                    {
                        if (scaffoldEntity.FlowName.Contains("验收确认"))
                        {
                            scaffoldEntity.AuditState = 5; //如果是专工，则状态为验收不通过
                            scaffoldEntity.FlowName = string.IsNullOrEmpty(scaffoldEntity.FlowName) ? "项目验收确认不通过" : scaffoldEntity.FlowName; ;
                            scaffoldEntity.InvestigateState = 1;
                            scaffoldEntity.FlowDeptId = "";
                            scaffoldEntity.FlowDeptName = "";
                            scaffoldEntity.FlowId = "";
                            scaffoldEntity.FlowRoleId = "";
                            scaffoldEntity.FlowRoleName = "";

                        }
                        else
                        {
                            scaffoldEntity.AuditState = 2;
                            scaffoldEntity.FlowName = string.IsNullOrEmpty(scaffoldEntity.FlowName) ? "审核不通过" : scaffoldEntity.FlowName;
                            scaffoldEntity.InvestigateState = 2;
                            scaffoldEntity.FlowDeptId = "";
                            scaffoldEntity.FlowDeptName = "";
                            scaffoldEntity.FlowId = "";
                            scaffoldEntity.FlowRoleId = "";
                            scaffoldEntity.FlowRoleName = "";
                        }

                        //审批不通过,推消息到申请人 2019.01.10 sx 暂不开放
                        UserEntity userEntity = userservice.GetEntity(scaffoldEntity.CreateUserId);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY006", scaffoldEntity.Id);
                        }

                        break;
                    }

                    //单位内部
                    if (scaffoldEntity.SetupCompanyType == 0)
                    {
                        moduleName = "(搭设验收-内部-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                    else
                    {
                        moduleName = "(搭设验收-外包-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                    //if (scaffoldEntity.SetupCompanyType == 0)
                    //{
                    //    if (scaffoldEntity.SetupType == 0)
                    //    {
                    //        moduleName = "(搭设验收-内部-6米以下)审核";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(搭设验收-内部-6米以上)审核";
                    //    }
                    //}
                    //else
                    //{
                    //    if (scaffoldEntity.SetupType == 0)
                    //    {
                    //        moduleName = "(搭设验收-外包-6米以下)审核";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(搭设验收-外包-6米以上)审核";
                    //    }
                    //}
                    mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, scaffoldEntity.FlowId);

                    //如果审核步骤不为空，处理流程信息及状态
                    if (mpcEntity != null)
                    {
                        scaffoldEntity.FlowDeptId = mpcEntity.CHECKDEPTID;
                        scaffoldEntity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        scaffoldEntity.FlowRoleId = mpcEntity.CHECKROLEID;
                        scaffoldEntity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        scaffoldEntity.FlowId = mpcEntity.ID;
                        scaffoldEntity.FlowName = mpcEntity.FLOWNAME;
                        scaffoldEntity.AuditState = 1;
                        scaffoldEntity.InvestigateState = 2;
                        scaffoldEntity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                        //如果审核同意
                        if (auditEntity != null && auditEntity.AuditState == 1)
                        {
                            //流程节点为验收确认时，状态为验收中
                            if (scaffoldEntity.FlowName.Contains("验收确认"))
                            {
                                scaffoldEntity.AuditState = 4;
                            }
                            if (checktype == 1)
                            {
                                scaffoldEntity.AuditState = 6;
                                scaffoldEntity.FlowName = string.IsNullOrEmpty(scaffoldEntity.FlowName) ? "项目验收确认通过" : scaffoldEntity.FlowName;
                                scaffoldEntity.InvestigateState = 1;

                                //更改实际搭设时间
                                var buildentity = this.GetEntity(scaffoldEntity.SetupInfoId);
                                buildentity.ActSetupStartDate = scaffoldEntity.ActSetupStartDate;
                                buildentity.ActSetupEndDate = scaffoldEntity.ActSetupEndDate;
                                service.SaveForm(scaffoldEntity.SetupInfoId, buildentity);
                            }
                            else
                            {
                                //否则状态为审核中
                                scaffoldEntity.FlowName = string.IsNullOrEmpty(scaffoldEntity.FlowName) ? "审核中" : scaffoldEntity.FlowName;
                            }

                            //string type = scaffoldEntity.FlowRemark != "1" ? "0" : "1";
                            //if (scaffoldEntity.AuditState == 4)
                            //{
                            //    //推送消息给脚手架验收项目确认人
                            //    this.SendMessage(scaffoldEntity.FlowDeptId, scaffoldEntity.FlowRoleId, "ZY007", scaffoldEntity.Id, "", "", type, !string.IsNullOrEmpty(scaffoldEntity.SpecialtyType) ? scaffoldEntity.SpecialtyType : "");
                            //}
                            //else
                            //{
                            //    //推送消息给脚手架验收审批人
                            //    this.SendMessage(scaffoldEntity.FlowDeptId, scaffoldEntity.FlowRoleId, "ZY008", scaffoldEntity.Id, "", "", type, !string.IsNullOrEmpty(scaffoldEntity.SpecialtyType) ? scaffoldEntity.SpecialtyType : "");
                            //}
                        }

                    }
                    else
                    {
                        scaffoldEntity.FlowRemark = "";
                        scaffoldEntity.AuditState = 3;
                        scaffoldEntity.FlowName = "已完结";
                        scaffoldEntity.InvestigateState = 3;
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";

                        //更改实际搭设时间
                        var buildentity = this.GetEntity(scaffoldEntity.SetupInfoId);
                        buildentity.ActSetupStartDate = scaffoldEntity.ActSetupStartDate;
                        buildentity.ActSetupEndDate = scaffoldEntity.ActSetupEndDate;
                        service.SaveForm(scaffoldEntity.SetupInfoId, buildentity);


                        var high = GetEntity(scaffoldEntity.Id);
                        if (high != null)
                        {
                            string Content = "脚手架验收类型：" + (high.SetupType == 0 ? "6米以下脚手架搭设申请" : "6米以上脚手架搭设申请") + "&#10;验收时间：" + high.SetupStartDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + high.SetupEndDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;验收地点：" + high.SetupAddress;
                            UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                            //推送给作业申请人
                            if (userEntity != null)
                            {
                                JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY022", "脚手架搭设验收申请已通过，请您及时处理", Content, scaffoldEntity.Id);
                            }
                        }
                    }

                    #endregion

                    break;
                case 2:

                    #region 拆除申请审核

                    //如果审核记录不为空，且为不同意，流程结束
                    if (auditEntity != null && auditEntity.AuditState == 0)
                    {
                        scaffoldEntity.AuditState = 2;
                        scaffoldEntity.FlowName = "已完结";
                        scaffoldEntity.InvestigateState = 3;
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";

                        //审批不通过,推消息到申请人 2019.01.10 sx 暂不开放
                        UserEntity userEntity = userservice.GetEntity(scaffoldEntity.CreateUserId);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY009", scaffoldEntity.Id);
                        }

                        break;
                    }
                    //审核同意，只处理6米以上的
                    if (scaffoldEntity.SetupCompanyType == 0)
                    {
                        moduleName = "(搭设拆除-内部-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                    else
                    {
                        moduleName = "(搭设拆除-外包-" + scaffoldEntity.SetupTypeName + ")审核";
                    }
                    //if (scaffoldEntity.SetupType == 1)
                    //{
                    //    if (scaffoldEntity.SetupCompanyType == 0)
                    //    {
                    //        moduleName = "(搭设拆除-内部-6米以上)审核";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(搭设拆除-外包-6米以上)审核";
                    //    }
                    //}
                    //else
                    //{
                    //    if (scaffoldEntity.SetupCompanyType == 0)
                    //    {
                    //        moduleName = "(搭设拆除-内部-6米以下)审核";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(搭设拆除-外包-6米以下)审核";
                    //    }
                    //}
                    mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, scaffoldEntity.FlowId);
                    //如果审核步骤不为空，处理流程信息及状态
                    if (mpcEntity != null)
                    {
                        scaffoldEntity.FlowDeptId = mpcEntity.CHECKDEPTID;
                        scaffoldEntity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        scaffoldEntity.FlowRoleId = mpcEntity.CHECKROLEID;
                        scaffoldEntity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        scaffoldEntity.FlowId = mpcEntity.ID;
                        scaffoldEntity.FlowName = mpcEntity.FLOWNAME;
                        scaffoldEntity.AuditState = 1;
                        scaffoldEntity.InvestigateState = 2;
                        scaffoldEntity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                        ////推送消息到有审批权限的人
                        //string type = scaffoldEntity.FlowRemark != "1" ? "0" : "1";
                        //this.SendMessage(scaffoldEntity.FlowDeptId, scaffoldEntity.FlowRoleId, "ZY010", scaffoldEntity.Id, "", "", type, !string.IsNullOrEmpty(scaffoldEntity.SpecialtyType) ? scaffoldEntity.SpecialtyType : "");
                    }
                    else
                    {
                        scaffoldEntity.FlowRemark = "";
                        scaffoldEntity.AuditState = 3;
                        scaffoldEntity.FlowName = "已完结";
                        scaffoldEntity.InvestigateState = 3;
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";


                        var high = GetEntity(scaffoldEntity.Id);
                        if (high != null)
                        {
                            string Content = "脚手架拆除类型：" + (high.SetupType == 0 ? "6米以下脚手架搭设申请" : "6米以上脚手架搭设申请") + "&#10;拆除时间：" + high.DismentleStartDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + " 到 " + high.DismentleEndDate.Value.ToString("yyyy年MM月dd日 HH时mm分") + "&#10;拆除地点：" + high.SetupAddress;
                            UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                            string[] workuserlist = (high.SetupChargePersonIds + "," + high.DismentlePersonsIds).Split(',');
                            DataTable dutyuserDt = new DataTable();
                            dutyuserDt = userservice.GetUserTable(workuserlist);
                            //推送给作业申请人
                            if (userEntity != null)
                            {
                                JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY023", "脚手架拆除申请已通过，请及时处理", Content, scaffoldEntity.Id);
                            }
                            //推送给作业负责人/作业人
                            if (dutyuserDt.Rows.Count > 0)
                            {
                                string Account = "";
                                string RealName = "";
                                foreach (DataRow item in dutyuserDt.Rows)
                                {
                                    Account += item["account"].ToString() + ",";
                                    RealName += item["realname"].ToString() + ",";
                                }
                                if (!string.IsNullOrEmpty(Account))
                                {
                                    Account = Account.Substring(0, Account.Length - 1);
                                    RealName = RealName.Substring(0, RealName.Length - 1);
                                }
                                JPushApi.PushMessage(Account, RealName, "ZY023", "您有一条新的脚手架拆除作业任务，请及时处理", Content, scaffoldEntity.Id);
                            }
                        }
                    }

                    #endregion

                    break;
                default:
                    break;
            }

            this.service.UpdateForm(scaffoldEntity, auditEntity, projects);
            var entity = GetEntity(keyValue);
            if (entity.AuditState == 1)
            {
                string executedept = string.Empty;
                highriskcommonapplybll.GetExecutedept(entity.SetupCompanyType.ToString(), entity.SetupCompanyId, entity.OutProjectId, out executedept);//获取执行部门
                string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //获取创建部门ID
                string outsouringengineerdept = string.Empty;
                highriskcommonapplybll.GetOutsouringengineerDept(entity.SetupCompanyId, out outsouringengineerdept);
                string accountstr = powerCheck.GetApproveUserAccount(entity.FlowId, entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //获取审核人账号
                                                                                                                                                                                     //推送消息到有审批权限的人
                DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
                string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                switch (entity.ScaffoldType)
                {
                    case 0:
                        JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY005", "", "", entity.Id);
                        break;
                    case 1:
                        if (entity.AuditState == 4)
                        {
                            JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY007", "", "", entity.Id);
                        }
                        else
                        {
                            JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY008", "", "", entity.Id);
                        }
                        break;
                    case 2:
                        JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY010", "", "", entity.Id);
                        break;
                    default:
                        break;
                }
            }
        }
        
        /// <summary>
        /// 台账状态操作
        /// </summary>
        /// <returns></returns>
        public void LedgerOp(string keyValue, string ledgerType, string type, string worktime, string issendmessage, string conditioncontent, string conditionid = "", string iscomplete = "")
        {
            string title = string.Empty;
            string message = string.Empty;
            string projectid = "";
            string workdeptid = "";
            var time = Convert.ToDateTime(worktime);
            Operator curUser = OperatorProvider.Provider.Current();
            string userids = "";
            if (type == "1")
            {
                #region 通用高风险作业
                HighRiskCommonApplyEntity entity = highriskcommonapplybll.GetEntity(keyValue);
                if (entity != null)
                {
                    string sql = "";
                    workdeptid = entity.WorkDeptId;
                    if (entity.WorkDeptType == "1")
                    {
                        projectid = entity.EngineeringId;
                    }
                    title = "高风险作业信息";
                    if (ledgerType == "0")
                    {
                        sql = string.Format("update bis_highriskcommonapply set RealityWorkStartTime=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='0',RealityWorkEndTime='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                        entity.RealityWorkStartTime = time;
                        entity.WorkOperate = "0";
                        entity.RealityWorkEndTime = null;
                        //即将作业
                        //新增一条短消息，发送给发包部门和安全主管部门的安全管理员、专工、负责人
                        //消息内容为：作业单位+在+作业地点+开始+作业类型，您好，2018年12月18日15时33分，检修部在机组排水槽完成起重吊装作业
                        message = string.Format("您好,{0},{1}在{2}开始{3}。", time.ToString("yyyy年MM月dd日 HH时mm分"), entity.WorkDeptName, entity.WorkPlace, getName(entity.WorkType, "CommonType"));

                        //
                        userids = entity.WorkUserIds;
                        HdgzUserPower(userids, 1);
                    }
                    else if (ledgerType == "1")
                    {
                        entity.RealityWorkEndTime = time;
                        if (iscomplete == "0")
                        {
                            entity.WorkOperate = "1";
                        }
                        sql = string.Format("update bis_highriskcommonapply set WorkOperate='{2}',RealityWorkEndTime=to_date('{1}','yyyy-mm-dd hh24:mi:ss') where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"), entity.WorkOperate);
                        //作业中
                        //新增一条短消息，发送给发包部门和安全主管部门的安全管理员、专工、负责人
                        //消息内容为：实际作业结束时间,作业单位+在+作业地点+完成 +作业类型，您好，2018年12月18日15时33分，检修部在机组排水槽完成起重吊装作业
                        message = string.Format("您好,{0},{1}在{2}完成{3}。", time.ToString("yyyy年MM月dd日 HH时mm分"), entity.WorkDeptName, entity.WorkPlace, getName(entity.WorkType, "CommonType"));
                    }
                    highriskcommonapplybll.UpdateData(sql);
                    //highriskcommonapplybll.SaveApplyForm(keyValue, entity);
                }
                #endregion
            }
            else if (type == "2")
            {
                #region 脚手架作业
                ScaffoldEntity entity = this.GetEntity(keyValue);
                if (entity != null)
                {
                    string sql = "";
                    workdeptid = entity.SetupCompanyId;
                    if (entity.SetupCompanyType == 1)
                    {
                        projectid = entity.OutProjectId;
                    }
                    if (ledgerType == "0")
                    {
                        sql = string.Format("update bis_scaffold set actsetupstartdate=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='0',actsetupenddate='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                        entity.ActSetupStartDate = time;
                        entity.WorkOperate = "0";
                        //即将搭设
                        //新增一条短消息，发送给发包部门和安全主管部门的安全管理员、专工、负责人
                        //消息内容为：搭设开始时间，搭设单位+于+搭设地点+开始+作业类型，如2018年12月18日15时33分，检修部在机组排水槽开始脚手架搭设作业
                        message = string.Format("您好,{0},{1}在{2}开始{3}。", time.ToString("yyyy年MM月dd日 HH时mm分"), entity.SetupCompanyName, entity.SetupAddress, "脚手架搭设作业");
                        title = "脚手架搭设作业消息";
                    }
                    if (ledgerType == "1")
                    {
                        
                        entity.ActSetupEndDate = time;
                        if (iscomplete == "0")
                        {
                            entity.WorkOperate = "1";
                        }
                        sql = string.Format("update bis_scaffold set actsetupenddate=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='{2}' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"),entity.WorkOperate);
                        //搭设中
                        //新增一条短消息，发送给发包部门和安全主管部门的安全管理员、专工、负责人
                        //消息内容为：搭设结束时间，搭设单位+于+搭设地点+开始+脚手架搭设作业，如2018年12月18日15时33分，检修部在机组排水槽完成脚手架搭设作业
                        message = string.Format("您好,{0},{1}在{2}完成{3}。", time.ToString("yyyy年MM月dd日 HH时mm分"), entity.SetupCompanyName, entity.SetupAddress, "脚手架搭设作业");
                        title = "脚手架搭设作业消息";
                    }
                    if (ledgerType == "4")
                    {
                        sql = string.Format("update bis_scaffold set realitydismentlestartdate=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='0',realitydismentleenddate='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                        entity.RealityDismentleStartDate = time;
                        entity.WorkOperate = "0";
                        //即将拆除
                        //新增一条短消息，发送给发包部门和安全主管部门的安全管理员、专工、负责人
                        //消息内容为：拆除开始时间，拆除单位+于+搭设地点+开始+作业类型，如2018年12月18日15时33分，检修部在机组排水槽开始脚手架拆除作业
                        message = string.Format("您好,{0},{1}在{2}开始{3}。", time.ToString("yyyy年MM月dd日 HH时mm分"), entity.SetupCompanyName, entity.SetupAddress, "脚手架拆除作业");
                        title = "脚手架拆除作业消息";
                    }
                    if (ledgerType == "5")
                    {
                        entity.RealityDismentleEndDate = time;
                        if (iscomplete == "0")
                        {
                            entity.WorkOperate = "1";
                        }
                        sql = string.Format("update bis_scaffold set realitydismentleenddate=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='{2}' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"), entity.WorkOperate);
                        //拆除中
                        //新增一条短消息，发送给发包部门和安全主管部门的安全管理员、专工、负责人
                        //消息内容为：拆除结束时间，拆除单位+于+搭设地点+开始+脚手架拆除作业，如2018年12月18日15时33分，检修部在机组排水槽完成脚手架拆除作业
                        message = string.Format("您好,{0},{1}在{2}完成{3}。", time.ToString("yyyy年MM月dd日 HH时mm分"), entity.SetupCompanyName, entity.SetupAddress, "脚手架拆除作业");
                        title = "脚手架拆除作业消息";
                    }
                    highriskcommonapplybll.UpdateData(sql);
                    //this.SaveForm(keyValue, entity);
                }
                #endregion
            }
            else if (type == "3")
            {
                #region 安全设施变动
                SafetychangeEntity entity = safetychangebll.GetEntity(keyValue);
                if (entity != null)
                {
                    workdeptid = entity.WORKUNITID;
                    if (entity.WORKUNITTYPE == "1")
                    {
                        projectid = entity.PROJECTID;
                    }
                    title = "安全设施变动信息";
                    if (ledgerType == "0")
                    {
                        entity.REALITYCHANGETIME = time;
                        //即将变动
                        //新增一条短消息，发送给发包部门和安全主管部门的安全管理员、专工、负责人
                        //消息内容为：您好，实际变动开始时间，作业单位+在+作业地点+开始+安全设施名称+变动形式
                        message = string.Format("您好,{0},{1}在{2}开始{3}{4}。", time.ToString("yyyy年MM月dd日 HH时mm分"), entity.WORKUNIT, entity.WORKPLACE, entity.CHANGENAME, entity.CHANGETYPE);

                    }
                    safetychangebll.SaveForm(keyValue, entity);
                }
                #endregion
            }
            else if (type == "4")
            {
                #region 使用消防水
                FireWaterEntity entity = firewaterbll.GetEntity(keyValue);
                if (entity != null)
                {
                    workdeptid = entity.WorkDeptId;
                    if (entity.WorkDeptType == "1")
                    {
                        projectid = entity.EngineeringId;
                    }
                    title = "使用消防水信息";
                    string sql = "";
                    if (ledgerType == "0")
                    {
                        sql = string.Format("update bis_firewater set realityworkstarttime=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='0',realityworkendtime='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                        entity.RealityWorkStartTime = time;
                        entity.WorkOperate = "0";
                        //即将作业
                        //新增一条短消息，发送给发包部门和安全主管部门的安全管理员、专工、负责人
                        //消息内容为： 实际使用消防水开始时间，使用消防水单位+在+使用消防水地点+开始进行消防水使用作业,请知晓。
                        message = string.Format("您好,{0},{1}在{2}开始进行消防水使用作业,请知晓。", time.ToString("yyyy年MM月dd日 HH时mm分"), entity.WorkDeptName, entity.WorkPlace);

                    }
                    else if (ledgerType == "1")
                    {
                        entity.RealityWorkEndTime = time;
                        if (iscomplete == "0")
                        {
                            entity.WorkOperate = "1";
                        }
                        sql = string.Format("update bis_firewater set realityworkendtime=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='{2}' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"),entity.WorkOperate);
                        //作业中
                        //新增一条短消息，发送给发包部门和安全主管部门的安全管理员、专工、负责人
                        //消息内容为：实际使用消防水结束时间,使用消防水单位+在+使用消防水地点+结束消防水使用作业，请知晓。
                        message = string.Format("您好,{0},{1}在{2}结束消防水使用作业,请知晓。", time.ToString("yyyy年MM月dd日 HH时mm分"), entity.WorkDeptName, entity.WorkPlace);
                    }
                    highriskcommonapplybll.UpdateData(sql);
                    //firewaterbll.SaveForm(keyValue, entity);
                }
                #endregion
            }
            #region 添加执行情况信息
            FireWaterCondition Conditionentity = new FireWaterCondition();
            Conditionentity.Id = !string.IsNullOrEmpty(conditionid) ? conditionid : "";
            Conditionentity.LedgerType = ledgerType;
            Conditionentity.ConditionTime = time;
            Conditionentity.ConditionContent = conditioncontent;
            Conditionentity.ConditionDept = curUser.DeptName;
            Conditionentity.ConditionDeptCode = curUser.DeptCode;
            Conditionentity.ConditionDeptId = curUser.DeptId;
            Conditionentity.ConditionPerson = curUser.UserName;
            Conditionentity.ConditionPersonId = curUser.UserId;
            Conditionentity.FireWaterId = keyValue;
            firewaterbll.SubmitCondition(conditionid, Conditionentity);

            #endregion

            #region 发送短消息
            //发送短消息
            if (issendmessage == "1")
            {
                IList<UserEntity> users1 = null;
                if (!string.IsNullOrEmpty(projectid))
                {
                    //配置的角色
                    string rolenames = dataitemdetailservice.GetItemValue("LedgerSendDept");
                    //责任部门
                    DepartmentEntity departEntity = departmentservice.GetEntity(new OutsouringengineerService().GetEntity(projectid).ENGINEERLETDEPTID);
                    if (departEntity != null && !string.IsNullOrEmpty(rolenames))
                    {
                        rolenames = "'" + rolenames.Replace(",", "','") + "'";
                        users1 = userservice.GetUserListByRoleName("'" + departEntity.DepartmentId + "'", rolenames, true, string.Empty);
                    }
                }
                //安全主管部门
                Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                IEnumerable<DepartmentEntity> departs = departmentservice.GetDepts(currUser.OrganizeId, 1);
                string deptids = string.Empty;
                //配置的角色
                string orgrolenames = dataitemdetailservice.GetItemValue("LedgerManageDept");
                IList<UserEntity> users2 = null;
                if (departs != null && departs.Count() > 0 && !string.IsNullOrEmpty(orgrolenames))
                {
                    deptids = string.Join(",", departs.Select(x => x.DepartmentId).ToArray());
                    deptids = "'" + deptids.Replace(",", "','") + "'";
                    orgrolenames = "'" + orgrolenames.Replace(",", "','") + "'";
                    //安全主管部门，角色的用户
                    users2 = userservice.GetUserListByRoleName(deptids, orgrolenames, true, string.Empty);
                }
                //作业单位部门
                IList<UserEntity> users3 = null;
                if (!string.IsNullOrEmpty(workdeptid))
                {
                    //配置的角色
                    string rolenames = dataitemdetailservice.GetItemValue("LedgerWorkDept");
                    if (!string.IsNullOrEmpty(rolenames))
                    {
                        rolenames = "'" + rolenames.Replace(",", "','") + "'";
                        users3 = userservice.GetUserListByRoleName("'" + workdeptid + "'", rolenames, true, string.Empty);
                    }
                }
                List<UserEntity> users = new List<UserEntity>();
                if (users1 != null && users1.Count > 0)
                {
                    users.AddRange(users1);
                    users = users.Union(users1).ToList();
                }
                if (users2 != null && users2.Count > 0)
                {
                    users.AddRange(users2);
                    users = users.Union(users2).ToList();
                }
                if (users3 != null && users3.Count > 0)
                {
                    users.AddRange(users3);
                    users = users.Union(users3).ToList();
                }
                if (users != null && users.Count > 0)
                {
                    string names = string.Join(",", users.Select(x => x.RealName).ToArray());
                    string accounts = string.Join(",", users.Select(x => x.Account).ToArray());
                    var senduser = new UserBLL().GetUserInfoByAccount("System");
                    MessageEntity msg = new MessageEntity
                    {
                        Title = title,
                        Content = message,
                        UserId = accounts,
                        UserName = names,
                        Status = "",
                        Url = string.Empty,
                        SendUser = senduser.Account,
                        SendUserName = senduser.RealName,
                        Category = "高风险作业"
                    };
                    if (new MessageBLL().SaveForm("", msg))
                    {
                        JPushApi.PublicMessage(msg);
                    }
                }
            }
            #endregion
        }
        #endregion
        /// <summary>
        /// 设置毕节人员权限
        /// </summary>
        public void HdgzUserPower(string userids,int isQx)
        {
            try
            {
                string isGZBJ = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("贵州毕节版本");
                if (string.IsNullOrWhiteSpace(isGZBJ))
                {
                    return;
                }
                DataItemDetailBLL data = new DataItemDetailBLL();
                var Key = data.GetItemValue("Hdgzappkey");//毕节URL密钥
                var baseurl = data.GetItemValue("HdgzBaseUrl");//毕节API服务器地址

                string Url = "/api/v2/employee/level/";//接口地址
                string[] levels = { "1"};
                string[] useridList = userids.Split(',');
                List<HdgzLevel> hdgzLevelList = new List<HdgzLevel>();
                foreach (string userid in useridList)
                {
                    HdgzLevel hdgzLevel = new HdgzLevel();
                    //ids += "'" + s + "',";
                    hdgzLevel.pin = new UserBLL().GetEntity(userid).IdentifyID;
                    hdgzLevel.levels = levels;
                    hdgzLevel.tag = isQx;
                    hdgzLevelList.Add(hdgzLevel);
                }
                
                //var model = new
                //{
                //    //pin = uentity.IdentifyID,
                //    levels = 1,
                //    tag = isQx
                //};
                SocketHelper.LoadHdgzCameraList(hdgzLevelList, baseurl, Url, Key);
            }
            catch { }
        }

        public string getName(string type, string encode)
        {
            string strName = "";
            var entity = new DataItemDetailBLL().GetDataItemListByItemCode("'" + encode + "'").Where(a => a.ItemValue == type).FirstOrDefault();
            if (entity != null)
                strName = entity.ItemName;
            return strName;
        }
        /// <summary>
        /// 获取人员
        /// </summary>
        /// <param name="flowdeptid"></param>
        /// <param name="flowrolename"></param>
        /// <param name="type"></param>
        public string GetUserName(string flowdeptid, string flowrolename, string type = "", string specialtytype = "")
        {
            return service.GetUserName(flowdeptid, flowrolename, type, specialtytype);
        }

        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            return service.GetAppFlowList(keyValue, modulename);
        }
        public class itemClass
        {
            public string itemvalue { get; set; }
            public string itemname { get; set; }
        }
        //
        public class HdgzLevel
        {
            public string pin { get; set; }
            public object levels { get; set; }
            public object tag { get; set; }
        }
    }
}
