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
using ERCHTMS.Busines.SystemManage;

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers
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
        public ActionResult SaveSettingForm(string keyValue, HTChangeInfoEntity entity)
        {
            Operator curUser = new OperatorProvider().Current();
            var cEntity = htchangeinfobll.GetEntityByCode(keyValue); //����HidCode��ȡ���Ķ���
            string  postponereason = Request.Form["POSTPONEREASON"] != null ? Request.Form["POSTPONEREASON"].ToString() : "";
            string hidid = Request.Form["HIDID"] != null ? Request.Form["HIDID"].ToString() : "";
            cEntity.POSTPONEDAYS = entity.POSTPONEDAYS;
            if (!string.IsNullOrEmpty(entity.POSTPONEDEPT))
            {
                entity.POSTPONEDEPT = "," + entity.POSTPONEDEPT + ",";
            }
            cEntity.POSTPONEDEPT = entity.POSTPONEDEPT;  //��������Code
            cEntity.POSTPONEDEPTNAME = entity.POSTPONEDEPTNAME;  //������������
            cEntity.APPLICATIONSTATUS = entity.APPLICATIONSTATUS;
            htchangeinfobll.SaveForm(cEntity.ID, cEntity); //������������

            //���������¼
            HTExtensionEntity exentity = new HTExtensionEntity();
            exentity.HIDCODE = cEntity.HIDCODE;
            exentity.HIDID = hidid;
            exentity.HANDLEDATE = DateTime.Now;
            exentity.POSTPONEDAYS = entity.POSTPONEDAYS.ToString();
            exentity.HANDLEUSERID = curUser.UserId;
            exentity.HANDLEUSERNAME = curUser.UserName;
            exentity.HANDLEDEPTCODE = curUser.DeptCode;
            exentity.HANDLEDEPTNAME = curUser.DeptName;
            exentity.HANDLETYPE = "0";  //��������
            exentity.HANDLEID = DateTime.Now.ToString("yyyyMMddhhmmss").ToString();
            exentity.POSTPONEREASON = postponereason;
            htextensionbll.SaveForm("", exentity);

            return Success("�����ɹ���");
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

            if(!string.IsNullOrEmpty(CHANGEID))
            {
                var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                entity.AUTOID = tempEntity.AUTOID;
                entity.APPLICATIONSTATUS = tempEntity.APPLICATIONSTATUS;
                entity.POSTPONEDAYS = tempEntity.POSTPONEDAYS;
                entity.POSTPONEDEPT = tempEntity.POSTPONEDEPT;
                entity.POSTPONEDEPTNAME = tempEntity.POSTPONEDEPTNAME;
            }

            htchangeinfobll.SaveForm(CHANGEID, entity);

            //�˻�
            if (ISBACK == "1")
            {

                IList<UserEntity> ulist = new List<UserEntity>();

                string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

                string[] pstr = HidApproval.Split('#');  //�ָ�������


                var createuserid = new HTBaseInfoBLL().GetEntity(keyValue).CREATEUSERID;

                var createUser = userbll.GetEntity(createuserid);

                foreach (string strArgs in pstr)
                {
                    string[] str = strArgs.Split('|');

                    //��ǰ������ͬ����Ϊ�����Ű�ȫ����Ա��֤
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "0")
                    {
                        //�Ǽ���Ϊ
                        DataTable rDt = userbll.HaveRoleListByKey(createuserid, dataitemdetailbll.GetItemValue("HidApprovalSetting"));
                        //��ȫ����Ա�Ǽǵ�
                        if (rDt.Rows.Count > 0)
                        {
                            //����Ǽ����ǰ�ȫ����Ա 3
                            participant = rDt.Rows[0]["account"].ToString();

                            wfFlag = "3";
                        }
                        else
                        {
                            //�Ǽ��˷ǰ�ȫ����Ա 2 
                            DataTable dt = htapprovalbll.GetDataTableByHidCode(entity.HIDCODE);
                            if (dt.Rows.Count > 0)
                            {
                                //��ȡ�˲��� 
                                participant = dt.Rows[0]["account"].ToString(); //������ Or �Ǽ���

                                wfFlag = "2";
                            }
                        }

                        break;
                    }
                    //ָ���������
                    if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                    {
                        //��ȡָ�����ŵ�������Ա   //ȡ�Ǽ��� ��ǰ����ָ������,���˻صǼ�,��֮�򵽺˲�

                        string curDeptCode = "'" + createUser.DepartmentCode + "'";

                        //��ǰ���Ƿ������ָ�����ŵ���
                        if (str[2].ToString().Contains(curDeptCode))
                        {
                            //ȡ�Ǽ�
                            participant = userbll.GetEntity(bEntity.CREATEUSERID).Account;

                            wfFlag = "3";
                        }
                        else 
                        {
                            DataTable dt = htapprovalbll.GetDataTableByHidCode(entity.HIDCODE);
                            if (dt.Rows.Count > 0) 
                            {
                                //��ȡ�˲��� 
                                participant = dt.Rows[0]["account"].ToString();

                                wfFlag = "2";
                            }
                        }
                        break;
                    }
                }

            }
            else //�����ύ����������
            {
                //������Ա
                UserEntity auser = userbll.GetEntity(aEntity.ACCEPTPERSON);

                participant = auser.Account;

                wfFlag = "1";
            }

            if (!string.IsNullOrEmpty(participant) && !string.IsNullOrEmpty(wfFlag))
            {
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬
                }
            }
            else 
            {
                return Error("����ʧ��!");
            }
            return Success("�����ɹ�!");
        }
        #endregion
    }
}
