
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using System;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.SystemManage.Controllers
{
    /// <summary>
    /// 描 述：消息通知
    /// </summary>
    public class MessageController : MvcControllerBase
    {
        private MessageBLL messagebll = new MessageBLL();
        private MessageUserSetBLL messageusersetbll = new MessageUserSetBLL();
        private MessageDetailBLL messagedetailbll = new MessageDetailBLL();

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
        /// 短消息设置关注页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AttentionFrom()
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
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                Operator currUser = OperatorProvider.Provider.Current();
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.username,t.sendtime,t.userid,t.title,t.content,d.status,t.readtime,t.remark,t.category,t.sendusername,t.senduser,nvl(d.undonenum,0) undonenum";
                pagination.p_tablename = string.Format(@"base_message t 
                                                    left join (select count(id) undonenum,d.messageid from  base_messagedetail d where d.status=0 group by d.messageid) d on d.messageid=t.id
                                                        left join (
                                                        select distinct messageid, status from base_messagedetail where useraccount ='{0}'
                                                    ) d on d.messageid=t.id ", currUser.Account);
                //pagination.sidx = "t.sendtime";//排序字段
                //pagination.sord = "desc";//排序方式
                
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                else
                {
                    pagination.conditionJson = string.Format(@" (t.userid like'%{0}%' or t.senduser = '{0}')", currUser.Account);
                }
                var data = messagebll.GetPageList(pagination, queryJson);
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
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 获取短消息详情列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetMessDetailListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.messageid,t.username,t.useraccount,t.userid,t.looktime,t.deptid,t.deptname,t.deptcode,t.status";
                pagination.p_tablename = @"base_messagedetail t";
                pagination.sidx = "t.looktime";//排序字段
                pagination.sord = "desc";//排序方式
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson = "  1=1 ";

                var data = messagedetailbll.GetPageList(pagination, queryJson);
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
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
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
            var data = messagebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取用户的消息设置
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserMessageSet(string userid)
        {
            var data = messageusersetbll.GetUserMessageSet(userid);
            return ToJsonResult(data);
        }
          /// <summary>
        /// 获取消息数量
        /// </summary>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetMessageCount()
        {
            Operator currUser = OperatorProvider.Provider.Current();
            return ToJsonResult(new MessageBLL().GetMessCountByUserId(currUser));
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
            messagebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, MessageEntity entity)
        {
            Operator currUser = OperatorProvider.Provider.Current();
            entity.SendTime = DateTime.Now;
            //entity.Status += currUser.Account + ",";
            if(messagebll.SaveForm(keyValue, entity)){
                JPushApi.PublicMessage(entity);
            }
            
            return Success("操作成功。");
        }
        /// <summary>
        /// 更新未查看状态
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="UserAccount"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult UpdateStatus(string keyValue, string UserAccount)
        {

            //entity = messagebll.GetEntity(keyValue);

            //entity.ReadTime = DateTime.Now;
            //entity.Status += UserAccount + ",";
            var detail = messagedetailbll.GetEntity(UserAccount, keyValue);
            if (detail != null)
            {
                detail.Status = 1;
                detail.LookTime = DateTime.Now;
                messagedetailbll.SaveForm(detail.Id, detail);
            }
            //messagebll.SaveForm(keyValue, entity);
            return Success("操作成功。");

        }


        public ActionResult SaveUserMessageSet(string keyValue, string SetCategory)
        {
            MessageUserSetEntity entity = new MessageUserSetEntity();
            if (!string.IsNullOrWhiteSpace(keyValue))
            {
                entity = messageusersetbll.GetEntity(keyValue);
            }
            entity.SetCategory = SetCategory;
            messageusersetbll.SaveUserMessageSet(keyValue, entity);
            return Success("操作成功。");
        }
        #endregion
    }
}
