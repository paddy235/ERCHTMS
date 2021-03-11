using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using System.Linq;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.PublicInfoManage;
using System;
using ERCHTMS.Busines.BaseManage;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：危大工程项目设置
    /// </summary>
    public class WorkMeetingController : MvcControllerBase
    {
        private WorkMeetingBLL workMeetingbll = new WorkMeetingBLL();
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
        /// 获取工作内容列表
        /// </summary>
        [HttpGet]
        public ActionResult GetMeasuresPageList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = "worktask,dangerpoint,measures,createuserid,createuserorgcode,createuserdeptcode,isover,remark,remark1";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"epg_workmeetingmeasures";
            pagination.conditionJson = " 1=1 ";
            if (!queryParam["recid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  workmeetingid ='{0}'", queryParam["recid"].ToString());
            }
            var data = workMeetingbll.GetPageList(pagination, queryJson);
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
        /// 获取外包单位
        /// </summary>       
        [HttpGet]
        public ActionResult GetEngineerDeptList()
        {
            DepartmentBLL deptbll = new DepartmentBLL();
            Operator curUser = OperatorProvider.Provider.Current();            
            string roleNames = curUser.RoleName;
            //分页获取数据
            Pagination pagination = new Pagination();
            pagination.page = 1;// int.Parse(dy.page ?? "1");
            pagination.rows = 9000; //int.Parse(dy.rows ?? "1");
            pagination.p_kid = "departmentid";
            pagination.p_fields = @" organizeid,parentid,encode,fullname,shortname,nature ";
            pagination.p_tablename = @"base_department";
            pagination.sidx = "encode";//排序字段
            pagination.sord = "desc";//排序方式
            pagination.conditionJson = " nature='承包商' ";
            if (curUser.IsSystem || curUser.RoleName.Contains("厂级部门用户") || curUser.RoleName.Contains("公司级用户") || curUser.RoleName.Contains("公司管理员"))
            {
                pagination.conditionJson += string.Format(" and encode like '{0}%' ", curUser.OrganizeCode);
            }
            else if (curUser.RoleName.Contains("承包商级用户"))
            {
                pagination.conditionJson += string.Format(" and departmentid ='{0}'", curUser.DeptId);
            }
            else
            {
                pagination.conditionJson += string.Format(" and departmentid in(select distinct(e.outprojectid) from epg_outsouringengineer e where e.engineerletdeptid='{0}')", curUser.DeptId);
            }
            var data = deptbll.GetPageList(pagination, "");

            return Content(data.ToJson());
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
            var watch = CommonHelper.TimerStart();
            var queryParam = queryJson.ToJObject();
            Operator opertator = new OperatorProvider().Current();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"autoid,createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,
modifyuserid,meetingname,meetingtype,meetingdate,shoudpernum,realpernum,address,engineername,isover,
(select fullname from base_department where departmentid=(select outprojectid from EPG_OutSouringEngineer where id=BIS_WORKMEETING.engineerid)) outprojectname";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"BIS_WORKMEETING";
            pagination.conditionJson = "iscommit='1' ";
            if (opertator.IsSystem || opertator.RoleName.Contains("厂级部门用户") || opertator.RoleName.Contains("公司级用户") || opertator.RoleName.Contains("公司管理员"))
            {
                pagination.conditionJson += string.Format(" and createuserorgcode = '{0}' ", opertator.OrganizeCode);
            }
            else if (opertator.RoleName.Contains("承包商级用户") || opertator.RoleName.Contains("分包商级用户"))
            {
                pagination.conditionJson += string.Format(@" and engineerid in (select e.ID from EPG_OutSouringEngineer e where (e.outprojectid ='{0}' or e.supervisorid='{0}')) ", opertator.DeptId);
                //pagination.conditionJson += string.Format(" and (t.outprojectid ='{0}' or t.supervisorid='{0}')", opertator.DeptId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(opertator.DeptId);
                while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }

                pagination.conditionJson += string.Format(@" and engineerid in (select e.ID from EPG_OutSouringEngineer e where e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%')) ", deptentity.EnCode);
                
            }
            //外包单位
            if (!queryParam["outprojectid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  engineerid in (select e.ID from EPG_OutSouringEngineer e where e.outprojectid ='{0}')", queryParam["outprojectid"].ToString());
            }
            //工程
            if (!queryParam["engineerid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  engineerid = '{0}' ", queryParam["engineerid"].ToString());
            }
            //年度
            if (!queryParam["year"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(meetingdate,'yyyy')={0}", queryParam["year"].ToString());
            }
            //类型 
            if (!queryParam["meetingtype"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and meetingtype = '{0}'", queryParam["meetingtype"].ToString());
            }
            //会议名称
            if (!queryParam["meetingname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and meetingname like '%{0}%'", queryParam["meetingname"].ToString());
            }
            //首页跳转
            if (!queryParam["pageType"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and to_char(meetingdate,'yyyy-MM-dd')=to_char(sysdate,'yyyy-MM-dd')");
            }
            var data = workMeetingbll.GetPageList(pagination, queryJson); 
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
            var data = workMeetingbll.GetList(queryJson).OrderBy(x=>x.CREATEDATE);
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
            OutsouringengineerBLL engneerBll = new OutsouringengineerBLL();
            var data = workMeetingbll.GetEntity(keyValue);
            if (data == null)
            {
                var loginUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                OutsouringengineerEntity eEntity = null;
                if (loginUser.isEpiboly == true)
                    eEntity = engneerBll.GetEntity(loginUser.ProjectID);  
                data = new WorkMeetingEntity()
                {
                    MEETINGDATE = DateTime.Now                    
                };
                if (eEntity != null)
                {
                    
                    data.ENGINEERID = eEntity.ID;
                    data.ENGINEERNAME = eEntity.ENGINEERNAME;
                    data.ENGINEERLEVEL = eEntity.ENGINEERLEVEL;
                    data.ENGINEERTYPE = eEntity.ENGINEERTYPE;
                    data.ENGINEERAREA = eEntity.ENGINEERAREA;
                    data.ENGINEERLETDEPT = eEntity.ENGINEERLETDEPT;
                    data.ENGINEERCONTENT = eEntity.ENGINEERCONTENT;
                    data.ENGINEERCODE = eEntity.ENGINEERCODE;
                    data.MEETINGNAME = string.Format("{0}{1}开工会", DateTime.Now.ToString("yyyyMMdd"), data.ENGINEERNAME);
                    
                }
            }
            var engneer = engneerBll.GetEntity(data.ENGINEERID);
            if (engneer != null)
            {
                data.OUTPROJECTNAME = new DepartmentBLL().GetEntity(engneer.OUTPROJECTID).FullName;
            }

            return ToJsonResult(data);
        }

        /// <summary>
        /// 根据当前登录人获取未提交的数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult GetNotCommitData(string userid) {
            var data = workMeetingbll.GetNotCommitData(userid);
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
            workMeetingbll.RemoveForm(keyValue);
            DeleteFiles(keyValue);
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
        public ActionResult SaveForm(string keyValue, WorkMeetingEntity entity)
        {
            workMeetingbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存开收工会（2019-5-20修改）
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveWorkMeetingForm(string keyValue, WorkMeetingEntity entity, string measuresJson,string ids)
        {
            try
            {
                List<WorkmeetingmeasuresEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WorkmeetingmeasuresEntity>>(measuresJson);
                workMeetingbll.SaveWorkMeetingForm(keyValue, entity, list, ids);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        #endregion
    }
}
