using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using System;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� ��������������
    /// </summary>
    public class TeamsInfoController : MvcControllerBase
    {
        private TeamsInfoBLL teamsinfobll = new TeamsInfoBLL();
        private SuperviseWorkInfoBLL superviseworkinfobLL = new SuperviseWorkInfoBLL();
        private TaskShareBLL tasksharebll = new TaskShareBLL();
        private TeamsWorkBLL teamsworkbll = new TeamsWorkBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        #region ��ͼ����
        /// <summary>
        /// ѡ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
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
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = teamsinfobll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetTeamSpecToJson(string queryJson)
        {
            var data = teamsinfobll.GetList(queryJson);
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
            teamsinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, TeamsInfoEntity entity)
        {
            teamsinfobll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult FinishTeamTask(string keyValue)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            try
            {

                TeamsInfoEntity u = teamsinfobll.GetEntity(keyValue);
                if (u != null)
                {
                    u.IsAccomplish = "1";
                    teamsinfobll.SaveForm(keyValue, u);
                    string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��" + user.UserName + "����������ɳɹ����û���Ϣ" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + "\r\n");
                }
            }
            catch (Exception ex)
            {
                //д����־�ļ�
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "�������������ʧ�ܣ��û���Ϣ" + Newtonsoft.Json.JsonConvert.SerializeObject(user) + ",�쳣��Ϣ��" + ex.Message + "\r\n");
                return Success("����ʧ�ܣ�������Ϣ��" + ex.Message);
            }
            return Success("�����ɹ�");
        }
    }
}
