using System;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// �� �����豸���߼�¼
    /// </summary>
    public class OfflinedeviceController : MvcControllerBase
    {
        private OfflinedeviceBLL offlinedevicebll = new OfflinedeviceBLL();

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
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = offlinedevicebll.GetList(queryJson);
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
            var data = offlinedevicebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�����豸ͳ��ͼ
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetOffinedeviceImage(int type)
        {
            List<object> dic = new List<object>();
            string[] Month = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };//���趨�·�
            DataTable dt = offlinedevicebll.GetTable(type);
            List<int> Num = new List<int>();
            for (int i = 0; i < Month.Length; i++)
            {
                int count = 0;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dt.Rows[j]["offtime"].ToString() == DateTime.Now.Year + "-" + Month[i])
                    {
                        count = Convert.ToInt32(dt.Rows[j]["offcount"]);
                        break;
                    }
                }
                Num.Add(count);
            }
            return JsonConvert.SerializeObject(new { x = Num });
        }

        /// <summary>
        /// ��ѯ�����豸ǰ����
        /// </summary>
        /// <param name="type">�豸���� 0��ǩ 1��վ 2�Ž� 3����ͷ</param>
        /// <param name="Time">1���� 2����</param>
        /// <param name="topNum">ǰ����</param>
        /// <returns></returns>
        [HttpGet]
        public string GetOffTop(int type, int Time, int topNum)
        {
            DataTable dt = offlinedevicebll.GetOffTop(type,Time,topNum);
            return JsonConvert.SerializeObject(dt);
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
            offlinedevicebll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OfflinedeviceEntity entity)
        {
            offlinedevicebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
