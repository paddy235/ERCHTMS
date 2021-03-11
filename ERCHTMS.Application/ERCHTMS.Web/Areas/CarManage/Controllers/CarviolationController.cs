using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using Newtonsoft.Json;
using System.Net.Http;
using System;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// �� ����Υ����Ϣ��
    /// </summary>
    public class CarviolationController : MvcControllerBase
    {
        private CarviolationBLL carviolationbll = new CarviolationBLL();

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
        /// ʵʱԤ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViolationRecord()
        {
            return View();
        }

        /// <summary>
        /// Υ�洦��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HandleForm()
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
            pagination.p_fields = "createdate,violationmsg,processmeasure,isprocess";
            pagination.p_tablename = "BIS_CARVIOLATION";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            pagination.conditionJson = "1=1";

            var data = carviolationbll.GetPageList(pagination, queryJson);

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
        /// ʵʱԤ���б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetViolationdListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "info.createdate,info.violationmsg,info.processmeasure,info.isprocess,info.modifydate,info.violationtype,cardno,hikpicsvr,vehiclepicurl";
            pagination.p_tablename = "bis_carviolation info ";
            pagination.conditionJson = " 1=1";
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var data = carviolationbll.GetPageList(pagination, queryJson);

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
            var data = carviolationbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡԤ���������ݣ�δ��������ݣ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(LoginMode.Ignore)]
        public ActionResult GetIndexWaring()
        {
            List<CarviolationEntity> data = carviolationbll.GetIndexWaring();
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡԤ������ͳ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerLogin(LoginMode.Ignore)]
        public ActionResult GetIndexWaringCount()
        {
            object data = carviolationbll.GetIndexWaringCount();
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ��������ץ�ĵ�ͼƬ
        /// </summary>
        /// <param name="hikpicsvr">����ͼƬ������Ψһ����</param>
        /// <param name="vehiclepicurl">����ץ��ͼƬ��ַ</param>
        /// <returns></returns>
        [HttpPost]
        [HandlerLogin(LoginMode.Ignore)]
        public ActionResult GetHikPicUrl(string hikpicsvr, string vehiclepicurl)
        {
            try
            {
                DataItemDetailBLL pdata = new DataItemDetailBLL();
                var pitem = pdata.GetItemValue("Hikappkey");//������������Կ
                var hikBaseUrl = pdata.GetItemValue("HikBaseUrl");//������������ַ
                string url = "/artemis/api/video/v1/events/picture";
                string key = string.Empty;// "21049470";
                string sign = string.Empty;// "4gZkNoh3W92X6C66Rb6X";
                if (!string.IsNullOrEmpty(pitem))
                {
                    key = pitem.Split('|')[0];
                    sign = pitem.Split('|')[1];
                }
                var model = new
                {
                    svrIndexCode = hikpicsvr,
                    picUri = vehiclepicurl
                };
                var msg = SocketHelper.LoadCameraList(model, hikBaseUrl, url, key, sign);
                SocketHelper.SetLog("OverSpeedPicOK", "������Ϣ ��ȡ����Υ��ץ��ͼƬ   ���� hikpicsvr:" + hikpicsvr + "   vehiclepicurl��" + vehiclepicurl, msg);
                return Json(msg);
            }
            catch (System.Exception ex)
            {
                SocketHelper.SetLog("OverSpeedPicEoor", "��ȡ����Υ��ץ��ͼƬ����   ���� hikpicsvr:" + hikpicsvr + "   vehiclepicurl��" + vehiclepicurl, JsonConvert.SerializeObject(ex));
                return Json(new { code = "-1", msg = ex.Message });
            }
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
            carviolationbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, CarviolationEntity entity)
        {
            carviolationbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        /// <summary>
        /// Υ�洦��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveHandleForm(string keyValue, string Content)
        {
            var data = carviolationbll.GetEntity(keyValue);
            if (data != null)
            {
                data.ProcessMeasure = Content;
                data.IsProcess = 1;
                carviolationbll.SaveForm(keyValue, data);
            }
            return Success("�����ɹ���");
        }

        #endregion
    }
}
