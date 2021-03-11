using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.Busines.RoutineSafetyWork;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using System.Collections.Generic;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using System.Linq;
using System;
using BSFramework.Util.Extension;

namespace ERCHTMS.Web.Areas.RoutineSafetyWork.Controllers
{
    /// <summary>
    /// �� ������ȫ��̬
    /// </summary>
    public class SecurityDynamicsController : MvcControllerBase
    {
        private SecurityDynamicsBLL securitydynamicsbll = new SecurityDynamicsBLL();
        private DailyexamineBLL dailyexaminebll = new DailyexamineBLL();
        private ManyPowerCheckBLL manyPowerCheckbll = new ManyPowerCheckBLL();
        private AptitudeinvestigateauditBLL aptitudeinvestigateauditbll = new AptitudeinvestigateauditBLL();
        #region ��ͼ����
        /// <summary>
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            //�Ƿ���Ҫ���
            ViewBag.IsCheck = 0;
            //��ѯ�Ƿ���������
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "��ȫ��̬";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            if (powerList.Count > 0)
                ViewBag.IsCheck = powerList.Count;
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            //�Ƿ���Ҫ���
            ViewBag.IsCheck = 0;
            //��ѯ�Ƿ���������
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "��ȫ��̬";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            if (powerList.Count > 0)
                ViewBag.IsCheck = powerList.Count;
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
            //�Ƿ���Ҫ���
            ViewBag.IsCheck = 0;
            //��ѯ�Ƿ���������
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            string moduleName = "��ȫ��̬";
            List<ManyPowerCheckEntity> powerList = manyPowerCheckbll.GetListBySerialNum(user.OrganizeCode, moduleName);
            

            pagination.p_kid = "ID";
            pagination.p_fields = "createuserid,Title,Publisher,ReleaseTime,IsSend,isover,flowdeptname,createusername,createdate,flowdept,flowrolename,flowrole,flowname";
            pagination.p_tablename = "BIS_SecurityDynamics t";
            //Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            pagination.conditionJson = string.Format(" CREATEUSERORGCODE ='{0}'", user.OrganizeCode);
            if (powerList.Count > 0)
            {
                var queryParam = queryJson.ToJObject();
                if (!queryParam["pagemode"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(" and (ISOVER='0' and flowdept='{1}')", user.UserId, user.DeptId);
                }
                else
                {
                    if (user.RoleName.IndexOf("������") >= 0)
                    {
                        pagination.conditionJson += string.Format(" and ((IsSend='0' and ISOVER='1') or (createuserid='{0}') or (ISOVER='0' and flowdept='{1}'))", user.UserId, user.DeptId);
                    }
                    else
                    {
                        pagination.conditionJson += string.Format(" and ((IsSend='0' and ISOVER='1') or (createuserid='{0}'))", user.UserId, user.DeptId);
                    }
                    //pagination.conditionJson += string.Format(" and ((IsSend='0' and ISOVER='1') or (createuserid='{0}') or (ISOVER='0' and flowdept='{1}'))", user.UserId, user.DeptId);
                }
            }
            else
            {
                pagination.conditionJson += string.Format(" and (IsSend='0' or createuserid='{0}')", user.UserId);
            }
            var watch = CommonHelper.TimerStart();
            var data = securitydynamicsbll.GetPageList(pagination, queryJson);
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
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string queryJson)
        {
            var data = securitydynamicsbll.GetList(queryJson);
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
            var data = securitydynamicsbll.GetEntity(keyValue);
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
            securitydynamicsbll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
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
        public ActionResult SaveForm(string keyValue, SecurityDynamicsEntity entity)
        {
            if (entity.IsSend == "0")//�ύ
            {
                entity.ReleaseTime = DateTime.Now;//����ʱ��
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.ISOVER = "1";
                entity.FLOWNAME = "";
            }
            if (entity.IsSend == "1")//����
            {
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.ISOVER = "0";
                entity.FLOWNAME = "������";
            }
            securitydynamicsbll.SaveForm(keyValue, entity);
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
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, SecurityDynamicsEntity entity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string outengineerid = curUser.DeptId;
            string flowid = string.Empty;

            string moduleName = "��ȫ��̬";
            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="outengineerid">����Id</param>
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, curUser.DeptId);

            //����ʱ����ݽ�ɫ�Զ����,��ʱ����ݹ��̺�������ò�ѯ�������Id
            OutsouringengineerEntity engineerEntity = new OutsouringengineerBLL().GetEntity(curUser.DeptId);
            List<ManyPowerCheckEntity> powerList = new ManyPowerCheckBLL().GetListBySerialNum(curUser.OrganizeCode, "��ȫ��̬");
            List<ManyPowerCheckEntity> checkPower = new List<ManyPowerCheckEntity>();
            //�Ȳ��ִ�в��ű���
            for (int i = 0; i < powerList.Count; i++)
            {
                if (powerList[i].CHECKDEPTCODE == "-1" || powerList[i].CHECKDEPTID == "-1")
                {
                    //powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntity(entity.PROJECTID).EnCode;
                    //powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntity(entity.PROJECTID).DepartmentId;
                    //powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).EnCode;
                    //powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).DepartmentId;
                    powerList[i].CHECKDEPTCODE = curUser.DeptCode;
                    powerList[i].CHECKDEPTID = curUser.DeptId;
                }
                //��������
                if (powerList[i].CHECKDEPTCODE == "-3" || powerList[i].CHECKDEPTID == "-3")
                {
                    if (entity.CreateUserDeptCode == null || entity.CreateUserDeptCode == "")
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntityByCode(curUser.DeptCode).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntityByCode(curUser.DeptCode).DepartmentId;
                    }
                    else
                    {
                        powerList[i].CHECKDEPTCODE = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).EnCode;
                        powerList[i].CHECKDEPTID = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).DepartmentId;
                    }
                }
            }
            //��¼���Ƿ������Ȩ��--�����Ȩ��ֱ�����ͨ��
            for (int i = 0; i < powerList.Count; i++)
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
                //���氲ȫ��̬��¼
                entity.FLOWDEPT = mpcEntity.CHECKDEPTID;
                entity.FLOWDEPTNAME = mpcEntity.CHECKDEPTNAME;
                entity.FLOWROLE = mpcEntity.CHECKROLEID;
                entity.FLOWROLENAME = mpcEntity.CHECKROLENAME;
                entity.IsSend = "0"; //����Ѿ��ӵǼǵ���˽׶�
                entity.ISOVER = "0"; //����δ��ɣ�1��ʾ���
                entity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "�����";
                entity.FlowId = mpcEntity.ID;
                //DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                //var userAccount = dt.Rows[0]["account"].ToString();
                //var userName = dt.Rows[0]["realname"].ToString();
                //JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
            }
            else  //Ϊ�����ʾ�Ѿ��������
            {
                entity.FLOWDEPT = "";
                entity.FLOWDEPTNAME = "";
                entity.FLOWROLE = "";
                entity.FLOWROLENAME = "";
                entity.IsSend = "0"; //����Ѿ��ӵǼǵ���˽׶�
                entity.ISOVER = "1"; //����δ��ɣ�1��ʾ���
                entity.FLOWNAME = "";
                entity.FlowId = flowid;
                entity.ReleaseTime = DateTime.Now;//����ʱ��
            }
            securitydynamicsbll.SaveForm(keyValue, entity);

            //�����˼�¼
            if (state == "1")
            {
                //�����Ϣ��
                AptitudeinvestigateauditEntity aidEntity = new AptitudeinvestigateauditEntity();
                aidEntity.AUDITRESULT = "0"; //ͨ��
                aidEntity.AUDITTIME = DateTime.Now;
                aidEntity.AUDITPEOPLE = curUser.UserName;
                aidEntity.AUDITPEOPLEID = curUser.UserId;
                aidEntity.APTITUDEID = entity.Id;  //������ҵ��ID 
                aidEntity.AUDITOPINION = ""; //������
                aidEntity.AUDITSIGNIMG = curUser.SignImg;
                if (null != mpcEntity)
                {
                    aidEntity.REMARK = (powerList[0].AUTOID.Value - 1).ToString(); //��ע �����̵�˳���

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
        [ValidateInput(false)]
        [AjaxOnly]
        public ActionResult ApporveForm(string keyValue, SecurityDynamicsEntity entity, AptitudeinvestigateauditEntity aentity)
        {
            Operator curUser = ERCHTMS.Code.OperatorProvider.Provider.Current();

            string state = string.Empty;

            string moduleName = "��ȫ��̬";

            entity = securitydynamicsbll.GetEntity(keyValue);

            string outengineerid = new DepartmentBLL().GetEntityByCode(entity.CreateUserDeptCode).DepartmentId;

            /// <param name="currUser">��ǰ��¼��</param>
            /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
            /// <param name="moduleName">ģ������</param>
            /// <param name="outengineerid">����Id</param>
            //ManyPowerCheckEntity mpcEntity = peoplereviewbll.CheckAuditPower(curUser, out state, moduleName, outengineerid);
            ManyPowerCheckEntity mpcEntity = dailyexaminebll.CheckAuditPower(curUser, out state, moduleName, outengineerid);

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

            #region  //���氲ȫ��̬��¼
            var smEntity = securitydynamicsbll.GetEntity(keyValue);
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
                    smEntity.IsSend = "0";
                    smEntity.ISOVER = "0";
                    smEntity.FlowId = mpcEntity.ID;//��ֵ����Id
                    smEntity.FLOWNAME = mpcEntity.CHECKDEPTNAME + "�����";
                    //DataTable dt = new UserBLL().GetUserAccountByRoleAndDept(curUser.OrganizeId, mpcEntity.CHECKDEPTID, mpcEntity.CHECKROLENAME);
                    //var userAccount = dt.Rows[0]["account"].ToString();
                    //var userName = dt.Rows[0]["realname"].ToString();
                    //JPushApi.PushMessage(userAccount, userName, "WB001", entity.ID);
                }
                else
                {
                    smEntity.FLOWDEPT = "";
                    smEntity.FLOWDEPTNAME = "";
                    smEntity.FLOWROLE = "";
                    smEntity.FLOWROLENAME = "";
                    smEntity.IsSend = "0";
                    smEntity.ISOVER = "1";
                    smEntity.FLOWNAME = "";
                    smEntity.ReleaseTime = DateTime.Now;//����ʱ��
                }
            }
            else //��˲�ͨ�� 
            {
                smEntity.FLOWDEPT = "";
                smEntity.FLOWDEPTNAME = "";
                smEntity.FLOWROLE = "";
                smEntity.FLOWROLENAME = "";
                smEntity.IsSend = "1"; //���ڵǼǽ׶�
                smEntity.ISOVER = "0"; //�Ƿ����״̬��ֵΪδ���
                smEntity.FLOWNAME = "��ˣ�����δͨ��";
                smEntity.FlowId = "";//���˺�����Id���
                //var applyUser = new UserBLL().GetEntity(smEntity.CREATEUSERID);
                //if (applyUser != null)
                //{
                //    JPushApi.PushMessage(applyUser.Account, smEntity.CREATEUSERNAME, "WB002", entity.ID);
                //}

            }
            //���°�ȫ��̬����״̬��Ϣ
            securitydynamicsbll.SaveForm(keyValue, smEntity);
            #endregion

            #region    //��˲�ͨ��
            if (aentity.AUDITRESULT == "1")
            {
                //�����ʷ��¼
                //HistoryRiskWorkEntity hsentity = new HistoryRiskWorkEntity();
                //hsentity.CREATEUSERID = smEntity.CREATEUSERID;
                //hsentity.CREATEUSERDEPTCODE = smEntity.CREATEUSERDEPTCODE;
                //hsentity.CREATEUSERORGCODE = smEntity.CREATEUSERORGCODE;
                //hsentity.CREATEDATE = smEntity.CREATEDATE;
                //hsentity.CREATEUSERNAME = smEntity.CREATEUSERNAME;
                //hsentity.MODIFYDATE = smEntity.MODIFYDATE;
                //hsentity.MODIFYUSERID = smEntity.MODIFYUSERID;
                //hsentity.MODIFYUSERNAME = smEntity.MODIFYUSERNAME;
                //hsentity.SUBMITDATE = smEntity.SUBMITDATE;
                //hsentity.SUBMITPERSON = smEntity.SUBMITPERSON;
                //hsentity.PROJECTID = smEntity.PROJECTID;
                //hsentity.CONTRACTID = smEntity.ID; //����ID
                //hsentity.ORGANIZER = smEntity.ORGANIZER;
                //hsentity.ORGANIZTIME = smEntity.ORGANIZTIME;
                //hsentity.ISOVER = smEntity.ISOVER;
                //hsentity.ISSAVED = smEntity.ISSAVED;
                //hsentity.FLOWDEPTNAME = smEntity.FLOWDEPTNAME;
                //hsentity.FLOWDEPT = smEntity.FLOWDEPT;
                //hsentity.FLOWROLENAME = smEntity.FLOWROLENAME;
                //hsentity.FLOWROLE = smEntity.FLOWROLE;
                //hsentity.FLOWNAME = smEntity.FLOWNAME;
                //hsentity.SummitContent = smEntity.SummitContent;
                //hsentity.ID = "";

                //historyRiskWorkbll.SaveForm(hsentity.ID, hsentity);

                //��ȡ��ǰҵ������������˼�¼
                var shlist = aptitudeinvestigateauditbll.GetAuditList(keyValue);
                //����������˼�¼����ID
                foreach (AptitudeinvestigateauditEntity mode in shlist)
                {
                    //mode.APTITUDEID = hsentity.ID; //��Ӧ�µ�ID
                    mode.REMARK = "99";
                    aptitudeinvestigateauditbll.SaveForm(mode.ID, mode);
                }
                //�������¸�����¼����ID
                //var flist = fileinfobll.GetImageListByObject(keyValue);
                //foreach (FileInfoEntity fmode in flist)
                //{
                //    fmode.RecId = hsentity.ID; //��Ӧ�µ�ID
                //    fileinfobll.SaveForm("", fmode);
                //}
            }
            #endregion

            return Success("�����ɹ�!");
        }
        #endregion
    }
}
