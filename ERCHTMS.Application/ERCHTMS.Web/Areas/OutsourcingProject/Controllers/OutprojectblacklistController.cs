using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ���������λ��������
    /// </summary>
    public class OutprojectblacklistController : MvcControllerBase
    {
        private OutprojectblacklistBLL outprojectblacklistbll = new OutprojectblacklistBLL();

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
            var data = outprojectblacklistbll.GetList(queryJson);
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
            var data = outprojectblacklistbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetPageBlackListJson(Pagination pagination, string queryJson) {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "bk.id";
                pagination.p_fields = @" bk.createdate,bk.outprojectid,
                                           t.outsourcingname,
                                           t.legalrep,
                                           bk.inblacklisttime,
                                            bk.inblacklistcause,
                                           bk.outblacklisttime,
                                           bk.outblacklistcause";
                pagination.p_tablename = @" epg_outprojectblacklist bk
                                            left join epg_outsourcingproject t on t.outprojectid = bk.outprojectid 
                                            left join base_department b on b.departmentid=bk.outprojectid ";
                pagination.sidx = "b.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
                {
                    pagination.conditionJson = string.Format("  1=1 ");
                }
                else if (currUser.RoleName.Contains("�а��̼��û�"))
                {
                    pagination.conditionJson = string.Format("  t.OUTPROJECTID ='{0}'", currUser.DeptId);
                }
                else
                {
                    pagination.conditionJson = string.Format(" t.outprojectid in(select distinct(t.outprojectid) from EPG_OUTSOURINGENGINEER t where t.engineerletdeptid='{0}')", currUser.DeptId);
                }
                var data = outprojectblacklistbll.GetPageBlackListJson(pagination, queryJson);

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

        [HttpGet]
        public ActionResult ToIndexData()
        {
            Operator user = OperatorProvider.Provider.Current();
            var list = outprojectblacklistbll.ToIndexData(user);
            return ToJsonResult(list);
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
            outprojectblacklistbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OutprojectblacklistEntity entity)
        {
            outprojectblacklistbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
