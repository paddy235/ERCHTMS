using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using ERCHTMS.Entity.LllegalStandard;
using ERCHTMS.Busines.LllegalStandard;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Code;
using ERCHTMS.Busines.SystemManage;
using System.Web;
using BSFramework.Util.Offices;
using System.Data;
using System.Collections;
using ERCHTMS.Entity.SystemManage.ViewModel;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.ManyPowerCheck.Controllers
{
    /// <summary>
    /// �� ����Υ�±�׼��
    /// </summary>
    public class ManyPowerCheckController : MvcControllerBase
    {
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();

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
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            var queryParam = queryJson.ToJObject();
            Operator opertator = new OperatorProvider().Current();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"a.autoid,a.createdate,a.createuserid,a.createuserdeptcode,a.createuserorgcode,a.modifydate,a.modifyuserid,a.belongmodule,a.belongmodulecode,
             a.moduleno,a.modulename,a.flowname,a.checkdeptid,a.checkdeptcode,a.checkdeptname,a.checkroleid,a.checkrolename,a.remark,b.fullname orginezename,a.serialnum";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"bis_manypowercheck a left join base_department b on a.createuserorgcode = b.encode";
            pagination.conditionJson = " 1=1";
            string authWhere = new AuthorizeBLL().GetModuleDataAuthority(ERCHTMS.Code.OperatorProvider.Provider.Current(), HttpContext.Request.Cookies["currentmoduleId"].Value, "CREATEUSERDEPTCODE", "CREATEUSERORGCODE");
            if (!string.IsNullOrEmpty(authWhere))
            {//����Ȩ��,��ϵͳ����Ա��ӵ����ݡ�
                pagination.conditionJson += " and (" + authWhere + " or a.CREATEUSERORGCODE='00')";
            }
            else
            {
                pagination.conditionJson += " and a.CREATEUSERORGCODE='" + opertator.OrganizeCode + "'";
            }
            //ģ������
            if (!queryParam["modulename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and  a.moduleno = '{0}' ", queryParam["modulename"].ToString());
            }
            //��˲�������
            if (!queryParam["checkdeptname"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and a.checkdeptname like '%{0}%'", queryParam["checkdeptname"].ToString());
            }
            //��ɫ���� 
            if (!queryParam["checkrolename"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and a.checkrolename like '%{0}%'", queryParam["checkrolename"].ToString());
            }
            //����ģ��
            if (!queryParam["belongmodule"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and a.belongmodulecode = '{0}'", queryParam["belongmodule"].ToString());
            }
            
            var data = manyPowerCheckbll.GetManyPowerCheckEntityPage(pagination, queryJson);
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
            var data = manyPowerCheckbll.GetEntity(keyValue);
            //����ֵ
            var josnData = new
            {
                data = data
            };

            return Content(josnData.ToJson());
        }

        /// <summary>
        /// �ж��Ƿ���������� 
        /// </summary>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult HasConfiguration(string modulename)
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var data = manyPowerCheckbll.GetList(user.OrganizeCode, modulename);

                if (data.Count > 0)
                {
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }
            catch (Exception ex)
            {
                return Content("false");
            }
            
        }
        /// <summary>
        /// �ж��Ƿ����ô���ָ���������Ƶ������ 
        /// </summary>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult HasConfigurationByFlowName(string modulename, string FlowName = "")
        {
            try
            {
                Operator user = OperatorProvider.Provider.Current();
                var data = manyPowerCheckbll.GetList(user.OrganizeCode, modulename);

                if (data.Count > 0)
                {
                    if (data.Where(t => t.FLOWNAME.Contains(FlowName)).Count() > 0)
                    {
                        return Content("true");
                    }
                    else
                    {
                        return Content("false");
                    }
                }
                else
                {
                    return Content("false");
                }
            }
            catch (Exception ex)
            {
                return Content("false");
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
            manyPowerCheckbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, ManyPowerCheckEntity entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.ScriptCurcontent))
            {
                entity.ScriptCurcontent = HttpUtility.UrlDecode(entity.ScriptCurcontent);
            }
            manyPowerCheckbll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

    }
}
