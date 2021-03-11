using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Net;
using ERCHTMS.Busines.SystemManage;
using System.Linq;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� �������������Ա
    /// </summary>
    public class StaffInfoController : MvcControllerBase
    {
        private StaffInfoBLL staffinfobll = new StaffInfoBLL();

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
        /// �ල�����б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaskIndex()
        {
            return View();
        }
        /// <summary>
        /// �ල����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaskForm()
        {
            return View();
        }
        /// <summary>
        /// �ļ�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ShowFiles()
        {
            return View();
        }

        /// <summary>
        /// ���ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckForm()
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
        public ActionResult GetStaffSpecToJson(string queryJson)
        {
            var data = staffinfobll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�ල�����б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetTaskTableToJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                var data = staffinfobll.GetDataTable(pagination, queryJson);
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
            catch (Exception ex)
            {
                return Error(ex.Message);
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
            var data = staffinfobll.GetEntity(keyValue);
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
            staffinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, StaffInfoEntity entity)
        {
            staffinfobll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        public ActionResult TestTask()
        {
            try
            {
                new StaffInfoBLL().SendTaskInfo();
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
