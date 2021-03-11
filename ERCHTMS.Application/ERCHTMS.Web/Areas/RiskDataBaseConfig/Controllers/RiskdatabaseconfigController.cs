using ERCHTMS.Entity.RiskDataBaseConfig;
using ERCHTMS.Busines.RiskDataBaseConfig;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System.Linq;

namespace ERCHTMS.Web.Areas.RiskDataBaseConfig.Controllers
{
    /// <summary>
    /// �� ������ȫ���չܿ����ñ�
    /// </summary>
    public class RiskdatabaseconfigController : MvcControllerBase
    {
        private RiskdatabaseconfigBLL riskdatabaseconfigbll = new RiskdatabaseconfigBLL();

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
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LecIndex()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LecForm()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LecCreate()
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
,createdate,risktype,configtype,configtypecode,itemtype,itemtypecode,deptid,deptcode,deptname,createusername";
                pagination.p_tablename = "bis_riskdatabaseconfig";
                if (user.IsSystem)
                {
                    pagination.conditionJson = "1=1";
                }
                else
                {
                    pagination.conditionJson = string.Format("((createuserid='System' and iscommit='1') or (createuserorgcode='{0}' and iscommit='1') or (createuserid='{1}'))", user.OrganizeCode, user.UserId);
                }
                var data = riskdatabaseconfigbll.GetPageList(pagination, queryJson);
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
                var data = riskdatabaseconfigbll.GetList();
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);
            }
           
        }
        /// <summary>
        /// �������ͻ�ȡ������������
        /// </summary>
        /// <param name="RiskType">��������</param>
        /// <param name="ConfigType">��������</param>
        /// <param name="ItemType">����ϸ��</param>
        /// <param name="DataType">������Դ1 ���վ��� 2 ����ѡ������</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetDataByType(string RiskType, string ConfigType, string DataType,string ItemType = "")
        {
            try
            {
                //��ѯ����λ�Ƿ������������,���û���ڲ�ѯϵͳ�Ƿ���������,��û���򷵻ؿ�
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var strWhere = string.Empty;
                if (!string.IsNullOrWhiteSpace(ItemType))
                {
                    strWhere = string.Format(@" and t.ItemType='{0}'",ItemType);
                }
                string sql = string.Format(@"select t.configcontent from bis_riskdatabaseconfig t where t.ConfigType='{0}' 
and t.iscommit='1' and t.datatype='{1}' and t.risktype='{2}' and t.createuserorgcode='{3}'{4}", ConfigType, DataType, RiskType,user.OrganizeCode,strWhere);
                var data = riskdatabaseconfigbll.GetTable(sql);
                if (data.Rows.Count > 0)
                {
                    return ToJsonResult(data);
                }
                else {
                     sql = string.Format(@"select t.configcontent from bis_riskdatabaseconfig t where t.ConfigType='{0}' 
and t.iscommit='1' and t.datatype='{1}' and t.risktype='{2}' and t.createuserorgcode='{3}'{4}", ConfigType, DataType, RiskType, "0", strWhere);
                     data = riskdatabaseconfigbll.GetTable(sql);
                     return ToJsonResult(data);
                }
                //var data = riskdatabaseconfigbll.GetList().Where(x => x.RiskType == RiskType && x.ConfigType == ConfigType && x.IsCommit == "1" && x.DataType == DataType);
                //if (!string.IsNullOrWhiteSpace(ItemType))
                //{
                //    var d = data.Where(x => x.ItemType == ItemType && x.DeptCode == user.OrganizeCode);
                //    if (d.ToList().Count == 0)
                //    {
                //        var d1 = data.Where(x => x.ItemType == ItemType && x.DeptCode == "0");
                //        return ToJsonResult(d1);
                //    }
                //    else {
                //        return ToJsonResult(d);
                //    }
                //}
                //else {
                //    var d2 = data.Where(x => x.DeptCode == user.OrganizeCode);
                //    if (d2.ToList().Count == 0)
                //    {
                //        var d3 = data.Where(x => x.DeptCode == "0");
                //        return ToJsonResult(d3);
                //    }
                //    else {
                //        return ToJsonResult(d2);
                //    }
                //}
            }
            catch (System.Exception ex)
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
                var data = riskdatabaseconfigbll.GetEntity(keyValue);
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);
            }
            
           
        }
        /// <summary>
        /// �Ƿ������ͬ���͵İ�ȫ��������
        /// </summary>
        /// <param name="RiskType">��������</param>
        /// <param name="ConfigType">��������</param>
        /// <param name="ItemType">����ϸ��</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsExitByType(string RiskType, string ConfigType, string ItemType)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var old = riskdatabaseconfigbll.GetList().Where(x => x.DeptCode == user.OrganizeCode && x.RiskType == RiskType && x.ConfigType == ConfigType);
                if (string.IsNullOrWhiteSpace(ItemType))
                {
                    if (old.ToList().Count > 0)
                    {
                        return ToJsonResult(false);
                    }
                }
                else
                {
                    old = old.Where(x => x.ItemType == ItemType).ToList();
                    if (old.ToList().Count > 0)
                    {
                        return ToJsonResult(false);
                    }
                }
                return ToJsonResult(true);
            }
            catch (System.Exception ex)
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
                riskdatabaseconfigbll.RemoveForm(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (System.Exception ex)
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
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, RiskdatabaseconfigEntity entity)
        {
            try
            {
                riskdatabaseconfigbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (System.Exception ex)
            {

                return Error(ex.Message);
            }
          
        }
        #endregion
    }
}
