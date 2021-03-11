using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using ERCHTMS.Code;
using System.Linq;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.AuthorizeManage;
using Aspose.Cells;
using System.Drawing;
using ERCHTMS.Busines.HighRiskWork;
using ERCHTMS.Busines.BaseManage;
using System.Threading.Tasks;
using ERCHTMS.Entity.SystemManage;
using System.Net;
using System.Text;
using System.IO;
using ERCHTMS.Busines.FlowManage;
using ERCHTMS.Entity.FlowManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Dynamic;
using System.Web;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� �������������Ϣ��
    /// </summary>
    public class OutsouringengineerController : MvcControllerBase
    {
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private OutsourcingprojectBLL outProjectbll = new OutsourcingprojectBLL();
        private AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private ModuleListColumnAuthBLL modulelistcolumnauthbll = new ModuleListColumnAuthBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private UserBLL userbll = new UserBLL();
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
        /// ͳ����ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Statistics()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��ת
        /// </summary>
        /// <returns></returns>
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpGet]
        public ActionResult IndexToList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }
         [HttpGet]
        public ActionResult  LevelExplain(){
            return View();
        }


        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FlowDesign()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FlowNodeForm()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        [HttpGet]
        public string OutEngineerStat(string deptid, string year = "")
        {
            return outsouringengineerbll.GetTypeCount(deptid, year);
        }
        [HttpGet]
        public string GetTypeList(string deptid, string year = "")
        {
            return outsouringengineerbll.GetTypeList(deptid, year);
        }
        [HttpGet]
        public string GetStateCount(string deptid, string year = "")
        {
            return outsouringengineerbll.GetStateCount(deptid, year);
        }
        [HttpGet]
        public string GetStateList(string deptid, string year = "")
        {
            return outsouringengineerbll.GetStateList(deptid, year);
        }
        /// <summary>
        /// ���ݵ�¼��id ��ѯ����(�Ѿ�ͨ���������Ĺ���)
        /// �����û���ѯȫ������
        /// �������Ų�ѯ������ĳа����µĹ���
        /// �а��̲�ѯ�Լ��Ĺ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEngineerDataByCurrdeptId(string mode = "")
        {
            Operator currUser = OperatorProvider.Provider.Current();
            DataTable dt = outsouringengineerbll.GetEngineerDataByCurrdeptId(currUser, mode);
            return ToJsonResult(dt);
        }

        /// <summary>
        /// ���ݵ�¼��id ��ѯ�Ѿ��ڽ��Ĺ���(�Ѿ�ͨ����������Ĺ���)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOnTheStock()
        {

            Operator currUser = OperatorProvider.Provider.Current();
            DataTable dt = outsouringengineerbll.GetOnTheStock(currUser);
            return ToJsonResult(dt);
        }
        /// <summary>
        /// ���ݲ���ID��ȡ�������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEngineerByDeptId(string deptId)
        {
            List<OutsouringengineerEntity> engList = outsouringengineerbll.GetList().Where(x => x.OUTPROJECTID == deptId).ToList();
            return ToJsonResult(engList);
        }

        /// <summary>
        /// ���ݲ���ID��ȡ�ڽ����������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //�༭��������ʾ�ڽ�,������ʾ�������й���״̬
        public ActionResult GetProjectByDeptId(string deptId, string stype)
        {
            List<OutsouringengineerEntity> engList = new List<OutsouringengineerEntity>();
            Operator currUser = OperatorProvider.Provider.Current();
            if (currUser.RoleName.Contains("����") || currUser.RoleName.Contains("��˾"))
            {

                engList = outsouringengineerbll.GetList().Where(x => x.OUTPROJECTID == deptId && x.ENGINEERSTATE == "002").ToList();
            }
            else if (currUser.RoleName.Contains("�а���") || currUser.RoleName.Contains("�ְ���"))
            {

                engList = outsouringengineerbll.GetList().Where(x => x.ID == currUser.ProjectID && x.ENGINEERSTATE == "002").ToList();
            }
            else
            {
                engList = outsouringengineerbll.GetList().Where(x => x.OUTPROJECTID == deptId && x.ENGINEERSTATE == "002" && x.ENGINEERLETDEPTID == currUser.DeptId).ToList();
            }
            return ToJsonResult(engList);
        }

        /// <summary>
        /// ���ݵ�λid ��ѯ�Ѿ��ڽ��Ĺ���(�Ѿ�ͨ����������Ĺ���)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEngineerDataByWBId(string deptId,string mode="")
        {
           
            DataTable dt = outsouringengineerbll.GetEngineerDataByWBId(deptId, mode);
            return ToJsonResult(dt);
        }

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
                pagination.p_kid = "e.ID";
                pagination.p_fields = @"  e.usedeptpeople, e.engineerusedept,e.usedeptpeopphone,
	                                        e.engineerdirector,e.engineerdirectorphone,e.engineerletdeptid,
                                           e.engineercode,b.senddeptid,b.senddeptname,e.engineerletdept,e.engineerletpeople,e.engineerletpeoplephone,
                                           e.engineername,d.itemname engineertype,l.itemname engineerlevel,
                                           e.outprojectid,e.planenddate,e.actualenddate,e.predicttime,
                                           s.itemvalue statecode,s.itemname engineerstate,
                                           e.createdate,b.fullname outprojectname,e.engineerarea,
                                            decode(ss.examstatus, '1', '', '��λ�������') examstatus,
                                           decode(ss.pactstatus, '1', '', 'Э�����') pactstatus,
                                           decode(ss.compactstatus, '1', '', '��ͬ����') compactstatus,
                                           decode(ss.threetwostatus, '1', '', '��������') threetwostatus,
                                           decode(ss.technicalstatus, '1', '', '��ȫ��������') technicalstatus,
                                           decode(ss.equipmenttoolstatus, '1', '', '����������') equipmenttoolstatus,
                                            decode(ss.peoplestatus, '1', '', '��Ա�������') peoplestatus,
                                            i.busvalidstarttime,i.busvalidendtime,i.splvalidstarttime,e.createuserid,
                                            i.splvalidendtime,i.cqvalidstarttime,i.cqvalidendtime,e.isdeptadd ";
                pagination.p_tablename = @"epg_outsouringengineer e
                                          left join base_department b on b.departmentid=e.outprojectid
                                          left join (select * from (select busvalidstarttime,outengineerid,busvalidendtime,
                                                                           splvalidstarttime,splvalidendtime,cqvalidstarttime,cqvalidendtime,
                                                                           row_number() over(partition by outengineerid order by createdate desc) rn
                                                          from epg_aptitudeinvestigateinfo)
                                                 where rn = 1) i on i.outengineerid = e.id
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate
                                          left join epg_startappprocessstatus ss on ss.outengineerid=e.id";
                pagination.sidx = "e.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                var queryParam = queryJson.ToJObject();
                if (queryParam["showview"].IsEmpty())//��վ�ල�в���Ҫ�������β���,ֻ�������ҵ��λ������,�ʼӴ��ж�
                {
                    if (currUser.IsSystem)
                    {
                        pagination.conditionJson = "  1=1 ";
                    }
                    else if (currUser.RoleName.Contains("ʡ��"))
                    {
                        pagination.conditionJson = string.Format(@" e.createuserorgcode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", currUser.NewDeptCode);
                    }
                    else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
                    { 
                        pagination.conditionJson = string.Format(" e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
                    }
                    else if (currUser.RoleName.Contains("�а��̼��û�"))
                    {
                        pagination.conditionJson = string.Format("  (e.outprojectid ='{0}' or e.supervisorid='{0}')", currUser.DeptId);
                    }
                    else
                    {
                        var deptentity = departmentbll.GetEntity(currUser.DeptId);
                        while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                        {
                            deptentity = departmentbll.GetEntity(deptentity.ParentId);
                        }
                        pagination.conditionJson = string.Format(" e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') ", deptentity.EnCode, currUser.UserId);

                        //pagination.conditionJson = string.Format("  e.engineerletdeptid ='{0}'", currUser.DeptId);
                    }
                }
                else
                {
                    pagination.conditionJson = "  1=1 ";
                }
                var data = outsouringengineerbll.GetPageList(pagination, queryJson);


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

                throw new Exception(ex.Message);
            }

        }
        /// <summary>
        /// ��ѯ�ѿ������������
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEngineerSelect(string queryJson)
        {

            //return ToJsonResult(dt);
            try
            {
                Pagination pagination = new Pagination();
                pagination.page = 1;
                pagination.rows = 100000000;
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "e.ID";
                pagination.p_fields = @"  e.usedeptpeople, e.engineerusedept,e.usedeptpeopphone,
	                                        e.engineerdirector,e.engineerdirectorphone,e.engineerletdeptid,
                                           e.engineercode,b.senddeptid,b.senddeptname,e.engineerletdept,e.engineerletpeople,e.engineerletpeoplephone,
                                           e.engineername,d.itemname engineertype,l.itemname engineerlevel,
                                           e.outprojectid,e.planenddate,e.actualenddate,e.predicttime,
                                           s.itemvalue statecode,s.itemname engineerstate,
                                           e.createdate,b.fullname outprojectname,e.engineerarea,
                                            decode(ss.examstatus, '1', '', '��λ�������') examstatus,
                                           decode(ss.pactstatus, '1', '', 'Э�����') pactstatus,
                                           decode(ss.compactstatus, '1', '', '��ͬ����') compactstatus,
                                           decode(ss.threetwostatus, '1', '', '��������') threetwostatus,
                                           decode(ss.technicalstatus, '1', '', '��ȫ��������') technicalstatus,
                                           decode(ss.equipmenttoolstatus, '1', '', '����������') equipmenttoolstatus,
                                            decode(ss.peoplestatus, '1', '', '��Ա�������') peoplestatus,
                                            i.busvalidstarttime,i.busvalidendtime,i.splvalidstarttime,e.createuserid,
                                            i.splvalidendtime,i.cqvalidstarttime,i.cqvalidendtime,e.isdeptadd ";
                pagination.p_tablename = @"epg_outsouringengineer e
                                          left join base_department b on b.departmentid=e.outprojectid
                                          left join (select * from (select busvalidstarttime,outengineerid,busvalidendtime,
                                                                           splvalidstarttime,splvalidendtime,cqvalidstarttime,cqvalidendtime,
                                                                           row_number() over(partition by outengineerid order by createdate desc) rn
                                                          from epg_aptitudeinvestigateinfo)
                                                 where rn = 1) i on i.outengineerid = e.id
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate
                                          left join epg_startappprocessstatus ss on ss.outengineerid=e.id";
                pagination.sidx = "e.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                var queryParam = queryJson.ToJObject();
                if (queryParam["showview"].IsEmpty())//��վ�ල�в���Ҫ�������β���,ֻ�������ҵ��λ������,�ʼӴ��ж�
                {
                    if (currUser.IsSystem)
                    {
                        pagination.conditionJson = "  1=1 ";
                    }
                    else if (currUser.RoleName.Contains("ʡ��"))
                    {
                        pagination.conditionJson = string.Format(@" e.createuserorgcode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", currUser.NewDeptCode);
                    }
                    else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
                    {
                        pagination.conditionJson = string.Format(" e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
                    }
                    else if (currUser.RoleName.Contains("�а��̼��û�"))
                    {
                        pagination.conditionJson = string.Format("  (e.outprojectid ='{0}' or e.supervisorid='{0}')", currUser.DeptId);
                    }
                    else
                    {
                        var deptentity = departmentbll.GetEntity(currUser.DeptId);
                        while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                        {
                            deptentity = departmentbll.GetEntity(deptentity.ParentId);
                        }
                        pagination.conditionJson = string.Format(" e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') ", deptentity.EnCode, currUser.UserId);

                        //pagination.conditionJson = string.Format("  e.engineerletdeptid ='{0}'", currUser.DeptId);
                    }
                }
                else
                {
                    pagination.conditionJson = "  1=1 ";
                }
                var data = outsouringengineerbll.GetPageList(pagination, queryJson);
                return ToJsonResult(data);
            }
            catch (System.Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// ��ҳ��תҳ���ѯ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HandlerAuthorize(PermissionMode.Ignore)]
        [HttpGet]
        public ActionResult GetIndexToList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "e.ID";
                pagination.p_fields = @" e.engineerletdept, e.engineerletdeptid,e.engineerletpeople,
                                               e.engineerletpeopleid,e.engineerletpeoplephone, e.engineerdirector,
                                               e.engineerdirectorphone,e.createdate,e.outprojectid,
                                               d.itemname engineertype,l.itemname engineerlevel,s.itemname engineerstate,
                                               e.engineername, e.id eid,b.fullname outprojectname,u.id unitid ";

                pagination.p_tablename = @"epg_outsouringengineer e
                                          left join base_department b on b.departmentid=e.outprojectid
                                          left join epg_outsourcingproject u on u.outprojectid=e.outprojectid
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate";
                var queryParam = queryJson.ToJObject();
                if (!queryParam["pageType"].IsEmpty())
                {
                    switch (queryParam["pageType"].ToString())
                    {
                        case "1"://��֤��
                            pagination.p_fields += string.Format(" ,decode(m.id,'','δ����','�ѽ���') mid,decode(m.paymentmoney,'',0,m.paymentmoney) paymentmoney ");
                            pagination.p_tablename += string.Format(" left join epg_safetyeamestmoney m on m.projectid = e.id ");
                            break;
                        case "2"://��ͬ
                            pagination.p_fields += string.Format(" ,decode(c.id,'','δǩ��','��ǩ��') cid ");
                            pagination.p_tablename += string.Format(" left join epg_compact c on c.projectid = e.id ");
                            break;
                        case "3"://Э��
                            pagination.p_fields += string.Format(" ,decode(p.id,'','δǩ��','��ǩ��') pid");
                            pagination.p_tablename += string.Format("  left join epg_protocol p on p.projectid = e.id");
                            break;
                        case "4"://��ȫ��������
                            pagination.p_fields += string.Format(" , decode(k.id,'','δ���а�ȫ��������','�ѽ���') kid ");
                            pagination.p_tablename += string.Format(" left join epg_techdisclosure k on k.projectid = e.id ");
                            break;
                        default:
                            break;
                    }
                }
                pagination.sidx = "e.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                else if (currUser.RoleName.Contains("ʡ��"))
                {
                    pagination.conditionJson = string.Format(@" e.createuserorgcode  in (select encode
                        from base_department d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", currUser.NewDeptCode);
                }
                else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
                {
                    pagination.conditionJson = string.Format(" e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
                }
                else if (currUser.RoleName.Contains("�а��̼��û�"))
                {
                    pagination.conditionJson = string.Format("  e.outprojectid ='{0}'", currUser.DeptId);
                }
                else
                {
                    pagination.conditionJson = string.Format("  e.engineerletdeptid='{0}'", currUser.DeptId);
                }
                pagination.conditionJson += string.Format(@" and e.id in (select distinct (t.outengineerid)
                          from epg_aptitudeinvestigateinfo t
                         where t.isauditover = 1)", currUser.DeptId);
                var data = outsouringengineerbll.GetIndexToList(pagination, queryJson);


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

                throw new Exception(ex.Message);
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
            var data = outsouringengineerbll.GetList(queryJson);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ����������������б�
        /// </summary>
        /// <param name="orgCode">����code</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetIsDeptAddList(string orgCode)
        {
            var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            if (currUser.RoleName.Contains("����") || currUser.RoleName.Contains("��˾��"))
            {
                var data = outsouringengineerbll.GetList("").Where(x => x.IsDeptAdd == 0 && x.CREATEUSERORGCODE == orgCode&&x.ENGINEERSTATE=="001");
                return ToJsonResult(data);
            }
            else if (currUser.RoleName.Contains("�а���"))
            {
                var data = outsouringengineerbll.GetList("").Where(x => x.IsDeptAdd == 0 && x.CREATEUSERORGCODE == orgCode && x.OUTPROJECTID == currUser.DeptId && x.ENGINEERSTATE == "001");
                return ToJsonResult(data);
            }
            else
            {
                var data = outsouringengineerbll.GetList("").Where(x => x.IsDeptAdd == 0 && x.CREATEUSERORGCODE == orgCode && x.ENGINEERLETDEPTID == currUser.DeptId && x.ENGINEERSTATE == "001");
                return ToJsonResult(data);
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
            var disBll = new DistrictBLL();
            var didBll = new DataItemDetailBLL();
            var data = outsouringengineerbll.GetEntity(keyValue);
            if (data != null)
            {
                if (!string.IsNullOrWhiteSpace(data.ENGINEERTYPE))
                {
                    var listType = didBll.GetDataItem("ProjectType", data.ENGINEERTYPE).ToList();
                    if (listType != null && listType.Count > 0)
                        data.ENGINEERTYPENAME = listType[0].ItemName;
                }
                if (!string.IsNullOrWhiteSpace(data.ENGINEERLEVEL))
                {
                    var listLevel = didBll.GetDataItem("ProjectLevel", data.ENGINEERLEVEL).ToList();
                    if (listLevel != null && listLevel.Count > 0)
                        data.ENGINEERLEVELNAME = listLevel[0].ItemName;
                }
                var proData = outProjectbll.GetOutProjectInfo(data.OUTPROJECTID);
                var SupervisorData = outProjectbll.GetOutProjectInfo(data.SupervisorId);
                var zzinfo = aptitudeinvestigateinfobll.GetEntityByOutEngineerId(keyValue);//��λ�������
                var ppinfo = peoplereviewbll.GetList("").Where(x => x.OUTENGINEERID == keyValue && x.ISAUDITOVER == "1").OrderByDescending(x => x.CREATEDATE).ToList().FirstOrDefault();//��Ա�������
                if (ppinfo == null) ppinfo = new PeopleReviewEntity() { ID = Guid.NewGuid().ToString() };
                if (zzinfo == null) zzinfo = new AptitudeinvestigateinfoEntity() { ID = Guid.NewGuid().ToString() };
                //var peopleData=
                var resultData = new
                {
                    data = data,
                    proData = proData,
                    zzinfo = zzinfo,
                    ppinfo = ppinfo,
                    SupervisorData = SupervisorData
                };
                return ToJsonResult(resultData);
            }
            else
            {
                return Error("����ʧ��");
            }
            //if (!string.IsNullOrWhiteSpace(data.ENGINEERAREA))
            //{
            //    var area = disBll.GetEntity(data.ENGINEERAREA);
            //    if (area != null)
            //        data.ENGINEERAREANAME = area.DistrictName;
            //}
            
        }
        [HttpGet]
        public ActionResult GetCompactNo(string keyValue)
        {
            var htnum = "";
            var list = new CompactBLL().GetListByProjectId(keyValue).OrderBy(x => x.CREATEDATE).ToList();
            if (list.Count > 0)
            {
                htnum = list.FirstOrDefault().COMPACTNO;
            }
            return ToJsonResult(htnum);

        }
        [HttpGet]
        public ActionResult GetEngineerEntity(string keyValue)
        {
            var data = outsouringengineerbll.GetEntity(keyValue);
            var proData = outProjectbll.GetOutProjectInfo(data.OUTPROJECTID);
            var resultData = new
            {
                data = data,
                proData = proData,

            };
            return ToJsonResult(resultData);
        }

        /// <summary>
        /// ���ݵ�ǰ��¼�� ��ȡ�Ѿ�ͣ���Ĺ�����Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetStopEngineerList()
        {
            var data = outsouringengineerbll.GetStopEngineerList();
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��������״̬ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetFlowData(string keyValue)
        {
            var josnData = outsouringengineerbll.GetFlow(keyValue);
            return Content(josnData.ToJson());
        }

        /// <summary>
        /// ���ݵ�ǰ��¼�˻�ȡ�����б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEngineerByCurrDept()
        {
            var data = outsouringengineerbll.GetEngineerByCurrDept();
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
            outsouringengineerbll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, OutsouringengineerEntity entity)
        {
            try
            {
                var currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                DataItemDetailBLL di = new DataItemDetailBLL();
                //�û�����а���ʱ�Զ������а���
                if (entity.IsDeptAdd == 1 && entity.OUTPROJECTID == null)
                {
                    DepartmentEntity until = new DepartmentEntity();
                    until.DepartmentId = Guid.NewGuid().ToString();
                    until.FullName = entity.OUTPROJECTNAME;
                    //until.SendDeptID=entity.ENGINEERLETDEPTID;
                    //until.SendDeptName=entity.ENGINEERLETDEPT;
                    until.Nature = "�а���";
                    until.EnabledMark = 0;
                    until.OrganizeId = currUser.OrganizeId;
                    until.IsOrg = 0;
                    until.IsTrain = 0;
                    until.IsTools = 0;
                    until.DeptType = entity.DeptType;
                    var temp = new DepartmentBLL().GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == entity.OUTPROJECTNAME).ToList().FirstOrDefault();
                    if (temp == null) //�ж�ϵͳ���Ƿ���ͬ���������λ�����û�����½�
                    {
                        var parent = new DepartmentBLL().GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.Description == "������̳а���").ToList().FirstOrDefault();
                        if (parent == null)
                        {
                            return Error("�õ糧��û�га��̽ڵ㡣");
                        }
                        else
                        {
                            until.ParentId = parent.DepartmentId;
                        }
                        until.SortCode = new DepartmentBLL().GetList().Where(x => x.Nature == "�а���" && x.OrganizeId == currUser.OrganizeId).ToList().Count + 1;
                        new DepartmentBLL().SaveForm(until.DepartmentId, until);

                        #region ͬ��������ƽ̨
                        DepartmentEntity dept = departmentbll.GetEntity(until.DepartmentId);
                        if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                        {
                            if (string.IsNullOrWhiteSpace(until.ParentId))
                            {
                                until.ParentId = "-1";
                            }
                            var task = Task.Factory.StartNew(() =>
                            {
                                List<DepartmentEntity> lstDepts = new List<DepartmentEntity>();
                                lstDepts.Add(until);
                                SaveDept(lstDepts, currUser.Account);
                            });

                        }
                        if (until.IsTrain == 1)
                        {
                            var task = Task.Factory.StartNew(() =>
                            {
                                departmentbll.SyncDept(until, keyValue);
                            });

                        }
                        if (until.IsTools == 1 && !string.IsNullOrWhiteSpace(until.ToolsKey))
                        {
                            string[] arr = until.ToolsKey.Split('|');
                            if (arr.Length == 3)
                            {
                                var task = Task.Factory.StartNew(() =>
                                {
                                    SyncToolsUsers(until, keyValue, arr);
                                });
                            }

                        }
                        DepartmentEntity org = departmentbll.GetEntity(until.OrganizeId);
                        if (org != null)
                        {
                            if (org.IsTrain == 1)
                            {
                                string way = di.GetItemValue("WhatWay");
                                //�Խ�.net��ѵƽ̨
                                if (way == "0")
                                {

                                }
                                //�Խ�java��ѵƽ̨
                                if (way == "1")
                                {
                                    string deptId = until.DepartmentId;
                                    dept = departmentbll.GetEntity(until.DepartmentId);
                                    if (dept != null)
                                    {
                                        string enCode = dept.EnCode;
                                        if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                        {
                                            string[] arr = dept.DeptKey.Split('|');
                                            if (arr.Length == 2)
                                            {
                                                deptId = arr[0];
                                                enCode = arr[1];
                                            }
                                        }
                                        string parentId = dept.ParentId;
                                        DepartmentEntity parentDept = departmentbll.GetEntity(parentId);

                                        if (!string.IsNullOrWhiteSpace(parentDept.DeptKey))
                                        {
                                            string[] arr = parentDept.DeptKey.Split('|');
                                            if (arr.Length == 2)
                                            {
                                                parentId = arr[0];
                                            }
                                        }

                                        string ModuleName = SystemInfo.CurrentModuleName;
                                        string ModuleId = SystemInfo.CurrentModuleId;
                                        Task.Factory.StartNew(() =>
                                        {
                                            if (dept != null)
                                            {
                                                object obj = new
                                                {
                                                    action = "add",
                                                    time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                    deptId = deptId,
                                                    deptCode = enCode,
                                                    deptName = dept.FullName,
                                                    parentId = parentId,
                                                    deptType = dept.Nature,
                                                    unitKind = dept.DeptType,
                                                    sort = dept.SortCode,
                                                    companyId = org.InnerPhone
                                                };
                                                List<object> list = new List<object>();
                                                list.Add(obj);
                                                Busines.JPush.JPushApi.PushMessage(list, 0);


                                                LogEntity logEntity = new LogEntity();
                                                logEntity.CategoryId = 5;
                                                logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                                                logEntity.OperateType = "�����򱣴�";
                                                logEntity.OperateAccount = currUser.Account + "��" + currUser.UserName + "��";
                                                logEntity.OperateUserId = currUser.UserId;

                                                logEntity.ExecuteResult = 1;
                                                logEntity.ExecuteResultJson = string.Format("ͬ�����ŵ�java��ѵƽ̨,ͬ����Ϣ��{0}", list.ToJson());
                                                logEntity.Module = ModuleName;
                                                logEntity.ModuleId = ModuleId;
                                                logEntity.WriteLog();
                                            }
                                        });
                                    }

                                }
                            }
                        }
                        #endregion

                        //�����ɹ���������λ��ֵ
                        entity.OUTPROJECTID = until.DepartmentId;
                    }
                    else
                    {
                        entity.OUTPROJECTID = temp.DepartmentId;
                    }
                    
                }
                if (string.IsNullOrEmpty(entity.SupervisorId) && !string.IsNullOrEmpty(entity.SupervisorName))
                {
                    DepartmentEntity until = new DepartmentEntity();
                    until.DepartmentId = Guid.NewGuid().ToString();
                    until.FullName = entity.SupervisorName;
                    until.Nature = "�а���";
                    until.EnabledMark = 0;
                    until.OrganizeId = currUser.OrganizeId;
                    until.IsOrg = 0;
                    until.IsTrain = 0;
                    until.IsTools = 0;
                    until.DeptType = "��ʱ";
                    var temp = new DepartmentBLL().GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.FullName == entity.OUTPROJECTNAME).ToList().FirstOrDefault();
                    if (temp == null) //�ж�ϵͳ���Ƿ���ͬ���������λ�����û�����½�
                    {
                        var parent = new DepartmentBLL().GetList().Where(x => x.OrganizeId == currUser.OrganizeId && x.Nature == "����" && x.Description == "������̳а���").ToList().FirstOrDefault();
                        if (parent == null)
                        {
                            return Error("�õ糧��û�га��̽ڵ㡣");
                        }
                        else
                        {
                            until.ParentId = parent.DepartmentId;
                        }
                        until.SortCode = new DepartmentBLL().GetList().Where(x => x.Nature == "�а���" && x.OrganizeId == currUser.OrganizeId).ToList().Count + 1;
                        new DepartmentBLL().SaveForm(until.DepartmentId, until);
                        #region ͬ��������ƽ̨
                        DepartmentEntity dept = departmentbll.GetEntity(until.DepartmentId);
                        if (!string.IsNullOrEmpty(di.GetItemValue("bzAppUrl")))
                        {
                            if (string.IsNullOrWhiteSpace(until.ParentId))
                            {
                                until.ParentId = "-1";
                            }
                            var task = Task.Factory.StartNew(() =>
                            {
                                List<DepartmentEntity> lstDepts = new List<DepartmentEntity>();
                                lstDepts.Add(until);
                                SaveDept(lstDepts, currUser.Account);
                            });

                        }
                        if (until.IsTrain == 1)
                        {
                            var task = Task.Factory.StartNew(() =>
                            {
                                departmentbll.SyncDept(until, keyValue);
                            });

                        }
                        if (until.IsTools == 1 && !string.IsNullOrWhiteSpace(until.ToolsKey))
                        {
                            string[] arr = until.ToolsKey.Split('|');
                            if (arr.Length == 3)
                            {
                                var task = Task.Factory.StartNew(() =>
                                {
                                    SyncToolsUsers(until, keyValue, arr);
                                });
                            }

                        }
                        DepartmentEntity org = departmentbll.GetEntity(until.OrganizeId);
                        if (org != null)
                        {
                            if (org.IsTrain == 1)
                            {
                                string way = di.GetItemValue("WhatWay");
                                //�Խ�.net��ѵƽ̨
                                if (way == "0")
                                {

                                }
                                //�Խ�java��ѵƽ̨
                                if (way == "1")
                                {
                                    string deptId = until.DepartmentId;
                                    dept = departmentbll.GetEntity(until.DepartmentId);
                                    if (dept != null)
                                    {
                                        string enCode = dept.EnCode;
                                        if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                        {
                                            string[] arr = dept.DeptKey.Split('|');
                                            if (arr.Length == 2)
                                            {
                                                deptId = arr[0];
                                                enCode = arr[1];
                                            }
                                        }
                                        string parentId = dept.ParentId;
                                        DepartmentEntity parentDept = departmentbll.GetEntity(parentId);

                                        if (!string.IsNullOrWhiteSpace(parentDept.DeptKey))
                                        {
                                            string[] arr = parentDept.DeptKey.Split('|');
                                            if (arr.Length == 2)
                                            {
                                                parentId = arr[0];
                                            }
                                        }

                                        string ModuleName = SystemInfo.CurrentModuleName;
                                        string ModuleId = SystemInfo.CurrentModuleId;
                                        Task.Factory.StartNew(() =>
                                        {
                                            if (dept != null)
                                            {
                                                object obj = new
                                                {
                                                    action = "add",
                                                    time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                    deptId = deptId,
                                                    deptCode = enCode,
                                                    deptName = dept.FullName,
                                                    parentId = parentId,
                                                    deptType = dept.Nature,
                                                    unitKind = dept.DeptType,
                                                    sort = dept.SortCode,
                                                    companyId = org.InnerPhone
                                                };
                                                List<object> list = new List<object>();
                                                list.Add(obj);
                                                Busines.JPush.JPushApi.PushMessage(list, 0);


                                                LogEntity logEntity = new LogEntity();
                                                logEntity.CategoryId = 5;
                                                logEntity.OperateTypeId = ((int)OperationType.Update).ToString();
                                                logEntity.OperateType = "�����򱣴�";
                                                logEntity.OperateAccount = currUser.Account + "��" + currUser.UserName + "��";
                                                logEntity.OperateUserId = currUser.UserId;

                                                logEntity.ExecuteResult = 1;
                                                logEntity.ExecuteResultJson = string.Format("ͬ�����ŵ�java��ѵƽ̨,ͬ����Ϣ��{0}", list.ToJson());
                                                logEntity.Module = ModuleName;
                                                logEntity.ModuleId = ModuleId;
                                                logEntity.WriteLog();
                                            }
                                        });
                                    }

                                }
                            }
                        }
                        #endregion
                        //�����ɹ��������λ��ֵ
                        entity.SupervisorId = until.DepartmentId;
                    }
                    else
                    {
                        entity.SupervisorId = temp.DepartmentId;
                    }
                        
                }
                outsouringengineerbll.SaveForm(keyValue, entity);

                return Success("�����ɹ���", new { outprojectid = entity.OUTPROJECTID, engineerid = entity.ID });
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
            
        }

        private void SaveDept(List<DepartmentEntity> list, string account)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Type", "application/json");
            //��������web api����ȡ����ֵ��Ĭ��Ϊpost��ʽ
            try
            {
                wc.UploadStringCompleted += wc_UploadStringCompleted;
                wc.UploadStringAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "PostDepartments "), "Post", list.ToJson());

            }
            catch (Exception ex)
            {
                //��ͬ�����д����־�ļ�
                string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��ͬ������ʧ�ܣ�ͬ����Ϣ" + list.ToJson() + ",�쳣��Ϣ��" + ex.Message + "\r\n");
            }
        }
        public void SyncToolsUsers(DepartmentEntity dept, string keyValue, string[] arr)
        {

            SstmsService.CheckHeader header = new SstmsService.CheckHeader();
            header.Key = arr[2];
            List<SstmsService.DictionaryEntry> dic = new List<SstmsService.DictionaryEntry>();
            SstmsService.DictionaryEntry de = new SstmsService.DictionaryEntry();
            de.Key = "LaterID";
            de.Value = arr[0];
            dic.Add(de);
            de = new SstmsService.DictionaryEntry();
            de.Key = "Identify";
            de.Value = "";
            dic.Add(de);
            // ʵ�����������
            SstmsService.DataServiceSoapClient service = new SstmsService.DataServiceSoapClient();
            SstmsService.PersonInfo[] data = service.GetPersons(header);
            //SstmsService.PersonInfo[] data = service.GetPersonsPage(header, dic.ToArray<SstmsService.DictionaryEntry>(), 1, 1000).List;
            DepartmentEntity org = departmentbll.GetEntity(dept.OrganizeId);
            IList<UserEntity> listUsers = new List<UserEntity>();
            string path = new DataItemDetailBLL().GetItemValue("imgPath");
            foreach (SstmsService.PersonInfo per in data)
            {
                string unitId = per.OwnerDeptID;
                string userId = "";
                string roleName = "�а��̼��û�";
                string roleId = "c5530ccf-e84e-4df8-8b27-fd8954a9bbe9";
                DataTable dt = departmentbll.GetDataTable(string.Format("select nature,deptid,a.deptcode,unitid from BIS_TOOLSDEPT a left join BASE_DEPARTMENT b on a.deptid=b.departmentid where unitid='{1}' and  a.deptcode like '{0}%'", dept.EnCode, per.UnitID));
                foreach (DataRow dr in dt.Rows)
                {
                    string deptId = dr["deptid"].ToString();
                    string deptCode = dr["deptcode"].ToString();
                    string nature = dr["nature"].ToString();
                    unitId = dr["unitid"].ToString();
                    if (unitId == per.UnitID)
                    {
                        switch (nature)
                        {
                            case "����":
                                roleName = "���ż��û�";
                                roleId = "6c094cef-cca3-4b41-a71b-6ee5e6b89008";
                                break;
                            case "רҵ":
                                roleName = "רҵ���û�";
                                roleId = "e3062d59-2484-4046-a420-478886d58656";
                                break;
                            case "����":
                                roleName = "���鼶�û�";
                                roleId = "d9432a6e-5659-4f04-9c10-251654199714";
                                break;
                            case "����":
                                roleName = "��˾���û�";
                                roleId = "aece6d68-ef8a-4eac-a746-e97f0067fab5";
                                break;
                            case "ʡ��":
                                roleName = "ʡ���û�";
                                roleId = "9a834c93-ff60-440e-845d-79b311eeacae";
                                break;
                        }
                        roleName += ",��ͨ�û�";
                        roleId += ",2a878044-06e9-4fe4-89f0-ba7bd5a1bde6";
                        UserEntity user = userbll.GetUserByIdCard(per.IdentifyID);
                        if (user == null)
                        {
                            //    userId = user.UserId;
                            //}
                            //else
                            //{
                            user = new UserEntity();
                            user.UserId = Guid.NewGuid().ToString();
                            user.Account = per.IdentifyID;
                            userId = user.UserId;
                            //}
                            user.MSN = "1";
                            user.UserType = "һ�㹤����Ա";
                            user.EnCode = per.TraID;
                            user.Degrees = user.DegreesID = per.Degrees;
                            user.Birthday = per.BirthDay;
                            user.RoleId = roleId;
                            user.RoleName = roleName;
                            user.IdentifyID = per.IdentifyID;
                            user.Gender = per.Sex;
                            user.Nation = per.Nation;
                            user.Email = per.Email;
                            user.EnterTime = per.EntranceDate;
                            user.RealName = per.PersonName;
                            user.Degrees = per.Degrees;
                            user.Native = per.Native;
                            user.DepartureTime = per.LeaveDate;
                            user.Telephone = per.TelPhone;
                            user.IsPresence = per.IsOut == "��" ? "0" : "1";
                            user.Password = "123456";
                            user.DepartmentId = deptId;
                            user.DepartmentCode = deptCode;
                            user.OrganizeId = org.DepartmentId;
                            user.OrganizeCode = org.EnCode;
                            user.Craft = per.Category;
                            user.IsEpiboly = roleName.Contains("�а���") || roleName.Contains("�ְ���") ? "1" : "0";
                            if (string.IsNullOrWhiteSpace(user.HeadIcon))
                            {
                                byte[] bytes = service.GetPersonPicture(header, per.ID);
                                if (bytes.Length > 0)
                                {
                                    string headIcon = Guid.NewGuid().ToString() + ".png";
                                    FileStream fs = new FileStream(path + "\\Resource\\PhotoFile\\" + headIcon, FileMode.Create, FileAccess.ReadWrite);
                                    fs.Write(bytes, 0, bytes.Length);
                                    fs.Close();
                                    user.HeadIcon = "/Resource/PhotoFile/" + headIcon;
                                }
                            }
                            if (user.ModifyDate != null)
                            {
                                if (per.OperDate > user.ModifyDate)
                                {
                                    userbll.SaveForm(userId, user);
                                }
                            }
                            else
                            {
                                userbll.SaveForm(userId, user);
                            }
                            user.Password = "123456";
                            listUsers.Add(user);
                        }
                    }

                }
            }
            if (listUsers.Count > 0)
            {
                if (!string.IsNullOrEmpty(new DataItemDetailBLL().GetItemValue("bzAppUrl")))
                {
                    Task.Factory.StartNew(() =>
                    {
                        ImportUser(listUsers);
                    });
                }
            }
        }
        private void ImportUser(IList<UserEntity> userList)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            //��������web api����ȡ����ֵ��Ĭ��Ϊpost��ʽ
            try
            {
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                //��ǰ�����û��˺�
                nc.Add("account", "System");
                foreach (UserEntity item in userList)
                {
                    //�û���Ϣ
                    item.Gender = item.Gender == "��" ? "1" : "0";
                    if (item.RoleName.Contains("���鼶�û�"))
                    {
                        if (item.RoleName.Contains("������"))
                        {
                            item.RoleId = "a1b68f78-ec97-47e0-b433-2ec4a5368f72";
                            item.RoleName = "���鳤";
                        }
                        else
                        {
                            item.RoleId = "e503d929-daa6-472d-bb03-42533a11f9c6";
                            item.RoleName = "�����Ա";
                        }
                    }
                    if (item.RoleName.Contains("���ż��û�"))
                    {
                        if (item.RoleName.Contains("������"))
                        {
                            item.RoleId = "1266af38-9c0a-4eca-a04a-9829bc2ee92d";
                            item.RoleName = "���Ź���Ա";
                        }
                        else
                        {
                            item.RoleId = "3a4b56ac-6207-429d-ac07-28ab49dca4a6";
                            item.RoleName = "���ż��û�";
                        }
                    }
                    if (item.RoleName.Contains("��˾���û�"))
                    {
                        //if (user.RoleName.Contains("������"))
                        //{
                        item.RoleId = "97869267-e5eb-4f20-89bd-61e7202c4ecd";
                        item.RoleName = "��������Ա";
                        // }

                    }
                    if (item.EnterTime == null)
                    {
                        item.EnterTime = DateTime.Now;
                    }
                }

                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(userList));
                wc.UploadValuesCompleted += wc_UploadValuesCompleted;
                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzAppUrl") + "SaveUsers"), nc);

            }
            catch (Exception ex)
            {
                //��ͬ�����д����־�ļ�
                string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��ͬ������ʧ�ܣ�ͬ����Ϣ" + Newtonsoft.Json.JsonConvert.SerializeObject(userList) + ",�쳣��Ϣ��" + ex.Message + "\r\n");
            }
        }
        void wc_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            var error = e.Error;
            //��ͬ�����д����־�ļ�
            string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            try
            {
                if (error == null)
                {
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "�����ؽ��>" + System.Text.Encoding.UTF8.GetString(e.Result) + "\r\n");
                }

            }
            catch (Exception ex)
            {
                string msg = error == null ? ex.Message : error.Message;
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��ͬ����������,������Ϣ>" + msg + "\r\n");
            }

        }
        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            var error = e.Error;
            //��ͬ�����д����־�ļ�
            string fileName = "dept_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            if (!System.IO.Directory.Exists(HttpContext.Server.MapPath("~/logs/syncbz")))
            {
                System.IO.Directory.CreateDirectory(HttpContext.Server.MapPath("~/logs/syncbz"));
            }
            try
            {
                if (error == null)
                {
                    System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "�����ؽ��>" + e.Result + "\r\n");
                }
            }
            catch (Exception ex)
            {
                string msg = error == null ? ex.Message : error.Message;
                System.IO.File.AppendAllText(HttpContext.Server.MapPath("~/logs/syncbz/" + fileName), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��ͬ����������,������Ϣ>" + msg + "\r\n");
            }

        }
        /// <summary>
        /// �޸Ĺ���״̬Ϊ���깤
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ProIsOver(string keyValue)
        {
            if (outsouringengineerbll.ProIsOver(keyValue))
            {
                return Success("�����ɹ���");
            }
            else
            {
                return Success("����ʧ�ܡ�����ϵ����Ա��");
            }

        }
        /// <summary>
        /// ɾ�������������������
        /// </summary>
        /// <param name="projectId">����Id</param>
        /// <returns></returns>
        public ActionResult DelProjectByDeptAdd(string projectId) {
            var isDel =new HighRiskCommonApplyBLL().GetProjectNum(projectId);
            if (isDel)
            {
                outsouringengineerbll.RemoveForm(projectId);
                return Success("ɾ���ɹ���");
            }
            else {
                return Error("�ù�������صĹ�������,����ɾ����");
            }
        }

        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveFlowForm(string keyValue, string InfoEntity, string ContentEntity)
        {
            try
            {
                WFSchemeInfoBLL wfschemeinfobll = new WFSchemeInfoBLL();
                OutprocessconfigBLL outprocessconfigbll = new OutprocessconfigBLL();
                DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
                WFSchemeInfoEntity entyity = InfoEntity.ToObject<WFSchemeInfoEntity>();
                
                WFSchemeContentEntity contententity = HttpUtility.UrlDecode(ContentEntity).ToObject<WFSchemeContentEntity>();
                wfschemeinfobll.SaveContentEntity(keyValue, contententity);
                //wfFlowInfoBLL.SaveForm(keyValue, entyity, contententity, shcemeAuthorizeData.Split(','));
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(contententity.SchemeContent);
                Flow flow = JsonConvert.DeserializeObject<Flow>(JsonConvert.SerializeObject(dy.Flow));
                nodes start = flow.nodes.Where(t => t.name == "��ʼ").FirstOrDefault();
                outprocessconfigbll.DeleteLinkData(contententity.Id);
                var dept = new DepartmentBLL().GetEntity(contententity.WFSchemeInfoId);
                for (int i = 0; i < flow.nodes.Count; i++)
                {
                    if (!(flow.nodes[i].name == "��ʼ" || flow.nodes[i].name == "����"))
                    {
                        List<lines> lines = flow.lines.Where(t => t.to == flow.nodes[i].id && t.from != start.id).ToList();
                        var configentity = new OutprocessconfigEntity();
                        string FrontModuleCode = "";
                        string FrontModuleName = "";
                        foreach (var item in lines)
                        {
                            FrontModuleCode += flow.nodes.Find(t => t.id == item.from).setInfo.NodeCode + ",";
                            FrontModuleName += flow.nodes.Find(t => t.id == item.from).setInfo.NodeName + ",";
                        }
                        if (dept != null)
                        {
                            configentity.DeptCode = dept.EnCode;
                            configentity.DeptId = dept.DepartmentId;
                            configentity.DeptName = dept.FullName;
                        }

                        configentity.FrontModuleCode = FrontModuleCode;
                        configentity.FrontModuleName = FrontModuleName;
                        configentity.ModuleCode = flow.nodes[i].setInfo.NodeCode;
                        configentity.ModuleName = flow.nodes[i].setInfo.NodeName;
                        configentity.RecId = contententity.Id;
                        configentity.Address = dataitemdetailbll.GetDataItemByDetailValue("OutProcessManagement", configentity.ModuleCode).FirstOrDefault().Description;
                        outprocessconfigbll.SaveForm("", configentity);
                    }

                }
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }

        }

        public ActionResult GetFlowForm(string keyValue)
        {
            try
            {
                WFSchemeInfoBLL wfschemeinfobll = new WFSchemeInfoBLL();
                var data = wfschemeinfobll.GetContentEntity(keyValue);
                var dept = new DepartmentBLL().GetEntity(data.WFSchemeInfoId);
                var result = new
                {
                    data = data,
                    dept = dept
                };
                return ToJsonResult(result);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        public ActionResult GetFlowFormBySchemeInfoId(string keyValue)
        {
            try
            {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                WFSchemeInfoBLL wfschemeinfobll = new WFSchemeInfoBLL();
                var data = wfschemeinfobll.GetSchemeEntityList(keyValue).FirstOrDefault();
                if (data == null)
                {
                    data = wfschemeinfobll.GetSchemeEntityList(user.OrganizeId).FirstOrDefault();
                }
                var dept = new DepartmentBLL().GetEntity(data.WFSchemeInfoId);
                var result = new
                {
                    data = data,
                    dept = dept
                };
                return ToJsonResult(result);
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }

        public ActionResult RemoveFlowForm(string keyValue)
        {
            try
            {
                WFSchemeInfoBLL wfschemeinfobll = new WFSchemeInfoBLL();
                var data = wfschemeinfobll.GetContentEntity(keyValue);
                OutprocessconfigBLL outprocessconfigbll = new OutprocessconfigBLL();
                outprocessconfigbll.DeleteLinkData(data.Id);
                wfschemeinfobll.RemoveContentEntity(keyValue);
                return Success("�����ɹ�");
            }
            catch (Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        /// <summary>
        /// ��������б���
        /// </summary>
        /// <param name="queryJson"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult ExportExcel(string queryJson, string fileName, string currentModuleId)
        {
            Aspose.Cells.Workbook wb = new Aspose.Cells.Workbook();
            string fName = "��������б�_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            var queryParam = queryJson.ToJObject();
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.rows = 100000000;
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.p_kid = "e.ID";
            pagination.p_fields = @"  e.usedeptpeople, e.engineerusedept,e.usedeptpeopphone,
	                                        e.engineerdirector,e.engineerdirectorphone,
                                           e.engineercode,b.senddeptid,b.senddeptname,e.engineerletdept,e.engineerletpeople,e.engineerletpeoplephone,
                                           e.engineername,d.itemname engineertype,l.itemname engineerlevel,
                                           e.outprojectid,e.planenddate,e.actualenddate,e.predicttime,
                                           s.itemvalue statecode,s.itemname engineerstate,
                                           e.createdate,b.fullname outprojectname,e.engineerarea,
                                            decode(ss.examstatus, '1', '', '��λ�������') examstatus,
                                           decode(ss.pactstatus, '1', '', 'Э�����') pactstatus,
                                           decode(ss.compactstatus, '1', '', '��ͬ����') compactstatus,
                                           decode(ss.threetwostatus, '1', '', '��������') threetwostatus,
                                           decode(ss.technicalstatus, '1', '', '��ȫ��������') technicalstatus,
                                           decode(ss.equipmenttoolstatus, '1', '', '����������') equipmenttoolstatus,
                                            decode(ss.peoplestatus, '1', '', '��Ա�������') peoplestatus,
                                            i.busvalidstarttime,i.busvalidendtime,i.splvalidstarttime,
                                            i.splvalidendtime,i.cqvalidstarttime,i.cqvalidendtime ";
            pagination.p_tablename = @"epg_outsouringengineer e
                                          left join base_department b on b.departmentid=e.outprojectid
                                          left join (select * from (select busvalidstarttime,outengineerid,busvalidendtime,
                                                                           splvalidstarttime,splvalidendtime,cqvalidstarttime,cqvalidendtime,
                                                                           row_number() over(partition by outengineerid order by createdate desc) rn
                                                          from epg_aptitudeinvestigateinfo)
                                                 where rn = 1) i on i.outengineerid = e.id
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate
                                          left join epg_startappprocessstatus ss on ss.outengineerid=e.id";
            pagination.sidx = "e.createdate";//�����ֶ�
            pagination.sord = "desc";//����ʽ
            if (currUser.IsSystem)
            {
                pagination.conditionJson = "  1=1 ";
            }
            else if (currUser.RoleName.Contains("ʡ��"))
            {
                pagination.conditionJson = string.Format(@" e.createuserorgcode  in (select encode
                        from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", currUser.NewDeptCode);
            }
            else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�"))
            {
                pagination.conditionJson = string.Format(" e.createuserorgcode like'%{0}%' ", currUser.OrganizeCode);
            }
            else if (currUser.RoleName.Contains("�а��̼��û�"))
            {
                pagination.conditionJson = string.Format("  e.outprojectid ='{0}'", currUser.DeptId);
            }
            else
            {
                pagination.conditionJson = string.Format("  e.engineerletdeptid ='{0}'", currUser.DeptId);
            }
            var data = outsouringengineerbll.GetPageList(pagination, queryJson);
            string p_fields = string.Empty;
            string p_fieldsName = string.Empty;
            //ϵͳĬ�ϵ��б�����
            var defaultdata = modulelistcolumnauthbll.GetEntity(currentModuleId, "", 0);
            if (null != defaultdata)
            {
                p_fields = defaultdata.DEFAULTCOLUMNFIELDS;
                p_fieldsName = defaultdata.DEFAULTCOLUMNNAME;
            }
            //��ǰ�û����б�����
            var userData = modulelistcolumnauthbll.GetEntity(currentModuleId, curUser.UserId, 1);
            //Ϊ�գ��Զ���ȡϵͳĬ��
            if (null != userData)
            {
                p_fields = userData.DEFAULTCOLUMNFIELDS;
                p_fieldsName = userData.DEFAULTCOLUMNNAME;
            }

            wb.Open(Server.MapPath("~/Resource/ExcelTemplate/tmp.xls"));
            Aspose.Cells.Worksheet sheet = wb.Worksheets[0] as Aspose.Cells.Worksheet;
            Aspose.Cells.Cell cell = sheet.Cells[0, 0];
            cell.PutValue("������̻�����Ϣ�б�"); //����
            cell.Style.Pattern = BackgroundType.Solid;
            cell.Style.Font.Size = 14;
            cell.Style.Font.Color = Color.Black;
            //�������������
            if (!string.IsNullOrEmpty(p_fieldsName))
            {
                //��̬����
                string[] p_filedsNameArray = p_fieldsName.Split(',');
                //�����
                Aspose.Cells.Cell serialcell = sheet.Cells[1, 0];
                serialcell.PutValue("���");

                for (int i = 0; i < p_filedsNameArray.Length; i++)
                {
                    Aspose.Cells.Cell curcell = sheet.Cells[1, i + 1];
                    curcell.PutValue(p_filedsNameArray[i].ToString()); //��ͷ
                }
                Aspose.Cells.Cells cells = sheet.Cells;
                cells.Merge(0, 0, 1, p_filedsNameArray.Length + 1);
            }
            //�������������
            if (!string.IsNullOrEmpty(p_fields))
            {
                //��̬����
                var columnslist = p_fields.Split(',');
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    //���к�
                    Aspose.Cells.Cell serialcell = sheet.Cells[i + 2, 0];
                    serialcell.PutValue(i + 1);
                    //�������
                    for (int j = 0; j < columnslist.Length; j++)
                    {
                        Aspose.Cells.Cell curcell = sheet.Cells[i + 2, j + 1];
                        curcell.PutValue(data.Rows[i][columnslist[j]].ToString());
                    }
                }
            }
            wb.Save(Server.MapPath("~/Resource/Temp/" + fName));
            return Success("�����ɹ���", fName);
        }
        #endregion
    }
}
