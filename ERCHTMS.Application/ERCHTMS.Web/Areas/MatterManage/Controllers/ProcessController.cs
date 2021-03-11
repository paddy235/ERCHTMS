using ERCHTMS.Entity.MatterManage;
using ERCHTMS.Busines.MatterManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.MatterManage.Controllers
{
    /// <summary>
    /// 描 述：过程记录管理
    /// </summary>
    public class ProcessController : MvcControllerBase
    {
        private OperticketmanagerBLL operticketmanagerbll = new OperticketmanagerBLL();

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
        /// 打印视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Stamp()
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
        public ActionResult GetPageList(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "Id";
            pagination.p_fields = "Numbers,createdate,GetData,ProductType,SupplyName,isdelete,PlateNumber,Dress,Remark,transporttype,takegoodsname,PassRemark,IsFirst,IsTrajectory,WeighingNum,DataBaseNum,OutDate,StayTime,Status,deletecontent";
            pagination.p_tablename = "WL_OPERTICKETMANAGER";
            pagination.conditionJson = "1=1 ";
            var watch = CommonHelper.TimerStart();
            var data = operticketmanagerbll.GetPageList(pagination, queryJson);
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
            var data = operticketmanagerbll.GetEntity(keyValue);
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
            operticketmanagerbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OperticketmanagerEntity entity)
        {
            operticketmanagerbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        /// <summary>
        /// 修改记录状态
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpateStatus(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                data.Isdelete = 0;
                data.DeleteContent = entity.DeleteContent;
                operticketmanagerbll.SaveForm(keyValue, data);
            }
            return Success("操作成功。");
        }

        /// <summary>
        /// 打印时将该记录生成二维码图片
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveImg(string keyValue, OperticketmanagerEntity entity)
        {
            var data = operticketmanagerbll.GetEntity(keyValue);
            if (data != null)
            {
                operticketmanagerbll.SaveForm(keyValue, data);
            }
            return Success("操作成功。");
        }


        #endregion
    }
}
