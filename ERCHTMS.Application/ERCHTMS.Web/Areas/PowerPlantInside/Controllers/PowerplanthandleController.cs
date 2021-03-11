using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using System.Linq;
using System;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using Aspose.Words;
using System.Data;
using BSFramework.Util.Extension;
using System.Web;
using System.Text;
using ERCHTMS.Busines.SystemManage;
using Aspose.Cells;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// 描 述：事故事件处理
    /// </summary>
    public class PowerplanthandleController : MvcControllerBase
    {
        private PowerplanthandleBLL powerplanthandlebll = new PowerplanthandleBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private PowerplanthandledetailBLL PowerplanthandledetailBLL = new PowerplanthandledetailBLL();
        private PowerplantreformBLL powerplantreformbll = new PowerplantreformBLL();
        private PowerplantcheckBLL powerplantcheckbll = new PowerplantcheckBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private UserBLL userbll = new UserBLL();


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
        /// 事故事件处理信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HandleForm()
        {
            return View();
        }

        /// <summary>
        /// 审核页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AppForm()
        {
            return View();
        }

        /// <summary>
        /// 整改验收信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AppHandleForm()
        {
            return View();
        }

        /// <summary>
        /// 历史审核记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }

        /// <summary>
        /// 流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WorkFlow()
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
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,a.accidenteventname,
           b.itemname as accidenteventtype,c.itemname as accidenteventproperty,to_char(a.happentime,'yyyy-MM-dd HH24:mi') happentime,a.belongdept,a.issaved,a.applystate,a.flowdeptname,a.flowdept,a.flowrolename,a.flowrole,a.flowname,a.flowid ";
            pagination.p_tablename = @"BIS_POWERPLANTHANDLE a
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
              left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue";
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }


            var watch = CommonHelper.TimerStart();
            var data = powerplanthandlebll.GetPageList(pagination, queryJson);
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
            var data = powerplanthandlebll.GetList(queryJson);
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
            var data = powerplanthandlebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 根据业务id获取对应的审核记录列表 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetSpecialAuditList(string keyValue)
        {
            var data = aptitudeinvestigateauditbll.GetAuditList(keyValue).Where(t => t.Disable == "0").OrderByDescending(x => x.AUDITTIME).ToList();
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="keyValue">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetHistoryAuditList(string keyValue)
        {
            try
            {
                var data = aptitudeinvestigateauditbll.GetAuditList(keyValue).Where(t => t.Disable == "1").OrderByDescending(x => x.AUDITTIME).ToList();
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }

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
        [HandlerMonitor(6, "事故事件处理删除")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            powerplanthandlebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "事故事件处理保存")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PowerplanthandleEntity entity)
        {
            entity.IsSaved = 0;
            entity.ApplyState = 0;
            powerplanthandlebll.SaveForm(keyValue, entity);
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
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "事故事件处理提交")]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, PowerplanthandleEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "(事故事件处理记录)审核";

            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="outengineerid">工程Id</param>
            ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            string flowid = string.Empty;
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
            foreach (var item in powerList)
            {
                if (item.CHECKDEPTID == "-3" || item.CHECKDEPTID == "-1")
                {
                    item.CHECKDEPTID = curUser.DeptId;
                    item.CHECKDEPTCODE = curUser.DeptCode;
                    item.CHECKDEPTNAME = curUser.DeptName;
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
                entity.FlowDept = mpcEntity.CHECKDEPTID;
                entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                entity.FlowRole = mpcEntity.CHECKROLEID;
                entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                entity.IsSaved = 1; //标记已经从登记到审核阶段
                entity.ApplyState = 1; 
                entity.FlowId = mpcEntity.ID;
                entity.FlowName = mpcEntity.CHECKDEPTNAME + "审核中";
                //更新事故事件处理信息状态
                IList<PowerplanthandledetailEntity> HandleDetailList = PowerplanthandledetailBLL.GetHandleDetailList(keyValue);
                foreach (var item in HandleDetailList)
                {
                    item.ApplyState = 1;
                    PowerplanthandledetailBLL.SaveForm(item.Id, item);
                }
            }
            else  //为空则表示已经完成流程
            {
                entity.FlowDept = "";
                entity.FlowDeptName = "";
                entity.FlowRole = "";
                entity.FlowRoleName = "";
                entity.IsSaved = 1; //标记已经从登记到审核阶段
                entity.ApplyState = 3; 
                entity.FlowName = "";
                entity.FlowId = "";
                //更新事故事件处理信息状态
                IList<PowerplanthandledetailEntity> HandleDetailList = PowerplanthandledetailBLL.GetHandleDetailList(keyValue);
                foreach (var item in HandleDetailList)
                {
                    //当处理信息是指定责任人时候,登记审核完成后将单条处理信息状态更改为整改中，反之更改为签收中
                    if (item.IsAssignPerson == "0")
                    {
                        item.ApplyState = 3;
                    }
                    else
                    {
                        item.ApplyState = 6;
                    }
                    PowerplanthandledetailBLL.SaveForm(item.Id, item);
                }
            }
            powerplanthandlebll.SaveForm(keyValue, entity);
            powerplanthandlebll.UpdateApplyStatus(keyValue);
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
                aidEntity.FlowId = flowid;
                aidEntity.Disable = "0";
                if (null != mpcEntity)
                {
                    aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
                }
                else
                {
                    aidEntity.REMARK = "7";
                }
                aidEntity.AUDITDEPTID = curUser.DeptId;
                aidEntity.AUDITDEPT = curUser.DeptName;
                aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            }

            return Success("操作成功!");
        }


        /// <summary>
        /// 登记的内容提交到审核或者结束（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="aentity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(8, "事故事件审核提交")]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "(事故事件处理记录)审核";

            PowerplanthandleEntity entity = powerplanthandlebll.GetEntity(keyValue);
            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="createdeptid">创建人部门ID</param>
            ManyPowerCheckEntity mpcEntity = powerplanthandlebll.CheckAuditPower(curUser, out state, moduleName, new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).DepartmentId);


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
            aidEntity.FlowId = entity.FlowId;
            aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
            aidEntity.Disable = "0";
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

            #region  //保存事故事件处理记录
            //审核通过
            if (aentity.AUDITRESULT == "0")
            {
                //0表示流程未完成，1表示流程结束
                if (null != mpcEntity)
                {
                    entity.FlowDept = mpcEntity.CHECKDEPTID;
                    entity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                    entity.FlowRole = mpcEntity.CHECKROLEID;
                    entity.FlowRoleName = mpcEntity.CHECKROLENAME;
                    entity.IsSaved = 1;
                    entity.ApplyState = 1;
                    entity.FlowId = mpcEntity.ID;
                    entity.FlowName = mpcEntity.CHECKDEPTNAME + "审核中";
                }
                else
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.IsSaved = 1;
                    entity.ApplyState = 3;
                    entity.FlowName = "";
                    entity.FlowId = "";
                    //更新事故事件处理信息状态
                    IList<PowerplanthandledetailEntity> HandleDetailList = PowerplanthandledetailBLL.GetHandleDetailList(keyValue);
                    foreach (var item in HandleDetailList)
                    {
                        //当处理信息是指定责任人时候,登记审核完成后将单条处理信息状态更改为整改中，反之更改为签收中
                        if (item.IsAssignPerson == "0")
                        {
                            item.ApplyState = 3;
                        }
                        else
                        {
                            item.ApplyState = 6;
                        }
                        PowerplanthandledetailBLL.SaveForm(item.Id, item);
                    }
                }
            }
            else //审核不通过 
            {
                entity.FlowDept = "";
                entity.FlowDeptName = "";
                entity.FlowRole = "";
                entity.FlowRoleName = "";
                entity.ApplyState = 0; //处于登记阶段
                entity.IsSaved = 0; //是否完成状态赋值为未完成
                entity.FlowName = "";
                entity.FlowId = "";
                //更新事故事件处理信息状态
                IList<PowerplanthandledetailEntity> HandleDetailList = PowerplanthandledetailBLL.GetHandleDetailList(keyValue);
                foreach (var item in HandleDetailList)
                {
                    item.ApplyState = 0;
                    PowerplanthandledetailBLL.SaveForm(item.Id, item);
                }

            }
            //更新事故事件基本状态信息
            powerplanthandlebll.SaveForm(keyValue, entity);
            powerplanthandlebll.UpdateApplyStatus(keyValue);
            #endregion

            #region    //审核不通过
            if (aentity.AUDITRESULT == "1")
            {

                //获取当前业务对象的所有审核记录
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //批量更新审核记录关联ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.Disable = "1";
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
            }
            #endregion

            return Success("操作成功!");
        }


        /// <summary>
        /// 导出事故事件整改验收表
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出事故事件整改验收表")]
        public ActionResult ExportPowerPlantHandleInfo(string keyValue)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                //报告对象

                string fileName = "事故事件整改验收表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                string strDocPath = Request.PhysicalApplicationPath + @"Resource\ExcelTemplate\事故事件整改验收表.docx";
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                DocumentBuilder builder = new DocumentBuilder(doc);
                DataTable dt = new DataTable();
                dt.Columns.Add("AccidentEventName"); //事故/事件名称
                dt.Columns.Add("AccidentEventType"); //事故/事件类型
                dt.Columns.Add("AccidentEventProperty"); //事故/事件性质
                dt.Columns.Add("HappenTime"); //发生时间
                dt.Columns.Add("BelongDept"); //所属部门
                dt.Columns.Add("SituationIntroduction"); //情况简述
                dt.Columns.Add("ReasonAndProblem"); //原因及存在问题
                DataRow row = dt.NewRow();


                //事故事件处理记录
                PowerplanthandleEntity powerplanthandleentity = powerplanthandlebll.GetEntity(keyValue);
                row["AccidentEventName"] = powerplanthandleentity.AccidentEventName;
                row["AccidentEventType"] = dataitemdetailbll.GetItemName("AccidentEventType", powerplanthandleentity.AccidentEventType);
                row["AccidentEventProperty"] = dataitemdetailbll.GetItemName("AccidentEventProperty", powerplanthandleentity.AccidentEventProperty);
                row["HappenTime"] = powerplanthandleentity.HappenTime.IsEmpty() ? "" : Convert.ToDateTime(powerplanthandleentity.HappenTime).ToString("yyyy-MM-dd");
                row["BelongDept"] = powerplanthandleentity.BelongDept;

                row["SituationIntroduction"] = powerplanthandleentity.SituationIntroduction;
                row["ReasonAndProblem"] = powerplanthandleentity.ReasonAndProblem;
                dt.Rows.Add(row);
                doc.MailMerge.Execute(dt);
                //审核记录
                List<AptitudeinvestigateauditEntity> list = aptitudeinvestigateauditbll.GetAuditList(keyValue).Where(t => t.Disable == "0").OrderByDescending(x => x.AUDITTIME).ToList();
                DataTable dtAptitud = new DataTable();
                dtAptitud.TableName = "U";
                dtAptitud.Columns.Add("ApproveIdea");
                dtAptitud.Columns.Add("ApprovePerson");
                dtAptitud.Columns.Add("Person");
                dtAptitud.Columns.Add("ApproveDate");
                foreach (var item in list)
                {
                    DataRow dtrow = dtAptitud.NewRow();
                    dtrow["ApproveIdea"] = item.AUDITOPINION;
                    dtrow["ApprovePerson"] = item.AUDITPEOPLE;
                    dtrow["Person"] = item.AUDITSIGNIMG.IsEmpty() ? Server.MapPath("~/content/Images/no_1.png") : System.IO.File.Exists(Server.MapPath("~/") + item.AUDITSIGNIMG.ToString().Replace("../../", "").ToString()) ? Server.MapPath("~/") + item.AUDITSIGNIMG.ToString().Replace("../../", "").ToString() : Server.MapPath("~/content/Images/no_1.png");
                    dtrow["ApproveDate"] = item.AUDITTIME.IsEmpty() ? "" : Convert.ToDateTime(item.AUDITTIME).ToString("yyyy-MM-dd");
                    dtAptitud.Rows.Add(dtrow);
                }
                doc.MailMerge.ExecuteWithRegions(dtAptitud);

                builder.MoveToBookmark("ReformAndCheck");
                StringBuilder html = new StringBuilder();
                html.Append("<table border='1' cellspacing='0' width='600'>");
                //整改信息
                IList<PowerplantreformEntity> reformlist = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleId == keyValue && t.Disable == 0).ToList();
                foreach (var item in reformlist)
                {
                    html.Append("<tr><td ><b>整改措施</b></td><td colspan='3'>" + item.RectificationMeasures + "</td></tr>");
                    html.Append("<tr><td><b>整改责任人</b></td><td>" + item.RectificationPerson + "</td><td><b>整改期限</b></td><td>" + (item.RectificationTime.IsEmpty() ? "" : Convert.ToDateTime(item.RectificationTime).ToString("yyyy-MM-dd")) + "</td></tr>");
                    html.Append("<tr><td><b>整改情况描述</b></td><td colspan='3'>" + item.RectificationSituation + "</td></tr>");
                    html.Append(@"<tr><td><b>整改责任人签名</b></td><td><img src='" + 
                        (item.RectificationPersonSignImg.IsEmpty() ? Server.MapPath("~/content/Images/no_1.png") : System.IO.File.Exists(Server.MapPath("~/") + item.RectificationPersonSignImg.ToString().Replace("../../", "").ToString()) ? 
                        Server.MapPath("~/") + item.RectificationPersonSignImg.ToString().Replace("../../", "").ToString() : Server.MapPath("~/content/Images/no_1.png"))
                        + "'></img></td><td ><b>整改完成时间</b></td><td>" + (item.RectificationEndTime.IsEmpty() ? "" : Convert.ToDateTime(item.RectificationEndTime).ToString("yyyy-MM-dd")) + "</td></tr>");
                    IList<PowerplantcheckEntity> checklist = powerplantcheckbll.GetList("").Where(t => t.PowerPlantReformId == item.Id && t.Disable == 0).ToList();
                    html.Append("<tr><td><b>验收意见</b></td><td ><b>验收人签名</b></td><td><b>验收人所属部门</b></td><td><b>验收时间</b></td></tr>");
                    foreach (var temp in checklist)
                    {
                        html.Append(@"<tr><td>" + temp.AuditOpinion + "</td><td><img src='" +
                            (temp.AuditSignImg.IsEmpty() ? Server.MapPath("~/content/Images/no_1.png") : System.IO.File.Exists(Server.MapPath("~/") + temp.AuditSignImg.ToString().Replace("../../", "").ToString()) ?
                        Server.MapPath("~/") + temp.AuditSignImg.ToString().Replace("../../", "").ToString() : Server.MapPath("~/content/Images/no_1.png"))
                            + "'></img></td><td>" + temp.AuditDept + "</td><td>" + temp.AuditTime + "</td></tr>");
                    }
                }
                html.Append("</table>");
                builder.InsertHtml(html.ToString());
                doc.MailMerge.DeleteFields();

                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
                return Success("导出成功!");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

            
        }
        #endregion

        #region 导出事故事件闭环整改情况汇总表
        /// <summary>
        /// 导出事故事件闭环整改情况汇总表
        /// </summary>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出事故事件闭环整改情况汇总表")]
        public ActionResult ExportPowerPlantList(string queryJson)
        {
            try
            {
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,a.accidenteventname,
                b.itemname as accidenteventtype,c.itemname as accidenteventproperty,to_char(a.happentime,'yyyy-MM-dd HH24:mi') happentime,a.belongdept,a.issaved,a.applystate,a.flowdeptname,a.flowdept,a.flowrolename,a.flowrole,a.flowname,a.flowid ";
                pagination.p_tablename = @"BIS_POWERPLANTHANDLE a
                left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
                (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
                  left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
                (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue";
                pagination.conditionJson = "1=1 ";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    //根据当前用户对模块的权限获取记录
                    string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "createuserdeptcode", "createuserorgcode");
                    if (!string.IsNullOrEmpty(where))
                    {
                        pagination.conditionJson += " and " + where;
                    }
                }
                pagination.rows = 10000;
                pagination.page = 1;
                string fileUrl = "~/Resource/ExcelTemplate/事故事件闭环整改情况汇总表.xlsx";
                DataTable data = powerplanthandlebll.GetPageList(pagination, queryJson);
                Workbook wb = new Workbook();
                wb.Open(Server.MapPath(fileUrl));
                Worksheet sheet = wb.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                string fielname = "事故事件闭环整改情况汇总表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //添加表头
                sheet.Cells[0, 0].PutValue("序号");
                cells.SetColumnWidth(0, 10);
                sheet.Cells[0, 1].PutValue("事故/事件名称");
                cells.SetColumnWidth(1, 30);
                sheet.Cells[0, 2].PutValue("发生时间");
                cells.SetColumnWidth(2, 20);
                sheet.Cells[0, 3].PutValue("原因及暴露问题");
                cells.SetColumnWidth(3, 30);
                sheet.Cells[0, 4].PutValue("整改(防范)措施");
                cells.SetColumnWidth(4, 30);
                sheet.Cells[0, 5].PutValue("整改责任人");
                cells.SetColumnWidth(5, 20);
                sheet.Cells[0, 6].PutValue("整改责任部门");
                cells.SetColumnWidth(6, 20);
                sheet.Cells[0, 7].PutValue("整改期限");
                cells.SetColumnWidth(7, 20);
                sheet.Cells[0, 8].PutValue("整改情况描述");
                cells.SetColumnWidth(8, 30);
                List<ManyPowerCheckEntity> ManyPowerCheckList = manypowercheckbll.GetList(user.OrganizeCode, "事故事件处理记录-验收");
                for (int i = 0; i < ManyPowerCheckList.Count; i++)
                {
                    if (i <= 2)
                    {
                        sheet.Cells[0, 9 + i].PutValue(ManyPowerCheckList[i].FLOWNAME);
                        cells.SetColumnWidth(9 + i, 20);
                    }
                }
                int lastcol = ManyPowerCheckList.Count > 3 ? 12 : 9 + ManyPowerCheckList.Count;
                sheet.Cells[0, lastcol].PutValue("流程状态");
                cells.SetColumnWidth(lastcol, 20);

                int extentrow = 0; //扩展行数,当一条事故事件记录对应多条处理记录时,该值进行累加
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    //添加事故事件基础信息
                    sheet.Cells[1 + i + extentrow, 0].PutValue(i + 1);
                    sheet.Cells[1 + i + extentrow, 1].PutValue(data.Rows[i]["accidenteventname"].ToString());
                    sheet.Cells[1 + i + extentrow, 2].PutValue(Convert.ToDateTime(data.Rows[i]["happentime"]).ToString("yyyy-MM-dd"));
                    //添加事故事件处理验收整改信息
                    var powerhandledetaillist = PowerplanthandledetailBLL.GetList("").Where(t => t.PowerPlantHandleId == data.Rows[i]["id"].ToString()).ToList();
                    
                    for (int j = 0; j < powerhandledetaillist.Count; j++)
                    {
                        sheet.Cells[1 + i + j + extentrow, 3].PutValue(powerhandledetaillist[j].RectificationMeasures); //原因及暴露问题
                        sheet.Cells[1 + i + j + extentrow, 4].PutValue(powerhandledetaillist[j].RectificationMeasures); //整改(防范)措施
                        sheet.Cells[1 + i + j + extentrow, 5].PutValue(powerhandledetaillist[j].RectificationDutyPerson); //整改责任人
                        sheet.Cells[1 + i + j + extentrow, 6].PutValue(powerhandledetaillist[j].RectificationDutyDept); //整改责任部门
                        sheet.Cells[1 + i + j + extentrow, 7].PutValue(Convert.ToDateTime(powerhandledetaillist[j].RectificationTime).ToString("yyyy-MM-dd")); //整改期限
                        var powerplantreform = powerplantreformbll.GetList("").Where(t => t.PowerPlantHandleDetailId == powerhandledetaillist[j].Id && t.Disable == 0).FirstOrDefault(); //整改信息
                        if (!powerplantreform.IsEmpty())
                        {
                            sheet.Cells[1 + i + j + extentrow, 8].PutValue(powerplantreform.RectificationSituation); //整改情况描述
                        }
                        var powerplantchecklist = powerplantcheckbll.GetList("").Where(t => t.PowerPlantHandleDetailId == powerhandledetaillist[j].Id && t.Disable == 0).ToList();  //验收信息
                        for (int k = 0; k < powerplantchecklist.Count; k++)
                        {
                            if (k < ManyPowerCheckList.Count)
                            {
                                sheet.Cells[1 + i + j + extentrow, 9 + k].PutValue(powerplantchecklist[k].AuditPeople); //验收人
                            }
                        }
                        string ApplyState = "申请中";
                        switch (powerhandledetaillist[j].ApplyState)
                        {
                            case 0:
                                ApplyState = "申请中";
                                break;
                            case 1:
                                ApplyState = "审核中";
                                break;
                            case 2:
                                ApplyState = "审核不通过";
                                break;
                            case 3:
                                ApplyState = "整改中";
                                break;
                            case 4:
                                ApplyState = "验收中";
                                break;
                            case 5:
                                ApplyState = "已完成";
                                break;
                            case 6:
                                ApplyState = "签收中";
                                break;
                            default:
                                break;
                        }
                        sheet.Cells[1 + i + j + extentrow, lastcol].PutValue(ApplyState); //流程状态
                    }
                    cells.Merge(1 + i + extentrow, 0, powerhandledetaillist.Count, 1);//序号合并
                    cells.Merge(1 + i + extentrow, 1, powerhandledetaillist.Count, 1);//事故事件名称合并
                    cells.Merge(1 + i + extentrow, 2, powerhandledetaillist.Count, 1);//发生事件合并
                    extentrow += powerhandledetaillist.Count - 1;
                }

                wb.Save(Server.UrlEncode(fielname), Aspose.Cells.FileFormatType.Excel2003, Aspose.Cells.SaveType.OpenInBrowser, resp);
                return Success("操作成功!");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion

        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetFlowList(string id)
        {
            Flow flow = new Flow();
            List<lines> lines = new List<lines>();
            List<nodes> nodes = new List<nodes>();
            PowerplanthandleEntity root = powerplanthandlebll.GetEntity(id);
            string deptname = departmentBLL.GetEntityByCode(root.CreateUserDeptCode).FullName;
            string deptid= departmentBLL.GetEntityByCode(root.CreateUserDeptCode).DepartmentId;
            nodes startnode = new nodes();
            startnode.id = root.Id;
            startnode.left = 400;
            startnode.top = 30;
            startnode.name = "创建任务<br />(" + deptname + ")";
            startnode.type = "startround";
            startnode.setInfo = new setInfo
            {
                Taged = 1,
                NodeDesignateData = new List<NodeDesignateData>{
                 new NodeDesignateData{
                   creatdept=deptname,
                   createuser=root.CreateUserName,
                   createdate=root.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                   status="已处理"
                 }
                }
            };
            nodes.Add(startnode);
            #region (事故事件处理记录)审核节点
            DataTable dtNodes = powerplanthandlebll.GetAuditInfo(id, "(事故事件处理记录)审核");
            if (dtNodes != null && dtNodes.Rows.Count > 0)
            {
                for (int i = 0; i < dtNodes.Rows.Count; i++)
                {
                    DataRow dr = dtNodes.Rows[i];
                    nodes node = new nodes();
                    node.alt = true;
                    node.isclick = false;
                    node.css = "";
                    node.id = dr["id"].ToString(); //主键
                    node.img = "";
                    node.name = dr["flowname"].ToString();
                    node.type = "stepnode";
                    node.width = 150;
                    node.height = 60;
                    node.left = 400;
                    node.top = ((i + 1) * 100) + 30;
                    setInfo sinfo = new setInfo();
                    sinfo.NodeName = node.name;
                    //审核记录
                    if (dr["auditdept"] != null && !string.IsNullOrEmpty(dr["auditdept"].ToString()))
                    {
                        sinfo.Taged = 1;
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        DateTime auditdate;
                        DateTime.TryParse(dr["audittime"].ToString(), out auditdate);
                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                        nodedesignatedata.creatdept = dr["auditdept"].ToString();
                        nodedesignatedata.createuser = dr["auditpeople"].ToString();
                        nodedesignatedata.status = dr["auditresult"].ToString() == "0" ? "同意" : "不同意";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = dtNodes.Rows[i - 1]["flowname"].ToString();
                        }

                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        node.setInfo = sinfo;
                    }
                    else
                    {
                        if (root.FlowId == dr["id"].ToString())
                        {
                            sinfo.Taged = 0;
                        }
                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                        nodedesignatedata.createdate = "无";

                        //部门,人员
                        var checkDeptId = dr["checkdeptid"].ToString();
                        var checkremark = dr["remark"].ToString();
                        string type = checkremark != "1" ? "0" : "1";
                        if (checkDeptId == "-3")
                        {
                            checkDeptId = deptid;
                            nodedesignatedata.creatdept = deptname;
                        }
                        else
                        {
                            nodedesignatedata.creatdept = dr["checkdeptname"].ToString();
                        }
                        string userNames = powerplanthandlebll.GetUserName(checkDeptId, dr["checkrolename"].ToString()).Split('|')[0];
                        nodedesignatedata.createuser = !string.IsNullOrEmpty(userNames) ? userNames : "无";

                        nodedesignatedata.status = "无";
                        if (i == 0)
                        {
                            nodedesignatedata.prevnode = "无";
                        }
                        else
                        {
                            nodedesignatedata.prevnode = dtNodes.Rows[i - 1]["flowname"].ToString();
                        }

                        nodelist.Add(nodedesignatedata);
                        sinfo.NodeDesignateData = nodelist;
                        node.setInfo = sinfo;
                    }
                    nodes.Add(node);

                    lines line = new lines();
                    line.alt = true;
                    line.id = Guid.NewGuid().ToString();
                    line.from = i == 0 ? root.Id : dtNodes.Rows[i - 1]["id"].ToString();
                    line.to = dtNodes.Rows[i]["id"].ToString();
                    line.name = "";
                    line.type = "sl";
                    lines.Add(line);
                }
                
            }
            #endregion

            #region 事故事件整改节点
            DataTable dtReformNodes = powerplanthandlebll.GetReformInfo(id);
            if (dtReformNodes != null && dtReformNodes.Rows.Count > 0)
            {
                for (int i = 0; i < dtReformNodes.Rows.Count; i++)
                {
                    DataRow dr = dtReformNodes.Rows[i];
                    #region 签收节点
                    Boolean HaveSignNode = dr["isassignperson"].ToString() == "1" ? true : false;
                    if (dr["isassignperson"].ToString() == "1")
                    {
                        nodes signnode = new nodes();
                        signnode.alt = true;
                        signnode.isclick = false;
                        signnode.css = "";
                        signnode.id = dr["id"].ToString() + "-01"; //主键
                        signnode.img = "";
                        signnode.name = dr["signdeptname"].ToString() + "签收";
                        signnode.type = "stepnode";
                        signnode.width = 150;
                        signnode.height = 60;
                        signnode.left = i * 200 + 100;
                        signnode.top = ((dtNodes.Rows.Count + 1) * 100) + 30;
                        setInfo signsinfo = new setInfo();
                        signsinfo.NodeName = signnode.name;
                        //审核记录
                        if (dr["realsignpersonname"] != null && !string.IsNullOrEmpty(dr["realsignpersonname"].ToString()))
                        {
                            signsinfo.Taged = 1;
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            DateTime auditdate;
                            DateTime.TryParse(dr["realsigndate"].ToString(), out auditdate);
                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                            nodedesignatedata.creatdept = dr["realsignpersondept"].ToString();
                            nodedesignatedata.createuser = dr["realsignpersonname"].ToString();
                            nodedesignatedata.status = "已签收";
                            nodedesignatedata.prevnode = dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["flowname"].ToString() : root.Id;
                            nodelist.Add(nodedesignatedata);
                            signsinfo.NodeDesignateData = nodelist;
                            signnode.setInfo = signsinfo;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(root.FlowId))
                            {
                                signsinfo.Taged = 0;
                            }
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = "无";
                            nodedesignatedata.createuser = !dr["signpersonname"].IsEmpty() ? dr["signpersonname"].ToString() : "无";
                            nodedesignatedata.creatdept = !dr["signdeptname"].IsEmpty() ? dr["signdeptname"].ToString() : "无";
                            nodedesignatedata.status = "无";
                            nodedesignatedata.prevnode = dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["flowname"].ToString() : root.Id;
                            nodelist.Add(nodedesignatedata);
                            signsinfo.NodeDesignateData = nodelist;
                            signnode.setInfo = signsinfo;
                        }
                        nodes.Add(signnode);

                        lines signline = new lines();
                        signline.alt = true;
                        signline.id = Guid.NewGuid().ToString();
                        signline.from = dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["id"].ToString() : root.Id;
                        signline.to = signnode.id;
                        signline.name = "";
                        signline.type = "sl";
                        lines.Add(signline);
                    }
                    #endregion
                    #region 整改节点
                    if (!dr["rectificationdutyperson"].IsEmpty())
                    {
                        nodes node = new nodes();
                        node.alt = true;
                        node.isclick = false;
                        node.css = "";
                        node.id = dr["id"].ToString(); //主键
                        node.img = "";
                        node.name = dr["rectificationdutydept"].ToString() + "整改";
                        node.type = "stepnode";
                        node.width = 150;
                        node.height = 60;
                        node.left = i * 200 + 100;
                        node.top = HaveSignNode ? ((dtNodes.Rows.Count + 1) * 100) + 130 : ((dtNodes.Rows.Count + 1) * 100) + 30;
                        setInfo sinfo = new setInfo();
                        sinfo.NodeName = node.name;
                        //审核记录
                        if (dr["rectificationperson"] != null && !string.IsNullOrEmpty(dr["rectificationperson"].ToString()))
                        {
                            sinfo.Taged = 1;
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            DateTime auditdate;
                            DateTime.TryParse(dr["createdate"].ToString(), out auditdate);
                            nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                            nodedesignatedata.creatdept = departmentBLL.GetEntityByCode(dr["createuserdeptcode"].ToString()).FullName;
                            nodedesignatedata.createuser = dr["rectificationperson"].ToString();
                            nodedesignatedata.status = "已整改";
                            nodedesignatedata.prevnode = HaveSignNode ? (dr["signpersonname"].ToString().Length > 20 ? dr["signpersonname"].ToString().Substring(0, 20) + "..." + "签收" : dr["signpersonname"].ToString() + "签收") : (dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["flowname"].ToString() : root.Id);
                            nodelist.Add(nodedesignatedata);
                            sinfo.NodeDesignateData = nodelist;
                            node.setInfo = sinfo;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(root.FlowId))
                            {
                                sinfo.Taged = 0;
                            }
                            List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                            NodeDesignateData nodedesignatedata = new NodeDesignateData();
                            nodedesignatedata.createdate = "无";
                            //nodedesignatedata.createuser = !dr["rectificationdutyperson"].IsEmpty() ? dr["rectificationdutyperson"].ToString() : "无";
                            //nodedesignatedata.creatdept = !dr["rectificationdutydept"].IsEmpty() ? dr["rectificationdutydept"].ToString() : "无";
                            string approveuserid = dr["rectificationdutypersonid"].IsEmpty() ? "" : dr["rectificationdutypersonid"].ToString();
                            string[] accounts = userbll.GetUserTable(approveuserid.Split(',')).AsEnumerable().Select(e => e.Field<string>("ACCOUNT")).ToArray();
                            string accountstr = accounts.Length > 0 ? string.Join(",", accounts) + "," : "";
                            string outtransferuseraccount = dr["outtransferuseraccount"].IsEmpty() ? "" : dr["outtransferuseraccount"].ToString();//转交申请人
                            string intransferuseraccount = dr["intransferuseraccount"].IsEmpty() ? "" : dr["intransferuseraccount"].ToString();//转交接收人
                            string[] outtransferuseraccountlist = outtransferuseraccount.Split(',');
                            string[] intransferuseraccountlist = intransferuseraccount.Split(',');
                            foreach (var item in intransferuseraccountlist)
                            {
                                if (!item.IsEmpty() && !accountstr.Contains(item + ","))
                                {
                                    accountstr += (item + ",");//将转交接收人加入审核账号中
                                }
                            }
                            foreach (var item in outtransferuseraccountlist)
                            {
                                if (!item.IsEmpty() && accountstr.Contains(item + ","))
                                {
                                    accountstr = accountstr.Replace(item + ",", "");//将转交申请人从审核账号中移除
                                }
                            }

                            DataTable dtuser = userbll.GetUserTable(accountstr.Split(','));
                            string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                            string[] deptnames = dtuser.AsEnumerable().Select(d => d.Field<string>("deptname")).ToArray().GroupBy(t => t).Select(p => p.Key).ToArray();
                            nodedesignatedata.createuser = usernames.Length > 0 ? string.Join(",", usernames) : "无";
                            nodedesignatedata.creatdept = deptnames.Length > 0 ? string.Join(",", deptnames) : "无";
                            nodedesignatedata.status = "无";
                            nodedesignatedata.prevnode = HaveSignNode ? (dr["signpersonname"].ToString().Length > 20 ? dr["signpersonname"].ToString().Substring(0, 20) + "..." + "签收" : dr["signpersonname"].ToString() + "签收") : (dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["flowname"].ToString() : root.Id);
                            nodelist.Add(nodedesignatedata);
                            sinfo.NodeDesignateData = nodelist;
                            node.setInfo = sinfo;
                        }
                        nodes.Add(node);

                        lines line = new lines();
                        line.alt = true;
                        line.id = Guid.NewGuid().ToString();
                        line.from = HaveSignNode ? node.id + "-01" : (dtNodes.Rows.Count > 0 ? dtNodes.Rows[dtNodes.Rows.Count - 1]["id"].ToString() : root.Id); //当有签收节点时候 from节点为签收节点的ID
                        line.to = node.id;
                        line.name = "";
                        line.type = "sl";
                        lines.Add(line);
                        #region  事故事件验收节点
                        if (dr["rectificationperson"] != null && !string.IsNullOrEmpty(dr["rectificationperson"].ToString()))
                        {
                            DataTable dtCheckNodes = powerplanthandlebll.GetCheckInfo(dr["id"].ToString(), "事故事件处理记录-验收");
                            if (dtCheckNodes != null && dtCheckNodes.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtCheckNodes.Rows.Count; j++)
                                {
                                    DataRow drtemp = dtCheckNodes.Rows[j];
                                    nodes nodetemp = new nodes();
                                    nodetemp.alt = true;
                                    nodetemp.isclick = false;
                                    nodetemp.css = "";
                                    nodetemp.id = Guid.NewGuid().ToString(); //主键
                                    nodetemp.img = "";
                                    nodetemp.name = drtemp["flowname"].ToString();
                                    nodetemp.type = "stepnode";
                                    nodetemp.width = 150;
                                    nodetemp.height = 60;
                                    nodetemp.left = node.left;
                                    nodetemp.top = ((j + 1) * 100) + node.top;
                                    setInfo sinfotemp = new setInfo();
                                    sinfotemp.NodeName = nodetemp.name;
                                    //审核记录
                                    if (drtemp["auditdept"] != null && !string.IsNullOrEmpty(drtemp["auditdept"].ToString()))
                                    {
                                        sinfotemp.Taged = 1;
                                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        DateTime auditdate;
                                        DateTime.TryParse(drtemp["audittime"].ToString(), out auditdate);
                                        nodedesignatedata.createdate = auditdate.ToString("yyyy-MM-dd HH:mm");
                                        nodedesignatedata.creatdept = drtemp["auditdept"].ToString();
                                        nodedesignatedata.createuser = drtemp["auditpeople"].ToString();
                                        nodedesignatedata.status = drtemp["auditresult"].ToString() == "0" ? "同意" : "不同意";
                                        if (j == 0)
                                        {
                                            nodedesignatedata.prevnode = node.name;
                                        }
                                        else
                                        {
                                            nodedesignatedata.prevnode = dtCheckNodes.Rows[j - 1]["flowname"].ToString();
                                        }

                                        nodelist.Add(nodedesignatedata);
                                        sinfotemp.NodeDesignateData = nodelist;
                                        nodetemp.setInfo = sinfotemp;
                                    }
                                    else
                                    {
                                        if (drtemp["flowid"].ToString() == dr["flowid"].ToString())
                                        {
                                            sinfotemp.Taged = 0;
                                        }
                                        List<NodeDesignateData> nodelist = new List<NodeDesignateData>();
                                        NodeDesignateData nodedesignatedata = new NodeDesignateData();
                                        nodedesignatedata.createdate = "无";

                                        //部门,人员
                                        var checkDeptId = drtemp["checkdeptid"].ToString();
                                        var checkremark = drtemp["remark"].ToString();
                                        string type = checkremark != "1" ? "0" : "1";
                                        if (checkDeptId == "-3" || checkDeptId == "-1")
                                        {
                                            checkDeptId = dr["realreformdeptid"].ToString();
                                            nodedesignatedata.creatdept = dr["realreformdept"].ToString();
                                        }
                                        else
                                        {
                                            nodedesignatedata.creatdept = drtemp["checkdeptname"].ToString();
                                        }
                                        string userNames = powerplanthandlebll.GetUserName(checkDeptId, drtemp["checkrolename"].ToString()).Split('|')[0];
                                        nodedesignatedata.createuser = !string.IsNullOrEmpty(userNames) ? userNames : "无";

                                        nodedesignatedata.status = "无";
                                        if (j == 0)
                                        {
                                            nodedesignatedata.prevnode = node.name;
                                        }
                                        else
                                        {
                                            nodedesignatedata.prevnode = dtCheckNodes.Rows[j - 1]["flowname"].ToString();
                                        }

                                        nodelist.Add(nodedesignatedata);
                                        sinfotemp.NodeDesignateData = nodelist;
                                        nodetemp.setInfo = sinfotemp;
                                    }
                                    nodes.Add(nodetemp);

                                    lines linetemp = new lines();
                                    linetemp.alt = true;
                                    linetemp.id = Guid.NewGuid().ToString();
                                    linetemp.from = node.id;
                                    linetemp.to = nodetemp.id;
                                    linetemp.name = "";
                                    linetemp.type = "sl";
                                    lines.Add(linetemp);
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }

            }
            #endregion
            flow.nodes = nodes;
            flow.lines = lines;
            flow.title = "事故事件处理流程图";
            return Success("获取数据成功", flow);
        }
    }
}
