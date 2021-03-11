using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Cache;
using System.Linq;
using System;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// 描 述：旁站监督作业信息
    /// </summary>
    public class SuperviseWorkInfoController : MvcControllerBase
    {
        private SuperviseWorkInfoBLL superviseworkinfobll = new SuperviseWorkInfoBLL();
        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private SafetychangeBLL safetychangebll = new SafetychangeBLL();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private DataItemCache dataItemCache = new DataItemCache();

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
        /// 选择高风险设施变动页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectChange()
        {
            return View();
        }

        /// <summary>
        /// 选择高风险脚手架页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectScaffold()
        {
            return View();
        }

        /// <summary>
        /// 选择高风险通用作业页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectCommon()
        {
            return View();
        }

        /// <summary>
        /// 选择作业类别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectWorkType()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="taskshareid">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetWorkSpecToJson(string taskshareid)
        {
            var data = superviseworkinfobll.GetList(string.Format(" and taskshareid='{0}'",taskshareid));
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="taskshareid">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetWorkByWidToJson(string workid)
        {
            var data = superviseworkinfobll.GetList(string.Format(" and id='{0}'", workid));
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="taskshareid">查询参数</param>
        /// <param name="teamid">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetWorkToJson(string taskshareid, string teamid)
        {
            var data = superviseworkinfobll.GetList(taskshareid,teamid);
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
            var data = superviseworkinfobll.GetEntity(keyValue);
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
            superviseworkinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SuperviseWorkInfoEntity entity)
        {
            superviseworkinfobll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveWorkForm(string taskshareid)
        {
            string jsondata = Request.Form["jsondata"].ToString();
            List<SuperviseWorkInfoEntity> list = JsonConvert.DeserializeObject<List<SuperviseWorkInfoEntity>>(jsondata);
            superviseworkinfobll.RemoveWorkByTaskShareId(taskshareid);
            for (int i = 0; i < list.Count; i++)
            {
                superviseworkinfobll.SaveForm("", list[i]);
            }
            return "1";
        }
        #endregion


        #region 高风险作业选择页面

        #region 通用
        /// <summary>
        /// 获取列表(获取即将作业和作业中)
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetSelectCommonWorkJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "Id as workid";
                pagination.p_fields = "case when workdepttype=0 then '单位内部' when workdepttype=1 then '外包单位' end workdepttypename,workdepttype,workdeptid,workdeptname,workdeptcode,applynumber,CreateDate,workplace,workcontent,workstarttime,workendtime,applyusername,EngineeringName,EngineeringId,workareacode,workareaname,workusernames";
                pagination.p_tablename = " bis_highriskcommonapply a";
                pagination.conditionJson = "applystate='5'";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司"))
                    {
                        pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format("  and ((WorkDeptCode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                    }
                    pagination.conditionJson += " and ((RealityWorkStartTime is null) or (RealityWorkStartTime is not null and RealityWorkEndTime is null))";
                }
                var data = highriskcommonapplybll.GetPageDataTable(pagination, queryJson);
                var watch = CommonHelper.TimerStart();
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
                return Error(ex.Message);
            }
        }
        #endregion

        #region 安全设施变动
        /// <summary>
        /// 获取设施变动列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetSelectChangeWorkJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "Id as workid";
                pagination.p_fields = "workunit,workunitid,workunitcode,case when workunittype='0'  then '单位内部'  when  workunittype='1' then '外包单位' end workunittypename,workunittype,changereason,workplace,applychangetime,returntime,projectid,projectname,workarea";
                pagination.p_tablename = " bis_Safetychange t";
                pagination.conditionJson = "iscommit=1 and isapplyover=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司"))
                    {
                        pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format("  and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                    }
                    pagination.conditionJson += " and ((t.id not in(select id from bis_safetychange where ((isaccpcommit=0 and isaccepover=0  and  RealityChangeTime is null) or (isaccpcommit=1 and isaccepover=1))))  or (isaccpcommit=0 and isaccepover=0 and  RealityChangeTime is null))";
                }
                var data = safetychangebll.GetPageList(pagination, queryJson);
                var watch = CommonHelper.TimerStart();
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
                return Error(ex.Message);
            }
          
        }
        #endregion

        #region 脚手架
        /// <summary>
        /// 获取脚手架列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetSelectScaffoldWorkJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "Id as workid";
                pagination.p_fields = "purpose,dismentlereason,setupcompanyname,setupcompanyid,setupcompanycode,case when setupcompanytype='0' then '单位内部'  when  setupcompanytype='1' then '外包单位' end setupcompanytypename,setupcompanytype,setupstartdate,setupenddate,setupaddress,dismentlestartdate,dismentleenddate,outprojectid,outprojectname,WORKAREA,setuppersons,dismentlepersons";
                pagination.p_tablename = " v_scaffoldledger";
                pagination.conditionJson = "1=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("厂级") || user.RoleName.Contains("公司"))
                    {
                        pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format("  and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                    }
                }
                var data = scaffoldbll.GetSelectPageList(pagination, queryJson);
                var watch = CommonHelper.TimerStart();
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
                return Error(ex.Message);
            }
           
        }
        #endregion
        #endregion

        #region 查询作业类别

        /// <summary>
        /// 查询作业类别树 
        /// </summary>
        /// <param name="checkMode">单选或多选，0:单选，1:多选</param>
        /// <param name="mode">查询模式，0:获取机构ID为Ids下所有的部门(即OrganizeId=Ids)，1:获取部门ID为Ids下所有部门(即ParentId=Ids)，2:获取部门的parentId在Ids中的部门(即ParentId in(Ids))，3:获取部门Id在Ids中的部门(即DepartmentId in(Ids))</param>
        /// <param name="typeIDs">角色IDs</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetTypeTreeJson(string typeIDs, int checkMode = 0, int mode = 0)
        {
            var treeList = new List<TreeEntity>();
            IEnumerable<DataItemModel> data = dataItemCache.GetDataItemList("TaskWorkType");
            foreach (DataItemModel item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? false : true;
                bool showcheck = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? true : false;
                tree.id = item.ItemDetailId;
                tree.text = item.ItemName;
                tree.value = item.ItemValue;
                tree.isexpand = true;
                tree.complete = true;
                if (!string.IsNullOrEmpty(typeIDs))
                {
                    var s = typeIDs.Split(',');
                    foreach (var arr in s)
                    {
                        if (arr == item.ItemValue) tree.checkstate = 1;
                    }
                }
                tree.showcheck = showcheck;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }
        #endregion
    }
}
