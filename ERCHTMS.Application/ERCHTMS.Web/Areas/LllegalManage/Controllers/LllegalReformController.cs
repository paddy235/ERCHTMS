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
using ERCHTMS.Busines.LllegalManage;
using ERCHTMS.Entity.LllegalManage;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    #region Υ��������Ϣ��
    /// <summary>
    /// �� ����Υ��������Ϣ��
    /// </summary>
    public class LllegalReformController : MvcControllerBase
    {

        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // Υ�»�����Ϣ
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //������Ϣ����
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //��׼��Ϣ����
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //������Ϣ����
        private LllegalExtensionBLL lllegalextensionbll = new LllegalExtensionBLL(); //Υ����������

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
        /// �б�ҳ��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult KmIndex() 
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
        /// �б�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DetailList()
        {
            return View();
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Approval() 
        {
            return View();
        }
        
        /// <summary>
        /// ����ת��
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeliverForm() 
        {
            return View();
        }
        #endregion

        #region �������Ϣ
        /// <summary>
        /// �������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, LllegalReformEntity entity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            LllegalReformEntity rEntity = lllegalreformbll.GetEntityByBid(keyValue);
            rEntity.REFORMFINISHDATE = entity.REFORMFINISHDATE;
            rEntity.REFORMMEASURE = entity.REFORMMEASURE;
            rEntity.REFORMSTATUS = entity.REFORMSTATUS;
            rEntity.MODIFYDATE = DateTime.Now;
            rEntity.MODIFYUSERID = curUser.UserId;
            rEntity.MODIFYUSERNAME = curUser.UserName;
            rEntity.REFORMPIC = entity.REFORMPIC;
            rEntity.REFORMATTACHMENT = entity.REFORMATTACHMENT;
            lllegalreformbll.SaveForm(rEntity.ID, rEntity);
            return Success("�����ɹ���");
        }
        #endregion

        #region ����������������
        /// <summary>
        /// ����������������
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSettingForm(string keyValue, LllegalReformEntity entity)
        {
            Operator curUser = new OperatorProvider().Current();
            LllegalRegisterEntity bsentity = lllegalregisterbll.GetEntity(keyValue);//����
            string rankname = string.Empty;

            bool isUpdateDate = false; //�Ƿ����ʱ��

            var cEntity = lllegalreformbll.GetEntityByBid(keyValue); //��ȡ���Ķ���
            string postponereason = Request.Form["POSTPONEREASON"] != null ? Request.Form["POSTPONEREASON"].ToString() : "";


            try
            {
                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue; //
                wfentity.argument1 = bsentity.MAJORCLASSIFY;
                wfentity.argument2 = bsentity.LLLEGALTYPE;
                wfentity.startflow = "������������";
                wfentity.submittype = "�ύ";
                wfentity.rankid = null;
                wfentity.user = curUser;
                wfentity.mark = "Υ����������";
                wfentity.organizeid = bsentity.BELONGDEPARTID; //��Ӧ�糧id

                //��ȡ��һ���̵Ĳ�����
                WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
                //���������¼
                LllegalExtensionEntity exentity = new LllegalExtensionEntity();
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
                        cEntity.REFORMDEADLINE = cEntity.REFORMDEADLINE.Value.AddDays(cEntity.POSTPONEDAYS.Value);

                        //��������ʱ��
                        LllegalAcceptEntity aEntity = lllegalacceptbll.GetEntityByBid(keyValue);
                        if (null != aEntity.ACCEPTTIME)
                        {
                            aEntity.ACCEPTTIME = aEntity.ACCEPTTIME.Value.AddDays(cEntity.POSTPONEDAYS.Value);
                        }
                        lllegalacceptbll.SaveForm(aEntity.ID, aEntity);

                        exentity.HANDLESIGN = "1"; //�ɹ����
                    }
                    cEntity.APPSIGN = "Web";
                    //����������Ϣ
                    lllegalreformbll.SaveForm(cEntity.ID, cEntity); //������������

                    exentity.LLLEGALID = keyValue;
                    exentity.HANDLEDATE = DateTime.Now;
                    exentity.POSTPONEDAYS = entity.POSTPONEDAYS.ToString();
                    exentity.HANDLEUSERID = curUser.UserId;
                    exentity.HANDLEUSERNAME = curUser.UserName;
                    exentity.HANDLEDEPTCODE = curUser.DeptCode;
                    exentity.HANDLEDEPTNAME = curUser.DeptName;
                    exentity.HANDLETYPE = "0";  //��������  ״̬���� 2 ʱ��ʾ����������� (����)
                    exentity.HANDLEID = DateTime.Now.ToString("yyyyMMddhhmmss").ToString();
                    exentity.POSTPONEREASON = postponereason;
                    exentity.APPSIGN = "Web";
                    lllegalextensionbll.SaveForm("", exentity);

                    //��������
                    if (wfFlag != "2") 
                    {
                          htworkflowbll.PushMessageForWorkFlow("Υ�¹�������", "������������", wfentity, result); 
                    }
      

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
        #endregion

        #region �ύ��������
        /// <summary>
        /// �����ύ
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalReformEntity rEntity, LllegalAcceptEntity acceptEntity)
        {
            Operator curUser = OperatorProvider.Provider.Current();

            string participant = string.Empty;

            string wfFlag = string.Empty;

            WfControlResult result  = new WfControlResult();
            try
            {
                //�����˻ز�����Ϣ
                LllegalRegisterEntity baseentity = lllegalregisterbll.GetEntity(keyValue);
                baseentity.RESEVERFOUR = entity.RESEVERFOUR;
                baseentity.RESEVERFIVE = entity.RESEVERFIVE;
                lllegalregisterbll.SaveForm(keyValue, baseentity);

                //������Ϣ
                LllegalReformEntity rfEntity = lllegalreformbll.GetEntityByBid(keyValue);
                rfEntity.REFORMFINISHDATE = rEntity.REFORMFINISHDATE;
                rfEntity.REFORMMEASURE = rEntity.REFORMMEASURE;
                rfEntity.REFORMSTATUS = rEntity.REFORMSTATUS;
                rfEntity.MODIFYDATE = DateTime.Now;
                rfEntity.MODIFYUSERID = curUser.UserId;
                rfEntity.MODIFYUSERNAME = curUser.UserName;
                rfEntity.REFORMPIC = rEntity.REFORMPIC;
                rfEntity.REFORMATTACHMENT = rEntity.REFORMATTACHMENT;
                lllegalreformbll.SaveForm(rfEntity.ID, rfEntity);


                WfControlObj wfentity = new WfControlObj();
                wfentity.businessid = keyValue;
                wfentity.argument1 = baseentity.MAJORCLASSIFY; //רҵ����
                wfentity.startflow = baseentity.FLOWSTATE;
                wfentity.rankid =null;
                wfentity.user = curUser;
                wfentity.organizeid = baseentity.BELONGDEPARTID; //��Ӧ�糧id
                if (entity.ADDTYPE == "2")
                {
                    wfentity.mark = "ʡ��Υ������";
                }
                else
                {
                    wfentity.mark = "����Υ������";
                }
                //�˻� 
                if (entity.RESEVERFOUR == "��")
                {
                    //��ʷ��¼
                    var reformitem = lllegalreformbll.GetHistoryList(entity.ID).ToList();
                    //���δ���Ŀ����˻�
                    if (reformitem.Count() == 0)
                    {
                        wfentity.submittype = "�˻�";
                    }
                    else
                    {
                        return Error("���Ĺ����Υ���޷��ٴ��˻�!");
                    }
                }
                else //�����ύ����������
                {
                    wfentity.submittype = "�ύ";
                }


                //��ȡ��һ���̵Ĳ�����
                result = wfcontrolbll.GetWfControl(wfentity);
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
                        //�ύ���̵���һ�ڵ�
                        if (!string.IsNullOrEmpty(participant))
                        {
                            int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                            if (count > 0)
                            {
                                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬
                            }
                        }
                        else
                        {
                            return Error("����ϵϵͳ����Ա��ȷ���ύ����!");
                        }
                    }
                    else 
                    {
                        //��ȡ
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                        #region ��ǰ������Υ�����Ľ׶�
                        if (tagName == "Υ������" )
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            rfEntity.REFORMPEOPLE = userentity.RealName;
                            rfEntity.REFORMPEOPLEID = userentity.UserId;
                            rfEntity.REFORMDEPTNAME = userentity.DeptName;
                            rfEntity.REFORMDEPTCODE = userentity.DepartmentCode;
                            rfEntity.REFORMTEL = userentity.Telephone;
                            lllegalreformbll.SaveForm(rfEntity.ID, rfEntity);
                        }
                        #endregion
                    }
                }
                //���Զ����������
                else if (result.code == WfCode.NoAutoHandle)
                {
                    bool isupdate = false;//�Ƿ��������״̬
                    //�˻ز���  ��������
                    if (entity.RESEVERFOUR == "��")
                    {
                        DataTable dt = htworkflowbll.GetBackFlowObjectByKey(keyValue);

                        if (dt.Rows.Count > 0)
                        {
                            wfFlag = dt.Rows[0]["wfflag"].ToString(); //��������

                            participant = dt.Rows[0]["participant"].ToString();  //ָ����

                            isupdate = dt.Rows[0]["isupdate"].ToString()=="1"; //�Ƿ��������״̬
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
                                htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬
                            }
                            result.message = "����ɹ�";
                            result.code = WfCode.Sucess;
                        }
                    }
                    else 
                    {
                        //����������״̬��
                        htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);

                        string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);
                        #region ��ǰ������Υ�����Ľ׶�
                        if (tagName == "Υ������")
                        {
                            UserInfoEntity userentity = userbll.GetUserInfoByAccount(participant);
                            rfEntity.REFORMPEOPLE = userentity.RealName;
                            rfEntity.REFORMPEOPLEID = userentity.UserId;
                            rfEntity.REFORMDEPTNAME = userentity.DeptName;
                            rfEntity.REFORMDEPTCODE = userentity.DepartmentCode;
                            rfEntity.REFORMTEL = userentity.Telephone;
                            lllegalreformbll.SaveForm(rfEntity.ID, rfEntity);
                        }
                        #endregion

                        result.message = "����ɹ�";
                        result.code = WfCode.Sucess;
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

        

        #region ��ȡ������ʷ��¼
        /// <summary>
        /// ��ȡ������ʷ��¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(string keyValue)
        {
            var data = lllegalreformbll.GetHistoryList(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region ��ȡ���������¼
        /// <summary>
        /// ��ȡ���������¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEntityJson(string keyValue)
        {
            var data = lllegalreformbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }
        #endregion
    }
    #endregion
}
