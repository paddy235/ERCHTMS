using System.Collections.Generic;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// �� �����Ž���Ȩ�ޱ�
    /// </summary>
    public class HikaccessaurhorityController : MvcControllerBase
    {
        private HikaccessaurhorityBLL hikaccessaurhoritybll = new HikaccessaurhorityBLL();

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
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Personindex()
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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "DEVICENAME,OUTTYPE,AREANAME,AREAID,HIKID,StartTime,EndTime";
            pagination.p_tablename = @"BIS_HIKACCESSAURHORITY";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            pagination.conditionJson = "1=1";
            //}
            //else
            //{




            //    string where = new ERCHTMS.Busines.AuthorizeManage.AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value);
            //    pagination.conditionJson += " and " + where;




            //}

            var data = hikaccessaurhoritybll.GetPageList(pagination, queryJson);

            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = hikaccessaurhoritybll.GetEntity(keyValue);
            return ToJsonResult(data);
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
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var pitem = pdata.GetItemValue("Hikappkey");//������������Կ
            var url = pdata.GetItemValue("HikBaseUrl");//������������ַ
            hikaccessaurhoritybll.RemoveForm(keyValue, pitem,url);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ͨ���û�ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveUserForm(string keyValue)
        {
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var pitem = pdata.GetItemValue("Hikappkey");//������������Կ
            var url = pdata.GetItemValue("HikBaseUrl");//������������ַ
            hikaccessaurhoritybll.RemoveUserForm(keyValue, pitem, url);
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// �Ž���Ȩ���·�
        /// </summary>
        /// <param name="StartTime">��ʼʱ��</param>
        /// <param name="EndTime">����ʱ��</param>
        /// <param name="DeptList">����/��Ա�б�</param>
        /// <param name="AccessList">�Ž����б�</param>
        /// <param name="Type">״̬ 0Ϊ�������� 1λ��Ա����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string StartTime, string EndTime, List<Access> DeptList, List<Access> AccessList, int Type)
        {
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var pitem = pdata.GetItemValue("Hikappkey");//������������Կ
            var url = pdata.GetItemValue("HikBaseUrl");//������������ַ
            hikaccessaurhoritybll.SaveForm(StartTime, EndTime, DeptList, AccessList, Type, pitem, url);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
