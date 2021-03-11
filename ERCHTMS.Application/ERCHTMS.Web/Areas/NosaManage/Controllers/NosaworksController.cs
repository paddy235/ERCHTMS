using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.BaseManage;
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.JPush;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// 描 述：工作任务
    /// </summary>
    public class NosaworksController : MvcControllerBase
    {
        private NosaworksBLL nosaworksbll = new NosaworksBLL();
        private NosaworkitemBLL nosaworkitembll = new NosaworkitemBLL();
        private NosaworkresultBLL nosaworkresultbll = new NosaworkresultBLL();

        #region 视图功能
        /// <summary>
        /// 上传成果
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UploadResult()
        {
            return View();
        }
        /// <summary>
        /// 上传列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UploadIndex()
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
        /// 详情页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
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
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = nosaworksbll.GetList(pagination, queryJson);
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
            var data = nosaworksbll.GetEntity(keyValue);
            //返回值
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
        }
        [HttpGet]
        public ActionResult GetDetailJson(string keyValue)
        {
            var data = nosaworksbll.GetEntity(keyValue);
            var resultInfo = nosaworkresultbll.GetList(string.Format(" and workid='{0}' order by createdate asc", keyValue));
            var itemInfo = nosaworkitembll.GetList(string.Format(" and workid='{0}' order by createdate asc", keyValue));
            //返回值
            var josnData = new
            {
                data,
                resultInfo,
                itemInfo
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
            nosaworksbll.RemoveForm(keyValue);
            new NosaworkresultBLL().RemoveByWorkId(keyValue);
            nosaworkitembll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, NosaworksEntity entity)
        {
            entity.DutyUserHtml = entity.DutyUserName;
            entity.DutyDepartHtml = entity.DutyDepartName;
            nosaworksbll.SaveForm(keyValue, entity);
            SaveWorkItem(entity);
            return Success("操作成功。");
        }
        private void SaveWorkItem(NosaworksEntity entity)
        {
            nosaworkitembll.RemoveForm(entity.ID);
            UserBLL userbll = new UserBLL();
            var listUserId = entity.DutyUserId.Split(new char[] { ',' });
            foreach(var userId in listUserId)
            {
                var user = userbll.GetUserInfoEntity(userId);
                var iEntity = new NosaworkitemEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    DutyUserId = userId,
                    DutyUserName = user.RealName,
                    DutyDepartId = user.DepartmentId,
                    DutyDepartName = user.DeptName,
                    WorkId = entity.ID,
                    IsSubmitted = "否",
                    State = "待上传",
                    CheckUserId = entity.EleDutyUserId,
                    CheckUserName = entity.EleDutyUserName
                };
                nosaworkitembll.SaveForm("", iEntity);
            }
        }        
        /// <summary>
        /// 复制任务
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CopyWork(string keyValue)
        {
            var msg = "复制成功";
            var newKeyValue = Guid.NewGuid().ToString();
            try
            {
                var entity = nosaworksbll.GetEntity(keyValue);
                if (entity != null)
                {                    
                    entity.ID = newKeyValue;
                    entity.IsSubmited = "否";
                    entity.SubmitUserId = entity.SubmitUserName = "";
                    entity.Pct = 0;
                    entity.CREATEDATE = entity.CREATEDATE.Value.AddSeconds(1);
                    nosaworksbll.SaveForm(newKeyValue, entity);
                    CopyWorkResult(keyValue, newKeyValue);
                    SaveWorkItem(entity);
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }

            return Success(msg, newKeyValue);
        }
        private void CopyWorkResult(string oldId,string newId)
        {
            var list = nosaworkresultbll.GetList(string.Format(" and workid='{0}' order by createdate asc", oldId));
            foreach(var rEntity in list)
            {
                rEntity.ID = Guid.NewGuid().ToString();
                rEntity.WorkId = newId;
                if (!string.IsNullOrWhiteSpace(rEntity.TemplatePath))
                {
                    string filePath = Server.MapPath(rEntity.TemplatePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        string sufx = System.IO.Path.GetExtension(filePath);
                        string newTemplatePath = string.Format("~/Resource/NosaWorkResult/{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), sufx);
                        string newFilePath = Server.MapPath(newTemplatePath);
                        System.IO.File.Copy(filePath, newFilePath);
                        rEntity.TemplatePath = newTemplatePath;
                    }
                }
                nosaworkresultbll.SaveForm(rEntity.ID, rEntity);
            }
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "数据导出")]
        public ActionResult Export(string queryJson, string sortname, string sortorder)
        {
            var pagination = new Pagination()
            {
                page = 1,
                rows = 100000,
                sidx = string.IsNullOrWhiteSpace(sortname) ? "createdate" : sortname,
                sord = string.IsNullOrWhiteSpace(sortorder) ? "asc" : sortorder
            };
            var dt = nosaworksbll.GetList(pagination, queryJson);
            string fileUrl = @"\Resource\ExcelTemplate\NOSA元素工作清单_导出模板.xlsx";
            AsposeExcelHelper.ExecuteResult(dt, fileUrl, "工作清单", "NOS元素工作清单");

            return Success("导出成功。");
        }
        /// <summary>
        /// 短消息提醒未提交工作成果的负责人
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult WarningDutyUser(string keyValue)
        {
            var msg = "已发送短消息提醒未提交工作成果的责任人";
            try
            {
                var entity = nosaworksbll.GetEntity(keyValue);
                if (entity != null)
                {
                    SendMessage(entity);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Success(msg);
        }
        private void SendMessage(NosaworksEntity entity)
        {
            var dList = entity.DutyUserId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var sList = new string[] {};
            if (!entity.SubmitUserId.IsNullOrWhiteSpace())
                sList = entity.SubmitUserId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if ( dList.Length >sList.Length)
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var eList = dList.Except(sList);
                var aList = new UserBLL().GetListForCon(x => eList.Contains(x.UserId));
                MessageEntity msg = new MessageEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = string.Join(",", aList.Select(x => x.Account)),
                    UserName = string.Join(",", aList.Select(x => x.RealName)),
                    SendTime = DateTime.Now,
                    SendUser = user.Account,
                    SendUserName = entity.CREATEUSERNAME,
                    Title = "NOSA工作成果上传提醒",
                    Content = string.Format("您有一项“{0}”的NOSA工作成果未上传，请即时上传。", entity.Name),
                    Category = "其它"
                };
                if (new MessageBLL().SaveForm("", msg))
                {
                    JPushApi.PublicMessage(msg);
                }
            }
        }
        #endregion
    }
}
