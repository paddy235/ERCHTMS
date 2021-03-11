using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.NosaManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BSFramework.Util.Log;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.NosaManage;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// 工作总结管理
    /// </summary>
    public class NewNosaMonthManagerController : MvcControllerBase
    {
        private NosaWorkSummaryManagerBLL worksummary = new NosaWorkSummaryManagerBLL();
        /// <summary>
        /// 元素负责人工作总结
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PersonWorkSummaryIndex()
        {
            return View();
        }
        /// <summary>
        /// 元素负责人工作总结
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PersonWorkSummaryForm()
        {
            return View();
        }
        /// <summary>
        /// 区域代表工作总结
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AreaWorkSummaryIndex()
        {
            return View();
        }
        /// <summary>
        /// 上传页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpLoadFrom()
        {
            return View();
        }
        /// <summary>
        /// 获取元素负责人工作总结
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetElementPageJson(Pagination pagination, string queryJson)
        {
            var curruser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = @" elementname,elementsuper,dutydepart,
                                       dutydepartid,dutydepartcode,month,(select count(1) from base_fileinfo o where o.recid=t.id) as filenum,
                                       elementsuperid,iscommit,createuserdeptcode,
                                       createdate,createuserorgcode,createusername";
            pagination.p_tablename = @"hrs_nosaworkmanager t";
            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" (createuserorgcode='{0}' or elementsuperid='{1}' ) ", curruser.OrganizeCode, curruser.UserId);
            if (!currUser.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "dutydepartcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            if (curruser.RoleName.Contains("专业级") || curruser.RoleName.Contains("班组级"))
            {
                var pDept = new DepartmentBLL().GetParentDeptBySpecialArgs(curruser.ParentId, "部门");
                pagination.conditionJson += string.Format(" and dutydepartcode like'{0}%'", pDept.EnCode);
            }
            var data = worksummary.GetElementPageJson(pagination, queryJson);
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
        /// 获取区域代表工作总结
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAreaWorkPageJson(Pagination pagination, string queryJson)
        {
            var curruser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "id";
            pagination.p_fields = @" areaid,areaname,dutydepart,areasuper,
                                       dutydepartid,dutydepartcode,month,(select count(1) from base_fileinfo o where o.recid=t.id) as filenum,
                                       areasuperid,iscommit,createuserdeptcode,
                                       createdate,createuserorgcode,createusername";
            pagination.p_tablename = @"hrs_nosaareaworksummary t";
            Operator currUser = OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" (createuserorgcode='{0}' or areasuperid='{1}' ) ", curruser.OrganizeCode, curruser.UserId);
            if (!currUser.IsSystem)
            {
                //根据当前用户对模块的权限获取记录
                string where = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "dutydepartcode", "createuserorgcode");
                if (!string.IsNullOrEmpty(where))
                {
                    pagination.conditionJson += " and " + where;
                }
            }
            if (curruser.RoleName.Contains("专业级") || curruser.RoleName.Contains("班组级"))
            {
                var pDept = new DepartmentBLL().GetParentDeptBySpecialArgs(curruser.ParentId, "部门");
                pagination.conditionJson += string.Format(" and dutydepartcode like'{0}%'", pDept.EnCode);
            }
            var data = worksummary.GetAreaWorkPageJson(pagination, queryJson);
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
        #region
        /// <summary>
        /// 一键提醒
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult KeyRemind(string type)
        {
            try
            {
                string personSql = string.Empty;
                switch (type)
                {
                    case "1":
                        personSql = string.Format(@"select u.account,u.realname,m.id
                                                                          from base_user u
                                                                         inner join (select distinct m.elementsuperid,m.id from hrs_nosaworkmanager m where to_char(m.month,'yyyy-MM')='{0}' and iscommit='0') m
                                                                            on m.elementsuperid = u.userid ", DateTime.Now.ToString("yyyy-MM"));

                        break;
                    case "2":
                        personSql = string.Format(@"select u.account,u.realname,m.id
                                                                          from base_user u
                                                                         inner join (select distinct m.areasuperid,m.id from hrs_nosaareaworksummary m where to_char(m.month,'yyyy-MM')='{0}' and iscommit='0') m
                                                                            on m.areasuperid = u.userid ", DateTime.Now.ToString("yyyy-MM"));
                        break;
                    default:
                        break;
                }
                DataTable dt = worksummary.GetTable(personSql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JPushApi.PushMessage(dt.Rows[i]["account"].ToString(), dt.Rows[i]["realname"].ToString(), "NosaW001", dt.Rows[i]["id"].ToString());
                }
                return Success("提醒成功");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        /// <summary>
        /// 同步元素负责人工作总结
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SyncPersonWorkSummary()
        {
            try
            {
                string strSql = string.Format(@"delete  from hrs_nosaworkmanager w where to_char(w.month,'yyyy-MM')='{0}' and iscommit='0' ", DateTime.Now.ToString("yyyy-MM"));
                worksummary.SyncPersonWorkSummary(strSql);

                string sql = string.Format(@"insert into hrs_nosaworkmanager
                                                  (ID,createuserid,createdate,createuserdeptcode,createuserorgcode,
                                                   createusername,elementname,elementid,elementsuperid,
                                                   elementsuper,dutydepart,dutydepartid,dutydepartcode,
                                                   month,iscommit)
                                                  select NewGuid(),n.createuserid,n.createdate,n.createuserdeptcode,
                                                         n.createuserorgcode,n.createusername,n.name,n.id,
                                                         n.dutyuserid,n.dutyusername,n.dutydepartname,n.dutydepartid,
                                                         d.encode,sysdate, 0
                                                    from hrs_nosaele n
                                                    left join base_department d on d.departmentid = n.dutydepartid  
                                                where n.state!=1 and id not in(select t.elementid from hrs_nosaworkmanager t where to_char(t.month,'yyyy-MM')='{0}' and t.iscommit='1')", DateTime.Now.ToString("yyyy-MM"));
                if (worksummary.SyncPersonWorkSummary(sql))
                {
                    return Success("更新成功");
                }
                else
                {
                    return Error("执行更新失败");
                }

            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 同步区域代表工作总结
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SyncAreaWorkSummary()
        {
            try
            {
                string strSql = string.Format(@"delete  from hrs_nosaareaworksummary w where to_char(w.month,'yyyy-MM')='{0}' and iscommit='0' ", DateTime.Now.ToString("yyyy-MM"));
                worksummary.SyncPersonWorkSummary(strSql);
                string sql = string.Format(@"insert into hrs_nosaareaworksummary
                                                  (ID,createuserid,createdate,createuserdeptcode,createuserorgcode,
                                                   createusername,areaname,areaid,areasuperid,areasuper,
                                                   dutydepart,dutydepartid,dutydepartcode,month,iscommit)
                                                  select NewGuid(),n.createuserid, n.createdate,
                                                         n.createuserdeptcode,n.createuserorgcode, n.createusername,
                                                         n.name, n.id, n.dutyuserid, n.dutyusername,
                                                         n.dutydepartname,n.dutydepartid, d.encode,sysdate,0 
                                                    from HRS_NOSAAREA n
                                                    left join base_department d on d.departmentid = n.dutydepartid 
                                        where n.state!=1 and id not in(select t.areaid from hrs_nosaareaworksummary t where to_char(t.month,'yyyy-MM')='{0}' and t.iscommit='1')", DateTime.Now.ToString("yyyy-MM"));
                if (worksummary.SyncPersonWorkSummary(sql))
                {
                    return Success("同步成功");
                }
                else
                {
                    return Error("执行同步失败");
                }
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 发送提醒
        /// </summary>
        /// <param name="objid">业务Id</param>
        /// <param name="superuserid">负责人Id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult sendMess(string objid, string superuserid)
        {
            var getUser = new UserBLL().GetEntity(superuserid);
            if (getUser != null)
            {
                JPushApi.PushMessage(getUser.Account, getUser.RealName, "NosaW001", objid);
                return Success("提醒成功"); ;
            }
            else
            {
                return Success("提醒失败,确认人员是否存在");
            }
        }
        /// <summary>
        /// 元素负责人工作总结提交
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommitWorkSummary(string keyValue)
        {
            worksummary.CommitPeopleSummary(keyValue);
            return Success("提交成功");
        }

        /// <summary>
        /// 区域代表工作总结提交
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CommitAreaWorkSummary(string keyValue)
        {
            worksummary.CommitAreaSummary(keyValue);
            return Success("提交成功");
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = worksummary.GetEntity(keyValue);
            return ToJsonResult(data);
        }
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
            worksummary.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, NosaPersonWorkSummaryEntity entity)
        {
            worksummary.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}