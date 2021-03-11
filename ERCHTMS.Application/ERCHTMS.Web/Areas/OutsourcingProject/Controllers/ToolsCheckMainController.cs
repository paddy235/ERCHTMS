using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.RoutineSafetyWork;
using ERCHTMS.Code;
using ERCHTMS.Entity.OutsourcingProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using Aspose.Words;
using System.IO;
using Aspose.Words.Saving;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Busines.SystemManage;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 工具器审核主控制器
    /// </summary>
    public class ToolsCheckMainController : MvcControllerBase
    {
        private OutsourcingprojectBLL outbll = new OutsourcingprojectBLL();
        private ProjecttoolsBLL projecttoolsbll = new ProjecttoolsBLL();
        private HistoryProtoolsBLL historyprotoolsbll = new HistoryProtoolsBLL();
        private HistorySpecificToolsBLL historyspecifictoolsbll = new HistorySpecificToolsBLL();
        private HistoryToolsBLL historytoolsbll = new HistoryToolsBLL();
        private HistoryAuditBLL historyauditbll = new HistoryAuditBLL();
        private ToolsAuditBLL ToolsAuditbll = new ToolsAuditBLL();
        private ToolsBLL toolsBll = new ToolsBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private OutsouringengineerBLL outsouringengineernll = new OutsouringengineerBLL();
        private SpecificToolsBLL specifictoolbll = new SpecificToolsBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();

        #region "工器具主界面"

        /// <summary>
        /// 工具器审核主页面Action
        /// </summary>
        /// <returns>工具器审核主页面</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 特种设备首页
        /// </summary>
        /// <returns>特种设备首页</returns>
        [HttpGet]
        public ActionResult IndexSp()
        {
            return View();
        }
        /// <summary>
        /// 获取工器具历史列表
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetToolsHistoryPageList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.id";
            pagination.p_fields = @" a.auditpeopleid,
                                       t.applypeopel,
                                       to_char(t.applytime, 'yyyy-mm-dd') as applytime,
                                       a.auditpeople,a.toolsid auditId,
                                       case when a.auditresult='0' then '合格'
                                            when a.auditresult='2' then '待审核'
                                            when a.auditresult='1' then '不合格' else '' end auditresult";
            pagination.p_tablename = @" epg_historytools t
                                            left join epg_toolsaudit a on t.id = a.toolsid";
            pagination.sidx = "t.createdate";//排序字段
            pagination.sord = "desc";//排序方式
            pagination.conditionJson = " a.auditresult='1' ";
            var data = historytoolsbll.GetHistoryPageList(pagination, queryJson);
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
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetToolsCheckPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "t.toolsid";
                pagination.p_fields = @"t.outprojectid,t.outengineerid,p.engineername,e.fullname,
                                        t.applypeopel,to_char(t.applytime,'yyyy-mm-dd') as applytime,
                                        t.auditstate,e.senddeptid,p.engineerareaname as districtname,d.itemname engineertype,
                                        l.itemname engineerlevel,p.engineerletdept,p.engineerletdeptid,
                                        t.createuserid,t.issaved,t.isover,t.flowid,t.flowdeptname,
                                        t.flowdept,t.flowrolename,t.flowrole,t.flowname,'' as approveuserids,t.specialtytype";
                pagination.p_tablename = @"epg_tools t 
                                left join epg_outsouringengineer p on t.outengineerid = p.id
                                left join base_department e on p.outprojectid = e.departmentid
                                left join ( select m.itemname,m.itemvalue from base_dataitem t
                                left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=p.engineertype
                                left join ( select m.itemname,m.itemvalue from base_dataitem t
                                left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=p.engineerlevel
                                left join ( select m.itemname,m.itemvalue from base_dataitem t
                                left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=p.engineerstate";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                string role = user.RoleName;

                string allrangedept = "";
                try
                {
                    allrangedept = dataitemdetailbll.GetDataItemByDetailCode("SBDept", "SBDeptId").FirstOrDefault().ItemValue;
                }
                catch (Exception)
                {

                }

                if (role.Contains("省级"))
                {
                    pagination.conditionJson = string.Format(@" t.createuserorgcode  in (select encode
                from base_department d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null) and (t.issaved='1' or t.createuserid='{1}') ", user.NewDeptCode, user.UserId);
                }
                else if (role.Contains("公司级用户") || role.Contains("厂级部门用户") || allrangedept.Contains(user.DeptId))
                {
                    pagination.conditionJson = string.Format(" t.createuserorgcode  = '{0}' and (t.issaved='1' or t.createuserid='{1}') ", user.OrganizeCode, user.UserId);
                }
                else if (role.Contains("承包商"))
                {
                    pagination.conditionJson = string.Format(" （e.departmentid = '{0}' or p.supervisorid='{0}' or t.createuserid='{1}') ", user.DeptId, user.UserId);
                }
                else
                {
                    var deptentity = departmentbll.GetEntity(user.DeptId);
                    while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" (p.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.issaved='1' or t.createuserid='{1}') ", deptentity.EnCode, user.UserId);

                }

                var watch = CommonHelper.TimerStart();
                ToolsBLL toolsBll = new ToolsBLL();
                var data = toolsBll.GetPageList(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = outsouringengineernll.GetEntity(data.Rows[i]["outengineerid"].ToString());
                    var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //获取下一步审核人
                    string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["toolsid"].ToString(), "",data.Rows[i]["specialtytype"].ToString(), excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
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
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetOutsouringengineerByID(string keyValue)
        {
            try
            {
                OutsouringengineerBLL bll = new OutsouringengineerBLL();
                var entity = bll.GetEntity(keyValue);
                return ToJsonResult(entity);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
            }
        /// <summary>
        /// 根据编号获取外包工程数据
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult GetOutengineerByID(string keyValue)
        {
            OutsouringengineerBLL bll = new OutsouringengineerBLL();
            var entity = bll.GetEntity(keyValue);
            var prodata = outbll.GetOutProjectInfo(entity.OUTPROJECTID);
            var resultData = new
            {
                data = entity,
                proData = prodata
            
            };
            return ToJsonResult(resultData);
        }
        /// <summary>
        /// 工具器验收申请/审核详细页 Action
        /// </summary>
        /// <returns>工具器验收申请/审核详细页</returns>
        public ActionResult ToolCheckFroms()
        {
            return View();
        }
        /// <summary>
        /// 特种设备验收申请/审核详细页 Action
        /// </summary>
        /// <returns>特种设备验收申请/审核详细页</returns>
        public ActionResult ToolCheckFromsSp()
        {
            return View();
        }
        /// <summary>
        /// 施工器具添加/修改页 Action
        /// </summary>
        /// <returns>施工器具添加/修改页</returns>
        public ActionResult ProjectToolFroms()
        {
            return View();
        }

        /// <summary>
        /// 特种设备添加/修改页 Action
        /// </summary>
        /// <returns>特种设备添加/修改页</returns>
        public ActionResult SpecificToolFroms()
        {
            return View();
        }
        /// <summary>
        /// 施工机具导入
        /// </summary>
        /// <returns>施工机具导入</returns>
        public ActionResult ImportEqu()
        {
            return View();
        }
        /// <summary>
        /// 历史施工器具视图
        /// </summary>
        /// <returns>页面视图</returns>
        [HttpGet]
        public ActionResult HistoryToolsIndex()
        {
            return View();
        }
        /// <summary>
        /// 历史特种设备视图
        /// </summary>
        /// <returns>页面视图</returns>
        [HttpGet]
        public ActionResult HistoryToolsIndexSp()
        {
            return View();
        }
        /// <summary>
        /// 历史特种设备详情视图
        /// </summary>
        /// <returns>页面视图</returns>
        [HttpGet]
        public ActionResult HistorySpecificToolFroms()
        {
            return View();
        }
        /// <summary>
        /// 历史普通设备详情视图
        /// </summary>
        /// <returns>页面视图</returns>
        [HttpGet]
        public ActionResult HistoryProjectToolFroms()
        {
            return View();
        }
        /// <summary>
        /// 历史施工器具视图
        /// </summary>
        /// <returns>页面视图</returns>
        [HttpGet]
        public ActionResult HistoryToolsFroms()
        {
            return View();
        }
        /// <summary>
        /// 历史特种设备视图
        /// </summary>
        /// <returns>页面视图</returns>
        [HttpGet]
        public ActionResult HistoryToolsFromsSp()
        {
            return View();
        }
        /// <summary>
        /// 流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            ToolsBLL toolsBll = new ToolsBLL();
            var data = toolsBll.GetList(queryJson);
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
            ToolsBLL toolsBll = new ToolsBLL();
            var data = toolsBll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 历史施工器具信息
        /// </summary>
        /// <returns>页面视图</returns>
        [HttpGet]
        public ActionResult GetHistoryForm(string keyValue, string auditId)
        {
            var histools = historytoolsbll.GetEntity(keyValue);
            var hisAudit = historyauditbll.GetEntity(auditId);
            var hisData = new
            {
                histools = histools,
                hisAudit = hisAudit
            };
            return ToJsonResult(hisData);
        }
        public ActionResult GetFlow(string keyValue)
        {
            try
            {
                return Content(toolsBll.GetFlow(keyValue, "设备工器具").ToJson());
            }
            catch (Exception ex)
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
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            //主记录
            toolsBll.RemoveForm(keyValue);
            //工器具及附件
            var prjList = projecttoolsbll.GetList(string.Format(" and TOOLSID = '{0}'", keyValue));
            prjList.Any(x =>
            {
                projecttoolsbll.RemoveForm(x.PROJECTTOOLSID);
                DeleteFiles(x.PROJECTTOOLSID);
                return false;
            });
            //特种设备及附件
            var spList = specifictoolbll.GetList(string.Format(" and TOOLSID = '{0}'", keyValue));
            spList.Any(x =>
            {
                specifictoolbll.RemoveForm(x.SPECIFICTOOLSID);
                DeleteFiles(x.REGISTERCARDFILE);
                DeleteFiles(x.QUALIFIED);
                DeleteFiles(x.CHECKREPORTFILE);
                DeleteFiles(x.SPECIFICTOOLSFILE);
                return false;
            });
            //审核记录及附件
            var adiList = ToolsAuditbll.GetAuditList(keyValue);
            adiList.Any(x =>
            {
                ToolsAuditbll.RemoveForm(x.AUDITID);
                DeleteFiles(x.AUDITFILE);      
                return false;
            });
            //历史记录         
            var hisToolsList = historytoolsbll.GetList(string.Format(" and TOOLSID = '{0}'", keyValue));
            hisToolsList.Any(x =>
            {
                var hKeyValue = x.ID;
                historytoolsbll.RemoveForm(hKeyValue);
                //历史工器具及附件                
                var prjHisList = historyprotoolsbll.GetList(string.Format(" and TOOLSID = '{0}'", hKeyValue));
                prjHisList.Any(y =>
                {
                    historyprotoolsbll.RemoveForm(y.ID);
                    DeleteFiles(x.ID);
                    return false;
                });
                //历史特种设备及附件
                var spHisList = historyspecifictoolsbll.GetList(string.Format(" and TOOLSID = '{0}'", hKeyValue));
                spHisList.Any(y =>
                {
                    specifictoolbll.RemoveForm(y.SPECIFICTOOLSID);
                    DeleteFiles(y.REGISTERCARDFILE);
                    DeleteFiles(y.QUALIFIED);
                    DeleteFiles(y.CHECKREPORTFILE);
                    DeleteFiles(y.SPECIFICTOOLSFILE);
                    return false;
                });
                //历史审核记录及附件
                var adiHisList = ToolsAuditbll.GetAuditList(hKeyValue);
                adiHisList.Any(y =>
                {
                    ToolsAuditbll.RemoveForm(y.AUDITID);
                    DeleteFiles(y.TOOLSID);
                    return false;
                });
                return false;
            });

            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>SaveToolsAuditForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string type, ToolsEntity entity)
        {
            toolsBll.SaveForm(keyValue, type, entity);
            return Success("操作成功。");
        }
        #endregion

        #endregion

        #region "施工器具界面方法"

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页查询条件</param>
        /// <param name="toolsId">工器具ID</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetProjectToolsListJson(Pagination pagination, string toolsId)
        {
            try
            {
                pagination.p_kid = "t.PROJECTTOOLSID";
                pagination.p_fields = @"t.TOOLSDEPTNAME,t.TOOLSNAME,t.TOOLSTYPE,t.TOOLSBUYTIME,t.TOOLSINITTIME,t.CREATEDATE,d.itemname as tooltypename,t.status,t.checkoption,t.toolscheckdate";
                pagination.p_tablename = @"EPG_PROJECTTOOLS t left join base_dataitemdetail d on t.tooltype=d.itemvalue and d.itemid =(select itemid from base_dataitem where itemcode='ToolEquipmentType')";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.conditionJson = string.Format("TOOLSID='{0}'", toolsId);
                ToolsEntity entity = toolsBll.GetEntity(toolsId);
                if (entity != null && !string.IsNullOrWhiteSpace(entity.FLOWNAME) && entity.FLOWNAME.Contains("验收"))
                {
                    if (user.RoleName.Contains("专工") || user.RoleName.Contains("安全管理员"))
                    {
                        List<DataItemModel> model = dataitemdetailbll.GetDataItemListByItemCode("ToolEquipmentType").ToList();
                        string type = string.Join("','", model.Where(t => string.IsNullOrWhiteSpace(t.Description) || (user.SpecialtyType + ",").Contains(t.Description.Replace("\n","") + ",")).Select(t => t.ItemValue).ToArray().Distinct());
                        pagination.conditionJson += string.Format(" and tooltype in ('" + type + "')");
                    }
                }
                pagination.sidx = "t.TOOLSINITTIME";//排序字段
                pagination.sord = "desc";//排序方式
                var watch = CommonHelper.TimerStart();
                var data = projecttoolsbll.GetPageList(pagination, toolsId);
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
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        [HttpGet]
        public ActionResult GetHistoryProjectTools(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.id";
            pagination.p_fields = @"t.TOOLSDEPTNAME,t.TOOLSNAME,t.TOOLSTYPE,t.TOOLSBUYTIME,t.TOOLSINITTIME,t.CREATEDATE,d.itemname as tooltypename,t.status,t.checkoption,t.toolscheckdate";
            pagination.p_tablename = @"epg_historyprojecttools t left join base_dataitemdetail d on t.tooltype=d.itemvalue and d.itemid =(select itemid from base_dataitem where itemcode='ToolEquipmentType')";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.sidx = "t.TOOLSINITTIME";//排序字段
            pagination.sord = "desc";//排序方式
            var watch = CommonHelper.TimerStart();
            var data = historyprotoolsbll.GetHistoryPageList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetProjectToolsFormJson(string keyValue)
        {

            var data = projecttoolsbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetProToolsHistoryForm(string keyValue)
        {

            var data = historyprotoolsbll.GetEntity(keyValue);
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
        public ActionResult RemoveProjectToolsForm(string keyValue)
        {


            projecttoolsbll.RemoveForm(keyValue);
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
        [AjaxOnly]
        public ActionResult SaveProjectToolsForm(string keyValue, ProjecttoolsEntity entity)
        {

            projecttoolsbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 施工机具导入
        /// </summary>
        /// <param name="outProId">外包单位Id</param>
        /// <param name="toolsId">设备工器具id</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string ImportEquipment(string outProId, string toolsId)
        {
            if (OperatorProvider.Provider.Current().IsSystem)
            {
                return "超级管理员无此操作权限";
            }
            string orgId = OperatorProvider.Provider.Current().OrganizeId;//所属公司
            string deptId = outProId;//外包单位
            int error = 0;
            string message = "请选择格式正确的文件再导入!";
            string falseMessage = "";
            int count = HttpContext.Request.Files.Count;
            if (count > 0)
            {
                HttpPostedFileBase file = HttpContext.Request.Files[0];
                if (string.IsNullOrEmpty(file.FileName))
                {
                    return message;
                }
                if (!(file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xls") || file.FileName.Substring(file.FileName.IndexOf('.')).Contains("xlsx")))
                {
                    return message;
                }
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + System.IO.Path.GetExtension(file.FileName);
                file.SaveAs(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                wb.Open(Server.MapPath("~/Resource/temp/" + fileName));
                Aspose.Cells.Cells cells = wb.Worksheets[0].Cells;
                DataTable dt = cells.ExportDataTable(1, 0, cells.MaxDataRow, cells.MaxColumn + 1, false);
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    //设备名称
                    string toolsname = dt.Rows[i][0].ToString();
                    //设备分类
                    string tooltype = dt.Rows[i][1].ToString();
                    //规格型号
                    string toolstype = dt.Rows[i][2].ToString();
                    //数量
                    string toolscount = dt.Rows[i][3].ToString();
                    //购置时间
                    string toolsbuytime = dt.Rows[i][4].ToString();
                    //出厂年月
                    string toolsinittime = dt.Rows[i][5].ToString();
                    //出厂编号
                    string toolsinitnumber = dt.Rows[i][6].ToString();
                    //制造单位名称
                    string toolsmadecompany = dt.Rows[i][7].ToString();
                    //最近检验时间
                    string toolscheckdate = dt.Rows[i][8].ToString();
                    //检验周期
                    string checkdays = dt.Rows[i][9].ToString();
                    //备注
                    string remark = dt.Rows[i][10].ToString();

                    //---****值存在空验证*****--
                    if (string.IsNullOrEmpty(toolsname) || string.IsNullOrEmpty(toolscount) || string.IsNullOrEmpty(toolstype) || string.IsNullOrEmpty(tooltype))
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行值存在空,未能导入.";
                        error++;
                        continue;
                    }
                    ProjecttoolsEntity pe = new ProjecttoolsEntity();
                    pe.PROJECTTOOLSID = Guid.NewGuid().ToString();
                    pe.TOOLSNAME = toolsname;
                    pe.TOOLSTYPE = toolstype;
                    pe.REMARK = remark;
                    pe.TOOLTYPE = dataitemdetailbll.GetItemValue(tooltype, "ToolEquipmentType");
                    try
                    {
                        if (!string.IsNullOrEmpty(toolscount))
                        {
                            pe.TOOLSCOUNT = Int32.Parse(toolscount);
                        }

                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行数量有误,未能导入.";
                        error++;
                        continue;
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(toolsbuytime))
                        {
                            pe.TOOLSBUYTIME = DateTime.Parse(DateTime.Parse(toolsbuytime).ToString("yyyy-MM-dd"));
                        }

                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行购置时间有误,未能导入.";
                        error++;
                        continue;
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(toolsinittime))
                        {
                            pe.TOOLSINITTIME = DateTime.Parse(toolsinittime).ToString("yyyy-MM-dd");
                        }

                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行出厂年月有误,未能导入.";
                        error++;
                        continue;
                    }
                    pe.TOOLSINITNUMBER = toolsinitnumber;
                    pe.TOOLSMADECOMPANY = toolsmadecompany;
                    try
                    {
                        if (!string.IsNullOrEmpty(toolscheckdate))
                        {
                            pe.TOOLSCHECKDATE = DateTime.Parse(DateTime.Parse(toolscheckdate).ToString("yyyy-MM-dd"));
                        }

                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行最近检验时间有误,未能导入.";
                        error++;
                        continue;
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(checkdays))
                        {
                            pe.CHECKDAYS = Int32.Parse(checkdays);
                        }

                    }
                    catch
                    {
                        falseMessage += "</br>" + "第" + (i + 2) + "行检查周期有误,未能导入.";
                        error++;
                        continue;
                    }
                    if(pe.TOOLSCHECKDATE.HasValue && pe.CHECKDAYS.HasValue)
                        pe.NEXTCHECKDATE = pe.TOOLSCHECKDATE.Value.AddDays((double)pe.CHECKDAYS).ToString("yyyy-MM-dd");
                    pe.TOOLSDEPTID = outProId;
                    pe.TOOLSDEPTNAME = outbll.GetOutProjectInfo(outProId).OUTSOURCINGNAME;
                    pe.TOOLSID = toolsId;
                    try
                    {
                        projecttoolsbll.SaveForm(pe.PROJECTTOOLSID, pe);
                    }
                    catch
                    {
                        error++;
                    }
                }
                count = dt.Rows.Count - 1;
                message = "共有" + count + "条记录,成功导入" + (count - error) + "条，失败" + error + "条";
                message += "</br>" + falseMessage;
            }
            return message;
        }

        #endregion
        #endregion

        #region "特种设备界面方法"
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="toolsId">工器具ID</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetSpecificToolsListJson(Pagination pagination, string toolsId)
        {
            SpecificToolsBLL SpecificToolsbll = new SpecificToolsBLL();
            pagination.p_kid = "t.SPECIFICTOOLSID";
            pagination.p_fields = @"t.OUTCOMPANYNAME,t.TOOLSNAME,t.TOOLSMODEL,to_char(t.TOOLSBUYDATE,'yyyy-MM-dd') as TOOLSBUYDATE,to_char(t.CHECKDATE,'yyyy-MM-dd') as CHECKDATE,to_char(t.NEXTCHECKDATE,'yyyy-MM-dd') as NEXTCHECKDATE";
            pagination.p_tablename = @"EPG_SPECIFICTOOLS t";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format("TOOLSID='{0}'", toolsId);
            pagination.sidx = "t.CREATEDATE";//排序字段
            pagination.sord = "desc";//排序方式
            var watch = CommonHelper.TimerStart();
            var data = SpecificToolsbll.GetPageList(pagination, toolsId);
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
        [HttpGet]
        public ActionResult GetHistorySpecificTools(Pagination pagination, string queryJson)
        {
            SpecificToolsBLL SpecificToolsbll = new SpecificToolsBLL();
            pagination.p_kid = "t.specifictoolsid";
            pagination.p_fields = @"t.outcompanyname,t.toolsname,t.toolsmodel,to_char(t.toolsbuydate,'yyyy-mm-dd') as toolsbuydate,
                                    to_char(t.checkdate,'yyyy-mm-dd') as checkdate,to_char(t.nextcheckdate,'yyyy-mm-dd') as nextcheckdate";
            pagination.p_tablename = @"epg_historyspecifictools t";
            pagination.sidx = "t.createdate";//排序字段
            pagination.sord = "desc";//排序方式
            var watch = CommonHelper.TimerStart();
            var data = historyspecifictoolsbll.GetHistoryPageList(pagination, queryJson);
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
        /// 根据工程ID获取列表
        /// </summary>
        /// <param name="toolsId">工器具ID</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetSpecificToolsByIDListJson(Pagination pagination, string projectId)
        {
            SpecificToolsBLL SpecificToolsbll = new SpecificToolsBLL();
            pagination.p_kid = "t.SPECIFICTOOLSID";
            pagination.p_fields = @"t.OUTCOMPANYNAME,t.TOOLSNAME,t.TOOLSMODEL,to_char(t.TOOLSBUYDATE,'yyyy-MM-dd') as TOOLSBUYDATE,to_char(t.CHECKDATE,'yyyy-MM-dd') as CHECKDATE,to_char(t.NEXTCHECKDATE,'yyyy-MM-dd') as NEXTCHECKDATE";
            pagination.p_tablename = @"EPG_SPECIFICTOOLS t left join EPG_TOOLS s on t.toolsid=s.toolsid left join EPG_TOOLSAUDIT a on  s.toolsid=a.toolsid";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" a.auditresult='0' and t.outprojectid='{0}' ", projectId);
            pagination.sidx = "t.CREATEDATE";//排序字段
            pagination.sord = "desc";//排序方式
            var watch = CommonHelper.TimerStart();
            var data = SpecificToolsbll.GetPageList(pagination, projectId);
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


        #region 获取数据
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetSpecificToolsFormJson(string keyValue)
        {
            SpecificToolsBLL SpecificToolsbll = new SpecificToolsBLL();
            var data = SpecificToolsbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetSpecificToolsHistoryForm(string keyValue)
        {
            var data = historyspecifictoolsbll.GetEntity(keyValue);
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
        public ActionResult RemoveSpecificToolsForm(string keyValue)
        {
            SpecificToolsBLL SpecificToolsbll = new SpecificToolsBLL();

            SpecificToolsbll.RemoveForm(keyValue);
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
        [AjaxOnly]
        public ActionResult SaveSpecificToolsForm(string keyValue, SpecificToolsEntity entity)
        {
            SpecificToolsBLL SpecificToolsbll = new SpecificToolsBLL();
            SpecificToolsbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
        #endregion

        #region "工器具审核表"

        #region 获取数据
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetAUDITFormJson(string keyValue)
        {
            ToolsAuditBLL ToolsAuditbll = new ToolsAuditBLL();
            var data = ToolsAuditbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 根据关联业务Id查询审核记录
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.auditid";
                pagination.p_fields = @" t.auditresult,t.auditpeopleid,t.audittime,t.auditopinion,t.auditpeople,t.auditdept,t.auditfile";
                pagination.p_tablename = @"epg_toolsaudit t";
                pagination.sidx = "t.audittime";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = " 1=1 ";
                var data = ToolsAuditbll.GetPageList(pagination, queryJson);


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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 获取文件信息列表
        /// </summary>
        ///<param name="fileId"></param>
        [HttpGet]
        public ActionResult GetFiles(string fileId)
        {
            FileInfoBLL fi = new FileInfoBLL();
            var data = fi.GetFiles(fileId);
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
        public ActionResult RemoveToolsAuditForm(string keyValue)
        {
            ToolsAuditBLL ToolsAuditbll = new ToolsAuditBLL();

            ToolsAuditbll.RemoveForm(keyValue);
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
        [AjaxOnly]
        public ActionResult SaveToolsAuditForm(string keyValue, ToolsAuditEntity entity,string recordData)
        {
            try
            {
                ToolsAuditBLL ToolsAuditbll = new ToolsAuditBLL();
                if (!string.IsNullOrWhiteSpace(recordData))
                {
                    List<ProjecttoolsEntity> list = JsonConvert.DeserializeObject<List<ProjecttoolsEntity>>(recordData);
                    int result = projecttoolsbll.UpdateMultData(list);
                    if (result == 0)
                    {
                        return Error("操作失败，请联系系统管理员。");
                    }
                }
                ToolsAuditbll.SaveForm(keyValue, entity, "设备工器具");
                return Success("操作成功。");
            }
            catch (Exception ex)
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
        [AjaxOnly]
        public ActionResult SaveToolsAuditFormSp(string keyValue, ToolsAuditEntity entity)
        {
            ToolsAuditBLL ToolsAuditbll = new ToolsAuditBLL();
            ToolsAuditbll.SaveForm(keyValue, entity, "特种设备工器具");
            return Success("操作成功。");
        }

        #region 提交到审核或者结束
        /// <summary>
        /// 登记的内容提交到审核或者结束（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, ToolsAuditEntity aentity)
        {
            ToolsAuditbll.SaveForm(keyValue, aentity, "设备工器具");
            return Success("操作成功。");
        }
        /// <summary>
        /// 登记的内容提交到审核或者结束（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveFormSp(string keyValue, ToolsAuditEntity aentity)
        {
            ToolsAuditbll.SaveForm(keyValue, aentity, "特种设备工器具");
            return Success("操作成功。");
        }
        #endregion
        #endregion
        #endregion

        /// <summary>
        /// 外包工程设备登记表导出
        /// </summary>
        [HandlerMonitor(0, "外包工程设备登记表")]
        public void ExportDetails(string keyValue,string type)
        {
            ToolsEntity tool = toolsBll.GetEntity(keyValue);
            string strengineer = "";//工程名称
            string strproject = "";//施工单位
            string strlaw = "";//法人代表
            string senddeptid = "";//发包部门
            string senddeptidea = "";//发包部门审核意见
            string deptduty = "";//项目所在部门负责人
            string skillperson = "";//生技部审核人 
            string safeperson = ""; // 安环部审核人
            string time1 = "", time2 = "", time3 = "";
            string outhead = string.Empty;
            string applyperson = string.Empty;
            string contractperiod = string.Empty;
            string applyTime = "";
            string engineerdirector = "";//施工负责人（黄金埠）
            string date1 = ""; //创建时间
            string shname1 = ""; //项目单位检查人签字（黄金埠）
            string date2 = ""; //项目单位检查人审核时间（黄金埠）
            string shremark2 = ""; //项目负责人意见（黄金埠）
            string shname2 = ""; //项目负责人（黄金埠）
            string date3 = "";//项目负责人审核时间（黄金埠）
            string shremark3 = ""; //安环部意见（黄金埠）
            string shname3 = ""; //安环部负责人（黄金埠）
            string date4 = ""; //安环部审核时间（黄金埠）
            date1 = DateTime.Parse(tool.CREATEDATE.ToString()).ToString("yyyy年MM月dd日");
            if (tool != null)
            {
                OutsouringengineerEntity eng = outsouringengineernll.GetEntity(tool.OUTENGINEERID);
                OutsourcingprojectEntity pro = outbll.GetOutProjectInfo(tool.OUTPROJECTID);
                if (eng != null) {
                    strengineer = eng.ENGINEERNAME;
                    strlaw = eng.ENGINEERDIRECTOR;
                    engineerdirector = eng.ENGINEERDIRECTOR;
                    var comList = new CompactBLL().GetComoactTimeByProjectId(eng.ID);
                    if (comList.Rows.Count > 0)
                    {
                        var startTime = string.Empty;
                        DateTime r = new DateTime();
                        if (DateTime.TryParse(comList.Rows[0]["mintime"].ToString(), out r))
                        {
                            startTime = r.ToString("yyyy年MM月dd日");
                        }
                        var endTime = string.Empty;
                        DateTime e = new DateTime();
                        if (DateTime.TryParse(comList.Rows[0]["maxtime"].ToString(), out e))
                        {
                            endTime = e.ToString("yyyy年MM月dd日");
                        }
                        contractperiod = startTime + "至" + endTime;
                    }
                }
                if (pro != null)
                {
                    strproject = pro.OUTSOURCINGNAME;
                  
                    //var depart = new DepartmentBLL().GetList().Where(x => x.DepartmentId == pro.OUTPROJECTID).ToList().FirstOrDefault();
                    //senddeptid = depart == null ? "" : depart.SendDeptID;
                    senddeptid = eng.ENGINEERLETDEPTID;
                }
                applyTime = tool.CREATEDATE.Value.ToString("yyyy年MM月dd日");
                applyperson = tool.CREATEUSERNAME;
            }
            var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
            string fileName = string.Empty;
            var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == userInfo.OrganizeCode && x.ModuleCode == "GQJYS").ToList();
            string tempPath = @"~/Resource/ExcelTemplate/外包工程设备登记表.doc";
            var tempEntity = tempconfig.FirstOrDefault();
            var IsGdxy = false;
            if (tempconfig.Count > 0)
            {
                if (tempEntity != null)
                {
                    switch (tempEntity.ProessMode)
                    {
                        case "TY"://通用处理方式
                            if (type == "sp")
                            {
                                tempPath = @"~/Resource/ExcelTemplate/外包工程设备登记表.doc";
                            }
                            else
                            {
                                tempPath = @"~/Resource/ExcelTemplate/外包工程施工机具登记表.doc";
                            }
                           
                            break;
                        case "HRCB"://华润
                            if (type == "sp")
                            {
                                tempPath = @"~/Resource/ExcelTemplate/外包工程设备登记表.doc";
                            }
                            else
                            {
                                tempPath = @"~/Resource/ExcelTemplate/外包工程施工机具登记表.doc";
                            }
                            break;
                        case "GDXY"://国电荥阳
                            tempPath = @"~/Resource/ExcelTemplate/国电荥阳外包工程施工机具登记.doc";
                            IsGdxy = true;
                            break;
                        case "HJB"://黄金埠
                            tempPath = @"~/Resource/ExcelTemplate/黄金埠外包工程施工机具登记.doc";
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                if (type == "sp")
                {
                    tempPath = @"~/Resource/ExcelTemplate/外包工程设备登记表.doc";
                }
                else
                {
                    tempPath = @"~/Resource/ExcelTemplate/外包工程施工机具登记表.doc";
                }
            }

            fileName = Server.MapPath(tempPath);

            //设备信息
            DataTable dt = new DataTable("U", "U");
            dt.Columns.Add("AX");
            dt.Columns.Add("BX");//设备名称
            dt.Columns.Add("CX");//型号
            dt.Columns.Add("DX");//数量
            dt.Columns.Add("EX");//最近检验日期
            dt.Columns.Add("FX");//检验周期
            dt.Columns.Add("GX");//使用登记号编号
            dt.Columns.Add("HX");
            dt.Columns.Add("IX");//出厂编号
            dt.Columns.Add("JX");//有效期(合格证)
            dt.Columns.Add("KX");//是否准用
            //TOOLSINITNUMBER
            List<ProjecttoolsEntity> projecttoollist = projecttoolsbll.GetList(string.Format(" and TOOLSID = '{0}'", keyValue)).ToList();
            List<ToolsAuditEntity> list = ToolsAuditbll.GetAuditList(keyValue).OrderBy(x => x.AUDITTIME).ToList();
            //List<AptitudeinvestigateauditEntity> auditList = new AptitudeinvestigateauditBLL().GetAuditList(keyValue).ToList();
            int count = 1;
            foreach (var item in projecttoollist)
            {
                DataRow row = dt.NewRow();
                row["AX"] = count.ToString();
                row["BX"] = item.TOOLSNAME;
                row["CX"] = item.TOOLSTYPE;
                row["DX"] = item.TOOLSCOUNT.ToString();
                if (item.TOOLSCHECKDATE != null)
                {
                    DateTime r = new DateTime();
                    if (DateTime.TryParse(item.TOOLSCHECKDATE.Value.ToString(), out r))
                    {
                        row["EX"] = r.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        row["EX"] = "";
                    }
                }
                else {
                    row["EX"] = "";
                }
                if (!string.IsNullOrWhiteSpace(item.NEXTCHECKDATE))
                {
                    DateTime e = new DateTime();
                    if (DateTime.TryParse(item.NEXTCHECKDATE.ToString(), out e))
                    {
                        row["FX"] = e.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        row["FX"] = "";
                    }
                }
                else {
                    row["FX"] = "";
                }
              
                //row["EX"] = Convert.ToDateTime(item.TOOLSCHECKDATE).ToString("yyyy-MM-dd");
                //row["FX"] = Convert.ToDateTime(item.NEXTCHECKDATE).ToString("yyyy-MM-dd");
                if (IsGdxy)
                {
                    row["GX"] = "";
                }
                else {
                    row["GX"] = list.Count > 0 ? list.First().AUDITPEOPLE.ToString() : "";
                }
               
                row["HX"] = "";
                count++;
                dt.Rows.Add(row);
            }
            List<SpecificToolsEntity> specificlist = specifictoolbll.GetList(string.Format(" and TOOLSID = '{0}'",keyValue)).ToList();
            foreach (var stool in specificlist)
            {
                DataRow row = dt.NewRow();
                row["AX"] = count.ToString();
                row["BX"] = stool.TOOLSNAME;
                row["CX"] = stool.TOOLSMODEL;
                row["DX"] = "1";
                DateTime r = new DateTime();
                if (stool.CHECKDATE != null)
                {
                    if (DateTime.TryParse(stool.CHECKDATE.Value.ToString(), out r))
                    {
                        row["EX"] = r.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        row["EX"] = "";
                    }
                }
                else {
                    row["EX"] = "";
                }
                if (stool.NEXTCHECKDATE != null)
                {
                    DateTime e = new DateTime();
                    if (DateTime.TryParse(stool.NEXTCHECKDATE.Value.ToString(), out e))
                    {
                        row["FX"] = e.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        row["FX"] = "";
                    }
                }
                else {
                    row["FX"] = "";
                }
              
                //row["EX"] = Convert.ToDateTime(stool.CHECKDATE).ToString("yyyy-MM-dd");
                //row["FX"] = Convert.ToDateTime(stool.NEXTCHECKDATE).ToString("yyyy-MM-dd"); 
                row["GX"] = stool.REGISTERNUMBER;
                count++;
                dt.Rows.Add(row);
            }
            if (dt.Rows.Count <= 0)
            {
                DataRow dr1 = dt.NewRow();
                dt.Rows.Add(dr1);
            }
            DataItemDetailBLL bll = new DataItemDetailBLL();
            DataTable dt1 = dt.Copy();
            dt1.TableName = "T1"; ;
            dt1.Clear();
            List<ProjecttoolsEntity> projecttoollist1 = projecttoollist.Where(t => t.TOOLTYPE == bll.GetItemValue("电动工器具", "ToolEquipmentType")).ToList();
            foreach (var item in projecttoollist1)
            {
                DataRow dtrow = dt1.NewRow();
                dtrow["AX"] = projecttoollist1.IndexOf(item) + 1;
                dtrow["BX"] = item.TOOLSNAME;
                dtrow["IX"] = item.TOOLSINITNUMBER;
                dtrow["JX"] = (item.TOOLSCHECKDATE == null ? "" : item.TOOLSCHECKDATE.Value.ToString("yyyy-MM-dd")) + "～" + item.NEXTCHECKDATE == null ? "" : item.NEXTCHECKDATE;
                dtrow["KX"] = item.Status == "0" ? "是" : "否";
                dt1.Rows.Add(dtrow);
            }
            if (dt1.Rows.Count <= 0)
            {
                DataRow dr1 = dt1.NewRow();
                dt1.Rows.Add(dr1);
            }
            DataTable dt2 = dt.Copy();
            dt2.TableName = "T2";
            dt2.Clear();
            List<ProjecttoolsEntity> projecttoollist2 = projecttoollist.Where(t => t.TOOLTYPE == bll.GetItemValue("安全工器具", "ToolEquipmentType")).ToList();
            foreach (var item in projecttoollist2)
            {
                DataRow dtrow = dt2.NewRow();
                dtrow["AX"] = projecttoollist2.IndexOf(item) + 1;
                dtrow["BX"] = item.TOOLSNAME;
                dtrow["IX"] = item.TOOLSINITNUMBER;
                dtrow["JX"] = (item.TOOLSCHECKDATE == null ? "" : item.TOOLSCHECKDATE.Value.ToString("yyyy-MM-dd")) + "～" + item.NEXTCHECKDATE == null ? "" : item.NEXTCHECKDATE;
                dtrow["KX"] = item.Status == "0" ? "是" : "否";
                dt2.Rows.Add(dtrow);
            }
            if (dt2.Rows.Count <= 0)
            {
                DataRow dr1 = dt2.NewRow();
                dt2.Rows.Add(dr1);
            }
            DataTable dt3 = dt.Copy();
            dt3.TableName = "T3"; ;
            dt3.Clear();
            List<ProjecttoolsEntity> projecttoollist3 = projecttoollist.Where(t => t.TOOLTYPE == bll.GetItemValue("其他工器具", "ToolEquipmentType")).ToList();
            foreach (var item in projecttoollist3)
            {
                DataRow dtrow = dt3.NewRow();
                dtrow["AX"] = projecttoollist3.IndexOf(item) + 1;
                dtrow["BX"] = item.TOOLSNAME;
                dtrow["IX"] = item.TOOLSINITNUMBER;
                dtrow["JX"] = (item.TOOLSCHECKDATE == null ? "" : item.TOOLSCHECKDATE.Value.ToString("yyyy-MM-dd")) + "～" + item.NEXTCHECKDATE == null ? "" : item.NEXTCHECKDATE;
                dtrow["KX"] = item.Status == "0" ? "是" : "否";
                dt3.Rows.Add(dtrow);
            }
            if (dt3.Rows.Count <= 0)
            {
                DataRow dr1 = dt3.NewRow();
                dt3.Rows.Add(dr1);
            }
            DataTable dt4 = dt.Copy();
            dt4.TableName = "T4"; ;
            dt4.Clear();
            List<ProjecttoolsEntity> projecttoollist4 = projecttoollist.Where(t => t.TOOLTYPE == bll.GetItemValue("检修电源接线盘工器具", "ToolEquipmentType")).ToList();
            foreach (var item in projecttoollist4)
            {
                DataRow dtrow = dt4.NewRow();
                dtrow["AX"] = projecttoollist4.IndexOf(item) + 1;
                dtrow["BX"] = item.TOOLSNAME;
                dtrow["IX"] = item.TOOLSINITNUMBER;
                dtrow["JX"] = (item.TOOLSCHECKDATE == null ? "" : item.TOOLSCHECKDATE.Value.ToString("yyyy-MM-dd")) + "～" + item.NEXTCHECKDATE == null ? "" : item.NEXTCHECKDATE;
                dtrow["KX"] = item.Status == "0" ? "是" : "否";
                dt4.Rows.Add(dtrow);
            }
            if (dt4.Rows.Count <= 0)
            {
                DataRow dr1 = dt4.NewRow();
                dt4.Rows.Add(dr1);
            }
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (senddeptid == list[i].AUDITDEPTID)
                    {
                        senddeptidea = list[i].AUDITOPINION;
                        deptduty = list[i].AUDITPEOPLE;
                        time1 = list[i].AUDITTIME.Value.ToString("yyyy-MM-dd");
                    }
                    if (tool.OUTPROJECTID == list[i].AUDITDEPTID)
                    {
                        outhead = list[i].AUDITPEOPLE;
                    }
                    string val = new DataItemDetailBLL().GetItemValue(userInfo.OrganizeCode);
                
                    if (!string.IsNullOrEmpty(val))
                    {
                        var deptList = val.Split(',');
                        if (deptList.Length > 1) {
                                if (list[i].AUDITDEPTID.ToString()==deptList[0])
                                {
                                    skillperson = list[i].AUDITPEOPLE;
                                    time2 = list[i].AUDITTIME.Value.ToString("yyyy-MM-dd");
                                }
                                if (list[i].AUDITDEPTID == deptList[1])
                                {
                                    safeperson = list[i].AUDITPEOPLE;
                                    time3 = list[i].AUDITTIME.Value.ToString("yyyy-MM-dd");
                                }
                        }
                    }
                }
                try
                {
                    shremark3 = list[list.Count - 1].AUDITOPINION;
                    shname3 = list[list.Count - 1].AUDITPEOPLE;
                    date4 = list[list.Count - 1].AUDITTIME.Value.ToString("yyyy年MM月dd日");
                    shremark2 = list[list.Count - 2].AUDITOPINION;
                    shname2 = list[list.Count - 2].AUDITPEOPLE;
                    date3 = list[list.Count - 2].AUDITTIME.Value.ToString("yyyy年MM月dd日");
                    list.RemoveRange(list.Count - 2, 2);
                    shname1 = string.Join("、", list.Select(t => t.AUDITPEOPLE).ToArray());
                    date2=list.Last().AUDITTIME.Value.ToString("yyyy年MM月dd日");
                }
                catch (Exception ex)
                {
                    
                }
            }
            Document doc = new Document(fileName);
            doc.MailMerge.ExecuteWithRegions(dt);
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.ExecuteWithRegions(dt2);
            doc.MailMerge.ExecuteWithRegions(dt3);
            doc.MailMerge.ExecuteWithRegions(dt4);
            doc.MailMerge.Execute(new string[] { "sname", "lawname", "ename", "applyTime", "idea", "dutyname", "deptname", "supervisiondept", "Time1", "Time2", "Time3", "applyperson", "outhead", "contractperiod", "engineerdirector","date1", "shname1", "date2", "shremark2", "shname2", "date3", "shremark3", "shname3", "date4" },
                new object[] { strproject, strlaw, strengineer, applyTime, senddeptidea, deptduty, skillperson, safeperson, time1, time2, time3, applyperson, outhead, contractperiod, engineerdirector, date1, shname1, date2, shremark2, shname2, date3 , shremark3, shname3, date4 });
            var docStream = new MemoryStream();
            doc.Save(docStream, SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
            Response.ContentType = "application/msword";
            Response.AddHeader("content-disposition", "attachment;filename=外包工程设备登记表.doc");
            Response.BinaryWrite(docStream.ToArray());
            Response.End();
        }


        /// <summary>
        /// 根据条件获取数据
        /// </summary>
        [HttpGet]
        public ActionResult GetOutDeptData(string outengineerid)
        {
            string[] str = new string[4];
            OutsouringengineerEntity eng = outsouringengineernll.GetEntity(outengineerid);
            if (eng != null)
            {
                var depart = new DepartmentBLL().GetList().Where(x => x.DepartmentId == eng.OUTPROJECTID).ToList().FirstOrDefault();
                str[0] = eng.ENGINEERNAME;//外包工程名称
                str[1] = depart != null ? depart.DepartmentId : "";//外包单位id
                str[2] = depart != null ? depart.FullName : "";//外包单位名称
                str[3] = depart != null ? depart.EnCode : "";//外包单位code
            }
            return ToJsonResult(str);

        }
    }
}
