using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// �� ���������ɹ�
    /// </summary>
    public class NosaworkitemController : MvcControllerBase
    {
        private NosaworksBLL nosaworksbll = new NosaworksBLL();
        private NosaworkresultBLL nosaworkresultbll = new NosaworkresultBLL();
        private NosaworkitemBLL nosaworkitembll = new NosaworkitemBLL();

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
        /// ��˳ɹ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CheckResult()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var data = nosaworkitembll.GetList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string workid,string dutyuserid,string keyValue)
        {
            var workInfo = nosaworksbll.GetEntity(workid);
            var resultInfo = nosaworkresultbll.GetList(string.Format(" and workid='{0}' order by createdate asc", workid));
            var data = nosaworkitembll.GetEntity(keyValue);
            if (data == null)
            {
                var list = nosaworkitembll.GetList(string.Format(" and workid='{0}' and dutyuserid='{1}' ", workid, dutyuserid)).ToList();
                if (list != null && list.Count > 0)
                    data = list[0];
            }
            //����ֵ
            var josnData = new
            {
                workInfo,
                resultInfo,
                data
            };

            return Content(josnData.ToJson());
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
            nosaworkitembll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, NosaworkitemEntity entity)
        {
            nosaworkitembll.SaveForm(keyValue, entity);
            if (entity.State == "ͨ��")
            {
                var wEntity = nosaworksbll.GetEntity(entity.WorkId);
                if (wEntity != null)
                {
                    //������ɽ��ȡ������ˡ����β��ŵ���ʾ��ǩ
                    UpdateWorkEntity(wEntity, entity);
                    nosaworksbll.SaveForm(wEntity.ID, wEntity);
                }
            }
            else if (entity.State == "��ͨ��")
            {
                var wEntity = nosaworksbll.GetEntity(entity.WorkId);
                if (wEntity != null && !string.IsNullOrWhiteSpace(wEntity.SubmitUserId) && !string.IsNullOrWhiteSpace(wEntity.SubmitUserName))
                {
                    wEntity.SubmitUserId = wEntity.SubmitUserId.Replace(entity.DutyUserId + ",", "");
                    wEntity.SubmitUserName = wEntity.SubmitUserName.Replace(entity.DutyUserName + ",", "");
                    //������ɽ��ȡ������ˡ����β��ŵ���ʾ��ǩ
                    UpdateWorkEntity(wEntity, entity);
                    nosaworksbll.SaveForm(wEntity.ID, wEntity);
                }
            }

            return Success("�����ɹ���");
        }
        private void UpdateWorkEntity(NosaworksEntity wEntity,NosaworkitemEntity iEntity)
        {
            if(wEntity!=null && iEntity != null)
            {
                //��ɽ���
                var total = wEntity.DutyUserId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Count();
                var num = nosaworkitembll.GetList(string.Format(" and workid='{0}' and state='ͨ��'", iEntity.WorkId)).Count();
                wEntity.Pct = (decimal)Math.Round((num * 1.0) / total * 100, 2);
                //�����ˡ����β��ű�ǩ                
                string oldName = iEntity.DutyUserName;                
                string newName = string.Format("<span style='color:#00CC99;' title='����ɱ�������'>{0},</span>", oldName);
                string oldDepartName = iEntity.DutyDepartName;                
                string newDepartName = string.Format("<span style='color:#00CC99;' title='����ɱ�������'>{0},</span>", oldDepartName);
                var list = nosaworkitembll.GetList(string.Format(" and workid='{0}' and dutydepartid='{1}'", iEntity.WorkId, iEntity.DutyDepartId)).ToList();
                if (iEntity.State == "ͨ��")
                {
                    Regex regTrm = new Regex(",</span>$");
                    Regex regUser = new Regex(oldName + ",?");
                    Regex regDept = new Regex(oldDepartName + ",?");
                    wEntity.DutyUserHtml = regUser.Replace(wEntity.DutyUserHtml, newName);
                    wEntity.DutyUserHtml = regTrm.Replace(wEntity.DutyUserHtml, "</span>");
                    if (list.Count == list.Count(x => x.State == "ͨ��"))
                    {
                        wEntity.DutyDepartHtml = regDept.Replace(wEntity.DutyDepartHtml, newDepartName);
                        wEntity.DutyDepartHtml = regTrm.Replace(wEntity.DutyDepartHtml, "</span>");
                    }
                }
            }
        }
        /// <summary>
        /// �ϴ������ɹ�
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult UploadForm()
        {
            var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string workId = Request["WorkId"];         
            var list = nosaworkitembll.GetList(string.Format(" and workid='{0}' and dutyuserid='{1}'", workId, user.UserId)).ToList();
            NosaworkitemEntity entity = null;
            if (list.Count > 0)
            {
                entity = list[0];
            }
            if (entity != null)
            {
                entity.IsSubmitted = Request["IsSubmited"];
                entity.State = entity.IsSubmitted == "��" ? "�����" : "���ϴ�";
                entity.UploadDate = DateTime.Now;
                nosaworkitembll.SaveForm(entity.ID, entity);
                if (entity.IsSubmitted == "��")
                {
                    var wEntity = nosaworksbll.GetEntity(entity.WorkId);
                    if (wEntity != null)
                    {
                        wEntity.SubmitUserId += entity.DutyUserId + ",";
                        wEntity.SubmitUserName += entity.DutyUserName + ",";                   
                        nosaworksbll.SaveForm(wEntity.ID, wEntity);
                    }
                }
            }
        
            return Success("�����ɹ���");
        }
        #endregion
    }
}
