using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using BSFramework.Util.Offices;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Busines.PublicInfoManage;
using System.Linq;
using System.Web;
using Aspose.Cells;
using System.Drawing;
using BSFramework.Data;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：日常考核表
    /// </summary>
    public class DailyexamineController : MvcControllerBase
    {
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private HistorydailyexamineBLL historydailyexaminebll = new HistorydailyexamineBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();


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
            ViewBag.Code = dailyexaminebll.GetMaxCode();
            return View();
        }

        /// <summary>
        /// 审核页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApproveForm()
        {
            return View();
        }

        /// <summary>
        /// 历史清单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }
        /// <summary>
        /// 考核汇总
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExamineCollent()
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
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "examinecode,examinetype,examinetodept,examinemoney,examineperson,remark,flowdept,flowrolename,createuserid,createuserdeptcode,createuserorgcode,issaved,isover,examinecontent";
            pagination.p_tablename = " epg_dailyexamine";
            pagination.conditionJson = "1=1";
            //pagination.sidx = "createdate";
            //pagination.sord = "desc";
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = dailyexaminebll.GetPageList(pagination, queryJson);
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
        public ActionResult GetHistoryPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = "examinecode,examinetype,examinetodept,examinemoney,examineperson,remark,flowdept,flowrolename,createuserid,createuserdeptcode,createuserorgcode,issaved,isover,examinecontent";
            pagination.p_tablename = " epg_historydailyexamine";
            pagination.conditionJson = "1=1";
            //pagination.sidx = "createdate";
            //pagination.sord = "desc";
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = dailyexaminebll.GetPageList(pagination, queryJson);
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
            var data = dailyexaminebll.GetList(queryJson);
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
            try
            {
                var data = dailyexaminebll.GetEntity(keyValue);
                var exDept = new DepartmentBLL().GetEntity(data.ExamineToDeptId);
                if (exDept != null)
                {
                    if (exDept.Nature == "承包商")
                    {
                        var result = new
                        {
                            data = data,
                            iscbs = true
                        };
                        return ToJsonResult(result);
                    }
                    else
                    {
                        var result = new
                        {
                            data = data,
                            iscbs = false
                        };
                        return ToJsonResult(result);
                    }
                }
                else
                {
                    return ToJsonResult(data);
                }
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
           
        }


        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            //var data = historydailyexaminebll.GetEntity(keyValue);
            //return ToJsonResult(data);
            try
            {
                var data = historydailyexaminebll.GetEntity(keyValue);
                var exDept = new DepartmentBLL().GetEntity(data.ExamineToDeptId);
                if (exDept != null)
                {
                    if (exDept.Nature == "承包商")
                    {
                        var result = new
                        {
                            data = data,
                            iscbs = true
                        };
                        return ToJsonResult(result);
                    }
                    else
                    {
                        var result = new
                        {
                            data = data,
                            iscbs = false
                        };
                        return ToJsonResult(result);
                    }
                }
                else
                {
                    return ToJsonResult(data);
                }
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 日常考核汇总
        /// </summary>
        /// <param name="pagination">查询语句</param>
        /// <param name="queryJson">查询条件</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetExamineCollent(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.id";
            var table = @"(select 
                                        examinetodeptid,wm_concat(id) id,
                                        examinetodept,max(createdate) createdate,
                                          wm_concat(distinct(examineperson)) examineperson,
                                           to_char(min(examinetime),'yyyy-MM-dd')||'~'|| to_char(max(examinetime),'yyyy-MM-dd') examinetime,
                                        sum(examinemoney) examinemoney,
                                        wm_concat(examinetype) examinetype,createuserorgcode
                                        from epg_dailyexamine t where 1=1 {0} group by examinetodeptid,examinetodept,createuserorgcode) t";
            var strWhere = string.Empty;
            pagination.p_fields = @" t.examinetodeptid,
                                       t.examinetodept,
                                       t.examineperson,
                                       t.examinetime,
                                       t.examinemoney,
                                       t.examinetype";
          
            pagination.conditionJson = "1=1";
            pagination.sidx = "t.createdate";
            pagination.sord = "desc";
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and t.createuserorgcode='" + user.OrganizeCode + "'";
            }
            if (!string.IsNullOrEmpty(queryJson))
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["examinetodeptid"].IsEmpty())
                {
                    strWhere += " and t.examinetodeptid ='" + queryParam["examinetodeptid"].ToString() + "'";
                }
                if (!queryParam["examinetype"].IsEmpty())
                {
                    strWhere += " and t.examinetype='" + queryParam["examinetype"].ToString() + "'";
                }
                if (!queryParam["examinecontent"].IsEmpty())
                {
                    strWhere += " and t.examinecontent like '%" + queryParam["examinecontent"].ToString() + "%'";
                }
                //开始时间
                if (!queryParam["sTime"].IsEmpty())
                {
                    strWhere += string.Format(@" and t.examinetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["sTime"].ToString());
                }
                //结束时间
                if (!queryParam["eTime"].IsEmpty())
                {
                    strWhere += string.Format(@" and t.examinetime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["eTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                }
            }
            table = string.Format(table, strWhere);
            pagination.p_tablename = table;
            var data = dailyexaminebll.GetExamineCollent(pagination, queryJson);
            var jsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return ToJsonResult(jsonData);
            //var data = historydailyexaminebll.GetExamineCollent(keyValue);
            //return ToJsonResult(data);
        }

        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerMonitor(3, "删除数据")]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            dailyexaminebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, DailyexamineEntity entity)
        {
            try
            {
                entity.IsOver = 0;
                entity.IsSaved = 0;
                dailyexaminebll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception)
            {
                return Error("操作失败。");
            }
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
        public ActionResult SubmitForm(string keyValue, DailyexamineEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "日常考核";

            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="outengineerid">工程Id</param>
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            string flowid = string.Empty;
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
            //先查出执行部门编码
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                {
                    if (curUser.RoleName.Contains("班组") || curUser.RoleName.Contains("专业"))
                    {
                        var pDept = new DepartmentBLL().GetParentDeptBySpecialArgs(curUser.ParentId, "部门");
                        powerList[i].CHECKDEPTCODE = pDept.EnCode;
                        powerList[i].CHECKDEPTID = pDept.DepartmentId;
                    }
                    else
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(curUser.DeptId).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(curUser.DeptId).DepartmentId;
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
            //if (curUser.RoleName.Contains("公司级用户"))
            //{
            //    mpcEntity = null;
            //}
            if (null != mpcEntity)
            {
                //保存三措两案记录
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
            dailyexaminebll.SaveForm(keyValue, entity);

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
        #endregion

        #region 提交到审核或者结束
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

            string moduleName = "日常考核";

            DailyexamineEntity entity = dailyexaminebll.GetEntity(keyValue);
            /// <param name="currUser">当前登录人</param>
            /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            /// <param name="moduleName">模块名称</param>
            /// <param name="createdeptid">创建人部门ID</param>
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, entity.CreateUserDeptId);


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
            aidEntity.AUDITSIGNIMG = HttpUtility.UrlDecode(aidEntity.AUDITSIGNIMG);
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
            //更新日常考核基本状态信息
            dailyexaminebll.SaveForm(keyValue, entity);
            #endregion

            #region    //审核不通过
            if (aentity.AUDITRESULT == "1")
            {
                //添加历史记录
                HistorydailyexamineEntity hsentity = new HistorydailyexamineEntity();
                hsentity.CreateUserId = entity.CreateUserId;
                hsentity.CreateUserDeptCode = entity.CreateUserDeptCode;
                hsentity.CreateUserOrgCode = entity.CreateUserOrgCode;
                hsentity.CreateDate = entity.CreateDate;
                hsentity.CreateUserName = entity.CreateUserName;
                hsentity.CreateUserDeptId = entity.CreateUserDeptId;
                hsentity.ModifyDate = entity.ModifyDate;
                hsentity.ModifyUserId = entity.ModifyUserId;
                hsentity.ModifyUserName = entity.ModifyUserName;
                hsentity.ExamineCode = entity.ExamineCode;
                hsentity.ExamineDept = entity.ExamineDept;
                hsentity.ExamineDeptId = entity.ExamineDeptId;
                hsentity.ExamineToDeptId = entity.ExamineToDeptId;
                hsentity.ExamineToDept = entity.ExamineToDept;
                hsentity.ExamineType = entity.ExamineType; //关联ID
                hsentity.ExamineMoney = entity.ExamineMoney;
                hsentity.ExaminePerson = entity.ExaminePerson;
                hsentity.ExaminePersonId = entity.ExaminePersonId; //关联ID
                hsentity.ExamineTime = entity.ExamineTime;
                hsentity.ExamineContent = entity.ExamineContent;
                hsentity.ExamineBasis = entity.ExamineBasis;
                hsentity.Remark = entity.Remark;
                hsentity.ContractId = entity.Id;//关联ID
                hsentity.IsSaved = 2;
                hsentity.IsOver = entity.IsOver;
                hsentity.FlowDeptName = entity.FlowDeptName;
                hsentity.FlowDept = entity.FlowDept;
                hsentity.FlowRoleName = entity.FlowRoleName;
                hsentity.FlowRole = entity.FlowRole;
                hsentity.FlowName = entity.FlowName;
                hsentity.Project = entity.Project;
                hsentity.ProjectId = entity.ProjectId;
                hsentity.Id = "";

                historydailyexaminebll.SaveForm(hsentity.Id, hsentity);

                //获取当前业务对象的所有审核记录
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //批量更新审核记录关联ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.APTITUDEID = hsentity.Id; //对应新的ID
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
                //批量更新附件记录关联ID
                var flist = fileinfobll.GetImageListByObject(keyValue);
                foreach (FileInfoEntity fmode in flist)
                {
                    fmode.RecId = hsentity.Id; //对应新的ID
                    fileinfobll.SaveForm("", fmode);
                }
            }
            #endregion

            return Success("操作成功!");
        }
        #endregion

        #region 导出数据
        /// <summary>
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                var queryParam = queryJson.ToJObject();
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;
                pagination.p_kid = "id";
                pagination.p_fields = "examinecode,examinetype,examinecontent,examinetodept,examinemoney,examineperson,remark";
                pagination.p_tablename = " epg_dailyexamine";
                pagination.conditionJson = "1=1";
                pagination.sidx = "createdate";
                pagination.sord = "desc";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                }
                DataTable exportTable = dailyexaminebll.GetPageList(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();

                excelconfig.Title = "日常考核";
                excelconfig.FileName = "日常考核信息导出.xls";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;

                excelconfig.IsAllSizeColumn = true;
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                //需跟数据源列顺序保持一致
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinecode", ExcelColumn = "考核编号", Width = 120 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinetype", ExcelColumn = "考核类别", Width = 120 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinecontent", ExcelColumn = "考核内容", Width = 400 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinetodept", ExcelColumn = "被考核单位", Width = 160 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examinemoney", ExcelColumn = "考核金额", Width = 120 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "examineperson", ExcelColumn = "考核人", Width = 160 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "remark", ExcelColumn = "备注", Width = 400 });

                //调用导出方法
                //ExcelHelper.ExcelDownload(exportTable, excelconfig);
                ExcelHelper.ExportByAspose(exportTable, excelconfig.FileName, excelconfig.ColumnEntity);
            }
            catch (Exception ex)
            {

            }
            return Success("导出成功。");
        }

        /// <summary>
        /// 导出考核汇总
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportExamineData(string queryJson)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 1000000000;

                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                var table = @"(select 
                                        examinetodeptid,wm_concat(id) id,
                                        examinetodept,max(createdate) createdate,
                                          wm_concat(distinct(examineperson)) examineperson,
                                           to_char(min(examinetime),'yyyy-MM-dd')||'~'|| to_char(max(examinetime),'yyyy-MM-dd') examinetime,
                                        sum(examinemoney) examinemoney,
                                        wm_concat(examinetype) examinetype,createuserorgcode
                                        from epg_dailyexamine t where 1=1 {0} group by examinetodeptid,examinetodept,createuserorgcode) t";
                var strWhere = string.Empty;
                pagination.p_fields = @" t.examinetodeptid,
                                       t.examinetodept,
                                       t.examineperson,
                                       t.examinetime,
                                       t.examinemoney,
                                       t.examinetype";

                pagination.conditionJson = "1=1";
                pagination.sidx = "t.createdate";
                pagination.sord = "desc";
                if (!user.IsSystem)
                {
                    pagination.conditionJson += " and t.createuserorgcode='" + user.OrganizeCode + "'";
                }
                if (!string.IsNullOrEmpty(queryJson))
                {
                    var queryParam = queryJson.ToJObject();
                    if (!queryParam["examinetodeptid"].IsEmpty())
                    {
                        strWhere += " and t.examinetodeptid ='" + queryParam["examinetodeptid"].ToString() + "'";
                    }
                    if (!queryParam["examinetype"].IsEmpty())
                    {
                        strWhere += " and t.examinetype='" + queryParam["examinetype"].ToString() + "'";
                    }
                    if (!queryParam["examinecontent"].IsEmpty())
                    {
                        strWhere += " and t.examinecontent like '%" + queryParam["examinecontent"].ToString() + "%'";
                    }
                    //开始时间
                    if (!queryParam["sTime"].IsEmpty())
                    {
                        strWhere += string.Format(@" and t.examinetime >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["sTime"].ToString());
                    }
                    //结束时间
                    if (!queryParam["eTime"].IsEmpty())
                    {
                        strWhere += string.Format(@" and t.examinetime < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["eTime"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
                    }
                }
                table = string.Format(table, strWhere);
                pagination.p_tablename = table;
                var data = dailyexaminebll.GetExportExamineCollent(pagination, queryJson);
                Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
                string fName = "日常考核汇总_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
                var num = wb.Worksheets[0].Cells.Columns.Count;

                Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
                Aspose.Cells.Cell cell = sheet.Cells[0, 0];
                cell.PutValue("考核汇总表"); //标题
                cell.Style.Pattern = BackgroundType.Solid;
                cell.Style.Font.Size = 16;
                cell.Style.Font.Color = Color.Black;
                List<string> colList = new List<string>() { "被考核单位", "考核金额", "考核类型", "考核人", "考核时间" };
                List<string> colList1 = new List<string>() { "examinetodept", "examinemoney", "examinetype", "examineperson", "examinetime" };
                for (int i = 0; i < colList.Count; i++)
                {
                    //序号列
                    Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                    serialcell.PutValue(" ");

                    for (int j = 0; j < colList.Count; j++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[1, j + 1];
                        sheet.Cells.SetColumnWidth(j + 1, 40);
                        curcell.Style.Pattern = BackgroundType.Solid;
                        curcell.Style.Font.Size = 12;
                        curcell.Style.Font.Color = Color.Black;
                        curcell.PutValue(colList[j].ToString()); //列头
                    }
                    Aspose.Cells.Cells cells = sheet.Cells;
                    cells.Merge(0, 0, 1, colList.Count + 1);
                }
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    //序列号
                    Aspose.Cells.Cell serialcell = sheet.Cells[i + 2, 0];
                    if (string.IsNullOrWhiteSpace(data.Rows[i]["parent"].ToString())) {
                        serialcell.PutValue("合计");
                    }
                    
                    //内容填充
                    for (int j = 0; j < colList1.Count; j++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[i + 2, j + 1];
                        curcell.PutValue(data.Rows[i][colList1[j]].ToString());
                    }
                   
                }
                HttpResponse resp = System.Web.HttpContext.Current.Response;
                wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
                return Success("导出成功。", fName);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
