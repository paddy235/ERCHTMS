using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// 描 述：工作成果
    /// </summary>
    public class NosaworkitemController : MvcControllerBase
    {
        private NosaworksBLL nosaworksbll = new NosaworksBLL();
        private NosaworkresultBLL nosaworkresultbll = new NosaworkresultBLL();
        private NosaworkitemBLL nosaworkitembll = new NosaworkitemBLL();

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
        /// 审核成果
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckResult()
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
            var data = nosaworkitembll.GetList(pagination, queryJson);
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
        public ActionResult GetFormJson(string workid,string dutyuserid,string keyValue)
        {
            var workInfo = nosaworksbll.GetEntity(workid);
            var resultInfo = nosaworkresultbll.GetList(string.Format(" and workid='{0}' order by createdate asc", workid));
            var data = nosaworkitembll.GetEntity(keyValue);
            if (data == null)
            {
                var list = nosaworkitembll.GetList(string.Format(" and workid='{0}' and dutyuserid='{1}' ", workid, dutyuserid)).ToList();
                if (list != null && list.Count > 0)
                    data = list[0];
            }
            //返回值
            var josnData = new
            {
                workInfo,
                resultInfo,
                data
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
        public ActionResult SaveForm(string keyValue, NosaworkitemEntity entity)
        {
            nosaworkitembll.SaveForm(keyValue, entity);
            if (entity.State == "通过")
            {
                var wEntity = nosaworksbll.GetEntity(entity.WorkId);
                if (wEntity != null)
                {
                    //更新完成进度、责任人、责任部门的显示标签
                    UpdateWorkEntity(wEntity, entity);
                    nosaworksbll.SaveForm(wEntity.ID, wEntity);
                }
            }
            else if (entity.State == "不通过")
            {
                var wEntity = nosaworksbll.GetEntity(entity.WorkId);
                if (wEntity != null && !string.IsNullOrWhiteSpace(wEntity.SubmitUserId) && !string.IsNullOrWhiteSpace(wEntity.SubmitUserName))
                {
                    wEntity.SubmitUserId = wEntity.SubmitUserId.Replace(entity.DutyUserId + ",", "");
                    wEntity.SubmitUserName = wEntity.SubmitUserName.Replace(entity.DutyUserName + ",", "");
                    //更新完成进度、责任人、责任部门的显示标签
                    UpdateWorkEntity(wEntity, entity);
                    nosaworksbll.SaveForm(wEntity.ID, wEntity);
                }
            }

            return Success("操作成功。");
        }
        private void UpdateWorkEntity(NosaworksEntity wEntity,NosaworkitemEntity iEntity)
        {
            if(wEntity!=null && iEntity != null)
            {
                //完成进度
                var total = wEntity.DutyUserId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Count();
                var num = nosaworkitembll.GetList(string.Format(" and workid='{0}' and state='通过'", iEntity.WorkId)).Count();
                wEntity.Pct = (decimal)Math.Round((num * 1.0) / total * 100, 2);
                //责任人、责任部门标签                
                string oldName = iEntity.DutyUserName;                
                string newName = string.Format("<span style='color:#00CC99;' title='已完成本次任务'>{0},</span>", oldName);
                string oldDepartName = iEntity.DutyDepartName;                
                string newDepartName = string.Format("<span style='color:#00CC99;' title='已完成本次任务'>{0},</span>", oldDepartName);
                var list = nosaworkitembll.GetList(string.Format(" and workid='{0}' and dutydepartid='{1}'", iEntity.WorkId, iEntity.DutyDepartId)).ToList();
                if (iEntity.State == "通过")
                {
                    Regex regTrm = new Regex(",</span>$");
                    Regex regUser = new Regex(oldName + ",?");
                    Regex regDept = new Regex(oldDepartName + ",?");
                    wEntity.DutyUserHtml = regUser.Replace(wEntity.DutyUserHtml, newName);
                    wEntity.DutyUserHtml = regTrm.Replace(wEntity.DutyUserHtml, "</span>");
                    if (list.Count == list.Count(x => x.State == "通过"))
                    {
                        wEntity.DutyDepartHtml = regDept.Replace(wEntity.DutyDepartHtml, newDepartName);
                        wEntity.DutyDepartHtml = regTrm.Replace(wEntity.DutyDepartHtml, "</span>");
                    }
                }
            }
        }
        /// <summary>
        /// 上传工作成果
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult UploadForm()
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string workId = Request["WorkId"];         
            var list = nosaworkitembll.GetList(string.Format(" and workid='{0}' and dutyuserid='{1}'", workId, user.UserId)).ToList();
            NosaworkitemEntity entity = null;
            if (list.Count > 0)
            {
                entity = list[0];
            }
            if (entity != null)
            {
                entity.IsSubmitted = Request["IsSubmited"];
                entity.State = entity.IsSubmitted == "是" ? "待审核" : "待上传";
                entity.UploadDate = DateTime.Now;
                nosaworkitembll.SaveForm(entity.ID, entity);
                if (entity.IsSubmitted == "是")
                {
                    var wEntity = nosaworksbll.GetEntity(entity.WorkId);
                    if (wEntity != null)
                    {
                        wEntity.SubmitUserId += entity.DutyUserId + ",";
                        wEntity.SubmitUserName += entity.DutyUserName + ",";                   
                        nosaworksbll.SaveForm(wEntity.ID, wEntity);
                    }
                }
            }
        
            return Success("操作成功。");
        }
        #endregion
    }
}
