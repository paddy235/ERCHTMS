using BSFramework.Util;
using BSFramework.Util.Extension;
using BSFramework.Util.Offices;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.ComprehensiveManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Entity.ComprehensiveManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    /// <summary>
    /// 描 述：信息发送详情
    /// </summary>
    public class InfoSubmitDetailsController : MvcControllerBase
    {
        private InfoSubmitBLL infoSubmitbll = new InfoSubmitBLL();
        private InfoSubmitDetailsBLL infoSubmitDetailsbll = new InfoSubmitDetailsBLL();

        #region 视图功能    
        /// <summary>
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// 添加修改页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
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
            return View();
        }       
        #endregion

        #region 获取数据        
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {  
            var data = infoSubmitDetailsbll.GetEntity(keyValue);         
            //返回值
            var josnData = new
            {
                data               
            };

            return Content(josnData.ToJson());
        }
        [HttpGet]
        public ActionResult GetEntityJson(string infoId,string userId)
        {
            var data = infoSubmitDetailsbll.GetList(string.Format(" and infoid='{0}' and createuserid='{1}'", infoId, userId)).FirstOrDefault();            
            //返回值
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = infoSubmitDetailsbll.GetList(pagination, queryJson);
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
            infoSubmitDetailsbll.RemoveForm(keyValue);//删除      
            DeleteFiles(keyValue);

            return Success("删除成功。");
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, InfoSubmitDetailsEntity pEntity)
        {       
            infoSubmitDetailsbll.SaveForm(keyValue, pEntity);

            return Success("操作成功。");
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="pEntity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, InfoSubmitDetailsEntity pEntity)
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pEntity.IsSubmit = "是";
            infoSubmitDetailsbll.SaveForm(keyValue, pEntity);
            //更新报送人员信息
            var entity = infoSubmitbll.GetEntity(pEntity.InfoId);
            if (entity != null)
            {
                entity.SubmitedUserId += user.UserId;
                entity.Remnum--;
                entity.Remnum = entity.Remnum < 0 ? 0 : entity.Remnum;
                if (!string.IsNullOrWhiteSpace(entity.RemUserName))
                {
                    entity.RemUserName = entity.RemUserName.Replace(user.UserName, "");
                    entity.RemUserName = string.Join(",",entity.RemUserName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                }
                if (!string.IsNullOrWhiteSpace(entity.RemDepartName))
                {
                    entity.RemDepartName = entity.RemDepartName.Replace(user.DeptName, "");
                    entity.RemDepartName = string.Join(",", entity.RemDepartName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                }                
                var num = entity.SubmitUserId.Split(new char[] { ',' }).Count();
                entity.Pct = (decimal)Math.Round((num - entity.Remnum.Value) * 1.0 / num * 100, 2);
                infoSubmitbll.SaveForm(entity.ID, entity);               
            }

            return Success("操作成功。");
        }
        #endregion

    }
}