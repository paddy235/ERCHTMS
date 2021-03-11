using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.BaseManage;
using System;
using ERCHTMS.Busines.SystemManage;
using System.Collections.Generic;

namespace ERCHTMS.Web.Areas.HiddenTroubleManage.Controllers
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public class HTAcceptInfoController : MvcControllerBase
    {
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL();
        private HTAcceptInfoBLL htacceptinfobll = new HTAcceptInfoBLL();
        private HTApprovalBLL htapprovalbll = new HTApprovalBLL();
        private HtReCheckBLL htrecheckbll = new HtReCheckBLL();
        private UserBLL userbll = new UserBLL();
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL();
        private HTEstimateBLL htestimatebll = new HTEstimateBLL();
        private HTChangeInfoBLL htchangeinfobll = new HTChangeInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL(); //���Ų�������
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
        public ActionResult GetListJson(string hideCode)
        {
            var data = htacceptinfobll.GetList(hideCode);
            return ToJsonResult(data);
        }


        [HttpGet]
        public ActionResult GetHistoryListJson(string keyCode)
        {
            var data = htacceptinfobll.GetHistoryList(keyCode);
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
            var data = htacceptinfobll.GetEntity(keyValue);
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
            htacceptinfobll.RemoveForm(keyValue);
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
        public ActionResult SaveForm(string keyValue, HTBaseInfoEntity bentity, HTAcceptInfoEntity entity, HTChangeInfoEntity centity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : ""; //����ID
            string ACCEPTSTATUS = Request.Form["ACCEPTSTATUS"] != null ? Request.Form["ACCEPTSTATUS"].ToString() : ""; //�������
            string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : ""; //����ID
            string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա
            string wfFlag = string.Empty; //���̱�ʶ

            string IIMajorRisks = dataitemdetailbll.GetItemValue("IIMajorRisks"); //II���ش�����

            string IMajorRisks = dataitemdetailbll.GetItemValue("IMajorRisks"); //I���ش�����


            var baseEntity = htbaseinfobll.GetEntity(keyValue);
            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue;
            wfentity.argument1 = bentity.MAJORCLASSIFY; //רҵ����
            wfentity.argument3 = bentity.HIDTYPE; //�������
            wfentity.argument4 = bentity.HIDBMID; //��������
            wfentity.startflow = baseEntity.WORKSTREAM;
            wfentity.rankid = baseEntity.HIDRANK;
            wfentity.user = curUser;
            wfentity.organizeid = baseEntity.HIDDEPART; //��Ӧ�糧id
            //ʡ���Ǽǵ�
            if (baseEntity.ADDTYPE == "2")
            {
                wfentity.mark = "ʡ�������Ų�";
            }
            else //����
            {
                wfentity.mark = "���������Ų�";
            }
            //����ͨ��
            if (ACCEPTSTATUS == "1")
            {
                wfentity.submittype = "�ύ";
            }
            else //���ղ�ͨ��
            {
                wfentity.submittype = "�˻�";
            }

            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
            
            //���ز�������ɹ�
            if (result.code == WfCode.Sucess)
            {
                participant = result.actionperson;

                wfFlag = result.wfflag;

                //����Ǹ���״̬
                if (result.ischangestatus)
                {
                    if (!string.IsNullOrEmpty(participant))
                    {
                        //�ύ����
                        int count = htworkflowbll.SubmitWorkFlow(wfentity, result, keyValue, participant, wfFlag, curUser.UserId);

                        if (count > 0)
                        {
                            #region ����������Ŀ
                            if (!string.IsNullOrEmpty(ACCEPTID))
                            {
                                var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                                entity.AUTOID = tempEntity.AUTOID;
                            }
                            if (null == entity.ACCEPTDATE)
                            {
                                entity.ACCEPTDATE = DateTime.Now;
                            }
                            htacceptinfobll.SaveForm(ACCEPTID, entity);

                            htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬

                            //�˻�������������ռ�¼
                            if (wfentity.submittype == "�˻�")
                            {
                                string tagName = htworkflowbll.QueryTagNameByCurrentWF(keyValue);

                                if (tagName == "��������")
                                {
                                    //���ļ�¼
                                    HTChangeInfoEntity chEntity = htchangeinfobll.GetEntity(CHANGEID);
                                    HTChangeInfoEntity newEntity = new HTChangeInfoEntity();
                                    newEntity = chEntity;
                                    newEntity.CREATEDATE = DateTime.Now;
                                    newEntity.MODIFYDATE = DateTime.Now;
                                    newEntity.ID = null;
                                    newEntity.AUTOID = chEntity.AUTOID + 1;
                                    newEntity.CHANGERESUME = null;
                                    newEntity.CHANGEFINISHDATE = null;
                                    newEntity.REALITYMANAGECAPITAL = 0;
                                    newEntity.ATTACHMENT = Guid.NewGuid().ToString(); //���ĸ���
                                    newEntity.HIDCHANGEPHOTO = Guid.NewGuid().ToString(); //����ͼƬ
                                    newEntity.APPSIGN = "Web";
                                    htchangeinfobll.SaveForm("", newEntity);
                                }
                        
                                //���ռ�¼
                                HTAcceptInfoEntity htacceptinfoentity = htacceptinfobll.GetEntityByHidCode(bentity.HIDCODE);
                                HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                                accptEntity = htacceptinfoentity;
                                accptEntity.ID = null;
                                accptEntity.AUTOID = htacceptinfoentity.AUTOID + 1;
                                accptEntity.CREATEDATE = DateTime.Now;
                                accptEntity.MODIFYDATE = DateTime.Now;
                                accptEntity.ACCEPTSTATUS = null;
                                accptEntity.ACCEPTIDEA = null;
                                accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString(); //����ͼƬ
                                accptEntity.APPSIGN = "Web";
                                htacceptinfobll.SaveForm("", accptEntity);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        return Error("����ϵϵͳ����Ա��ȷ���Ƿ������������������!");
                    }
                }
                else
                {
                    //������Ϣ
                    if (!string.IsNullOrEmpty(ACCEPTID))
                    {
                        var tempEntity = htacceptinfobll.GetEntity(ACCEPTID);
                        entity.AUTOID = tempEntity.AUTOID;
                    }
                    if (null == entity.ACCEPTDATE)
                    {
                        entity.ACCEPTDATE = DateTime.Now;
                    }
                    htacceptinfobll.SaveForm(ACCEPTID, entity);

                    //�����һ�����ն���
                    HTAcceptInfoEntity accptEntity = new HTAcceptInfoEntity();
                    accptEntity = entity;
                    accptEntity.ID = string.Empty;
                    accptEntity.AUTOID = entity.AUTOID + 1;
                    accptEntity.CREATEDATE = DateTime.Now;
                    accptEntity.MODIFYDATE = DateTime.Now;
                    accptEntity.ACCEPTSTATUS = string.Empty;
                    accptEntity.ACCEPTIDEA = string.Empty;
                    accptEntity.ACCEPTDATE = null;
                    accptEntity.DAMAGEDATE = null;
                    accptEntity.ACCEPTPHOTO = Guid.NewGuid().ToString(); //����ͼƬ
                    accptEntity.APPSIGN = "Web";
                    htacceptinfobll.SaveForm("", accptEntity);
                    //��ȡ
                    htworkflowbll.SubmitWorkFlowNoChangeStatus(wfentity, result, keyValue, participant, curUser.UserId);
                }
                return Success(result.message);
            }
            else
            {
                return Error(result.message);
            }
        }
        #endregion

    }
}
