using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� �������ص�װ��ҵ������Ա��
    /// </summary>
    public class LifthoistpersonController : MvcControllerBase
    {
        private LifthoistpersonBLL lifthoistpersonbll = new LifthoistpersonBLL();

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
            var data = lifthoistpersonbll.GetList(queryJson);
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
            var data = lifthoistpersonbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ֤���Ų����ظ�
        /// </summary>
        /// <param name="CertificateNum">֤����</param>
        /// <param name="keyValue">����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExistCertificateNum(string CertificateNum, string keyValue)
        {
            try
            {
                bool IsOk = lifthoistpersonbll.ExistCertificateNum(CertificateNum, keyValue);
                return Content(IsOk.ToString());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// ��ȡ���ص�װ��Ա��Ϣ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="workId">���ص�װ��ҵ����ID</param>
        /// <returns></returns>
        public ActionResult GetPersonListJson(Pagination pagination, string workId)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "a.ID";
                pagination.p_fields = "a.recid,a.PERSONTYPE,a.personname,a.personid,a.CERTIFICATENUM,filelist.filenum,a.belongdeptname";
                pagination.p_tablename = @" BIS_LIFTHOISTPERSON a left join (select id,count(fileid) as filenum from BIS_LIFTHOISTPERSON person left join base_fileinfo fileinfo on person.id=fileinfo.recid group by person.id) filelist on a.id=filelist.id ";
                pagination.conditionJson = string.Format("a.recid='{0}'", workId);
                pagination.sidx = "a.createdate";
                pagination.sord = "desc";
                var data = lifthoistpersonbll.GetPageList(pagination, "");
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
                throw ex;
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
        public ActionResult RemoveForm(string keyValue)
        {
            lifthoistpersonbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveForm(string keyValue, LifthoistpersonEntity entity)
        {
            lifthoistpersonbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
