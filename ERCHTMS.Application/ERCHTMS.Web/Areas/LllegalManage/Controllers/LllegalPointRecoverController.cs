using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Busines.LllegalManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：违章积分恢复单
    /// </summary>
    public class LllegalPointRecoverController : MvcControllerBase
    {
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL();
        private LllegalPointRecoverBLL lllegalpointrecoverbll = new LllegalPointRecoverBLL();
        private LllegalPointRecoverDetailBLL lllegalpointrecoverdetailbll = new LllegalPointRecoverDetailBLL(); //明细


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
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RecordIndex()
        {
            return View();
        }
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RecordForm()
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

        #region 获取违章列表数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();

            pagination.p_fields = "username,createusername,createdate,id";
            pagination.p_tablename = string.Format(@" (select wm_concat(b.recoverusername) username,a.createdate,a.id,a.createusername  from BIS_LLLEGALPOINTRECOVER  a
                                                      left join bis_lllegalpointrecoverdetail b on a.id =b.recoverid group by a.createdate,a.id ,a.createusername) a");
            pagination.conditionJson = "1=1";
            var data = lllegalregisterbll.GetGeneralQuery(pagination);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());

        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetDetailListJson(Pagination pagination, string keyValue) 
        {
            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();

            pagination.p_fields = "id,username,deptname";
            pagination.p_tablename = string.Format(@" (select a.id, b.realname username,b.deptname from bis_lllegalpointrecoverdetail a 
                                     left join v_userinfo b on a.recoveruserid = b.userid  where a.recoverid ='{0}') a", keyValue);
            pagination.conditionJson = "1=1";
            var data = lllegalregisterbll.GetGeneralQuery(pagination);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = lllegalpointrecoverbll.GetEntity(keyValue);
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
            lllegalpointrecoverbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, LllegalPointRecoverEntity entity)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.RECOVERNUMBER = DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            lllegalpointrecoverbll.SaveForm(keyValue, entity);
            //用户集合
            string userset = Request.Form["userset"];
            if (!string.IsNullOrEmpty(userset))
            {
                JArray jarray = (JArray)JsonConvert.DeserializeObject(userset);
                foreach (JObject rhInfo in jarray)
                {
                    string userid = rhInfo["userid"].ToString(); //用户id
                    string username = rhInfo["username"].ToString(); //用户姓名
                    string year = rhInfo["year"].ToString(); //年度
                    LllegalPointRecoverDetailEntity detailEntity = new LllegalPointRecoverDetailEntity();
                    detailEntity.RECOVERUSERID = userid;
                    detailEntity.RECOVERUSERNAME = username;
                    detailEntity.RECOVERYEAR = year;
                    detailEntity.RECOVERID = entity.ID;
                    lllegalpointrecoverdetailbll.SaveForm("", detailEntity); //保存明细
                }
            }
            return Success("操作成功。");
        }
        #endregion
    }
}