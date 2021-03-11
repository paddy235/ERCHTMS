using System.Collections.Generic;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// �� ����Σ�����س�����
    /// </summary>
    public class HazardouscarController : MvcControllerBase
    {
        private HazardouscarBLL hazardouscarbll = new HazardouscarBLL();

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
        public ActionResult ProcessManage()
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
        public ActionResult BackGetListPageJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,createdate,modifyuserid,modifydate,createuserdeptcode,createuserorgcode,outtime,state,carno,dirver,phone,thecompany,hazardousname,Hazardousid,NVL(vnum,0) vnum,num";
            pagination.p_tablename = @" ( select id,createuserid,createdate,modifyuserid,modifydate,createuserdeptcode,createuserorgcode,outtime,state,carno,dirver,phone,thecompany,hazardousname,Hazardousid,vnum,NVL(num,0) num  from bis_hazardouscar  hazardous
            left join (select count(cid) vnum ,cid from BIS_CARVIOLATION  group by cid  )  cv on hazardous.id=cv.cid
            left join (select sum(t.Netwneight) num,d.baseid from WL_CALCULATE d join WL_CALCULATEDetailed t on d.id=t.baseid where d.datatype=5 group by d.baseid) wl on wl.baseid=hazardous.id
            ) v1 ";
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

            var data = hazardouscarbll.GetPageList(pagination, queryJson);

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
        /// ��ȡ��ʻ״̬�б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetStatus()
        {
            List<DataItemDetailEntity> ddlist = new List<DataItemDetailEntity>();
            DataItemDetailEntity d1 = new DataItemDetailEntity();
            d1.ItemName = "����";
            d1.ItemValue = "0";
            DataItemDetailEntity d2 = new DataItemDetailEntity();
            d2.ItemName = "�쳣";
            d2.ItemValue = "1";
            ddlist.Add(d1);
            ddlist.Add(d2);
            return Content(ddlist.ToJson());
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = hazardouscarbll.GetList(queryJson);
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
            var data = hazardouscarbll.GetEntity(keyValue);
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
            hazardouscarbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HazardouscarEntity entity)
        {
            hazardouscarbll.SaveForm(keyValue, entity);
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
        public ActionResult ChangeGps(string keyValue, HazardouscarEntity entity, List<PersongpsEntity> pergps)
        {
            hazardouscarbll.ChangeGps(keyValue, entity, pergps);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
