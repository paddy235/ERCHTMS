using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using System.Data;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// 描 述：五定安全检查 整改验收表
    /// </summary>
    public class FivesafetycheckauditController : MvcControllerBase
    {
        private FivesafetycheckauditBLL fivesafetycheckauditbll = new FivesafetycheckauditBLL();
        private FivesafetycheckBLL fivesafetycheckbll = new FivesafetycheckBLL();

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
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GeDataTableByIds(string ids)
        {
            var data = fivesafetycheckauditbll.GeDataTableByIds(ids);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = fivesafetycheckauditbll.GetList(queryJson);
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
            var data = fivesafetycheckauditbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = fivesafetycheckauditbll.GetPageListJson(pagination, queryJson);
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
        #endregion

        #region 提交数据
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValueids"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckNumAudit(string keyValueids,string queryJson)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            foreach (string keyValue in queryJson.Split(','))
            {
                FivesafetycheckauditEntity applyentity = fivesafetycheckauditbll.GetEntity(keyValue);

                if (applyentity.ACCEPTREUSLT != "0" && applyentity.ACTIONRESULT == "0" && applyentity.ACCEPTUSERID == curUser.UserId) // 已整改完，未验收的，验收人为当前用户的数据执行批量验收操作
                {
                    applyentity.ACCEPTCONTENT = "";
                    applyentity.ACCEPTREUSLT = "0";

                    fivesafetycheckauditbll.SaveForm(keyValue, applyentity);

                    #region 更新主表信息
                    string checkid = applyentity.CHECKID;
                    var checkentity = fivesafetycheckbll.GetEntity(checkid);

                    DataTable dt = fivesafetycheckauditbll.GetDataTable("select id from BIS_FIVESAFETYCHECKAUDIT where checkpass  = '1' and (ACCEPTREUSLT is null or  ACCEPTREUSLT = '1') and checkid = '" + checkid + "' ");
                    // 所有的验收已完成，更新主表信息为已完成
                    if (dt.Rows.Count == 0)
                    {
                        checkentity.ISOVER = 3;

                    }
                    fivesafetycheckbll.SaveForm(checkid, checkentity);
                }

                #endregion

                
            }
            return Success("操作成功。");

        }


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
            fivesafetycheckauditbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FivesafetycheckauditEntity entity)
        {
            fivesafetycheckauditbll.SaveForm(keyValue, entity);
            return Success("操作成功。");
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="istype">0:整改 1：验收</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApplyForm(string keyValue,string istype, FivesafetycheckauditEntity entity)
        {
            FivesafetycheckauditEntity applyentity = fivesafetycheckauditbll.GetEntity(keyValue);
            
            if (istype == "0") // 0:整改 1：验收
            {
                applyentity.BEIZHU = entity.BEIZHU;
                applyentity.ACTUALDATE = entity.ACTUALDATE;
                applyentity.ACTIONRESULT = entity.ACTIONRESULT;
                if (entity.ACTIONRESULT == "0")
                {
                    applyentity.ACCEPTREUSLT = "";
                    applyentity.ACCEPTCONTENT = "";   
                }
            }
            else
            {
                applyentity.ACCEPTCONTENT = entity.ACCEPTCONTENT;
                applyentity.ACCEPTREUSLT = entity.ACCEPTREUSLT;
                if (entity.ACCEPTREUSLT == "1") //不同意
                {
                    applyentity.ACTIONRESULT = "1";
                    applyentity.ACTUALDATE = null;

                    JPushApi.PushMessage(new UserBLL().GetEntity(applyentity.DUTYUSERID).Account, applyentity.DUTYUSERNAME, "WDJC001", applyentity.ID);

                }
            }
            
            fivesafetycheckauditbll.SaveForm(keyValue, applyentity);

            #region 更新主表信息
            string checkid = applyentity.CHECKID;
            var checkentity = fivesafetycheckbll.GetEntity(checkid);

            if (istype == "0") // 0:整改 1：验收
            {
                //整改同意，需要判断是否全部整改完
                if (entity.ACTIONRESULT == "0")
                {
                    DataTable dt = fivesafetycheckauditbll.GetDataTable("select id from BIS_FIVESAFETYCHECKAUDIT where checkpass  = '1' and (ACTIONRESULT is null or  ACTIONRESULT = '1') and checkid = '" + checkid + "' ");
                    // 所有的整改已完成，更新主表信息为验收中
                    if (dt.Rows.Count == 0)
                    {
                        checkentity.ISOVER = 2;

                    }
                }
            }
            else
            {
                if (entity.ACCEPTREUSLT == "0") //同意
                {
                    DataTable dt = fivesafetycheckauditbll.GetDataTable("select id from BIS_FIVESAFETYCHECKAUDIT where checkpass  = '1' and (ACCEPTREUSLT is null or  ACCEPTREUSLT = '1') and checkid = '" + checkid + "' ");
                    // 所有的验收已完成，更新主表信息为已完成
                    if (dt.Rows.Count == 0)
                    {
                        checkentity.ISOVER = 3;

                    }
                }
                else // 不同意将直接改成整改中
                {
                    checkentity.ISOVER = 1;
                }
            }
            fivesafetycheckbll.SaveForm(checkid, checkentity);

            #endregion


            return Success("操作成功。");
        }
        #endregion
    }
}
