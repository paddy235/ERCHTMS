using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.CarManage;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// 描 述：预警类型管理
    /// </summary>
    public class EarlywarningmanageController : MvcControllerBase
    {
        private EarlywarningmanageBLL earlywarningmanagebll = new EarlywarningmanageBLL();
        private SafeworkcontrolBLL safeworkcontrolbll = new SafeworkcontrolBLL();

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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = earlywarningmanagebll.GetList(queryJson);
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
            var data = earlywarningmanagebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "d.ID";
            pagination.p_fields = "d.condition,d.indexNom,d.IndexUnit,d.WarningResult,d.isEnable  ";
            pagination.p_tablename = @" bis_EarlyWarningManage d  ";
            pagination.conditionJson = " 1=1 ";

            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {//关键字查询
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and d.condition like '%{0}%'", keyword);
            }
            var data = safeworkcontrolbll.GetPageList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
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
            earlywarningmanagebll.RemoveForm(keyValue);
            //将信息同步到后台计算服务中
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            SendData sd = new SendData();
            sd.DataName = "DelEarlywarningmanageEntity";
            sd.EntityString = keyValue;
            rh.SendMessage(JsonConvert.SerializeObject(sd));
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
        public ActionResult SaveForm(string keyValue, EarlywarningmanageEntity entity)
        {
            earlywarningmanagebll.SaveForm(keyValue, entity);
            //将信息同步到后台计算服务中
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            SendData sd = new SendData();
            if(string.IsNullOrEmpty(keyValue)){ sd.DataName = "AddEarlywarningmanageEntity";}
            else { sd.DataName = "UpdateEarlywarningmanageEntity"; };
            sd.EntityString = JsonConvert.SerializeObject(entity);
            rh.SendMessage(JsonConvert.SerializeObject(sd));
            return Success("操作成功。");
        }
        /// <summary>
        /// 预警类型是否启用禁用
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectCheckEnabled(string keyValue,bool status)
        {
            var data = earlywarningmanagebll.GetEntity(keyValue);
            if (data != null)
            {
                if (status)
                {
                    data.Isenable = 1;
                }
                else
                {
                    data.Isenable = 0;
                }
                earlywarningmanagebll.SaveForm(keyValue, data);

                //将信息同步到后台计算服务中
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                sd.DataName = "UpdateEarlywarningmanageEntity";
                sd.EntityString = JsonConvert.SerializeObject(data);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
            }
            return Success("操作成功。");
        }
        
        #endregion
    }
}
