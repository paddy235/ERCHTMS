using ERCHTMS.Entity.KbsDeviceManage;
using ERCHTMS.Busines.KbsDeviceManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.CarManage;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.KbsDeviceManage.Controllers
{
    /// <summary>
    /// �� ����Ԥ�����͹���
    /// </summary>
    public class EarlywarningmanageController : MvcControllerBase
    {
        private EarlywarningmanageBLL earlywarningmanagebll = new EarlywarningmanageBLL();
        private SafeworkcontrolBLL safeworkcontrolbll = new SafeworkcontrolBLL();

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
            var data = earlywarningmanagebll.GetList(queryJson);
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
            var data = earlywarningmanagebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "d.ID";
            pagination.p_fields = "d.condition,d.indexNom,d.IndexUnit,d.WarningResult,d.isEnable  ";
            pagination.p_tablename = @" bis_EarlyWarningManage d  ";
            pagination.conditionJson = " 1=1 ";

            var queryParam = queryJson.ToJObject();
            if (!queryParam["keyword"].IsEmpty())
            {//�ؼ��ֲ�ѯ
                string keyword = queryParam["keyword"].ToString();
                pagination.conditionJson += string.Format(" and d.condition like '%{0}%'", keyword);
            }
            var data = safeworkcontrolbll.GetPageList(pagination, queryJson);
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
            earlywarningmanagebll.RemoveForm(keyValue);
            //����Ϣͬ������̨���������
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            SendData sd = new SendData();
            sd.DataName = "DelEarlywarningmanageEntity";
            sd.EntityString = keyValue;
            rh.SendMessage(JsonConvert.SerializeObject(sd));
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
        public ActionResult SaveForm(string keyValue, EarlywarningmanageEntity entity)
        {
            earlywarningmanagebll.SaveForm(keyValue, entity);
            //����Ϣͬ������̨���������
            RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
            SendData sd = new SendData();
            if(string.IsNullOrEmpty(keyValue)){ sd.DataName = "AddEarlywarningmanageEntity";}
            else { sd.DataName = "UpdateEarlywarningmanageEntity"; };
            sd.EntityString = JsonConvert.SerializeObject(entity);
            rh.SendMessage(JsonConvert.SerializeObject(sd));
            return Success("�����ɹ���");
        }
        /// <summary>
        /// Ԥ�������Ƿ����ý���
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectCheckEnabled(string keyValue,bool status)
        {
            var data = earlywarningmanagebll.GetEntity(keyValue);
            if (data != null)
            {
                if (status)
                {
                    data.Isenable = 1;
                }
                else
                {
                    data.Isenable = 0;
                }
                earlywarningmanagebll.SaveForm(keyValue, data);

                //����Ϣͬ������̨���������
                RabbitMQHelper rh = RabbitMQHelper.CreateInstance();
                SendData sd = new SendData();
                sd.DataName = "UpdateEarlywarningmanageEntity";
                sd.EntityString = JsonConvert.SerializeObject(data);
                rh.SendMessage(JsonConvert.SerializeObject(sd));
            }
            return Success("�����ɹ���");
        }
        
        #endregion
    }
}
