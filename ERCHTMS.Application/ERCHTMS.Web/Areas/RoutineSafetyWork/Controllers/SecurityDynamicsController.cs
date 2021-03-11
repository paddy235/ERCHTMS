using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using System.Linq;
using System;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// 描 述：安全动态
    /// </summary>
    public class SecurityDynamicsController : MvcControllerBase
    {
        private SecurityDynamicsBLL securitydynamicsbll = new SecurityDynamicsBLL();
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //是否需要审核
            ViewBag.IsCheck = 0;
            //查询是否配置流程
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "安全动态";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            if (powerList.Count > 0)
                ViewBag.IsCheck = powerList.Count;
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            //是否需要审核
            ViewBag.IsCheck = 0;
            //查询是否配置流程
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "安全动态";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            if (powerList.Count > 0)
                ViewBag.IsCheck = powerList.Count;
            return View();
        }
        /// <summary>
        /// 审核详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApproveForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            //是否需要审核
            ViewBag.IsCheck = 0;
            //查询是否配置流程
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "安全动态";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            

            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,Title,Publisher,ReleaseTime,IsSend,isover,flowdeptname,createusername,createdate,flowdept,flowrolename,flowrole,flowname";
            pagination.p_tablename = "BIS_SecurityDynamics t";
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            if (powerList.Count > 0)
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["pagemode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and (ISOVER='0' and flowdept='{1}')", user.UserId, user.DeptId);
                }
                else
                {
                    if (user.RoleName.IndexOf("负责人") >= 0)
                    {
                        pagination.conditionJson += string.Format(" and ((IsSend='0' and ISOVER='1') or (createuserid='{0}') or (ISOVER='0' and flowdept='{1}'))", user.UserId, user.DeptId);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((IsSend='0' and ISOVER='1') or (createuserid='{0}'))", user.UserId, user.DeptId);
                    }
                    //pagination.conditionJson += string.Format(" and ((IsSend='0' and ISOVER='1') or (createuserid='{0}') or (ISOVER='0' and flowdept='{1}'))", user.UserId, user.DeptId);
                }
            }
            else
            {
                pagination.conditionJson += string.Format(" and (IsSend='0' or createuserid='{0}')", user.UserId);
            }
            var watch = CommonHelper.TimerStart();
            var data = securitydynamicsbll.GetPageList(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = securitydynamicsbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = securitydynamicsbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            securitydynamicsbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, SecurityDynamicsEntity entity)
        {
            if (entity.IsSend == "0")//提交
            {
                entity.ReleaseTime = DateTime.Now;//发布时间
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.ISOVER = "1";
                entity.FLOWNAME = "";
            }
            if (entity.IsSend == "1")//保存
            {
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.ISOVER = "0";
                entity.FLOWNAME = "申请中";
            }
            securitydynamicsbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 登记的内容提交到审核或者结束
        /// <summary>
        /// 登记的内容提交到审核或者结束（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, SecurityDynamicsEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string outengineerid = curUser.DeptId;
            string flowid = string.Empty;

            string moduleName = "安全动态";
            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="outengineerid">工程Id</param>
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            //新增时会根据角色自动审核,此时需根据工程和审核配置查询审核流程Id
            OutsouringengineerEntity engineerEntity = new OutsouringengineerBLL().GetEntity(curUser.DeptId);
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, "安全动态");
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
            //先查出执行部门编码
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                {
                    //powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(entity.PROJECTID).EnCode;
                    //powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(entity.PROJECTID).DepartmentId;
                    //powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).EnCode;
                    //powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).DepartmentId;
                    powerList[i].CHECKDEPTCODE = curUser.DeptCode;
                    powerList[i].CHECKDEPTID = curUser.DeptId;
                }
                //创建部门
                if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                {
                    if (entity.CreateUserDeptCode == null || entity.CreateUserDeptCode == "")
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntityByCode(curUser.DeptCode).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntityByCode(curUser.DeptCode).DepartmentId;
                    }
                    else
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).DepartmentId;
                    }
                }
            }
            //登录人是否有审核权限--有审核权限直接审核通过
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTID == curUser.DeptId)
                {
                    var rolelist = curUser.RoleName.Split(',');
                    for (int j = 0; j < rolelist.Length; j++)
                    {
                        if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                        {
                            checkPower.Add(powerList[i]);
                            break;
                        }
                    }
                }
            }
            if (checkPower.Count > 0)
            {
                ManyPowerCheckEntity check = checkPower.Last();//当前

                for (int i = 0; i < powerList.Count; i++)
                {
                    if (check.ID == powerList[i].ID)
                    {
                        flowid = powerList[i].ID;
                    }
                }
            }
            if (null != mpcEntity)
            {
                //保存安全动态记录
                entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                entity.FLOWROLE = mpcEntity.CHECKROLEID;
                entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                entity.IsSend = "0"; //标记已经从登记到审核阶段
                entity.ISOVER = "0"; //流程未完成，1表示完成
                entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审核中";
                entity.FlowId = mpcEntity.ID;
                //DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                //var userAccount = dt.Rows[0]["account"].ToString();
                //var userName = dt.Rows[0]["realname"].ToString();
                //JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
            }
            else  //为空则表示已经完成流程
            {
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.IsSend = "0"; //标记已经从登记到审核阶段
                entity.ISOVER = "1"; //流程未完成，1表示完成
                entity.FLOWNAME = "";
                entity.FlowId = flowid;
                entity.ReleaseTime = DateTime.Now;//发布时间
            }
            securitydynamicsbll.SaveForm(keyValue, entity);

            //添加审核记录
            if (state == "1")
            {
                //审核信息表
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = "0"; //通过
                aidEntity.AUDITTIME = DateTime.Now;
                aidEntity.AUDITPEOPLE = curUser.UserName;
                aidEntity.AUDITPEOPLEID = curUser.UserId;
                aidEntity.APTITUDEID = entity.Id;  //关联的业务ID 
                aidEntity.AUDITOPINION = ""; //审核意见
                aidEntity.AUDITSIGNIMG = curUser.SignImg;
                if (null != mpcEntity)
                {
                    aidEntity.REMARK = (powerList[0].AUTOID.Value - 1).ToString(); //备注 存流程的顺序号

                    //aidEntity.FlowId = mpcEntity.ID;
                }
                else
                {
                    aidEntity.REMARK = "7";
                }
                aidEntity.FlowId = flowid;
                //if (curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("厂级部门用户"))
                //{
                //    aidEntity.AUDITDEPTID = curUser.OrganizeId;
                //    aidEntity.AUDITDEPT = curUser.OrganizeName;
                //}
                //else
                //{
                aidEntity.AUDITDEPTID = curUser.DeptId;
                aidEntity.AUDITDEPT = curUser.DeptName;
                //}
                aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            }

            return Success("操作成功!");
        }
        #endregion

        #region 提交到审核或者结束
        /// <summary>
        /// 登记的内容提交到审核或者结束（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, SecurityDynamicsEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "安全动态";

            entity = securitydynamicsbll.GetEntity(keyValue);

            string outengineerid = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).DepartmentId;

            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="outengineerid">工程Id</param>
            //ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, outengineerid);
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, outengineerid);

            #region //审核信息表
            AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
            aidEntity.AUDITRESULT = aentity.AUDITRESULT; //通过
            aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //审核时间
            aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //审核人员姓名
            aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//审核人员id
            aidEntity.APTITUDEID = keyValue;  //关联的业务ID 
            aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//审核部门id
            aidEntity.AUDITDEPT = aentity.AUDITDEPT; //审核部门
            aidEntity.AUDITOPINION = aentity.AUDITOPINION; //审核意见
            aidEntity.FlowId = aentity.FlowId;
            aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
            if (null != mpcEntity)
            {
                aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
            }
            else
            {
                aidEntity.REMARK = "7";
            }
            aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            #endregion

            #region  //保存安全动态记录
            var smEntity = securitydynamicsbll.GetEntity(keyValue);
            //审核通过
            if (aentity.AUDITRESULT == "0")
            {
                //0表示流程未完成，1表示流程结束
                if (null != mpcEntity)
                {
                    smEntity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    smEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    smEntity.FLOWROLE = mpcEntity.CHECKROLEID;
                    smEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    smEntity.IsSend = "0";
                    smEntity.ISOVER = "0";
                    smEntity.FlowId = mpcEntity.ID;//赋值流程Id
                    smEntity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审核中";
                    //DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                    //var userAccount = dt.Rows[0]["account"].ToString();
                    //var userName = dt.Rows[0]["realname"].ToString();
                    //JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
                }
                else
                {
                    smEntity.FLOWDEPT = "";
                    smEntity.FLOWDEPTNAME = "";
                    smEntity.FLOWROLE = "";
                    smEntity.FLOWROLENAME = "";
                    smEntity.IsSend = "0";
                    smEntity.ISOVER = "1";
                    smEntity.FLOWNAME = "";
                    smEntity.ReleaseTime = DateTime.Now;//发布时间
                }
            }
            else //审核不通过 
            {
                smEntity.FLOWDEPT = "";
                smEntity.FLOWDEPTNAME = "";
                smEntity.FLOWROLE = "";
                smEntity.FLOWROLENAME = "";
                smEntity.IsSend = "1"; //处于登记阶段
                smEntity.ISOVER = "0"; //是否完成状态赋值为未完成
                smEntity.FLOWNAME = "审核（批）未通过";
                smEntity.FlowId = "";//回退后流程Id清空
                //var applyUser = new UserBLL().GetEntity(smEntity.CREATEUSERID);
                //if (applyUser != null)
                //{
                //    JPushApi.PushMessage(applyUser.Account, smEntity.CREATEUSERNAME, "WB002", entity.ID);
                //}

            }
            //更新安全动态基本状态信息
            securitydynamicsbll.SaveForm(keyValue, smEntity);
            #endregion

            #region    //审核不通过
            if (aentity.AUDITRESULT == "1")
            {
                //添加历史记录
                //HistoryRiskWorkEntity hsentity = new HistoryRiskWorkEntity();
                //hsentity.CREATEUSERID = smEntity.CREATEUSERID;
                //hsentity.CREATEUSERDEPTCODE = smEntity.CREATEUSERDEPTCODE;
                //hsentity.CREATEUSERORGCODE = smEntity.CREATEUSERORGCODE;
                //hsentity.CREATEDATE = smEntity.CREATEDATE;
                //hsentity.CREATEUSERNAME = smEntity.CREATEUSERNAME;
                //hsentity.MODIFYDATE = smEntity.MODIFYDATE;
                //hsentity.MODIFYUSERID = smEntity.MODIFYUSERID;
                //hsentity.MODIFYUSERNAME = smEntity.MODIFYUSERNAME;
                //hsentity.SUBMITDATE = smEntity.SUBMITDATE;
                //hsentity.SUBMITPERSON = smEntity.SUBMITPERSON;
                //hsentity.PROJECTID = smEntity.PROJECTID;
                //hsentity.CONTRACTID = smEntity.ID; //关联ID
                //hsentity.ORGANIZER = smEntity.ORGANIZER;
                //hsentity.ORGANIZTIME = smEntity.ORGANIZTIME;
                //hsentity.ISOVER = smEntity.ISOVER;
                //hsentity.ISSAVED = smEntity.ISSAVED;
                //hsentity.FLOWDEPTNAME = smEntity.FLOWDEPTNAME;
                //hsentity.FLOWDEPT = smEntity.FLOWDEPT;
                //hsentity.FLOWROLENAME = smEntity.FLOWROLENAME;
                //hsentity.FLOWROLE = smEntity.FLOWROLE;
                //hsentity.FLOWNAME = smEntity.FLOWNAME;
                //hsentity.SummitContent = smEntity.SummitContent;
                //hsentity.ID = "";

                //historyRiskWorkbll.SaveForm(hsentity.ID, hsentity);

                //获取当前业务对象的所有审核记录
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //批量更新审核记录关联ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    //mode.APTITUDEID = hsentity.ID; //对应新的ID
                    mode.REMARK = "99";
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
                //批量更新附件记录关联ID
                //var flist = fileinfobll.GetImageListByObject(keyValue);
                //foreach (FileInfoEntity fmode in flist)
                //{
                //    fmode.RecId = hsentity.ID; //对应新的ID
                //    fileinfobll.SaveForm("", fmode);
                //}
            }
            #endregion

            return Success("操作成功!");
        }
        #endregion
    }
}
