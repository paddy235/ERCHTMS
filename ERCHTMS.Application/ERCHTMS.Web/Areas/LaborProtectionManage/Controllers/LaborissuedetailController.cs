using System.Web;
using ERCHTMS.Entity.LaborProtectionManage;
using ERCHTMS.Busines.LaborProtectionManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.LaborProtectionManage.Controllers
{
    /// <summary>
    /// �� �����Ͷ��������ű�����
    /// </summary>
    public class LaborissuedetailController : MvcControllerBase
    {
        private LaborissuedetailBLL laborissuedetailbll = new LaborissuedetailBLL();
        private LaborinfoBLL laborinfobll = new LaborinfoBLL();
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
        public ActionResult SeeIndex()
        {
            return View();
        }
        #endregion

        #region ��ȡ����

        public ActionResult GetIssueList(string keyValue,string InfoId)
        {
            //�������������ʱ
            if (keyValue==null||keyValue == "")
            {
                string newinfoid = "";
                string[] ids=InfoId.Split(',');
                foreach (string id in ids)
                {
                    if (newinfoid == "")
                    {
                        newinfoid = "'" + id + "'";
                    }
                    else
                    {
                        newinfoid += ",'" + id + "'";
                    }
                }
                var data = laborinfobll.Getplff(newinfoid);
                return ToJsonResult(data);
            }
            else
            {
                var data = laborissuedetailbll.GetList(keyValue);
                return ToJsonResult(data);
            }
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = laborissuedetailbll.GetList(queryJson);
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
            var data = laborissuedetailbll.GetEntity(keyValue);
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
            laborissuedetailbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }

        /// <summary>
        /// ������������������޸ģ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveListForm(string json)
        {
            json = HttpUtility.UrlDecode(json);
            //���û��������ֱ�ӷ���
            if (json == "")
            {
                return Error("�޶�Ӧ���ݣ�������Ч");
            }
            laborissuedetailbll.SaveListForm( json);
            return Success("�����ɹ���");
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
        public ActionResult SaveForm(string keyValue, LaborissuedetailEntity entity, string json, string InfoId)
        {
            json = HttpUtility.UrlDecode(json);
            if (json == "")
            {
                return Error("�޶�Ӧ���ݣ�������Ч");
            }
            laborissuedetailbll.SaveForm(keyValue, entity, json, InfoId);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
