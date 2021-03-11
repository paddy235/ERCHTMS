using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using System.Linq;
using System;
using BSFramework.Util.Extension;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// �� �����ļ�ͨ������
    /// </summary>
    public class FileInformController : MvcControllerBase
    {
        private FileInformBLL FileInformbll = new FileInformBLL();
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //�Ƿ���Ҫ���
            ViewBag.IsCheck = 0;
            //��ѯ�Ƿ���������
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "��ȫ��̬";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            if (powerList.Count > 0)
                ViewBag.IsCheck = powerList.Count;
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            //�Ƿ���Ҫ���
            ViewBag.IsCheck = 0;
            //��ѯ�Ƿ���������
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "��ȫ��̬";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            if (powerList.Count > 0)
                ViewBag.IsCheck = powerList.Count;
            return View();
        }
        /// <summary>
        /// �������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApproveForm()
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
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,Title,Publisher,IsSend,createusername,createdate,ReleaseTime";
            pagination.p_tablename = "BIS_FileInform t";
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            
            var watch = CommonHelper.TimerStart();
            var data = FileInformbll.GetPageList(pagination, queryJson);
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
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = FileInformbll.GetList(queryJson);
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
            var data = FileInformbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ����չʾ����
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetScrnEntity()
        {
            try
            {
                Operator curUser = OperatorProvider.Provider.Current();
                var list = FileInformbll.GetList("").Where(t => t.CreateUserOrgCode == curUser.OrganizeCode && t.IsSend == "0").OrderBy(t => t.CreateDate).Take(2).ToList();
                List<object> data = new List<object>();
                foreach (var item in list)
                {
                    IList<Photo> pList = new List<Photo>(); //����
                    DataTable file = fileInfoBLL.GetFiles(item.Id);
                    foreach (DataRow dr in file.Rows)
                    {
                        Photo p = new Photo();
                        p.fileid = dr["fileid"].ToString();
                        p.filename = dr["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + dr["filepath"].ToString().Substring(1);
                        pList.Add(p);
                    }
                    data.Add(new { id = item.Id, title = item.Title, file = pList });
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
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
            FileInformbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, FileInformEntity entity)
        {
            FileInformbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion
    }
}
