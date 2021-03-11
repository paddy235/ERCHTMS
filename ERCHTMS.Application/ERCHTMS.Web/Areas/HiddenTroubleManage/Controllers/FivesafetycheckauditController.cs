using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Code;
using System.Data;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// �� �����嶨��ȫ��� �������ձ�
    /// </summary>
    public class FivesafetycheckauditController : MvcControllerBase
    {
        private FivesafetycheckauditBLL fivesafetycheckauditbll = new FivesafetycheckauditBLL();
        private FivesafetycheckBLL fivesafetycheckbll = new FivesafetycheckBLL();

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
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GeDataTableByIds(string ids)
        {
            var data = fivesafetycheckauditbll.GeDataTableByIds(ids);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = fivesafetycheckauditbll.GetList(queryJson);
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
            var data = fivesafetycheckauditbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = fivesafetycheckauditbll.GetPageListJson(pagination, queryJson);
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
        #endregion

        #region �ύ����
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValueids"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckNumAudit(string keyValueids,string queryJson)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            foreach (string keyValue in queryJson.Split(','))
            {
                FivesafetycheckauditEntity applyentity = fivesafetycheckauditbll.GetEntity(keyValue);

                if (applyentity.ACCEPTREUSLT != "0" && applyentity.ACTIONRESULT == "0" && applyentity.ACCEPTUSERID == curUser.UserId) // �������꣬δ���յģ�������Ϊ��ǰ�û�������ִ���������ղ���
                {
                    applyentity.ACCEPTCONTENT = "";
                    applyentity.ACCEPTREUSLT = "0";

                    fivesafetycheckauditbll.SaveForm(keyValue, applyentity);

                    #region ����������Ϣ
                    string checkid = applyentity.CHECKID;
                    var checkentity = fivesafetycheckbll.GetEntity(checkid);

                    DataTable dt = fivesafetycheckauditbll.GetDataTable("select id from BIS_FIVESAFETYCHECKAUDIT where checkpass  = '1' and (ACCEPTREUSLT is null or  ACCEPTREUSLT = '1') and checkid = '" + checkid + "' ");
                    // ���е���������ɣ�����������ϢΪ�����
                    if (dt.Rows.Count == 0)
                    {
                        checkentity.ISOVER = 3;

                    }
                    fivesafetycheckbll.SaveForm(checkid, checkentity);
                }

                #endregion

                
            }
            return Success("�����ɹ���");

        }


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
            fivesafetycheckauditbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, FivesafetycheckauditEntity entity)
        {
            fivesafetycheckauditbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="istype">0:���� 1������</param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApplyForm(string keyValue,string istype, FivesafetycheckauditEntity entity)
        {
            FivesafetycheckauditEntity applyentity = fivesafetycheckauditbll.GetEntity(keyValue);
            
            if (istype == "0") // 0:���� 1������
            {
                applyentity.BEIZHU = entity.BEIZHU;
                applyentity.ACTUALDATE = entity.ACTUALDATE;
                applyentity.ACTIONRESULT = entity.ACTIONRESULT;
                if (entity.ACTIONRESULT == "0")
                {
                    applyentity.ACCEPTREUSLT = "";
                    applyentity.ACCEPTCONTENT = "";   
                }
            }
            else
            {
                applyentity.ACCEPTCONTENT = entity.ACCEPTCONTENT;
                applyentity.ACCEPTREUSLT = entity.ACCEPTREUSLT;
                if (entity.ACCEPTREUSLT == "1") //��ͬ��
                {
                    applyentity.ACTIONRESULT = "1";
                    applyentity.ACTUALDATE = null;

                    JPushApi.PushMessage(new UserBLL().GetEntity(applyentity.DUTYUSERID).Account, applyentity.DUTYUSERNAME, "WDJC001", applyentity.ID);

                }
            }
            
            fivesafetycheckauditbll.SaveForm(keyValue, applyentity);

            #region ����������Ϣ
            string checkid = applyentity.CHECKID;
            var checkentity = fivesafetycheckbll.GetEntity(checkid);

            if (istype == "0") // 0:���� 1������
            {
                //����ͬ�⣬��Ҫ�ж��Ƿ�ȫ��������
                if (entity.ACTIONRESULT == "0")
                {
                    DataTable dt = fivesafetycheckauditbll.GetDataTable("select id from BIS_FIVESAFETYCHECKAUDIT where checkpass  = '1' and (ACTIONRESULT is null or  ACTIONRESULT = '1') and checkid = '" + checkid + "' ");
                    // ���е���������ɣ�����������ϢΪ������
                    if (dt.Rows.Count == 0)
                    {
                        checkentity.ISOVER = 2;

                    }
                }
            }
            else
            {
                if (entity.ACCEPTREUSLT == "0") //ͬ��
                {
                    DataTable dt = fivesafetycheckauditbll.GetDataTable("select id from BIS_FIVESAFETYCHECKAUDIT where checkpass  = '1' and (ACCEPTREUSLT is null or  ACCEPTREUSLT = '1') and checkid = '" + checkid + "' ");
                    // ���е���������ɣ�����������ϢΪ�����
                    if (dt.Rows.Count == 0)
                    {
                        checkentity.ISOVER = 3;

                    }
                }
                else // ��ͬ�⽫ֱ�Ӹĳ�������
                {
                    checkentity.ISOVER = 1;
                }
            }
            fivesafetycheckbll.SaveForm(checkid, checkentity);

            #endregion


            return Success("�����ɹ���");
        }
        #endregion
    }
}
