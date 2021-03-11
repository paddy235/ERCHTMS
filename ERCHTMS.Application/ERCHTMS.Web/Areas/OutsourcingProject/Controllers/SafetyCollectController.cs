using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using System.Linq;
using System;
using System.Data;
using System.Web;
using Aspose.Words;
using Aspose.Words.Tables;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：外包-安全验收
    /// </summary>
    public class SafetyCollectController : MvcControllerBase
    {
        private SafetyCollectBLL SafetyCollectbll = new SafetyCollectBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //流程业务对象
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private OutsourcingprojectBLL outProjectbll = new OutsourcingprojectBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
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
            var data = SafetyCollectbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        //[HandlerMonitor(3, "分页查询用户信息!")]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            queryJson = queryJson ?? "";
            pagination.p_kid = "t.Id";
            pagination.p_fields = "t.EngineerId,t.CREATEUSERID,t.CREATEUSERDEPTCODE,t.CREATEUSERORGCODE,t.CREATEDATE,t.CREATEUSERNAME,o.engineerletdept,o.engineername,p.outsourcingname,t.FLOWNAME,t.FLOWROLE,t.FLOWROLENAME,t.FLOWDEPT,t.FLOWDEPTNAME,t.ISSAVED,t.ISOVER,t.FlowId";
            pagination.p_tablename = "EPG_SAFETYCOLLECT t left join EPG_OUTSOURINGENGINEER o on t.engineerid=o.id left join EPG_OUTSOURCINGPROJECT p on o.outprojectid=p.outprojectid";
            //pagination.conditionJson = "1=1";

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

            if (role.Contains("省级"))
            {
                pagination.conditionJson = string.Format(@" t.createuserorgcode  in (select encode
                from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", user.NewDeptCode);
            }
            else if (role.Contains("公司级用户") || role.Contains("厂级部门用户") || allrangedept.Contains(user.DeptId))
            {
                pagination.conditionJson = string.Format(" t.createuserorgcode  = '{0}'", user.OrganizeCode);
            }
            else if (role.Contains("承包商级用户"))
            {
                pagination.conditionJson = string.Format(" (o.outprojectid ='{0}' or o.supervisorid='{0}' or t.createuserid = '{1}')", user.DeptId, user.UserId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(user.DeptId);
                while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }
                pagination.conditionJson = string.Format(" o.engineerletdeptid  in (select departmentid from base_department where encode like '{0}%') ", deptentity.EnCode);

            }

            var watch = CommonHelper.TimerStart();
            var data = SafetyCollectbll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = SafetyCollectbll.GetEntity(keyValue);
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
            SafetyCollectbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyCollectEntity entity)
        {
            entity.ISSAVED = "0"; //标记申请中
            SafetyCollectbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #region 登记的内容提交到审核或者结束
        /// <summary>
        /// 登记的内容提交到审核或者结束（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, SafetyCollectEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string flowid = string.Empty;

            string moduleName = "竣工安全验收";

            // <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            //新增时会根据角色自动审核
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, "竣工安全验收");
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
            var outsouringengineer = outsouringengineerbll.GetEntity(entity.EngineerId);
            //先查出执行部门编码
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                {
                    var createdeptentity = new DepartmentBLL().GetEntity(outsouringengineer.ENGINEERLETDEPTID);
                    var createdeptentity2 = new DepartmentEntity();
                    while (createdeptentity.Nature == "专业" || createdeptentity.Nature == "班组")
                    {
                        createdeptentity2 = new DepartmentBLL().GetEntity(createdeptentity.ParentId);
                        if (createdeptentity2.Nature != "专业" || createdeptentity2.Nature != "班组") {
                            break;
                        }
                    }
                    powerList[i].CHECKDEPTCODE = createdeptentity.DeptCode;
                    powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;
                    if (createdeptentity2 != null) {
                        powerList[i].CHECKDEPTCODE = createdeptentity.DeptCode + "," + createdeptentity2.DeptCode;
                        powerList[i].CHECKDEPTID = createdeptentity.DepartmentId + "," + createdeptentity2.DepartmentId;
                    }
                }
                //创建部门
                if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                {
                    var createdeptentity = new DepartmentBLL().GetEntityByCode(curUser.DeptCode);
                    while (createdeptentity.Nature == "专业" || createdeptentity.Nature == "班组")
                    {
                        createdeptentity = new DepartmentBLL().GetEntity(createdeptentity.ParentId);
                    }
                    powerList[i].CHECKDEPTCODE = createdeptentity.DeptCode;
                    powerList[i].CHECKDEPTID = createdeptentity.DepartmentId;
                }
            }
            //登录人是否有审核权限--有审核权限直接审核通过
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTID.Contains(curUser.DeptId))
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
                state = "1";
                ManyPowerCheckEntity check = checkPower.Last();//当前

                for (int i = 0; i < powerList.Count; i++)
                {
                    if (check.ID == powerList[i].ID)
                    {
                        flowid = powerList[i].ID;
                    }
                }
            }
            else
            {
                state = "0";
                mpcEntity = powerList.First();
            }
            if (null != mpcEntity)
            {
                entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                entity.FLOWROLE = mpcEntity.CHECKROLEID;
                entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                entity.ISSAVED = "1"; //标记已经从登记到审核阶段
                entity.ISOVER = "0"; //流程未完成，1表示完成
                //entity.FLOWNAME = entity.FLOWDEPTNAME + "审核中";
                if (mpcEntity.CHECKDEPTNAME == "执行部门" && mpcEntity.CHECKROLENAME == "负责人")
                {
                    entity.FLOWNAME = outsouringengineer.ENGINEERLETDEPT + "审批中";
                }
                else
                {
                    entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审批中";
                }
                entity.FlowId = mpcEntity.ID;
                
            }
            else  //为空则表示已经完成流程
            {
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.ISSAVED = "1"; //标记已经从登记到审核阶段
                entity.ISOVER = "1"; //流程未完成，1表示完成
                entity.FLOWNAME = "";
                entity.FlowId = flowid;
            }
            SafetyCollectbll.SaveForm(keyValue, entity);

            //添加审核记录
            if (state == "1")
            {
                //审核信息表
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = "0"; //通过
                aidEntity.AUDITTIME = DateTime.Now;
                aidEntity.AUDITPEOPLE = curUser.UserName;
                aidEntity.AUDITPEOPLEID = curUser.UserId;
                aidEntity.APTITUDEID = entity.ID;  //关联的业务ID 
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
                aidEntity.AUDITDEPTID = curUser.DeptId;
                aidEntity.AUDITDEPT = curUser.DeptName;

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
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, SafetyCollectEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "竣工安全验收";


            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="outengineerid">工程Id</param>
            //ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, outengineerid);
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

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

            #region  //保存竣工安全验收记录
            var smEntity = SafetyCollectbll.GetEntity(keyValue);
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
                    smEntity.ISSAVED = "1";
                    smEntity.ISOVER = "0";
                    smEntity.FlowId = mpcEntity.ID;//赋值流程Id
                    smEntity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审批中";

                }
                else
                {
                    smEntity.FLOWDEPT = "";
                    smEntity.FLOWDEPTNAME = "";
                    smEntity.FLOWROLE = "";
                    smEntity.FLOWROLENAME = "";
                    smEntity.ISSAVED = "1";
                    smEntity.ISOVER = "1";
                    smEntity.FLOWNAME = "";
                }
            }
            else //审核不通过 归档
            {
                smEntity.FLOWDEPT = "";
                smEntity.FLOWDEPTNAME = "";
                smEntity.FLOWROLE = "";
                smEntity.FLOWROLENAME = "";
                smEntity.ISSAVED = "2"; //标记审核不通过
                smEntity.ISOVER = "1"; //流程结束
                smEntity.FLOWNAME = "";
                //smEntity.FlowId = mpcEntity.ID;//回退后流程Id清空
                //var applyUser = new UserBLL().GetEntity(smEntity.CREATEUSERID);
                //if (applyUser != null)
                //{
                //    JPushApi.PushMessage(applyUser.Account, smEntity.CREATEUSERNAME, "WB002", entity.ID);
                //}

            }
            //更新竣工安全验收基本状态信息
            SafetyCollectbll.SaveForm(keyValue, smEntity);
            #endregion

            #region    //审核不通过
            if (aentity.AUDITRESULT == "1")
            {
                //获取当前业务对象的所有审核记录
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //批量更新审核记录关联ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    //mode.APTITUDEID = hsentity.ID; //对应新的ID
                    //mode.REMARK = "99";
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
            }
            #endregion

            return Success("操作成功!");
        }
        #endregion
        #endregion

        #region 数据导出
        /// <summary>
        /// 导出用户列表
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "导出通知公告数据")]
        public ActionResult ExportData(string queryJson)
        {
            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "Id";
            pagination.p_fields = "(case isremind when 1 then '是' else '否' end) as IsRemind,Title,IssueDeptName,IssuerName,IssueTime";
            pagination.p_tablename = "HRS_SafetyCollect";
            pagination.conditionJson = "1=1";

            var watch = CommonHelper.TimerStart();
            var data = SafetyCollectbll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "通知公告";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "通知公告.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
            excelconfig.ColumnEntity = listColumnEntity;
            ColumnEntity columnentity = new ColumnEntity();
            //excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "r", ExcelColumn = "序号", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "isremind", ExcelColumn = "重要", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title", ExcelColumn = "标题", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuedeptname", ExcelColumn = "发布部门", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuername", ExcelColumn = "发布人", Alignment = "center" });
            excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "issuetime", ExcelColumn = "发布时间", Alignment = "center" });

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);

            return Success("导出成功。");
        }
        public ActionResult ExportSafetyCollect(string keyValue)
        {
            try
            {
                var userInfo = OperatorProvider.Provider.Current();  //获取当前用户
                var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == userInfo.OrganizeCode && x.ModuleCode == "RYZZSC").ToList();
                string tempPath = @"~/Resource/ExcelTemplate/外包工程竣工安全验收表.doc";
                var tempEntity = tempconfig.FirstOrDefault();
                string fileName = "外包工程竣工安全验收表_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";

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
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataSet ds = new DataSet();
            DataTable dtPro = new DataTable("project");
            dtPro.Columns.Add("EngineerName");//外包工程名称
            dtPro.Columns.Add("OUTPROJECTNAME");//承包单位名称
            dtPro.Columns.Add("OrgName");//电厂名称
            dtPro.Columns.Add("Reason");//申请理由
            dtPro.Columns.Add("AUDITSIGNIMG1");//承包商单位负责人

            dtPro.Columns.Add("AUDITOPINION1");  //部门负责人意见
            dtPro.Columns.Add("AUDITSIGNIMG2"); //部门负责人签名
            dtPro.Columns.Add("DATE1"); //部门审核时间

            dtPro.Columns.Add("AUDITOPINION2");  //生技部负责人意见
            dtPro.Columns.Add("AUDITSIGNIMG3"); //生技部负责人签名
            dtPro.Columns.Add("DATE2"); //生技部审核时间

            dtPro.Columns.Add("AUDITOPINION3");  //安环部负责人意见
            dtPro.Columns.Add("AUDITSIGNIMG4"); //安环部负责人签名
            dtPro.Columns.Add("DATE3"); //安环部审核时间

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            var sc = SafetyCollectbll.GetEntity(keyValue);
            

            DataRow row = dtPro.NewRow();
            if (sc != null)
            {
                OutsouringengineerEntity eng = outsouringengineerbll.GetEntity(sc.EngineerId);
                OutsourcingprojectEntity pro = outProjectbll.GetOutProjectInfo(eng.OUTPROJECTID);

                row["EngineerName"] = eng.ENGINEERNAME;
                row["OUTPROJECTNAME"] = pro.OUTSOURCINGNAME;
                row["OrgName"] = userInfo.OrganizeName;
                row["Reason"] = sc.Reason;
                row["AUDITSIGNIMG1"] = eng.UnitSuper;
                //审核记录
                List<AptitudeinvestigateauditEntity> list = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                #region 通用版本审核记录
                var i = 0;
                foreach (AptitudeinvestigateauditEntity entity in list)
                {
                    i++;
                    if (i == 1)
                    {
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["AUDITSIGNIMG2"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["AUDITSIGNIMG2"] = filepath;
                            }
                            else
                            {
                                row["AUDITSIGNIMG2"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("AUDITSIGNIMG2");
                        builder.InsertImage(row["AUDITSIGNIMG2"].ToString(), 80, 35);
                        row["AUDITOPINION1"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        row["DATE1"] = entity.AUDITTIME.Value.ToString("yyyy年MM月dd日");
                    }
                    if (i == 2)
                    {
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["AUDITSIGNIMG3"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["AUDITSIGNIMG3"] = filepath;
                            }
                            else
                            {
                                row["AUDITSIGNIMG3"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("AUDITSIGNIMG3");
                        builder.InsertImage(row["AUDITSIGNIMG3"].ToString(), 80, 35);
                        row["AUDITOPINION2"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        row["DATE2"] = entity.AUDITTIME.Value.ToString("yyyy年MM月dd日");
                    }
                    if (i == 3)
                    {
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["AUDITSIGNIMG4"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["AUDITSIGNIMG4"] = filepath;
                            }
                            else
                            {
                                row["AUDITSIGNIMG4"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("AUDITSIGNIMG4");
                        builder.InsertImage(row["AUDITSIGNIMG4"].ToString(), 80, 35);
                        row["AUDITOPINION3"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        row["DATE3"] = entity.AUDITTIME.Value.ToString("yyyy年MM月dd日");
                    }
                }
                #endregion
            }

            dtPro.Rows.Add(row);
            doc.MailMerge.Execute(dtPro);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), Aspose.Words.ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }
        #endregion
    }
}

