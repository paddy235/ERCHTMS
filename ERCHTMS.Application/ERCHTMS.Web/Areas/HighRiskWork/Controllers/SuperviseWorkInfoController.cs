using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.Busines.HighRiskWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using ERCHTMS.Code;
using ERCHTMS.Entity.SystemManage.ViewModel;
using ERCHTMS.Cache;
using System.Linq;
using System;

namespace ERCHTMS.Web.Areas.HighRiskWork.Controllers
{
    /// <summary>
    /// �� ������վ�ල��ҵ��Ϣ
    /// </summary>
    public class SuperviseWorkInfoController : MvcControllerBase
    {
        private SuperviseWorkInfoBLL superviseworkinfobll = new SuperviseWorkInfoBLL();
        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private SafetychangeBLL safetychangebll = new SafetychangeBLL();
        private ScaffoldBLL scaffoldbll = new ScaffoldBLL();
        private DataItemCache dataItemCache = new DataItemCache();

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
        /// ѡ��߷�����ʩ�䶯ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectChange()
        {
            return View();
        }

        /// <summary>
        /// ѡ��߷��ս��ּ�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectScaffold()
        {
            return View();
        }

        /// <summary>
        /// ѡ��߷���ͨ����ҵҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectCommon()
        {
            return View();
        }

        /// <summary>
        /// ѡ����ҵ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelectWorkType()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="taskshareid">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetWorkSpecToJson(string taskshareid)
        {
            var data = superviseworkinfobll.GetList(string.Format(" and taskshareid='{0}'",taskshareid));
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="taskshareid">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetWorkByWidToJson(string workid)
        {
            var data = superviseworkinfobll.GetList(string.Format(" and id='{0}'", workid));
            return ToJsonResult(data);
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="taskshareid">��ѯ����</param>
        /// <param name="teamid">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetWorkToJson(string taskshareid, string teamid)
        {
            var data = superviseworkinfobll.GetList(taskshareid,teamid);
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
            var data = superviseworkinfobll.GetEntity(keyValue);
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
            superviseworkinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, SuperviseWorkInfoEntity entity)
        {
            superviseworkinfobll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }


        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        [HttpPost]
        public string SaveWorkForm(string taskshareid)
        {
            string jsondata = Request.Form["jsondata"].ToString();
            List<SuperviseWorkInfoEntity> list = JsonConvert.DeserializeObject<List<SuperviseWorkInfoEntity>>(jsondata);
            superviseworkinfobll.RemoveWorkByTaskShareId(taskshareid);
            for (int i = 0; i < list.Count; i++)
            {
                superviseworkinfobll.SaveForm("", list[i]);
            }
            return "1";
        }
        #endregion


        #region �߷�����ҵѡ��ҳ��

        #region ͨ��
        /// <summary>
        /// ��ȡ�б�(��ȡ������ҵ����ҵ��)
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetSelectCommonWorkJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "Id as workid";
                pagination.p_fields = "case when workdepttype=0 then '��λ�ڲ�' when workdepttype=1 then '�����λ' end workdepttypename,workdepttype,workdeptid,workdeptname,workdeptcode,applynumber,CreateDate,workplace,workcontent,workstarttime,workendtime,applyusername,EngineeringName,EngineeringId,workareacode,workareaname,workusernames";
                pagination.p_tablename = " bis_highriskcommonapply a";
                pagination.conditionJson = "applystate='5'";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("����") || user.RoleName.Contains("��˾"))
                    {
                        pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format("  and ((WorkDeptCode in(select encode from base_department where encode like '{0}%'))  or (engineeringid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                    }
                    pagination.conditionJson += " and ((RealityWorkStartTime is null) or (RealityWorkStartTime is not null and RealityWorkEndTime is null))";
                }
                var data = highriskcommonapplybll.GetPageDataTable(pagination, queryJson);
                var watch = CommonHelper.TimerStart();
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
        #endregion

        #region ��ȫ��ʩ�䶯
        /// <summary>
        /// ��ȡ��ʩ�䶯�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetSelectChangeWorkJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "Id as workid";
                pagination.p_fields = "workunit,workunitid,workunitcode,case when workunittype='0'  then '��λ�ڲ�'  when  workunittype='1' then '�����λ' end workunittypename,workunittype,changereason,workplace,applychangetime,returntime,projectid,projectname,workarea";
                pagination.p_tablename = " bis_Safetychange t";
                pagination.conditionJson = "iscommit=1 and isapplyover=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("����") || user.RoleName.Contains("��˾"))
                    {
                        pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format("  and ((workunitcode in(select encode from base_department where encode like '{0}%'))  or (projectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                    }
                    pagination.conditionJson += " and ((t.id not in(select id from bis_safetychange where ((isaccpcommit=0 and isaccepover=0  and  RealityChangeTime is null) or (isaccpcommit=1 and isaccepover=1))))  or (isaccpcommit=0 and isaccepover=0 and  RealityChangeTime is null))";
                }
                var data = safetychangebll.GetPageList(pagination, queryJson);
                var watch = CommonHelper.TimerStart();
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
        #endregion

        #region ���ּ�
        /// <summary>
        /// ��ȡ���ּ��б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetSelectScaffoldWorkJson(Pagination pagination, string queryJson)
        {
            try
            {
                pagination.p_kid = "Id as workid";
                pagination.p_fields = "purpose,dismentlereason,setupcompanyname,setupcompanyid,setupcompanycode,case when setupcompanytype='0' then '��λ�ڲ�'  when  setupcompanytype='1' then '�����λ' end setupcompanytypename,setupcompanytype,setupstartdate,setupenddate,setupaddress,dismentlestartdate,dismentleenddate,outprojectid,outprojectname,WORKAREA,setuppersons,dismentlepersons";
                pagination.p_tablename = " v_scaffoldledger";
                pagination.conditionJson = "1=1";
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("����") || user.RoleName.Contains("��˾"))
                    {
                        pagination.conditionJson += " and createuserorgcode='" + user.OrganizeCode + "'";
                    }
                    else
                    {
                        pagination.conditionJson += string.Format("  and ((setupcompanyid in(select departmentid from base_department where encode like '{0}%'))  or (outprojectid in(select id from epg_outsouringengineer a where a.engineerletdeptid = '{1}')))", user.DeptCode, user.DeptId);
                    }
                }
                var data = scaffoldbll.GetSelectPageList(pagination, queryJson);
                var watch = CommonHelper.TimerStart();
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
        #endregion
        #endregion

        #region ��ѯ��ҵ���

        /// <summary>
        /// ��ѯ��ҵ����� 
        /// </summary>
        /// <param name="checkMode">��ѡ���ѡ��0:��ѡ��1:��ѡ</param>
        /// <param name="mode">��ѯģʽ��0:��ȡ����IDΪIds�����еĲ���(��OrganizeId=Ids)��1:��ȡ����IDΪIds�����в���(��ParentId=Ids)��2:��ȡ���ŵ�parentId��Ids�еĲ���(��ParentId in(Ids))��3:��ȡ����Id��Ids�еĲ���(��DepartmentId in(Ids))</param>
        /// <param name="typeIDs">��ɫIDs</param>
        /// <returns>��������Json</returns>
        [HttpGet]
        public ActionResult GetTypeTreeJson(string typeIDs, int checkMode = 0, int mode = 0)
        {
            var treeList = new List<TreeEntity>();
            IEnumerable<DataItemModel> data = dataItemCache.GetDataItemList("TaskWorkType");
            foreach (DataItemModel item in data)
            {
                TreeEntity tree = new TreeEntity();
                bool hasChildren = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? false : true;
                bool showcheck = data.Count(t => t.ParentId == item.ItemDetailId) == 0 ? true : false;
                tree.id = item.ItemDetailId;
                tree.text = item.ItemName;
                tree.value = item.ItemValue;
                tree.isexpand = true;
                tree.complete = true;
                if (!string.IsNullOrEmpty(typeIDs))
                {
                    var s = typeIDs.Split(',');
                    foreach (var arr in s)
                    {
                        if (arr == item.ItemValue) tree.checkstate = 1;
                    }
                }
                tree.showcheck = showcheck;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson());
        }
        #endregion
    }
}
