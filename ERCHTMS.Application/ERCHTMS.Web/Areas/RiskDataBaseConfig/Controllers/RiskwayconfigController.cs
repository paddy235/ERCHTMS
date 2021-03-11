using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.Busines.RiskDataBaseConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using System.Collections.Generic;
using System.Linq;

namespace ERCHTMS.Web.Areas.RiskDataBaseConfig.Controllers
{
    /// <summary>
    /// �� ������ȫ���չܿ�ȡֵ���ñ�
    /// </summary>
    public class RiskwayconfigController : MvcControllerBase
    {
        private RiskwayconfigBLL riskwayconfigbll = new RiskwayconfigBLL();
        private RiskwayconfigdetailBLL riskwayconfigdetailbll = new RiskwayconfigdetailBLL();

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
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateForm()
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
                pagination.p_kid = "id";
                pagination.p_fields = @"createuserid,createuserdeptcode,createuserorgcode,iscommit
,createdate,risktype,waytype,waytypecode,deptid,deptcode,deptname,createusername";
                pagination.p_tablename = "bis_riskwayconfig";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("((createuserid='System' and iscommit='1') or (createuserorgcode='{0}' and iscommit='1') or (createuserid='{1}'))", user.OrganizeCode,user.UserId);
                }
                var data = riskwayconfigbll.GetPageList(pagination, queryJson);
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
            try
            {
                var data = riskwayconfigbll.GetList(queryJson);
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        /// <summary>
        /// �������ͻ�ȡ����
        /// </summary>
        /// <param RiskType="��������"></param>
        /// <param WayType="ȡֵ����"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataByType(string RiskType, string WayType)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = riskwayconfigbll.GetList("").Where(x => x.RiskType == RiskType && x.WayType == WayType&&x.IsCommit=="1").ToList();
                if (data.Count > 0)
                {
                    var returnData = data.Where(x => x.DeptCode == user.OrganizeCode).ToList();
                    if (returnData.Count > 0)
                    {
                        for (int i = 0; i < returnData.Count; i++)
                        {
                            returnData[i].DetaileList = riskwayconfigdetailbll.GetList("").Where(x => x.WayConfigId == returnData[i].ID).ToList();
                        }
                        return ToJsonResult(returnData);
                    }
                    else
                    {
                        var rData = data.Where(x => x.DeptCode == "0").ToList();
                        for (int i = 0; i < rData.Count; i++)
                        {
                            rData[i].DetaileList = riskwayconfigdetailbll.GetList("").Where(x => x.WayConfigId == rData[i].ID).ToList();
                        }
                        return ToJsonResult(rData);
                    }
                }
                else
                {
                    return ToJsonResult(data);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
          
        }
        /// <summary>
        /// �Ƿ������ͬ��������
        /// </summary>
        /// <param RiskType="��������"></param>
        /// <param WayType="ȡֵ����"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsExistDataByType(string RiskType, string WayType) {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var data = riskwayconfigbll.GetList("").Where(x => x.RiskType == RiskType && x.WayType == WayType && x.DeptCode == user.OrganizeCode).ToList();
                if (data.Count > 0)
                {
                    return ToJsonResult(false);
                }
                else
                {
                    return ToJsonResult(true);
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
          
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            try
            {
                var data = riskwayconfigbll.GetEntity(keyValue);
                data.DetaileList = riskwayconfigdetailbll.GetList("").Where(x => x.WayConfigId == data.ID).ToList();
                return ToJsonResult(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
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
            try
            {
                riskwayconfigbll.RemoveForm(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
          
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
        public ActionResult SaveForm(string keyValue, RiskwayconfigEntity entity, string wayconfigArray)
        {
            try
            {
                List<RiskwayconfigdetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RiskwayconfigdetailEntity>>(wayconfigArray);
                riskwayconfigbll.SaveForm(keyValue, entity, list);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        #endregion
    }
}
