using ERCHTMS.Entity.NosaManage;
using ERCHTMS.Busines.NosaManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Busines.BaseManage;
using System;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.JPush;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.NosaManage.Controllers
{
    /// <summary>
    /// �� ������������
    /// </summary>
    public class NosaworksController : MvcControllerBase
    {
        private NosaworksBLL nosaworksbll = new NosaworksBLL();
        private NosaworkitemBLL nosaworkitembll = new NosaworkitemBLL();
        private NosaworkresultBLL nosaworkresultbll = new NosaworkresultBLL();

        #region ��ͼ����
        /// <summary>
        /// �ϴ��ɹ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UploadResult()
        {
            return View();
        }
        /// <summary>
        /// �ϴ��б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UploadIndex()
        {
            return View();
        }
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
        /// ����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
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
            var data = nosaworksbll.GetList(pagination, queryJson);
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
        public ActionResult GetFormJson(string keyValue)
        {
            var data = nosaworksbll.GetEntity(keyValue);
            //����ֵ
            var josnData = new
            {
                data
            };

            return Content(josnData.ToJson());
        }
        [HttpGet]
        public ActionResult GetDetailJson(string keyValue)
        {
            var data = nosaworksbll.GetEntity(keyValue);
            var resultInfo = nosaworkresultbll.GetList(string.Format(" and workid='{0}' order by createdate asc", keyValue));
            var itemInfo = nosaworkitembll.GetList(string.Format(" and workid='{0}' order by createdate asc", keyValue));
            //����ֵ
            var josnData = new
            {
                data,
                resultInfo,
                itemInfo
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
            nosaworksbll.RemoveForm(keyValue);
            new NosaworkresultBLL().RemoveByWorkId(keyValue);
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
        public ActionResult SaveForm(string keyValue, NosaworksEntity entity)
        {
            entity.DutyUserHtml = entity.DutyUserName;
            entity.DutyDepartHtml = entity.DutyDepartName;
            nosaworksbll.SaveForm(keyValue, entity);
            SaveWorkItem(entity);
            return Success("�����ɹ���");
        }
        private void SaveWorkItem(NosaworksEntity entity)
        {
            nosaworkitembll.RemoveForm(entity.ID);
            UserBLL userbll = new UserBLL();
            var listUserId = entity.DutyUserId.Split(new char[] { ',' });
            foreach(var userId in listUserId)
            {
                var user = userbll.GetUserInfoEntity(userId);
                var iEntity = new NosaworkitemEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    DutyUserId = userId,
                    DutyUserName = user.RealName,
                    DutyDepartId = user.DepartmentId,
                    DutyDepartName = user.DeptName,
                    WorkId = entity.ID,
                    IsSubmitted = "��",
                    State = "���ϴ�",
                    CheckUserId = entity.EleDutyUserId,
                    CheckUserName = entity.EleDutyUserName
                };
                nosaworkitembll.SaveForm("", iEntity);
            }
        }        
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CopyWork(string keyValue)
        {
            var msg = "���Ƴɹ�";
            var newKeyValue = Guid.NewGuid().ToString();
            try
            {
                var entity = nosaworksbll.GetEntity(keyValue);
                if (entity != null)
                {                    
                    entity.ID = newKeyValue;
                    entity.IsSubmited = "��";
                    entity.SubmitUserId = entity.SubmitUserName = "";
                    entity.Pct = 0;
                    entity.CREATEDATE = entity.CREATEDATE.Value.AddSeconds(1);
                    nosaworksbll.SaveForm(newKeyValue, entity);
                    CopyWorkResult(keyValue, newKeyValue);
                    SaveWorkItem(entity);
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }

            return Success(msg, newKeyValue);
        }
        private void CopyWorkResult(string oldId,string newId)
        {
            var list = nosaworkresultbll.GetList(string.Format(" and workid='{0}' order by createdate asc", oldId));
            foreach(var rEntity in list)
            {
                rEntity.ID = Guid.NewGuid().ToString();
                rEntity.WorkId = newId;
                if (!string.IsNullOrWhiteSpace(rEntity.TemplatePath))
                {
                    string filePath = Server.MapPath(rEntity.TemplatePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        string sufx = System.IO.Path.GetExtension(filePath);
                        string newTemplatePath = string.Format("~/Resource/NosaWorkResult/{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), sufx);
                        string newFilePath = Server.MapPath(newTemplatePath);
                        System.IO.File.Copy(filePath, newFilePath);
                        rEntity.TemplatePath = newTemplatePath;
                    }
                }
                nosaworkresultbll.SaveForm(rEntity.ID, rEntity);
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerMonitor(0, "���ݵ���")]
        public ActionResult Export(string queryJson, string sortname, string sortorder)
        {
            var pagination = new Pagination()
            {
                page = 1,
                rows = 100000,
                sidx = string.IsNullOrWhiteSpace(sortname) ? "createdate" : sortname,
                sord = string.IsNullOrWhiteSpace(sortorder) ? "asc" : sortorder
            };
            var dt = nosaworksbll.GetList(pagination, queryJson);
            string fileUrl = @"\Resource\ExcelTemplate\NOSAԪ�ع����嵥_����ģ��.xlsx";
            AsposeExcelHelper.ExecuteResult(dt, fileUrl, "�����嵥", "NOSԪ�ع����嵥");

            return Success("�����ɹ���");
        }
        /// <summary>
        /// ����Ϣ����δ�ύ�����ɹ��ĸ�����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult WarningDutyUser(string keyValue)
        {
            var msg = "�ѷ��Ͷ���Ϣ����δ�ύ�����ɹ���������";
            try
            {
                var entity = nosaworksbll.GetEntity(keyValue);
                if (entity != null)
                {
                    SendMessage(entity);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Success(msg);
        }
        private void SendMessage(NosaworksEntity entity)
        {
            var dList = entity.DutyUserId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var sList = new string[] {};
            if (!entity.SubmitUserId.IsNullOrWhiteSpace())
                sList = entity.SubmitUserId.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if ( dList.Length >sList.Length)
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var eList = dList.Except(sList);
                var aList = new UserBLL().GetListForCon(x => eList.Contains(x.UserId));
                MessageEntity msg = new MessageEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = string.Join(",", aList.Select(x => x.Account)),
                    UserName = string.Join(",", aList.Select(x => x.RealName)),
                    SendTime = DateTime.Now,
                    SendUser = user.Account,
                    SendUserName = entity.CREATEUSERNAME,
                    Title = "NOSA�����ɹ��ϴ�����",
                    Content = string.Format("����һ�{0}����NOSA�����ɹ�δ�ϴ����뼴ʱ�ϴ���", entity.Name),
                    Category = "����"
                };
                if (new MessageBLL().SaveForm("", msg))
                {
                    JPushApi.PublicMessage(msg);
                }
            }
        }
        #endregion
    }
}
