using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.ComprehensiveManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.ComprehensiveManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System;

namespace ERCHTMS.Web.Areas.ComprehensiveManage.Controllers
{
    /// <summary>
    /// �� ������Ϣ���ͱ�
    /// </summary>
    public class InfoSubmitController : MvcControllerBase
    {
        private UserBLL userbll = new UserBLL(); //�û���������
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private InfoSubmitBLL infoSubmitbll = new InfoSubmitBLL();

        #region ��ͼ����
        [HttpGet]
        public ActionResult Detail()
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
            ViewBag.ehsDepartCode = "";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
            
            //��������
            //ViewBag.Num = infoSubmitbll.CountIndex("1");

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
        [HttpGet]
        public ActionResult ReferForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ReferDetail()
        {
            return View();
        }
        [HttpGet]
        public ActionResult InfoFiles()
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
            var watch = CommonHelper.TimerStart();            
            var data = infoSubmitbll.GetList(pagination, queryJson);
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
            var data = infoSubmitbll.GetEntity(keyValue);                  
            //����ֵ
            var josnData = new
            {
                data = data
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
            var infoDetailBll = new InfoSubmitDetailsBLL();

            infoSubmitbll.RemoveForm(keyValue);//ɾ������
            var list = infoDetailBll.GetList(string.Format(" and infoid='{0}'",keyValue));//ɾ������
            foreach(var detail in list)
            {
                DeleteFiles(detail.ID);                
            }
            infoDetailBll.RemoveFormByInfoId(keyValue);

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
        public ActionResult SaveForm(string keyValue, InfoSubmitEntity entity)
        {
            //���������Ϣ
            entity.IsSubmit = "��";
            entity.Pct = 0;
            entity.Remnum = entity.SubmitUserId.Split(new char[] { ',' }).Count();
            entity.RemUserName = entity.SubmitUserName;
            entity.RemDepartName = entity.SubmitDepartName;
            infoSubmitbll.SaveForm(keyValue, entity);

            return Success("�����ɹ���");
        }
        /// <summary>
        /// �ύ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, InfoSubmitEntity entity)
        {
            //���������Ϣ
            entity.IsSubmit = "��";
            entity.Pct = 0;
            entity.Remnum = entity.SubmitUserId.Split(new char[] { ',' }).Count();
            entity.RemUserName = entity.SubmitUserName;
            entity.RemDepartName = entity.SubmitDepartName;
            infoSubmitbll.SaveForm(keyValue, entity);

            return Success("�����ɹ���");
        }       
        #endregion
    }
}
