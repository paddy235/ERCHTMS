using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using System.Linq;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：外包流程配置表
    /// </summary>
    public class OutprocessconfigController : MvcControllerBase
    {
        private OutprocessconfigBLL outprocessconfigbll = new OutprocessconfigBLL();

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
        public ActionResult Create()
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
        public ActionResult GetListJson()
        {
            var data = outprocessconfigbll.GetList();
            return ToJsonResult(data);
        }
        /// <summary>
        /// 是否存在配置项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsConfigExist()
        {
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                var data = outprocessconfigbll.GetList().Where(x => x.DeptId == currUser.OrganizeId).ToList();
                return ToJsonResult(data.Count);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }


        }
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"b.fullname as deptname ";
                pagination.p_tablename = @"WF_SCHEMECONTENT t inner join base_department b on t.WFSCHEMEINFOID=b.departmentid";
                pagination.sidx = "t.createdate";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                //else if (currUser.RoleName.Contains("省级"))
                //{
                //    pagination.conditionJson = string.Format(@" t.deptcode  in (select encode
                //        from base_department d
                //        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", currUser.NewDeptCode);
                //}
                //else 
                //{
                //    pagination.conditionJson = string.Format(" t.deptid ='{0}' ", currUser.OrganizeId);
                //}

                var data = outprocessconfigbll.GetPageListJson(pagination, queryJson);


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
            var data = outprocessconfigbll.GetEntity(keyValue);
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
            outprocessconfigbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OutprocessconfigEntity entity, string mode)
        {
            if (string.IsNullOrWhiteSpace(entity.FrontModuleCode) && string.IsNullOrWhiteSpace(entity.FrontModuleName))
            {
                entity.FrontModuleName = " ";
                entity.FrontModuleCode = " ";
            }
            if (mode == "Create")
            {
                var count = outprocessconfigbll.IsExistByModuleCode(entity.DeptId, entity.ModuleCode);
                if (count > 0)
                {
                    return Error("该模块已经配置,请勿重复配置。");
                }
                else
                {
                    outprocessconfigbll.SaveForm(keyValue, entity);
                    return Success("配置成功。");
                }
            }
            else
            {
                outprocessconfigbll.SaveForm(keyValue, entity);
                return Success("配置成功。");
            }


        }
        #endregion
    }
}
