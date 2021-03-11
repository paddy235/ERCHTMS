using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using ERCHTMS.Entity.PowerPlantInside;
using ERCHTMS.Busines.PowerPlantInside;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Aspose.Words.Lists;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using ServiceStack.Text;
using Svg;
using Svg.Transforms;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.PublicInfoManage;

namespace ERCHTMS.Web.Areas.PowerPlantInside.Controllers
{
    /// <summary>
    /// 描 述：单位内部快报
    /// </summary>
    public class PowerplantinsideController : MvcControllerBase
    {
        private PowerplantinsideBLL powerplantinsidebll = new PowerplantinsideBLL();
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

        /// <summary>
        /// 统计页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PowerplantStatistics()
        {
            return View();
        }

        /// <summary>
        /// 审核页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ApproveForm()
        {
            return View();
        }

        /// <summary>
        /// 选择页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
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
            var data = powerplantinsidebll.GetList(queryJson);
            return ToJsonResult(data);
        }

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
            pagination.p_fields = @"a.CreateUserId,a.CreateDate,a.CreateUserName,a.ModifyUserId,a.ModifyDate,a.ModifyUserName,a.CreateUserDeptCode,a.CreateUserOrgCode,a.accidenteventname,a.accidenteventno,f.itemname as belongsystem,a.district,
           b.itemname as accidenteventtype,a.accidenteventtype as accidenteventtypevalue,c.itemname as accidenteventproperty,a.accidenteventproperty as accidenteventpropertyvalue,a.accidenteventcausename as accidenteventcause,
            to_char(a.happentime,'yyyy-MM-dd HH24:mi') happentime,a.belongdept,a.belongdeptid,a.belongdeptcode,e.itemname as specialty,a.issaved,a.isover,a.flowdeptname,a.flowdept,a.flowrolename,a.flowrole,a.flowname,a.flowid ";
            pagination.p_tablename = @"BIS_POWERPLANTINSIDE a
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
              left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where   itemcode = 'SpecialtyType') ) e on a.Specialty = e.itemvalue
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where   itemcode = 'BelongSystem') ) f on a.belongsystem = f.itemvalue";
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
            var data = powerplantinsidebll.GetPageList(pagination, queryJson);
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
        public ActionResult GetFormJson(string keyValue)
        {
            var data = powerplantinsidebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 年度变化统计列表
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public string GetStatisticsList(int year, string mode)
        {
            return powerplantinsidebll.GetStatisticsList(year,mode);
        }
        /// <summary>
        ///月度变化统计图
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet]
        public string GetStatisticsHighchart(string year, string mode)
        {
            return powerplantinsidebll.GetStatisticsHighchart(year, mode);
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
            powerplantinsidebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, PowerplantinsideEntity entity)
        {
            entity.IsOver = 0;
            entity.IsSaved = 0;
            powerplantinsidebll.SaveForm(keyValue, entity);
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
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, PowerplantinsideEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "事故事件快报-内部审核";

            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="outengineerid">工程Id</param>
            ManyPowerCheckEntity mpcEntity = powerplantinsidebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            string flowid = string.Empty;
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
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
                entity.IsOver = 0; //流程未完成，1表示完成
                entity.FlowID = mpcEntity.ID;
                entity.FlowName = mpcEntity.CHECKDEPTNAME + "审核中";
            }
            else  //为空则表示已经完成流程
            {
                entity.FlowDept = "";
                entity.FlowDeptName = "";
                entity.FlowRole = "";
                entity.FlowRoleName = "";
                entity.IsSaved = 1; //标记已经从登记到审核阶段
                entity.IsOver = 1; //流程未完成，1表示完成
                entity.FlowName = "";
                entity.FlowID = flowid;
            }
            powerplantinsidebll.SaveForm(keyValue, entity);

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
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "事故事件快报-内部审核";

            PowerplantinsideEntity entity = powerplantinsidebll.GetEntity(keyValue);
            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="createdeptid">创建人部门ID</param>
            ManyPowerCheckEntity mpcEntity = powerplantinsidebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);


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
            aidEntity.FlowId = entity.FlowID;
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

            #region  //保存日常考核
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
                    entity.IsOver = 0;
                    entity.FlowID = mpcEntity.ID;
                    entity.FlowName = mpcEntity.CHECKDEPTNAME + "审核中";
                }
                else
                {
                    entity.FlowDept = "";
                    entity.FlowDeptName = "";
                    entity.FlowRole = "";
                    entity.FlowRoleName = "";
                    entity.IsSaved = 1;
                    entity.IsOver = 1;
                    entity.FlowName = "";
                }
            }
            else //审核不通过 
            {
                entity.FlowDept = "";
                entity.FlowDeptName = "";
                entity.FlowRole = "";
                entity.FlowRoleName = "";
                entity.IsOver = 0; //处于登记阶段
                entity.IsSaved = 0; //是否完成状态赋值为未完成
                entity.FlowName = "";
                entity.FlowID = "";

            }
            //更新事故事件基本状态信息
            powerplantinsidebll.SaveForm(keyValue, entity);
            #endregion

            #region    //审核不通过
            if (aentity.AUDITRESULT == "1")
            {
                ////添加历史记录
                //HistorydailyexamineEntity hsentity = new HistorydailyexamineEntity();
                //hsentity.CreateUserId = entity.CreateUserId;
                //hsentity.CreateUserDeptCode = entity.CreateUserDeptCode;
                //hsentity.CreateUserOrgCode = entity.CreateUserOrgCode;
                //hsentity.CreateDate = entity.CreateDate;
                //hsentity.CreateUserName = entity.CreateUserName;
                //hsentity.CreateUserDeptId = entity.CreateUserDeptId;
                //hsentity.ModifyDate = entity.ModifyDate;
                //hsentity.ModifyUserId = entity.ModifyUserId;
                //hsentity.ModifyUserName = entity.ModifyUserName;
                //hsentity.ExamineCode = entity.ExamineCode;
                //hsentity.ExamineDept = entity.ExamineDept;
                //hsentity.ExamineDeptId = entity.ExamineDeptId;
                //hsentity.ExamineToDeptId = entity.ExamineToDeptId;
                //hsentity.ExamineToDept = entity.ExamineToDept;
                //hsentity.ExamineType = entity.ExamineType; //关联ID
                //hsentity.ExamineMoney = entity.ExamineMoney;
                //hsentity.ExaminePerson = entity.ExaminePerson;
                //hsentity.ExaminePersonId = entity.ExaminePersonId; //关联ID
                //hsentity.ExamineTime = entity.ExamineTime;
                //hsentity.ExamineContent = entity.ExamineContent;
                //hsentity.ExamineBasis = entity.ExamineBasis;
                //hsentity.Remark = entity.Remark;
                //hsentity.ContractId = entity.Id;//关联ID
                //hsentity.IsSaved = 2;
                //hsentity.IsOver = entity.IsOver;
                //hsentity.FlowDeptName = entity.FlowDeptName;
                //hsentity.FlowDept = entity.FlowDept;
                //hsentity.FlowRoleName = entity.FlowRoleName;
                //hsentity.FlowRole = entity.FlowRole;
                //hsentity.FlowName = entity.FlowName;
                //hsentity.Id = "";

                //historydailyexaminebll.SaveForm(hsentity.Id, hsentity);

                //获取当前业务对象的所有审核记录
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //批量更新审核记录关联ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.Disable = "1";
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
                ////批量更新附件记录关联ID
                //var flist = fileinfobll.GetImageListByObject(keyValue);
                //foreach (FileInfoEntity fmode in flist)
                //{
                //    fmode.RecId = hsentity.Id; //对应新的ID
                //    fileinfobll.SaveForm("", fmode);
                //}
            }
            #endregion

            return Success("操作成功!");
        }
        #endregion

        #region 数据导出
        /// <summary>
        /// 事故事件快报
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "单位内部事故事件快报")]
        public ActionResult ExportBulletinList(string condition, string queryJson)
        {
            Pagination pagination = new Pagination();
            queryJson = queryJson ?? "";
            pagination.page = 1;
            pagination.rows = 100000000;
            pagination.p_kid = "ID";
            pagination.p_fields = @"a.accidenteventname,a.accidenteventno,
           b.itemname as accidenteventtype,c.itemname as accidenteventproperty,f.itemname as belongsystem,a.accidenteventcausename as accidenteventcause,a.happentime,a.district,a.belongdept,e.itemname as specialty ";
            pagination.p_tablename = @"BIS_POWERPLANTINSIDE a 
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventType') ) b on a.accidenteventtype = b.itemvalue
              left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'AccidentEventProperty') ) c on a.accidenteventproperty = c.itemvalue
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where   itemcode = 'SpecialtyType' ) ) e on a.Specialty = e.itemvalue
            left join ( select * from base_dataitemdetail  where itemid = ( select itemid from base_dataitem where  parentid = 
            (select itemid from base_dataitem where itemname = '单位内部快报' ) and  itemcode = 'BelongSystem') ) f on a.belongsystem = f.itemvalue";
            pagination.sord = "CreateDate";
            #region 权限校验
            pagination.conditionJson = "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (user.IsSystem)
            {
                pagination.conditionJson = "1=1";
            }
            else
            {
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }

            }
            #endregion
            var data = powerplantinsidebll.GetPageList(pagination, queryJson);

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();
            excelconfig.Title = "单位内部事故事件快报";
            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.FileName = "单位内部事故事件快报.xls";
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
            List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();

            ColumnEntity columnentity = new ColumnEntity();

            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventname".ToLower(), ExcelColumn = "事故/事件名称" });
            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventno".ToLower(), ExcelColumn = "编号" });
            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventtype".ToLower(), ExcelColumn = "事故或事件类型" });
            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventproperty".ToLower(), ExcelColumn = "事故或事件性质" });
            listColumnEntity.Add(new ColumnEntity() { Column = "belongsystem".ToLower(), ExcelColumn = "所属系统" });
            listColumnEntity.Add(new ColumnEntity() { Column = "accidenteventcause".ToLower(), ExcelColumn = "影响事故事件因素" });
            listColumnEntity.Add(new ColumnEntity() { Column = "happentime".ToLower(), ExcelColumn = "发生时间", Alignment = "Center" });
            listColumnEntity.Add(new ColumnEntity() { Column = "district".ToLower(), ExcelColumn = "地点(区域)" });
            listColumnEntity.Add(new ColumnEntity() { Column = "belongdept".ToLower(), ExcelColumn = "所属部门/单位" });
            listColumnEntity.Add(new ColumnEntity() { Column = "specialty".ToLower(), ExcelColumn = "相关专业" });
            excelconfig.ColumnEntity = listColumnEntity;

            //调用导出方法
            ExcelHelper.ExcelDownload(data, excelconfig);
            return Success("导出成功。");
        }

        /// <summary>
        /// 事故事件快报
        /// </summary>
        /// <returns></returns>
        [HandlerMonitor(0, "事故事件统计")]
        public ActionResult ExportStatisticsExcel(int year, string mode)
        {
            string jsonList = powerplantinsidebll.GetStatisticsList(year, mode); ;

            dynamic dyObj = JsonConvert.DeserializeObject(jsonList);
            ;
            DataTable tb = JsonToDataTable(dyObj.rows.ToString());

            //设置导出格式
            ExcelConfig excelconfig = new ExcelConfig();

            excelconfig.TitleFont = "微软雅黑";
            excelconfig.TitlePoint = 25;
            excelconfig.IsAllSizeColumn = true;
            //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出

            switch (mode)
            {
                case "0":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "年事故事件类型统计";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "年事故事件类型统计" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "type", ExcelColumn = "类型", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "人身", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "设备", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "消防", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "交通", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "环保", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "职业健康", Alignment = "center"},                     
                        new ColumnEntity() {Column = "Total", ExcelColumn = "总计", Alignment = "center"}
                    };
                    break;
                case "1":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "年事故事件性质统计";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "年事故事件性质统计" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "type", ExcelColumn = "性质", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "二类障碍", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "异常", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "未遂", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "小微事件", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "一类障碍", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "一般", Alignment = "center"},                      
                        new ColumnEntity() {Column = "Total", ExcelColumn = "总计", Alignment = "center"}
                    };
                    break;
                case "2":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "年影响事故事件因素统计";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "年影响事故事件因素统计" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "TypeName", ExcelColumn = "影响因素", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "1月", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "2月", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "3月", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "4月", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "5月", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "6月", Alignment = "center"},
                        new ColumnEntity() {Column = "num7", ExcelColumn = "7月", Alignment = "center"},
                        new ColumnEntity() {Column = "num8", ExcelColumn = "8月", Alignment = "center"},
                        new ColumnEntity() {Column = "num9", ExcelColumn = "9月", Alignment = "center"},
                        new ColumnEntity() {Column = "num10", ExcelColumn = "10月", Alignment = "center"},
                        new ColumnEntity() {Column = "num11", ExcelColumn = "11月", Alignment = "center"},
                        new ColumnEntity() {Column = "num12", ExcelColumn = "12月", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "总计", Alignment = "center"}
                    };
                    break;
                case "3":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "年事故事件发生的部门统计";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "年事故事件发生的部门统计" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "dept", ExcelColumn = "各个部门", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "营销部", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "技术支持部", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "发电部", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "办公室", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "EHS部", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "相关方", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "全厂", Alignment = "center"}
                    };
                    break;
                case "4":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "年事故事件所属专业统计";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "年事故事件所属专业统计" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                       new ColumnEntity() {Column = "type", ExcelColumn = "所属专业", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "汽机专业", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "锅炉专业", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "电气专业", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "灰硫专业", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "化学专业", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "燃料专业", Alignment = "center"},
                        new ColumnEntity() {Column = "num7", ExcelColumn = "热控专业", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "总计", Alignment = "center"}
                    };
                    break;
                case "5":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "年事故事件所属机组统计";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "年事故事件所属机组统计" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "type", ExcelColumn = "所属系统", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "#1机组", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "#2机组", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "#3机组", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "#4机组", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "公用系统", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "总计", Alignment = "center"}
                    };
                    break;
                case  "6":
                    excelconfig.Title = year + "-" + DateTime.Now.Year + "年事故事件月度变化统计";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "年事故事件月度变化统计" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>()
                    {
                        new ColumnEntity() {Column = "cs", ExcelColumn = "月份", Alignment = "center"},
                        new ColumnEntity() {Column = "num1", ExcelColumn = "1月", Alignment = "center"},
                        new ColumnEntity() {Column = "num2", ExcelColumn = "2月", Alignment = "center"},
                        new ColumnEntity() {Column = "num3", ExcelColumn = "3月", Alignment = "center"},
                        new ColumnEntity() {Column = "num4", ExcelColumn = "4月", Alignment = "center"},
                        new ColumnEntity() {Column = "num5", ExcelColumn = "5月", Alignment = "center"},
                        new ColumnEntity() {Column = "num6", ExcelColumn = "6月", Alignment = "center"},
                        new ColumnEntity() {Column = "num7", ExcelColumn = "7月", Alignment = "center"},
                        new ColumnEntity() {Column = "num8", ExcelColumn = "8月", Alignment = "center"},
                        new ColumnEntity() {Column = "num9", ExcelColumn = "9月", Alignment = "center"},
                        new ColumnEntity() {Column = "num10", ExcelColumn = "10月", Alignment = "center"},
                        new ColumnEntity() {Column = "num11", ExcelColumn = "11月", Alignment = "center"},
                        new ColumnEntity() {Column = "num12", ExcelColumn = "12月", Alignment = "center"},
                        new ColumnEntity() {Column = "Total", ExcelColumn = "总计", Alignment = "center"}
                    };
                    break;
                case "7":
                    int newyear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
                    excelconfig.Title = year + "-" + newyear + "年事故事件年度变化统计";
                    excelconfig.FileName = year + "-" + DateTime.Now.Year + "年事故事件年度变化统计" + ".xls";
                    excelconfig.ColumnEntity = new List<ColumnEntity>();
                    excelconfig.ColumnEntity.AddIfNotExists(new ColumnEntity() { Column = "cs", ExcelColumn = "年份", Alignment = "center" });
                    for (int i = year + 1; i <= newyear; i++)
                    {
                        excelconfig.ColumnEntity.AddIfNotExists(new ColumnEntity() { Column = "num" + i, ExcelColumn = i + "年", Alignment = "center" });
                    }

                    break;
            }
           

            //调用导出方法
            ExcelHelper.ExportByAspose(tb, excelconfig.FileName, excelconfig.ColumnEntity);
            return Success("导出成功。");
        }

        #region 导出图片
        //HighCharts 导出图片 svg
        //filename type width scale svg
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Export(FormCollection fc)
        {
            string tType = fc["type"];
            string tSvg = fc["svg"];
            string tFileName = fc["filename"];
            string tWidth = fc["width"];
            if (string.IsNullOrEmpty(tFileName))
            {
                tFileName = "DefaultChart";
            }
            MemoryStream tData = new MemoryStream(Encoding.UTF8.GetBytes(tSvg));
            Svg.SvgDocument tSvgObj = SvgDocument.Open<SvgDocument>(tData);
            tSvgObj.Transforms = new SvgTransformCollection();
            float scalar = (float)int.Parse(tWidth) / (float)tSvgObj.Width;
            tSvgObj.Transforms.Add(new SvgScale(scalar, scalar));
            tSvgObj.Width = new SvgUnit(tSvgObj.Width.Type, tSvgObj.Width * scalar);
            tSvgObj.Height = new SvgUnit(tSvgObj.Height.Type, tSvgObj.Height * scalar);
            MemoryStream tStream = new MemoryStream();
            string tTmp = new Random().Next().ToString();
            string tExt = "";

            switch (tType)
            {
                case "image/png":
                    tExt = "png";
                    break;
                case "image/jpeg":
                    tExt = "jpg";
                    break;
                case "application/pdf":
                    tExt = "pdf";
                    break;
                case "image/svg+xml":
                    tExt = "svg";
                    break;
            }

            // Svg.SvgDocument tSvgObj = SvgDocument.Open<SvgDocument>(tData);
            switch (tExt)
            {
                case "jpg":
                    tSvgObj.Draw().Save(tStream, ImageFormat.Jpeg);
                    break;
                case "png":
                    tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                    break;
                case "pdf":
                    PdfWriter tWriter = null;
                    Document tDocumentPdf = null;
                    try
                    {
                        tSvgObj.Draw().Save(tStream, ImageFormat.Png);
                        tDocumentPdf = new Document(new iTextSharp.text.Rectangle((float)tSvgObj.Width, (float)tSvgObj.Height));
                        tDocumentPdf.SetMargins(0.0f, 0.0f, 0.0f, 0.0f);
                        iTextSharp.text.Image tGraph = iTextSharp.text.Image.GetInstance(tStream.ToArray());
                        tGraph.ScaleToFit((float)tSvgObj.Width, (float)tSvgObj.Height);

                        tStream = new MemoryStream();
                        tWriter = PdfWriter.GetInstance(tDocumentPdf, tStream);
                        tDocumentPdf.Open();
                        tDocumentPdf.NewPage();
                        tDocumentPdf.Add(tGraph);
                        tDocumentPdf.Close();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        tDocumentPdf.Close();
                        tWriter.Close();
                        tData.Dispose();
                        tData.Close();

                    }
                    break;
                case "svg":
                    tStream = tData;
                    break;
            }
            tFileName = tFileName + "." + tExt;
            return File(tStream.ToArray(), tType, tFileName);
        }

        #endregion
        #endregion

        #region Json 字符串 转换为 DataTable数据集合
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合 格式[{"xxx":"yyy","x1":"yy2"},{"x2":"y2","x3":"y4"}]
        /// </summary>  
        /// <param name="json"></param>
        /// <returns></returns>
        public DataTable JsonToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion
    }
}
