using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System;
using ERCHTMS.Code;
using ERCHTMS.Busines.PublicInfoManage;
using System.Data;
using ERCHTMS.Entity.PublicInfoManage;
using System.Web;
using Aspose.Words;
using System.Collections.Generic;
using System.Linq;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ����������������Ϣ��
    /// </summary>
    public class AptitudeinvestigateinfoController : MvcControllerBase
    {
        private AptitudeinvestigateinfoBLL aptitudeinvestigateinfobll = new AptitudeinvestigateinfoBLL();
        private OutsourcingprojectBLL Outsourcingprojectbll = new OutsourcingprojectBLL();
        private OutsouringengineerBLL Outsouringengineernll = new OutsouringengineerBLL();
        private AptitudeinvestigateauditBLL auditbll = new AptitudeinvestigateauditBLL();
        private FileInfoBLL filebll = new FileInfoBLL();
        private HistoryRecordBLL historyrecordbll = new HistoryRecordBLL();
        private HistoryRecordDetailBLL historyrecorddetailbll = new HistoryRecordDetailBLL();
        private HistoryAuditBLL hisauditbll = new HistoryAuditBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private AptitudeinvestigatepeopleBLL aptitudeinvestigatepeoplebll = new AptitudeinvestigatepeopleBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();


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
        /// ��ʷ��¼�б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryRecord()
        {
            return View();
        }
        /// <summary>
        /// ��ʷ��¼����ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryRecordDetail()
        {
            return View();
        }
        /// <summary>
        /// ��Ա��������б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PeopleList()
        {
            return View();
        }
        /// <summary>
        /// ��Ա�����������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PeopleForm()
        {
            var isldap = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("IsOpenPassword");
            return View();
        }
        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PhyInfoForm()
        {
            return View();
        }
        /// <summary>
        /// ��ʷ��¼�б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryPeopleList()
        {
            return View();
        }
        /// <summary>
        /// ��ʷ��¼ҳ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HistoryPeopleForm()
        {
            return View();
        }
        /// <summary>
        /// �������ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Flow()
        {
            return View();
        }
        /// <summary>
        /// �����޸�����֤��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddZzzjForm()
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
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.outengineerid,e.engineername,e.engineerareaname as districtname,d.itemname engineertype,l.itemname engineerlevel,
                                       e.id eid,t.issaveorcommit,t.nextcheckrolename,
                                       p.outprojectid,t.isauditover,t.nextcheckdeptid,e.engineerarea,
                                       p.outsourcingname,e.engineerstate,
                                       t.createusername,
                                       t.createdate,t.createuserorgcode,t.flowid,
                                       t.createuserid,e.engineerletdept,e.engineerletdeptid,'' as approveuserids ";
                pagination.p_tablename = @" epg_aptitudeinvestigateinfo t
                                          left join epg_outsouringengineer e on e.id = t.outengineerid
                                          left join epg_outsourcingproject p on p.outprojectid = t.outprojectid
                                           left join base_department b on b.departmentid = t.outprojectid 
                                            left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate";
                pagination.sidx = "t.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                string allrangedept = "";
                try
                {
                    allrangedept = dataitemdetailbll.GetDataItemByDetailCode("SBDept", "SBDeptId").FirstOrDefault().ItemValue;
                }
                catch (Exception)
                {

                }
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1 ";
                }
                else if (currUser.RoleName.Contains("ʡ��"))
                {
                    pagination.conditionJson = string.Format(@" t.createuserorgcode  in (select encode from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", currUser.NewDeptCode);
                }
                else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�") || allrangedept.Contains(currUser.DeptId))
                {
                    pagination.conditionJson = string.Format(" (t.issaveorcommit='1' or t.createuserid='{0}') ", currUser.UserId);
                }
                else if (currUser.RoleName.Contains("�а��̼��û�"))
                {
                    pagination.conditionJson = string.Format(" ( e.outprojectid ='{0}' or e.supervisorid='{0}' or t.createuserid='{1}') ", currUser.DeptId, currUser.UserId);
                }
                else
                {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" (e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.issaveorcommit='1' or t.createuserid='{1}') ", deptentity.EnCode, currUser.UserId);
                }
                //pagination.conditionJson += string.Format(" or t.createuserid='{0}' ", currUser.UserId);
                var data = aptitudeinvestigateinfobll.GetPageList(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = Outsouringengineernll.GetEntity(data.Rows[i]["outengineerid"].ToString());
                    var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //��ȡ��һ�������
                    string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
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
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }

        }
        /// <summary>
        /// ��ȡ��ʷ��¼�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�Json</returns>
        [HttpGet]
        public ActionResult GetHistoryPageListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @" t.createuserid,t.applypeople,t.applypeopleid,t.applydate,
                                           t.createusername,
                                           t.createdate,
                                           t.historyzzscid,
                                           a.auditpeopleid,
                                       a.auditpeople,a.id auditId,
                                       case when a.auditresult='0' then '�ϸ�'
                                            when a.auditresult='2' then '�����'
                                            when a.auditresult='1' then '���ϸ�' else '' end auditresult";
                pagination.p_tablename = @" epg_historyrecord t
                                             left join epg_aptitudeinvestigateaudit a on t.id = a.aptitudeid ";
                pagination.conditionJson = " a.auditresult='1' ";
                pagination.sidx = "t.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                var data = historyrecordbll.GetHistoryPageList(pagination, queryJson);
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
        [HttpGet]
        public ActionResult GetHistoryPeopleList(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @" t.createuserid,t.createdate,t.createusername,
                                           a.auditpeopleid,
                                       a.auditpeople,a.id auditId,
                                       case when a.auditresult='0' then '�ϸ�'
                                            when a.auditresult='2' then '�����'
                                            when a.auditresult='1' then '���ϸ�' else '' end auditresult";
                pagination.p_tablename = @" epg_historypeoplereview t
                                            left join epg_aptitudeinvestigateaudit a on t.id = a.aptitudeid";
                pagination.sidx = "t.createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                pagination.conditionJson = " a.auditresult='1' ";
                var data = historyrecordbll.GetHistoryPeopleList(pagination, queryJson);
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
        /// ��ȡ���������ʷ��¼���鼰�����ʷ��¼
        /// </summary>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue, string HisAuditId)
        {
            var hisrecordDetail = historyrecorddetailbll.GetEntity(keyValue);
            return ToJsonResult(hisrecordDetail);
        }
        /// <summary>
        /// ��ȡ��Ա���������ʷ��¼ʵ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPeopleReviewEntity(string keyValue)
        {
            var data = historyrecordbll.GetPeopleReviewEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = aptitudeinvestigateinfobll.GetList(queryJson);

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

            var data = aptitudeinvestigateinfobll.GetEntity(keyValue);
            var OutprojectEntity = Outsourcingprojectbll.GetOutProjectInfo(data.OUTPROJECTID);
            var OutengineerEntity = Outsouringengineernll.GetEntity(data.OUTENGINEERID);
            //var AuditEntity = auditbll.GetAuditEntity(keyValue);
            //if (AuditEntity == null)
            //{
            //    AuditEntity = new AptitudeinvestigateauditEntity();
            //}
            var resultData = new
            {
                data = data,
                OutprojectEntity = OutprojectEntity,
                OutengineerEntity = OutengineerEntity,
                //AuditEntity = AuditEntity
            };
            return ToJsonResult(resultData);

        }

        /// <summary>
        /// ��ȡ����֤��ʵ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetZzzjFormJson(string keyValue) {
            var data = aptitudeinvestigateinfobll.GetZzzjFormJson(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ȡ��Ա�������ʵ��
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPeopleReview(string keyValue)
        {
            var data = peoplereviewbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        /// <summary>
        /// ��ѯ�������ͼ
        /// </summary>
        /// <param name="keyValue">����</param>
        /// <param name="urltype">��ѯ���ͣ�1 ��λ���� 2 ��Ա���� 3 �����豸���� 4 �綯/��ȫ���������� 5�������� 6�볧��� 7��������</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAuditFlowData(string keyValue, string urltype) {
            try
            {
                var data = aptitudeinvestigateinfobll.GetAuditFlowData(keyValue, urltype);
                return ToJsonResult(data);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
           
        }
        /// <summary>
        /// ���������λId��ȡ���һ�����ͨ����������Ϣ
        /// �����λ�༭ʱ���ȡ��һ�����ͨ������Ϣ
        /// ��������ֶ�,����ǰ�˸�ֵFormʱ���������ֶ�ֵ��ɴ���
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAptituInfoByOutProject(string outProjectId, string keyValue)
        {
            var zzData = aptitudeinvestigateinfobll.GetListByOutprojectId(outProjectId);
            if (zzData != null)
            {
                var data = new
                {
                    BUSCODE = zzData.BUSCODE,
                    BUSCERTIFICATE = zzData.BUSCERTIFICATE,
                    BUSVALIDSTARTTIME = zzData.BUSVALIDSTARTTIME,
                    BUSVALIDENDTIME = zzData.BUSVALIDENDTIME,
                    SPLCODE = zzData.SPLCODE,
                    SPLCERTIFICATE = zzData.SPLCERTIFICATE,
                    SPLVALIDSTARTTIME = zzData.SPLVALIDSTARTTIME,
                    SPLVALIDENDTIME = zzData.SPLVALIDENDTIME,
                    UnitSuper = zzData.UnitSuper,
                    UnitSuperPhone = zzData.UnitSuperPhone,
                    ProjectManager = zzData.ProjectManager,
                    ProjectManagerTel = zzData.ProjectManagerTel,
                    SafetyModerator = zzData.SafetyModerator,
                    SafetyModeratorTel = zzData.SafetyModeratorTel,
                    //CQCODE = zzData.CQCODE,
                    //CQORG = zzData.CQORG,
                    //CQRANGE = zzData.CQRANGE,
                    //CQLEVEL = zzData.CQLEVEL,
                    //CQVALIDSTARTTIME = zzData.CQVALIDSTARTTIME,
                    //CQVALIDENDTIME = zzData.CQVALIDENDTIME,
                    ID = zzData.ID
                };
                //�༭ʱͬ���ϴ�ͨ����˵ĵ�λ���ʸ���
                //���ǰ��ͬ���������ĸ�����ɾ������ͬ��
                //��֤�����һ��ͨ����˵����������Ϣһ��
                for (int i = 1; i <= 8; i++)
                {
                    var id = zzData.ID + "0" + i;
                    var file1 = filebll.GetFiles(id);
                    if (file1.Rows.Count > 0)
                    {
                        var key = keyValue + "0" + i;
                        var fileEdit1 = filebll.GetFiles(key);
                        if (fileEdit1.Rows.Count > 0)
                        {
                            foreach (DataRow item in fileEdit1.Rows)
                            {
                                filebll.RemoveForm(item["FileId"].ToString());
                            }

                        }
                        foreach (DataRow item in file1.Rows)
                        {
                            FileInfoEntity itemFile = new FileInfoEntity();
                            //itemFile.FileId = Guid.NewGuid().ToString();
                            itemFile.FileName = item["FileName"].ToString();
                            itemFile.FilePath = item["filepath"].ToString();
                            itemFile.FileSize = item["filesize"].ToString();
                            itemFile.RecId = (keyValue + "0" + i).ToString();
                            filebll.SaveForm(itemFile.FileId, itemFile);
                        }
                    }
                }
                //��ѯ����֤����Ϣ������ǰ�����ĸ�ֵ
                List<QualificationEntity> quaOldList = aptitudeinvestigateinfobll.GetZzzjList().Where(x => x.InfoId == keyValue).ToList();
                if (quaOldList.Count > 0) {
                    for (int i = 0; i < quaOldList.Count; i++)
                    {
                        aptitudeinvestigateinfobll.RemoveZzzjForm(quaOldList[i].ID);
                    }
                }
                List<QualificationEntity> quaList = aptitudeinvestigateinfobll.GetZzzjList().Where(x => x.InfoId == zzData.ID).ToList();
                for (int i = 0; i < quaList.Count; i++)
                {
                    var file1 =new FileInfoBLL().GetFiles(quaList[i].ID);
                    quaList[i].ID = "";
                    quaList[i].InfoId = keyValue;
                    aptitudeinvestigateinfobll.SaveZzzjForm("", quaList[i]);
                    if (file1.Rows.Count > 0)
                    {
                        var key = quaList[i].ID;
                        foreach (DataRow item in file1.Rows)
                        {
                            FileInfoEntity itemFile = new FileInfoEntity();
                            itemFile.FileName = item["FileName"].ToString();
                            itemFile.FilePath = item["filepath"].ToString();
                            itemFile.FileSize = item["filesize"].ToString();
                            itemFile.RecId = key;
                            new FileInfoBLL().SaveForm(itemFile.FileId, itemFile);
                        }
                    }
                }
                return ToJsonResult(data);
            }
            else
            {
                return ToJsonResult(null);
            }


        }
        [HttpGet]
        public ActionResult GetPagePeopleReviewListJson(Pagination pagination, string queryJson)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "t.id";
                pagination.p_fields = @"e.engineername,b.senddeptid,b.senddeptname,
                                       e.id eid,t.issaveorcommit,t.nextauditdeptcode,t.nextauditdeptid,t.nextauditrole,
                                       p.outprojectid,t.isauditover,t.createuserid,
                                       p.outsourcingname,e.engineerareaname as districtname,d.itemname engineertype,l.itemname engineerlevel,
                                       t.createusername,e.engineerletdeptid,e.engineerletdept,
                                       t.createdate,'' as approveuserids,t.outengineerid,t.flowid ";
                pagination.p_tablename = @" epg_peoplereview t
                                          left join epg_outsouringengineer e on e.id = t.outengineerid
                                          left join epg_outsourcingproject p on p.outprojectid = e.outprojectid
                                          left join base_department b on b.departmentid = e.outprojectid 
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=e.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=e.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=e.engineerstate";
                if (string.IsNullOrWhiteSpace(pagination.sidx)) {
                    pagination.sidx = "t.createdate";//�����ֶ�
                }
                if (string.IsNullOrWhiteSpace(pagination.sord))
                {
                    pagination.sord = "desc";//����ʽ
                }
               
                Operator currUser = OperatorProvider.Provider.Current();
                string allrangedept = "";
                try
                {
                    allrangedept = dataitemdetailbll.GetDataItemByDetailCode("SBDept", "SBDeptId").FirstOrDefault().ItemValue;
                }
                catch (Exception)
                {

                }
                if (currUser.IsSystem)
                {
                    pagination.conditionJson = "  1=1  ";
                }
                else if (currUser.RoleName.Contains("ʡ��"))
                {
                    pagination.conditionJson = string.Format(@" t.createuserorgcode  in (select encode from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", currUser.NewDeptCode);
                }
                else if (currUser.RoleName.Contains("���������û�") || currUser.RoleName.Contains("��˾���û�") || allrangedept.Contains(currUser.DeptId))
                {
                    pagination.conditionJson = string.Format("  (t.issaveorcommit='1' or t.createuserid='{0}') ",currUser.UserId);
                }
                else if (currUser.RoleName.Contains("�а��̼��û�"))
                {
                    pagination.conditionJson = string.Format("  (e.outprojectid ='{0}' or e.supervisorid='{0}' or t.createuserid='{1}') ", currUser.DeptId, currUser.UserId);
                }
                else {
                    var deptentity = departmentbll.GetEntity(currUser.DeptId);
                    while (deptentity.Nature=="����" || deptentity.Nature=="רҵ")
                    {
                        deptentity = departmentbll.GetEntity(deptentity.ParentId);
                    }
                    pagination.conditionJson = string.Format(" (e.engineerletdeptid in (select departmentid from base_department where encode like '{0}%') and t.issaveorcommit='1' or t.createuserid='{1}') ", deptentity.EnCode, currUser.UserId);
                }
                //pagination.conditionJson += string.Format(" or t.createuserid='{0}' ", currUser.UserId);
                var data = peoplereviewbll.GetPagePeopleReviewListJson(pagination, queryJson);
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    var engineerEntity = Outsouringengineernll.GetEntity(data.Rows[i]["outengineerid"].ToString());
                    var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //��ȡ��һ�������
                    string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["outengineerid"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["outengineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
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
            catch (System.Exception ex)
            {
                return Error(ex.ToString());
            }
        }
        /// <summary>
        /// ��ȡ����֤���б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetZzzjPageJson(Pagination pagination, string queryJson) {
            try
            {
                var watch = CommonHelper.TimerStart();
                pagination.p_kid = "id";
                pagination.p_fields = @"createuserid,createuserdeptcode,createuserorgcode,createdate,createusername,
                                        modifydate,modifyuserid,modifyusername,cqcode,cqorg,cqrange,cqlevel,
                                        to_char(cqvalidstarttime,'yyyy-MM-dd') starttime,to_char(cqvalidendtime,'yyyy-MM-dd') endtime,
                                        cqvalidstarttime,cqvalidendtime,infoid";
                pagination.p_tablename = @" epg_qualification ";
                pagination.sidx = "createdate";//�����ֶ�
                pagination.sord = "desc";//����ʽ
                Operator currUser = OperatorProvider.Provider.Current();
                pagination.conditionJson = string.Format(" createuserorgcode='{0}'", currUser.OrganizeCode);
                var data = aptitudeinvestigateinfobll.GetZzzjPageJson(pagination, queryJson);
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
                aptitudeinvestigateinfobll.RemoveForm(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
           
        }

        /// <summary>
        /// ɾ����Ա�����������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemovePeopleReview(string keyValue)
        {
            try
            {
                peoplereviewbll.RemoveForm(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
        }
        /// <summary>
        /// ɾ������֤����Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveZzzjForm(string keyValue) {
            try
            {
                aptitudeinvestigateinfobll.RemoveZzzjForm(keyValue);
                return Success("ɾ���ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
         
        }
        /// <summary>
        /// ɾ����Ա�������idɾ����Ա
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AjaxOnly]
        //public ActionResult RemovePeople(string keyValue)
        //{
        //    peoplereviewbll.RemovePeople(keyValue);
        //    return Success("ɾ���ɹ���");
        //}

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, AptitudeinvestigateinfoEntity entity)
        {
            try
            {
                aptitudeinvestigateinfobll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SavePeopleReview(string keyValue, PeopleReviewEntity entity)
        {
            try
            {
                peoplereviewbll.SaveForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveZzzjForm(string keyValue, QualificationEntity entity)
        {
            try
            {
                aptitudeinvestigateinfobll.SaveZzzjForm(keyValue, entity);
                return Success("�����ɹ���");
            }
            catch (Exception ex)
            {

                return Error(ex.Message);
            }
        }

        public ActionResult ExportPeopleRegister(string keyValue)
        {
            try
            {
                var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�
                var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == userInfo.OrganizeCode && x.ModuleCode == "RYZZSC").ToList();
                string tempPath = @"~/Resource/ExcelTemplate/��Ա�ǼǱ�.doc";
                var tempEntity = tempconfig.FirstOrDefault();
                string fileName = "��Ա�ǼǱ�_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                if (tempconfig.Count > 0)
                {
                    if (tempEntity != null)
                    {
                        switch (tempEntity.ProessMode)
                        {
                            case "TY"://ͨ�ô���ʽ
                                tempPath = @"~/Resource/ExcelTemplate/��Ա�ǼǱ�.doc";
                                break;
                            case "HRCB"://����
                                tempPath = @"~/Resource/ExcelTemplate/��Ա�ǼǱ�.doc";
                                break;
                            case "GDXY"://��������
                                tempPath = @"~/Resource/ExcelTemplate/����������Ա�ǼǱ�.doc";
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    tempPath = @"~/Resource/ExcelTemplate/��Ա�ǼǱ�.doc";
                }
                ExportDataByCode(keyValue, tempPath, fileName);
                return Success("�����ɹ�!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }


        public void ExportReport(string keyValue)
        {
            try
            {
                var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�
                string strDocPath = Server.MapPath("~/Resource/ExcelTemplate/��Ա������������汾.doc");
                string fileName = "��ط�ʩ����Ա���������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
                Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
                DocumentBuilder builder = new DocumentBuilder(doc);
                DataSet ds = new DataSet();
                DataTable dtPro = new DataTable("project");
                dtPro.Columns.Add("createdate");//����ʱ��
                dtPro.Columns.Add("outsourcingproject");//��λ����
                dtPro.Columns.Add("outsouringengineer");//��������
                dtPro.Columns.Add("predicttime");//�ƻ�����
                dtPro.Columns.Add("engineerdirector");//ʩ��������
                dtPro.Columns.Add("workpeoplecount");  //ʩ������
                dtPro.Columns.Add("engineerletdeptid"); //�ù�����
                dtPro.Columns.Add("linkman"); //��ϵ��

                dtPro.Columns.Add("ygauditidea");//�ù�����������
                dtPro.Columns.Add("ygauditdate");//�ù��������ʱ��
                dtPro.Columns.Add("ehsauditidea");//EHS��������
                dtPro.Columns.Add("ehsauditdate");//EHS�����ʱ��
                dtPro.Columns.Add("bgsauditidea");//�칫��������
                dtPro.Columns.Add("bgsauditdate");//�칫�����ʱ��

                HttpResponse resp = System.Web.HttpContext.Current.Response;

                PeopleReviewEntity p = peoplereviewbll.GetEntity(keyValue);

                OutsouringengineerEntity eng = Outsouringengineernll.GetEntity(p.OUTENGINEERID);
                OutsourcingprojectEntity pro = Outsourcingprojectbll.GetOutProjectInfo(eng.OUTPROJECTID);

                DataRow row = dtPro.NewRow();
                row["createdate"] =Convert.ToDateTime(p.CREATEDATE).ToString("yyyy��MM��dd��");
                row["outsourcingproject"] = pro.OUTSOURCINGNAME;
                row["outsouringengineer"] = eng.ENGINEERNAME;
                row["predicttime"] = eng.PREDICTTIME;
                row["engineerdirector"] = eng.ENGINEERDIRECTOR;
                row["engineerletdeptid"] = eng.ENGINEERLETDEPT;
                row["linkman"] = pro.LINKMAN;
                row["workpeoplecount"] = aptitudeinvestigatepeoplebll.GetList("").Where(t => t.PEOPLEREVIEWID == keyValue).Count();
                List<AptitudeinvestigateauditEntity> list = auditbll.GetAuditList(keyValue).OrderByDescending(x => x.AUDITTIME).ToList();
                string pic = Server.MapPath("~/content/Images/no_1.png");//Ĭ��ͼƬ
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var filepath = list[i].AUDITSIGNIMG == null ? "" : (Server.MapPath("~/") + list[i].AUDITSIGNIMG.ToString().Replace("../../", "").ToString()).Replace(@"\/", "/").ToString();
                        var stime = Convert.ToDateTime(list[i].AUDITTIME);
                        if (i == 0)
                        {
                            row["ygauditidea"] = list[i].AUDITOPINION;
                            //zauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("ygauditusername");
                            row["ygauditdate"] = stime.ToString("yyyy��MM��dd��");
                        }
                        else if (i == 1)
                        {
                            row["ehsauditidea"] = list[i].AUDITOPINION;
                            //sauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("ehsauditusername");
                            row["ehsauditdate"] = stime.ToString("yyyy��MM��dd��");
                        }
                        else if (i == 2)
                        {
                            row["bgsauditidea"] = list[i].AUDITOPINION;
                            //aauditusername = list[i].AuditUserName;
                            builder.MoveToMergeField("bgsauditusername");
                            row["bgsauditdate"] = stime.ToString("yyyy��MM��dd��");

                        }

                        if (!System.IO.File.Exists(filepath))
                        {
                            filepath = pic;
                        }
                        builder.InsertImage(filepath, 80, 35);
                    }
                }
                dtPro.Rows.Add(row);
                doc.MailMerge.Execute(dtPro);
                doc.MailMerge.DeleteFields();
                doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ExportDataByCode(string keyValue, string tempPath, string fileName)
        {
            var userInfo = OperatorProvider.Provider.Current();  //��ȡ��ǰ�û�
            string strDocPath = Server.MapPath(tempPath);
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DataSet ds = new DataSet();
            DataTable dtPro = new DataTable("project");
            dtPro.Columns.Add("untilname");//��λ����
            dtPro.Columns.Add("legalperson");//���˴���
            dtPro.Columns.Add("projectname");//��������
            dtPro.Columns.Add("nowtime");
            dtPro.Columns.Add("itemopinion");
            dtPro.Columns.Add("itemhead");  //��Ŀ������
            dtPro.Columns.Add("techhead"); //���ɼ���������
            dtPro.Columns.Add("safehead"); //��ȫ�ල������

            dtPro.Columns.Add("contractperiod");//��ͬ����
            dtPro.Columns.Add("dutydept");//��Ŀ������
            dtPro.Columns.Add("applyperson");//�Ʊ���
            dtPro.Columns.Add("applytime");//�Ʊ�ʱ��
            dtPro.Columns.Add("outhead"); 
            
            DataTable dt = new DataTable("people");
            dt.Columns.Add("no");
            dt.Columns.Add("name");
            dt.Columns.Add("sex");
            dt.Columns.Add("idcard");
            dt.Columns.Add("worktype");
            dt.Columns.Add("health");
            dt.Columns.Add("workyear");
            dt.Columns.Add("education");
            dt.Columns.Add("birthday");
            dt.Columns.Add("cardno");

            HttpResponse resp = System.Web.HttpContext.Current.Response;

            PeopleReviewEntity p = peoplereviewbll.GetEntity(keyValue);

            OutsouringengineerEntity eng = Outsouringengineernll.GetEntity(p.OUTENGINEERID);
            OutsourcingprojectEntity pro = Outsourcingprojectbll.GetOutProjectInfo(eng.OUTPROJECTID);

            DataRow row = dtPro.NewRow();
            var comList = new CompactBLL().GetComoactTimeByProjectId(eng.ID);
            if (comList.Rows.Count > 0)
            {
                var startTime = string.Empty;
                DateTime r = new DateTime();
                if (DateTime.TryParse(comList.Rows[0]["mintime"].ToString(), out r))
                {
                    startTime = r.ToString("yyyy��MM��dd��");
                }
                var endTime = string.Empty;
                DateTime e= new DateTime();
                if (DateTime.TryParse(comList.Rows[0]["maxtime"].ToString(), out e))
                {
                    endTime = e.ToString("yyyy��MM��dd��");
                }
                row["contractperiod"] = startTime + "��" + endTime;
            }
            row["untilname"] = pro.OUTSOURCINGNAME;
            row["legalperson"] = pro.LEGALREP;

            row["projectname"] = eng.ENGINEERNAME;
            row["nowtime"] = DateTime.Now.ToString("yyyy-MM-dd");
            row["dutydept"] = eng.ENGINEERLETDEPT;
            row["applyperson"] = p.CREATEUSERNAME;
            row["applytime"] = p.CREATEDATE.Value.ToString("yyyy��MM��dd��");
            var sendDeptid = eng.ENGINEERLETDEPTID;
            List<AptitudeinvestigateauditEntity> list = auditbll.GetAuditList(keyValue).OrderByDescending(x => x.AUDITTIME).ToList();
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (sendDeptid == list[i].AUDITDEPTID)
                    {
                        var person = new UserBLL().GetEntity(list[i].AUDITPEOPLEID);
                        if (person != null)
                        {
                            //if (person.RoleName.Contains("������"))
                            //{
                                row["itemopinion"] = list[i].AUDITOPINION;
                                row["itemhead"] = list[i].AUDITPEOPLE;
                            //}
                        }
                    }
                    if (eng.OUTPROJECTID == list[i].AUDITDEPTID)
                    {
                        var person = new UserBLL().GetEntity(list[i].AUDITPEOPLEID);
                        if (person != null)
                        {
                            if (person.RoleName.Contains("������"))
                            {
                                row["outhead"] = list[i].AUDITPEOPLE;
                            }
                        }
                    }
                    string val = new DataItemDetailBLL().GetItemValue(userInfo.OrganizeCode);

                    if (!string.IsNullOrEmpty(val))
                    {
                        var deptList = val.Split(',');
                        if (deptList.Length > 1)
                        {
                            if (list[i].AUDITDEPTID.ToString() == deptList[0])
                            {
                                row["techhead"] = list[i].AUDITPEOPLE;
                            }
                            if (list[i].AUDITDEPTID == deptList[1])
                            {
                                row["safehead"] = list[i].AUDITPEOPLE;
                            }
                        }
                    }
                }
            }
            dtPro.Rows.Add(row);
            List<AptitudeinvestigatepeopleEntity> PeopleList = new AptitudeinvestigatepeopleBLL().GetList("").Where(x => x.PEOPLEREVIEWID == keyValue).ToList();
            if (PeopleList.Count > 0)
            {
                for (int i = 0; i < PeopleList.Count; i++)
                {
                    DataRow dtrow = dt.NewRow();
                    dtrow["no"] = (i + 1);
                    dtrow["name"] = PeopleList[i].REALNAME;
                    dtrow["sex"] = PeopleList[i].GENDER;
                    dtrow["idcard"] = PeopleList[i].IDENTIFYID;
                    dtrow["worktype"] = PeopleList[i].WORKOFTYPE;
                    dtrow["health"] = PeopleList[i].STATEOFHEALTH;
                    dtrow["workyear"] = PeopleList[i].WORKYEAR;
                    dtrow["education"] = PeopleList[i].DEGREESID;
                    dtrow["birthday"] = PeopleList[i].BIRTHDAY == null ? "" : PeopleList[i].BIRTHDAY.Value.ToString("yyyy-MM-dd");
                    string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        UserId = PeopleList[i].ID
                    });
                    var cardlist = new CertificateinspectorsBLL().GetList(queryJson).ToList();
                    for (int j = 0; j < cardlist.Count; j++)
                    {
                        dtrow["cardno"] += cardlist[j].CREDENTIALSCODE + ",";
                    }
                    if (dtrow["cardno"].ToString().Length > 0) {
                        dtrow["cardno"] = dtrow["cardno"].ToString().Substring(0, dtrow["cardno"].ToString().Length - 1);
                    }
                    dt.Rows.Add(dtrow);
                }
            }
            ds.Tables.Add(dt);
            ds.Tables.Add(dtPro);
            doc.MailMerge.Execute(dtPro);
            doc.MailMerge.ExecuteWithRegions(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }
        #endregion
    }
}
