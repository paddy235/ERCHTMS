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
        private LllegalReformBLL lllegalreformbll = new LllegalReformBLL(); //������Ϣ����
        private LllegalApproveBLL lllegalapprovebll = new LllegalApproveBLL(); //��׼��Ϣ����
        private LllegalAcceptBLL lllegalacceptbll = new LllegalAcceptBLL(); //������Ϣ����
        private LllegalPunishBLL lllegalpunishbll = new LllegalPunishBLL(); //������Ϣ����

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
        public ActionResult SubmitForm(string keyValue, LllegalRegisterEntity entity, LllegalApproveEntity pEntity, LllegalPunishEntity pbEntity, LllegalReformEntity rEntity, LllegalAcceptEntity aEntity)
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

            bool isSubmit = true; //�Ƿ�Ҫִ���ύ����,��ȫ���������ڿ���װ����Υ��ת����װ�ò���

            bool isAddScore = false; //�Ƿ���ӵ��û�����

            var lllegatypename = dataitemdetailbll.GetEntity(entity.LLLEGALTYPE).ItemName;

            //��ͨ��,�˻ص�Υ�µǼǣ�������I����׼����II����׼
            if (pEntity.APPROVERESULT == "0")
            {
                wfFlag = "2";
                string createuserid = lllegalregisterbll.GetEntity(keyValue).CREATEUSERID;
                UserEntity userEntity = userbll.GetEntity(createuserid);
                participant = userEntity.Account;  //�Ǽ��û�
                errorMsg = "�Ǽ��û�";
            }
            else  //��׼ͨ��
            {
                // ��ȫ��������Ա
                if (userbll.GetSafetyAndDeviceDept(curUser).Contains("1"))
                {
                    //��ǰ��������װ�ò��ţ�ֱ�ӵ�����
                    if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                    {
                        //ȡ������
                        wfFlag = "1";  // II����׼=>����
                        //�����װ���� ���ύ������
                        UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //�����û�����
                        //ȡ������
                        participant = reformUser.Account;

                        errorMsg = "����������";

                        isAddScore = true;
                    }
                    else 
                    {
                        //�ж��Ƿ�װ����Υ��
                        if (lllegatypename == dataitemdetailbll.GetItemValue("ApplianceClass"))
                        {
                            //���ĺ�׼���˺ţ����Ϊװ�ò����û�  �˲�����Ҫ����״̬
                            isSubmit = false;
                            //ȡװ�ò����û�
                            participant = userbll.GetSafetyDeviceDeptUser("1", curUser);
                            errorMsg = "װ�ò����û�";
                        }
                        else
                        {
                            //����Ƿ�װ����Υ�£�ͨ�����������
                            //ȡ������
                            wfFlag = "1";  // II����׼=>����
                            //�����װ���� ���ύ������
                            UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //�����û�����
                            //ȡ������
                            participant = reformUser.Account;

                            errorMsg = "����������";

                            isAddScore = true;
                        }
                    }
      
                }
                //װ���û�
                else if (userbll.GetSafetyAndDeviceDept(curUser).Contains("2"))
                {
                       wfFlag = "1";  // II����׼=>����
                       //�����װ���� ���ύ������
                       UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //�����û�����
                       //ȡ������
                       participant = reformUser.Account;

                       errorMsg = "����������";

                       isAddScore = true;
                }
                else  //����������Ա 
                {

                   //�ϱ������
                    if (entity.ISUPSAFETY == "1")
                    {
                        wfFlag = "3";
                        //ȡ��ȫ������ ��������II����׼
                        //ȡ��ȫ�������û�
                        participant = userbll.GetSafetyDeviceDeptUser("0", curUser);
                        errorMsg = "��ȫ�������û�";
                    }
                    else //���ϱ�����£��ύ������ 
                    {
                        wfFlag = "1";  //I����׼=>����
                        //�����װ���� ���ύ������
                        UserEntity reformUser = userbll.GetEntity(rEntity.REFORMPEOPLEID); //�����û�����
                        //ȡ������
                        participant = reformUser.Account;

                        errorMsg = "����������";

                        isAddScore = true;

                    }
                }
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

            //�����׼������Ϣ (��ִ�����������ϵ��ύ,���޷����к�׼)
            if (isSubmit) 
            {
                pEntity.LLLEGALID = keyValue;
                lllegalapprovebll.SaveForm("", pEntity);

                //��������������Ϣ(�ر���Ժ�׼����)
                pbEntity.MARK = "1"; //��ʾ���˼�¼�µ�
                pbEntity.LLLEGALID = keyValue;
                pbEntity.APPROVEID = pEntity.ID;
                pbEntity.CREATEDATE = DateTime.Now;
                pbEntity.CREATEUSERDEPTCODE = curUser.DeptCode;
                pbEntity.CREATEUSERID = curUser.UserId;
                pbEntity.CREATEUSERNAME = curUser.UserName;
                pbEntity.CREATEUSERORGCODE = curUser.OrganizeCode;
                pbEntity.MODIFYDATE = DateTime.Now;
                pbEntity.MODIFYUSERID = curUser.UserId;
                pbEntity.MODIFYUSERNAME = curUser.UserName;
                lllegalpunishbll.SaveForm("", pbEntity);
            }

            //ͬʱ�Ի����Ŀ������ݽ��и�����Ӧ�ĳͷ�ֵ
            LllegalPunishEntity punishEntity = lllegalpunishbll.GetEntityByBid(keyValue);
            if (null != punishEntity)
            {
                //punishEntity.APPROVEID = pEntity.ID;
                punishEntity.PERSONINCHARGEID = pbEntity.PERSONINCHARGEID;
                punishEntity.PERSONINCHARGENAME = pbEntity.PERSONINCHARGENAME;
                punishEntity.ECONOMICSPUNISH = pbEntity.ECONOMICSPUNISH;
                punishEntity.LLLEGALPOINT = pbEntity.LLLEGALPOINT;
                punishEntity.AWAITJOB = pbEntity.AWAITJOB;
                punishEntity.LLLEGAOTHER = pbEntity.LLLEGAOTHER;
                punishEntity.FIRSTINCHARGEID = pbEntity.FIRSTINCHARGEID;
                punishEntity.FIRSTINCHARGENAME = pbEntity.FIRSTINCHARGENAME;
                punishEntity.FIRSTECONOMICSPUNISH = pbEntity.FIRSTECONOMICSPUNISH;
                punishEntity.FIRSTLLLEGALPOINT = pbEntity.FIRSTLLLEGALPOINT;
                punishEntity.FIRSTAWAITJOB = pbEntity.FIRSTAWAITJOB;
                punishEntity.FIRSTOTHER = pbEntity.FIRSTOTHER;
                punishEntity.SECONDINCHARGEID = pbEntity.SECONDINCHARGEID;
                punishEntity.SECONDINCHARGENAME = pbEntity.SECONDINCHARGENAME;
                punishEntity.SECONDECONOMICSPUNISH = pbEntity.SECONDECONOMICSPUNISH;
                punishEntity.SECONDLLLEGALPOINT = pbEntity.SECONDLLLEGALPOINT;
                punishEntity.SECONDAWAITJOB = pbEntity.SECONDAWAITJOB;
                punishEntity.SECONDOTHER = pbEntity.SECONDOTHER;
                lllegalpunishbll.SaveForm(punishEntity.ID, punishEntity);
            }


            //  string ReformID = Request.Form["REFORMID"] != null ? Request.Form["REFORMID"].ToString() : ""; //����ID
            //Υ�����ļ�¼
            LllegalReformEntity cEntity = lllegalreformbll.GetEntityByBid(keyValue);
            cEntity.REFORMDEADLINE = rEntity.REFORMDEADLINE;
            cEntity.REFORMPEOPLE = rEntity.REFORMPEOPLE;
            cEntity.REFORMPEOPLEID = rEntity.REFORMPEOPLEID;
            cEntity.REFORMDEPTCODE = rEntity.REFORMDEPTCODE;
            cEntity.REFORMDEPTNAME = rEntity.REFORMDEPTNAME;
            cEntity.REFORMTEL = rEntity.REFORMTEL;
            lllegalreformbll.SaveForm(cEntity.ID, cEntity);


            // string AcceptID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : ""; // ����ID 
            //��������
            LllegalAcceptEntity aptEntity = lllegalacceptbll.GetEntityByBid(keyValue);
            aptEntity.ACCEPTPEOPLE = aEntity.ACCEPTPEOPLE;
            aptEntity.ACCEPTPEOPLEID = aEntity.ACCEPTPEOPLEID;
            aptEntity.ACCEPTDEPTCODE = aEntity.ACCEPTDEPTCODE;
            aptEntity.ACCEPTDEPTNAME = aEntity.ACCEPTDEPTNAME;
            aptEntity.ACCEPTTIME = aEntity.ACCEPTTIME;
            lllegalacceptbll.SaveForm(aptEntity.ID, aptEntity);


            //����û����ֹ���
            if (isAddScore)
            {
                lllegalpunishbll.SaveUserScore(pbEntity.PERSONINCHARGEID, entity.LLLEGALLEVEL);
                lllegalpunishbll.SaveUserScore(pbEntity.FIRSTINCHARGEID, entity.LLLEGALLEVEL);
                lllegalpunishbll.SaveUserScore(pbEntity.SECONDINCHARGEID, entity.LLLEGALLEVEL);
            }

            //ȷ��Ҫ�ύ
            if (isSubmit)
            {
                //�ύ���̵���һ�ڵ�
                if (!string.IsNullOrEmpty(participant))
                {
                    int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                    if (count > 0)
                    {
                        htworkflowbll.UpdateFlowStateByObjectId("bis_lllegalregister", "flowstate", keyValue);  //����ҵ������״̬
                    }
                }
                else
                {
                    return Error("����ϵϵͳ����Ա��ȷ��" + errorMsg + "!");
                }
            }
            else  //��ȫ�����Ŷ�װ����Υ�½���ת����ת����װ�ò��ŵ�λ�£������������״̬
            {
                bool isSuccess = htworkflowbll.SubmitWorkFlowNoChangeStatus(keyValue, participant, curUser.UserId);

                if (isSuccess)
                {
                    return Success("�����ɹ�!");
                }
                else
                {
                    return Error("����ϵϵͳ����Ա��ȷ��" + errorMsg + "!");
                }
            }
            return Success("�����ɹ�!");
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
    }
}
