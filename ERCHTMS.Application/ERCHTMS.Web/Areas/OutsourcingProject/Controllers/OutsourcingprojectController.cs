using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：外包单位基础信息表
    /// </summary>
    public class OutsourcingprojectController : MvcControllerBase
    {
        private OutsourcingprojectBLL outsourcingprojectbll = new OutsourcingprojectBLL();
        private AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();
        private FileInfoBLL fileBll = new FileInfoBLL();

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

        [HttpGet]
        public ActionResult StatisticDefault()
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
            var data = outsourcingprojectbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 分页列表
        /// servicesstarttime 关联实际开工时间最小值
        /// servicesendtime   关联实际完工时间最大值
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
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.outsourcingname, 
                                            s.servicesstarttime, 
                                            s.servicesendtime, 
                                            s.engineerletdeptid,
                                            decode(t.outorin,'0','入场','1','离场','') outorin,
                                            t.legalrep,
                                            t.legalrepphone,
                                            t.outprojectid,b.managerid,
                                            b.senddeptid,b.createdate";
                pagination.p_tablename = @" base_department b
                                      inner join epg_outsourcingproject t on b.departmentid = t.outprojectid
                                      left join (select min(e.planenddate) servicesstarttime,
                                                    max(e.actualenddate) servicesendtime,e.outprojectid outprojectid,max(e.engineerletdeptid) engineerletdeptid
                                      from  epg_outsouringengineer e 
                                      group by e.outprojectid)s on   t.outprojectid=s.outprojectid ";
                pagination.sidx = "b.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                else if (currUser.RoleName.Contains("省级"))
                {
                    pagination.conditionJson = string.Format(@" b.encode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.description is null)", currUser.NewDeptCode);
                }
                else if (currUser.RoleName.Contains("厂级部门用户") || currUser.RoleName.Contains("公司级用户"))
                {
                    pagination.conditionJson = string.Format(" b.encode like '{0}%' ", currUser.OrganizeCode);
                }
                else if (currUser.RoleName.Contains("承包商级用户"))
                {
                    pagination.conditionJson = string.Format("  b.departmentid ='{0}'", currUser.DeptId);
                }
                else
                {
                    pagination.conditionJson = string.Format("  b.departmentid in(select distinct(t.outprojectid) from EPG_OUTSOURINGENGINEER t where t.engineerletdeptid='{0}')", currUser.DeptId);
                }
                pagination.conditionJson += string.Format(" and t.blackliststate='0' and b.parentid in (select departmentid from base_department t where t.description ='外包工程承包商' and Organizeid='{0}') ", currUser.OrganizeId);
                var data = outsourcingprojectbll.GetPageList(pagination, queryJson);


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

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = outsourcingprojectbll.GetEntity(keyValue);
            var zzData = aptitudeinvestigateinfobll.GetListByOutprojectId(data.OUTPROJECTID);

            var resultData = new
            {
                data = data,
                zzData = zzData
            };
            return ToJsonResult(resultData);
        }

        [HttpGet]
        public ActionResult GetEntityByDeptId(string DeptId)
        {
            var data = outsourcingprojectbll.GetOutProjectInfo(DeptId);
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
            outsourcingprojectbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OutsourcingprojectEntity entity)
        {
            outsourcingprojectbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }
        /// <summary>
        /// 离场/入场
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult Leave(string keyValue,string state)
        {
            OutsourcingprojectEntity pro= outsourcingprojectbll.GetEntity(keyValue);
            //单位离场时,该单位下的人员全部离场
            if (state == "1")
            {
                pro.OUTORIN = state;
                pro.LEAVETIME = DateTime.Now;
               var userList= new UserBLL().GetList().Where(x => x.DepartmentId == pro.OUTPROJECTID).ToList();
                if (userList.Count > 0) {
                    for (int i = 0; i < userList.Count; i++)
                    {
                        userList[i].DepartureTime = DateTime.Now;
                        userList[i].IsPresence = "0";
                        new UserBLL().SaveForm(userList[i].UserId,userList[i]);
                    }
                }
            }
            else {
                pro.OUTORIN = state;
                pro.LEAVETIME = null;
            }
            //pro.OUTORIN = state;
            //pro.LEAVETIME = DateTime.Now;
            outsourcingprojectbll.SaveForm(keyValue, pro);
            return Success("操作成功。");
        }
        #endregion


        [HttpGet]
        public string StaQueryList(string queryJson)
        {

            return outsourcingprojectbll.StaQueryList(queryJson);
        }
        [HttpGet]

        public ActionResult GetAllFactory() {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            return ToJsonResult(new DepartmentBLL().GetAllFactory(user));
        }
    }
}
