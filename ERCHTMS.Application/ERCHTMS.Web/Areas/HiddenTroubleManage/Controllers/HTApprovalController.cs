using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using System;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Entity.BaseManage;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTApprovalController : MvcControllerBase
    {
        private HTApprovalBLL htapprovebll = new HTApprovalBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //��������
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL(); //��������
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //������Ϣ
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL(); //��������
        private DepartmentBLL departmentbll = new DepartmentBLL(); //���Ų�������
        private UserBLL userbll = new UserBLL(); //��Ա��������
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();

        private WfControlBLL wfcontrolbll = new WfControlBLL();//�Զ������̷���

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
            Operator curUser = OperatorProvider.Provider.Current();

            string actionName = string.Empty;

            string GDXJ_HYC_ORGCODE = dataitemdetailbll.GetItemValue("GDXJ_HYC_ORGCODE");

            //�����½������ר��
            if (curUser.OrganizeCode == GDXJ_HYC_ORGCODE)
            {
                string[] allKeys = Request.QueryString.AllKeys;
                if (allKeys.Count() > 0)
                {
                    actionName = "HYCForm?";
                    int num = 0;
                    foreach (string str in allKeys)
                    {
                        string strValue = Request.QueryString[str];
                        if (num == 0)
                        {
                            actionName += str + "=" + strValue;
                        }
                        else
                        {
                            actionName += "&" + str + "=" + strValue;
                        }
                        num++;
                    }
                }
                else
                {
                    actionName = "HYCForm";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HYCForm()
        {
            return View();
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CForm()
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
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(string hideCode)
        {
            var data = htapprovebll.GetList(hideCode);
            return ToJsonResult(data);
        }

        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htapprovebll.GetHistoryList(keyCode);
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
            var data = htapprovebll.GetEntity(keyValue);
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
            htapprovebll.RemoveForm(keyValue);
            return Success("ɾ���ɹ���");
        }
        #endregion


        #region �����ύ����(����)
        /// <summary>
        /// �����ύ
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string isUpSubmit, HTBaseInfoEntity bentity, HTApprovalEntity entity, HTChangeInfoEntity chEntity, HTAcceptInfoEntity aEntity)
        {

            Operator curUser = OperatorProvider.Provider.Current();

            string wfFlag = string.Empty;  //���̱�ʶ

            #region ���������Ϣ

            //����ID
            string APPROVALID = Request.Form["APPROVALID"] != null ? Request.Form["APPROVALID"].ToString() : "";

            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : "";

            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : "";

            APPROVALID = APPROVALID == "&nbsp;" ? "" : APPROVALID;

            //�����ع�
            string EXPOSURESTATE = Request.Form["EXPOSURESTATE"] != null ? Request.Form["EXPOSURESTATE"].ToString() : "";
            //�豸
            if (string.IsNullOrEmpty(bentity.DEVICEID))
            {
                bentity.DEVICEID = string.Empty;
            }
            if (string.IsNullOrEmpty(bentity.DEVICENAME))
            {
                bentity.DEVICENAME = string.Empty;
            }
            if (string.IsNullOrEmpty(bentity.DEVICECODE))
            {
                bentity.DEVICECODE = string.Empty;
            }
            //��������������Ϣ
            htbaseinfobll.SaveForm(keyValue, bentity);

            //��������
            if (!string.IsNullOrEmpty(CHANGEID))
            {
                var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                chEntity.AUTOID = tempEntity.AUTOID;
                chEntity.BACKREASON = "";  //����ԭ��
                chEntity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                chEntity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                chEntity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                chEntity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
            }
            htchangeinfobll.SaveForm(CHANGEID, chEntity);

            //��������
            if (!string.IsNullOrEmpty(ACCEPTID))
            {
                var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                aEntity.AUTOID = tempEntity.AUTOID;
            }
            htacceptinfobll.SaveForm(ACCEPTID, aEntity);
            #endregion

            string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա

            HTBaseInfoEntity baseEntity = htbaseinfobll.GetEntity(keyValue);

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue;
            wfentity.argument1 = bentity.MAJORCLASSIFY; //רҵ����
            wfentity.argument2 = curUser.DeptId; //��ǰ����
            wfentity.argument3 = bentity.HIDTYPE; //�������
            wfentity.argument4 = bentity.HIDBMID; //��������
            string startflow = baseEntity.WORKSTREAM;
            wfentity.startflow = startflow;
            wfentity.rankid = baseEntity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "���������Ų�";
            wfentity.organizeid = baseEntity.HIDDEPART; //��Ӧ�糧id
            //���ؽ��
            WfControlResult result = new WfControlResult();

            if (isUpSubmit == "1")  //�ϱ����Ҵ����ϼ�����
            {
                #region �ϱ�

                wfentity.submittype = "�ϱ�";
                //��ȡ��һ���̵Ĳ�����
                result = wfcontrolbll.GetWfControl(wfentity);

                //����ɹ�
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson; //Ŀ�����̲�����

                    if (!string.IsNullOrEmpty(participant))
                    {
                        //��������������Ϣ
                        htapprovebll.SaveForm(APPROVALID, entity);

                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        return Success(result.message);
                    }
                    else
                    {
                        return Error("��ǰ�ϼ�������������Ա,�����ϱ�,����ϵϵͳ����Ա��������!");
                    }

                }
                else
                {
                    return Error(result.message);
                }
                #endregion
            }
            else  //���ϱ�������ͨ����Ҫ�ύ���ģ�������ͨ���˻ص��Ǽ�
            {
                /****�жϵ�ǰ���Ƿ�����ͨ��*****/
                #region �жϵ�ǰ���Ƿ�����ͨ��
                //����ͨ������ֱ�ӽ�������
                if (entity.APPROVALRESULT == "1")
                {
                    wfentity.submittype = "�ύ";
                    //��ָ������������
                    if (chEntity.ISAPPOINT == "0")
                    {
                        wfentity.submittype = "�ƶ��ύ";
                    }
                    //�ж��Ƿ���ͬ���ύ
                    bool ismajorpush = GetCurUserWfAuth(baseEntity.HIDRANK, "��������", "��������", "���������Ų�", "ͬ���ύ", baseEntity.MAJORCLASSIFY, null, null, keyValue);
                    if (ismajorpush) 
                    {
                        wfentity.submittype = "ͬ���ύ";
                    }

                    #region �����½��汾
                    if (baseEntity.ADDTYPE == "3")
                    {
                        //�Ǳ���������
                        if (baseEntity.ISSELFCHANGE == "0")
                        {
                            wfentity.submittype = "�ƶ��ύ";

                            //����Ѿ��ƶ������ļƻ�,�����ύ����������
                            if (baseEntity.ISFORMULATE == "1")
                            {
                                wfentity.submittype = "�ύ";
                            }
                            //�����ǰ�������������Ĳ��ţ���ֱ���ύ
                            if (curUser.DeptId == chEntity.CHANGEDUTYDEPARTID)
                            {
                                wfentity.submittype = "�ύ";
                            }
                            //�����ǰ���������Ǵ������ţ���ֱ���ύ���Ǳ��������ĵİ��ಿ
                            if (curUser.DeptCode == baseEntity.CREATEUSERDEPTCODE)
                            {
                                wfentity.submittype = "�ƶ��ύ";
                            }
                        }
                        else  //��������������£� ��˾���û�������Σ�������ֱ�ӵ�������
                        {
                            UserEntity userEntity = userbll.GetEntity(baseEntity.CREATEUSERID);
                            if (userEntity.RoleName.Contains("��˾���û�") && curUser.RoleName.Contains("��˾���û�"))
                            {
                                wfentity.submittype = "�ƶ��ύ";
                            }
                        }
                    } 
                    #endregion
                }
                else  //������ͨ�����˻ص��Ǽ� 
                {
                    wfentity.submittype = "�˻�";

                    #region �����½��汾
                    if (baseEntity.ADDTYPE == "3")
                    {
                        //�Ѿ��ƶ������ļƻ��������ƶ��ƻ��˻�
                        if (baseEntity.ISFORMULATE == "1")
                        {
                            wfentity.submittype = "�ƶ��˻�";
                        }
                    } 
                    #endregion
                }
                //��ȡ��һ���̵Ĳ�����
                result = wfcontrolbll.GetWfControl(wfentity);
                //����ɹ�
                if (result.code == WfCode.Sucess)
                {
                    participant = result.actionperson;

                    wfFlag = result.wfflag;

                    if (!string.IsNullOrEmpty(participant))
                    {
                        //����Ǹ���״̬
                        if (result.ischangestatus)
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);
                            if (count > 0)
                            {
                                //��������������Ϣ
                                htapprovebll.SaveForm(APPROVALID, entity);
                                htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                                return Success(result.message);
                            }
                            else
                            {
                                return Error("��ǰ�û�������Ȩ��!");
                            }
                        }
                        else  //������״̬�������
                        {
                            //��������������Ϣ
                            htapprovebll.SaveForm(APPROVALID, entity);

                            htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                            return Success(result.message);
                        }
                    }
                    else
                    {
                        return Error("Ŀ�����̲�����δ����!");
                    }

                }
                else
                {
                    return Error(result.message);
                }
                #endregion
            }

        }
        #endregion

        #region ʡ���Ǽǵ������ύ���ϱ�(����)
        /// <summary>
        /// �����ύ
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveCForm(string keyValue, string isUpSubmit, HTBaseInfoEntity bentity, HTApprovalEntity entity, HTChangeInfoEntity chEntity, HTAcceptInfoEntity aEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            string wfFlag = string.Empty;  //���̱�ʶ

            IList<UserEntity> ulist = new List<UserEntity>();

            #region ���������Ϣ

            //����ID
            string APPROVALID = Request.Form["APPROVALID"] != null ? Request.Form["APPROVALID"].ToString() : "";

            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : "";

            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : "";

            APPROVALID = APPROVALID == "&nbsp;" ? "" : APPROVALID;

            //�����ع�
            string EXPOSURESTATE = Request.Form["EXPOSURESTATE"] != null ? Request.Form["EXPOSURESTATE"].ToString() : "";
            //��������������Ϣ
            htbaseinfobll.SaveForm(keyValue, bentity);

            //��������
            if (!string.IsNullOrEmpty(CHANGEID))
            {
                var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                chEntity.AUTOID = tempEntity.AUTOID;
                chEntity.BACKREASON = "";  //����ԭ��
                chEntity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                chEntity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                chEntity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                chEntity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
            }
            htchangeinfobll.SaveForm(CHANGEID, chEntity);

            //��������
            if (!string.IsNullOrEmpty(ACCEPTID))
            {
                var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                aEntity.AUTOID = tempEntity.AUTOID;
            }
            htacceptinfobll.SaveForm(ACCEPTID, aEntity);
            #endregion

            string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա

            bool isgoback = false;

            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue;
            string startflow = htbaseinfobll.GetEntity(keyValue).WORKSTREAM;
            wfentity.startflow = startflow;
            wfentity.rankid = bentity.HIDRANK;
            wfentity.user = curUser;
            wfentity.mark = "ʡ�������Ų�";
            wfentity.organizeid = bentity.HIDDEPART; //��Ӧ�糧id
            //���ؽ��
            WfControlResult result = new WfControlResult();

            if (isUpSubmit == "1")  //�ϱ����Ҵ����ϼ�����
            {
                wfentity.submittype = "�ϱ�";
            }
            else  //���ϱ�������ͨ����Ҫ�ύ���ģ�������ͨ���˻ص��Ǽ�
            {
                /****�жϵ�ǰ���Ƿ�����ͨ��*****/
                #region �жϵ�ǰ���Ƿ�����ͨ��
                //����ͨ������ֱ�ӽ�������
                if (entity.APPROVALRESULT == "1")
                {
                    wfentity.submittype = "�ύ";
                }
                else  //������ͨ�����˻ص��Ǽ� 
                {
                    wfentity.submittype = "�˻�";
                    isgoback = true;
                }
                #endregion
            }

   
            //��ȡ��һ���̵Ĳ�����
            result = wfcontrolbll.GetWfControl(wfentity);

            //���ز�������ɹ�
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;
                wfFlag = result.wfflag;
                //����״̬
                if (result.ischangestatus)
                {
                    if (!string.IsNullOrEmpty(participant))
                    {
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            //��������������Ϣ
                            htapprovebll.SaveForm(APPROVALID, entity);
                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                        }
                        else
                        {
                            return Error("��ǰ�û�������Ȩ��!");
                        }
                    }
                }
                else //������״̬
                {
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //��������������Ϣ
                        htapprovebll.SaveForm(APPROVALID, entity);

                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                    }
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }

        }
        #endregion

        public bool GetCurUserWfAuth(string rankid, string workflow, string endflow, string mark, string submittype, string arg1 = "", string arg2 = "", string arg3 = "", string businessid = "")
        {
            Operator curUser = OperatorProvider.Provider.Current();
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = businessid; 
            wfentity.startflow = workflow;
            wfentity.endflow = endflow;
            wfentity.submittype = submittype;
            wfentity.rankid = rankid;
            wfentity.user = curUser;
            wfentity.mark = mark;
            wfentity.isvaliauth = true;
            wfentity.argument1 = arg1;
            wfentity.argument2 = arg2;
            wfentity.argument3 = arg3;

            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);

            return result.ishave;
        }
    }
}
