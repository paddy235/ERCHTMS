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
using System.Drawing;
using BSFramework.Data;
using BSFramework.Util.Extension;
using Aspose.Words;
using Aspose.Words.Tables;
using ERCHTMS.Busines.AuthorizeManage;
using System.Text;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：安全考核主表
    /// </summary>
    public class SafetyAssessmentController : MvcControllerBase
    {
        private SafetyAssessmentBLL safetyassessmentbll = new SafetyAssessmentBLL();
        private SafetyassessmentpersonBLL safetyassessmentpersonbll = new SafetyassessmentpersonBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private HistorysafetyassessmentBLL historysafetyassessmentbll = new HistorysafetyassessmentBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private UserBLL userbll = new UserBLL();
        private SafestandardBLL safestandardbll = new SafestandardBLL();
        private SafestandarditemBLL safestandarditembll = new SafestandarditemBLL();

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
        /// 历史页面
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
        public ActionResult Form()
        {
            ViewBag.Code = safetyassessmentbll.GetMaxCode();
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScoreForm()
        {
            return View();
        }

        /// <summary>
        /// 流程图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FlowForm()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApplyForm()
        {
            return View();
        }

        /// <summary>
        /// 导出页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportTotalForm()
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
            var data = safetyassessmentbll.GetList(queryJson);
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
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            //string strfild = "(select to_char(wm_concat(to_char(c.SCORE))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) applyscore,";
            //strfild += "(select to_char(wm_concat(to_char(c.EVALUATESCORE))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) applyevaluatescore,";
            //strfild += "(select to_char(wm_concat(to_char(c.EVALUATECONTENT))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) applycontent,";
            // isapply 大于0说明可以审批，等于0说明不是负责人不能审批
            pagination.p_fields =  "(select to_char(wm_concat(to_char(c.EVALUATEDEPTNAME))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) as examinetodept,  case when((flowdept = '" + user.DeptId + "') and instr('" + user.RoleName + "',FLOWROLENAME) >0) then 1 else 0 end isapply, createuserid, createuserdeptcode, createuserorgcode, createdate, createusername, modifydate, modifyuserid, modifyusername, flowid, numcode, examinecode, examinedept, examinedeptid, examineperson, examinepersonid, examinetype,  to_char(t.examinetime,'yyyy-MM-dd') as examinetime, examinereason, examinebasis, flowdeptname, flowdept, flowrolename, flowrole, flowname, issaved, isover, evaluatetype,examinetypename";
            pagination.p_tablename = " epg_safetyassessment t ";

            //pagination.conditionJson = "1=1";
            pagination.conditionJson = "((CREATEUSERID = '" + user.UserId + "') or( ISSAVED = '1'))";
            if (!user.IsSystem)
            {

                string authType = new AuthorizeBLL().GetOperAuthorzeType(user, HttpContext.Request.Cookies["currentmoduleId"].Value, "search");
                if (!string.IsNullOrEmpty(authType))
                {

                    switch (authType)
                    {
                        case "1":
                            pagination.conditionJson += " and createuserid='" + user.UserId + "'";
                            break;
                        case "2":
                            pagination.conditionJson += " and CREATEUSERDEPTCODE='" + user.DeptCode + "'";
                            break;
                        case "3":
                            var deptentity = departmentbll.GetEntity(user.DeptId);
                            while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                            {
                                deptentity = departmentbll.GetEntity(deptentity.ParentId);
                            }
                            pagination.conditionJson += " and (CREATEUSERDEPTCODE like '" + user.DeptCode + "%' or CREATEUSERDEPTCODE in(select dept.ENCODE from base_department dept where dept.DEPARTMENTID in ( select a.outprojectid from epg_outsouringengineer a where a.engineerletdeptid in (select departmentid from base_department where encode like '" + deptentity.EnCode + "%'))) )";
                            break;
                        case "4":
                            pagination.conditionJson += " and CREATEUSERDEPTCODE like '" + user.OrganizeCode + "%'";
                            break;
                        //case "5":
                        //    pagination.conditionJson += string.Format(" and createdeptcode in(select encode from BASE_DEPARTMENT where deptcode like '{0}%')", user.NewDeptCode);
                        //    break;
                    }
                }
                else
                {
                    //pagination.conditionJson += " and createdeptcode like '" + user.OrganizeCode + "%'";
                }

            }
            // 默认当前人或者机构提交的人可以看
            //pagination.conditionJson = "( (CREATEUSERID = '" + user.UserId + "') or (CREATEUSERORGCODE = '" + user.OrganizeCode + "' and ISSAVED = '1'))";
            //pagination.conditionJson = "()";
            //pagination.sidx = "createdate";
            //pagination.sord = "desc";
            // 
            //if (!user.IsSystem)
            //{
            //    pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            //}
            var data = safetyassessmentbll.GetPageList(pagination, queryJson);
            data.Columns.Add("applycontent", Type.GetType("System.String"));
            foreach (DataRow dr in data.Rows)
            {
                var personeva =  safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '"+ dr["id"]+ "' ");
                string applycontent = "";
                int num = 0;
                foreach (SafetyassessmentpersonEntity pe in personeva)
                {
                    applycontent += pe.EVALUATEDEPTNAME +"("+ pe.SCORE + "/"+pe.EVALUATESCORE+"/"+pe.EVALUATECONTENT + ")";
                    if (personeva.Count()-1 > num)
                    {
                        applycontent += "、";
                    }
                    num++;
                }
                dr["applycontent"] = applycontent;
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


        /// <summary>
        /// 获取历史列表
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
            // isapply 大于0说明可以审批，等于0说明不是负责人不能审批
            pagination.p_fields = " (select to_char(wm_concat(to_char(c.EVALUATEDEPTNAME))) from EPG_SAFETYASSESSMENTPERSON c  where c.SAFETYASSESSMENTID=t.ID) as examinetodept,  case when((flowdept = '" + user.DeptId + "') and instr('" + user.RoleName + "',FLOWROLENAME) >0) then 1 else 0 end isapply, createuserid, createuserdeptcode, createuserorgcode, createdate, createusername, modifydate, modifyuserid, modifyusername, flowid, numcode, examinecode, examinedept, examinedeptid, examineperson, examinepersonid, examinetype, to_char(t.examinetime,'yyyy-MM-dd') as examinetime, examinereason, examinebasis, flowdeptname, flowdept, flowrolename, flowrole, flowname, issaved, isover, evaluatetype,examinetypename";
            pagination.p_tablename = " epg_historysafetyassessment t ";

            pagination.conditionJson = "1=1";
            // 默认当前人和审核人可以看
            //pagination.conditionJson = "()";
            //pagination.sidx = "createdate";
            //pagination.sord = "desc";
            // 
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
            }
            var data = safetyassessmentbll.GetPageList(pagination, queryJson);
            data.Columns.Add("applycontent", Type.GetType("System.String"));
            foreach (DataRow dr in data.Rows)
            {
                var personeva = safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '" + dr["id"] + "' ");
                string applycontent = "";
                int num = 0;
                foreach (SafetyassessmentpersonEntity pe in personeva)
                {
                    applycontent += pe.EVALUATEDEPTNAME + "(" + pe.SCORE + "/" + pe.EVALUATESCORE + "/" + pe.EVALUATECONTENT + ")";
                    if (personeva.Count() - 1 > num)
                    {
                        applycontent += "、";
                    }
                    num++;
                }
                dr["applycontent"] = applycontent;
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


        /// <summary>
        /// 获取考核评分列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetScoreListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @"  createuserid,scoretype, score, evaluatetype, evaluatetypename, evaluatedept, evaluatedeptname, evaluatescore, evaluatecontent, evaluateother, safetyassessmentid  ";
            pagination.p_tablename = @" epg_safetyassessmentperson t ";

            var queryParam = queryJson.ToJObject();
            if (!queryParam["ID"].IsEmpty() && !queryParam["ID"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" t.safetyassessmentid = '{0}'", queryParam["ID"].ToString());
            }

            var watch = CommonHelper.TimerStart();
            var data = safetyassessmentpersonbll.GetList(pagination, queryJson);
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
            var data = safetyassessmentbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取历史记录数 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJsontotal(string keyValue)
        {
            int resultNum = safetyassessmentbll.GetFormJsontotal(keyValue);
            return ToJsonResult(resultNum);
        }

        /// <summary>
        /// 获取历史实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            var data = historysafetyassessmentbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取考核信息实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetScoreFormJson(string keyValue)
        {
            var data = safetyassessmentpersonbll.GetEntity(keyValue);
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
            safetyassessmentbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 删除考核数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveScoreForm(string keyValue)
        {
            safetyassessmentpersonbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SafetyAssessmentEntity entity)
        {
            try
            {
                entity.IsOver = 0;
                entity.IsSaved = 0;
                if (entity.EXAMINECODE == "" || entity.EXAMINECODE == null)
                {
                    entity.EXAMINECODE = safetyassessmentbll.GetMaxCode();
                }
                safetyassessmentbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception e)
            {
                //return Error("操作失败。");
                return Error(e.Message);
            }
        }

        /// <summary>
        /// 保存考核信息表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveScoreForm(string keyValue, SafetyassessmentpersonEntity entity)
        {
            try
            {
                safetyassessmentpersonbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (Exception e)
            {

                return Success(e.Message);
            }
            
            
        }

        /// <summary>
        /// 根据案件主键ID删除考核信息表
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public ActionResult DelByKeyId(string keyValue)
        {
            try
            {
                safetyassessmentpersonbll.DelByKeyId(keyValue);
                return Success("切换成功。");
            }
            catch (Exception e)
            {

                return Success(e.Message);
            }


        }


        #region 
        /// <summary>
        /// 提交（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, SafetyAssessmentEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string result = "0";
            var personeva = safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '" + keyValue + "' ");
            if (personeva.Count() > 0)
            {
                string moduleName = "安全考核审核";

                /// <param name="currUser">当前登录人</param>
                /// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
                /// <param name="moduleName">模块名称</param>
                /// <param name="outengineerid">工程Id</param>
                ManyPowerCheckEntity mpcEntity = safetyassessmentbll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId, "0");

                string flowid = string.Empty;

                if (null != mpcEntity)
                {
                    //保存安全考核记录
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.IsSaved = 1; //标记已经从登记到审核阶段
                    entity.IsOver = 0; //流程未完成，1表示完成
                    entity.FLOWID = mpcEntity.ID;
                    entity.FLOWNAME = mpcEntity.FLOWNAME + "审核中";
                }
                if (entity.EXAMINECODE == "" || entity.EXAMINECODE == null)
                {
                    entity.EXAMINECODE = safetyassessmentbll.GetMaxCode();
                }
                safetyassessmentbll.SaveForm(keyValue, entity);

                return Success("操作成功!");
            }
            else
            {
                return Error("请填写考核信息!");
            }
             
        }
        #endregion






        #region 安全考核审批考核
        /// <summary>
        /// 安全考核审批考核
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

            string moduleName = "安全考核审核";

            SafetyAssessmentEntity entity = safetyassessmentbll.GetEntity(keyValue);
            ///// <param name="currUser">当前登录人</param>
            ///// <param name="state">是否有权限审核 1：能审核 0 ：不能审核</param>
            ///// <param name="moduleName">模块名称</param>
            ///// <param name="createdeptid">创建人部门ID</param>
            ManyPowerCheckEntity mpcEntity = safetyassessmentbll.CheckAuditPower(curUser, out state, moduleName, entity.CreateUserDeptId,"1");


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
            aidEntity.FlowId = entity.FLOWID;
            aidEntity.AUDITSIGNIMG = HttpUtility.UrlDecode(aidEntity.AUDITSIGNIMG);
            aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
            if (null != mpcEntity)
            {
                aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //备注 存流程的顺序号
            }
            else
            {
                aidEntity.REMARK = "1";
            }
            aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            #endregion

            #region  //保存安全考核
            //审核通过
            if (aentity.AUDITRESULT == "0")
            {
                //0表示流程未完成，1表示流程结束
                if (null != mpcEntity)
                {
                    entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    entity.FLOWROLE = mpcEntity.CHECKROLEID;
                    entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    entity.IsSaved = 1;
                    entity.IsOver = 0;
                    entity.FLOWID = mpcEntity.ID;
                    entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "审核中";
                }
                else
                {
                    entity.FLOWDEPT = "";
                    entity.FLOWDEPTNAME = "";
                    entity.FLOWROLE = "";
                    entity.FLOWROLENAME = "";
                    entity.IsSaved = 1;
                    entity.IsOver = 1;
                    entity.FLOWNAME = "";
                }
            }
            else //审核不通过 
            {
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.IsOver = 0; //处于登记阶段
                entity.IsSaved = 0; //是否完成状态赋值为未完成
                entity.FLOWNAME = "";
                entity.FLOWID = "";

            }
            //更新日常考核基本状态信息
            safetyassessmentbll.SaveForm(keyValue, entity);
            #endregion

            #region    //审核不通过
            if (aentity.AUDITRESULT == "1")
            {
                //添加历史记录
                ///
                HistorysafetyassessmentEntity hsentity = new HistorysafetyassessmentEntity();
                hsentity.CREATEUSERID = entity.CREATEUSERID;
                hsentity.CREATEUSERDEPTCODE = entity.CREATEUSERDEPTCODE;
                hsentity.CREATEUSERORGCODE = entity.CREATEUSERORGCODE;
                hsentity.CREATEDATE = entity.CREATEDATE;
                hsentity.CREATEUSERNAME = entity.CREATEUSERNAME;
                //hsentity.CreateUserDeptId = entity.CreateUserDeptId;
                hsentity.MODIFYDATE = entity.MODIFYDATE;
                hsentity.MODIFYUSERID = entity.MODIFYUSERID;
                hsentity.MODIFYUSERNAME = entity.MODIFYUSERNAME;
                hsentity.FLOWID = entity.FLOWID;
                hsentity.NUMCODE = entity.NUMCODE;
                hsentity.EXAMINECODE = entity.EXAMINECODE;
                hsentity.EXAMINEDEPT = entity.EXAMINEDEPT;
                hsentity.EXAMINEDEPTID = entity.EXAMINEDEPTID;
                hsentity.EXAMINEPERSON = entity.EXAMINEPERSON; //关联ID
                hsentity.EXAMINEPERSONID = entity.EXAMINEPERSONID;
                hsentity.EXAMINETYPE = entity.EXAMINETYPE;
                hsentity.EXAMINETIME = entity.EXAMINETIME; //关联ID
                hsentity.EXAMINEREASON = entity.EXAMINEREASON;
                hsentity.EXAMINEBASIS = entity.EXAMINEBASIS;
                hsentity.FLOWDEPTNAME = entity.FLOWDEPTNAME;
                hsentity.FLOWDEPT = entity.FLOWDEPT;
                hsentity.CONTRACTID = entity.ID;//关联ID
                hsentity.FLOWROLENAME = entity.FLOWROLENAME;
                hsentity.FLOWROLE = entity.FLOWROLE;
                hsentity.FLOWNAME = entity.FLOWNAME;
                hsentity.ISSAVED = 1;
                hsentity.ISOVER = entity.IsOver;
                hsentity.EVALUATETYPE = entity.EvaluateType;
                hsentity.EXAMINETYPENAME = entity.EXAMINETYPENAME;
                hsentity.ID = "";

                historysafetyassessmentbll.SaveForm(hsentity.ID, hsentity);

                //获取当前业务对象的所有审核记录
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //批量更新审核记录关联ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.APTITUDEID = hsentity.ID; //对应新的ID
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
                //批量更新附件记录关联ID
                var flist = fileinfobll.GetImageListByObject(keyValue);
                foreach (FileInfoEntity fmode in flist)
                {
                    fmode.RecId = hsentity.ID; //对应新的ID
                    fileinfobll.SaveForm("", fmode);
                }

                //批量更新考核信息

                var personeva = safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '" + keyValue + "' ");

                foreach (SafetyassessmentpersonEntity samode in personeva)
                {
                    samode.SAFETYASSESSMENTID = hsentity.ID; //对应新的ID
                    safetyassessmentpersonbll.SaveForm("", samode);
                }
            }
            #endregion

            return Success("操作成功!");
        }
        #endregion


        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetApplyListJson(Pagination pagination, string queryJson)
        {
           

            var watch = CommonHelper.TimerStart();
            var queryParam = queryJson.ToJObject();
            var data = aptitudeinvestigateauditbll.GetAuditList(queryParam["ID"].ToString());
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
        /// 查询审核流程图
        /// </summary>
        /// <param name="keyValue">主键</param>
        /// <param name="urltype">查询类型：0：安全考核</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAuditFlowData(string keyValue, string urltype)
        {
            try
            {
                var data = safetyassessmentbll.GetAuditFlowData(keyValue, urltype);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 安全考核表单的导出
        /// </summary>
        /// <param name="keyValue"></param>
        public void ExportDataSafeAssment(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/安全奖励单.doc");
            
            DataTable dt = new DataTable();
            dt.Columns.Add("numcode"); //编号
            dt.Columns.Add("deptsorpersons"); //奖励单位
            dt.Columns.Add("createtime");
            dt.Columns.Add("resaon"); 
            dt.Columns.Add("basis");
            dt.Columns.Add("money");
            dt.Columns.Add("auditreason");
            dt.Columns.Add("auditdept");
            dt.Columns.Add("audittime");
            dt.Columns.Add("deptname");
            dt.Columns.Add("gzpic");

            dt.Columns.Add("assmentimage");

            DataRow row = dt.NewRow();

            SafetyAssessmentEntity entity = safetyassessmentbll.GetEntity(keyValue); // 基本信息
            if (entity.EvaluateType == "0")
            {
                fileName = Server.MapPath("~/Resource/ExcelTemplate/安全处罚单.doc");
            }
            else
            {
                fileName = Server.MapPath("~/Resource/ExcelTemplate/安全奖励单.doc");
            }
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            var personeva = safetyassessmentpersonbll.GetList(" SAFETYASSESSMENTID = '" + entity.ID + "' "); // 考核信息
            var auditlist = aptitudeinvestigateauditbll.GetAuditList(entity.ID); // 审批信息
            string deptsorpersons = "";
            int numfor = 1;
            int isAJB = 0;
            decimal money = 0;
            foreach (SafetyassessmentpersonEntity samode in personeva)
            {
                if (numfor >= personeva.Count())
                {
                    deptsorpersons += samode.EVALUATEDEPTNAME ;
                }
                else
                {
                    deptsorpersons += samode.EVALUATEDEPTNAME + "、";
                }
                if (samode.SCORE != "" && samode.SCORE != null)
                {
                    money += Convert.ToDecimal(samode.SCORE);
                }

                numfor++;
            }
            row["numcode"] = entity.EXAMINECODE;
            row["deptsorpersons"] = deptsorpersons;
            row["createtime"] = entity.EXAMINETIME.Value.ToString("yyyy年MM月dd日");
            row["resaon"] = entity.EXAMINEREASON+","+entity.EXAMINEBASIS;
            row["basis"] = entity.EXAMINEBASIS;;
            row["money"] = CmycurD(money);
            foreach (var audit in auditlist)
            {
                string audittype = audit.AUDITRESULT == "0" ? "通过;" : "不通过;";
                row["auditreason"] = audittype +  audit.AUDITOPINION;
                row["auditdept"] = audit.AUDITDEPT;
                row["audittime"] = audit.AUDITTIME.Value.ToString("yyyy年MM月dd日");
                row["deptname"] = audit.AUDITDEPT;
                string userid = audit.AUDITPEOPLEID;
                UserEntity useren =  userbll.GetEntity(audit.AUDITPEOPLEID);
                // 如果是安监部负责人，就显示图标
                if (audit.AUDITDEPT == "安监部" && user.RoleName.Contains("负责人"))
                {
                    isAJB = 1;
                }
            }
            Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
            db.MoveToMergeField("assmentimage");

            var data = fileinfobll.GetFiles(entity.ID);

            if (data != null)
            {
                foreach (DataRow item in data.Rows)
                {
                    var path = item.Field<string>("FilePath");
                    db.InsertImage(Server.MapPath(path), 190, 140);
                }

            }
            else
            {
                row["assmentimage"] = "";
            }

            
            db.MoveToMergeField("gzpic");
            if (isAJB == 1)
            {
                Aspose.Words.Drawing.Shape shape = db.InsertImage(Server.MapPath("~/content/Images/Page0001.png"), 120, 112);
            }
            else
            {
                row["gzpic"] = "";
            }
            
           
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);

            if (entity.EvaluateType == "0")
            {
                doc.Save(resp, Server.UrlEncode("安全处罚单_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            }
            else
            {
                doc.Save(resp, Server.UrlEncode("安全奖励单_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            }
            
        }

        /// <summary>
        /// 数字转大写
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string CmycurD(decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //如果该位是亿位或元位，则必须写上 
                    ch2 = str2.Substring(i, 1);
                }
                str5 = str5 + ch1 + ch2;

                if (i == j - 1 && str3 == "0")
                {
                    //最后一位（分）为0时，加上“整” 
                    str5 = str5 + '整';
                }
            }
            if (num == 0)
            {
                str5 = "零元整";
            }
            return str5;
        }



        /// <summary>
        /// 安全考核汇总导出
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deptid"></param>
        public void ExportDataTotal(string time,string deptid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/X年度X月份安全、文明生产及环保考核情况统计.doc");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("year"); 
            dt.Columns.Add("month"); 
            DataRow row = dt.NewRow();

            row["year"] = Convert.ToDateTime(time).ToString("yyyy");
            row["month"] = Convert.ToDateTime(time).ToString("MM");


            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);

            DataTable dt1 = new DataTable("A");
            dt1.Columns.Add("sortnum");
            dt1.Columns.Add("time");
            dt1.Columns.Add("deptname");
            dt1.Columns.Add("reasonbasis");
            dt1.Columns.Add("money");
            dt1.Columns.Add("score");
            dt1.Columns.Add("remark");
            string deptcode = departmentbll.GetEntity(deptid).EnCode;
            DataTable assmentData = safetyassessmentbll.ExportDataTotal(time, deptcode);
            if (assmentData.Rows.Count > 0)
            {
                for (int i = 0; i < assmentData.Rows.Count; i++)
                {
                    DataRow row1 = dt1.NewRow();
                    row1["sortnum"] = i + 1;
                    row1["time"] = assmentData.Rows[i]["CREATEDATE"];

                    row1["deptname"] = assmentData.Rows[i]["DEPTRESULTNAME"];
                    string con = "";
                    if (assmentData.Rows[i]["SCORETYPE"].ToString() == "0")
                    {
                        con += "考核" + assmentData.Rows[i]["EVALUATEDEPTNAME"].ToString();
                    }
                    else
                    {
                        con += "奖励" + assmentData.Rows[i]["EVALUATEDEPTNAME"].ToString();
                    }
                    if (assmentData.Rows[i]["SCORE"].ToString() != "")
                    {
                        con += assmentData.Rows[i]["SCORE"].ToString()+"元;";
                    }
                    if (assmentData.Rows[i]["EVALUATESCORE"].ToString() != "")
                    {
                        con += assmentData.Rows[i]["EVALUATESCORE"].ToString() + "分;";
                    }
                    if (assmentData.Rows[i]["EVALUATECONTENT"].ToString() != "")
                    {
                        con += "绩效"+assmentData.Rows[i]["EVALUATECONTENT"].ToString() + ";";
                    }
                    row1["reasonbasis"] = assmentData.Rows[i]["EXAMINEREASON"] + "  " + assmentData.Rows[i]["EXAMINEBASIS"]+";"+ con;
                    row1["money"] = ((assmentData.Rows[i]["SCORETYPE"].ToString()=="0" && assmentData.Rows[i]["SCORE"].ToString() != "") ?"-":"") + assmentData.Rows[i]["SCORE"];
                    row1["score"] = assmentData.Rows[i]["EVALUATESCORE"];
                    row1["remark"] = assmentData.Rows[i]["EXAMINETYPENAME"];
                    dt1.Rows.Add(row1);
                }
            }
            //builder.StartTable
            doc.MailMerge.ExecuteWithRegions(dt1);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode("安全考核汇总表_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        /// <summary>
        /// 导出外部部门考核
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deptid"></param>
        public void ExportDataOutDept(string time, string deptid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/X年X月外委维护单位考核情况明细汇总表.doc");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);

            //  获取外部部门考核信息
            string deptcode = departmentbll.GetEntity(deptid).EnCode;
            DataTable assmentData = safetyassessmentbll.ExportDataOutDept(time, deptcode);

            DataTable dtyear = new DataTable();
            dtyear.Columns.Add("year");
            DataRow rowyear = dtyear.NewRow();
            //rowyear["year"] = DateTime.Now.ToString("yyyy年MM月");
            rowyear["year"] = Convert.ToDateTime(time).ToString("yyyy年MM月");


            dtyear.Rows.Add(rowyear);
            doc.MailMerge.Execute(dtyear);

            builder.MoveToBookmark("table");
            StringBuilder strexcel = new StringBuilder();
            strexcel.Append(@"<table   border=0 cellspacing=0 cellpadding=0 style='border-collapse:collapse;TABLE-LAYOUT:fixed;word-break: break-all; width:100%'>");


            strexcel.Append(@"  <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;height:18.25pt'>
                                    <td width=691 colspan=8 style='width:518.3pt;border:none;border-bottom:solid windowtext 1.0pt;
                                    mso-border-bottom-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                    height:18.25pt'>
                                    <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                    vertical-align:middle'><b><span lang=EN-US style='font-size:10.5pt;
                                    font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                    AR'><o:p>&nbsp;</o:p></span></b></p>
                                    </td>
                                    <td width=236 colspan=4 style='width:177.0pt;border:none;border-bottom:solid windowtext 1.0pt;
                                    mso-border-bottom-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                    height:18.25pt'>
                                    <p class=MsoNormal align=left style='text-align:left;text-indent:10.5pt;
                                    mso-char-indent-count:1.0;mso-pagination:widow-orphan;layout-grid-mode:char;
                                    vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
                                    mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>报表日期：");
            strexcel.Append(DateTime.Now.ToString("yyyy年MM月dd日"));
            strexcel.Append(@" <span lang=EN-US><o:p></o:p></span></b></span></p>
                                    </td>
                                    </tr>");
            #region 表头
            strexcel.Append(@"  <tr style='mso-yfti-irow:1;height:27.5pt'>
                                      <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                      padding:.75pt .75pt .75pt .75pt;height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:宋体;mso-bidi-font-family:宋体'>责任单位<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:宋体;mso-bidi-font-family:宋体'>序号<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                      height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>时间<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                      mso-border-top-alt:windowtext;mso-border-left-alt:windowtext;mso-border-bottom-alt:
                                      black;mso-border-right-alt:black;mso-border-style-alt:solid;mso-border-width-alt:
                                      .5pt;padding:.75pt .75pt .75pt .75pt;height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>考核内容<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=124 style='width:93.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>考核依据<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
                                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                      height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>考核金额<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>（元）<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
                                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                      height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>提出考核部门或专业<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
                                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                      height:27.5pt'>
                                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>备注<span lang=EN-US><o:p></o:p></span></span></b></p>
                                      </td>
                                     </tr>");
            #endregion

            #region 中间内容
            decimal litterNum = 0; //小计
            decimal insertNum = 0; //增加合计
            decimal lessNum = 0; //扣钱合计
            if (assmentData.Rows.Count > 0)
            {
                int rowstar = 0; //节点开始
                string deotcode = string.Empty; //部门code
                int num = 0; // 显示第一列相同的抬头多少行
                int xNum = 0; //序号
                
                foreach (DataRow dr in assmentData.Rows)
                {
                    if (deotcode != null && deotcode != dr["BMNAMECODE"].ToString() && deotcode != "")
                    {
                        rowstar = 0;
                        strexcel.Append(@"<tr style='mso-yfti-irow:8;height:23.05pt'>
                                              <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                              none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                              style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                              solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                              solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                              solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                              border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                              mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                              height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                              normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
                                              宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                              </td>
                                              <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                              none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                              mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                              style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                              0pt;mso-bidi-language:AR'>月度工作效果评价考核项<span lang=EN-US><o:p></o:p></span></span></b></p>
                                              </td>
                                              <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                              lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                              mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                             </tr>");
                        // 添加小计
                        strexcel.Append(@"<tr style='mso-yfti-irow:9;height:23.05pt'>
                              <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                              none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                              font-family:宋体;mso-bidi-font-family:宋体;color:black'>小计：</span></b><span
                              lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                              color:black'><o:p></o:p></span></p>
                              </td>
                              <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                              solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                              solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                              solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                              border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                              mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                              mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                              height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                              normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
                              宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                              </td>
                              <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                              none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                              mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                              mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                              vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                              lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                              mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                              </td>
                              <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'>");
                        strexcel.Append(litterNum);
                        strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                              lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                              mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>  </tr>");
                        litterNum = 0;
                    }
                    strexcel.Append("<tr style='mso-yfti-irow:2;height:23.05pt'>");
                    
                    if (rowstar == 0)
                    {
                        rowstar = 1;
                        xNum = 1; // 新的部门显示 1
                        num = 0;
                        foreach (DataRow drnum in assmentData.Rows)
                        {
                            if (dr["BMNAMECODE"].ToString() == drnum["BMNAMECODE"].ToString())
                            {
                                num++;
                                if (drnum["SCORE"] != null && drnum["SCORE"].ToString() != "")
                                {
                                    if (drnum["SCORETYPE"].ToString() == "0")
                                    {
                                        litterNum = litterNum - Convert.ToDecimal(drnum["SCORE"].ToString());
                                        lessNum = lessNum - Convert.ToDecimal(drnum["SCORE"].ToString());
                                    }
                                    else
                                    {
                                        litterNum = litterNum + Convert.ToDecimal(drnum["SCORE"].ToString());
                                        insertNum = insertNum + Convert.ToDecimal(drnum["SCORE"].ToString());
                                    }
                                }

                            }

                        }

                        //第一列
                        strexcel.Append("<td width=74 rowspan=" + num + " style='width:55.55pt;border:solid windowtext 1.0pt;border-top:none; mso - border - top - alt:solid windowtext .5pt; mso - border - alt:solid windowtext .5pt;padding: .75pt .75pt .75pt .75pt; height: 23.05pt'>" +
                                           "<p class=MsoNormal align = center style='text-align:center;mso-pagination:widow-orphan;layout -grid-mode:char;vertical-align:middle'>" +
                                           "<span style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;'>" + dr["BMNAME"].ToString() +
                                            "<span lang = EN - US ></span></span></p>" +
                                          "</td>");

                    }

                    #region 中间通用内容加载
                    // 添加序号
                    strexcel.Append("<td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:black'>" + xNum + "</span></p></td>");
                    //添加时间
                    strexcel.Append("<td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none; border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt; mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体; mso-font-kerning:0pt;mso-bidi-language:AR'>" + Convert.ToDateTime(dr["CREATEDATE"].ToString()).ToString("MM月dd日") + "</span><span lang=EN-US></span></span></p></td>");
                    //考核内容
                    //strexcel.Append("<td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt; word-break: break-all;  word-wrap: break-word; '><p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEREASON"].ToString()  + "<o:p>&nbsp;</o:p></span></p></td>");
                    string khnrhtml = "";
                    for (int i = 0; i < dr["EXAMINEREASON"].ToString().Length; i = i + 26)
                    {
                        if (dr["EXAMINEREASON"].ToString().Length > i + 26)
                        {
                            khnrhtml += "<span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEREASON"].ToString().Substring(i, 26) + "</span>";
                        }
                        else
                        {
                            khnrhtml += "<span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEREASON"].ToString().Substring(i, dr["EXAMINEREASON"].ToString().Length - i) + "</span>";
                        }


                    }
                    strexcel.Append("<td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt; mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt; word-break: break-all;  word-wrap: break-word; '><p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;vertical-align:middle'>" +
                       khnrhtml +
                        "</p></td>");

                    //考核依据
                    //strexcel.Append("<td width=124 style='width:93.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt: solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEBASIS"].ToString() + "<o:p>&nbsp;</o:p></span></p></td>");
                    string htmlsqpn = "";
                    for (int i = 0; i < dr["EXAMINEBASIS"].ToString().Length; i = i + 8)
                    {
                        if (dr["EXAMINEBASIS"].ToString().Length > i + 8)
                        {
                            htmlsqpn += "<span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEBASIS"].ToString().Substring(i, 8) + "</span>";
                        }
                        else
                        {
                            htmlsqpn += "<span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEBASIS"].ToString().Substring(i, dr["EXAMINEBASIS"].ToString().Length - i) + "</span>";
                        }


                    }
                    strexcel.Append("<td width=124 style='width:93.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt: solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt;word-break: break-all;word-wrap: break-word;'>" +
                        "  <p>" + htmlsqpn + "</p>" +
                        "</td>");
                    // 金额
                    strexcel.Append(" <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none; border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan; vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + ((dr["SCORETYPE"].ToString() == "0" && dr["SCORE"].ToString() !="") ? "-" : "") + dr["SCORE"].ToString() + "<o:p>&nbsp;</o:p></span></p>" +
                                      "</td>");
                    // 提出考核部门
                    strexcel.Append(" <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom: solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt; mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>" +
                                          "<p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EXAMINEDEPT"].ToString() + "<o:p>&nbsp;</o:p></span></p>" +
                                          "</td>");
                    // 备注
                    //strexcel.Append(@"<td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>" +
                    //                  "<p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EVALUATEOTHER"].ToString()  + "</span></p>" +
                    //                  "</td>");
                    string bzhtml = "";
                    for (int i = 0; i < dr["EVALUATEOTHER"].ToString().Length; i = i + 6)
                    {
                        if (dr["EVALUATEOTHER"].ToString().Length > i + 6)
                        {
                            bzhtml += "<span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EVALUATEOTHER"].ToString().Substring(i, 6) + "</span>";
                        }
                        else
                        {
                            bzhtml += "<span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>" + dr["EVALUATEOTHER"].ToString().Substring(i, dr["EVALUATEOTHER"].ToString().Length - i) + "</span>";
                        }


                    }
                    strexcel.Append(@"<td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>" +
                                      "<p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'>" + bzhtml+
                                      "</p>" +
                                      "</td>");
                    #endregion
                    strexcel.Append("</tr>");
                    //if (deotcode != null && deotcode != dr["BMNAMECODE"].ToString() && deotcode != "")
                    //{
                    //    rowstar = 0;
                        
                       
                        

                    //}
                    xNum++; 
                    deotcode = dr["BMNAMECODE"].ToString();

                }
                strexcel.Append(@"<tr style='mso-yfti-irow:8;height:23.05pt'>
                                              <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                              none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                              style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                              solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                              solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                              solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                              border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                              mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                              height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                              normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
                                              宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                              </td>
                                              <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                              none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                              mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                              style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                              0pt;mso-bidi-language:AR'>月度工作效果评价考核项<span lang=EN-US><o:p></o:p></span></span></b></p>
                                              </td>
                                              <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                              <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                              <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                              lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                              mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                              </td>
                                             </tr>");
                strexcel.Append(@"<tr style='mso-yfti-irow:9;height:23.05pt'>
                              <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                              none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                              font-family:宋体;mso-bidi-font-family:宋体;color:black'>小计：</span></b><span
                              lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                              color:black'><o:p></o:p></span></p>
                              </td>
                              <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                              solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                              solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                              solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                              border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                              mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                              mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                              height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                              normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
                              宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                              </td>
                              <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                              none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                              mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                              mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                              vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                              lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                              mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                              </td>
                              <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                              border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                              solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'>");
                strexcel.Append(litterNum);
                strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                              vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                              宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                              mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>
                              <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                              solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                              mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                              padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                              <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                              lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                              mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                              </td>  </tr>");
                litterNum = 0;

            }
            #region  
            //strexcel.Append(@"<tr style='mso-yfti-irow:2;height:23.05pt'>
            //      <td width=74 rowspan=6 style='width:55.55pt;border:solid windowtext 1.0pt;
            //      border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
            //      font-family:宋体;mso-bidi-font-family:宋体;color:black'>【</span><span
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue'>被考核<span
            //      lang=EN-US><o:p></o:p></span></span></p>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
            //      font-family:宋体;mso-bidi-font-family:宋体;color:blue'>单位</span><span
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:black'>】<span
            //      lang=EN-US><o:p></o:p></span></span></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>1<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'>X</span><span style='font-size:
            //      10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue;mso-font-kerning:
            //      0pt;mso-bidi-language:AR'>月<span lang=EN-US>X</span>日<span lang=EN-US><o:p></o:p></span></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:3;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>2<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:4;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>3<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:5;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>4<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:6;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>5<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:7;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>6<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:8;height:23.05pt'>
            //      <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
            //      宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
            //      0pt;mso-bidi-language:AR'>月度工作效果评价考核项<span lang=EN-US><o:p></o:p></span></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:9;height:23.05pt'>
            //      <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //      font-family:宋体;mso-bidi-font-family:宋体;color:black'>小计：</span></b><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      color:black'><o:p></o:p></span></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
            //      宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
            //      mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:10;height:23.05pt'>
            //      <td width=74 rowspan=3 style='width:55.55pt;border:solid windowtext 1.0pt;
            //      border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
            //      font-family:宋体;mso-bidi-font-family:宋体;color:black'>【</span><span
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue'>被考核<span
            //      lang=EN-US><o:p></o:p></span></span></p>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
            //      font-family:宋体;mso-bidi-font-family:宋体;color:blue'>单位</span><span
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:black'>】<b><span
            //      lang=EN-US><o:p></o:p></span></b></span></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>1<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
            //      宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:11;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>2<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
            //      宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:12;height:23.05pt'>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>3<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
            //      宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>
            //     <tr style='mso-yfti-irow:13;height:23.05pt'>
            //      <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b><span lang=EN-US
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:black'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //      宋体;mso-bidi-font-family:宋体;color:black'>4<o:p></o:p></span></p>
            //      </td>
            //      <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //      border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //      mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //      height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //      normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
            //      宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
            //      0pt;mso-bidi-language:AR'>月度工作效果评价考核项<span lang=EN-US><o:p></o:p></span></span></b></p>
            //      </td>
            //      <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //      </td>
            //      <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //      </td>
            //     </tr>");
            #endregion

            #endregion


            #region 合计
            // 奖励合计
            strexcel.Append(@"<tr style='mso-yfti-irow:15;height:23.05pt'>
                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt; padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:black'>奖励合计：<span lang=EN-US><o:p></o:p></span></span></b></p></td>
                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p></td>
                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt; height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p></td>
                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-f
amily:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(insertNum);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                 </tr>");

            // 扣钱合计
            strexcel.Append(@"<tr style='mso-yfti-irow:16;height:25.3pt'>
                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:宋体;mso-bidi-font-family:宋体;color:black'>考核合计：<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>
                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                  normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
                                  宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(lessNum);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                 </tr>");

            // 合计
            strexcel.Append(@"<tr style='mso-yfti-irow:17;height:25.3pt'>
                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:宋体;mso-bidi-font-family:宋体;color:black'>合计：<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>
                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                  normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
                                  宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(insertNum + lessNum);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
                                  </td>
                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                 </tr>");

            #region html 合计
            //            strexcel.Append(@" <tr style='mso-yfti-irow:15;height:23.05pt'>
            //                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt; padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:black'>奖励合计：<span lang=EN-US><o:p></o:p></span></span></b></p></td>
            //                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p></td>
            //                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt; height:23.05pt'><p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p></td>
            //                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-f
            //amily:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                 </tr>
            //                                 <tr style='mso-yfti-irow:16;height:25.3pt'>
            //                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                                  font-family:宋体;mso-bidi-font-family:宋体;color:black'>考核合计：<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                                  </td>
            //                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                                  宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                                  height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //                                  normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
            //                                  宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                 </tr>
            //                                 <tr style='mso-yfti-irow:17;height:25.3pt'>
            //                                  <td width=74 style='width:55.55pt;border:solid windowtext 1.0pt;border-top:
            //                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                                  font-family:宋体;mso-bidi-font-family:宋体;color:black'>合计：<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                                  </td>
            //                                  <td width=40 style='width:30.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                                  宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                  <td width=74 colspan=2 style='width:55.5pt;border-top:none;border-left:none;
            //                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
            //                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                                  height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //                                  normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
            //                                  宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=362 colspan=2 style='width:271.5pt;border-top:none;border-left:
            //                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  mso-border-alt:solid black .5pt;mso-border-left-alt:solid windowtext .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=124 style='width:93.0pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=75 colspan=3 style='width:56.25pt;border-top:none;border-left:none;
            //                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=90 style='width:67.5pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                                  </td>
            //                                  <td width=88 style='width:66.0pt;border-top:none;border-left:none;border-bottom:
            //                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
            //                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                                  </td>
            //                                 </tr>");
            #endregion

            #endregion

            #region  倒数第二排意见
            strexcel.Append(@"<tr style='mso-yfti-irow:18;height:80.2pt'>
                                  <td width=171 colspan=3 valign=top style='width:128.3pt;border-top:none;
                                  border-left:solid windowtext 1.0pt;border-bottom:solid windowtext 1.0pt;
                                  border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体'>发电部意见：<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;mso-layout-grid-align:none;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>发电部部（签章）<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span style='font-size:10.5pt;
                                  font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>年<span lang=EN-US><span style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>月<span
                                  lang=EN-US><span style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>日</span><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体'><o:p></o:p></span></p>
                                  </td>
                                  <td width=198 colspan=2 valign=top style='width:148.5pt;border-top:none;
                                  border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>燃料储运部意见：<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;mso-layout-grid-align:none;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>燃料储运部（签章）<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp; </span></span><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>年<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>月<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>日<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=181 valign=top style='width:135.75pt;border-top:none;border-left:
                                  none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>设备管理部意见：<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;mso-layout-grid-align:none;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>设备管理部（签章）<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp;</span></span><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>年<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>月<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>日<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=171 colspan=3 valign=top style='width:128.25pt;border-top:none;
                                  border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:
                                  bold'>安全环保部意见：<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR;mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                                  mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR;
                                  mso-bidi-font-weight:bold'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;text-indent:31.5pt;
                                  mso-char-indent-count:3.0;mso-pagination:widow-orphan;layout-grid-mode:char;
                                  mso-layout-grid-align:none;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp;</span></span><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>安全环保部（签章）<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp;</span></span><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>年<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp; </span></span>月<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>日<span lang=EN-US
                                  style='mso-bidi-font-weight:bold'><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=206 colspan=3 valign=top style='width:154.5pt;border-top:none;
                                  border-left:none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:80.2pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>综合保障部意见：<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;text-indent:31.5pt;
                                  mso-char-indent-count:3.0;mso-pagination:widow-orphan;layout-grid-mode:char;
                                  mso-layout-grid-align:none;vertical-align:middle'><span style='font-size:
                                  10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;
                                  mso-bidi-language:AR'>综合保障部（签章）<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><span style='mso-spacerun:yes'>&nbsp; </span></span><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>年<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>月<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>日<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                 </tr>");
            #endregion

            // 最后一排意见
            strexcel.Append(@" <tr style='mso-yfti-irow:19;mso-yfti-lastrow:yes;height:25.3pt'>
                                  <td width=927 colspan=12 valign=top style='width:695.3pt;border-top:none;
                                  border-left:solid windowtext 1.0pt;border-bottom:solid windowtext 1.0pt;
                                  border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>生产副总经理（总工程师）批示：<span lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal style='text-indent:21.0pt;mso-char-indent-count:2.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal style='mso-pagination:widow-orphan;layout-grid-mode:char;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  <p class=MsoNormal align=right style='text-align:right;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                  </span><span
                                  style='mso-spacerun:yes'></span></span><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>年<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>月<span lang=EN-US><span
                                  style='mso-spacerun:yes'>&nbsp;&nbsp; </span></span>日<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                 </tr>");


            strexcel.Append(@"</table>");
            builder.InsertHtml(strexcel.ToString());

            doc.Save(resp, Server.UrlEncode("外委单位考核表_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        /// <summary>
        /// 导出内部部门考核
        /// </summary>
        /// <param name="time"></param>
        /// <param name="deptid"></param>
        public void ExportDataInDept(string time,string deptid)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //报告对象
            string fileName = Server.MapPath("~/Resource/ExcelTemplate/X年X月安全管理工作奖励、考核情况汇总报表.doc");
            Aspose.Words.Document doc = new Aspose.Words.Document(fileName);
            DocumentBuilder builder = new DocumentBuilder(doc);


            #region  标题的修改
            DataTable dtyear = new DataTable();
            dtyear.Columns.Add("ogname");
            dtyear.Columns.Add("year");
            DataRow rowyear = dtyear.NewRow();
            rowyear["year"] = Convert.ToDateTime(time).ToString("yyyy年MM月");
            rowyear["ogname"] = user.OrganizeName;
            dtyear.Rows.Add(rowyear);
            doc.MailMerge.Execute(dtyear);
            #endregion

            builder.MoveToBookmark("table");
            StringBuilder strexcel = new StringBuilder();
            strexcel.Append(@"<table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 style='border-collapse:collapse'>");



            //  获取内部部门考核信息
            string deptcode = departmentbll.GetEntity(deptid).EnCode;
            DataTable assmentData = safetyassessmentbll.ExportDataInDept(time, deptcode);
            // 获取内部最上级部门 = 
            DataTable InDeptData = safetyassessmentbll.GetInDeptData();

            int deptnum = 0; // 部门数量

            deptnum = InDeptData.Rows.Count;


            #region 填报信息
            strexcel.Append(@" <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;height:23.05pt'>
                                  <td width=471 colspan=");
            strexcel.Append((deptnum+4)/2);
            strexcel.Append(@"  style='width:353.3pt;border:none;border-bottom:solid windowtext 1.0pt;
                                  mso-border-bottom-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:23.05pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>填报部门（盖章）：");
            strexcel.Append(departmentbll.GetEntity(deptid).FullName);
            strexcel.Append(@" <span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=476 colspan=");
            strexcel.Append((deptnum + 4)-((deptnum + 4) / 2));
            strexcel.Append(@"  style='width:356.9pt;border:none;border-bottom:solid windowtext 1.0pt;
                                  mso-border-bottom-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:23.05pt'>
                                  <p class=MsoNormal style='text-indent:136.5pt;mso-char-indent-count:13.0;
                                  mso-pagination:widow-orphan;layout-grid-mode:char;vertical-align:middle'><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>填报（考核）日期：");
            strexcel.Append(DateTime.Now.ToString("yyyy年MM月dd日"));
            strexcel.Append(@" <span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                 </tr>");
            #endregion


            #region 第一行固定
            strexcel.Append(@"<tr style='mso-yfti-irow:1;height:25.3pt'>
                                  <td width=34 rowspan=2 style='width:25.55pt;border:solid windowtext 1.0pt;
                                  border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体;color:black'>序号<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=98 rowspan=2 style='width:73.5pt;border-top:none;border-left:none;
                                  border-bottom:solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:25.3pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>奖励、考核依据</span></b><span style='font-size:9.0pt;font-family:宋体;mso-bidi-font-family:
                                  宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>（注明依据《部门管理工作考核实施细则》中第几条，即考核标准）</span><span
                                  lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                  mso-font-kerning:0pt;mso-bidi-language:AR'><o:p></o:p></span></p>
                                  </td>
                                  <td width=536 colspan=");
            strexcel.Append(deptnum);
            strexcel.Append(@" style='width:402.0pt;border-top:none;border-left:
                                  none;border-bottom:solid windowtext 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;
                                  mso-border-alt:solid windowtext .5pt;mso-border-right-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>月度考核情况（扣减在数值前标“<span
                                  lang=EN-US>-</span>”，奖励直接填数值）<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>
                                  <td width=64 rowspan=2 style='width:48.0pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
                                  solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                  height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>考核合计<span
                                  lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>
                                  <td width=215 rowspan=2 style='width:161.15pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid windowtext .5pt;mso-border-left-alt:solid black .5pt;
                                  mso-border-alt:solid black .5pt;mso-border-top-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:25.3pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>考核情况说明<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span style='font-size:9.0pt;
                                  font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>（要求：一事项作一说明，并列出被考核人员或部门名单及考核金额）</span><span lang=EN-US style='font-size:
                                  10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;
                                  mso-bidi-language:AR'><o:p></o:p></span></p>
                                  </td>
                                 </tr>");
            #endregion

            #region 第二行部门动态
            strexcel.Append(@"<tr style='mso-yfti-irow:2;height:27.5pt'>");
            foreach (DataRow dr in InDeptData.Rows)
            {
                strexcel.Append("<td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom: "+
                                 " solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt; "+
                                 " mso-border-left-alt:solid windowtext .5pt;mso-border-top-alt:windowtext; "+
                                 " mso-border-left-alt:windowtext;mso-border-bottom-alt:black;mso-border-right-alt: "+
                                 " black;mso-border-style-alt:solid;mso-border-width-alt:.5pt;padding:.75pt .75pt .75pt .75pt; "+
                                 " height:27.5pt'> "+
                                 " <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan; "+
                                 " layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt; "+
                                 " font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language: "+
                                 " AR'>"+ dr["FULLNAME"] + "<span lang=EN-US><o:p></o:p></span></span></b></p> "+
                                 " </td>");
            }
            strexcel.Append(@"</tr>");
            //strexcel.Append(@" <tr style='mso-yfti-irow:2;height:27.5pt'>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-top-alt:windowtext;
            //                      mso-border-left-alt:windowtext;mso-border-bottom-alt:black;mso-border-right-alt:
            //                      black;mso-border-style-alt:solid;mso-border-width-alt:.5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>综合<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>管理部<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>计划<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>经营部<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>财务<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>资产部<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>燃料<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>储运部<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>安全<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>环保部<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>设备<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>管理部<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid windowtext .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>发电部<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>政治<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>工作部<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>纪委<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>办公室<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:
            //                      solid black .5pt;mso-border-top-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:27.5pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>综合<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>保障部<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                     </tr>");
            #endregion

            #region 动态绑定数据
            int coneNum = 1;
            List<string> list = new List<string>();
            Dictionary<string, decimal> jl = new Dictionary<string, decimal>();  // 奖励部门统计集合
            Dictionary<string, decimal> cf = new Dictionary<string, decimal>();  // 处罚部门统计集合
            // 将ID去重插入list集合
            foreach (DataRow dr in assmentData.Rows)
            {
                if (!list.Contains(dr["ID"].ToString()))
                {
                    list.Add(dr["ID"].ToString());
                }
              
            }
            // 遍历集合
            foreach (string strlist in list)
            {
                strexcel.Append("<tr style='mso-yfti-irow:3;height:23.05pt'>");
                // 序号
                strexcel.Append(@" <td width=34 style='width:25.55pt;border:solid windowtext 1.0pt;border-top:
                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;color:black'>");
                strexcel.Append(coneNum);
                strexcel.Append(@" <o:p></o:p></span></p></td>");

                //奖励考核依据
                                     
                strexcel.Append(@"<td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                   <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
                                      normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
                                      宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>");
                foreach(DataRow drkh in assmentData.Rows)
                {
                    if (strlist == drkh["ID"].ToString())
                    {
                        if (drkh["EXAMINEBASISID"].ToString() == "")
                        {
                            break;
                        }
                        string[] standearr = drkh["EXAMINEBASISID"].ToString().Split(',');
                        foreach (string stan in standearr)
                        {
                            SafestandarditemEntity ite =  safestandarditembll.GetEntity(stan);
                            SafestandardEntity staninfo = safestandardbll.GetEntity(ite.STID);
                            strexcel.Append(staninfo.NAME+" ");
                        }

                        
                        //strexcel.Append(drkh["EXAMINEBASISID"].ToString());
                        break;
                    }
                }
                strexcel.Append(@"<o:p>&nbsp;</o:p></span></b></p></td>");


                decimal testTotal = 0; // 每行考核合计
                string khContent = string.Empty;
                string dxContent = string.Empty;

                #region 部门内容遍历
                // 遍历中间的内容
                foreach (DataRow drdept in InDeptData.Rows)
                {
                    //khContent = string.Empty;
                    //dxContent = string.Empty;
               
                    decimal deptscore = 0;
                    string inDeptid = drdept["ENCODE"].ToString();
                    
                    strexcel.Append(@" <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
                                                solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                                height:23.05pt'>
                                               <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                           layout-grid-mode:char;vertical-align:middle'><span
                                          lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                          mso-font-kerning:0pt;mso-bidi-language:AR'>");
                    foreach (DataRow datadr in assmentData.Rows)
                    {
                        
                        if (inDeptid == datadr["BMNAMECODE"].ToString() && datadr["ID"].ToString() == strlist)
                        {
                            if (datadr["SCORE"].ToString() != "")
                            {
                                if (datadr["SCORETYPE"].ToString() == "0")
                                {

                                    testTotal = testTotal - Convert.ToDecimal(datadr["SCORE"].ToString());
                                    deptscore = deptscore - Convert.ToDecimal(datadr["SCORE"].ToString());

                                    if (cf.ContainsKey(inDeptid))
                                    {
                                        cf[inDeptid] = cf[inDeptid] - Convert.ToDecimal(datadr["SCORE"].ToString());
                                    }
                                    else
                                    {
                                        cf[inDeptid] =  - Convert.ToDecimal(datadr["SCORE"].ToString());
                                    }

                                }
                                else
                                {
                                    testTotal = testTotal + Convert.ToDecimal(datadr["SCORE"].ToString());
                                    deptscore = deptscore + Convert.ToDecimal(datadr["SCORE"].ToString());

                                    if (jl.ContainsKey(inDeptid))
                                    {
                                        jl[inDeptid] = jl[inDeptid] + Convert.ToDecimal(datadr["SCORE"].ToString());
                                    }
                                    else
                                    {
                                        jl[inDeptid] =  Convert.ToDecimal(datadr["SCORE"].ToString());
                                    }
                                }
                            }

                            if (khContent == null || khContent == "")
                            {
                                khContent = datadr["EXAMINEREASON"].ToString() + " " + datadr["EXAMINEBASIS"].ToString();
                            }
                            dxContent = dxContent + (datadr["SCORETYPE"].ToString() == "0" ? "处罚" : "奖励") + datadr["EVALUATEDEPTNAME"].ToString();
                            if (datadr["SCORE"].ToString() != "")
                            {
                                dxContent = dxContent + "金额" + datadr["SCORE"].ToString()+"元;";
                            }
                            if (datadr["EVALUATESCORE"].ToString() != "")
                            {
                                dxContent = dxContent + "积分" + datadr["EVALUATESCORE"].ToString() + "分;";

                            }
                            if (datadr["EVALUATECONTENT"].ToString() != "")
                            {
                                dxContent = dxContent + "绩效" + datadr["EVALUATECONTENT"].ToString() + ";";

                            }


                        }
                       
                    }
                    strexcel.Append(deptscore);
                    strexcel.Append(@" <o:p>&nbsp;</o:p></span></p></td>");
                }
                #endregion

                // 考核合计
                strexcel.Append(@" <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                      AR'>");
                strexcel.Append(testTotal + " <o:p>&nbsp;</o:p></span></p></td>");

                // 考核情况
                                      
                strexcel.Append(@"<td width=215 style='width:161.15pt;border-top:none;border-left:none;
                                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
                                      mso-font-kerning:0pt;mso-bidi-language:AR'>");
                //strexcel.Append(khContent + dxContent + "<o:p>&nbsp;</o:p></span></p></td>");
                
                string khnrhtml = "";
                string resulkhqk = khContent + dxContent;
                for (int i = 0; i < resulkhqk.Length; i = i + 11)
                {
                    if (resulkhqk.Length > i + 11)
                    {
                        khnrhtml += "<span lang=EN-US  style = 'font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体; mso - font - kerning:0pt; mso - bidi - language:AR'>" + resulkhqk.Substring(i, 11) + "</span>";
                    }
                    else
                    {
                        khnrhtml += "<span lang=EN-US  style = 'font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体; mso - font - kerning:0pt; mso - bidi - language:AR'>" + resulkhqk.Substring(i, resulkhqk.Length - i) + "</span>";
                    }


                }

                strexcel.Append(khnrhtml+ "</p></td>");


                strexcel.Append("</tr>");
                coneNum++;
            }



            //strexcel.Append(@"<tr style='mso-yfti-irow:3;height:23.05pt'>
            //                      <td width=34 style='width:25.55pt;border:solid windowtext 1.0pt;border-top:
            //                      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;color:black'>1<o:p></o:p></span></p>
            //                      </td>
            //                      <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b style='mso-bidi-font-weight:
            //                      normal'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:
            //                      宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;color:blue;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=215 style='width:161.15pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //                      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;color:blue;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                     </tr>");
            #endregion

            #region 共计合计
            // 共计
            strexcel.Append(@"<tr style='mso-yfti-irow:11;height:23.05pt'>");
            // 第一列第二列固定
            strexcel.Append(@"<td width=34 style='width:25.55pt;border:solid windowtext 1.0pt;border-top:
                                  none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p>
                                  </td>
                                  <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>共计<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>");
            // 第三列开始遍历部门
            decimal allTotal = 0;
            foreach (DataRow dr in InDeptData.Rows)
            {

                strexcel.Append(@" <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
                                                solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                                height:23.05pt'>
                                                <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                                vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>");
                decimal cftotal = 0;
                if (cf.ContainsKey(dr["ENCODE"].ToString()))
                {
                    cftotal = cftotal + cf[dr["ENCODE"].ToString()];
                }
                if (jl.ContainsKey(dr["ENCODE"].ToString()))
                {
                    cftotal = cftotal + jl[dr["ENCODE"].ToString()];
                }
                allTotal += cftotal;
                strexcel.Append(cftotal);
                strexcel.Append(@" <o:p>&nbsp;</o:p></span></p></td>");
            }
            // 固定最后两列
            strexcel.Append(@"<td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(allTotal);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                                  </td>
                                  <td width=215 style='width:161.15pt;border-top:none;border-left:none;
                                  border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
                                  solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
                                  </td>");

            strexcel.Append(@"</tr>");

            // 奖励金额
            strexcel.Append(@" <tr style='mso-yfti-irow:12;height:23.05pt'>");
            // 第一列和第二列
            strexcel.Append(@"<td width=34 rowspan=2 style='width:25.55pt;border:solid windowtext 1.0pt;
                                  border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体'>其中<span lang=EN-US><o:p></o:p></span></span></p>
                                  </td>
                                  <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>奖励金额合计<span lang=EN-US><o:p></o:p></span></span></b></p>
                                  </td>");
            decimal jlAlltotal = 0;
            foreach (DataRow dr in InDeptData.Rows)
            {
                strexcel.Append(@" <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
                                                solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                                height:23.05pt'>
                                                <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                                vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>");
                decimal jltotal = 0;
                if (jl.ContainsKey(dr["ENCODE"].ToString()))
                {
                    jltotal = jltotal + jl[dr["ENCODE"].ToString()];
                }
                jlAlltotal += jltotal;
                strexcel.Append(jltotal);
                strexcel.Append(@" <o:p>&nbsp;</o:p></span></p></td>");
            }
            strexcel.Append(@"<td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(jlAlltotal);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                                  </td>
                                <td width=215 rowspan=2 style='width:161.15pt;border-top:none;border-left:
                                  none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
                                  mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;
                                  mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
                                  font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
                                  AR'>说明<span lang=EN-US>:</span></span></b><span lang=EN-US style='font-size:
                                  10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;
                                  mso-bidi-language:AR'>1</span><span style='font-size:10.5pt;font-family:宋体;
                                  mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>、考核数据以元为单位，小数点后数值舍去；<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
                                  layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>2</span><span style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>、涉及按比例扣减工资时，请与人资部联系提供计算基础数值。<span
                                  lang=EN-US><o:p></o:p></span></span></p>
                                  </td>");

            strexcel.Append(@"</tr>");

            // 考核金额 
            strexcel.Append(@" <tr style='mso-yfti-irow:12;height:23.05pt'>");
            // 第二列
            strexcel.Append(@" <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
                                  solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
                                  solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
                                  solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
                                  style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
                                  0pt;mso-bidi-language:AR'>考核金额合计<span lang=EN-US style='color:blue'><o:p></o:p></span></span></b></p>
                                  </td>");
            decimal cfAlltotal = 0;
            foreach (DataRow dr in InDeptData.Rows)
            {
                strexcel.Append(@" <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
                                                solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
                                                mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
                                                height:23.05pt'>
                                                <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                                vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>");
                decimal cftotal = 0;
                if (cf.ContainsKey(dr["ENCODE"].ToString()))
                {
                    cftotal = cftotal + cf[dr["ENCODE"].ToString()];
                }
                cfAlltotal += cftotal;
                strexcel.Append(cftotal);
                strexcel.Append(@"<o:p>&nbsp;</o:p></span> </p></td>");
            }
            strexcel.Append(@"<td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
                                  solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
                                  mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
                                  padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
                                  <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
                                  vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
                                  宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>");
            strexcel.Append(cfAlltotal);
            strexcel.Append(@"<o:p>&nbsp;</o:p></span></p>
                                  </td>");
            strexcel.Append(@"</tr>");



            //strexcel.Append(@"<tr style='mso-yfti-irow:11;height:23.05pt'>
            //                      <td width=34 style='width:25.55pt;border:solid windowtext 1.0pt;border-top:
            //                      none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'>共计<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span lang=EN-US style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=215 style='width:161.15pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //                      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                     </tr>
            //                     <tr style='mso-yfti-irow:12;height:23.05pt'>
            //                      <td width=34 rowspan=2 style='width:25.55pt;border:solid windowtext 1.0pt;
            //                      border-top:none;mso-border-top-alt:solid windowtext .5pt;mso-border-alt:solid windowtext .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><span style='font-size:10.5pt;font-family:宋体;
            //                      mso-bidi-font-family:宋体'>其中<span lang=EN-US><o:p></o:p></span></span></p>
            //                      </td>
            //                      <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'>奖励金额合计<span lang=EN-US><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=215 rowspan=2 style='width:161.15pt;border-top:none;border-left:
            //                      none;border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;
            //                      mso-border-top-alt:solid black .5pt;mso-border-left-alt:solid black .5pt;
            //                      mso-border-alt:solid black .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><b><span style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'>说明<span lang=EN-US>:</span></span></b><span lang=EN-US style='font-size:
            //                      10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;
            //                      mso-bidi-language:AR'>1</span><span style='font-size:10.5pt;font-family:宋体;
            //                      mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>、考核数据以元为单位，小数点后数值舍去；<span
            //                      lang=EN-US><o:p></o:p></span></span></p>
            //                      <p class=MsoNormal align=left style='text-align:left;mso-pagination:widow-orphan;
            //                      layout-grid-mode:char;vertical-align:middle'><span lang=EN-US
            //                      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'>2</span><span style='font-size:10.5pt;font-family:
            //                      宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:AR'>、涉及按比例扣减工资时，请与人资部联系提供计算基础数值。<span
            //                      lang=EN-US><o:p></o:p></span></span></p>
            //                      </td>
            //                     </tr>
            //                     <tr style='mso-yfti-irow:13;mso-yfti-lastrow:yes;height:23.05pt'>
            //                      <td width=98 style='width:73.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid windowtext 1.0pt;border-right:solid windowtext 1.0pt;mso-border-top-alt:
            //                      solid windowtext .5pt;mso-border-left-alt:solid windowtext .5pt;mso-border-alt:
            //                      solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal align=center style='text-align:center;mso-pagination:widow-orphan;
            //                      vertical-align:middle'><b style='mso-bidi-font-weight:normal'><span
            //                      style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:
            //                      0pt;mso-bidi-language:AR'>考核金额合计<span lang=EN-US style='color:blue'><o:p></o:p></span></span></b></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;mso-border-alt:solid black .5pt;
            //                      mso-border-left-alt:solid windowtext .5pt;padding:.75pt .75pt .75pt .75pt;
            //                      height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=60 style='width:45.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=62 style='width:46.5pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><b
            //                      style='mso-bidi-font-weight:normal'><span lang=EN-US style='font-size:10.5pt;
            //                      font-family:宋体;mso-bidi-font-family:宋体;mso-font-kerning:0pt;mso-bidi-language:
            //                      AR'><o:p>&nbsp;</o:p></span></b></p>
            //                      </td>
            //                      <td width=55 style='width:41.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=59 style='width:44.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=48 style='width:36.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=47 style='width:35.25pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=49 style='width:36.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=53 style='width:39.75pt;border-top:none;border-left:none;
            //                      border-bottom:solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:
            //                      solid black .5pt;mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                      <td width=64 style='width:48.0pt;border-top:none;border-left:none;border-bottom:
            //                      solid black 1.0pt;border-right:solid black 1.0pt;mso-border-top-alt:solid black .5pt;
            //                      mso-border-left-alt:solid black .5pt;mso-border-alt:solid black .5pt;
            //                      padding:.75pt .75pt .75pt .75pt;height:23.05pt'>
            //                      <p class=MsoNormal style='mso-pagination:widow-orphan;vertical-align:middle'><span
            //                      lang=EN-US style='font-size:10.5pt;font-family:宋体;mso-bidi-font-family:宋体;
            //                      color:blue;mso-font-kerning:0pt;mso-bidi-language:AR'><o:p>&nbsp;</o:p></span></p>
            //                      </td>
            //                     </tr>");
            #endregion

            strexcel.Append(@" </table>");
            builder.InsertHtml(strexcel.ToString());

            doc.Save(resp, Server.UrlEncode("内部部门考核表_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".doc"), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        #endregion
    }
}
