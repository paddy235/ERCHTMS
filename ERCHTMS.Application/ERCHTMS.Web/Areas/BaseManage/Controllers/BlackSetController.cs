using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Busines.PersonManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using System.Collections.Generic;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
namespace ERCHTMS.Web.Areas.BaseManage.Controllers
{
    /// <summary>
    /// 描 述：黑名单标准设置
    /// </summary>
    public class BlackSetController : MvcControllerBase
    {
        private BlackSetBLL scoresetbll = new BlackSetBLL();

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
            var data = scoresetbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetItemListJson()
        {
            var data = scoresetbll.GetList(ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取年龄条件
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAgeRange(string deptCode)
        {
            var data = scoresetbll.GetAgeRange(ERCHTMS.Code.OperatorProvider.Provider.Current().OrganizeCode);
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
            DataItemDetailBLL itemBll = new DataItemDetailBLL();
            ERCHTMS.Entity.SystemManage.DataItemDetailEntity entity = itemBll.GetEntity(keyValue);
            return ToJsonResult(entity);
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
            scoresetbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        ///  <param name="score">初始积分值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue,string itemJson)
        {
            List<BlackSetEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BlackSetEntity>>(itemJson);
            if (list.Count>0)
            {
                new BlackSetBLL().SaveForm(keyValue, list);
                return Success("操作成功。");
            }
            else
            {
                return Error("请填写完整正确的数据！");
            }
          
        }
        #endregion
    }
}
