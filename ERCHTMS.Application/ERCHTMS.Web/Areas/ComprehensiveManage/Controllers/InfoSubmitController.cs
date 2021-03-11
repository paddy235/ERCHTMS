using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.ComprehensiveManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    /// <summary>
    /// 描 述：信息报送表
    /// </summary>
    public class InfoSubmitController : MvcControllerBase
    {
        private UserBLL userbll = new UserBLL(); //用户操作对象
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private InfoSubmitBLL infoSubmitbll = new InfoSubmitBLL();

        #region 视图功能
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }      
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ehsDepartCode = "";
            //当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            
            //报送提醒
            //ViewBag.Num = infoSubmitbll.CountIndex("1");

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
        public ActionResult ReferForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ReferDetail()
        {
            return View();
        }
        [HttpGet]
        public ActionResult InfoFiles()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();            
            var data = infoSubmitbll.GetList(pagination, queryJson);
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
            var data = infoSubmitbll.GetEntity(keyValue);                  
            //返回值
            var josnData = new
            {
                data = data
            };

            return Content(josnData.ToJson());
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
            var infoDetailBll = new InfoSubmitDetailsBLL();

            infoSubmitbll.RemoveForm(keyValue);//删除申请
            var list = infoDetailBll.GetList(string.Format(" and infoid='{0}'",keyValue));//删除详情
            foreach(var detail in list)
            {
                DeleteFiles(detail.ID);                
            }
            infoDetailBll.RemoveFormByInfoId(keyValue);

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
        public ActionResult SaveForm(string keyValue, InfoSubmitEntity entity)
        {
            //保存基本信息
            entity.IsSubmit = "否";
            entity.Pct = 0;
            entity.Remnum = entity.SubmitUserId.Split(new char[] { ',' }).Count();
            entity.RemUserName = entity.SubmitUserName;
            entity.RemDepartName = entity.SubmitDepartName;
            infoSubmitbll.SaveForm(keyValue, entity);

            return Success("操作成功。");
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, InfoSubmitEntity entity)
        {
            //保存基本信息
            entity.IsSubmit = "是";
            entity.Pct = 0;
            entity.Remnum = entity.SubmitUserId.Split(new char[] { ',' }).Count();
            entity.RemUserName = entity.SubmitUserName;
            entity.RemDepartName = entity.SubmitDepartName;
            infoSubmitbll.SaveForm(keyValue, entity);

            return Success("操作成功。");
        }       
        #endregion
    }
}
