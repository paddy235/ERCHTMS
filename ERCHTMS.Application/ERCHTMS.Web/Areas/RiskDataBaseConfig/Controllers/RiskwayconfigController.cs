using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.Busines.RiskDataBaseConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Web.Areas.RiskDataBaseConfig.Controllers
{
    /// <summary>
    /// 描 述：安全风险管控取值配置表
    /// </summary>
    public class RiskwayconfigController : MvcControllerBase
    {
        private RiskwayconfigBLL riskwayconfigbll = new RiskwayconfigBLL();
        private RiskwayconfigdetailBLL riskwayconfigdetailbll = new RiskwayconfigdetailBLL();

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
        public ActionResult CreateForm()
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
,createdate,risktype,waytype,waytypecode,deptid,deptcode,deptname,createusername";
                pagination.p_tablename = "bis_riskwayconfig";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("((createuserid='System' and iscommit='1') or (createuserorgcode='{0}' and iscommit='1') or (createuserid='{1}'))", user.OrganizeCode,user.UserId);
                }
                var data = riskwayconfigbll.GetPageList(pagination, queryJson);
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
                var data = riskwayconfigbll.GetList(queryJson);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// 根据类型获取数据
        /// </summary>
        /// <param RiskType="风险类型"></param>
        /// <param WayType="取值类型"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataByType(string RiskType, string WayType)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = riskwayconfigbll.GetList("").Where(x => x.RiskType == RiskType && x.WayType == WayType&&x.IsCommit=="1").ToList();
                if (data.Count > 0)
                {
                    var returnData = data.Where(x => x.DeptCode == user.OrganizeCode).ToList();
                    if (returnData.Count > 0)
                    {
                        for (int i = 0; i < returnData.Count; i++)
                        {
                            returnData[i].DetaileList = riskwayconfigdetailbll.GetList("").Where(x => x.WayConfigId == returnData[i].ID).ToList();
                        }
                        return ToJsonResult(returnData);
                    }
                    else
                    {
                        var rData = data.Where(x => x.DeptCode == "0").ToList();
                        for (int i = 0; i < rData.Count; i++)
                        {
                            rData[i].DetaileList = riskwayconfigdetailbll.GetList("").Where(x => x.WayConfigId == rData[i].ID).ToList();
                        }
                        return ToJsonResult(rData);
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
        /// 是否存在相同类型数据
        /// </summary>
        /// <param RiskType="风险类型"></param>
        /// <param WayType="取值类型"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsExistDataByType(string RiskType, string WayType) {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = riskwayconfigbll.GetList("").Where(x => x.RiskType == RiskType && x.WayType == WayType && x.DeptCode == user.OrganizeCode).ToList();
                if (data.Count > 0)
                {
                    return ToJsonResult(false);
                }
                else
                {
                    return ToJsonResult(true);
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
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
                var data = riskwayconfigbll.GetEntity(keyValue);
                data.DetaileList = riskwayconfigdetailbll.GetList("").Where(x => x.WayConfigId == data.ID).ToList();
                return ToJsonResult(data);
            }
            catch (Exception ex)
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
                riskwayconfigbll.RemoveForm(keyValue);
                return Success("删除成功。");
            }
            catch (Exception ex)
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
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, RiskwayconfigEntity entity, string wayconfigArray)
        {
            try
            {
                List<RiskwayconfigdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RiskwayconfigdetailEntity>>(wayconfigArray);
                riskwayconfigbll.SaveForm(keyValue, entity, list);
                return Success("操作成功。");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
    }
}
