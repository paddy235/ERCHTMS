using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using System;
using System.Linq;
using ERCHTMS.Entity.BaseManage;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Aspose.Words;
using System.Data;
using System.Web;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：入厂许可申请
    /// </summary>
    public class IntromissionController : MvcControllerBase
    {
        private IntromissionBLL intromissionbll = new IntromissionBLL(); //入厂许可申请操作
        private IntromissionHistoryBLL intromissionhistorybll = new IntromissionHistoryBLL();  //入厂许可申请历史操作
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private InvestigateBLL investigatebll = new InvestigateBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private InvestigateContentBLL investigatecontentbll = new InvestigateContentBLL();
        private InvestigateRecordBLL investigaterecordbll = new InvestigateRecordBLL();
        private InvestigateDtRecordBLL investigatedtrecordbll = new InvestigateDtRecordBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL(); //审核记录
        private OutsouringengineerBLL Outsouringengineernll = new OutsouringengineerBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AppForm()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryForm()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = intromissionbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        #endregion

        #region 获取历史列表
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistoryIntromissionList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "a.id";
            pagination.p_fields = @"a.createuserid,a.createdate, a.applypeople,a.applytime,a.investigatestate,a.outengineerid,b.engineername,c.fullname,
                   a.flowdeptname,a.flowdept,a.flowrolename,a.flowrole,a.flowname,a.flowid ";
            pagination.p_tablename = @" epg_intromissionhistory a  
                                       left join epg_outsouringengineer b on a.outengineerid = b.id 
                                      left join base_department c on b.outprojectid=c.departmentid";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
            {
                pagination.conditionJson = string.Format(" a.createuserorgcode  = '{0}'", user.OrganizeCode);
            }
            else if (role.Contains("承包商级用户"))
            {
                pagination.conditionJson = string.Format(" c.departmentid = '{0}'", user.DeptId);
            }
            else
            {
                pagination.conditionJson = string.Format(" b.engineerletdeptid = '{0}'", user.DeptId);
            }
            var queryParam = queryJson.ToJObject();
            //时间范围
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and to_date(to_char(a.applytime,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //查询条件
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            pagination.conditionJson += string.Format(" and  a.intromissionid = '{0}'", queryParam["keyValue"].ToString());

            var watch = CommonHelper.TimerStart();
            var data = intromissionbll.GetIntromissionPageList(pagination, queryJson);
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
        #endregion

        #region 获取历史实体
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            var data = intromissionhistorybll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region 获取审查记录集合

        /// <summary>
        /// 获取审查记录集合 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetHistoryDtRecordListJson(string keyValue)
        {
            var data = intromissionbll.GetHistoryDtRecordList(keyValue);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetHistoryStartRecordList(string keyValue)
        {
            var data = intromissionbll.GetHistoryStartRecordList(keyValue);
            return ToJsonResult(data);
        }

        #endregion

        #region 获取列表
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetIntromissionList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "a.id";
            pagination.p_fields = @"a.createuserid,a.createdate, a.applypeople,a.applytime,
                                    a.investigatestate,a.outengineerid,b.engineername,c.fullname,
                                    a.flowdeptname,a.flowdept,a.flowrolename,a.flowrole,a.flowname,a.flowid,
                                     b.engineerareaname as districtname,d.itemname engineertype,l.itemname engineerlevel,b.engineerletdept,b.engineerletdeptid,'' as approveuserids";
            pagination.p_tablename = @" epg_intromission a  
                                      left join epg_outsouringengineer b on a.outengineerid = b.id 
                                      left join base_department c on b.outprojectid=c.departmentid
                                      left join ( select m.itemname,m.itemvalue from base_dataitem t
                                      left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=b.engineertype
                                      left join ( select m.itemname,m.itemvalue from base_dataitem t
                                      left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=b.engineerlevel
                                      left join ( select m.itemname,m.itemvalue from base_dataitem t
                                      left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=b.engineerstate ";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;

            string allrangedept = "";
            try
            {
                allrangedept = dataitemdetailbll.GetDataItemByDetailCode("SBDept", "SBDeptId").FirstOrDefault().ItemValue;
            }
            catch (Exception)
            {

            }
            //公用下不显示登记
            pagination.conditionJson = "(( a.investigatestate !='0' and ";

            if (role.Contains("公司级用户") || role.Contains("厂级部门用户") || allrangedept.Contains(user.DeptId))
            {
                pagination.conditionJson += string.Format(" a.createuserorgcode  = '{0}'", user.OrganizeCode);
            }
            else if (role.Contains("承包商级用户"))
            {
                pagination.conditionJson += string.Format("( c.departmentid = '{0}' or b.supervisorid ='{0}')", user.DeptId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(user.DeptId);
                while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }
                pagination.conditionJson += string.Format(" b.engineerletdeptid in (select departmentid from base_department where encode like '{0}%')  ", deptentity.EnCode);

               // pagination.conditionJson += string.Format(" b.engineerletdeptid = '{0}'", user.DeptId);
            }
            pagination.conditionJson += string.Format(" )  or  (a.investigatestate ='0'  and a.createuserid = '{0}'))", user.UserId);
            

            var watch = CommonHelper.TimerStart();
            var data = intromissionbll.GetIntromissionPageList(pagination, queryJson);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var engineerEntity = Outsouringengineernll.GetEntity(data.Rows[i]["outengineerid"].ToString());
                var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                //获取下一步审核人
                string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept,data.Rows[i]["outengineerid"].ToString());
                data.Rows[i]["approveuserids"] = str;
            }

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
        #endregion

        #region 获取审查记录集合

        /// <summary>
        /// 获取审查记录集合 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetDtRecordListJson(string keyValue)
        {
            var data = intromissionbll.GetDtRecordList(keyValue);
            if (data.Rows.Count > 0)
            {
                return ToJsonResult(data);
            }
            else
            {
                return ToJsonResult(new { });
            }
        }
        /// <summary>
        /// 获取开工申请审查记录集合 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetStartForRecordListJson(string keyValue)
        {
            var data = intromissionbll.GetStartRecordList(keyValue);
            if (data.Rows.Count > 0)
            {
                return ToJsonResult(data);
            }
            else
            {
                return ToJsonResult(new { });
            }
        }
        #endregion

        #region 获取实体
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = intromissionbll.GetEntity(keyValue);
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
            intromissionbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        #endregion

        #region 保存表单（新增、修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, IntromissionEntity entity)
        {
            entity.INVESTIGATESTATE = "0";
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.APPLYTIME = DateTime.Now;
            }
            intromissionbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion

        #region 保存审查内容（修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveAppForm(string keyValue, string recordData, IntromissionEntity entity)
        {
            JArray arr = (JArray)JsonConvert.DeserializeObject(recordData);
            //只更新审查内容
            for (int i = 0; i < arr.Count(); i++)
            {
                string id = arr[i]["id"].ToString();  //主键
                string result = arr[i]["result"].ToString(); //结果
                string people = arr[i]["people"].ToString(); //选择的人员
                string peopleid = arr[i]["peopleid"].ToString(); //选择的人员
                string signpic = string.IsNullOrWhiteSpace(arr[i]["signpic"].ToString()) ? "" : arr[i]["signpic"].ToString().Replace("../..", "");
                if (!string.IsNullOrEmpty(id))
                {
                    var scEntity = investigatedtrecordbll.GetEntity(id); //审查内容项
                    scEntity.INVESTIGATERESULT = result;
                    scEntity.INVESTIGATEPEOPLE = people;
                    scEntity.INVESTIGATEPEOPLEID = peopleid;
                    scEntity.SIGNPIC = signpic;
                    investigatedtrecordbll.SaveForm(id, scEntity);
                }
            }

            return Success("操作成功。");
        }
        #endregion

        #region  登记提交到下一个流程
        /// <summary>
        /// 登记提交到下一个流程
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, IntromissionEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string deptid = string.Empty;
            string deptname = string.Empty;
            string roleid = string.Empty;
            string rolename = string.Empty;

            bool isMulti = false;

            //保存信息 
            entity.INVESTIGATESTATE = "0";
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.APPLYTIME = DateTime.Now;
            }
            intromissionbll.SaveForm(keyValue, entity);

            string moduleName = "入厂许可审查";
            string outengineerid = entity.OUTENGINEERID;

            //判断是否需要审查(审查配置表)
            var list = investigatebll.GetList(curUser.OrganizeId).Where(p => p.SETTINGTYPE == "入厂许可").ToList();

            ManyPowerCheckEntity mpcEntity = null;

            InvestigateEntity investigateEntity = null;

            bool isUseSetting = true;

            if (list.Count() > 0)
            {
                investigateEntity = list.FirstOrDefault();
            }
            //审查配置不为空
            if (null != investigateEntity)
            {
                //启用审查
                if (investigateEntity.ISUSE == "是")
                {
                    entity.FLOWNAME = "审查中";
                    entity.INVESTIGATESTATE = "1"; //审查状态

                    //新增审查记录
                    InvestigateRecordEntity rcEntity = new InvestigateRecordEntity();
                    rcEntity.INTOFACTORYID = entity.ID;
                    rcEntity.INVESTIGATETYPE = "0";//当前记录标识
                    investigaterecordbll.SaveForm("", rcEntity);

                    //获取审查内容
                    var contentList = investigatecontentbll.GetList(investigateEntity.ID).ToList();

                    //批量增加审查内容到审查记录中
                    foreach (InvestigateContentEntity icEntity in contentList)
                    {
                        InvestigateDtRecordEntity dtEntity = new InvestigateDtRecordEntity();
                        dtEntity.INVESTIGATERECORDID = rcEntity.ID;
                        dtEntity.INVESTIGATECONTENT = icEntity.INVESTIGATECONTENT;
                        dtEntity.INVESTIGATECONTENTID = icEntity.ID;
                        investigatedtrecordbll.SaveForm("", dtEntity);
                    }
                }
                else  //未启用审查，直接跳转到审核 
                {
                    entity.FLOWNAME = "审核中";
                    entity.INVESTIGATESTATE = "2"; //审核状态
                }
            }
            else
            {
                //如果没有审查配置，直接到审核
                isUseSetting = false;
                entity.FLOWNAME = "审核中";
                entity.INVESTIGATESTATE = "2"; //审核状态
            }
            string status = "";
            //更改申请信息状态
            mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out status, moduleName, outengineerid, false, entity.FLOWID);

            if (null != mpcEntity)
            {
                entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                entity.FLOWROLE = mpcEntity.CHECKROLEID;
                entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                entity.FLOWID = mpcEntity.ID;
                DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                var userAccount = dt.Rows[0]["account"].ToString();
                var userName = dt.Rows[0]["realname"].ToString();
                JPushApi.PushMessage(userAccount, userName, "WB013", entity.ID);
            }
            else
            {
                entity.FLOWNAME = "已完结";
                entity.INVESTIGATESTATE = "3"; //完结状态
            }
            intromissionbll.SaveForm(entity.ID, entity);

            //推送到下一个流程
            return Success("操作成功。");
        }
        #endregion

        #region 提交审查/审核内容（修改）
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitAppForm(string keyValue, string state, string recordData, IntromissionEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            int noDoneCount = 0; //未完成个数

            bool isUseSetting = true;

            int isBack = 0;

            string newKeyValue = string.Empty;

            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string status = "";
            //更改申请信息状态
            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out status, "入厂许可审查", entity.OUTENGINEERID,false,entity.FLOWID);


            JArray arr = new JArray();

            if (!recordData.IsEmpty())
            {
                arr = (JArray)JsonConvert.DeserializeObject(recordData);
            }

            //审查状态下更新审查内容
            if (state == "1")
            {
                //只更新审查内容
                for (int i = 0; i < arr.Count(); i++)
                {
                    string id = arr[i]["id"].ToString();  //主键
                    string result = arr[i]["result"].ToString(); //结果
                    string people = arr[i]["people"].ToString(); //选择的人员
                    string peopleid = arr[i]["peopleid"].ToString(); //选择的人员
                    string signpic = string.IsNullOrWhiteSpace(arr[i]["signpic"].ToString()) ? "" : HttpUtility.UrlDecode(arr[i]["signpic"].ToString()).Replace("../..", "");
                    if (!string.IsNullOrEmpty(id))
                    {
                        var scEntity = investigatedtrecordbll.GetEntity(id); //审查内容项
                        scEntity.INVESTIGATERESULT = result;
                        if (result == "未完成") { noDoneCount += 1; } //存在未完成的则累加
                        scEntity.INVESTIGATEPEOPLE = people;
                        scEntity.INVESTIGATEPEOPLEID = peopleid;
                        scEntity.SIGNPIC = HttpUtility.UrlDecode(signpic);
                        //更新当前流程进行中的审查内容
                        investigatedtrecordbll.SaveForm(id, scEntity);
                    }
                }
                //退回操作(审查退回)
                if (noDoneCount > 0)
                {
                    entity.FLOWDEPT = " ";
                    entity.FLOWDEPTNAME = " ";
                    entity.FLOWROLE = " ";
                    entity.FLOWROLENAME = " ";
                    entity.FLOWID = " ";
                    entity.INVESTIGATESTATE = "0"; //更改状态为登记状态
                    entity.FLOWNAME = "";

                    isBack = 1; //审查退回
                    var applyUser = new UserBLL().GetEntity(entity.APPLYPEOPLEID);
                    if (applyUser != null)
                    {
                        JPushApi.PushMessage(applyUser.Account, entity.CREATEUSERNAME, "WB012", entity.ID);
                    }
                    //  AddBackData(keyValue, out newKeyValue);
                }
                else
                {
                    if (null != mpcEntity)
                    {
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.FLOWID = mpcEntity.ID;
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB013", entity.ID);
                    }
                    entity.FLOWNAME = "审核中";
                    entity.INVESTIGATESTATE = "2"; //更改状态为审核
                }
            }
            else
            {
                //同意进行下一步
                if (aentity.AUDITRESULT == "0")
                {
                    //下一步流程不为空
                    if (null != mpcEntity)
                    {
                        entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                        entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                        entity.FLOWROLE = mpcEntity.CHECKROLEID;
                        entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                        entity.FLOWID = mpcEntity.ID;
                        DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                        var userAccount = dt.Rows[0]["account"].ToString();
                        var userName = dt.Rows[0]["realname"].ToString();
                        JPushApi.PushMessage(userAccount, userName, "WB013", entity.ID);
                    }
                    else
                    {
                        entity.FLOWDEPT = " ";
                        entity.FLOWDEPTNAME = " ";
                        entity.FLOWROLE = " ";
                        entity.FLOWROLENAME = " ";
                        entity.FLOWID = " ";
                        entity.FLOWNAME = "已完结";
                        entity.INVESTIGATESTATE = "3"; //更改状态为完结状态
                    }

                    //添加审核记录
                    aentity.APTITUDEID = keyValue; //关联id 
                    aentity.AUDITSIGNIMG = HttpUtility.UrlDecode(aentity.AUDITSIGNIMG);
                    aentity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
                    aptitudeinvestigateauditbll.SaveForm("", aentity);
                }
                else  //退回到申请人 (审核退回)
                {
                    entity.FLOWDEPT = " ";
                    entity.FLOWDEPTNAME = " ";
                    entity.FLOWROLE = " ";
                    entity.FLOWROLENAME = " ";
                    entity.FLOWID = " ";
                    entity.INVESTIGATESTATE = "0"; //更改状态为登记状态
                    entity.FLOWNAME = "";
                    isBack = 2; //审核退回
                    var applyUser = new UserBLL().GetEntity(entity.APPLYPEOPLEID);
                    if (applyUser != null)
                    {
                        JPushApi.PushMessage(applyUser.Account, entity.CREATEUSERNAME, "WB012", entity.ID);
                    }
                    //  AddBackData(keyValue, out newKeyValue);  //添加历史记录
                }
            }
            //更改入厂许可申请单
            intromissionbll.SaveForm(keyValue, entity);

            //退回操作
            if (isBack > 0)
            {
                //添加历史记录
                AddBackData(keyValue, out newKeyValue);

                //审核退回
                if (isBack > 1)
                {
                    //获取当前业务对象的所有历史审核记录
                    var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                    //批量更新审核记录关联ID
                    foreach (AptitudeinvestigateauditEntity mode in shlist)
                    {
                        mode.APTITUDEID = newKeyValue; //对应新的关联ID
                        aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                    }
                    //添加审核记录
                    aentity.APTITUDEID = newKeyValue; //关联id 
                    aentity.AUDITSIGNIMG = HttpUtility.UrlDecode(aentity.AUDITSIGNIMG);
                    aentity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
                    aptitudeinvestigateauditbll.SaveForm("", aentity);
                }
            }

            return Success("操作成功。");
        }
        #endregion

        #region 退回添加到历史记录信息
        /// <summary>
        /// 退回添加到历史记录信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="arr"></param>
        public void AddBackData(string keyValue, out string newKeyValue)
        {
            //退回的同时保存原始的申请记录
            var dentity = intromissionbll.GetEntity(keyValue); //原始记录
            IntromissionHistoryEntity hentity = new IntromissionHistoryEntity();
            hentity.CREATEUSERID = dentity.CREATEUSERID;
            hentity.CREATEUSERDEPTCODE = dentity.CREATEUSERDEPTCODE;
            hentity.CREATEUSERORGCODE = dentity.CREATEUSERORGCODE;
            hentity.CREATEDATE = dentity.CREATEDATE;
            hentity.CREATEUSERNAME = dentity.CREATEUSERNAME;
            hentity.MODIFYDATE = dentity.MODIFYDATE;
            hentity.MODIFYUSERID = dentity.MODIFYUSERID;
            hentity.MODIFYUSERNAME = dentity.MODIFYUSERNAME;
            hentity.OUTENGINEERID = dentity.OUTENGINEERID;
            hentity.INTROMISSIONID = dentity.ID;
            hentity.APPLYPEOPLEID = dentity.APPLYPEOPLEID;
            hentity.APPLYPEOPLE = dentity.APPLYPEOPLE;
            hentity.APPLYTIME = dentity.APPLYTIME;
            hentity.INVESTIGATESTATE = dentity.INVESTIGATESTATE;
            hentity.REMARK = dentity.REMARK;
            hentity.FLOWDEPTNAME = dentity.FLOWDEPTNAME;
            hentity.FLOWDEPT = dentity.FLOWDEPT;
            hentity.FLOWROLENAME = dentity.FLOWROLENAME;
            hentity.FLOWROLE = dentity.FLOWROLE;
            hentity.FLOWNAME = dentity.FLOWNAME;
            hentity.FLOWID = dentity.FLOWNAME;
            intromissionhistorybll.SaveForm("", hentity);

            newKeyValue = hentity.ID;

            //更新审查记录单关联ID
            InvestigateRecordEntity irEntity = investigaterecordbll.GetEntityByIntroKey(keyValue); //审查记录单
            if (null != irEntity)
            {
                irEntity.INTOFACTORYID = newKeyValue;
                irEntity.INVESTIGATETYPE = "1"; //历史记录标识
                investigaterecordbll.SaveForm(irEntity.ID, irEntity);
            }
        }
        #endregion

        #region 导出入厂许可审批表
        
        /// <summary>
        /// 入厂许可审批表
        /// </summary>
        /// <param name="keyValue">入厂许可审批ID</param>
        /// <returns></returns>
        public ActionResult ExportWord(string keyValue)
        {
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象

            string fileName = "入厂许可审批表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == user.OrganizeCode && x.ModuleCode == "RCXK").ToList();
            var tempEntity = tempconfig.FirstOrDefault();
            string tempPath = "";
            if (tempconfig.Count > 0)
            {
                if (tempEntity != null)
                {
                    switch (tempEntity.ProessMode)
                    {
                        case "TY"://通用处理方式
                            tempPath = "Resource\\ExcelTemplate\\入厂许可审批表.doc";
                            break;
                        case "HJB":
                            tempPath = "Resource\\ExcelTemplate\\黄金埠入厂许可审批表.doc";
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                tempPath = "Resource\\ExcelTemplate\\入厂许可审批表.doc";
            }
            string strDocPath = Request.PhysicalApplicationPath + tempPath;
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DataTable dt = new DataTable("B");
            dt.Columns.Add("OutSourcingDept"); //外包单位
            dt.Columns.Add("OutSourcingProject"); //工程名称
            dt.Columns.Add("ApplyNo"); //申请编号
            dt.Columns.Add("ProjectManager"); //项目经理和电话
            dt.Columns.Add("ContractName"); //合同名称
            dt.Columns.Add("ContractNo"); //合同编号
            for (int i = 1; i < 16; i++)
            {
                dt.Columns.Add("Result" + i);
            }
            dt.Columns.Add("approveidea1"); 
            dt.Columns.Add("approveperson1"); 
            dt.Columns.Add("approvedate1"); 
            dt.Columns.Add("approveidea2"); 
            dt.Columns.Add("approveperson2"); 
            dt.Columns.Add("approvedate2"); 


            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("ApproveDate");
            dt1.Columns.Add("ApprovePerson");
            dt1.Columns.Add("ApproveIdea");
            dt1.Columns.Add("ApproveContent");
            DataRow row = dt.NewRow();
            //外包工程记录
            DataTable sdt = intromissionbll.GetOutSourcingProjectByIntromId(keyValue);
            if (sdt.Rows.Count == 1)
            {
                row["OutSourcingDept"] = sdt.Rows[0]["fullname"].ToString(); //外包单位
                row["OutSourcingProject"] = sdt.Rows[0]["engineername"].ToString(); //工程名称
                row["ApplyNo"] = sdt.Rows[0]["applyno"].ToString();
                row["ProjectManager"] = sdt.Rows[0]["engineerletpeople"].ToString() + "    " + sdt.Rows[0]["engineerletpeoplephone"].ToString();
                row["ContractName"] = sdt.Rows[0]["engineername"].ToString() + "合同";
                var engineer = Outsouringengineernll.GetEntity(sdt.Rows[0]["outengineerid"].ToString());
                var compact = new CompactBLL().GetListByProjectId(sdt.Rows[0]["outengineerid"].ToString()).OrderByDescending(t => t.CREATEDATE).FirstOrDefault();
                row["ContractNo"] = compact == null ? "" : compact.COMPACTNO;
                //审核记录
                List<AptitudeinvestigateauditEntity> list = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                int num = 1;
                for (int i = 0; i < list.Count; i++)
                {
                    DataRow row1 = dt1.NewRow();
                    var filepath = (Server.MapPath("~/") + list[i].AUDITSIGNIMG.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row1["ApprovePerson"] = filepath;
                    }
                    else
                    {
                        row1["ApprovePerson"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    //row1["ApprovePerson"] = Server.MapPath("~/") + list[i].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();

                    row1["ApproveIdea"] = !string.IsNullOrEmpty(list[i].AUDITOPINION) ? list[i].AUDITOPINION.ToString() : "";
                    DateTime ApproveDate = list[i].AUDITTIME.Value;
                    row1["ApproveDate"] = ApproveDate.Year.ToString() + "年" + ApproveDate.Month.ToString() + "月" + ApproveDate.Day.ToString() + "日";

                    if (i == 0)
                    {
                        row1["ApproveContent"] = "项目所在部门接收意见";
                    }
                    else if (i == list.Count - 1)
                    {
                        row1["ApproveContent"] = "分管领导意见";
                    }
                    else
                    {
                        row1["ApproveContent"] = list[i].AUDITDEPT + "审查意见";
                    }
                    dt1.Rows.Add(row1);
                }
                if (list.Count > 0)
                {
                    row["approveidea1"] = !string.IsNullOrEmpty(list[0].AUDITOPINION) ? list[0].AUDITOPINION.ToString() : "";
                    var filepath = (Server.MapPath("~/") + list[0].AUDITSIGNIMG.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson1"] = filepath;
                    }
                    else
                    {
                        row["approveperson1"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    DateTime ApproveDate = list[0].AUDITTIME.Value;
                    row["approvedate1"] = ApproveDate.Year.ToString() + "年" + ApproveDate.Month.ToString() + "月" + ApproveDate.Day.ToString() + "日";
                }
                if (list.Count > 1)
                {
                    row["approveidea2"] = !string.IsNullOrEmpty(list[1].AUDITOPINION) ? list[1].AUDITOPINION.ToString() : "";
                    var filepath = (Server.MapPath("~/") + list[1].AUDITSIGNIMG.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                    if (System.IO.File.Exists(filepath))
                    {
                        row["approveperson2"] = filepath;
                    }
                    else
                    {
                        row["approveperson2"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    DateTime ApproveDate = list[1].AUDITTIME.Value;
                    row["approvedate2"] = ApproveDate.Year.ToString() + "年" + ApproveDate.Month.ToString() + "月" + ApproveDate.Day.ToString() + "日";
                }
                //foreach (AptitudeinvestigateauditEntity entity in list)
                //{
                //    DataRow row1 = dt1.NewRow();
                //    row1["ApprovePerson"] = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();

                //    row1["ApproveIdea"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                //    DateTime ApproveDate = entity.AUDITTIME.Value;
                //    row1["ApproveDate"] = ApproveDate.Year.ToString() + "年" + ApproveDate.Month.ToString() + "月" + ApproveDate.Day.ToString() + "日";
                //    num++;

                //}
            }
            
            doc.MailMerge.ExecuteWithRegions(dt1);
            DataTable dtCert = intromissionbll.GetDtRecordList(keyValue);//人员证书信息
            for (int i = 0; i < dtCert.Rows.Count; i++)
            {
                if (dtCert.Rows[i]["investigateresult"].ToString() == "无此项")
                {
                    dtCert.Rows[i]["investigateresult"] = "/";
                    dtCert.Rows[i]["signpic"] = Server.MapPath("~/content/Images/no_1.png");
                }
                else 
                {
                    if (string.IsNullOrWhiteSpace(dtCert.Rows[i]["signpic"].ToString()))
                    {
                        dtCert.Rows[i]["signpic"] = Server.MapPath("~/content/Images/no_1.png");
                    }
                    else {
                        if (System.IO.File.Exists((Server.MapPath("~/") + dtCert.Rows[i]["signpic"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString()))
                        {
                            dtCert.Rows[i]["signpic"] = (Server.MapPath("~/") + dtCert.Rows[i]["signpic"].ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        }
                        else {
                            dtCert.Rows[i]["signpic"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                    }
                }
                int temp = i + 1;
                if (dt.Columns.Contains("Result" + temp))
                {
                    row["Result" + temp] = dtCert.Rows[i]["investigateresult"].ToString() == "已完成" ? "有" : "无";
                }
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            dtCert.TableName = "Investigate";
            doc.MailMerge.ExecuteWithRegions(dtCert);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            return Success("导出成功!");
        }
        #endregion
    }

}