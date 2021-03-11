using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// 描 述：安全技术交底
    /// </summary>
    public class TechDisclosureController : MvcControllerBase
    {
        private TechDisclosureBLL techdisclosurebll = new TechDisclosureBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private HistoryTechDisclosureBLL historytechdisclosurebll = new HistoryTechDisclosureBLL();

        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }
        [HttpGet]
        public ActionResult HistoryIndex()
        {
            return View();
        }
        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @" r.id as engineerid,e.fullname,t.ENGINEERNAME,t.DisclosureType,to_char(t.DisclosureDate,'yyyy-MM-dd') as DisclosureDate,disclosurename,disclosuremajordept,d.itemname as disclosuremajor,t.DisclosurePersonNum,t.DisclosurePlace,t.DisclosurePerson,t.ReceivePerson,t.CreateUserId,'' as approveuserids,flowid,issubmit,status ";
            pagination.p_tablename = @"EPG_TechDisclosure t left join EPG_OutSouringEngineer r 
on t.projectid=r.id left join base_department e on r.outprojectid=e.departmentid left join base_dataitemdetail d on t.disclosuremajor=d.itemvalue and d.itemid =(select itemid from base_dataitem where itemcode='BelongMajor')";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            pagination.conditionJson += "(";
            if (role.Contains("省级"))
            {
                pagination.conditionJson += string.Format(@" t.createuserorgcode  in (select encode
                from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '厂级' and d.description is null)", user.NewDeptCode);
            }
            else if (role.Contains("公司级用户") || role.Contains("厂级部门用户"))
            {
                pagination.conditionJson += string.Format(" t.CREATEUSERORGCODE  = '{0}'", user.OrganizeCode);
            }
            else if (role.Contains("承包商级用户"))
            {
                pagination.conditionJson += string.Format(" (e.departmentid = '{0}' or r.supervisorid = '{0}' )", user.DeptId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(user.DeptId);
                while (deptentity.Nature == "班组" || deptentity.Nature == "专业")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }
                pagination.conditionJson += string.Format(" r.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') ", deptentity.EnCode);
                //pagination.conditionJson = string.Format(" r.engineerletdeptid = '{0}'", user.DeptId);
            }
            pagination.conditionJson += " or t.projectid is null)";
            var watch = CommonHelper.TimerStart();
            var data = techdisclosurebll.GetList(pagination, queryJson);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["status"].ToString() == "1")
                {
                    var engineerEntity = outsouringengineerbll.GetEntity(data.Rows[i]["engineerid"].ToString());
                    var excutdept = engineerEntity == null ? "" : departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = engineerEntity == null ? "" : departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = engineerEntity == null ? "" : string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //获取下一步审核人
                    string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["engineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
                }
            }
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
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = @" r.id as engineerid,e.fullname,t.ENGINEERNAME,t.DisclosureType,to_char(t.DisclosureDate,'yyyy-MM-dd') as DisclosureDate,disclosurename,disclosuremajordept,d.itemname as disclosuremajor,t.DisclosurePersonNum,t.DisclosurePlace,t.DisclosurePerson,t.ReceivePerson,t.CreateUserId,'' as approveuserids,flowid,issubmit,status ";
            pagination.p_tablename = @"EPG_HistoryTechDisclosure t left join EPG_OutSouringEngineer r 
on t.projectid=r.id left join base_department e on r.outprojectid=e.departmentid left join base_dataitemdetail d on t.disclosuremajor=d.itemvalue and d.itemid =(select itemid from base_dataitem where itemcode='BelongMajor')";
            pagination.conditionJson += "1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            var queryParam = queryJson.ToJObject();
            if (!queryParam["contractid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.recid='{0}'", queryParam["contractid"].ToString());
            }
            var watch = CommonHelper.TimerStart();
            var data = techdisclosurebll.GetList(pagination, queryJson);
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
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = techdisclosurebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            var data = historytechdisclosurebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetNameByPorjectId(string projectId, string type)
        {

            var data = techdisclosurebll.GetNameByPorjectId(projectId, type);
            var num = data.Rows.Count + 1;
            var no = string.Empty;
            switch (num.ToString().Length)
            {
                case 1:
                    no = "00" + num;
                    break;
                case 2:
                    no = "0" + num;
                    break;
                case 3:
                    no = num.ToString();
                    break;
                default:
                    break;
            }
            return ToJsonResult(no);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            techdisclosurebll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, TechDisclosureEntity entity)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                techdisclosurebll.SaveForm(keyValue, entity);
                return Success("操作成功。");
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        /// <summary>
        /// 登记的内容提交到审核或者结束（提交到下一个流程）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, TechDisclosureEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                techdisclosurebll.ApporveForm(keyValue, entity, aentity);
                return Success("操作成功。");
            }
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        #endregion
    }
}
