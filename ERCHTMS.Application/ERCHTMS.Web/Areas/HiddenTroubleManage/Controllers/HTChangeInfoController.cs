using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Entity.BaseManage;
using System.Data;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using System.Collections.Generic;
using System;
using System.Linq;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTChangeInfoController : MvcControllerBase
    {
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL();
        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTExtensionBLL htextensionbll = new HTExtensionBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private WfControlBLL wfcontrolbll = new WfControlBLL();//�Զ������̷���
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
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
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Approval()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeliverForm() 
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
        public ActionResult GetListJson(string queryJson)
        {
            var data = htchangeinfobll.GetList(queryJson);
            return ToJsonResult(data);
        }

        /// <summary>
        /// ���ݲ��ű����ȡʵ��
        /// </summary>
        /// <param name="HidCode"></param>
        /// <returns></returns>
        public ActionResult GetEntityByCode(string keyValue)
        {
            var data = htchangeinfobll.GetEntityByCode(keyValue);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htchangeinfobll.GetHistoryList(keyCode);
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
            var data = htchangeinfobll.GetEntity(keyValue);
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
        [HandlerMonitor(6, "ɾ������������Ϣ")]
        public ActionResult RemoveForm(string keyValue)
        {
            htchangeinfobll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }


        /// <summary>
        /// �������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, HTChangeInfoEntity entity)
        {
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //����ID
            htchangeinfobll.SaveForm(CHANGEID, entity);
            return Success("�����ɹ���");
        }



        /// <summary>
        /// ����������������
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSettingForm(string keyCode, HTChangeInfoEntity entity)
        {
            Operator curUser = new OperatorProvider().Current();
            string hidid = Request.Form["HIDID"] != null ? Request.Form["HIDID"].ToString() : ""; //��������
            HTBaseInfoEntity bsentity = new HTBaseInfoBLL().GetEntity(hidid);//����
            string rankname = string.Empty;

            bool isUpdateDate = false; //�Ƿ����ʱ��

            var cEntity = htchangeinfobll.GetEntityByCode(keyCode); //����HidCode��ȡ���Ķ���
            string postponereason = Request.Form["POSTPONEREASON"] != null ? Request.Form["POSTPONEREASON"].ToString() : "";
            string controlmeasure = Request.Form["CONTROLMEASURE"] != null ? Request.Form["CONTROLMEASURE"].ToString() : ""; //�ܿش�ʩ

            try
            {
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = hidid; //
                wfentity.argument1 = bsentity.MAJORCLASSIFY;
                wfentity.argument3 = bsentity.HIDTYPE; //�������
                wfentity.startflow = "������������";
                wfentity.submittype = "�ύ";
                wfentity.rankid = bsentity.HIDRANK;
                wfentity.user = curUser;
                wfentity.mark = "������������";
                wfentity.organizeid = bsentity.HIDDEPART; //��Ӧ�糧id

                //��ȡ��һ���̵Ĳ�����
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //���������¼
                HTExtensionEntity exentity = new HTExtensionEntity();
                //����ɹ�
                if (result.code == WfCode.Sucess)
                {
                    string participant = result.actionperson;
                    string wfFlag = result.wfflag;

                    cEntity.POSTPONEPERSON = "," + participant + ",";  // ���ڵ�ǰ���˻��ж��Ƿ���в�����Ȩ��
                    cEntity.POSTPONEDAYS = entity.POSTPONEDAYS; //��������
                    cEntity.POSTPONEDEPT = result.deptcode;  //��������Code
                    cEntity.POSTPONEDEPTNAME = result.deptname;  //������������
                    cEntity.POSTPONEPERSONNAME = result.username;
                    cEntity.APPLICATIONSTATUS = wfFlag;
                    //�Ƿ����ʱ�䣬�ۼ�����
                    if (wfFlag == "2") { isUpdateDate = true; }
                    //���������������������ͨ������������Ľ���ʱ�䡢����ʱ�䣬������Ӧ����������
                    if (isUpdateDate)
                    {
                        //���¸�ֵ���Ľ���ʱ��
                        cEntity.CHANGEDEADINE = cEntity.CHANGEDEADINE.Value.AddDays(cEntity.POSTPONEDAYS);

                        //��������ʱ��
                        HTAcceptInfoEntity aEntity = htacceptinfobll.GetEntityByHidCode(keyCode);
                        if (null != aEntity.ACCEPTDATE) 
                        {
                            aEntity.ACCEPTDATE = aEntity.ACCEPTDATE.Value.AddDays(cEntity.POSTPONEDAYS);
                        }
                        htacceptinfobll.SaveForm(aEntity.ID, aEntity);

                        exentity.HANDLESIGN = "1"; //�ɹ����
                    }
                    cEntity.APPSIGN = "Web";
                    //����������Ϣ
                    htchangeinfobll.SaveForm(cEntity.ID, cEntity); //������������

                    exentity.HIDCODE = cEntity.HIDCODE;
                    exentity.HIDID = hidid;
                    exentity.HANDLEDATE = DateTime.Now;
                    exentity.POSTPONEDAYS = entity.POSTPONEDAYS.ToString();
                    exentity.HANDLEUSERID = curUser.UserId;
                    exentity.HANDLEUSERNAME = curUser.UserName;
                    exentity.HANDLEDEPTCODE = curUser.DeptCode;
                    exentity.HANDLEDEPTNAME = curUser.DeptName;
                    exentity.HANDLETYPE = "0";  //��������  ״̬���� 2 ʱ��ʾ����������� (����)
                    exentity.HANDLEID = DateTime.Now.ToString("yyyyMMddhhmmss").ToString();
                    exentity.POSTPONEREASON = postponereason;//��������
                    exentity.CONTROLMEASURE = controlmeasure;//��ʱ�ܿش�ʩ
                    exentity.APPSIGN = "Web";
                    htextensionbll.SaveForm("", exentity);

                    //��������
                    htworkflowbll.PushMessageForWorkFlow("������������", "������������", wfentity, result); 

                    return Success(result.message);
                }
                else
                {
                    return Error(result.message);
                }
            }
            catch (Exception ex)
            {
                 return Error(ex.Message.ToString());
            }
        }

        /// <summary>
        /// �ύ�����������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, HTBaseInfoEntity bEntity, HTChangeInfoEntity entity, HTAcceptInfoEntity aEntity)
        {

            Operator curUser = OperatorProvider.Provider.Current();

            string CHANGERESULT = Request.Form["CHANGERESULT"] != null ? Request.Form["CHANGERESULT"].ToString() : ""; //���Ľ��
            string ISBACK = Request.Form["ISBACK"] != null ? Request.Form["ISBACK"].ToString() : ""; //�Ƿ����
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //����ID

            string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա
            string wfFlag = string.Empty; //���̱�ʶ
            try
            {

                if (!string.IsNullOrEmpty(CHANGEID))
                {
                    var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                    entity.AUTOID = tempEntity.AUTOID;
                    entity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                    entity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                    entity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                    entity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
                }
                entity.APPSIGN = "Web";
                htchangeinfobll.SaveForm(CHANGEID, entity);

                //����������Ϣ
                var baseEntity = htbaseinfobll.GetEntity(keyValue);


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.argument1 = bEntity.MAJORCLASSIFY; //רҵ����
                wfentity.argument3 = bEntity.HIDTYPE; //�������
                wfentity.argument4 = bEntity.HIDBMID; //��������
                wfentity.startflow = baseEntity.WORKSTREAM;
                wfentity.rankid = baseEntity.HIDRANK;
                wfentity.user = curUser;
                wfentity.organizeid = baseEntity.HIDDEPART; //��Ӧ�糧id
                if (baseEntity.ADDTYPE == "2")
                {
                    wfentity.mark = "ʡ�������Ų�";
                }
                else
                {
                    wfentity.mark = "���������Ų�";
                }
                //�˻� 
                if (ISBACK == "1")
                {
                    //��ʷ��¼
                    var changeitem = htchangeinfobll.GetHistoryList(entity.HIDCODE).ToList();
                    //���δ���Ŀ����˻�
                    if (changeitem.Count() == 0)
                    {
                        wfentity.submittype = "�˻�";
                    }
                    else
                    {
                        return Error("���Ĺ���������޷��ٴ��˻�!");
                    }
                }
                else //�����ύ����������
                {
                    wfentity.submittype = "�ύ";
                }


                //��ȡ��һ���̵Ĳ�����
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //����ɹ�
                if (result.code == WfCode.Sucess)
                {
                    //������
                    participant = result.actionperson;
                    //״̬
                    wfFlag = result.wfflag;

                    //����Ǹ���״̬
                    if (result.ischangestatus)
                    {
                        if (!string.IsNullOrEmpty(participant) && !string.IsNullOrEmpty(wfFlag))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                            }

                            string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                            if (tagName == "�ƶ����ļƻ�" && wfentity.submittype == "�˻�")
                            {
                                UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                                entity.CHARGEPERSONNAME  = userentity.RealName;
                                entity.CHARGEPERSON = userentity.Account;
                                entity.CHARGEDEPTID = userentity.DepartmentId;
                                entity.CHARGEDEPTNAME = userentity.DeptName;
                                entity.ISAPPOINT = "0";
                                htchangeinfobll.SaveForm(entity.ID, entity);
                            }
                        }
                    }
                    else
                    {
                        //��ȡ
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                        #region ��ǰ�������������Ľ׶�
                        if (tagName == "��������")
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            entity.CHANGEPERSONNAME = userentity.RealName;
                            entity.CHANGEPERSON = userentity.UserId;
                            entity.CHANGEDUTYDEPARTNAME = userentity.DeptName;
                            entity.CHANGEDUTYDEPARTID = userentity.DepartmentId;
                            entity.CHANGEDUTYDEPARTCODE = userentity.DepartmentCode;
                            entity.CHANGEDUTYTEL = userentity.Telephone;
                            htchangeinfobll.SaveForm(entity.ID, entity);
                        }
                        #endregion
                    }
                }
                //���Զ����������
                else if (result.code == WfCode.NoAutoHandle)
                {
                    bool isupdate = false;//�Ƿ��������״̬
                    //�˻ز���  ��������
                    if (ISBACK == "1")
                    {
                        DataTable dt = htworkflowbll.GetBackFlowObjectByKey(keyValue);

                        if (dt.Rows.Count > 0)
                        {
                            wfFlag = dt.Rows[0]["wfflag"].ToString(); //��������

                            participant = dt.Rows[0]["participant"].ToString();  //ָ����

                            isupdate = dt.Rows[0]["isupdate"].ToString() == "1"; //�Ƿ��������״̬
                        }
                    }

                    //��������״̬�������
                    if (isupdate)
                    {
                        if (!string.IsNullOrEmpty(participant) && !string.IsNullOrEmpty(wfFlag))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                            }

                            string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                            if (tagName == "�ƶ����ļƻ�" && wfentity.submittype == "�˻�")
                            {
                                UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                                entity.CHARGEPERSONNAME = userentity.RealName;
                                entity.CHARGEPERSON = userentity.Account;
                                entity.CHARGEDEPTID = userentity.DepartmentId;
                                entity.CHARGEDEPTNAME = userentity.DeptName;
                                entity.ISAPPOINT = "0";
                                htchangeinfobll.SaveForm(entity.ID, entity);
                            }

                            result.message = "����ɹ�";
                            result.code = WfCode.Sucess;
                        }
                    }
                    else
                    {
                        //��ȡ
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                        #region ��ǰ�������������Ľ׶�
                        if (tagName == "��������")
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            entity.CHANGEPERSONNAME = userentity.RealName;
                            entity.CHANGEPERSON = userentity.UserId;
                            entity.CHANGEDUTYDEPARTNAME = userentity.DeptName;
                            entity.CHANGEDUTYDEPARTID = userentity.DepartmentId;
                            entity.CHANGEDUTYDEPARTCODE = userentity.DepartmentCode;
                            entity.CHANGEDUTYTEL = userentity.Telephone;
                            htchangeinfobll.SaveForm(entity.ID, entity);
                        }
                        #endregion
                    }
                }

                if (result.code == WfCode.Sucess)
                {
                    return Success(result.message);
                }
                else //��������״̬
                {
                    return Error(result.message);
                }
            }
            catch (Exception ex)
            {
              return Error(ex.Message);
            }
          
        }
        #endregion
    }
}
