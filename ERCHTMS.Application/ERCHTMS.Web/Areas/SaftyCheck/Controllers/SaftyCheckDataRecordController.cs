using ERCHTMS.Entity.SaftyCheck;
using ERCHTMS.Busines.SaftyCheck;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using ERCHTMS.Code;
using System;
using System.Linq;
using System.Web;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using BSFramework.Util.Offices;
using System.Dynamic;
using Newtonsoft.Json;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.EquipmentManage;
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;
using BSFramework.Util.Extension;
using System.Text;
using ERCHTMS.Busines.HazardsourceManage;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using Aspose.Words;
namespace ERCHTMS.Web.Areas.SaftyCheck.Controllers
{
    /// <summary>
    /// 描 述：安全检查记录
    /// </summary>
    public class SaftyCheckDataRecordController : MvcControllerBase
    {
        private SaftyCheckDataRecordBLL srbll = new SaftyCheckDataRecordBLL();
        private SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
        private SaftyCheckContentBLL scbll = new SaftyCheckContentBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private OrganizeBLL organizeBLL = new OrganizeBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息

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
        [HttpGet]
        public ActionResult MakeNotice()
        {

            return View();
        }
        [HttpGet]
        public ActionResult WorkFlow()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Import()
        {

            return View();
        }
        [HttpGet]
        public ActionResult SuperiorIndex()
        {

            return View();
        }
        [HttpGet]
        public ActionResult SuperiorForm()
        {

            return View();
        }
        [HttpGet]
        public ActionResult TaskIndex()
        {

            return View();
        }
        /// <summary>
        /// 选择检查单位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {

            return View();
        }
        /// <summary>
        ///查看检查内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WorkContent()
        {
            return View();
        }
        /// <summary>
        /// 专项检查表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXIndex()
        {
            return View();
        }
        /// <summary>
        /// 省公司安全检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXGrpIndex()
        {
            return View();
        }
        /// <summary>
        /// 电厂查看由省公司组织的安全检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXChkIndex()
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
        /// <summary>
        /// 专项表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXForm()
        {
            
            return View();
        }
        /// <summary>
        /// 省公司安全检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXGrpForm()
        {
            return View();
        }        
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Details()
        {
            return View();
        }
        [HttpGet]
        public ActionResult PlanDetails()
        {
            return View();
        }
        /// <summary>
        /// 省公司登记检查结果
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GrpDetails()
        {
            return View();
        }
        /// <summary>
        /// 专项检查查看记录页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXDetails()
        {
            return View();
        }
        /// <summary>
        ///省公司下发检查任务
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PlanIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult PlanForm()
        {
            return View();
        }
        [HttpGet]
        public ActionResult PlanAllot()
        {
            return View();
        }
        [HttpGet]
        public ActionResult WorkSet()
        {
            return View();
        }
        #region 首页列表
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IndexView()
        {
            return View();
        }

        [HttpGet]
        public ActionResult check()
        {
            return View();
        }
        #endregion

        #endregion
        public string GetDeptIds(string deptId)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            DataTable dt = new DataTable();
            if (user.RoleName.Contains("厂级部门") || user.RoleName.Contains("公司级"))
            {
                dt = departmentBLL.GetDataTable(string.Format("select  distinct OUTPROJECTID from EPG_OUTSOURINGENGINEER "));
            }
            else
            {
                dt = departmentBLL.GetDataTable(string.Format("select  distinct OUTPROJECTID from EPG_OUTSOURINGENGINEER where ENGINEERLETDEPTID in ('{0}') ", deptId.Replace(",", "','")));
            }

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sb.AppendFormat("{0},", dr[0].ToString());
            }
            return sb.ToString().Trim(',');
        }
        #region 获取数据
         /// <summary>
        /// 根据查询条件获取部门树 
        /// </summary>
        ///<param name="json">json查询条件,字段说明:
        ///Ids:单位Id，多个逗号分隔
        ///DeptIds:页面带过来的部门id,多个用逗号分隔(以设置默认选中状态)
        ///KeyWord:部门名称查询关键字
        ///SelectMode:单选或多选，0:单选，1:多选
        ///Mode:查询模式（1:获取部门Id为ids下的所有子部门,2:获取部门Id包含在Ids中的部门（不含本单位）,3:获取当前用户所在单位下的所有子部门（含本单位）,4:获取当前用户所在单位下的所有子部门（含本单位但不包含承包商和分包商）,5:获取省级下所有厂级(前提是当前用户属于省级用户),
        ///6:获取省级本部部门和下属所有厂级(前提是当前用户属于省级用户),7:获取当前用户下的管辖的承包商(前提是当前用户部门属于厂级下的部门),8:获取当前用户所在厂级下的所有承包商(前提是当前用户部门属于厂级下的部门)
        ///</param>
        /// <returns>返回树形Json</returns>
        [HttpGet]
        public ActionResult GetDepartTreeJson(string json)
        {
            ConditionJson con = JsonConvert.DeserializeObject<ConditionJson>(json);
            int checkMode = con.SelectMode;
            int mode = con.Mode;
            string deptIds = con.DeptIds;
            string keyword = con.KeyWord;
            string parentId = "0";
            OrganizeBLL orgBLL = new OrganizeBLL();
            var treeList = new List<TreeEntity>();
            IEnumerable<DepartmentEntity> data = new List<DepartmentEntity>();
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();

            if (user.RoleName.Contains("省级"))
            {
                data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.NewDeptCode) && (t.Nature == "厂级"));
                parentId = data.ToList().FirstOrDefault().ParentId;
            }
            else if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
            {
                 data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.NewDeptCode));
                 parentId = user.OrganizeId;
            }
            else
            {
                //含本部门及下属部门及管辖承包商
                string departIds = GetDeptIds(user.DeptId);
                data = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "外包工程承包商" || (departIds.Contains(t.DepartmentId) && t.DepartmentId != "0")).OrderBy(x => x.SortCode).ToList();
                //data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode));
                parentId = user.OrganizeId;
                //data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode));
                //parentId = user.OrganizeId;
            }
            //按部门名称进行搜索
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                data = data.Where(t => t.FullName.Contains(keyword.Trim()));
            }
            data = data.OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);
            List<DepartmentEntity> newList = data.ToList();
            List<DepartmentEntity> lstDepts = newList.Where(t => t.Description == "外包工程承包商").ToList();
            foreach (DepartmentEntity dept1 in lstDepts)
            {
                newList.Remove(dept1);
                string newId = "cx100_" + Guid.NewGuid().ToString();
                    List<DepartmentEntity> lstDept2 = newList.Where(t => t.DeptType == "长协" && t.ParentId == dept1.DepartmentId).ToList();
                    if (lstDept2.Count > 0)
                    {
                        DepartmentEntity cxDept = new DepartmentEntity
                        {
                            DepartmentId = newId,
                            FullName = "长协外包单位",
                            EnCode = "cx100_" + dept1.EnCode,
                            ParentId = dept1.ParentId,
                            Description = "外包工程承包商",
                            Nature = dept1.Nature,
                            DeptCode = dept1.DeptCode,
                            Manager = dept1.Manager,
                            ManagerId = dept1.ManagerId,
                            IsOrg = dept1.IsOrg
                        };
                        newList.Add(cxDept);
                    }
                    foreach (DepartmentEntity dept2 in lstDept2)
                    {
                        dept2.ParentId = newId;
                    }
                    newId = "ls100_" + Guid.NewGuid().ToString();

                   lstDept2 = newList.Where(t => t.DeptType == "临时" && t.ParentId == dept1.DepartmentId).ToList();
                    if (lstDept2.Count > 0)
                    {
                        newList.Add(new DepartmentEntity
                        {
                            DepartmentId = newId,
                            FullName = "临时外包单位",
                            EnCode = "ls100_" + dept1.EnCode,
                            ParentId = dept1.ParentId,
                            Description = "外包工程承包商",
                            Nature = dept1.Nature,
                            DeptCode = dept1.DeptCode,
                            Manager = dept1.Manager,
                            ManagerId = dept1.ManagerId,
                            IsOrg = dept1.IsOrg
                        });
                    }
                    foreach (DepartmentEntity dept2 in lstDept2)
                    {
                        dept2.ParentId = newId;
                    }
            }
            foreach (DepartmentEntity item in newList)
            {
                int chkState = 0;
                //设置部门默认选中状态
                if (!string.IsNullOrEmpty(deptIds))
                {
                    string[] arrids = deptIds.Split(',');
                    if (arrids.Contains(item.DepartmentId) || arrids.Contains(item.EnCode))
                    {
                        chkState = 1;
                    }
                }
                TreeEntity tree = new TreeEntity();
                bool hasChildren = newList.Count(t => t.ParentId == item.DepartmentId) == 0 ? false : true;
                tree.id = item.DepartmentId;
                tree.text = item.FullName;
                tree.value = item.EnCode;
                tree.isexpand = false;
                tree.complete = true;
                tree.checkstate = chkState;
                tree.showcheck = checkMode == 0 ? false : true;
                if (item.Description == "外包工程承包商" || item.Description == "各电厂" || item.Description == "区域子公司")
                {
                    tree.showcheck = false;
                }
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.AttributeA = "Nature";
                tree.AttributeValueA = item.Nature;
                

                StringBuilder sbAccounts = new StringBuilder();
                StringBuilder sbNames = new StringBuilder();
                string sql = "";
                if(user.RoleName.Contains("省级"))
                {
                    //DepartmentEntity dept = departmentBLL.GetEntity(item.DepartmentId);
                    DataTable dtDept=departmentBLL.GetDataTable(string.Format("select t.departmentid from BASE_DEPARTMENT t where t.isorg=1 and t.parentid='{0}'", item.DepartmentId));
                    if (dtDept.Rows.Count>0)
                    {
                        string dIds = "";
                        foreach (DataRow dr in dtDept.Rows)
                        {
                            dIds += dr[0].ToString()+",";
                        }
                        dIds = dIds.TrimEnd(',');
                        sql = string.Format("select account,u.realname from base_user u where ispresence='1' and (u.departmentid='{0}' or u.departmentid in('{1}') ) and ((u.rolename like '%公司级用户%' and rolename like '%安全管理员%') or (u.rolename like '%厂级部门%' and rolename like '%负责人%') )", item.DepartmentId, dIds.Replace(",","','"));
                    }
                    else
                    {
                        sql = string.Format("select account,u.realname from base_user u where ispresence='1' and u.departmentid='{0}' and ((u.rolename like '%公司级用户%' and rolename like '%安全管理员%') or (u.rolename like '%厂级部门%' and rolename like '%负责人%') )", item.DepartmentId);
                    }
                   
                }
                else
                {
                    sql = string.Format("select account,u.realname from base_user u where ispresence='1' and u.departmentid='{0}' and ((u.rolename like '%公司级用户%' and rolename like '%安全管理员%') or (u.rolename like '%厂级部门%' and rolename like '%负责人%') or (u.rolename like '%负责人%' or rolename like '%安全管理员%'))", item.DepartmentId);
                }
                DataTable dt = departmentBLL.GetDataTable(sql);
                foreach(DataRow dr in dt.Rows)
                {
                    sbAccounts.AppendFormat("{0},",dr[0].ToString());
                    sbNames.AppendFormat("{0},", dr[1].ToString());
                }
                tree.Attribute = "managerName";
                tree.AttributeValue = sbNames.ToString().TrimEnd(',');
                tree.AttributeB = "managerAccount";
                tree.AttributeValueB = sbAccounts.ToString().TrimEnd(',');
                tree.AttributeD = "IsDept";
                tree.AttributeValueD = !string.IsNullOrWhiteSpace(item.Description) ? "0" : "1";
                treeList.Add(tree);
            }
            return Content(treeList.TreeToJson(parentId));
        }
        /// <summary>
        /// 获取省公司的电厂数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCheckedDepart()
        {
            var user = OperatorProvider.Provider.Current();
            object data = null;
            if(user.RoleName.Contains("省级"))
            {
                 data = new DepartmentBLL().GetList().Where(x => x.DeptCode.StartsWith(user.OrganizeCode) && (x.Nature == "电厂" || x.Nature == "厂级")).OrderBy(x => x.DeptCode).Select(x => { return new { ItemValue = x.DepartmentId, ItemName = x.FullName,ItemCode=x.EnCode }; });
            }
            else if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级部门"))
            {
                data = new DepartmentBLL().GetList().Where(x => x.OrganizeId == user.OrganizeId && (x.Nature == "部门")).OrderBy(x => x.DeptCode).Select(x => { return new { ItemValue = x.DepartmentId, ItemName = x.FullName, ItemCode = x.EnCode }; });
            }
            else 
            {
                data = new DepartmentBLL().GetList().Where(x => x.EnCode.StartsWith(user.DeptCode) && (x.Nature == "专业" || x.Nature == "班组" || x.DepartmentId == user.DeptId)).OrderBy(x => x.DeptCode).Select(x => { return new { ItemValue = x.DepartmentId, ItemName = x.FullName, ItemCode = x.EnCode }; });
            }
            if(data!=null)
            {
                if ((data as IEnumerable<object>).Count() == 0)
                {
                    data = new List<dynamic>() { new { ItemValue = user.OrganizeId, ItemName = user.OrganizeName, ItemCode = user.OrganizeCode } };
                }
            }
            else
            {
                data = new List<dynamic>() { new { ItemValue = user.OrganizeId, ItemName = user.OrganizeName, ItemCode = user.OrganizeCode } };
            }
            return ToJsonResult(data);
        }
        [HttpGet]
        public ActionResult GetSbJson(string keyValue)
        {
            SpecialEquipmentBLL specialequipmentbll = new SpecialEquipmentBLL();
            var entity = specialequipmentbll.GetEquimentList(keyValue);
            if (entity == null)//如果该设备被删除的话 返回空
            {
                return ToJsonResult("");
            }
            else
            {
                return ToJsonResult(entity);
            }
        }
        [HttpGet]
        public ActionResult GetCount(string recId)
        {
            var count = sdbll.GetCount(recId);
            return Success("获取成功",count);

        }
        /// <summary>
        /// 日常检查列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   

        public ActionResult GetPageListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = "CreateDate,CheckBeginTime,CheckMan,CheckManId,createuserid,createuserdeptcode,createuserorgcode,CheckDataRecordName,t.SolvePerson,CheckUserIds,isauto,isover,0 wtCount";

            pagination.conditionJson = "(datatype=0 or datatype=2)";
            var user = OperatorProvider.Provider.Current();
            string where1 = "";
            string arg = user.DeptCode;
            dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(queryJson);
            string m = dy.indexData;
            string mode = dy.mode;
            if (m != "" && mode != "")
            {
                if (mode == "1")//首页全部(本部门)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                    {
                        pagination.p_tablename = " bis_saftycheckdatarecord t ";
                        pagination.conditionJson += string.Format(" and solveperson is null and belongdept like '{0}%'", user.OrganizeCode);
                    }
                    else
                    {
                        pagination.p_tablename = string.Format(@"bis_saftycheckdatarecord t");
                        pagination.conditionJson += string.Format(@"  and belongdept like '{0}%' and checkDataType='1'   and   solveperson is null", user.OrganizeCode);
                    }
                }
                else//首页我待办跳转
                {
                    pagination.p_tablename = " bis_saftycheckdatarecord t ";

                    pagination.conditionJson += string.Format(@" and checkDataType='1' and  checkmanid like '%{0}%'  and ((instr(solveperson,'{0}'))=0 or solveperson is null) and belongdept like '{1}%'", user.Account, user.OrganizeCode, user.UserId);

                }

            }
            else
            {
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("省级用户"))
                    {
                       
                        pagination.conditionJson += string.Format(" and (t1.id is not null or createuserdeptcode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid))", user.OrganizeCode);
                    }
                    else
                    {
                           
                        where1 = string.Format(" or senddeptid='{0}'", user.DeptId);
                        pagination.conditionJson += string.Format("  and (t1.id is not null or createuserdeptcode='{0}')", arg);
                    }
                    pagination.p_tablename = string.Format(@"bis_saftycheckdatarecord t left join(select id from (select id,(','||checkmanid||',') as checkmanid  from  bis_saftycheckdatarecord) a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode like '{0}%' {1} )
) b on a.checkmanid  like '%'||b.account||'%' where account is not null group by id)t1
on t.ID=t1.id", arg, where1);
                }
                else
                {
                    pagination.p_tablename = "bis_saftycheckdatarecord t";
                }
            }
            pagination.conditionJson += string.Format(" and checkeddepartid is null and createuserdeptcode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.OrganizeCode); 
            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageList(pagination, queryJson);
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
        /// 专项检查列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   

        public ActionResult GetPageListJsonForType(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = "CreateDate,CheckBeginTime,CheckEndTime,CheckDataRecordName,SolveCount,createuserid,createuserdeptcode,createuserorgcode,t.SolvePerson,t.CheckedDepart,startdate,enddate,CheckUserIds,isauto,isover";
            pagination.p_tablename = "bis_saftycheckdatarecord t";
            var user = OperatorProvider.Provider.Current();
            string where1 = "";
            string arg = "";
            pagination.conditionJson = " datatype in(0,2)";

            dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(queryJson);
            string m = dy.indexData;
            string mode = dy.mode;
            if (m != "" && mode != "")//首页待办跳转
            {
                if (mode == "1")//首页全部(本部门)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                    {
                        pagination.p_tablename = " bis_saftycheckdatarecord t ";
                        pagination.conditionJson += string.Format(" and  solveperson is null and belongdept like '{0}%'", user.OrganizeCode);
                    }
                    else
                    {
                        pagination.p_tablename = string.Format(@" bis_saftycheckdatarecord t right join (select * from (select id,(','||checkmanid||',') as solveperson  from  bis_saftycheckdatarecord) a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.solveperson  like '%'||b.account||'%' where (account is not null or solveperson = ',,'))d
 on t.id=d.id ", user.DeptCode);
                        pagination.conditionJson += string.Format(@" and t.id in(select id from bis_saftycheckdatarecord t left join  (select recid from (select id,(','||CheckManid||',') as CheckManAccount,recid  from  BIS_SAFTYCHECKDATADETAILED) a 
left join (select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.CheckManAccount  like '%'||b.account||'%' where account is not null  group by recid)t1
on t.id=t1.recid where t1.recid is not null and t.solveperson is null) ", user.DeptCode);
                    }
                }
                else//首页我待办跳转
                {
                    if (user.RoleName.Contains("省级") || user.RoleName.Contains("集团"))
                    {
                        pagination.p_tablename = string.Format(" bis_saftycheckdatarecord t left join  (select recid from BIS_SAFTYCHECKDATADETAILED where instr(CheckManId,'{0}')>0 group by  recid)t1 on t.id=t1.recid", user.Account);
                        pagination.conditionJson += string.Format(@" and ((instr(solveperson,'{0}'))=0 or solveperson is null) and createuserorgcode= '{1}'", user.Account, user.OrganizeCode);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(@" and id in(select distinct  a.recid from bis_saftycheckdatadetailed a left join bis_saftycontent b on a.id=b.detailid where  b.id is null and (',' || a.CheckManid || ',') like '%,{0},%'
) ", user.Account);
                        //pagination.p_tablename = string.Format(" bis_saftycheckdatarecord t left join  (select recid from BIS_SAFTYCHECKDATADETAILED where instr(CheckManId,'{0}')>0 group by  recid)t1 on t.id=t1.recid", user.Account);
                        //pagination.conditionJson += string.Format(@"  and recid is not null and ((instr(solveperson,'{0}'))=0 or solveperson is null) and belongdept like  '{1}%'", user.Account, user.OrganizeCode);
                    }
                }
            }
            else
            {
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("集团用户") || user.RoleName.Contains("省级用户") )
                    {
                        arg = user.OrganizeCode;
                        pagination.conditionJson += string.Format(" and ((belongdept in (select encode from BASE_DEPARTMENT t where deptcode<>'{0}' and deptcode like '{0}%') and datatype=0) or (belongdept in (select encode from BASE_DEPARTMENT t where deptcode<>'{0}' and deptcode like '{0}%') and status=2))", user.NewDeptCode);
                      
                    }
                    else if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                    {
                        arg = user.OrganizeCode;
                        pagination.conditionJson += string.Format(" and ((belongdept like '{0}%' and datatype=0) or (datatype=2 and belongdept like '{0}%'))", user.OrganizeCode);

                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((datatype=0 and createuserdeptcode like '{0}%') or ((status=2 and (',' || checkdeptid || ',') like '%,{0}%') or (datatype=0 and id in(select recid from BIS_SAFETYCHECKDEPT where deptcode in(select encode from base_department d where d.encode like '{0}%' )))))", user.DeptCode);
                      
                    }
                }
            }
            
            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageListForType(pagination, queryJson);
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

        public ActionResult GetPageListJsonForPlan(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = "t.CreateDate,CheckBeginTime,CheckEndTime,CheckDataRecordName,SolveCount,t.createuserid,t.createuserdeptcode,t.createuserorgcode,t.CheckedDepart,startdate,enddate,checkdeptcode,datatype,status,issubmit";
            pagination.p_tablename = "bis_saftycheckdatarecord t";
            var user = OperatorProvider.Provider.Current();
            string arg = "";
            pagination.conditionJson = " 1=1";

            dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(queryJson);
            string m = dy.indexData;
            string mode = dy.mode;
            if (m != "" && mode != "")//首页待办跳转
            {
                if (mode == "1")//首页全部(本部门)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                    {
                        pagination.p_tablename = " bis_saftycheckdatarecord t ";
                        pagination.conditionJson += string.Format(" and  solveperson is null and belongdept like '{0}%'", user.OrganizeCode);
                    }
                    else
                    {
                        pagination.p_tablename = string.Format(@" bis_saftycheckdatarecord t right join (select * from (select id,(','||checkmanid||',') as solveperson  from  bis_saftycheckdatarecord) a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.solveperson  like '%'||b.account||'%' where (account is not null or solveperson = ',,'))d
 on t.id=d.id ", user.DeptCode);
                        pagination.conditionJson += string.Format(@" and t.id in(select id from bis_saftycheckdatarecord t left join  (select recid from (select id,(','||CheckManid||',') as CheckManAccount,recid  from  BIS_SAFTYCHECKDATADETAILED) a 
left join (select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.CheckManAccount  like '%'||b.account||'%' where account is not null  group by recid)t1
on t.id=t1.recid where t1.recid is not null and t.solveperson is null) ", user.DeptCode);
                    }
                }
                else//首页我待办跳转
                {
                    if (user.RoleName.Contains("省级") || user.RoleName.Contains("集团"))
                    {
                        pagination.p_tablename = string.Format(" bis_saftycheckdatarecord t left join  (select recid from BIS_SAFTYCHECKDATADETAILED where instr(CheckManId,'{0}')>0 group by  recid)t1 on t.id=t1.recid", user.Account);
                        pagination.conditionJson += string.Format(@" and ((instr(solveperson,'{0}'))=0 or solveperson is null) and createuserorgcode= '{1}'", user.Account, user.OrganizeCode);
                    }
                    else
                    {
                        pagination.p_tablename = string.Format(" bis_saftycheckdatarecord t left join  (select recid from BIS_SAFTYCHECKDATADETAILED where instr(CheckManId,'{0}')>0 group by  recid)t1 on t.id=t1.recid", user.Account);
                        pagination.conditionJson += string.Format(@"  and recid is not null and ((instr(solveperson,'{0}'))=0 or solveperson is null) and belongdept like  '{1}%'", user.Account, user.OrganizeCode);
                    }
                }
            }
            else
            {
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("集团用户") || user.RoleName.Contains("省级用户"))
                    {
                        arg = user.OrganizeCode;
                        pagination.conditionJson += string.Format(" and createuserorgcode='{0}' and rid is null", user.OrganizeCode);

                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((belongdeptid='{0}' and (',' ||receiveuserids || ',') like '%,{1},%') or dutydept='{2}')", user.DeptId, user.Account,user.DeptCode);
                    }
                }
            }

            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageListForType(pagination, queryJson);
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
        public ActionResult GetCheckTaskList(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.ID";
            pagination.p_fields = "t.CreateDate,CheckBeginTime,CheckEndTime,CheckDataRecordName,SolveCount,t.createuserid,t.createuserdeptcode,t.createuserorgcode,t.CheckedDepart,startdate,enddate,checkdeptcode,datatype,status,issubmit,t.createusername,DEPTNAME,count,0 wzcount,dutydept,dutyuserid,0 wtCount,0 Count1,0 wzCount1,0 wtCount1,rid";
            pagination.p_tablename = "bis_saftycheckdatarecord t left join v_userinfo u on t.createuserid=u.userid";
            var user = OperatorProvider.Provider.Current();
            pagination.conditionJson = " 1=1";
            if(!queryJson.Contains("rId"))
            {
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("集团用户") || user.RoleName.Contains("省级用户"))
                    {

                        pagination.conditionJson += string.Format(" and createuserorgcode='{0}' and rid is null", user.OrganizeCode);

                    }
                    else
                    {

                        if (user.RoleName.Contains("公司级"))
                        {
                            pagination.conditionJson += string.Format(" and belongdeptid='{2}' and ((createuserid='{0}' and rid is null) or ((',' ||receiveuserids || ',') like '%,{1},%'))", user.UserId, user.Account, user.OrganizeId);
                        }
                        else if( user.RoleName.Contains("厂级"))
                        {
                            pagination.conditionJson += string.Format(" and (belongdeptid='{2}' or CheckedDepartID like '%{3}%') and ((createuserid='{0}' and rid is null) or ((',' ||receiveuserids || ',') like '%,{1},%'))", user.UserId, user.Account, user.OrganizeId, user.DeptId);
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(" and belongdeptid='{2}' and ((createuserid='{0}' and rid is null) or ((',' ||receiveuserids || ',') like '%,{1},%'))", user.UserId, user.Account, user.DeptId);
                        }
                    }
                }
            }
            var data = srbll.GetCheckTaskList(pagination, queryJson);
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
        public ActionResult GetTaskList(Pagination pagination, string queryJson,string deptCode)
        {
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "t.ID";
            pagination.p_fields = "t.CreateDate,CheckBeginTime,CheckEndTime,CheckDataRecordName,0.1 SolveCount,t.CheckedDepart,startdate,enddate,status,count,0 wzcount,0 wtCount,0 count1,0 wzcount1,0 wtCount1";
            pagination.p_tablename = "bis_saftycheckdatarecord t";
            pagination.conditionJson = string.Format(" t.belongdept like '{0}%' ", deptCode);
            var user = OperatorProvider.Provider.Current();
            var data = srbll.GetCheckTaskList(pagination, queryJson);
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
        /// 省公司安全检查
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPageListJsonForGrpType(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = "CreateDate,CheckBeginTime,CheckEndTime,CheckDataRecordName,SolveCount,createuserid,createuserdeptcode,createuserorgcode,t.SolvePerson,t.CheckedDepart,t.checkeddepartid,t.checkdatatype";
            //pagination.p_tablename = "bis_saftycheckdatarecord t";
            var user = OperatorProvider.Provider.Current();
            string where1 = "";
            string arg = "";
            pagination.conditionJson = " (datatype=0 or datatype is null)";

            dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(queryJson);
            string m = dy.indexData;
            string mode = dy.mode;
            if (m != "" && mode != "")//首页待办跳转
            {
                if (mode == "1")//首页全部(本部门)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                    {
                        pagination.p_tablename = " bis_saftycheckdatarecord t ";
                        pagination.conditionJson += string.Format(" and  solveperson is null and belongdept like '{0}%'", user.OrganizeCode);
                    }
                    else
                    {
                        pagination.p_tablename = string.Format(@" bis_saftycheckdatarecord t right join (select * from (select id,(','||checkmanid||',') as solveperson  from  bis_saftycheckdatarecord) a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.solveperson  like '%'||b.account||'%' where (account is not null or solveperson = ',,'))d
 on t.id=d.id ", user.DeptCode);
                        pagination.conditionJson += string.Format(@" and t.id in(select id from bis_saftycheckdatarecord t left join  (select recid from (select id,(','||CheckManid||',') as CheckManAccount,recid  from  BIS_SAFTYCHECKDATADETAILED) a 
left join (select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode= '{0}')
) b on a.CheckManAccount  like '%'||b.account||'%' where account is not null  group by recid)t1
on t.id=t1.recid where t1.recid is not null and t.solveperson is null) ", user.DeptCode);
                    }
                }
                else//首页我待办跳转
                {
                    pagination.p_tablename = string.Format(" bis_saftycheckdatarecord t");
                    pagination.conditionJson += string.Format(@" and id in (select a.recid from bis_saftycheckdatadetailed a where  instr((',' || a.checkmanid || ','),',{0},')>0 and  a.id not in(select b.detailid from BIS_SAFTYCONTENT b)
)", user.Account);
                }
            }
            else
            {
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("省级用户"))
                    {
                        arg = user.OrganizeCode;
                        pagination.conditionJson += string.Format(" and (recid is not null or createuserdeptcode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid))", user.OrganizeCode);
                    }
                    else
                    {
                        arg = user.DeptCode;
                        where1 = string.Format(" or senddeptid='{0}'", user.DeptId);
                        pagination.conditionJson += string.Format(" and (recid is not null or createuserdeptcode='{0}')", arg);
                    }
                    pagination.p_tablename = string.Format(@" bis_saftycheckdatarecord t left join  (select recid from (select id,(','||CheckManId||',') as CheckManAccount,recid  from  BIS_SAFTYCHECKDATADETAILED) a 
left join (select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode like '{0}%' {1} )
) b on a.CheckManAccount  like '%'||b.account||'%' where account is not null  group by recid)t1
on t.id=t1.recid ", arg, where1);
                }
                else
                {
                    pagination.p_tablename = "bis_saftycheckdatarecord t";
                }
            }
            pagination.conditionJson += string.Format(" and createuserdeptcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.OrganizeCode);
            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageListForType(pagination, queryJson);
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
        /// 登记上级单位安全检查
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPageListJsonForSuperior(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = "CreateDate,CheckBeginTime,CheckEndTime,CheckDataRecordName,SolveCount,createuserid,createuserdeptcode,createuserorgcode,t.SolvePerson,t.CheckedDepart,t.checkeddepartid,t.checkdatatype";
            //pagination.p_tablename = "bis_saftycheckdatarecord t";
            var user = OperatorProvider.Provider.Current();
         
            string arg = "";
            pagination.conditionJson = " datatype=0 and CheckedDepartID='"+user.OrganizeId+"'";

            dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(queryJson);
            string m = dy.indexData;
            string mode = dy.mode;
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("省级用户"))
                    {
                        arg = user.OrganizeCode;
                       // pagination.conditionJson += string.Format(" and (recid is not null or createuserdeptcode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid))", user.OrganizeCode);
                    }
                    else
                    {
                        arg = user.DeptCode;
                    }
                    pagination.p_tablename = string.Format(@" bis_saftycheckdatarecord t left join  (select distinct recid from (select t1.id,(','|| t1.CheckManId||',') as CheckManAccount,t1.recid  from  BIS_SAFTYCHECKDATADETAILED  t1 inner join bis_saftycheckdatarecord t2 on t1.recid=t2.id where CheckedDepartID='{1}') a 
left join (select (','||account||',') as account from base_user where departmentcode like '{0}%'
) b on a.CheckManAccount  like '%'||b.account||'%')t1
on t.id=t1.recid ", arg,user.OrganizeId);
                }
                else
                {
                    pagination.p_tablename = "bis_saftycheckdatarecord t";
                }
            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageListForType(pagination, queryJson);
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
        /// 电厂查看省公司添加的安全检查
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public ActionResult GetPageListJsonForChk(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = "CreateDate,CheckBeginTime,CheckEndTime,CheckDataRecordName,SolveCount,createuserid,createuserdeptcode,createuserorgcode,t.SolvePerson,(select fullname from base_department where encode=t.createuserorgcode) as CheckDept";
            var user = OperatorProvider.Provider.Current();

            pagination.conditionJson = " 1=1";
            pagination.conditionJson += string.Format(" and (IsSynView='1' or (IsSynView='0' and CheckBeginTime<=TO_DATE('{0}','yyyy-mm-dd')))", DateTime.Now.ToString("yyyy-MM-dd"));
            pagination.p_tablename = "bis_saftycheckdatarecord t";            
            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageListForType(pagination, queryJson);
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
        /// 获取特种设备关联检查记录列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>
        [HttpGet]
        public ActionResult GetPageListJsonByTz(Pagination pagination, string keyValue)
        {
            pagination.p_kid = "b.ID";
            pagination.p_fields = string.Format(@"b.checkendtime,b.checkdatarecordname,case when b.checkdatatype=1 then b.checkman else (select wm_concat(modifyusername) from BIS_SAFTYCHECKDATADETAILED 
where recid=b.id and checkobjectid='{0}') end as CHECKMAN,(select count(id) from bis_htbaseinfo o where o.safetycheckobjectid=b.id and o.deviceid like '%{0}%') as Count", keyValue);
            pagination.p_tablename = @"bis_saftycheckdatarecord b ";
            pagination.conditionJson = string.Format(@" b.id in(
select t.id from bis_saftycheckdatarecord t left join BIS_SAFTYCHECKDATADETAILED d on t.id=d.RECID
where d.checkobjecttype like '%0%' and d.checkobjectid like '%{0}%' group by t.id) ", keyValue);
            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageListJsonByTz(pagination);
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
        /// 导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,CheckLevel,'未开始检查' SolveCount,'' count,t.SolvePerson,'' count1";
                pagination.sord = "desc";
                pagination.sidx = "CreateDate";
                var user = OperatorProvider.Provider.Current();
                string where1 = "";
                string arg = user.DeptCode;
                pagination.conditionJson = " 1=1";
                pagination.p_tablename = "bis_saftycheckdatarecord t";

                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("集团用户") || user.RoleName.Contains("省级用户"))
                    {
                        arg = user.OrganizeCode;
                        pagination.conditionJson += string.Format(" and ((belongdept in (select encode from BASE_DEPARTMENT t where deptcode<>'{0}' and deptcode like '{0}%') and datatype=0) or (belongdept in (select encode from BASE_DEPARTMENT t where deptcode<>'{0}' and deptcode like '{0}%') and status=2))", user.NewDeptCode);

                    }
                    else if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                    {
                        arg = user.OrganizeCode;
                        pagination.conditionJson += string.Format(" and ((belongdept like '{0}%' and datatype=0) or (datatype=2 and belongdept like '{0}%'))", user.OrganizeCode);

                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((datatype=0 and createuserdeptcode like '{0}%') or ((status=2 and (',' || checkdeptid || ',') like '%,{0},%') or (datatype=0 and id in(select recid from BIS_SAFETYCHECKDEPT where deptcode in(select encode from base_department d where d.encode like '{0}%' )))))", user.DeptCode);

                    }

//                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("集团") || user.RoleName.Contains("省级用户"))
//                    {
                        
//                        pagination.conditionJson += string.Format(" and (recid is not null or createuserdeptcode in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid))", user.OrganizeCode);
//                    }
//                    else
//                    {
//                        arg = user.DeptCode;
//                        where1 = string.Format(" or senddeptid='{0}'", user.DeptId);
//                        pagination.conditionJson += string.Format(" and (recid is not null or createuserdeptcode='{0}')", arg);
//                    }
//                    pagination.conditionJson += string.Format(" and t.checkeddepartid is null and t.createuserdeptcode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.OrganizeCode);
//                    pagination.p_tablename = string.Format(@" bis_saftycheckdatarecord t left join  (select recid from (select id,(','||CheckManAccount||',') as CheckManAccount,recid  from  BIS_SAFTYCONTENT) a 
//left join (select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where encode like '{0}%' {1} )
//) b on a.CheckManAccount  like '%'||b.account||'%' where account is not null  group by recid)t1
//on t.id=t1.recid ", arg, where1);
                }
                //else
                //{
                //    pagination.p_tablename = "bis_saftycheckdatarecord t";
                //}
                DataTable dt = srbll.ExportData(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "检查信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "检查信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "检查开始时间" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "检查结束时间" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "检查名称" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checklevel", ExcelColumn = "检查级别" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "solvecount", ExcelColumn = "检查进度(%)" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count", ExcelColumn = "不符合项", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count1", ExcelColumn = "处理进度", Width = 30 });
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error("导出失败，原因：" + ex.Message);
            }
            return Success("导出成功。");
        }
        public ActionResult ExportTask(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckDataRecordName,CheckBeginTime,CheckEndTime,t.CheckedDepart,t.createusername,t.CreateDate,DEPTNAME,count,0 WzCount,0 SolveCount,0 wtCount,0 count1,0 WzCount1,0 wtCount1,'' title1,'' title2";
                pagination.p_tablename = "bis_saftycheckdatarecord t left join v_userinfo u on t.createuserid=u.userid";
                var user = OperatorProvider.Provider.Current();
                pagination.conditionJson = " 1=1";
                if (!queryJson.Contains("rId"))
                {
                    if (!user.IsSystem)
                    {
                        if (user.RoleName.Contains("集团用户") || user.RoleName.Contains("省级用户"))
                        {

                            pagination.conditionJson += string.Format(" and createuserorgcode='{0}' and rid is null", user.OrganizeCode);

                        }
                        else
                        {
                            if (user.RoleName.Contains("公司级") || user.RoleName.Contains("厂级"))
                            {
                                pagination.conditionJson += string.Format(" and belongdeptid='{2}' and ((createuserid='{0}' and rid is null) or ((',' ||receiveuserids || ',') like '%,{1},%'))", user.UserId, user.Account, user.OrganizeId);
                            }
                            else
                            {
                                pagination.conditionJson += string.Format(" and belongdeptid='{2}' and ((createuserid='{0}' and rid is null) or ((',' ||receiveuserids || ',') like '%,{1},%'))", user.UserId, user.Account, user.DeptId);
                            }
                        }
                    }
                }
                var dt = srbll.GetCheckTaskList(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "检查任务信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "检查任务信息.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "检查名称", Alignment="center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "要求检查开始时间", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "要求检查结束时间", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkeddepart", ExcelColumn = "检查单位", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createusername", ExcelColumn = "创建人", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "创建时间", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "创建单位", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title1", ExcelColumn = "不符合项", Alignment = "center",Width=20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title2", ExcelColumn = "处理进度", Alignment = "center", Width = 20 });
                
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error("导出失败，原因：" + ex.Message);
            }
            return Success("导出成功。");
        }
        /// <summary>
        /// 省公司列表导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportGrpData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,t.CheckedDepart,CheckLevel,SJCheckLevel,'未开始检查' SolveCount,'' count,t.SolvePerson,'' count1";
                pagination.sord = "desc";
                pagination.sidx = "CreateDate";
                var user = OperatorProvider.Provider.Current();
                string arg = "";
                pagination.conditionJson = " datatype=0 and CheckedDepartID='" + user.OrganizeId + "'";

                dynamic dy = Newtonsoft.Json.JsonConvert.DeserializeObject(queryJson);
                string m = dy.indexData;
                string mode = dy.mode;
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("省级用户"))
                    {
                        arg = user.OrganizeCode;
                        // pagination.conditionJson += string.Format(" and (recid is not null or createuserdeptcode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid))", user.OrganizeCode);
                    }
                    else
                    {
                        arg = user.DeptCode;
                    }
                    pagination.p_tablename = string.Format(@" bis_saftycheckdatarecord t left join  (select distinct recid from (select t1.id,(','|| t1.CheckManId||',') as CheckManAccount,t1.recid  from  BIS_SAFTYCHECKDATADETAILED  t1 inner join bis_saftycheckdatarecord t2 on t1.recid=t2.id where CheckedDepartID='{1}') a 
left join (select (','||account||',') as account from base_user where departmentcode like '{0}%'
) b on a.CheckManAccount  like '%'||b.account||'%')t1
on t.id=t1.recid ", arg, user.OrganizeId);
                }
                else
                {
                    pagination.p_tablename = "bis_saftycheckdatarecord t";
                }
                DataTable dt = srbll.ExportData(pagination, queryJson);
                dt.Columns.Remove("SJCheckLevel");
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "检查信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "检查信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "检查开始时间", Alignment="center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "检查结束时间", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "检查名称", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkeddepart", ExcelColumn = "被检查单位", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checklevel", ExcelColumn = "检查级别", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "solvecount", ExcelColumn = "检查进度(%)", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count", ExcelColumn = "不符合项", Width = 20, Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count1", ExcelColumn = "处理进度", Width = 20, Alignment = "center" });
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error("导出失败，原因："+ex.Message);
            }
            return Success("导出成功。");
        }
        /// <summary>
        /// 电厂导出省公司的检查
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult ExportChkData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,CheckLevel,SJCheckLevel,'未开始检查' SolveCount,'' count,t.SolvePerson,(select fullname from base_department where encode=t.createuserorgcode) as checkorg,'' count1";
                pagination.p_tablename = "bis_saftycheckdatarecord t";
                pagination.conditionJson = " 1=1";
                pagination.conditionJson += string.Format(" and (IsSynView='1' or (IsSynView='0' and CheckBeginTime<=TO_DATE('{0}','yyyy-mm-dd')))", DateTime.Now.ToString("yyyy-MM-dd"));
                pagination.sord = "desc";
                pagination.sidx = "CreateDate";
                DataTable dt = srbll.ExportData(pagination, queryJson);
                dt.Columns.Remove("SJCheckLevel"); dt.Columns.Remove("count1");
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "检查信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "检查信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "检查开始时间" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "检查结束时间" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "检查名称" });                
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checklevel", ExcelColumn = "检查级别" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "solvecount", ExcelColumn = "检查进度(%)" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count", ExcelColumn = "不符合项", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkorg", ExcelColumn = "检查单位" });
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error("导出失败，原因：" + ex.Message);
            }
            return Success("导出成功。");
        }
        /// <summary>
        /// 根据部门和类型获取部门的检查表内容
        /// </summary>
        /// <param name="DeptCode">部门code集合</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddDeptCheckTable(string DeptCode, string Type)
        {
            var data = srbll.AddDeptCheckTable(DeptCode, Type);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 日常检查导出
        /// </summary>
        [HandlerMonitor(0, "导出数据")]
        public ActionResult DailyExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,CheckLevel,CheckMan,'未开始检查' SolveCount,'' count,t.SolvePerson,'' count1";
                pagination.conditionJson = "1=1";
                pagination.sidx = "CreateDate desc,id";
                pagination.sord = "desc";
                var user = OperatorProvider.Provider.Current();
                string where1 = "";
                string arg = user.DeptCode;
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级") || user.RoleName.Contains("集团") || user.RoleName.Contains("省级用户"))
                    {
                        pagination.conditionJson += string.Format(" and (t1.id is not null or createuserdeptcode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid))", user.OrganizeCode);
                    }
                    else
                    {
                        arg = user.DeptCode;
                        where1 = string.Format(" or senddeptid='{0}'", user.DeptId);
                        pagination.conditionJson += string.Format("  and (t1.id is not null or createuserdeptcode='{0}')", arg);

                    }
                    pagination.conditionJson += string.Format(" and checkeddepartid is null and createuserdeptcode in(select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.OrganizeCode); 
                    pagination.p_tablename = string.Format(@"bis_saftycheckdatarecord t left join(select id from (select id,(','||checkmanid||',') as checkmanid  from  bis_saftycheckdatarecord) a 
left join (
select (','||account||',') as account from base_user where  departmentcode in(select encode from base_department  where deptcode like '{0}%' {1} )
) b on a.checkmanid  like '%'||b.account||'%' where account is not null group by id)t1
on t.ID=t1.id ", arg, where1);
                }
                else
                {
                    pagination.p_tablename = "bis_saftycheckdatarecord t";
                }
                DataTable dt = srbll.ExportData(pagination, queryJson);
                //设置导出格式
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "日常检查信息";
                excelconfig.TitleFont = "微软雅黑";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "日常检查信息导出.xls";
                excelconfig.IsAllSizeColumn = true;
                //每一列的设置,没有设置的列信息，系统将按datatable中的列名导出
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
              
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "检查时间" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "检查名称" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checklevel", ExcelColumn = "检查级别" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkman", ExcelColumn = "检查人员" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count", ExcelColumn = "不符合项" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count1", ExcelColumn = "处理进度" });
                //调用导出方法
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("导出成功。");
        }

        /// <summary>
        /// 日常检查详情页面导出
        /// </summary>
        public ActionResult ExportDetails(string keyValue, string projectItem, string entity)
        {
            try
            {
             
                entity = HttpUtility.UrlDecode(entity);
                var watch = CommonHelper.TimerStart();
                SaftyCheckDataRecordEntity se = Newtonsoft.Json.JsonConvert.DeserializeObject<SaftyCheckDataRecordEntity>(entity);
                projectItem = HttpUtility.UrlDecode(projectItem);
                //加载导出模板
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/Temp/html.docx"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);
                //DataTable dt = new DataTable();
                //dt.Columns.Add("CheckDataRecordName");
                //dt.Columns.Add("CheckBeginTime");
                //dt.Columns.Add("CheckMan");
                //dt.Columns.Add("CheckDataType");
                //dt.Columns.Add("CheckLevel");
                //DataRow dr = dt.NewRow();
                //dr["CheckDataRecordName"] = se.CheckDataRecordName;
                //dr["CheckBeginTime"] = Convert.ToDateTime(se.CheckBeginTime).ToShortDateString();
                //dr["CheckMan"] = se.CheckMan;
                //DataItemBLL dataItemBLL = new DataItemBLL();
                //DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                //string id = dataItemBLL.GetList().Where(a => a.ItemCode == "SaftyCheckType").FirstOrDefault().ItemId;
                //dr["CheckDataType"] = dataItemDetailBLL.GetList(id).Where(a => a.ItemValue == se.CheckDataType.ToString()).FirstOrDefault().ItemName;

                //string ids = dataItemBLL.GetList().Where(a => a.ItemCode == "SaftyCheckLevel").FirstOrDefault().ItemId;
                //dr["CheckLevel"] = dataItemDetailBLL.GetList(ids).Where(a => a.ItemValue == se.CheckLevel.ToString()).FirstOrDefault().ItemName;

                //dt.Rows.Add(dr);

                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                string CheckDataRecordName = se.CheckDataRecordName;
                string CheckBeginTime = Convert.ToDateTime(se.CheckBeginTime).ToShortDateString();
                string CheckMan = se.CheckMan;
                DataItemBLL dataItemBLL = new DataItemBLL();
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                string id = dataItemBLL.GetList().Where(a => a.ItemCode == "SaftyCheckType").FirstOrDefault().ItemId;
                string CheckDataType = dataItemDetailBLL.GetList(id).Where(a => a.ItemValue == se.CheckDataType.ToString()).FirstOrDefault().ItemName;

                string ids = dataItemBLL.GetList().Where(a => a.ItemCode == "SaftyCheckLevel").FirstOrDefault().ItemId;
                string  CheckLevel = "省公司安全检查";
                if (se.CheckLevel != "0")
                    CheckLevel = dataItemDetailBLL.GetList(ids).Where(a => a.ItemValue == se.CheckLevel.ToString()).FirstOrDefault().ItemName;

                //检查出的隐患
                IEnumerable<HTBaseInfoEntity> hidentity = htbaseinfobll.GetList("").Where(a => a.SAFETYCHECKOBJECTID == keyValue);
                string hidstr = "";
                int i = 1;
                string HidRankids = dataItemBLL.GetList().Where(a => a.ItemCode == "HidRank").FirstOrDefault().ItemId;
                SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
                foreach (HTBaseInfoEntity item in hidentity)
                {
                    var sd = sdbll.GetEntity(item.RELEVANCEID);
                    string remark = item.HIDPOINTNAME;
                    if(sd!=null)
                    {
                        remark = sd.CheckObject;
                    }
                    hidstr += i + "、【" + remark+ "】 (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == item.HIDRANK).FirstOrDefault().ItemName + ")" + item.HIDDESCRIBE + "\r\n";
                    i++;
                }
                DataTable dt = new DepartmentBLL().GetDataTable(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='PECCANCY_ADD')", user.OrganizeId));
                if (dt.Rows[0][0].ToString()!="0")
                {
                    HidRankids = dataItemBLL.GetList().Where(a => a.ItemCode == "LllegalLevel").FirstOrDefault().ItemId;
                    LllegalRegisterBLL reg = new LllegalRegisterBLL();
                    var illList = reg.GetList(" and reseverone='" + keyValue + "'");
                    foreach (LllegalRegisterEntity ill in illList)
                    {
                        var sd = sdbll.GetEntity(ill.RESEVERTWO);
                        string remark = "违章";
                        if (sd != null)
                        {
                            remark = sd.CheckObject;
                        }

                        if (!string.IsNullOrEmpty(ill.LLLEGALLEVEL))
                        {
                            hidstr += i + "、【" + remark + "】 (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == ill.LLLEGALLEVEL).FirstOrDefault().ItemName + ")" + ill.LLLEGALDESCRIBE + "\r\n";
                        }
                        else
                        {
                            hidstr += i + "、【" + remark + "】" + ill.LLLEGALDESCRIBE + "\r\n";
                        }
                        i++;
                    }
                }
                dt = new DepartmentBLL().GetDataTable(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='QUESTION_ADD')", user.OrganizeId));
                if (dt.Rows[0][0].ToString() != "0")
                {
                    DataTable dtQuestion = new DepartmentBLL().GetDataTable(string.Format("select QUESTIONDESCRIBE from BIS_QUESTIONINFO where checkid='{0}'", keyValue));
                    foreach (DataRow dr in dtQuestion.Rows)
                    {
                        hidstr += i + "、【问题】" + dr[0].ToString() + "\r\n";
                        i++;
                    }
                }
              
                //dt.Rows.Add(dr);
                string[] arr = { "CheckDataRecordName", "CheckBeginTime", "CheckMan", "CheckDataType", "CheckLevel", "CheckHid", "CheckArea", "CheckAim", "CheckContent" };
                string[] arr2 = { CheckDataRecordName, CheckBeginTime, CheckMan, CheckDataType, CheckLevel, hidstr, se.AreaName,se.Aim,se.Remark };

                //导入到书签
                db.MoveToBookmark("HTML");
                db.InsertHtml(projectItem.Replace(@"\", "").TrimStart('"').TrimEnd('"'));
                //doc.MailMerge.Execute(dt);
                doc.MailMerge.Execute(arr, arr2);

                //设置文件名
                string fileName = "安全检查记录_"+DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                doc.Save(Server.MapPath("~/Resource/Temp/" + fileName));
                string path = Server.MapPath("~/Resource/Temp/" + fileName);

                return Success("操作成功", fileName);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// 专项检查详情页面导出
        /// </summary>
        public ActionResult ExportDetailsZX(string keyValue, string ctype, string projectItem, string entity)
        {
            try
            {
                projectItem = HttpUtility.UrlDecode(projectItem);
                entity = HttpUtility.UrlDecode(entity);
                var watch = CommonHelper.TimerStart();
                SaftyCheckDataRecordEntity se = Newtonsoft.Json.JsonConvert.DeserializeObject<SaftyCheckDataRecordEntity>(entity);
                //加载导出模板
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/Temp/html2.docx"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);

                DataItemBLL dataItemBLL = new DataItemBLL();
                var lstItems = dataItemBLL.GetList();
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                string id = lstItems.Where(a => a.ItemCode == "SaftyCheckType").FirstOrDefault().ItemId;
                string saftyCheckType = dataItemDetailBLL.GetList(id).Where(a => a.ItemValue == se.CheckDataType.ToString()).FirstOrDefault().ItemName;

                string ids = lstItems.Where(a => a.ItemCode == "SaftyCheckLevel").FirstOrDefault().ItemId;
                string checkLevel = "省公司安全检查";
                if (se.CheckLevel != "0")
                {
                    checkLevel = dataItemDetailBLL.GetList(ids).Where(a => a.ItemValue == se.CheckLevel.ToString()).FirstOrDefault().ItemName;
                }
                else
                {
                    ids = lstItems.Where(a => a.ItemCode == "SuperiorCheckLevel").FirstOrDefault().ItemId;
                    checkLevel = dataItemDetailBLL.GetList(ids).Where(a => a.ItemValue == se.SJCheckLevel).FirstOrDefault().ItemName;
                }
                //dt.Rows.Add(dr);
                string[] arr = { "CheckDataRecordName", "CheckBeginTime", "CheckEndTime", "CheckManageMan", "CheckDept", "Ctype", "CheckDataType", "CheckLevel", "CheckArea", "CheckAim", "CheckContent" };
                string[] arr2 = { se.CheckDataRecordName, Convert.ToDateTime(se.CheckBeginTime).ToShortDateString(), Convert.ToDateTime(se.CheckEndTime).ToShortDateString(), se.CheckManageMan, se.CheckDept, ctype, saftyCheckType, checkLevel, se.AreaName, se.Aim, se.Remark };
                //导入到书签
                doc.MailMerge.Execute(arr, arr2);
                db.MoveToBookmark("HTML");
                db.InsertHtml(projectItem.Replace(@"\", "").TrimStart('"').TrimEnd('"'));

                //设置文件名
                string fileName ="安全检查计划_"+ DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                doc.Save(Server.MapPath("~/Resource/Temp/" + fileName));
                string path = Server.MapPath("~/Resource/Temp/" + fileName);

                return Success("操作成功", fileName);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 查看详情导出
        /// </summary>
        public ActionResult ExportDetailsXQ(string keyValue, string ctype, string projectItem, string entity, string CheckDate)
        {
            try
            {
                projectItem = HttpUtility.UrlDecode(projectItem);
                entity = HttpUtility.UrlDecode(entity);
                var watch = CommonHelper.TimerStart();
                SaftyCheckDataRecordEntity se = Newtonsoft.Json.JsonConvert.DeserializeObject<SaftyCheckDataRecordEntity>(entity);
                projectItem = HttpUtility.UrlDecode(projectItem);
                //加载导出模板
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/Temp/html3.docx"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);

                DataItemBLL dataItemBLL = new DataItemBLL();
                var lstItems = dataItemBLL.GetList();
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                string id = lstItems.Where(a => a.ItemCode == "SaftyCheckType").FirstOrDefault().ItemId;
                string saftyCheckType = dataItemDetailBLL.GetList(id).Where(a => a.ItemValue == se.CheckDataType.ToString()).FirstOrDefault().ItemName;

                string ids = lstItems.Where(a => a.ItemCode == "SaftyCheckLevel").FirstOrDefault().ItemId;
                string checkLevel = "省公司安全检查";
                if (se.CheckLevel != "0")
                {
                    checkLevel = dataItemDetailBLL.GetList(ids).Where(a => a.ItemValue == se.CheckLevel.ToString()).FirstOrDefault().ItemName;
                }
                else
                {
                    ids = lstItems.Where(a => a.ItemCode == "SuperiorCheckLevel").FirstOrDefault().ItemId;
                    checkLevel = dataItemDetailBLL.GetList(ids).Where(a => a.ItemValue == se.SJCheckLevel.ToString()).FirstOrDefault().ItemName;
                }

                //检查出的隐患
                IEnumerable<HTBaseInfoEntity> hidentity = htbaseinfobll.GetList("").Where(a => a.SAFETYCHECKOBJECTID == keyValue);
                string hidstr = "";
                int i = 1;
                string HidRankids = dataItemBLL.GetList().Where(a => a.ItemCode == "HidRank").FirstOrDefault().ItemId;
                foreach (HTBaseInfoEntity item in hidentity)
                {
                    if (!string.IsNullOrEmpty(item.HIDPOINTNAME))
                    {
                        hidstr += i + "、【" + item.HIDPOINTNAME + "】 (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == item.HIDRANK).FirstOrDefault().ItemName + ")" + item.HIDDESCRIBE + "\r\n";
                    }
                    else
                    {
                        hidstr += i + "、 (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == item.HIDRANK).FirstOrDefault().ItemName + ")" + item.HIDDESCRIBE + "\r\n";
                    }
                    i++;
                }
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DataTable dt = new DepartmentBLL().GetDataTable(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='PECCANCY_ADD')", user.OrganizeId));
                if(dt.Rows[0][0].ToString()!="0")
                {
                    HidRankids = dataItemBLL.GetList().Where(a => a.ItemCode == "LllegalLevel").FirstOrDefault().ItemId;
                    //违章
                    LllegalRegisterBLL reg = new LllegalRegisterBLL();
                    var illList = reg.GetList(" and reseverone='" + keyValue + "'");
                    foreach (LllegalRegisterEntity ill in illList)
                    {
                        var sd = sdbll.GetEntity(ill.RESEVERTWO);
                        string remark = "违章";
                        if (sd != null)
                        {
                            remark = sd.CheckObject;
                        }

                        if (!string.IsNullOrEmpty(ill.LLLEGALLEVEL))
                        {
                            hidstr += i + "、【" + remark + "】 (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == ill.LLLEGALLEVEL).FirstOrDefault().ItemName + ")" + ill.LLLEGALDESCRIBE + "\r\n";
                        }
                        else
                        {
                            hidstr += i + "、【" + remark + "】" + ill.LLLEGALDESCRIBE + "\r\n";
                        }
                        i++;
                    }
                }
                dt = new DepartmentBLL().GetDataTable(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='QUESTION_ADD')", user.OrganizeId));
               if (dt.Rows[0][0].ToString() != "0")
               {
                   DataTable dtQuestion = new DepartmentBLL().GetDataTable(string.Format("select QUESTIONDESCRIBE from BIS_QUESTIONINFO where checkid='{0}'", keyValue));
                   foreach (DataRow dr in dtQuestion.Rows)
                   {
                       hidstr += i + "、【问题】" + dr[0].ToString() + "\r\n";
                       i++;
                   }
               }
              
                //dt.Rows.Add(dr);

                string[] arr = { "CheckDataRecordName", "CheckBeginTime", "CheckManageMan", "CheckDept", "Ctype", "CheckDataType", "CheckLevel", "SolvePersonName", "CheckHid", "CheckArea", "CheckAim", "CheckContent" };
                string[] arr2 = { se.CheckDataRecordName, CheckDate, se.CheckManageMan, se.CheckDept, ctype, saftyCheckType, checkLevel, se.SolvePersonName, hidstr,se.AreaName,se.Aim,se.Remark };
                //导入到书签
                db.MoveToBookmark("HTML");
                db.InsertHtml(projectItem.Replace(@"\", "").TrimStart('"').TrimEnd('"'));
                doc.MailMerge.Execute(arr, arr2);
                //设置文件名
                string fileName = "安全检查记录_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                doc.Save(Server.MapPath("~/Resource/Temp/" + fileName));
                string path = Server.MapPath("~/Resource/Temp/" + fileName);

                return Success("操作成功", fileName);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// 下载
        /// </summary>
        [HttpGet]
        public ActionResult DowlodeData()
        {
            string path = Path.Combine(GlobalUtil.TemplatePath.LocalPath, "myxls.xls");
            GlobalUtil.DownLoadFile(path, true);
            return Success("操作成功。");
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = srbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue,int mode=0)
        {
            var data = srbll.GetEntity(keyValue);
            if (mode==1)
            {
                DataTable dt = new DepartmentBLL().GetDataTable(string.Format("select count(1) from BIS_SAFTYCHECKDATADETAILED where recid='{0}'",keyValue));
                data.Count = dt.Rows[0][0].ToInt();
            }
            return ToJsonResult(data);
        }
        /// <summary>
        /// 根据部门CODE获取部门人员集合
        /// </summary>
        /// <param name="Encode">部门Code</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetPeopleByEncode(string Encode)
        {
            var data = srbll.GetPeopleByEncode(Encode);
            return ToJsonResult(data);
        }
        /// <summary>
        /// 获取实体(追加各个检查记录里面的人员) 
        /// </summary>
        /// <param name="recid">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJsonAddMans(string recid)
        {
            SaftyCheckDataRecordEntity data = srbll.GetEntity(recid);
            //IEnumerable<SaftyCheckDataDetailEntity> se = sdbll.GetList("").Where(a => a.RecID == recid);
            //string names = "";
            //foreach (SaftyCheckDataDetailEntity item in se)
            //{
            //    if (item.CheckMan == null) continue;
            //    string[] arr = item.CheckMan.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //    foreach (string name in arr)
            //    {
            //        if (!names.Contains(name))
            //            names += name + ",";
            //    }
            //}
            //names = names.TrimEnd(',');
            if (!string.IsNullOrEmpty(data.CheckMan))
            {
                data.CheckMan = data.CheckMan.TrimEnd('|');
            }

            return ToJsonResult(data);
        }
        /// <summary>
        /// 提交登记检查的结果 
        /// </summary>
        /// <param name="projectItem">结果集</param>
        /// <returns>返回对象Json</returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult SaveResult(string projectItem)
        {
            projectItem = HttpUtility.UrlDecode(projectItem);
            List<SaftyCheckDataDetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
            sdbll.SaveResultForm(list);
            return Success("操作成功。");
        }


        #region 首页数据列表
        /// <summary>
        /// 首页列表
        /// </summary>
        /// <param name="pagination">分页参数</param>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回分页列表Json</returns>   
        public ActionResult GetIndexData(Pagination pagination, string queryJson,string queryYear = "")
        {
            queryYear = string.IsNullOrWhiteSpace(queryYear) ? DateTime.Now.Year.ToString() : queryYear;
            pagination.p_kid = "t.ID";
            pagination.p_fields = "CreateDate,CheckDataType,CheckBeginTime,CheckEndTime,CheckDataRecordName,count";
            pagination.p_tablename = "bis_saftycheckdatarecord t";
            pagination.conditionJson = "datatype in(0,2) and to_char(t.checkbegintime,'yyyy')='" + queryYear + "'";
            var user = OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string arg = "";
                if(user.RoleName.Contains("集团")|| user.RoleName.Contains("省级用户"))
                {
                    if (queryJson.Contains("deptCode"))
                    {
                        var queryParam = queryJson.ToJObject();
                        pagination.conditionJson += string.Format(" and createuserorgcode='{0}'", queryParam["deptCode"].ToString());
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and createuserdeptcode  in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.OrganizeCode);
                    }
                  
                }
                else if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                {
                    pagination.conditionJson += string.Format("  and (belongdept like '{0}%' or ',' || CheckDeptCode like '%,{0}%' or ',' || CHECKDEPTID like '%,{0}%')", user.OrganizeCode);
                    //pagination.conditionJson += string.Format(" and ((createuserorgcode='{0}' and datatype in(0,2)) or (belongdept like '{0}%' and datatype=2))", user.OrganizeCode);

                    //pagination.conditionJson += string.Format(" and ((datatype=0 and ( ',' || CheckDeptCode like '%,{0}%' or ',' || CHECKDEPTID like '%,{0}%')) or (datatype=2 and checkeddepartid like '%{1}%'))", user.OrganizeCode, user.OrganizeId);
                     
                }
                else
                {
                    arg = user.DeptCode;
                    pagination.conditionJson += string.Format(" and (belongdept like '{0}%' or  ',' || checkdeptid || ',' like '%,{0}%' or ',' || checkdeptcode || ',' like '%,{0}%') ", user.DeptCode);
                    //pagination.conditionJson += string.Format("  and ((datatype=0 and belongdept in(select encode from base_department  where encode like '{0}%'  or senddeptid='{1}')) or (status=2 and (',' || checkdeptid || ',') like '%,{0}%') or createuserdeptcode in(select encode from base_department  where encode like '{0}%'  or senddeptid='{1}'))", arg, user.DeptId);
                }
            }
            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageList(pagination, queryJson);
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
        public ActionResult GetAllData(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = "CreateDate,CheckDataType,CheckBeginTime,CheckEndTime,CheckDataRecordName,count";
            pagination.p_tablename = "bis_saftycheckdatarecord t";
            pagination.conditionJson = "datatype in(0,2)";
            if (!queryJson.Contains("curym"))
            {
                pagination.conditionJson = "1=1 and to_char(t.CreateDate,'yyyy')='" + DateTime.Now.Year + "'";
            }
            var user = OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string arg = "";
                if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级用户"))
                {
                    if (queryJson.Contains("deptCode"))
                    {
                        var queryParam = queryJson.ToJObject();
                        pagination.conditionJson += string.Format(" and createuserorgcode='{0}'", queryParam["deptCode"].ToString());
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(queryJson))
                        {
                            var queryParam = queryJson.ToJObject();
                            if (!queryParam["deptId"].IsEmpty())
                            {
                                pagination.conditionJson += string.Format(" and ((checkeddepartid like '%{1}%' and datatype=2) or ((datatype=0 or datatype=2) and createuserdeptcode  in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)))", user.OrganizeCode, queryParam["deptId"].ToString());
                            }
                            else
                            {
                                //pagination.conditionJson += string.Format("  and ( ',' || t.checkdeptid like '%,{0}%' or belongdept like '{0}%')", user.OrganizeCode);
                            }
                        }
                    }
                }
                else if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                {
                    pagination.conditionJson += string.Format("  and (belongdept like '{0}%' or (checkeddepartid  like '%{1}%'  and datatype=2))", user.OrganizeCode, user.OrganizeId);
                }
                else
                {
                    arg = user.DeptCode;
                    pagination.conditionJson += string.Format("  and belongdept in(select encode from base_department  where encode like '{0}%'  or senddeptid='{1}')", arg, user.DeptId);
                }
            }
            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageList(pagination, queryJson);
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
        public ActionResult GetDataForTask(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.ID";
            pagination.p_fields = "CreateDate,CheckDataType,CheckBeginTime,CheckEndTime,CheckDataRecordName,count";
            pagination.p_tablename = "bis_saftycheckdatarecord t";
            pagination.conditionJson = "1=1";
            var user = OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                string arg = "";
                if (user.RoleName.Contains("集团") || user.RoleName.Contains("省级用户"))
                {
                    if (queryJson.Contains("deptCode"))
                    {
                        var queryParam = queryJson.ToJObject();
                        pagination.conditionJson += string.Format(" and createuserorgcode='{0}'", queryParam["deptCode"].ToString());
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and createuserdeptcode  in (select encode from base_department start with encode='{0}' connect by  prior departmentid = parentid)", user.OrganizeCode);
                    }

                }
                else if (user.RoleName.Contains("公司") || user.RoleName.Contains("厂级"))
                {
                    pagination.conditionJson += string.Format("  and checkeddepartid='{0}'", user.OrganizeId);
                }
                else
                {
                    arg = user.DeptCode;
                    pagination.conditionJson += string.Format("  and belongdept in(select encode from base_department  where encode like '{0}%'  or senddeptid='{1}')", arg, user.DeptId);
                }
            }
            var watch = CommonHelper.TimerStart();
            var data = srbll.GetPageList(pagination, queryJson);
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

        #endregion


        /// <summary>
        /// 检查计划是否含有当前登录人 
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult CheckInfo(string recid)
        {
            int count = sdbll.GetCheckCount(recid, OperatorProvider.Provider.Current().Account);
            bool result = count > 0 ? true : false;
            return ToJsonResult(result);
        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 更改登记人
        /// </summary>
        [HttpGet]
        public ActionResult RegisterPer(string userAccount, string id)
        {
            srbll.RegisterPer(userAccount, id);
            return Success("成功。");
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "删除检查记录")]
        public ActionResult RemoveForm(string keyValue)
        {
            srbll.RemoveForm(keyValue);
            
            return Success("删除成功。");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "设置周期性执行任务状态")]
        public ActionResult SetStatus(string keyValue,int status=0)
        {
            try
            {
                new SaftyCheckDataBLL().SetStatus(keyValue, status);
                return Success("操作成功。");
            }
            catch(Exception ex)
            {
                return Success(ex.Message);
            }
          
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="projectItem">检查项目</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AjaxOnly]
        [HandlerMonitor(5, "新增或者修改检查记录")]
        public ActionResult SaveForm(string keyValue, string projectItem, SaftyCheckDataRecordEntity entity)
        {
            
            var user = OperatorProvider.Provider.Current();

            if (!string.IsNullOrEmpty(entity.BelongDeptID))
            {
                if (!user.IsSystem)
                {
                    if (!string.IsNullOrEmpty(user.DeptId))
                        entity.BelongDeptID = user.DeptId;
                    else
                        entity.BelongDeptID = user.OrganizeId;
                }
                DepartmentEntity deptC = departmentBLL.GetEntity(entity.BelongDeptID);
                if (deptC != null)
                    entity.BelongDept = deptC.EnCode;
                else
                {
                    var orgentity = organizeBLL.GetEntity(entity.BelongDeptID);
                    entity.BelongDept = orgentity.EnCode;
                }

            }
            projectItem = HttpUtility.UrlDecode(projectItem);
            //保存安全检查记录表
            string recid = "";
            int count = srbll.SaveForm(keyValue, entity, ref recid);
            //保存安全检查表项目
            if (count > 0 && projectItem.Length > 2)
            {
                if (sdbll.Remove(entity.ID) >= 0)
                {
                List<SaftyCheckDataDetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
                sdbll.SaveForm(entity.ID, list);
                }
            }
            return Success("操作成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="projectItem">检查项目</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "新增或者修改检查记录")]
        public ActionResult AllotTask(string keyValue,string projectItem)
        {
            var user = OperatorProvider.Provider.Current();   
            projectItem = HttpUtility.UrlDecode(projectItem);
            List<SaftyCheckDataRecordEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataRecordEntity>>(projectItem);
            string id = "";
            SaftyCheckDataRecordEntity sd = srbll.GetEntity(keyValue);
            sd.Status = 1;
            sd.IsSubmit = 1;
            if (srbll.SaveForm(keyValue, sd, ref id) > 0)
            {
                HazardsourceBLL hazardsourcebll = new HazardsourceBLL();
                foreach (SaftyCheckDataRecordEntity entity in list)
                {
                    string ckName = entity.CheckDataRecordName;
                    int status = entity.Status;
                    entity.Status =0;
                    entity.IsSubmit = 1;
                    entity.DutyDept = entity.BelongDept;
                    entity.DutyUserId = sd.DutyUserId;
                    entity.CheckDataRecordName = sd.CheckDataRecordName;
                    entity.Aim = sd.Aim;
                    entity.AreaName = sd.AreaName;
                    if (string.IsNullOrWhiteSpace(entity.Remark))
                    {
                        entity.Remark = sd.Remark;
                    }
                    if (srbll.SaveForm(entity.ID, entity, ref id) > 0)
                    {
                        string newId = Guid.NewGuid().ToString();
                        if (status == 0)
                        {
                            StringBuilder sb = new StringBuilder("begin\r\n");
                            //sb.AppendFormat("insert into BIS_SAFTYCHECKDATA(id,checkdataname,checkdatatype,belongdeptid,belongdept,checkdatatypename,belongdeptcode,CREATEUSERID,CREATEDATE,CREATEUSERNAME) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}');\r\n", newId, ckName, entity.CheckDataType, entity.CheckedDepartID, entity.CheckedDepart, "", entity.BelongDept, user.UserId, "to_date('" + DateTime.Now + "','yyyy-mm-dd hh24:mi:ss')", user.UserName);
                            //sb.AppendFormat("delete from bis_saftycheckdatadetailed where recid='{1}';\r\n insert into bis_saftycheckdatadetailed(id,riskname,checkcontent,belongdeptid,belongdept,recid,checkman,checkmanid,checkdataid,checkobject,checkobjectid,checkobjecttype,checkstate) select id || '" + newId + "',t.riskname,t.checkcontent,t.belongdeptid,t.belongdept,'{1}',t.checkman,t.checkmanid,t.checkdataid,t.checkobject,t.checkobjectid,t.checkobjecttype,t.checkstate from bis_saftycheckdatadetailed t where recid='{0}';\r\n ", entity.ID, newId);

                            //newId = Guid.NewGuid().ToString();
                            sb.AppendFormat("delete from bis_saftycheckdatadetailed where recid='{1}';\r\n insert into bis_saftycheckdatadetailed(id,riskname,checkcontent,belongdeptid,belongdept,recid,checkman,checkmanid,checkdataid,checkobject,checkobjectid,checkobjecttype,checkstate,autoid) select id || '" + newId + "',t.riskname,t.checkcontent,t.belongdeptid,t.belongdept,'{1}',t.checkman,t.checkmanid,t.checkdataid,t.checkobject,t.checkobjectid,t.checkobjecttype,t.checkstate,autoid from bis_saftycheckdatadetailed t where recid='{0}';\r\n end\r\n commit;", keyValue, entity.ID);
                       
                            hazardsourcebll.ExecuteBySql(sb.ToString());
                        }
                        newId = new Random().Next(1, 1000).ToString();
                        hazardsourcebll.ExecuteBySql(string.Format(@"insert into BASE_FILEINFO(fileid,folderid,filename,filepath,filesize,fileextensions,filetype,isshare,recid,DeleteMark,createdate)
select t.fileid || '{2}' || rownum,t.folderid,t.filename,t.filepath,t.filesize,t.fileextensions,t.filetype,t.isshare,'{1}',DeleteMark,createdate from BASE_FILEINFO t where recid='{0}'", keyValue, entity.ID, newId));

                    }
                }
            }
           
            return Success("操作成功。", list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "新增或者修改检查记录")]
        public ActionResult Save(string projectItem, string recId, SaftyCheckDataRecordEntity entity)
        {
            projectItem = HttpUtility.UrlDecode(projectItem);
            SaftyCheckDataRecordEntity sc = srbll.GetEntity(recId);
            if (sc!=null)
            {
                sc.IsAuto = entity.IsAuto;
                sc.AutoType = entity.AutoType;
                sc.Weeks = entity.Weeks;
                sc.Days = entity.Days;
                sc.ThWeeks = entity.ThWeeks;
                sc.Display = entity.Display;
                sc.IsSkip = entity.IsSkip;
                sc.SelType = entity.SelType;
                sc.Remark = entity.Remark;
                string rid="";
                srbll.SaveForm(recId, sc, ref rid);
                //保存安全检查表项目
                if (!string.IsNullOrWhiteSpace(projectItem))
                {
                    if (sdbll.Remove(recId) >= 0)
                    {
                        List<SaftyCheckDataDetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
                        sdbll.SaveForm(recId, list);
                    }
                }
            }
            return Success("操作成功。");
        }
        [HttpPost]
        [AjaxOnly]
        [HandlerMonitor(5, "新增或者修改检查记录")]
        public ActionResult SaveContent(string jsonContent)
        {
            try
            {
                var user = OperatorProvider.Provider.Current();
                List<SaftyCheckDataDetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(jsonContent);
                foreach (SaftyCheckDataDetailEntity entity in list)
                {
                    SaftyCheckContentEntity sc = scbll.Get(entity.ID);
                    if(sc==null)
                    {
                        sc = new SaftyCheckContentEntity();
                        sc.CheckManName = user.UserName;
                        sc.CheckObject = entity.CheckObject;
                        sc.CheckObjectId = entity.CheckObjectId;
                        sc.CheckObjectType = entity.CheckObjectType;
                        sc.CheckManAccount = user.Account;
                        sc.SaftyContent = entity.CheckContent;
                        sc.Recid = entity.RecID;
                        sc.DetailId = entity.ID;
                        sc.IsSure = entity.IsSure;
                        sc.Remark = entity.Remark;
                        scbll.SaveForm(sc.ID, sc);
                    }
              
                }
                return Success("操作成功");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        
        /// <summary>
        /// 保存到专项检查表单（制定检查计划的时候）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <param name="projectItem">检查项目</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5,"保存检查计划")]
        public ActionResult SaveFormDate(string keyValue, SaftyCheckDataRecordEntity entity, string CheckDeptCode, string projectItem,string mode,string rId="")
        {

            var user = OperatorProvider.Provider.Current();
            List<SaftyCheckDataDetailEntity> list = new List<SaftyCheckDataDetailEntity>();
            if (!string.IsNullOrEmpty(projectItem))
            {
                projectItem = HttpUtility.UrlDecode(projectItem);
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
            }
            if (string.IsNullOrEmpty(rId))
            {
                if (!string.IsNullOrEmpty(entity.BelongDeptID) && string.IsNullOrWhiteSpace(mode))
                {
                    if (!user.IsSystem)
                    {
                        if (!string.IsNullOrEmpty(user.DeptId))
                            entity.BelongDeptID = user.DeptId;
                        else
                            entity.BelongDeptID = user.OrganizeId;
                    }
                    DepartmentEntity deptC = departmentBLL.GetEntity(entity.BelongDeptID);
                    if (deptC != null)
                        entity.BelongDept = deptC.EnCode;
                    else
                    {
                        var orgentity = organizeBLL.GetEntity(entity.BelongDeptID);
                        entity.BelongDept = orgentity.EnCode;
                    }

                }
                //保存安全检查表
                string recid = "";
                if (!string.IsNullOrEmpty(entity.CheckUserIds))
                {
                    entity.CheckUserIds = entity.CheckUserIds.Trim(',');
                }
                //if (entity.DataType == 1 && entity.IsSubmit == 1)
                //{
                if (string.IsNullOrWhiteSpace(mode))
                {
                    if (user.RoleName.Contains("省级") || user.RoleName.Contains("公司级"))
                    {
                        entity.DutyDept = user.OrganizeCode;
                    }
                    else
                    {
                        entity.DutyDept = user.DeptCode;
                    }
                    DepartmentEntity dept = departmentBLL.GetEntity(user.DeptId);
                    entity.DutyUser = dept.Nature;
                }
                //}
                if(entity.DataType==2)
                {
                    entity.CheckDept = user.DeptName;
                    entity.CheckDeptID = user.DeptCode;
                }
                int count = srbll.SaveForm(keyValue, entity, ref recid);
                if (count > 0)
                {
                 
                    if (entity.DataType == 1 && entity.IsSubmit == 1)
                    {
                        sdbll.Remove(entity.ID);
                        sdbll.Save(entity.ID, list, entity, user,CheckDeptCode);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(projectItem))
                        {
                            sdbll.Remove(entity.ID);
                            //保存在检查详情表
                            sdbll.Save(entity.ID, list, CheckDeptCode);
                        }
                        else
                        {
                            sdbll.Remove(entity.ID);
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(projectItem))
                {
                    sdbll.Remove(rId);
                    sdbll.Save(rId, list, CheckDeptCode);
                }
                else
                {
                    sdbll.Remove(rId);
                }
            }
            return Success("操作成功。");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveFormForSuperior(string keyValue, SaftyCheckDataRecordEntity entity, string CheckDeptCode, string projectItem, string mode, string rId = "")
        {

            var user = OperatorProvider.Provider.Current();
            List<SaftyCheckDataDetailEntity> list = new List<SaftyCheckDataDetailEntity>();
            if (!string.IsNullOrEmpty(projectItem))
            {
                projectItem = HttpUtility.UrlDecode(projectItem);
                list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
            }
            if (string.IsNullOrEmpty(rId))
            {
                //if (!string.IsNullOrEmpty(entity.BelongDeptID) && string.IsNullOrWhiteSpace(mode))
                //{
                //    if (!user.IsSystem)
                //    {
                //        if (!string.IsNullOrEmpty(user.DeptId))
                //            entity.BelongDeptID = user.DeptId;
                //        else
                //            entity.BelongDeptID = user.OrganizeId;
                //    }
                //    DepartmentEntity deptC = departmentBLL.GetEntity(entity.BelongDeptID);
                //    if (deptC != null)
                //        entity.BelongDept = deptC.EnCode;
                //    else
                //    {
                //        var orgentity = organizeBLL.GetEntity(entity.BelongDeptID);
                //        entity.BelongDept = orgentity.EnCode;
                //    }

                //}
                //保存安全检查表
                string recid = "";
                if (!string.IsNullOrEmpty(entity.CheckUserIds))
                {
                    entity.CheckUserIds = entity.CheckUserIds.Trim(',');
                }
                //if (entity.DataType == 1 && entity.IsSubmit == 1)
                //{
                if (string.IsNullOrWhiteSpace(mode))
                {
                    if (user.RoleName.Contains("省级") || user.RoleName.Contains("公司级"))
                    {
                        entity.DutyDept = user.OrganizeCode;
                    }
                    else
                    {
                        entity.DutyDept = user.DeptCode;
                    }
                    DepartmentEntity dept = departmentBLL.GetEntity(user.DeptId);
                    entity.DutyUser = dept.Nature;
                }
                //}
                if (entity.DataType == 2)
                {
                    entity.CheckDept = user.DeptName;
                    entity.CheckDeptID = user.DeptCode;
                }
                int count = srbll.SaveForm(keyValue, entity, ref recid);
                if (count > 0)
                {

                    if (entity.DataType == 1 && entity.IsSubmit == 1)
                    {
                        sdbll.Remove(entity.ID);
                        sdbll.Save(entity.ID, list, entity, user, CheckDeptCode);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(projectItem))
                        {
                            sdbll.Remove(entity.ID);
                            //保存在检查详情表
                            sdbll.Save(entity.ID, list, CheckDeptCode);
                        }
                        else
                        {
                            sdbll.Remove(entity.ID);
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(projectItem))
                {
                    sdbll.Remove(rId);
                    sdbll.Save(rId, list, CheckDeptCode);
                }
                else
                {
                    sdbll.Remove(rId);
                }
            }
            return Success("操作成功。");
        }
        private string GetUsers(string deptId,string accounts,string names)
        {
          UserBLL userBll=new UserBLL();
          string []arr0=accounts.Split(',');
          string []arr1=names.Split(',');
          StringBuilder sb = new StringBuilder();
          for (int j = 0; j < arr0.Length;j++ )
          {
              UserInfoEntity user = userBll.GetUserInfoByAccount(arr0[j]);
              if (user!=null)
              {
                  if (user.DepartmentId == deptId)
                  {
                      sb.AppendFormat("{0},", arr1[j]);
                  }
              }
          }
          return sb.ToString().Trim(',');
        }
        [HttpPost]
        public ActionResult GetFlowList(string id,string code)
        {
            var dtDept = departmentBLL.GetDataTable(string.Format("select encode from BASE_DEPARTMENT t where t.departmentid=(select d.organizeid from BASE_DEPARTMENT d where d.encode='{0}')",code));
            if(dtDept.Rows.Count>0)
            {
                code=dtDept.Rows[0][0].ToString();
            }
            Flow flow = new Flow();
            List<lines> lines = new List<lines>();
            List<nodes> nodes = new List<nodes>();
            SaftyCheckDataRecordEntity root = srbll.GetEntity(id);
            string deptname=departmentBLL.GetEntityByCode(root.CreateUserOrgCode).FullName;
            nodes node = new nodes();
            node.id = root.ID;
            node.left = 400;
            node.top = 10;
            node.name = "创建任务<br />("+deptname+")";
            node.type = "startround";
            node.setInfo = new setInfo
            {
                Taged=1,
                NodeDesignateData = new List<NodeDesignateData>{
                 new NodeDesignateData{
                   creatdept=deptname,
                   createuser=root.CreateUserName,
                   createdate=root.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                   status="已处理"
                 }
             }
            };

            nodes.Add(node);
            DataTable dtNodes = departmentBLL.GetDataTable(string.Format("select t.id,t.createusername,t.createdate,checkeddepart,status,t.receiveusers names,t.receiveuserids accounts,checkeddepartid deptid  from BIS_SAFTYCHECKDATARECORD t  where rid='{0}' and dutydept like '{1}%'", id,code));
            int j = 0;
            int i = 1;
            foreach(DataRow dr in dtNodes.Rows)
            {
                deptname = dr["checkeddepart"].ToString();
                node = new nodes();
                node.id = dr[0].ToString();
                node.left =j*150;
                node.top =150;
                node.width = 120;
                node.type = "stepnode";
                node.name = "分解任务<br />(" + deptname + ")";
                node.setInfo = new setInfo
                {
                    Taged=dr["status"].ToString()=="0"?0:1,
                    NodeDesignateData = new List<NodeDesignateData>{
                       new NodeDesignateData{
                         creatdept=deptname,
                          createuser=GetUsers(dr["deptid"].ToString(),dr["accounts"].ToString(),dr["names"].ToString()),
                          createdate=dr["CreateDate"].ToString(),
                          status=dr["status"].ToString()=="0"?"处理中":"已处理"
                        }
                    }
                    
                };
                nodes.Add(node);

                lines line = new lines();
                line.id = Guid.NewGuid().ToString();
                line.to = node.id;
                line.from = id;  
                line.type = "sl";
                lines.Add(line);
                j++;

                DataTable dtChild = departmentBLL.GetDataTable(string.Format("select t.id,t.createusername,t.createdate,checkeddepart,status,t.receiveusers names,t.receiveuserids accounts,checkeddepartid deptid  from BIS_SAFTYCHECKDATARECORD t where rid='{0}' and dutydept like '{1}%'", dr[0].ToString(),code));
                int k = 0;
                foreach (DataRow row in dtChild.Rows)
                {
                    deptname = row["checkeddepart"].ToString();
                    nodes node1 = new nodes();
                    node1.id = row[0].ToString();
                    node1.left =k*160;
                    node1.top = 300;
                    node1.width = 130;
                    node1.type = "stepnode";
                    node1.name = "分解任务<br />("+dr["checkeddepart"].ToString()+"-"+ deptname + ")";
                    node1.setInfo = new setInfo
                    {
                        Taged = row["status"].ToString() == "0" ? 0 : 1,
                        NodeDesignateData = new List<NodeDesignateData>{
                       new NodeDesignateData{
                         creatdept=deptname,
                         createuser=GetUsers(row["deptid"].ToString(),row["accounts"].ToString(),row["names"].ToString()),
                         createdate=row["CreateDate"].ToString(),
                         status=row["status"].ToString()=="0"?"处理中":"已处理"
                        }
                    }
                    };
                    nodes.Add(node1);

                    line = new lines();
                    line.id = Guid.NewGuid().ToString();
                    line.to = node1.id;
                    line.from = dr[0].ToString();
                    line.type = "sl";
                    lines.Add(line);
                    k++;

                    DataTable dtChild1 = departmentBLL.GetDataTable(string.Format("select t.id,t.createusername,t.createdate,checkeddepart,status,t.receiveusers names,t.receiveuserids accounts,checkeddepartid deptid  from BIS_SAFTYCHECKDATARECORD t where rid='{0}' and dutydept like '{1}%'", node1.id, code));

                    foreach (DataRow row1 in dtChild1.Rows)
                    {
                        deptname = row1["checkeddepart"].ToString();
                        nodes node2 = new nodes();
                        node2.id = Guid.NewGuid().ToString();
                        node2.left = i* 120;
                        node2.top = 400;
                        node2.width = 100;
                        node2.type = "stepnode";
                        node2.name = "分解任务<br />(" + deptname + ")";
                        node2.setInfo = new setInfo
                        {
                            Taged = row1["status"].ToString() == "0" ? 0 : 1,
                            NodeDesignateData = new List<NodeDesignateData>{
                       new NodeDesignateData{
                         creatdept=deptname,
                         createuser=GetUsers(row1["deptid"].ToString(),row1["accounts"].ToString(),row1["names"].ToString()),
                         createdate=row1["CreateDate"].ToString(),
                         status=row1["status"].ToString()=="0"?"处理中":"已处理"
                        }
                    }
                        };
                        nodes.Add(node2);

                        line = new lines();
                        line.id = Guid.NewGuid().ToString();
                        line.to = node2.id;
                        line.from = node1.id;
                        line.type = "sl";
                        lines.Add(line);
                        i++;
                    }
                }
            }
           // flow.activeID = nodes[0].id;
            flow.nodes = nodes;
            flow.lines = lines;
            flow.title = "检查任务分配流程";
            return Success("获取数据成功", flow);
        }
        #endregion

        #region 首页预警
        /// <summary>
        /// 安全检查预警
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSafeCheckWarning(string time = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var data = string.IsNullOrEmpty(time) ? srbll.GetSafeCheckWarning(user) : srbll.GetSafeCheckWarningByTime(user, time, 1);
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }
        /// <summary>
        /// 安全检查预警
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetSafeCheckWarningByTime(string time = "")
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            object obj = srbll.GetSafeCheckWarningByTime(user, time);
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 安全检查预警
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateCheckUsers()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                return Error("对不起,您没有权限进行此操作！");
            }
            try
            {
               srbll.UpdateCheckUsers();
               return Success("操作成功");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        #endregion

        /// 无损压缩图片  
        /// <param name="sFile">原图片</param>  
        /// <param name="dFile">压缩后保存位置</param>  
        /// <param name="dHeight">高度</param>  
        /// <param name="dWidth"></param>  
        /// <param name="flag">压缩质量(数字越小压缩率越高) 1-100</param>  
        /// <returns></returns>  

        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);

            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();
            //以下代码为保存图片时，设置压缩质量  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100  
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径  
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }
        /// <summary>
        /// 导出检查记录汇总表（华润湖北）
        /// </summary>
        /// <param name="checkId">检查记录ID</param>
        /// <param name="checkType">检查类型（0：非日常检查，1：日常检查,2：隐患排查）</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportResult(string checkId, int checkType = 0)
        {
            try
            {
                string checkDept = "";
                string checkUser = "";
                string checkTime = "";
                string checkarea = string.Empty;
                int mode = 0;
                //安全检查入口
                SaftyCheckDataRecordEntity entity = srbll.GetEntity(checkId);
                if (null != entity) 
                {
                    //if (string.IsNullOrEmpty(entity.AreaName))
                    //{
                    //    checkarea = entity.Remark; 
                    //}
                    //else
                    //{
                        checkarea = entity.AreaName; 
                   // }
                }
                if (checkType == 0)
                {
                    checkDept = entity.CheckDept;
                    checkUser = entity.CheckUsers;
                    checkTime = entity.CheckBeginTime.Value.ToString("yyyy.MM.dd") + "-" + entity.CheckEndTime.Value.ToString("yyyy.MM.dd");
                }
                if (checkType == 1)
                {
                    checkUser = entity.CheckMan;
                    checkDept = new UserBLL().GetUserInfoByAccount(entity.CheckManID).DeptName;
                    checkTime = entity.CheckBeginTime.Value.ToString("yyyy.MM.dd");
                }
                if (checkType == 2)  //隐患入口
                {
                    mode = 1;
                    HTBaseInfoEntity htentity = htbaseinfobll.GetEntity(checkId);
                    checkDept = htentity.CHECKDEPARTNAME;
                    checkUser = htentity.CHECKMANNAME;
                    checkarea = htentity.HIDPOINTNAME;
                    checkTime = htentity.CHECKDATE.Value.ToString("yyyy.MM.dd");
                }
              
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/DocumentFile/安全检查结果导出.doc"));
                doc.MailMerge.Execute(new string[] { "deptname", "members", "area", "time" }, new string[] { checkDept, checkUser, checkarea, checkTime });
                DataTable dt = new HTBaseInfoBLL().GetHiddenOfSafetyCheck(checkId, mode);
                dt.TableName = "T";
                dt.Columns.Add("img1"); dt.Columns.Add("img2"); dt.Columns.Add("img3");
                dt.Columns.Add("reimg1"); dt.Columns.Add("reimg2"); dt.Columns.Add("reimg3");
                foreach(DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["filepath"].ToString()))
                    {
                        string[] arr = dr["filepath"].ToString().Split(',');
                        int len = arr.Length >= 3 ? 3 : arr.Length;
                        for (int j = 0; j < len; j++)
                        {
                            string path = Server.MapPath(arr[j]);
                            if (System.IO.File.Exists(path))
                            {
                                string fileName = Server.MapPath("~/Resource/Temp/" + System.IO.Path.GetFileName(arr[j]));
                                if (GetPicThumbnail(path, fileName, 140, 160, 20))
                                {
                                    dr["img" + (j + 1)] = fileName;
                                }
                            }
                           
                        }
                    }
                    if (!string.IsNullOrEmpty(dr["reformfilepath"].ToString()))
                    {
                        string[] arr = dr["reformfilepath"].ToString().Split(',');
                        int len = arr.Length >= 3 ? 3 : arr.Length;
                        for (int j = 0; j < len; j++)
                        {
                            string path = Server.MapPath(arr[j]);
                            if (System.IO.File.Exists(path))
                            {
                                string fileName = Server.MapPath("~/Resource/Temp/" + System.IO.Path.GetFileName(arr[j]));
                                if (GetPicThumbnail(path, fileName, 140, 160, 20))
                                {
                                    dr["reimg" + (j + 1)] = fileName;
                                }
                            }
                        }
                    }
                }
                doc.MailMerge.ExecuteWithRegions(dt);

                DocumentBuilder db = new DocumentBuilder(doc);
                db.MoveToMergeField("deptname");
                db.InsertHtml("<span>" + checkDept + "</span>");

                string filename = Guid.NewGuid().ToString() + ".doc"; 
                doc.MailMerge.DeleteFields();
                doc.Save(Server.MapPath("~/Resource/temp/" + filename), Aspose.Words.SaveFormat.Doc);
                return Success("操作成功", filename);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
    }
}
