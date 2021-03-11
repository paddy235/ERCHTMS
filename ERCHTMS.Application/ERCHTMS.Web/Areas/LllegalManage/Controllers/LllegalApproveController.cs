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
using System;
using System.Collections.Generic;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Busines.LllegalManage;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Dynamic;
using System.Data;
using System.Linq;
using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.Attributes;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// �� ����Υ��������Ϣ��
    /// </summary>
    public class LllegalApproveController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //����ҵ�����
        private DepartmentBLL departmentbll = new DepartmentBLL(); //���Ų�������
        private UserBLL userbll = new UserBLL(); //��Ա��������
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL(); // Υ�»�����Ϣ
        private LllegalAwardDetailBLL lllegalawarddetailbll = new LllegalAwardDetailBLL(); //Υ�½�����Ϣ
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //������Ϣ����
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //��׼��Ϣ����
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //������Ϣ����
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); //������Ϣ����

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
            string km_major_role = dataitemdetailbll.GetItemValue("KM_MAJOR_ROLE"); //�������� 
            //����
            if (!string.IsNullOrEmpty(km_major_role))
            {
                string actionName = string.Empty;
                string[] allKeys = Request.QueryString.AllKeys;
                if (allKeys.Count() > 0)
                {
                    actionName = "KmForm?";
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
                    actionName = "KmForm";
                }
                return Redirect(actionName);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult KmForm()
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
        #endregion

        #region �����ύ��׼����
        /// <summary>
        /// �����ύ
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalApproveEntity pEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
        {

            /*
             ע����׼������ 
             * 
             *  ȷ��Ϊװ��������£������ǰ��׼���ǰ�ȫ��������Ա, ���ж��Ƿ�Ϊװ���࣬������� ��ֱ�ӵ����Ļ��˻أ�����ǣ���ת����װ�ò��� ���˻�(�����ύ)
             *  ����Ƿǰ�ȫ������Ա,���ύ����ȫ��������Ա���˻ص��Ǽ�.
             */

            string errorMsg = string.Empty;

            Operator curUser = OperatorProvider.Provider.Current();

            string wfFlag = string.Empty;  //���̱�ʶ

            string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա

            var lllegatypename = "";
            if (!string.IsNullOrWhiteSpace(entity.LLLEGALTYPE))
            {
                lllegatypename = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE).ItemName;
            }

            //����Υ�»�����Ϣ
            LllegalRegisterEntity baseentity = lllegalregisterbll.GetEntity(keyValue);
            entity.AUTOID = baseentity.AUTOID;
            entity.CREATEDATE = baseentity.CREATEDATE;
            entity.CREATEUSERDEPTCODE = baseentity.CREATEUSERDEPTCODE;
            entity.CREATEUSERID = baseentity.CREATEUSERID;
            entity.CREATEUSERNAME = baseentity.CREATEUSERNAME;
            entity.CREATEUSERORGCODE = baseentity.CREATEUSERORGCODE;
            entity.MODIFYDATE = DateTime.Now;
            entity.MODIFYUSERID = curUser.UserId;
            entity.MODIFYUSERNAME = curUser.UserName;
            entity.RESEVERFOUR = "";
            entity.RESEVERFIVE = "";
            lllegalregisterbll.SaveForm(keyValue, entity);

            #region ������Ϣ
           
            string RELEVANCEDATA = Request.Form["RELEVANCEDATA"];
            if (!string.IsNullOrEmpty(RELEVANCEDATA))
            {
                //��ɾ��������Ϣ����
                lllegalpunishbll.DeleteLllegalPunishList(entity.ID, "");

                JArray jarray = (JArray)JsonConvert.DeserializeObject(RELEVANCEDATA);

                foreach (JObject rhInfo in jarray)
                {
                    //string relevanceId = rhInfo["ID"].ToString(); //����id
                    string assessobject = rhInfo["ASSESSOBJECT"].ToString();
                    string personinchargename = rhInfo["PERSONINCHARGENAME"].ToString(); //��������������
                    string personinchargeid = rhInfo["PERSONINCHARGEID"].ToString();//����������id
                    string performancepoint = rhInfo["PERFORMANCEPOINT"].ToString();//EHS��Ч���� 
                    string economicspunish = rhInfo["ECONOMICSPUNISH"].ToString(); // ���ô���
                    string education = rhInfo["EDUCATION"].ToString(); //������ѵ
                    string lllegalpoint = rhInfo["LLLEGALPOINT"].ToString();//Υ�¿۷�
                    string awaitjob = rhInfo["AWAITJOB"].ToString();//����
                    LllegalPunishEntity newpunishEntity = new LllegalPunishEntity();
                    newpunishEntity.LLLEGALID = entity.ID;
                    newpunishEntity.ASSESSOBJECT = assessobject; //���˶���
                    newpunishEntity.PERSONINCHARGEID = personinchargeid;
                    newpunishEntity.PERSONINCHARGENAME = personinchargename;
                    newpunishEntity.PERFORMANCEPOINT = !string.IsNullOrEmpty(performancepoint) ? Convert.ToDecimal(performancepoint) : 0;
                    newpunishEntity.ECONOMICSPUNISH = !string.IsNullOrEmpty(economicspunish) ? Convert.ToDecimal(economicspunish) : 0;
                    newpunishEntity.EDUCATION = !string.IsNullOrEmpty(education) ? Convert.ToDecimal(education) : 0;
                    newpunishEntity.LLLEGALPOINT = !string.IsNullOrEmpty(lllegalpoint) ? Convert.ToDecimal(lllegalpoint) : 0;
                    newpunishEntity.AWAITJOB = !string.IsNullOrEmpty(awaitjob) ? Convert.ToDecimal(awaitjob) : 0;
                    newpunishEntity.MARK = assessobject.Contains("����") ? "0" : "1"; //���0���ˣ�1����
                    lllegalpunishbll.SaveForm("", newpunishEntity);
                }
            }
            #endregion

            #region Υ�½�����Ϣ
            string AWARDDATA = Request.Form["AWARDDATA"];
            if (!string.IsNullOrEmpty(AWARDDATA))
            {  //��ɾ����������
                lllegalawarddetailbll.DeleteLllegalAwardList(entity.ID);

                JArray jarray = (JArray)JsonConvert.DeserializeObject(AWARDDATA);

                foreach (JObject rhInfo in jarray)
                {
                    string userid = rhInfo["USERID"].ToString(); //�����û�
                    string username = rhInfo["USERNAME"].ToString();
                    string deptid = rhInfo["DEPTID"].ToString();//�����û�����
                    string deptname = rhInfo["DEPTNAME"].ToString();
                    string points = rhInfo["POINTS"].ToString();  //��������
                    string money = rhInfo["MONEY"].ToString(); //�������

                    LllegalAwardDetailEntity awardEntity = new LllegalAwardDetailEntity();
                    awardEntity.LLLEGALID = entity.ID;
                    awardEntity.USERID = userid; //��������
                    awardEntity.USERNAME = username;
                    awardEntity.DEPTID = deptid;
                    awardEntity.DEPTNAME = deptname;
                    awardEntity.POINTS = !string.IsNullOrEmpty(points) ? int.Parse(points) : 0;
                    awardEntity.MONEY = !string.IsNullOrEmpty(money) ? Convert.ToDecimal(money) : 0;
                    lllegalawarddetailbll.SaveForm("", awardEntity);
                }
            }
            #endregion

            //Υ�����ļ�¼
            LllegalReformEntity cEntity = lllegalreformbll.GetEntityByBid(keyValue);
            cEntity.REFORMDEADLINE = rEntity.REFORMDEADLINE;
            cEntity.REFORMPEOPLE = rEntity.REFORMPEOPLE;
            cEntity.REFORMPEOPLEID = rEntity.REFORMPEOPLEID;
            cEntity.REFORMDEPTCODE = rEntity.REFORMDEPTCODE;
            cEntity.REFORMDEPTNAME = rEntity.REFORMDEPTNAME;
            cEntity.REFORMTEL = rEntity.REFORMTEL;
            cEntity.REFORMSTATUS = string.Empty;
            cEntity.REFORMCHARGEDEPTID = rEntity.REFORMCHARGEDEPTID;
            cEntity.REFORMCHARGEDEPTNAME = rEntity.REFORMCHARGEDEPTNAME;
            cEntity.REFORMCHARGEPERSON = rEntity.REFORMCHARGEPERSON;
            cEntity.REFORMCHARGEPERSONNAME = rEntity.REFORMCHARGEPERSONNAME;
            cEntity.ISAPPOINT = rEntity.ISAPPOINT;
            lllegalreformbll.SaveForm(cEntity.ID, cEntity);

            //Υ������
            LllegalAcceptEntity aptEntity = lllegalacceptbll.GetEntityByBid(keyValue);
            aptEntity.ACCEPTPEOPLE = aEntity.ACCEPTPEOPLE;
            aptEntity.ACCEPTPEOPLEID = aEntity.ACCEPTPEOPLEID;
            aptEntity.ACCEPTDEPTCODE = aEntity.ACCEPTDEPTCODE;
            aptEntity.ACCEPTDEPTNAME = aEntity.ACCEPTDEPTNAME;
            aptEntity.ACCEPTTIME = aEntity.ACCEPTTIME;
            lllegalacceptbll.SaveForm(aptEntity.ID, aptEntity);


            WfControlObj wfentity = new WfControlObj();
            wfentity.businessid = keyValue; //
            wfentity.argument1 = entity.MAJORCLASSIFY; //רҵ����
            wfentity.argument2 = entity.LLLEGALTYPE;//Υ������
            wfentity.argument3 = curUser.DeptId;//��ǰ����������
            wfentity.argument4 = entity.LLLEGALTEAMCODE;//Υ�²���
            wfentity.argument5 = entity.LLLEGALLEVEL; //Υ�¼���
            wfentity.startflow = baseentity.FLOWSTATE;
            //�ϱ����Ҵ����ϼ�����
            if (entity.ISUPSAFETY == "1")
            {
                wfentity.submittype = "�ϱ�";
            }
            else  //���ϱ�������ͨ����Ҫ�ύ���ģ�������ͨ���˻ص��Ǽ�
            {
                /****�жϵ�ǰ���Ƿ�����ͨ��*****/
                #region �жϵ�ǰ���Ƿ�����ͨ��
                //����ͨ������ֱ�ӽ�������
                if (pEntity.APPROVERESULT == "1")
                {
                    wfentity.submittype = "�ύ";
                    //��ָ������������
                    if (rEntity.ISAPPOINT == "0")
                    {
                        wfentity.submittype = "�ƶ��ύ";
                    }
                    //�ж��Ƿ���ͬ���ύ
                    bool ismajorpush = GetCurUserWfAuth(null, baseentity.FLOWSTATE, baseentity.FLOWSTATE, "����Υ������", "ͬ���ύ", entity.MAJORCLASSIFY, null, null, keyValue);
                    if (ismajorpush)
                    {
                        wfentity.submittype = "ͬ���ύ";
                    }
                }
                else  //������ͨ�����˻ص��Ǽ� 
                {
                    wfentity.submittype = "�˻�";
                }
                #endregion
            }
            wfentity.rankid = null;
            wfentity.user = curUser;
            wfentity.mark = "����Υ������";
            wfentity.organizeid = entity.BELONGDEPARTID; //��Ӧ�糧id
            //��ȡ��һ���̵Ĳ�����
            WfControlResult result = wfcontrolbll.GetWfControl(wfentity);
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
                            //����Υ�º�׼
                            pEntity.LLLEGALID = keyValue;
                            lllegalapprovebll.SaveForm("", pEntity);

                            htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬

                            return Success(result.message);
                        }
                        else
                        {
                            return Error("��ǰ�û��޺�׼Ȩ��!");
                        }
                    }
                    else  //������״̬�������
                    {
                        //����Υ�º�׼
                        pEntity.LLLEGALID = keyValue;
                        lllegalapprovebll.SaveForm("", pEntity);

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
       

        }
        #endregion

        #region ��ȡ��׼��ʷ��¼
        /// <summary>
        /// ��ȡ��׼��ʷ��¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetHistoryListJson(string keyValue)
        {
            var data = lllegalapprovebll.GetHistoryList(keyValue);
            return ToJsonResult(data);
        }
        #endregion

        #region ��ȡ��׼��ϸ��Ϣ
        /// <summary>
        /// ��ȡ��׼��ϸ��Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetEntityJson(string keyValue)
        {
            //��ȡ��Ӧ�ĺ�׼��Ϣ
            var approveData = lllegalapprovebll.GetEntity(keyValue);
            var punishData = lllegalpunishbll.GetEntityByApproveId(keyValue);
            var josnData = new
            {
                approveData = approveData, //��׼������Ϣ
                punishData = punishData  //����������Ϣ 
            };
            return Content(josnData.ToJson());
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
