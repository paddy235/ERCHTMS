using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.Busines.RiskDataBaseConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Linq;

namespace ERCHTMS.Web.Areas.RiskDataBaseConfig.Controllers
{
    /// <summary>
    /// 描 述：安全风险管控配置表
    /// </summary>
    public class RiskdatabaseconfigController : MvcControllerBase
    {
        private RiskdatabaseconfigBLL riskdatabaseconfigbll = new RiskdatabaseconfigBLL();

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
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LecIndex()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LecForm()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LecCreate()
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
            try
            {
                var watch = CommonHelper.TimerStart();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "id";
                pagination.p_fields = @"createuserid,createuserdeptcode,createuserorgcode,iscommit
,createdate,risktype,configtype,configtypecode,itemtype,itemtypecode,deptid,deptcode,deptname,createusername";
                pagination.p_tablename = "bis_riskdatabaseconfig";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("((createuserid='System' and iscommit='1') or (createuserorgcode='{0}' and iscommit='1') or (createuserid='{1}'))", user.OrganizeCode, user.UserId);
                }
                var data = riskdatabaseconfigbll.GetPageList(pagination, queryJson);
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

                return Error(ex.Message);
            }
         
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            try
            {
                var data = riskdatabaseconfigbll.GetList();
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);
            }
           
        }
        /// <summary>
        /// 根据类型获取风险配置数据
        /// </summary>
        /// <param name="RiskType">风险类型</param>
        /// <param name="ConfigType">配置类型</param>
        /// <param name="ItemType">类型细分</param>
        /// <param name="DataType">数据来源1 风险矩阵法 2 风险选择配置</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataByType(string RiskType, string ConfigType, string DataType,string ItemType = "")
        {
            try
            {
                //查询本单位是否存在配置内容,如果没有在查询系统是否内置配置,都没有则返回空
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var strWhere = string.Empty;
                if (!string.IsNullOrWhiteSpace(ItemType))
                {
                    strWhere = string.Format(@" and t.ItemType='{0}'",ItemType);
                }
                string sql = string.Format(@"select t.configcontent from bis_riskdatabaseconfig t where t.ConfigType='{0}' 
and t.iscommit='1' and t.datatype='{1}' and t.risktype='{2}' and t.createuserorgcode='{3}'{4}", ConfigType, DataType, RiskType,user.OrganizeCode,strWhere);
                var data = riskdatabaseconfigbll.GetTable(sql);
                if (data.Rows.Count > 0)
                {
                    return ToJsonResult(data);
                }
                else {
                     sql = string.Format(@"select t.configcontent from bis_riskdatabaseconfig t where t.ConfigType='{0}' 
and t.iscommit='1' and t.datatype='{1}' and t.risktype='{2}' and t.createuserorgcode='{3}'{4}", ConfigType, DataType, RiskType, "0", strWhere);
                     data = riskdatabaseconfigbll.GetTable(sql);
                     return ToJsonResult(data);
                }
                //var data = riskdatabaseconfigbll.GetList().Where(x => x.RiskType == RiskType && x.ConfigType == ConfigType && x.IsCommit == "1" && x.DataType == DataType);
                //if (!string.IsNullOrWhiteSpace(ItemType))
                //{
                //    var d = data.Where(x => x.ItemType == ItemType && x.DeptCode == user.OrganizeCode);
                //    if (d.ToList().Count == 0)
                //    {
                //        var d1 = data.Where(x => x.ItemType == ItemType && x.DeptCode == "0");
                //        return ToJsonResult(d1);
                //    }
                //    else {
                //        return ToJsonResult(d);
                //    }
                //}
                //else {
                //    var d2 = data.Where(x => x.DeptCode == user.OrganizeCode);
                //    if (d2.ToList().Count == 0)
                //    {
                //        var d3 = data.Where(x => x.DeptCode == "0");
                //        return ToJsonResult(d3);
                //    }
                //    else {
                //        return ToJsonResult(d2);
                //    }
                //}
            }
            catch (System.Exception ex)
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
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
                var data = riskdatabaseconfigbll.GetEntity(keyValue);
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);
            }
            
           
        }
        /// <summary>
        /// 是否存在相同类型的安全风险配置
        /// </summary>
        /// <param name="RiskType">风险类型</param>
        /// <param name="ConfigType">配置类型</param>
        /// <param name="ItemType">类型细分</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsExitByType(string RiskType, string ConfigType, string ItemType)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var old = riskdatabaseconfigbll.GetList().Where(x => x.DeptCode == user.OrganizeCode && x.RiskType == RiskType && x.ConfigType == ConfigType);
                if (string.IsNullOrWhiteSpace(ItemType))
                {
                    if (old.ToList().Count > 0)
                    {
                        return ToJsonResult(false);
                    }
                }
                else
                {
                    old = old.Where(x => x.ItemType == ItemType).ToList();
                    if (old.ToList().Count > 0)
                    {
                        return ToJsonResult(false);
                    }
                }
                return ToJsonResult(true);
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);
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
            try
            {
                riskdatabaseconfigbll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, RiskdatabaseconfigEntity entity)
        {
            try
            {
                riskdatabaseconfigbll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);
            }
          
        }
        #endregion
    }
}
