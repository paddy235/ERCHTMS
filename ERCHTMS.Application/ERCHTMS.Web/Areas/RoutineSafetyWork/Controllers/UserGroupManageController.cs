using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Code;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// �� �����û������
    /// </summary>
    public class UserGroupManageController : MvcControllerBase
    {
        private UserGroupManageBLL usergroupmanagebll = new UserGroupManageBLL();
        private UserBLL userBLL = new UserBLL();

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
        /// ѡ���û���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            pagination.p_kid = "ID";
            pagination.p_fields = "UserGroupName,UserId,UserName";
            pagination.p_tablename = "BIS_UserGroupManage t";
            pagination.conditionJson = string.Format(" createuserid='{0}'", user.UserId);
            var watch = CommonHelper.TimerStart();
            var data = usergroupmanagebll.GetPageList(pagination);
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
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = usergroupmanagebll.GetList();
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = usergroupmanagebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetTreeListJson(string queryJson)
        {
            Operator user = OperatorProvider.Provider.Current();
            List<UserGroupManageEntity> data = usergroupmanagebll.GetList().Where(a => a.CreateUserId == user.UserId).ToList();
            var treeList = new List<TreeEntity>();
            foreach (UserGroupManageEntity item in data)
            {
                TreeEntity tree = new TreeEntity();
                tree.id = item.Id;
                tree.text = item.UserGroupName;
                tree.value = item.Id;
                tree.isexpand = false;
                tree.parentId = "0";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson("0"));

        }
        /// <summary>
        /// ѡ���û�ҳ��ʹ�ã����ж�Ȩ�ޣ�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetUserListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "USERID";
            pagination.p_fields = "REALNAME,MOBILE,OrganizeName,ORGANIZEID,DEPTNAME,DEPARTMENTID,DEPARTMENTCODE,DUTYNAME,POSTNAME,ROLENAME,ROLEID,MANAGER,ENABLEDMARK,ENCODE,ACCOUNT,NICKNAME,HEADICON,GENDER,EMAIL,OrganizeCode";
            pagination.p_tablename = "V_USERINFO t";
            pagination.conditionJson = "Account!='System'";
            var watch = CommonHelper.TimerStart();
            var data = usergroupmanagebll.GetPageList(pagination, queryJson);
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
            usergroupmanagebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, UserGroupManageEntity entity)
        {
            usergroupmanagebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
