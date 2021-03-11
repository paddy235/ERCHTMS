
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
    /// �� ������Ϣ֪ͨ
    /// </summary>
    public class MessageController : MvcControllerBase
    {
        private MessageBLL messagebll = new MessageBLL();
        private MessageUserSetBLL messageusersetbll = new MessageUserSetBLL();
        private MessageDetailBLL messagedetailbll = new MessageDetailBLL();

        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// ����Ϣ���ù�עҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AttentionFrom()
        {
            return View();
        }

        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
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
                //pagination.sidx = "t.sendtime";//�����ֶ�
                //pagination.sord = "desc";//����ʽ
                
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
        /// ��ȡ����Ϣ�����б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetMessDetailListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.messageid,t.username,t.useraccount,t.userid,t.looktime,t.deptid,t.deptname,t.deptcode,t.status";
                pagination.p_tablename = @"base_messagedetail t";
                pagination.sidx = "t.looktime";//�����ֶ�
                pagination.sord = "desc";//����ʽ
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = messagebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�û�����Ϣ����
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
        /// ��ȡ��Ϣ����
        /// </summary>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetMessageCount()
        {
            Operator currUser = OperatorProvider.Provider.Current();
            return ToJsonResult(new MessageBLL().GetMessCountByUserId(currUser));
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            messagebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
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
            
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ����δ�鿴״̬
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
            return Success("�����ɹ���");

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
            return Success("�����ɹ���");
        }
        #endregion
    }
}
