using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Collections.Generic;
using System.Data;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.OutsourcingProject;
using System.Linq;
using System;
using System.Web;
using Aspose.Words;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Entity.OutsourcingProject;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// 描 述：事故事件处理信息
    /// </summary>
    public class PowerplanthandledetailController : MvcControllerBase
    {
        private PowerplanthandledetailBLL powerplanthandledetailbll = new PowerplanthandledetailBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private UserBLL userbll = new UserBLL();
        private PowerplanthandleBLL powerplanthandlebll = new PowerplanthandleBLL();
        private PowerplantreformBLL powerplantreformbll = new PowerplantreformBLL();
        private PowerplantcheckBLL powerplantcheckbll = new PowerplantcheckBLL();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
            
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
            var data = powerplanthandledetailbll.GetList(queryJson);
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
            var data = powerplanthandledetailbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取事故事件处理信息列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="recid">关联事故事件处理记录ID</param>
        /// <returns></returns>
        public ActionResult GetHandleDetailListJson(Pagination pagination, string recid)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "a.ID";
                pagination.p_fields = "a.powerplanthandleid,a.rectificationmeasures,a.rectificationdutyperson,a.rectificationdutydept,a.APPLYSTATE,a.rectificationtime,b.id as PowerPlantReformId,a.reasonandproblem,'' as applystatename,a.signpersonname,e.outtransferuseraccount,e.intransferuseraccount,e.outtransferusername,e.intransferusername,a.flowdept,a.flowrole";
                pagination.p_tablename = @" bis_powerplanthandledetail a left join bis_powerplantreform b on a.id=b.powerplanthandledetailid and b.disable=0
                                            left join (select recid,flowid,outtransferuseraccount,intransferuseraccount,outtransferusername,intransferusername,row_number()  over(partition by recid,flowid order by createdate desc) as num from BIS_TRANSFERRECORD where disable=0) e on a.id=e.recid and e.num=1";
                pagination.conditionJson = string.Format("a.powerplanthandleid='{0}'", recid);
                pagination.sidx = "a.createdate";
                pagination.sord = "desc";
                var data = powerplanthandledetailbll.GetPageList(pagination, "");
                foreach (DataRow item in data.Rows)
                {
                    switch (item["applystate"].ToString())
                    {
                        case "0":
                            item["applystatename"] = "申请中";
                            break;
                        case "1":
                            item["applystatename"] = "审核中";
                            break;
                        case "2":
                            item["applystatename"] = "审核不通过";
                            break;
                        case "3":
                            string rectificationdutyperson = item["rectificationdutyperson"].ToString(); //整改负责人
                            string outtransferusername = item["outtransferusername"].IsEmpty() ? "" : item["outtransferusername"].ToString();//转交申请人
                            string intransferuseruser = item["intransferusername"].IsEmpty() ? "" : item["intransferusername"].ToString();//转交接收人
                            string[] outtransferusernamelist = outtransferusername.Split(',');
                            string[] intransferuseruserlist = intransferuseruser.Split(',');
                            foreach (var temp in intransferuseruserlist)
                            {
                                if (!temp.IsEmpty() && !rectificationdutyperson.Contains(temp + ","))
                                {
                                    rectificationdutyperson += (temp + ",");//将转交接收人加入整改人中
                                }
                            }
                            foreach (var temp in outtransferusernamelist)
                            {
                                if (!temp.IsEmpty() && rectificationdutyperson.Contains(temp + ","))
                                {
                                    rectificationdutyperson = rectificationdutyperson.Replace(temp + ",", "");//将转交申请人从整改人中移除
                                }
                            }
                            item["applystatename"] = (rectificationdutyperson.Length > 18 ? rectificationdutyperson.Substring(0, 18) + "..." : rectificationdutyperson.ToString()) + "整改中";
                            break;
                        case "4":
                            string[] deptlist = item["flowdept"].ToString().Split(',');
                            string[] rolelist = item["flowrole"].ToString().Split(',');
                            IList<UserEntity> userlist = userbll.GetUserListByDeptId("'" + string.Join("','", deptlist) + "'", "'" + string.Join("','", rolelist) + "'", true, "");
                            string username = "";
                            if (userlist.Count > 0)
                            {
                                foreach (var temp in userlist)
                                {
                                    username += temp.RealName + ",";
                                }
                                username = string.IsNullOrEmpty(username) ? "" : username.Substring(0, username.Length - 1);
                            }
                            item["applystatename"] = (username.Length > 18 ? username.Substring(0, 18) + "..." : username.ToString()) + "验收中";
                            break;
                        case "5":
                            item["applystatename"] = "已完成";
                            break;
                        case "6":
                            item["applystatename"] = (item["signpersonname"].ToString().Length > 18 ? item["signpersonname"].ToString().Substring(0, 18) + "..." : item["signpersonname"].ToString()) + "签收中";
                            break;
                        default:
                            break;
                    }
                    
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
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取签收人
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSignPerson(string deptid)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                string roles = dataitemdetailbll.GetItemValue(user.OrganizeId, "SignPersonRole");
                IList<UserEntity> userlist = new List<UserEntity>();
                if (string.IsNullOrEmpty(roles))
                {
                    userlist = userbll.GetUserListByRoleName("'" + deptid + "'", "'负责人','安全管理员','专工'", true, "");
                }
                else
                {
                    string[] rolelist = roles.Split('|');
                    userlist = userbll.GetUserListByDeptId("'" + deptid + "'", "'" + string.Join("','", rolelist) + "'", true, "");
                }
                string username = "";
                string userid = "";
                if (userlist.Count > 0)
                {
                    foreach (var item in userlist)
                    {
                        username += item.RealName + ",";
                        userid += item.UserId + ",";
                    }
                    username = string.IsNullOrEmpty(username) ? "" : username.Substring(0, username.Length - 1);
                    userid = string.IsNullOrEmpty(userid) ? "" : userid.Substring(0, userid.Length - 1);
                }
                return Success("获取数据成功", new { username = username, userid = userid });
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
        [HandlerMonitor(6, "事故事件处理信息删除")]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            try
            {
                powerplanthandledetailbll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandlerMonitor(5, "事故事件处理信息保存")]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, PowerplanthandledetailEntity entity)
        {
            try
            {
                powerplanthandledetailbll.SaveForm(keyValue, entity);
                if (entity.ApplyState == 3)
                {
                    powerplanthandlebll.UpdateApplyStatus(entity.PowerPlantHandleId);
                }
                return Success("操作成功。");
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }


        /// <summary>
        /// 导出事故事件处理信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "导出事故事件处理信息")]
        public ActionResult ExportSGSJCLXX(string keyValue)
        {
            try
            {
                var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
                var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == userInfo.OrganizeCode && x.ModuleCode == "SGSJCLJL").ToList();
                string tempPath = @"~/Resource/ExcelTemplate/事故事件导出模板-西塞山.doc";
                var tempEntity = tempconfig.FirstOrDefault();
                string fileName = "事故事件处理信息导出表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                if (tempconfig.Count > 0)
                {
                    if (tempEntity != null)
                    {
                        switch (tempEntity.ProessMode)
                        {
                            case "TY"://通用处理方式
                                tempPath = @"~/Resource/ExcelTemplate/事故事件导出模板-西塞山.doc";
                                break;
                            case "GDXY"://国电荥阳
                                tempPath = @"~/Resource/ExcelTemplate/事故事件导出模板_国电荥阳.doc";
                                break;
                            case "HRCB"://华润赤壁
                                tempPath = @"~/Resource/ExcelTemplate/事故事件导出模板-西塞山.doc";
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    tempPath = @"~/Resource/ExcelTemplate/事故事件导出模板-西塞山.doc";
                }
                ExportDataByCode(keyValue, tempPath, fileName);
                return Success("导出成功!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        private void ExportDataByCode(string keyValue, string tempPath, string fileName)
        {
            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
            string strDocPath = Server.MapPath(tempPath);
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DataTable dt = new DataTable("people");
            dt.Columns.Add("ApplyCode"); //编号
            dt.Columns.Add("CreateDate"); //填报日期
            dt.Columns.Add("AccidentEventName");  //事故事件名称
            dt.Columns.Add("HappenTime");  //发生时间
            dt.Columns.Add("BelongDept");  //责任归属
            dt.Columns.Add("AccidentEventProperty");  //性质
            dt.Columns.Add("SituationIntroduction");  //情况简述
            dt.Columns.Add("ReasonAndProblem");  //暴露原因及问题
            dt.Columns.Add("RectificationMeasures"); //防范措施及要求
            dt.Columns.Add("SignDeptName");  //发至单位
            dt.Columns.Add("RealSignPersonName");  //签收人
            dt.Columns.Add("ApproveDept");    //审核部门
            dt.Columns.Add("ApprovePerson");  //审核人
            dt.Columns.Add("RectificationPerson");  //整改负责人
            dt.Columns.Add("RectificationDutyDept"); //责任部门
            dt.Columns.Add("RectificationTime");  //完成期限
            dt.Columns.Add("RectificationSituation");  //措施完成情况
            dt.Columns.Add("RectificationPersonSignImg");  //措施完成责任人签名
            dt.Columns.Add("ReformDate");  //整改日期
            dt.Columns.Add("CheckPerson1");  //部门负责人
            dt.Columns.Add("CheckIdea1");  //验收意见1
            dt.Columns.Add("CheckIdea2");  //验收意见2
            dt.Columns.Add("CheckPerson2");   //验收人2
            dt.Columns.Add("CheckDate2");  //验收日期2
            dt.Columns.Add("CheckIdea3");  //验收意见3
            dt.Columns.Add("CheckPerson3");  //验收人3
            dt.Columns.Add("CheckDate3");  //验收日期3
            dt.Columns.Add("CheckDept3");  //验收部门3
            dt.Columns.Add("CheckIdea4");    //验收意见4
            dt.Columns.Add("CheckPerson4");  //验收人4
            dt.Columns.Add("CheckDate4");  //验收日期4
            dt.Columns.Add("CheckDept4");  //验收部门4

            HttpResponse resp = System.Web.HttpContext.Current.Response;
            string defaultimage = Server.MapPath("~/content/Images/no_1.png");
            DataRow row = dt.NewRow();
            PowerplanthandledetailEntity powerplanthandledetailentity = powerplanthandledetailbll.GetEntity(keyValue);
            PowerplanthandleEntity powerplanthandleentity = powerplanthandlebll.GetEntity(powerplanthandledetailentity.PowerPlantHandleId);
            AptitudeinvestigateauditEntity aptitudeinvestigateauditentity = aptitudeinvestigateauditbll.GetAuditList(powerplanthandleentity.Id).Where(t => t.Disable == "0").OrderByDescending(t => t.AUDITTIME).FirstOrDefault();
            PowerplantreformEntity powerplantreformentity = powerplantreformbll.GetList("").Where(t => t.Disable == 0 && t.PowerPlantHandleDetailId == keyValue).FirstOrDefault();
            IList<PowerplantcheckEntity> powerplantcheckentitylist = powerplantcheckbll.GetList("").Where(t => t.Disable == 0 && t.PowerPlantHandleDetailId == keyValue).OrderBy(t => t.CreateDate).ToList();

            row["ApplyCode"] = powerplanthandledetailentity.ApplyCode;
            row["CreateDate"] = Convert.ToDateTime(powerplanthandledetailentity.CreateDate).ToString("yyyy年MM月dd日");
            row["AccidentEventName"] = powerplanthandleentity.AccidentEventName;
            row["HappenTime"] = Convert.ToDateTime(powerplanthandleentity.HappenTime).ToString("yyyy年MM月dd日");
            row["BelongDept"] = powerplanthandleentity.BelongDept;
            row["AccidentEventProperty"] = scaffoldbll.getName(powerplanthandleentity.AccidentEventProperty, "AccidentEventProperty");
            row["SituationIntroduction"] = powerplanthandleentity.SituationIntroduction;
            row["ReasonAndProblem"] = powerplanthandledetailentity.ReasonAndProblem;
            row["RectificationMeasures"] = powerplanthandledetailentity.RectificationMeasures;
            row["SignDeptName"] = powerplanthandledetailentity.SignDeptName;
            row["RealSignPersonName"] = powerplanthandledetailentity.RealSignPersonName;
            row["ApproveDept"] = aptitudeinvestigateauditentity == null ? "" : aptitudeinvestigateauditentity.AUDITDEPT;
            row["ApprovePerson"] = aptitudeinvestigateauditentity == null ? "" : (System.IO.File.Exists(Server.MapPath("~/") + aptitudeinvestigateauditentity.AUDITSIGNIMG.Replace("../../", "").ToString()) ? Server.MapPath("~/") + aptitudeinvestigateauditentity.AUDITSIGNIMG.Replace("../../", "").ToString() : defaultimage);
            row["RectificationPerson"] = powerplantreformentity.RectificationPerson;
            row["RectificationDutyDept"] = powerplantreformentity.RectificationDutyDept;
            row["RectificationTime"] = Convert.ToDateTime(powerplantreformentity.RectificationTime).ToString("yyyy年MM月dd日");
            row["RectificationSituation"] = powerplantreformentity.RectificationSituation;
            row["RectificationPersonSignImg"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantreformentity.RectificationPersonSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantreformentity.RectificationPersonSignImg.Replace("../../", "").ToString() : defaultimage;
            row["ReformDate"] = Convert.ToDateTime(powerplantreformentity.CreateDate).ToString("yyyy年MM月dd日");
            for (int i = 0; i < powerplantcheckentitylist.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        row["CheckPerson1"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString() : defaultimage;
                        row["CheckIdea1"] = string.IsNullOrEmpty(powerplantcheckentitylist[i].AuditOpinion) ? "同意" : powerplantcheckentitylist[i].AuditOpinion;
                        break;
                    case 1:
                        row["CheckIdea2"] = powerplantcheckentitylist[i].AuditOpinion;
                        row["CheckPerson2"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString() : defaultimage;
                        row["CheckDate2"] = Convert.ToDateTime(powerplantcheckentitylist[i].AuditTime).ToString("yyyy年MM月dd日");
                        break;
                    case 2:
                        row["CheckIdea3"] = powerplantcheckentitylist[i].AuditOpinion;
                        row["CheckPerson3"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString() : defaultimage;
                        row["CheckDate3"] = Convert.ToDateTime(powerplantcheckentitylist[i].AuditTime).ToString("yyyy年MM月dd日");
                        row["CheckDept3"] = powerplantcheckentitylist[i].AuditDept;
                        break;
                    case 3:
                        row["CheckIdea4"] = powerplantcheckentitylist[i].AuditOpinion;
                        row["CheckPerson4"] = System.IO.File.Exists(Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString()) ? Server.MapPath("~/") + powerplantcheckentitylist[i].AuditSignImg.Replace("../../", "").ToString() : defaultimage;
                        row["CheckDate4"] = Convert.ToDateTime(powerplantcheckentitylist[i].AuditTime).ToString("yyyy年MM月dd日");
                        row["CheckDept4"] = powerplantcheckentitylist[i].AuditDept;
                        break;
                    default:
                        break;
                }
            }

            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }
        #endregion
    }
}
