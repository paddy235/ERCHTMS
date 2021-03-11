using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using BSFramework.Util.Extension;
using System;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Entity.PublicInfoManage;
using System.Web;
using System.Data;
using Aspose.Words;
using Aspose.Words.Tables;
using ERCHTMS.Busines.BaseManage;
using System.Collections.Generic;
using ERCHTMS.Busines.JPush;
using System.Linq;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Busines.SystemManage;
using System.Data.Common;
using BSFramework.Data;

namespace ERCHTMS.Web.Areas.OutsourcingProject.Controllers
{
    /// <summary>
    /// �� ����������ʩ����
    /// </summary>
    public class SchemeMeasureController : MvcControllerBase
    {
        private SchemeMeasureBLL schememeasurebll = new SchemeMeasureBLL();
        private PeopleReviewBLL peoplereviewbll = new PeopleReviewBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        private HistorySchemeMeasureBLL historyschememeasurebll = new HistorySchemeMeasureBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        private OutsouringengineerBLL outsouringengineerbll = new OutsouringengineerBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private CompactBLL compactbll = new CompactBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DataItemBLL dataitembll = new DataItemBLL();
        private UserBLL userbll = new UserBLL();
        private ManyPowerCheckBLL manypowercheckbll = new ManyPowerCheckBLL();
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
        /// �������ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ApproveForm()
        {
            return View();
        }

        /// <summary>
        /// ��ʷ��¼��Ϣ
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public ActionResult HistoryIndex()
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
            pagination.p_kid = "t.id";
            pagination.p_fields = @"  r.id as engineerid,e.fullname,nvl(t.ENGINEERNAME,r.engineername) as engineername,to_char(t.submitdate,'yyyy-mm-dd') as submitdate,
                                        t.submitperson,(select count(1) from base_fileinfo o where o.recid=t.id) as filenum,
                                        t.createuserid ,t.issaved,t.isover,t.flowdeptname,t.createusername,t.createdate,
                                        t.flowid,t.flowdept,t.flowrolename,t.flowrole,t.flowname,t.organizer,t.organiztime,
                                        r.engineerareaname as districtname,d.itemname engineertype,nvl(k.itemname,l.itemname) engineerlevel,r.engineerletdept,r.engineerletdeptid,'' as approveuserids,
                                        (select b.itemname
                                              from base_dataitem a
                                             inner join base_dataitemdetail b
                                                on a.itemid = b.itemid
                                             where a.itemcode = 'BelongMajor'
                                               and b.itemvalue = t.BELONGMAJOR
                                            ) belongmajor,t.belongdeptname,t.engineerletdeptid as kbengineerletdeptid";
            pagination.p_tablename = @" epg_schememeasure t 
                                        left join epg_outsouringengineer r  on t.projectid=r.id 
                                        left join base_department e on r.outprojectid=e.departmentid
                                         left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectType') d on d.itemvalue=r.engineertype
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') l on l.itemvalue=r.engineerlevel
                                          left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='OutProjectState') s on s.itemvalue=r.engineerstate
                                            left join ( select m.itemname,m.itemvalue from base_dataitem t
                                          left join base_dataitemdetail m on m.itemid=t.itemid where t.itemcode='ProjectLevel') k on k.itemvalue=t.ENGINEERLEVEL";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            string allrangedept = "";
            try
            {
                allrangedept = dataitemdetailbll.GetDataItemByDetailCode("SBDept", "SBDeptId").FirstOrDefault().ItemValue;
            }
            catch (Exception)
            {

            }
            pagination.conditionJson += "(";
            if (role.Contains("ʡ��"))
            {
                pagination.conditionJson += string.Format(@" t.createuserorgcode  in (select encode
                from BASE_DEPARTMENT d
                        where d.deptcode like '{0}%' and d.nature = '����' and d.description is null)", user.NewDeptCode);
            }
            else if (role.Contains("��˾���û�") || role.Contains("���������û�") || allrangedept.Contains(user.DeptId))
            {
                pagination.conditionJson += string.Format(" t.createuserorgcode  = '{0}'", user.OrganizeCode);
            }
            else if (role.Contains("�а��̼��û�"))
            {
                pagination.conditionJson += string.Format(" (r.outprojectid ='{0}' or r.supervisorid='{0}' or t.createuserid = '{1}')", user.DeptId, user.UserId);
            }
            else
            {
                var deptentity = departmentbll.GetEntity(user.DeptId);
                while (deptentity.Nature == "����" || deptentity.Nature == "רҵ")
                {
                    deptentity = departmentbll.GetEntity(deptentity.ParentId);
                }
                pagination.conditionJson += string.Format(" r.engineerletdeptid  in (select departmentid from base_department where encode like '{0}%') ", deptentity.EnCode);
                
            }

            // ����ʲ�������˿��Կ����ֶ�����Ĺ�������������(����idΪnull �ύ�������) 2020/12/02
            pagination.conditionJson += string.Format(" or (t.PROJECTID is null and t.ISSAVED =1 )) ");

            var watch = CommonHelper.TimerStart();
            var data = schememeasurebll.GetList(pagination, queryJson);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                var engineerEntity = outsouringengineerbll.GetEntity(data.Rows[i]["engineerid"].ToString());
                if (engineerEntity != null)
                {
                    var excutdept = departmentbll.GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                    var outengineerdept = departmentbll.GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                    var supervisordept = string.IsNullOrEmpty(engineerEntity.SupervisorId) ? "" : departmentbll.GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    //��ȡ��һ�������
                    string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", excutdept, outengineerdept, "", "", "", supervisordept, data.Rows[i]["engineerid"].ToString());
                    data.Rows[i]["approveuserids"] = str;
                }
                else
                {
                    string str = manypowercheckbll.GetApproveUserId(data.Rows[i]["flowid"].ToString(), data.Rows[i]["id"].ToString(), "", "", data.Rows[i]["kbengineerletdeptid"].ToString(), "", "", "", "", "", data.Rows[i]["engineerid"].ToString());
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



        #region ��ȡ��ʷ��¼�б�
        /// <summary>
        /// ��ȡ��ʷ��¼�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(Pagination pagination, string queryJson)
        {
            pagination.p_kid = "t.id";
            pagination.p_fields = @"  r.id as engineerid,e.fullname,nvl(t.ENGINEERNAME,r.engineername) engineername,to_char(t.submitdate,'yyyy-mm-dd') as submitdate,t.submitperson,
                                     (select count(1) from base_fileinfo o where o.recid=t.id) as filenum,t.createuserid ,t.issaved,t.isover,sdf.AUDITDEPT flowdeptname,t.flowdept,t.flowrolename,t.flowrole,(case when sdf.AUDITRESULT ='1' then '��ͨ��' else 'ͨ��'  end) flowname,t.organizer,t.organiztime";
            pagination.p_tablename = @" epg_historyschememeasure t 
                                        left join epg_outsouringengineer r  on t.projectid=r.id 
                                        left join base_department e on r.outprojectid=e.departmentid
                                        left join epg_aptitudeinvestigateaudit sdf on sdf.aptitudeid = t.id";

            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string role = user.RoleName;
            if (role.Contains("��˾���û�") || role.Contains("���������û�"))
            {
                pagination.conditionJson = string.Format(" t.createuserorgcode  = '{0}'", user.OrganizeCode);
            }
            else if (role.Contains("�а��̼��û�"))
            {
                pagination.conditionJson = string.Format(" e.departmentid = '{0}'", user.DeptId);
            }
            else
            {
                pagination.conditionJson = string.Format(" r.engineerletdeptid = '{0}'", user.DeptId);
            }
            var queryParam = queryJson.ToJObject();
            //ʱ�䷶Χ
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and to_date(to_char(t.SubmitDate,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            //����ID
            if (!queryParam["contractid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.contractid = '{0}'", queryParam["contractid"].ToString());
            }

            pagination.conditionJson += string.Format(" or  (t.createuserid = '{0}'", user.UserId);

            //ʱ�䷶Χ
            if (!queryParam["sTime"].IsEmpty() || !queryParam["eTime"].IsEmpty())
            {
                string startTime = queryParam["sTime"].ToString();
                string endTime = queryParam["eTime"].ToString();
                if (queryParam["sTime"].IsEmpty())
                {
                    startTime = "1899-01-01";
                }
                if (queryParam["eTime"].IsEmpty())
                {
                    endTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                pagination.conditionJson += string.Format(" and to_date(to_char(t.SubmitDate,'yyyy-MM-dd'),'yyyy-MM-dd') between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", startTime, endTime);
            }
            //��ѯ����
            if (!queryParam["condition"].IsEmpty() && !queryParam["txtSearch"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and {0} like '%{1}%'", queryParam["condition"].ToString(), queryParam["txtSearch"].ToString());
            }
            //����ID
            if (!queryParam["contractid"].IsEmpty())
            {
                pagination.conditionJson += string.Format(" and t.contractid = '{0}'", queryParam["contractid"].ToString());
            }
            pagination.conditionJson += ")";

            var watch = CommonHelper.TimerStart();
            var data = schememeasurebll.GetList(pagination, string.Empty);
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
        #endregion

        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = schememeasurebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetHistoryFormJson(string keyValue)
        {
            var data = historyschememeasurebll.GetEntity(keyValue);
            return ToJsonResult(data);
        }


        /// <summary>
        /// ���ݹ���ID��ȡ���һ�����ͨ����������������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetSchemeMeasureListByOutengineerId(string keyValue)
        {
            var data = schememeasurebll.GetSchemeMeasureListByOutengineerId(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region ɾ������
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
            schememeasurebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        #endregion

        #region ��������������޸ģ�
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, SchemeMeasureEntity entity)
        {
            entity.ISSAVED = "0"; //��ǵ�ǰ��¼���ڵǼǽ׶�
            entity.ISOVER = "0"; //����δ��ɣ�1��ʾ��� 
            schememeasurebll.SaveForm(keyValue, entity);
            return Success("�����ɹ���");
        }
        #endregion

        #region �Ǽǵ������ύ����˻��߽���
        /// <summary>
        /// �Ǽǵ������ύ����˻��߽������ύ����һ�����̣�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, SchemeMeasureEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;
           
            string moduleName = "��������";
            // �������id�ǿգ�˵�����������ͣ����µ�����
            if (entity.PROJECTID == null || entity.PROJECTID == "")
            {
                moduleName = "��������_���乤��";
            }
            else
            {
                var Outsouringengineer = outsouringengineerbll.GetEntity(entity.PROJECTID);
                if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
                {
                    switch (Outsouringengineer.ENGINEERLEVEL)
                    {
                        case "001":
                            moduleName = "��������_һ������";
                            break;
                        case "002":
                            moduleName = "��������_��������";
                            break;
                        case "003":
                            moduleName = "��������_��������";
                            break;
                        case "004":
                            moduleName = "��������_�ļ�����";
                            break;
                        default:
                            break;
                    }
                }
            }
            string outengineerid = entity.PROJECTID;
            string flowid = string.Empty;

            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="outengineerid">����Id</param>
            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, outengineerid, true, "", entity.ENGINEERLETDEPTID);
            //����ʱ����ݽ�ɫ�Զ����,��ʱ����ݹ��̺�������ò�ѯ�������Id
            OutsouringengineerEntity engineerEntity = new OutsouringengineerBLL().GetEntity(entity.PROJECTID);
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, moduleName);
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();

            //�Ȳ��ִ�в��ű���
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                {
                    powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(engineerEntity.ENGINEERLETDEPTID).EnCode;
                    powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(engineerEntity.ENGINEERLETDEPTID).DepartmentId;
                }
                //�����λ
                if (powerList[i].CHECKDEPTCODE == "-2" || powerList[i].CHECKDEPTID == "-2")
                {
                    powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(engineerEntity.OUTPROJECTID).EnCode;
                    powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(engineerEntity.OUTPROJECTID).DepartmentId;
                }
                //����λ
                if (powerList[i].CHECKDEPTCODE == "-6" || powerList[i].CHECKDEPTID == "-6")
                {
                    if (!string.IsNullOrEmpty(engineerEntity.SupervisorId))
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(engineerEntity.SupervisorId).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(engineerEntity.SupervisorId).DepartmentId;
                    }
                    else
                    {
                        powerList[i].CHECKDEPTCODE = "";
                        powerList[i].CHECKDEPTID = "";
                    }
                   
                }
            }
            //��¼���Ƿ������Ȩ��--�����Ȩ��ֱ�����ͨ��
            for (int i = 0; i < powerList.Count; i++)
            {

                if (powerList[i].ApplyType == "0")
                {
                    if (powerList[i].CHECKDEPTID == curUser.DeptId)
                    {
                        var rolelist = curUser.RoleName.Split(',');
                        for (int j = 0; j < rolelist.Length; j++)
                        {
                            if (powerList[i].CHECKROLENAME.Contains(rolelist[j]))
                            {
                                checkPower.Add(powerList[i]);
                                break;
                            }
                        }
                    }
                }
                else if (powerList[i].ApplyType == "1")
                {
                    var parameter = new List<DbParameter>();
                    //ȡ�ű�����ȡ�˻��ķ�Χ��Ϣ
                    if (powerList[i].ScriptCurcontent.Contains("@outengineerid"))
                    {
                        parameter.Add(DbParameters.CreateDbParameter("@outengineerid", !string.IsNullOrEmpty(outengineerid) ? outengineerid : ""));
                    }
                    if (powerList[i].ScriptCurcontent.Contains("@engineerletdeptid"))
                    {
                        parameter.Add(DbParameters.CreateDbParameter("@engineerletdeptid", !string.IsNullOrEmpty(entity.ENGINEERLETDEPTID) ? entity.ENGINEERLETDEPTID : ""));
                    }
                    DbParameter[] arrayparam = parameter.ToArray();
                    var userIds = userbll.FindList(powerList[i].ScriptCurcontent, arrayparam).Cast<UserEntity>().Aggregate("", (current, user) => current + (user.UserId + ",")).Trim(',');
                    if (userIds.Contains(curUser.UserId))
                    {
                        checkPower.Add(powerList[i]);
                        //break;
                    }
                }
            }
            if (checkPower.Count > 0)
            {
                ManyPowerCheckEntity check = checkPower.Last();//��ǰ

                for (int i = 0; i < powerList.Count; i++)
                {
                    if (check.ID == powerList[i].ID)
                    {
                        flowid = powerList[i].ID;
                    }
                }
            }
            if (null != mpcEntity)
            {
                //��������������¼
                entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                entity.FLOWROLE = mpcEntity.CHECKROLEID;
                entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                entity.ISSAVED = "1"; //����Ѿ��ӵǼǵ���˽׶�
                entity.ISOVER = "0"; //����δ��ɣ�1��ʾ���
                entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "�����";
                entity.FlowId = mpcEntity.ID;
                if (mpcEntity.ApplyType == "1") //����ǽű�����
                {
                    var parameter = new List<DbParameter>();
                    if (mpcEntity.ScriptCurcontent.Contains("@engineerletdeptid"))
                    {
                        parameter.Add(DbParameters.CreateDbParameter("@engineerletdeptid", !string.IsNullOrEmpty(entity.ENGINEERLETDEPTID) ? entity.ENGINEERLETDEPTID : ""));
                    }
                    DbParameter[] arrayparam = parameter.ToArray();
                    var userIds = userbll.FindList(mpcEntity.ScriptCurcontent, arrayparam);
                    var userAccount = "";
                    var userName = "";
                    int num = 1;
                    foreach (UserEntity info in userIds)
                    {
                        if (userIds.Count() == num)
                        {
                            userAccount += info.UserId ;
                            userName += info.RealName ;
                        }
                        else
                        {
                            userAccount += info.UserId + ",";
                            userName += info.RealName + ",";
                        }
                        
                        num++;
                    }
                    if(userAccount != "")
                    {
                        JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
                    }
                    
                }
                else
                {
                    DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                    var userAccount = dt.Rows[0]["account"].ToString();
                    var userName = dt.Rows[0]["realname"].ToString();
                    JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
                }
                
            }
            else  //Ϊ�����ʾ�Ѿ��������
            {
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.ISSAVED = "1"; //����Ѿ��ӵǼǵ���˽׶�
                entity.ISOVER = "1"; //����δ��ɣ�1��ʾ���
                entity.FLOWNAME = "";
                entity.FlowId = "";
            }
            schememeasurebll.SaveForm(keyValue, entity);

            //�����˼�¼
            if (state == "1")
            {
                //�����Ϣ��
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = "0"; //ͨ��
                aidEntity.AUDITTIME = DateTime.Now;
                aidEntity.AUDITPEOPLE = curUser.UserName;
                aidEntity.AUDITPEOPLEID = curUser.UserId;
                aidEntity.APTITUDEID = entity.ID;  //������ҵ��ID 
                aidEntity.AUDITOPINION = ""; //������
                aidEntity.AUDITSIGNIMG = curUser.SignImg;
                if (null != mpcEntity)
                {
                    aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //��ע �����̵�˳���
                   
                    //aidEntity.FlowId = mpcEntity.ID;
                }
                else
                {
                    aidEntity.REMARK = "7";
                }
                aidEntity.FlowId = flowid;
                //if (curUser.RoleName.Contains("��˾���û�") || curUser.RoleName.Contains("���������û�"))
                //{
                //    aidEntity.AUDITDEPTID = curUser.OrganizeId;
                //    aidEntity.AUDITDEPT = curUser.OrganizeName;
                //}
                //else
                //{
                aidEntity.AUDITDEPTID = curUser.DeptId;
                aidEntity.AUDITDEPT = curUser.DeptName;
                //}
                aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            }

            return Success("�����ɹ�!");
        }
        #endregion

        #region �ύ����˻��߽���
        /// <summary>
        /// �Ǽǵ������ύ����˻��߽������ύ����һ�����̣�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, SchemeMeasureEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            
            string moduleName = "��������";
            // �������id�ǿգ�˵�����������ͣ����µ�����
            if (entity.PROJECTID == null || entity.PROJECTID == "")
            {
                moduleName = "��������_���乤��";
            }
            else
            {
                var Outsouringengineer = outsouringengineerbll.GetEntity(entity.PROJECTID);
                if (dataitemdetailbll.GetDataItemListByItemCode("FlowWithRiskLevel").Count() > 0)
                {
                    switch (Outsouringengineer.ENGINEERLEVEL)
                    {
                        case "001":
                            moduleName = "��������_һ������";
                            break;
                        case "002":
                            moduleName = "��������_��������";
                            break;
                        case "003":
                            moduleName = "��������_��������";
                            break;
                        case "004":
                            moduleName = "��������_�ļ�����";
                            break;
                        default:
                            break;
                    }
                }
            }
            
                

            string outengineerid = entity.PROJECTID;

            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="outengineerid">����Id</param>
            ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, outengineerid, true,"", entity.ENGINEERLETDEPTID);


            #region //�����Ϣ��
            AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
            aidEntity.AUDITRESULT = aentity.AUDITRESULT; //ͨ��
            aidEntity.AUDITTIME = Convert.ToDateTime(aentity.AUDITTIME.Value.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss")); //���ʱ��
            aidEntity.AUDITPEOPLE = aentity.AUDITPEOPLE;  //�����Ա����
            aidEntity.AUDITPEOPLEID = aentity.AUDITPEOPLEID;//�����Աid
            aidEntity.APTITUDEID = keyValue;  //������ҵ��ID 
            aidEntity.AUDITDEPTID = aentity.AUDITDEPTID;//��˲���id
            aidEntity.AUDITDEPT = aentity.AUDITDEPT; //��˲���
            aidEntity.AUDITOPINION = aentity.AUDITOPINION; //������
            aidEntity.FlowId = aentity.FlowId;
            aentity.AUDITSIGNIMG = HttpUtility.UrlDecode(aentity.AUDITSIGNIMG);
            aidEntity.AUDITSIGNIMG = string.IsNullOrWhiteSpace(aentity.AUDITSIGNIMG) ? "" : aentity.AUDITSIGNIMG.ToString().Replace("../..", "");
            if (null != mpcEntity)
            {
                aidEntity.REMARK = (mpcEntity.AUTOID.Value - 1).ToString(); //��ע �����̵�˳���
            }
            else
            {
                aidEntity.REMARK = "7";
            }
            aptitudeinvestigateauditbll.SaveForm(aidEntity.ID, aidEntity);
            #endregion

            #region  //��������������¼
            var smEntity = schememeasurebll.GetEntity(keyValue);
            //���ͨ��
            if (aentity.AUDITRESULT == "0")
            {
                //0��ʾ����δ��ɣ�1��ʾ���̽���
                if (null != mpcEntity)
                {
                    smEntity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                    smEntity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                    smEntity.FLOWROLE = mpcEntity.CHECKROLEID;
                    smEntity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                    smEntity.ISSAVED = "1";
                    smEntity.ISOVER = "0";
                    smEntity.FlowId = mpcEntity.ID;//��ֵ����Id
                    smEntity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "�����";
                    DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                    var userAccount = dt.Rows[0]["account"].ToString();
                    var userName = dt.Rows[0]["realname"].ToString();
                    JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
                }
                else
                {
                    smEntity.FLOWDEPT = "";
                    smEntity.FLOWDEPTNAME = "";
                    smEntity.FLOWROLE = "";
                    smEntity.FLOWROLENAME = "";
                    smEntity.ISSAVED = "1";
                    smEntity.ISOVER = "1";
                    smEntity.FLOWNAME = "";
                    smEntity.FlowId = "";
                }
            }
            else //��˲�ͨ�� 
            {
                smEntity.FLOWDEPT = "";
                smEntity.FLOWDEPTNAME = "";
                smEntity.FLOWROLE = "";
                smEntity.FLOWROLENAME = "";
                smEntity.ISSAVED = "0"; //���ڵǼǽ׶�
                smEntity.ISOVER = "0"; //�Ƿ����״̬��ֵΪδ���
                smEntity.FLOWNAME = "";
                smEntity.FlowId = "";//���˺�����Id���
                var applyUser = new UserBLL().GetEntity(smEntity.CREATEUSERID);
                if (applyUser != null)
                {
                    JPushApi.PushMessage(applyUser.Account, smEntity.CREATEUSERNAME, "WB002", entity.ID);
                }

            }
            //����������������״̬��Ϣ
            schememeasurebll.SaveForm(keyValue, smEntity);
            #endregion

            #region    //��˲�ͨ��
            if (aentity.AUDITRESULT == "1")
            {
                //�����ʷ��¼
                HistorySchemeMeasureEntity hsentity = new HistorySchemeMeasureEntity();
                hsentity.CREATEUSERID = smEntity.CREATEUSERID;
                hsentity.CREATEUSERDEPTCODE = smEntity.CREATEUSERDEPTCODE;
                hsentity.CREATEUSERORGCODE = smEntity.CREATEUSERORGCODE;
                hsentity.CREATEDATE = smEntity.CREATEDATE;
                hsentity.CREATEUSERNAME = smEntity.CREATEUSERNAME;
                hsentity.MODIFYDATE = smEntity.MODIFYDATE;
                hsentity.MODIFYUSERID = smEntity.MODIFYUSERID;
                hsentity.MODIFYUSERNAME = smEntity.MODIFYUSERNAME;
                hsentity.SUBMITDATE = smEntity.SUBMITDATE;
                hsentity.SUBMITPERSON = smEntity.SUBMITPERSON;
                hsentity.PROJECTID = smEntity.PROJECTID;
                hsentity.CONTRACTID = smEntity.ID; //����ID
                hsentity.ORGANIZER = smEntity.ORGANIZER;
                hsentity.ORGANIZTIME = smEntity.ORGANIZTIME;
                hsentity.ISOVER = smEntity.ISOVER;
                hsentity.ISSAVED = smEntity.ISSAVED;
                hsentity.FLOWDEPTNAME = smEntity.FLOWDEPTNAME;
                hsentity.FLOWDEPT = smEntity.FLOWDEPT;
                hsentity.FLOWROLENAME = smEntity.FLOWROLENAME;
                hsentity.FLOWROLE = smEntity.FLOWROLE;
                hsentity.FLOWNAME = smEntity.FLOWNAME;
                hsentity.SummitContent = smEntity.SummitContent;
                // ����ʲ�����ֶ�
                hsentity.ENGINEERCODE = smEntity.ENGINEERCODE;
                hsentity.ENGINEERNAME = smEntity.ENGINEERNAME;
                hsentity.ENGINEERTYPE = smEntity.ENGINEERTYPE;
                hsentity.ENGINEERAREANAME = smEntity.ENGINEERAREANAME;
                hsentity.ENGINEERLEVEL = smEntity.ENGINEERLEVEL;
                hsentity.ENGINEERLETDEPT = smEntity.ENGINEERLETDEPT;
                hsentity.ENGINEERLETDEPTID = smEntity.ENGINEERLETDEPTID;
                hsentity.ENGINEERLETDEPTCODE = smEntity.ENGINEERLETDEPTCODE;
                hsentity.ENGINEERAREA = smEntity.ENGINEERAREA;
                hsentity.ENGINEERCONTENT = smEntity.ENGINEERCONTENT;
                hsentity.BELONGMAJOR = smEntity.BELONGMAJOR;
                hsentity.BELONGDEPTNAME = smEntity.BELONGDEPTNAME;
                hsentity.BELONGDEPTID = smEntity.BELONGDEPTID;
                hsentity.BELONGCODE = smEntity.BELONGCODE;

                hsentity.ID = "";

                historyschememeasurebll.SaveForm(hsentity.ID, hsentity);

                //��ȡ��ǰҵ������������˼�¼
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //����������˼�¼����ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    mode.APTITUDEID = hsentity.ID; //��Ӧ�µ�ID
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
                //�������¸�����¼����ID
                var flist = fileinfobll.GetImageListByObject(keyValue);
                foreach (FileInfoEntity fmode in flist)
                {
                    fmode.RecId = hsentity.ID; //��Ӧ�µ�ID
                    fileinfobll.SaveForm("", fmode);
                }
            }
            #endregion

            return Success("�����ɹ�!");
        }
        #endregion

        #region ������������������
        /// <summary>
        /// ������������������
        /// </summary>
        /// <param name="keyValue">����������¼ID</param>
        /// <param name="projectId">�������id</param>
        /// <returns></returns>
        public ActionResult ExportWord(string keyValue)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == curUser.OrganizeCode && x.ModuleCode == "SCLA").ToList();
            string tempPath =@"Resource\ExcelTemplate\������������ģ��.doc";
            var tempEntity = tempconfig.FirstOrDefault();
            string fileName = "��������������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            if (tempconfig.Count > 0)
            {
                if (tempEntity != null)
                {
                    switch (tempEntity.ProessMode)
                    {
                        case "TY"://ͨ�ô���ʽ
                            tempPath = @"Resource\ExcelTemplate\������������ģ��.doc";
                            break;
                        case "HRCB"://����
                            tempPath = @"Resource\ExcelTemplate\������������ģ��.doc";
                            break;
                        case "GDXY"://��������
                            tempPath = @"Resource\ExcelTemplate\����������������������.doc";
                            break;
                        case "HJB"://�ƽ�
                            tempPath = @"Resource\ExcelTemplate\�ƽ���������������.doc";
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                tempPath = @"Resource\ExcelTemplate\������������ģ��.doc";
            }
            ExportTyData(keyValue, tempPath, fileName, tempEntity == null ? "" : tempEntity.ProessMode);
        
            return Success("�����ɹ�!");
        }
        /// <summary>
        /// ͨ�õ���
        /// </summary>
        /// <param name="keyValue">������������</param>
        /// <returns></returns>
        private void ExportTyData(string keyValue, string tempPath,string fileName,string ProessMode)
        {
            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������

            //string fileName = "��������������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Request.PhysicalApplicationPath + tempPath;
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("Title"); //���� --�����������
            dt.Columns.Add("EngineerLetDept"); //������λ
            dt.Columns.Add("OutSourcingDept"); //�����λ
            dt.Columns.Add("ContractNo"); //��ͬ���
            dt.Columns.Add("SummitContent"); //�ύ����
            dt.Columns.Add("OutSourcingProject"); //��������
            dt.Columns.Add("Organizer"); //������Ա
            dt.Columns.Add("OrganizeTime"); //����ʱ��
            dt.Columns.Add("Principal"); //���̸�����
            dt.Columns.Add("SubmitTime"); //�ύʱ��
            dt.Columns.Add("Person1"); //רҵ������Ա
            dt.Columns.Add("ApproveDate1"); //���ʱ��
            dt.Columns.Add("Person2"); //��������
            dt.Columns.Add("ApproveDate2"); //���ʱ��
            dt.Columns.Add("ApproveIdea3"); //������ר��������
            dt.Columns.Add("Person3"); //������ר��
            dt.Columns.Add("ApproveDate3"); //������ר�����ʱ��
            dt.Columns.Add("ApproveIdea4"); //����������������
            dt.Columns.Add("Person4"); //����������
            dt.Columns.Add("ApproveDate4"); //�������������ʱ��
            dt.Columns.Add("ApproveIdea5"); //������ר��������
            dt.Columns.Add("Person5"); //������ר��
            dt.Columns.Add("ApproveDate5"); //������ר�����ʱ��
            dt.Columns.Add("ApproveIdea6"); // ����������������
            dt.Columns.Add("Person6"); // ����������
            dt.Columns.Add("ApproveDate6"); // �������������ʱ��
            dt.Columns.Add("ApproveIdea7"); //��˾�쵼������
            dt.Columns.Add("Person7"); //�ֹ��쵼
            dt.Columns.Add("ApproveDate7"); //�ֹ��쵼���ʱ��
            dt.Columns.Add("SubmitDate");
            dt.Columns.Add("ApproveIdea1");
            dt.Columns.Add("ApproveIdea2");
            dt.Columns.Add("Dept2");
            dt.Columns.Add("Dept3");
            dt.Columns.Add("Dept4");
            dt.Columns.Add("SupervisorName");//����λ
            dt.Columns.Add("idea2");//�ܼ�����ʦ������
            dt.Columns.Add("person1");//������ʦ
            dt.Columns.Add("date1");//������ʦ����
            dt.Columns.Add("person2");//�ܼ�����ʦ
            dt.Columns.Add("date2");//�ܼ�����ʦ����
            dt.Columns.Add("person3");//��Ŀ�鳤
            dt.Columns.Add("person4");//רҵ����
            dt.Columns.Add("person5");//��������
            dt.Columns.Add("idea5");//�������
            dt.Columns.Add("date5");//�������ʱ��
            dt.Columns.Add("person6");//�����������
            dt.Columns.Add("idea6");//������������
            dt.Columns.Add("date6");//���������ʱ��
            dt.Columns.Add("idea7");//�ֹ��쵼���
            dt.Columns.Add("person7");//�ֹ��쵼�����
            dt.Columns.Add("date7");//�ֹ��쵼���ʱ��

            DataRow row = dt.NewRow();

            //��ȡ����������¼

            //������̼�¼
            DataTable sdt = schememeasurebll.GetObjectByKeyValue(keyValue);
            var SCENTITY = schememeasurebll.GetEntity(keyValue);
            if (sdt.Rows.Count == 1)
            {
                row["Title"] = sdt.Rows[0]["engineername"].ToString();
                row["EngineerLetDept"] = sdt.Rows[0]["engineerletdept"].ToString();//������λ
                row["OutSourcingDept"] = sdt.Rows[0]["fullname"].ToString(); //�����λ
                //��ȡ��ͬ���
                var compactList = compactbll.GetListByProjectId(sdt.Rows[0]["engineerid"].ToString());
                if (compactList.Count > 0)
                {
                    row["ContractNo"] = compactList[0].COMPACTNO.ToString();
                }
                else
                {
                    row["ContractNo"] = "";
                }

                row["OutSourcingProject"] = sdt.Rows[0]["engineername"].ToString() == "" ? SCENTITY.ENGINEERNAME : sdt.Rows[0]["engineername"].ToString(); //��������

                row["Organizer"] = sdt.Rows[0]["organizer"].ToString(); //������Ա
                DateTime organiztime = Convert.ToDateTime(sdt.Rows[0]["organiztime"].ToString());
                row["OrganizeTime"] = organiztime.Year.ToString() + "��" + organiztime.Month.ToString() + "��" + organiztime.Day.ToString() + "��"; //����ʱ��

                row["Principal"] = sdt.Rows[0]["engineerdirector"].ToString(); //���̸�����
                if (SCENTITY.PROJECTID == "")
                {
                    row["SubmitTime"] = "";
                }
                else
                {
                    DateTime engineerdate = Convert.ToDateTime(sdt.Rows[0]["engineerdate"].ToString()); //�ύʱ��
                    row["SubmitTime"] = engineerdate.ToString("yyyy��MM��dd��");
                }
                

                DateTime submitdate = Convert.ToDateTime(sdt.Rows[0]["submitdate"].ToString());
                row["SubmitDate"] = submitdate.ToString("yyyy��MM��dd��");
                row["SummitContent"] = sdt.Rows[0]["summitcontent"].ToString();
                row["SupervisorName"] = sdt.Rows[0]["supervisorname"].ToString();
                //��˼�¼
                List<AptitudeinvestigateauditEntity> list = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                #region ͨ�ð汾��˼�¼
                foreach (AptitudeinvestigateauditEntity entity in list)
                {
                    if (entity.REMARK == "1")
                    {
                        row["Dept2"] = entity.AUDITDEPT.ToString();
                        //ִ�в���ר��
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person1"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person1"] = filepath;
                            }
                            else
                            {
                                row["Person1"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("Person1");
                        builder.InsertImage(row["Person1"].ToString(), 80, 35);
                        row["ApproveIdea1"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        row["ApproveDate1"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "2")
                    {
                        row["Dept2"] = entity.AUDITDEPT.ToString();
                        //ִ�в��Ÿ�����
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person2"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person2"] = filepath;
                            }
                            else
                            {
                                row["Person2"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("Person2");
                        builder.InsertImage(row["Person2"].ToString(), 80, 35);
                        row["ApproveIdea2"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        row["ApproveDate2"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "3")
                    {
                        row["Dept3"] = entity.AUDITDEPT.ToString();
                        //������ר�����
                        row["ApproveIdea3"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person3"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person3"] = filepath;
                            }
                            else
                            {
                                row["Person3"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("Person3");
                        builder.InsertImage(row["Person3"].ToString(), 80, 35);
                        row["ApproveDate3"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "4")
                    {
                        row["Dept3"] = entity.AUDITDEPT.ToString();
                        //���������������
                        row["ApproveIdea4"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person4"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person4"] = filepath;
                            }
                            else
                            {
                                row["Person4"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("Person4");
                        builder.InsertImage(row["Person4"].ToString(), 80, 35);
                        row["ApproveDate4"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "5")
                    {
                        row["Dept4"] = entity.AUDITDEPT.ToString();
                        //������ר�����
                        row["ApproveIdea5"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";

                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person5"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person5"] = filepath;
                            }
                            else
                            {
                                row["Person5"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }

                        builder.MoveToMergeField("Person5");
                        builder.InsertImage(row["Person5"].ToString(), 80, 35);
                        row["ApproveDate5"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "6")
                    {
                        row["Dept4"] = entity.AUDITDEPT.ToString();
                        //���������������
                        row["ApproveIdea6"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person6"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person6"] = filepath;
                            }
                            else
                            {
                                row["Person6"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("Person6");
                        builder.InsertImage(row["Person6"].ToString(), 80, 35);
                        row["ApproveDate6"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "7")
                    {

                        //��˾�쵼���
                        row["ApproveIdea7"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person7"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person7"] = filepath;
                            }
                            else
                            {
                                row["Person7"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("Person7");
                        builder.InsertImage(row["Person7"].ToString(), 80, 35);
                        row["ApproveDate7"] = entity.AUDITTIME.Value.ToString("yyyy��MM��dd��");
                    }
                }
                #endregion

                #region �ƽ���˼�¼
                if (ProessMode == "HJB")
                {
                    if (string.IsNullOrWhiteSpace(row["SupervisorName"].ToString())) //�ж��Ƿ��м���λ
                    {
                        Table table = (Table)doc.GetChild(NodeType.Table, 0, true);
                        table.Rows[2].Remove();
                        if (list.Count > 0)
                        {
                            //ִ�в���ר��
                            if (string.IsNullOrWhiteSpace(list[0].AUDITSIGNIMG))
                            {
                                row["person3"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[0].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person3"] = filepath;
                                }
                                else
                                {
                                    row["person3"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person3");
                            builder.InsertImage(row["person3"].ToString(), 80, 35);
                        }
                        if (list.Count > 1)
                        {
                            if (string.IsNullOrWhiteSpace(list[1].AUDITSIGNIMG))
                            {
                                row["person4"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[1].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person4"] = filepath;
                                }
                                else
                                {
                                    row["person4"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person4");
                            builder.InsertImage(row["person4"].ToString(), 80, 35);
                        }
                        if (list.Count > 2)
                        {
                            row["idea5"] = !string.IsNullOrEmpty(list[2].AUDITOPINION) ? list[2].AUDITOPINION.ToString() : "";
                            if (string.IsNullOrWhiteSpace(list[2].AUDITSIGNIMG))
                            {
                                row["person5"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[2].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person5"] = filepath;
                                }
                                else
                                {
                                    row["person5"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person5");
                            builder.InsertImage(row["person5"].ToString(), 80, 35);
                            row["date5"] = list[2].AUDITTIME.Value.ToString("yyyy��MM��dd��");
                        }
                        if (list.Count > 3)
                        {
                            row["idea6"] = !string.IsNullOrEmpty(list[3].AUDITOPINION) ? list[3].AUDITOPINION.ToString() : "";
                            if (string.IsNullOrWhiteSpace(list[3].AUDITSIGNIMG))
                            {
                                row["person6"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[3].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person6"] = filepath;
                                }
                                else
                                {
                                    row["person6"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person6");
                            builder.InsertImage(row["person6"].ToString(), 80, 35);
                            row["date6"] = list[3].AUDITTIME.Value.ToString("yyyy��MM��dd��");
                        }
                        if (list.Count > 4)
                        {
                            row["idea7"] = !string.IsNullOrEmpty(list[4].AUDITOPINION) ? list[4].AUDITOPINION.ToString() : "";
                            if (string.IsNullOrWhiteSpace(list[4].AUDITSIGNIMG))
                            {
                                row["person7"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[4].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person7"] = filepath;
                                }
                                else
                                {
                                    row["person7"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person7");
                            builder.InsertImage(row["person7"].ToString(), 80, 35);
                            row["date7"] = list[4].AUDITTIME.Value.ToString("yyyy��MM��dd��");
                        }
                    }
                    else
                    {
                        if (list.Count > 0)
                        {
                            //ִ�в���ר��
                            if (string.IsNullOrWhiteSpace(list[0].AUDITSIGNIMG))
                            {
                                row["person1"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[0].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person1"] = filepath;
                                }
                                else
                                {
                                    row["person1"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person1");
                            builder.InsertImage(row["person1"].ToString(), 80, 35);
                            row["date1"] = list[0].AUDITTIME.Value.ToString("yyyy��MM��dd��");
                        }
                        if (list.Count > 1)
                        {
                            row["idea2"] = !string.IsNullOrEmpty(list[1].AUDITOPINION) ? list[1].AUDITOPINION.ToString() : "";
                            if (string.IsNullOrWhiteSpace(list[1].AUDITSIGNIMG))
                            {
                                row["person2"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[1].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person2"] = filepath;
                                }
                                else
                                {
                                    row["person2"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person2");
                            builder.InsertImage(row["person2"].ToString(), 80, 35);
                            row["date2"] = list[1].AUDITTIME.Value.ToString("yyyy��MM��dd��");
                        }
                        if (list.Count > 2)
                        {
                            //ִ�в���ר��
                            if (string.IsNullOrWhiteSpace(list[2].AUDITSIGNIMG))
                            {
                                row["person3"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[2].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person3"] = filepath;
                                }
                                else
                                {
                                    row["person3"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person3");
                            builder.InsertImage(row["person3"].ToString(), 80, 35);
                        }
                        if (list.Count > 3)
                        {
                            if (string.IsNullOrWhiteSpace(list[3].AUDITSIGNIMG))
                            {
                                row["person4"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[3].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person4"] = filepath;
                                }
                                else
                                {
                                    row["person4"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person4");
                            builder.InsertImage(row["person4"].ToString(), 80, 35);
                        }
                        if (list.Count > 4)
                        {
                            row["idea5"] = !string.IsNullOrEmpty(list[4].AUDITOPINION) ? list[4].AUDITOPINION.ToString() : "";
                            if (string.IsNullOrWhiteSpace(list[4].AUDITSIGNIMG))
                            {
                                row["person5"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[4].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person5"] = filepath;
                                }
                                else
                                {
                                    row["person5"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person5");
                            builder.InsertImage(row["person5"].ToString(), 80, 35);
                            row["date5"] = list[4].AUDITTIME.Value.ToString("yyyy��MM��dd��");
                        }
                        if (list.Count > 5)
                        {
                            row["idea6"] = !string.IsNullOrEmpty(list[5].AUDITOPINION) ? list[5].AUDITOPINION.ToString() : "";
                            if (string.IsNullOrWhiteSpace(list[5].AUDITSIGNIMG))
                            {
                                row["person6"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[5].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person6"] = filepath;
                                }
                                else
                                {
                                    row["person6"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person6");
                            builder.InsertImage(row["person6"].ToString(), 80, 35);
                            row["date6"] = list[5].AUDITTIME.Value.ToString("yyyy��MM��dd��");
                        }
                        if (list.Count > 6)
                        {
                            row["idea7"] = !string.IsNullOrEmpty(list[6].AUDITOPINION) ? list[6].AUDITOPINION.ToString() : "";
                            if (string.IsNullOrWhiteSpace(list[6].AUDITSIGNIMG))
                            {
                                row["person7"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                            else
                            {
                                var filepath = Server.MapPath("~/") + list[6].AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                                if (System.IO.File.Exists(filepath))
                                {
                                    row["person7"] = filepath;
                                }
                                else
                                {
                                    row["person7"] = Server.MapPath("~/content/Images/no_1.png");
                                }
                            }
                            builder.MoveToMergeField("person7");
                            builder.InsertImage(row["person7"].ToString(), 80, 35);
                            row["date7"] = list[6].AUDITTIME.Value.ToString("yyyy��MM��dd��");
                        }
                    }
                }
                #endregion



            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }
        #endregion

        #region ������ʷ��������������
        /// <summary>
        /// ��ʷ��¼����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="tempPath"></param>
        /// <param name="fileName"></param>
        private void ExportHisTyData(string keyValue, string tempPath, string fileName) {

            HttpResponse resp = System.Web.HttpContext.Current.Response;
            //�������

            //string fileName = "��������������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            string strDocPath = Request.PhysicalApplicationPath + tempPath;
            Aspose.Words.Document doc = new Aspose.Words.Document(strDocPath);
            DocumentBuilder builder = new DocumentBuilder(doc);
            DataTable dt = new DataTable();
            dt.Columns.Add("Title"); //���� --�����������
            dt.Columns.Add("EngineerLetDept"); //������λ
            dt.Columns.Add("OutSourcingDept"); //�����λ
            dt.Columns.Add("ContractNo"); //��ͬ���
            dt.Columns.Add("SummitContent"); //�ύ����
            dt.Columns.Add("OutSourcingProject"); //��������
            dt.Columns.Add("Organizer"); //������Ա
            dt.Columns.Add("OrganizeTime"); //����ʱ��
            dt.Columns.Add("Principal"); //���̸�����
            dt.Columns.Add("SubmitTime"); //�ύʱ��
            dt.Columns.Add("Person1"); //רҵ������Ա
            dt.Columns.Add("ApproveDate1"); //���ʱ��
            dt.Columns.Add("Person2"); //��������
            dt.Columns.Add("ApproveDate2"); //���ʱ��
            dt.Columns.Add("ApproveIdea3"); //������ר��������
            dt.Columns.Add("Person3"); //������ר��
            dt.Columns.Add("ApproveDate3"); //������ר�����ʱ��
            dt.Columns.Add("ApproveIdea4"); //����������������
            dt.Columns.Add("Person4"); //����������
            dt.Columns.Add("ApproveDate4"); //�������������ʱ��
            dt.Columns.Add("ApproveIdea5"); //������ר��������
            dt.Columns.Add("Person5"); //������ר��
            dt.Columns.Add("ApproveDate5"); //������ר�����ʱ��
            dt.Columns.Add("ApproveIdea6"); // ����������������
            dt.Columns.Add("Person6"); // ����������
            dt.Columns.Add("ApproveDate6"); // �������������ʱ��
            dt.Columns.Add("ApproveIdea7"); //��˾�쵼������
            dt.Columns.Add("Person7"); //�ֹ��쵼
            dt.Columns.Add("ApproveDate7"); //�ֹ��쵼���ʱ��
            dt.Columns.Add("SubmitDate");
            dt.Columns.Add("ApproveIdea1");
            dt.Columns.Add("ApproveIdea2");
            dt.Columns.Add("Dept2");
            dt.Columns.Add("Dept3");
            dt.Columns.Add("Dept4");
            DataRow row = dt.NewRow();

            //��ȡ����������¼

            //������̼�¼
            DataTable sdt = schememeasurebll.GetHistoryObjectByKeyValue(keyValue);
            if (sdt.Rows.Count == 1)
            {
                row["Title"] = sdt.Rows[0]["engineername"].ToString();
                row["EngineerLetDept"] = sdt.Rows[0]["engineerletdept"].ToString();//������λ
                row["OutSourcingDept"] = sdt.Rows[0]["fullname"].ToString(); //�����λ
                //��ȡ��ͬ���
                var compactList = compactbll.GetListByProjectId(sdt.Rows[0]["engineerid"].ToString());
                if (compactList.Count > 0)
                {
                    row["ContractNo"] = compactList[0].COMPACTNO.ToString();
                }
                else
                {
                    row["ContractNo"] = "";
                }

                row["OutSourcingProject"] = sdt.Rows[0]["engineername"].ToString(); //��������

                row["Organizer"] = sdt.Rows[0]["organizer"].ToString(); //������Ա
                DateTime organiztime = Convert.ToDateTime(sdt.Rows[0]["organiztime"].ToString());
                row["OrganizeTime"] = organiztime.Year.ToString() + "��" + organiztime.Month.ToString() + "��" + organiztime.Day.ToString() + "��"; //����ʱ��

                row["Principal"] = sdt.Rows[0]["engineerdirector"].ToString(); //���̸�����
                DateTime engineerdate = Convert.ToDateTime(sdt.Rows[0]["engineerdate"].ToString()); //�ύʱ��
                row["SubmitTime"] = engineerdate.ToString("yyyy��MM��dd��"); ; //����ʱ�� //����ʱ��
                DateTime submitdate = Convert.ToDateTime(sdt.Rows[0]["submitdate"].ToString());
                row["SubmitDate"] = submitdate.ToString("yyyy��MM��dd��");
                row["SummitContent"] = sdt.Rows[0]["summitcontent"].ToString();
                //��˼�¼
                List<AptitudeinvestigateauditEntity> list = aptitudeinvestigateauditbll.GetAuditList(keyValue);

                foreach (AptitudeinvestigateauditEntity entity in list)
                {

                    if (entity.REMARK == "1")
                    {
                        row["Dept2"] = entity.AUDITDEPT.ToString();
                        //ִ�в���ר��
                        //row["Person1"] = entity.AUDITPEOPLE.ToString();
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person1"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person1"] = filepath;
                            }
                            else
                            {
                                row["Person1"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }

                        builder.MoveToMergeField("Person1");
                        builder.InsertImage(row["Person1"].ToString(), 80, 35);
                        row["ApproveIdea1"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        DateTime ApproveDate1 = entity.AUDITTIME.Value;
                        row["ApproveDate1"] = ApproveDate1.ToString("yyyy��MM��dd��") ;
                    }
                    if (entity.REMARK == "2")
                    {
                        row["Dept2"] = entity.AUDITDEPT.ToString();
                        //ִ�в��Ÿ�����
                        //row["Person2"] = entity.AUDITPEOPLE.ToString();
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person2"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person2"] = filepath;
                            }
                            else
                            {
                                row["Person2"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }

                        builder.MoveToMergeField("Person2");
                        builder.InsertImage(row["Person2"].ToString(), 80, 35);
                        row["ApproveIdea2"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        DateTime ApproveDate2 = entity.AUDITTIME.Value;
                        row["ApproveDate2"] = ApproveDate2.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "3")
                    {
                        row["Dept3"] = entity.AUDITDEPT.ToString();
                        //������ר�����
                        row["ApproveIdea3"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        //row["Person3"] = entity.AUDITPEOPLE.ToString();
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person3"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person3"] = filepath;
                            }
                            else
                            {
                                row["Person3"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }

                        builder.MoveToMergeField("Person3");
                        builder.InsertImage(row["Person3"].ToString(), 80, 35);
                        DateTime ApproveDate3 = entity.AUDITTIME.Value;
                        row["ApproveDate3"] = ApproveDate3.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "4")
                    {
                        row["Dept3"] = entity.AUDITDEPT.ToString();
                        //���������������
                        row["ApproveIdea4"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        //row["Person4"] = entity.AUDITPEOPLE.ToString();
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person4"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person4"] = filepath;
                            }
                            else
                            {
                                row["Person4"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }

                        builder.MoveToMergeField("Person4");
                        builder.InsertImage(row["Person4"].ToString(), 80, 35);
                        DateTime ApproveDate4 = entity.AUDITTIME.Value;
                        row["ApproveDate4"] = ApproveDate4.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "5")
                    {
                        row["Dept4"] = entity.AUDITDEPT.ToString();
                        //������ר�����
                        row["ApproveIdea5"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        //row["Person5"] = entity.AUDITPEOPLE.ToString();
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person5"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person5"] = filepath;
                            }
                            else
                            {
                                row["Person5"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }

                        builder.MoveToMergeField("Person5");
                        builder.InsertImage(row["Person5"].ToString(), 80, 35);
                        DateTime ApproveDate5 = entity.AUDITTIME.Value;
                        row["ApproveDate5"] = ApproveDate5.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "6")
                    {
                        row["Dept4"] = entity.AUDITDEPT.ToString();
                        //���������������
                        row["ApproveIdea6"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        //row["Person6"] = entity.AUDITPEOPLE.ToString();
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person6"] = Server.MapPath("~/content/Images/no_1.png");
                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person6"] = filepath;
                            }
                            else
                            {
                                row["Person6"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }

                        builder.MoveToMergeField("Person6");
                        builder.InsertImage(row["Person6"].ToString(), 80, 35);
                        DateTime ApproveDate6 = entity.AUDITTIME.Value;
                        row["ApproveDate6"] = ApproveDate6.ToString("yyyy��MM��dd��");
                    }
                    if (entity.REMARK == "7")
                    {
                        //��˾�쵼���
                        row["ApproveIdea7"] = !string.IsNullOrEmpty(entity.AUDITOPINION) ? entity.AUDITOPINION.ToString() : "";
                        //row["Person7"] = entity.AUDITPEOPLE.ToString();
                        // var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                        if (string.IsNullOrWhiteSpace(entity.AUDITSIGNIMG))
                        {
                            row["Person7"] = Server.MapPath("~/content/Images/no_1.png");

                        }
                        else
                        {
                            var filepath = Server.MapPath("~/") + entity.AUDITSIGNIMG.ToString().Replace("../../", "").ToString();
                            if (System.IO.File.Exists(filepath))
                            {
                                row["Person7"] = filepath;
                            }
                            else
                            {
                                row["Person7"] = Server.MapPath("~/content/Images/no_1.png");
                            }
                        }
                        builder.MoveToMergeField("Person7");
                        builder.InsertImage(row["Person7"].ToString(), 80, 35);

                        DateTime ApproveDate7 = entity.AUDITTIME.Value;
                        row["ApproveDate7"] = ApproveDate7.ToString("yyyy��MM��dd��");
                    }
                }
            }
            dt.Rows.Add(row);
            doc.MailMerge.Execute(dt);
            doc.MailMerge.DeleteFields();
            doc.Save(resp, Server.UrlEncode(fileName), ContentDisposition.Attachment, Aspose.Words.Saving.SaveOptions.CreateSaveOptions(SaveFormat.Doc));
        }

        /// <summary>
        /// ������������������
        /// </summary>
        /// <param name="keyValue">����������¼ID</param>
        /// <param name="projectId">�������id</param>
        /// <returns></returns>
        public ActionResult ExportHistoryWord(string historyKeyValue)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            var tempconfig = new TempConfigBLL().GetList("").Where(x => x.DeptCode == curUser.OrganizeCode && x.ModuleCode == "SCLA").ToList();
            string tempPath = @"Resource\ExcelTemplate\������������ģ��.doc";
            var tempEntity = tempconfig.FirstOrDefault();
            string fileName = "��������������_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".doc";
            if (tempconfig.Count > 0)
            {
                if (tempEntity != null)
                {
                    switch (tempEntity.ProessMode)
                    {
                        case "TY"://ͨ�ô���ʽ
                            tempPath = @"Resource\ExcelTemplate\������������ģ��.doc";
                            break;
                        case "HRCB"://����
                            tempPath = @"Resource\ExcelTemplate\������������ģ��.doc";
                            break;
                        case "GDXY"://��������
                            tempPath = @"Resource\ExcelTemplate\����������������������.doc";
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                tempPath = @"Resource\ExcelTemplate\������������ģ��.doc";
            }
            ExportHisTyData(historyKeyValue, tempPath, fileName);

            return Success("�����ɹ�!");

        }
        #endregion
    }
}
