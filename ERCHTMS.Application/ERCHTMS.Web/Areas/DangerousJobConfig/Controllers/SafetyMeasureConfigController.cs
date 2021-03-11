using ERCHTMS.Entity.DangerousJobConfig;
using ERCHTMS.Busines.DangerousJobConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using ERCHTMS.Code;
using System;
using System.Linq;

namespace ERCHTMS.Web.Areas.DangerousJobConfig.Controllers
{
    /// <summary>
    /// �� ����Σ����ҵ��ȫ��ʩ����
    /// </summary>
    public class SafetyMeasureConfigController : MvcControllerBase
    {
        private SafetyMeasureConfigBLL safetymeasureconfigbll = new SafetyMeasureConfigBLL();
        private SafetyMeasureDetailBLL safetymeasuredetailbll = new SafetyMeasureDetailBLL();

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
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                pagination.p_kid = "a.id";
                pagination.p_fields = @"a.createuserid,a.createuserdeptcode,a.createuserorgcode
                ,a.createdate,worktype,d.itemname as worktypename,a.configtypename,deptid,deptcode,deptname,a.createusername";
                pagination.p_tablename = "dj_safetymeasureconfig a left join base_dataitemdetail d on a.worktype=d.itemvalue and d.itemid =(select itemid from base_dataitem where itemcode='DangerousJobConfig')";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("a.createuserid='System'  or a.createuserorgcode='{0}' or a.createuserid='{1}'", user.OrganizeCode, user.UserId);
                }
                var data = safetymeasureconfigbll.GetPageListJson(pagination, queryJson);
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
            catch (Exception ex)
            {

                return Error(ex.Message);
            }

        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = safetymeasureconfigbll.GetList(queryJson);
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
            var data = safetymeasureconfigbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ���ü���
        /// </summary>
        /// <param name="WorkType"></param>
        /// <param name="ConfigType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetConfigList(string WorkType, string ConfigType)
        {
            try
            {
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var entity = safetymeasureconfigbll.GetList("").Where(t => t.WorkType == WorkType && t.ConfigType == ConfigType && t.DeptCode == user.OrganizeCode).FirstOrDefault();
                if (entity != null)
                {
                    var data = safetymeasuredetailbll.GetList("").Where(t => t.RecId == entity.Id).OrderBy(t => t.SortNum);
                    return Success("��ȡ�ɹ�", data);
                }
                else
                {
                    return Error("����ϵϵͳ����Ա������Ӧ��!");
                }
               
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
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
            safetymeasureconfigbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="Detail">��������</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, SafetyMeasureConfigEntity entity,string Detail)
        {
            try
            {
                List<SafetyMeasureDetailEntity> list = JsonConvert.DeserializeObject<List<SafetyMeasureDetailEntity>>(Detail);
                safetymeasureconfigbll.SaveForm(keyValue, entity, list);
                return Success("�����ɹ���");
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }
        #endregion
    }
}
