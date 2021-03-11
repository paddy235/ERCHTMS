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
    /// �� ������ȫ����¼
    /// </summary>
    public class SaftyCheckDataRecordController : MvcControllerBase
    {
        private SaftyCheckDataRecordBLL srbll = new SaftyCheckDataRecordBLL();
        private SaftyCheckDataDetailBLL sdbll = new SaftyCheckDataDetailBLL();
        private SaftyCheckContentBLL scbll = new SaftyCheckContentBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private OrganizeBLL organizeBLL = new OrganizeBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //����������Ϣ

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
        /// ѡ���鵥λ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Select()
        {

            return View();
        }
        /// <summary>
        ///�鿴�������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WorkContent()
        {
            return View();
        }
        /// <summary>
        /// ר�����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXIndex()
        {
            return View();
        }
        /// <summary>
        /// ʡ��˾��ȫ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXGrpIndex()
        {
            return View();
        }
        /// <summary>
        /// �糧�鿴��ʡ��˾��֯�İ�ȫ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXChkIndex()
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
        /// ר���ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXForm()
        {
            
            return View();
        }
        /// <summary>
        /// ʡ��˾��ȫ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXGrpForm()
        {
            return View();
        }        
        /// <summary>
        /// ��ҳ��
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
        /// ʡ��˾�ǼǼ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GrpDetails()
        {
            return View();
        }
        /// <summary>
        /// ר����鿴��¼ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ZXDetails()
        {
            return View();
        }
        /// <summary>
        ///ʡ��˾�·��������
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
        #region ��ҳ�б�
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
            if (user.RoleName.Contains("��������") || user.RoleName.Contains("��˾��"))
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
        #region ��ȡ����
         /// <summary>
        /// ���ݲ�ѯ������ȡ������ 
        /// </summary>
        ///<param name="json">json��ѯ����,�ֶ�˵��:
        ///Ids:��λId��������ŷָ�
        ///DeptIds:ҳ��������Ĳ���id,����ö��ŷָ�(������Ĭ��ѡ��״̬)
        ///KeyWord:�������Ʋ�ѯ�ؼ���
        ///SelectMode:��ѡ���ѡ��0:��ѡ��1:��ѡ
        ///Mode:��ѯģʽ��1:��ȡ����IdΪids�µ������Ӳ���,2:��ȡ����Id������Ids�еĲ��ţ���������λ��,3:��ȡ��ǰ�û����ڵ�λ�µ������Ӳ��ţ�������λ��,4:��ȡ��ǰ�û����ڵ�λ�µ������Ӳ��ţ�������λ���������а��̺ͷְ��̣�,5:��ȡʡ�������г���(ǰ���ǵ�ǰ�û�����ʡ���û�),
        ///6:��ȡʡ���������ź��������г���(ǰ���ǵ�ǰ�û�����ʡ���û�),7:��ȡ��ǰ�û��µĹ�Ͻ�ĳа���(ǰ���ǵ�ǰ�û��������ڳ����µĲ���),8:��ȡ��ǰ�û����ڳ����µ����га���(ǰ���ǵ�ǰ�û��������ڳ����µĲ���)
        ///</param>
        /// <returns>��������Json</returns>
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

            if (user.RoleName.Contains("ʡ��"))
            {
                data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.NewDeptCode) && (t.Nature == "����"));
                parentId = data.ToList().FirstOrDefault().ParentId;
            }
            else if (user.RoleName.Contains("��˾��") || user.RoleName.Contains("����"))
            {
                 data = departmentBLL.GetList().Where(t => t.DeptCode.StartsWith(user.NewDeptCode));
                 parentId = user.OrganizeId;
            }
            else
            {
                //�������ż��������ż���Ͻ�а���
                string departIds = GetDeptIds(user.DeptId);
                data = departmentBLL.GetList(user.OrganizeId).Where(t => t.EnCode.Contains(user.DeptCode) || t.Description == "������̳а���" || (departIds.Contains(t.DepartmentId) && t.DepartmentId != "0")).OrderBy(x => x.SortCode).ToList();
                //data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode));
                parentId = user.OrganizeId;
                //data = departmentBLL.GetList().Where(t => t.EnCode.StartsWith(user.DeptCode));
                //parentId = user.OrganizeId;
            }
            //���������ƽ�������
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                data = data.Where(t => t.FullName.Contains(keyword.Trim()));
            }
            data = data.OrderBy(t => t.DeptCode).OrderBy(t => t.SortCode);
            List<DepartmentEntity> newList = data.ToList();
            List<DepartmentEntity> lstDepts = newList.Where(t => t.Description == "������̳а���").ToList();
            foreach (DepartmentEntity dept1 in lstDepts)
            {
                newList.Remove(dept1);
                string newId = "cx100_" + Guid.NewGuid().ToString();
                    List<DepartmentEntity> lstDept2 = newList.Where(t => t.DeptType == "��Э" && t.ParentId == dept1.DepartmentId).ToList();
                    if (lstDept2.Count > 0)
                    {
                        DepartmentEntity cxDept = new DepartmentEntity
                        {
                            DepartmentId = newId,
                            FullName = "��Э�����λ",
                            EnCode = "cx100_" + dept1.EnCode,
                            ParentId = dept1.ParentId,
                            Description = "������̳а���",
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

                   lstDept2 = newList.Where(t => t.DeptType == "��ʱ" && t.ParentId == dept1.DepartmentId).ToList();
                    if (lstDept2.Count > 0)
                    {
                        newList.Add(new DepartmentEntity
                        {
                            DepartmentId = newId,
                            FullName = "��ʱ�����λ",
                            EnCode = "ls100_" + dept1.EnCode,
                            ParentId = dept1.ParentId,
                            Description = "������̳а���",
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
                //���ò���Ĭ��ѡ��״̬
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
                if (item.Description == "������̳а���" || item.Description == "���糧" || item.Description == "�����ӹ�˾")
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
                if(user.RoleName.Contains("ʡ��"))
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
                        sql = string.Format("select account,u.realname from base_user u where ispresence='1' and (u.departmentid='{0}' or u.departmentid in('{1}') ) and ((u.rolename like '%��˾���û�%' and rolename like '%��ȫ����Ա%') or (u.rolename like '%��������%' and rolename like '%������%') )", item.DepartmentId, dIds.Replace(",","','"));
                    }
                    else
                    {
                        sql = string.Format("select account,u.realname from base_user u where ispresence='1' and u.departmentid='{0}' and ((u.rolename like '%��˾���û�%' and rolename like '%��ȫ����Ա%') or (u.rolename like '%��������%' and rolename like '%������%') )", item.DepartmentId);
                    }
                   
                }
                else
                {
                    sql = string.Format("select account,u.realname from base_user u where ispresence='1' and u.departmentid='{0}' and ((u.rolename like '%��˾���û�%' and rolename like '%��ȫ����Ա%') or (u.rolename like '%��������%' and rolename like '%������%') or (u.rolename like '%������%' or rolename like '%��ȫ����Ա%'))", item.DepartmentId);
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
        /// ��ȡʡ��˾�ĵ糧����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCheckedDepart()
        {
            var user = OperatorProvider.Provider.Current();
            object data = null;
            if(user.RoleName.Contains("ʡ��"))
            {
                 data = new DepartmentBLL().GetList().Where(x => x.DeptCode.StartsWith(user.OrganizeCode) && (x.Nature == "�糧" || x.Nature == "����")).OrderBy(x => x.DeptCode).Select(x => { return new { ItemValue = x.DepartmentId, ItemName = x.FullName,ItemCode=x.EnCode }; });
            }
            else if (user.RoleName.Contains("��˾��") || user.RoleName.Contains("��������"))
            {
                data = new DepartmentBLL().GetList().Where(x => x.OrganizeId == user.OrganizeId && (x.Nature == "����")).OrderBy(x => x.DeptCode).Select(x => { return new { ItemValue = x.DepartmentId, ItemName = x.FullName, ItemCode = x.EnCode }; });
            }
            else 
            {
                data = new DepartmentBLL().GetList().Where(x => x.EnCode.StartsWith(user.DeptCode) && (x.Nature == "רҵ" || x.Nature == "����" || x.DepartmentId == user.DeptId)).OrderBy(x => x.DeptCode).Select(x => { return new { ItemValue = x.DepartmentId, ItemName = x.FullName, ItemCode = x.EnCode }; });
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
            if (entity == null)//������豸��ɾ���Ļ� ���ؿ�
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
            return Success("��ȡ�ɹ�",count);

        }
        /// <summary>
        /// �ճ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   

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
                if (mode == "1")//��ҳȫ��(������)
                {
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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
                else//��ҳ�Ҵ�����ת
                {
                    pagination.p_tablename = " bis_saftycheckdatarecord t ";

                    pagination.conditionJson += string.Format(@" and checkDataType='1' and  checkmanid like '%{0}%'  and ((instr(solveperson,'{0}'))=0 or solveperson is null) and belongdept like '{1}%'", user.Account, user.OrganizeCode, user.UserId);

                }

            }
            else
            {
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����") || user.RoleName.Contains("ʡ���û�"))
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
        /// ר�����б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   

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
            if (m != "" && mode != "")//��ҳ������ת
            {
                if (mode == "1")//��ҳȫ��(������)
                {
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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
                else//��ҳ�Ҵ�����ת
                {
                    if (user.RoleName.Contains("ʡ��") || user.RoleName.Contains("����"))
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
                    if (user.RoleName.Contains("�����û�") || user.RoleName.Contains("ʡ���û�") )
                    {
                        arg = user.OrganizeCode;
                        pagination.conditionJson += string.Format(" and ((belongdept in (select encode from BASE_DEPARTMENT t where deptcode<>'{0}' and deptcode like '{0}%') and datatype=0) or (belongdept in (select encode from BASE_DEPARTMENT t where deptcode<>'{0}' and deptcode like '{0}%') and status=2))", user.NewDeptCode);
                      
                    }
                    else if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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
            if (m != "" && mode != "")//��ҳ������ת
            {
                if (mode == "1")//��ҳȫ��(������)
                {
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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
                else//��ҳ�Ҵ�����ת
                {
                    if (user.RoleName.Contains("ʡ��") || user.RoleName.Contains("����"))
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
                    if (user.RoleName.Contains("�����û�") || user.RoleName.Contains("ʡ���û�"))
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
                    if (user.RoleName.Contains("�����û�") || user.RoleName.Contains("ʡ���û�"))
                    {

                        pagination.conditionJson += string.Format(" and createuserorgcode='{0}' and rid is null", user.OrganizeCode);

                    }
                    else
                    {

                        if (user.RoleName.Contains("��˾��"))
                        {
                            pagination.conditionJson += string.Format(" and belongdeptid='{2}' and ((createuserid='{0}' and rid is null) or ((',' ||receiveuserids || ',') like '%,{1},%'))", user.UserId, user.Account, user.OrganizeId);
                        }
                        else if( user.RoleName.Contains("����"))
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
        /// ʡ��˾��ȫ���
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
            if (m != "" && mode != "")//��ҳ������ת
            {
                if (mode == "1")//��ҳȫ��(������)
                {
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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
                else//��ҳ�Ҵ�����ת
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
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����") || user.RoleName.Contains("ʡ���û�"))
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
        /// �Ǽ��ϼ���λ��ȫ���
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
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����") || user.RoleName.Contains("ʡ���û�"))
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
        /// �糧�鿴ʡ��˾��ӵİ�ȫ���
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
        /// ��ȡ�����豸��������¼�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
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
        /// ����
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,CheckLevel,'δ��ʼ���' SolveCount,'' count,t.SolvePerson,'' count1";
                pagination.sord = "desc";
                pagination.sidx = "CreateDate";
                var user = OperatorProvider.Provider.Current();
                string where1 = "";
                string arg = user.DeptCode;
                pagination.conditionJson = " 1=1";
                pagination.p_tablename = "bis_saftycheckdatarecord t";

                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("�����û�") || user.RoleName.Contains("ʡ���û�"))
                    {
                        arg = user.OrganizeCode;
                        pagination.conditionJson += string.Format(" and ((belongdept in (select encode from BASE_DEPARTMENT t where deptcode<>'{0}' and deptcode like '{0}%') and datatype=0) or (belongdept in (select encode from BASE_DEPARTMENT t where deptcode<>'{0}' and deptcode like '{0}%') and status=2))", user.NewDeptCode);

                    }
                    else if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
                    {
                        arg = user.OrganizeCode;
                        pagination.conditionJson += string.Format(" and ((belongdept like '{0}%' and datatype=0) or (datatype=2 and belongdept like '{0}%'))", user.OrganizeCode);

                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((datatype=0 and createuserdeptcode like '{0}%') or ((status=2 and (',' || checkdeptid || ',') like '%,{0},%') or (datatype=0 and id in(select recid from BIS_SAFETYCHECKDEPT where deptcode in(select encode from base_department d where d.encode like '{0}%' )))))", user.DeptCode);

                    }

//                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����") || user.RoleName.Contains("����") || user.RoleName.Contains("ʡ���û�"))
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
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "�����Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "�����Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "��鿪ʼʱ��" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "������ʱ��" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "�������" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checklevel", ExcelColumn = "��鼶��" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "solvecount", ExcelColumn = "������(%)" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count", ExcelColumn = "��������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count1", ExcelColumn = "�������", Width = 30 });
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error("����ʧ�ܣ�ԭ��" + ex.Message);
            }
            return Success("�����ɹ���");
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
                        if (user.RoleName.Contains("�����û�") || user.RoleName.Contains("ʡ���û�"))
                        {

                            pagination.conditionJson += string.Format(" and createuserorgcode='{0}' and rid is null", user.OrganizeCode);

                        }
                        else
                        {
                            if (user.RoleName.Contains("��˾��") || user.RoleName.Contains("����"))
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
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "���������Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "���������Ϣ.xls";
                excelconfig.IsAllSizeColumn = true;
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "�������", Alignment="center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "Ҫ���鿪ʼʱ��", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "Ҫ�������ʱ��", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkeddepart", ExcelColumn = "��鵥λ", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createusername", ExcelColumn = "������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "createdate", ExcelColumn = "����ʱ��", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "deptname", ExcelColumn = "������λ", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title1", ExcelColumn = "��������", Alignment = "center",Width=20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "title2", ExcelColumn = "�������", Alignment = "center", Width = 20 });
                
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error("����ʧ�ܣ�ԭ��" + ex.Message);
            }
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ʡ��˾�б���
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportGrpData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,t.CheckedDepart,CheckLevel,SJCheckLevel,'δ��ʼ���' SolveCount,'' count,t.SolvePerson,'' count1";
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
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����") || user.RoleName.Contains("ʡ���û�"))
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
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "�����Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "�����Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "��鿪ʼʱ��", Alignment="center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "������ʱ��", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "�������", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkeddepart", ExcelColumn = "����鵥λ", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checklevel", ExcelColumn = "��鼶��", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "solvecount", ExcelColumn = "������(%)", Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count", ExcelColumn = "��������", Width = 20, Alignment = "center" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count1", ExcelColumn = "�������", Width = 20, Alignment = "center" });
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error("����ʧ�ܣ�ԭ��"+ex.Message);
            }
            return Success("�����ɹ���");
        }
        /// <summary>
        /// �糧����ʡ��˾�ļ��
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult ExportChkData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,CheckLevel,SJCheckLevel,'δ��ʼ���' SolveCount,'' count,t.SolvePerson,(select fullname from base_department where encode=t.createuserorgcode) as checkorg,'' count1";
                pagination.p_tablename = "bis_saftycheckdatarecord t";
                pagination.conditionJson = " 1=1";
                pagination.conditionJson += string.Format(" and (IsSynView='1' or (IsSynView='0' and CheckBeginTime<=TO_DATE('{0}','yyyy-mm-dd')))", DateTime.Now.ToString("yyyy-MM-dd"));
                pagination.sord = "desc";
                pagination.sidx = "CreateDate";
                DataTable dt = srbll.ExportData(pagination, queryJson);
                dt.Columns.Remove("SJCheckLevel"); dt.Columns.Remove("count1");
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "�����Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 16;
                excelconfig.FileName = "�����Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "��鿪ʼʱ��" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkendtime", ExcelColumn = "������ʱ��" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "�������" });                
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checklevel", ExcelColumn = "��鼶��" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "solvecount", ExcelColumn = "������(%)" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count", ExcelColumn = "��������", Width = 20 });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkorg", ExcelColumn = "��鵥λ" });
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error("����ʧ�ܣ�ԭ��" + ex.Message);
            }
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ���ݲ��ź����ͻ�ȡ���ŵļ�������
        /// </summary>
        /// <param name="DeptCode">����code����</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddDeptCheckTable(string DeptCode, string Type)
        {
            var data = srbll.AddDeptCheckTable(DeptCode, Type);
            return ToJsonResult(data);
        }

        /// <summary>
        /// �ճ���鵼��
        /// </summary>
        [HandlerMonitor(0, "��������")]
        public ActionResult DailyExportData(string queryJson)
        {
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                pagination.p_kid = "t.ID";
                pagination.p_fields = "CheckBeginTime,checkendtime,CheckDataRecordName,CheckLevel,CheckMan,'δ��ʼ���' SolveCount,'' count,t.SolvePerson,'' count1";
                pagination.conditionJson = "1=1";
                pagination.sidx = "CreateDate desc,id";
                pagination.sord = "desc";
                var user = OperatorProvider.Provider.Current();
                string where1 = "";
                string arg = user.DeptCode;
                if (!user.IsSystem)
                {
                    if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����") || user.RoleName.Contains("����") || user.RoleName.Contains("ʡ���û�"))
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
                //���õ�����ʽ
                ExcelConfig excelconfig = new ExcelConfig();
                excelconfig.Title = "�ճ������Ϣ";
                excelconfig.TitleFont = "΢���ź�";
                excelconfig.TitlePoint = 25;
                excelconfig.FileName = "�ճ������Ϣ����.xls";
                excelconfig.IsAllSizeColumn = true;
                //ÿһ�е�����,û�����õ�����Ϣ��ϵͳ����datatable�е���������
                List<ColumnEntity> listColumnEntity = new List<ColumnEntity>();
                excelconfig.ColumnEntity = listColumnEntity;
                ColumnEntity columnentity = new ColumnEntity();
              
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkbegintime", ExcelColumn = "���ʱ��" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkdatarecordname", ExcelColumn = "�������" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checklevel", ExcelColumn = "��鼶��" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "checkman", ExcelColumn = "�����Ա" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count", ExcelColumn = "��������" });
                excelconfig.ColumnEntity.Add(new ColumnEntity() { Column = "count1", ExcelColumn = "�������" });
                //���õ�������
                ExcelHelper.ExcelDownload(dt, excelconfig);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("�����ɹ���");
        }

        /// <summary>
        /// �ճ��������ҳ�浼��
        /// </summary>
        public ActionResult ExportDetails(string keyValue, string projectItem, string entity)
        {
            try
            {
             
                entity = HttpUtility.UrlDecode(entity);
                var watch = CommonHelper.TimerStart();
                SaftyCheckDataRecordEntity se = Newtonsoft.Json.JsonConvert.DeserializeObject<SaftyCheckDataRecordEntity>(entity);
                projectItem = HttpUtility.UrlDecode(projectItem);
                //���ص���ģ��
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
                string  CheckLevel = "ʡ��˾��ȫ���";
                if (se.CheckLevel != "0")
                    CheckLevel = dataItemDetailBLL.GetList(ids).Where(a => a.ItemValue == se.CheckLevel.ToString()).FirstOrDefault().ItemName;

                //����������
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
                    hidstr += i + "����" + remark+ "�� (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == item.HIDRANK).FirstOrDefault().ItemName + ")" + item.HIDDESCRIBE + "\r\n";
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
                        string remark = "Υ��";
                        if (sd != null)
                        {
                            remark = sd.CheckObject;
                        }

                        if (!string.IsNullOrEmpty(ill.LLLEGALLEVEL))
                        {
                            hidstr += i + "����" + remark + "�� (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == ill.LLLEGALLEVEL).FirstOrDefault().ItemName + ")" + ill.LLLEGALDESCRIBE + "\r\n";
                        }
                        else
                        {
                            hidstr += i + "����" + remark + "��" + ill.LLLEGALDESCRIBE + "\r\n";
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
                        hidstr += i + "�������⡿" + dr[0].ToString() + "\r\n";
                        i++;
                    }
                }
              
                //dt.Rows.Add(dr);
                string[] arr = { "CheckDataRecordName", "CheckBeginTime", "CheckMan", "CheckDataType", "CheckLevel", "CheckHid", "CheckArea", "CheckAim", "CheckContent" };
                string[] arr2 = { CheckDataRecordName, CheckBeginTime, CheckMan, CheckDataType, CheckLevel, hidstr, se.AreaName,se.Aim,se.Remark };

                //���뵽��ǩ
                db.MoveToBookmark("HTML");
                db.InsertHtml(projectItem.Replace(@"\", "").TrimStart('"').TrimEnd('"'));
                //doc.MailMerge.Execute(dt);
                doc.MailMerge.Execute(arr, arr2);

                //�����ļ���
                string fileName = "��ȫ����¼_"+DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                doc.Save(Server.MapPath("~/Resource/Temp/" + fileName));
                string path = Server.MapPath("~/Resource/Temp/" + fileName);

                return Success("�����ɹ�", fileName);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }

        /// <summary>
        /// ר��������ҳ�浼��
        /// </summary>
        public ActionResult ExportDetailsZX(string keyValue, string ctype, string projectItem, string entity)
        {
            try
            {
                projectItem = HttpUtility.UrlDecode(projectItem);
                entity = HttpUtility.UrlDecode(entity);
                var watch = CommonHelper.TimerStart();
                SaftyCheckDataRecordEntity se = Newtonsoft.Json.JsonConvert.DeserializeObject<SaftyCheckDataRecordEntity>(entity);
                //���ص���ģ��
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/Temp/html2.docx"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);

                DataItemBLL dataItemBLL = new DataItemBLL();
                var lstItems = dataItemBLL.GetList();
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                string id = lstItems.Where(a => a.ItemCode == "SaftyCheckType").FirstOrDefault().ItemId;
                string saftyCheckType = dataItemDetailBLL.GetList(id).Where(a => a.ItemValue == se.CheckDataType.ToString()).FirstOrDefault().ItemName;

                string ids = lstItems.Where(a => a.ItemCode == "SaftyCheckLevel").FirstOrDefault().ItemId;
                string checkLevel = "ʡ��˾��ȫ���";
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
                //���뵽��ǩ
                doc.MailMerge.Execute(arr, arr2);
                db.MoveToBookmark("HTML");
                db.InsertHtml(projectItem.Replace(@"\", "").TrimStart('"').TrimEnd('"'));

                //�����ļ���
                string fileName ="��ȫ���ƻ�_"+ DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                doc.Save(Server.MapPath("~/Resource/Temp/" + fileName));
                string path = Server.MapPath("~/Resource/Temp/" + fileName);

                return Success("�����ɹ�", fileName);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// �鿴���鵼��
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
                //���ص���ģ��
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/Temp/html3.docx"));
                Aspose.Words.DocumentBuilder db = new Aspose.Words.DocumentBuilder(doc);

                DataItemBLL dataItemBLL = new DataItemBLL();
                var lstItems = dataItemBLL.GetList();
                DataItemDetailBLL dataItemDetailBLL = new DataItemDetailBLL();
                string id = lstItems.Where(a => a.ItemCode == "SaftyCheckType").FirstOrDefault().ItemId;
                string saftyCheckType = dataItemDetailBLL.GetList(id).Where(a => a.ItemValue == se.CheckDataType.ToString()).FirstOrDefault().ItemName;

                string ids = lstItems.Where(a => a.ItemCode == "SaftyCheckLevel").FirstOrDefault().ItemId;
                string checkLevel = "ʡ��˾��ȫ���";
                if (se.CheckLevel != "0")
                {
                    checkLevel = dataItemDetailBLL.GetList(ids).Where(a => a.ItemValue == se.CheckLevel.ToString()).FirstOrDefault().ItemName;
                }
                else
                {
                    ids = lstItems.Where(a => a.ItemCode == "SuperiorCheckLevel").FirstOrDefault().ItemId;
                    checkLevel = dataItemDetailBLL.GetList(ids).Where(a => a.ItemValue == se.SJCheckLevel.ToString()).FirstOrDefault().ItemName;
                }

                //����������
                IEnumerable<HTBaseInfoEntity> hidentity = htbaseinfobll.GetList("").Where(a => a.SAFETYCHECKOBJECTID == keyValue);
                string hidstr = "";
                int i = 1;
                string HidRankids = dataItemBLL.GetList().Where(a => a.ItemCode == "HidRank").FirstOrDefault().ItemId;
                foreach (HTBaseInfoEntity item in hidentity)
                {
                    if (!string.IsNullOrEmpty(item.HIDPOINTNAME))
                    {
                        hidstr += i + "����" + item.HIDPOINTNAME + "�� (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == item.HIDRANK).FirstOrDefault().ItemName + ")" + item.HIDDESCRIBE + "\r\n";
                    }
                    else
                    {
                        hidstr += i + "�� (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == item.HIDRANK).FirstOrDefault().ItemName + ")" + item.HIDDESCRIBE + "\r\n";
                    }
                    i++;
                }
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DataTable dt = new DepartmentBLL().GetDataTable(string.Format("select count(1) from  base_appsettingassociation a where a.deptid='{0}' and a.moduleid in(select id from BASE_MENUCONFIG t where t.modulecode='PECCANCY_ADD')", user.OrganizeId));
                if(dt.Rows[0][0].ToString()!="0")
                {
                    HidRankids = dataItemBLL.GetList().Where(a => a.ItemCode == "LllegalLevel").FirstOrDefault().ItemId;
                    //Υ��
                    LllegalRegisterBLL reg = new LllegalRegisterBLL();
                    var illList = reg.GetList(" and reseverone='" + keyValue + "'");
                    foreach (LllegalRegisterEntity ill in illList)
                    {
                        var sd = sdbll.GetEntity(ill.RESEVERTWO);
                        string remark = "Υ��";
                        if (sd != null)
                        {
                            remark = sd.CheckObject;
                        }

                        if (!string.IsNullOrEmpty(ill.LLLEGALLEVEL))
                        {
                            hidstr += i + "����" + remark + "�� (" + dataItemDetailBLL.GetList(HidRankids).Where(a => a.ItemDetailId == ill.LLLEGALLEVEL).FirstOrDefault().ItemName + ")" + ill.LLLEGALDESCRIBE + "\r\n";
                        }
                        else
                        {
                            hidstr += i + "����" + remark + "��" + ill.LLLEGALDESCRIBE + "\r\n";
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
                       hidstr += i + "�������⡿" + dr[0].ToString() + "\r\n";
                       i++;
                   }
               }
              
                //dt.Rows.Add(dr);

                string[] arr = { "CheckDataRecordName", "CheckBeginTime", "CheckManageMan", "CheckDept", "Ctype", "CheckDataType", "CheckLevel", "SolvePersonName", "CheckHid", "CheckArea", "CheckAim", "CheckContent" };
                string[] arr2 = { se.CheckDataRecordName, CheckDate, se.CheckManageMan, se.CheckDept, ctype, saftyCheckType, checkLevel, se.SolvePersonName, hidstr,se.AreaName,se.Aim,se.Remark };
                //���뵽��ǩ
                db.MoveToBookmark("HTML");
                db.InsertHtml(projectItem.Replace(@"\", "").TrimStart('"').TrimEnd('"'));
                doc.MailMerge.Execute(arr, arr2);
                //�����ļ���
                string fileName = "��ȫ����¼_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                doc.Save(Server.MapPath("~/Resource/Temp/" + fileName));
                string path = Server.MapPath("~/Resource/Temp/" + fileName);

                return Success("�����ɹ�", fileName);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }

        }
        /// <summary>
        /// ����
        /// </summary>
        [HttpGet]
        public ActionResult DowlodeData()
        {
            string path = Path.Combine(GlobalUtil.TemplatePath.LocalPath, "myxls.xls");
            GlobalUtil.DownLoadFile(path, true);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = srbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
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
        /// ���ݲ���CODE��ȡ������Ա����
        /// </summary>
        /// <param name="Encode">����Code</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetPeopleByEncode(string Encode)
        {
            var data = srbll.GetPeopleByEncode(Encode);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡʵ��(׷�Ӹ�������¼�������Ա) 
        /// </summary>
        /// <param name="recid">����ֵ</param>
        /// <returns>���ض���Json</returns>
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
        /// �ύ�ǼǼ��Ľ�� 
        /// </summary>
        /// <param name="projectItem">�����</param>
        /// <returns>���ض���Json</returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult SaveResult(string projectItem)
        {
            projectItem = HttpUtility.UrlDecode(projectItem);
            List<SaftyCheckDataDetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
            sdbll.SaveResultForm(list);
            return Success("�����ɹ���");
        }


        #region ��ҳ�����б�
        /// <summary>
        /// ��ҳ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>   
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
                if(user.RoleName.Contains("����")|| user.RoleName.Contains("ʡ���û�"))
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
                else if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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
                if (user.RoleName.Contains("����") || user.RoleName.Contains("ʡ���û�"))
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
                else if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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
                if (user.RoleName.Contains("����") || user.RoleName.Contains("ʡ���û�"))
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
                else if (user.RoleName.Contains("��˾") || user.RoleName.Contains("����"))
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
        /// ���ƻ��Ƿ��е�ǰ��¼�� 
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

        #region �ύ����
        /// <summary>
        /// ���ĵǼ���
        /// </summary>
        [HttpGet]
        public ActionResult RegisterPer(string userAccount, string id)
        {
            srbll.RegisterPer(userAccount, id);
            return Success("�ɹ���");
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(6, "ɾ������¼")]
        public ActionResult RemoveForm(string keyValue)
        {
            srbll.RemoveForm(keyValue);
            
            return Success("ɾ���ɹ���");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "����������ִ������״̬")]
        public ActionResult SetStatus(string keyValue,int status=0)
        {
            try
            {
                new SaftyCheckDataBLL().SetStatus(keyValue, status);
                return Success("�����ɹ���");
            }
            catch(Exception ex)
            {
                return Success(ex.Message);
            }
          
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="projectItem">�����Ŀ</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AjaxOnly]
        [HandlerMonitor(5, "���������޸ļ���¼")]
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
            //���氲ȫ����¼��
            string recid = "";
            int count = srbll.SaveForm(keyValue, entity, ref recid);
            //���氲ȫ������Ŀ
            if (count > 0 && projectItem.Length > 2)
            {
                if (sdbll.Remove(entity.ID) >= 0)
                {
                List<SaftyCheckDataDetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
                sdbll.SaveForm(entity.ID, list);
                }
            }
            return Success("�����ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="projectItem">�����Ŀ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "���������޸ļ���¼")]
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
           
            return Success("�����ɹ���", list);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5, "���������޸ļ���¼")]
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
                //���氲ȫ������Ŀ
                if (!string.IsNullOrWhiteSpace(projectItem))
                {
                    if (sdbll.Remove(recId) >= 0)
                    {
                        List<SaftyCheckDataDetailEntity> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaftyCheckDataDetailEntity>>(projectItem);
                        sdbll.SaveForm(recId, list);
                    }
                }
            }
            return Success("�����ɹ���");
        }
        [HttpPost]
        [AjaxOnly]
        [HandlerMonitor(5, "���������޸ļ���¼")]
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
                return Success("�����ɹ�");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        
        /// <summary>
        /// ���浽ר��������ƶ����ƻ���ʱ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <param name="projectItem">�����Ŀ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [HandlerMonitor(5,"������ƻ�")]
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
                //���氲ȫ����
                string recid = "";
                if (!string.IsNullOrEmpty(entity.CheckUserIds))
                {
                    entity.CheckUserIds = entity.CheckUserIds.Trim(',');
                }
                //if (entity.DataType == 1 && entity.IsSubmit == 1)
                //{
                if (string.IsNullOrWhiteSpace(mode))
                {
                    if (user.RoleName.Contains("ʡ��") || user.RoleName.Contains("��˾��"))
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
                            //�����ڼ�������
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
            return Success("�����ɹ���");
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
                //���氲ȫ����
                string recid = "";
                if (!string.IsNullOrEmpty(entity.CheckUserIds))
                {
                    entity.CheckUserIds = entity.CheckUserIds.Trim(',');
                }
                //if (entity.DataType == 1 && entity.IsSubmit == 1)
                //{
                if (string.IsNullOrWhiteSpace(mode))
                {
                    if (user.RoleName.Contains("ʡ��") || user.RoleName.Contains("��˾��"))
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
                            //�����ڼ�������
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
            return Success("�����ɹ���");
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
            node.name = "��������<br />("+deptname+")";
            node.type = "startround";
            node.setInfo = new setInfo
            {
                Taged=1,
                NodeDesignateData = new List<NodeDesignateData>{
                 new NodeDesignateData{
                   creatdept=deptname,
                   createuser=root.CreateUserName,
                   createdate=root.CreateDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                   status="�Ѵ���"
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
                node.name = "�ֽ�����<br />(" + deptname + ")";
                node.setInfo = new setInfo
                {
                    Taged=dr["status"].ToString()=="0"?0:1,
                    NodeDesignateData = new List<NodeDesignateData>{
                       new NodeDesignateData{
                         creatdept=deptname,
                          createuser=GetUsers(dr["deptid"].ToString(),dr["accounts"].ToString(),dr["names"].ToString()),
                          createdate=dr["CreateDate"].ToString(),
                          status=dr["status"].ToString()=="0"?"������":"�Ѵ���"
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
                    node1.name = "�ֽ�����<br />("+dr["checkeddepart"].ToString()+"-"+ deptname + ")";
                    node1.setInfo = new setInfo
                    {
                        Taged = row["status"].ToString() == "0" ? 0 : 1,
                        NodeDesignateData = new List<NodeDesignateData>{
                       new NodeDesignateData{
                         creatdept=deptname,
                         createuser=GetUsers(row["deptid"].ToString(),row["accounts"].ToString(),row["names"].ToString()),
                         createdate=row["CreateDate"].ToString(),
                         status=row["status"].ToString()=="0"?"������":"�Ѵ���"
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
                        node2.name = "�ֽ�����<br />(" + deptname + ")";
                        node2.setInfo = new setInfo
                        {
                            Taged = row1["status"].ToString() == "0" ? 0 : 1,
                            NodeDesignateData = new List<NodeDesignateData>{
                       new NodeDesignateData{
                         creatdept=deptname,
                         createuser=GetUsers(row1["deptid"].ToString(),row1["accounts"].ToString(),row1["names"].ToString()),
                         createdate=row1["CreateDate"].ToString(),
                         status=row1["status"].ToString()=="0"?"������":"�Ѵ���"
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
            flow.title = "��������������";
            return Success("��ȡ���ݳɹ�", flow);
        }
        #endregion

        #region ��ҳԤ��
        /// <summary>
        /// ��ȫ���Ԥ��
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
        /// ��ȫ���Ԥ��
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
        /// ��ȫ���Ԥ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateCheckUsers()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (!user.IsSystem)
            {
                return Error("�Բ���,��û��Ȩ�޽��д˲�����");
            }
            try
            {
               srbll.UpdateCheckUsers();
               return Success("�����ɹ�");
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        #endregion

        /// ����ѹ��ͼƬ  
        /// <param name="sFile">ԭͼƬ</param>  
        /// <param name="dFile">ѹ���󱣴�λ��</param>  
        /// <param name="dHeight">�߶�</param>  
        /// <param name="dWidth"></param>  
        /// <param name="flag">ѹ������(����ԽСѹ����Խ��) 1-100</param>  
        /// <returns></returns>  

        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;

            //����������
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
            //���´���Ϊ����ͼƬʱ������ѹ������  
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//����ѹ���ı���1-100  
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
                    ob.Save(dFile, jpegICIinfo, ep);//dFile��ѹ�������·��  
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
        /// ��������¼���ܱ����������
        /// </summary>
        /// <param name="checkId">����¼ID</param>
        /// <param name="checkType">������ͣ�0�����ճ���飬1���ճ����,2�������Ų飩</param>
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
                //��ȫ������
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
                if (checkType == 2)  //�������
                {
                    mode = 1;
                    HTBaseInfoEntity htentity = htbaseinfobll.GetEntity(checkId);
                    checkDept = htentity.CHECKDEPARTNAME;
                    checkUser = htentity.CHECKMANNAME;
                    checkarea = htentity.HIDPOINTNAME;
                    checkTime = htentity.CHECKDATE.Value.ToString("yyyy.MM.dd");
                }
              
                Aspose.Words.Document doc = new Aspose.Words.Document(Server.MapPath("~/Resource/DocumentFile/��ȫ���������.doc"));
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
                return Success("�����ɹ�", filename);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            
        }
    }
}
