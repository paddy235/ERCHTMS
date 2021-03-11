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

namespace ERCHTMS.Web.Areas.HiddenTroubleManageBz.Controllers
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


        /// <summary>
        /// �Ƿ�ָ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IsAssignDepartment()
        {
            Operator curUser = OperatorProvider.Provider.Current();

            bool isSuccessful = false;

            string HidApproval = dataitemdetailbll.GetItemValue("HidApproval");

            string[] pstr = HidApproval.Split('#');  //�ָ�������

            foreach (string strArgs in pstr)
            {
                string[] str = strArgs.Split('|');
                //ָ������
                if (str[0].ToString() == curUser.OrganizeId && str[1].ToString() == "1")
                {
                    //��ȡָ�����ŵ�������Ա
                    isSuccessful = true;

                    break;
                }
            }

            if (isSuccessful)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }

        /// <summary>
        /// �����ύ
        /// </summary>
        /// <param name="isUpSubmit"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, string submitUser, string isUpSubmit, HTApprovalEntity entity, HTChangeInfoEntity chEntity, HTAcceptInfoEntity aEntity)
        {

            Operator curUser = OperatorProvider.Provider.Current();

            string wfFlag = string.Empty;  //���̱�ʶ

            bool isParentDept = false;

            int upMark = 0;

            //���ż��û�
            if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidDepartment")).Rows.Count > 0)
            {
                upMark = 1;

                isParentDept = true; //�ϱ���������λ
            }
            //�а����û����ϱ�����������
            else if (userbll.HaveRoleListByKey(curUser.UserId, dataitemdetailbll.GetItemValue("HidContractor")).Rows.Count > 0)
            {
                upMark = 2;

                isParentDept = true;  //�ϼ�����Ϊ��������
            }
            else
            {
                isParentDept = departmentbll.IsExistSuperior(curUser.DeptId);  //�ж��Ƿ�����ϼ�����
            }
           
            string participant = string.Empty;  //��ȡ������һ�ڵ�Ĳ�����Ա

            //�ϱ����Ҳ������ϼ����ţ�������ʾ
            if (isUpSubmit == "1" && !isParentDept)
            {
                return Error("��ǰ�������ϼ����ţ��޷������ϱ�����!");
            }
            else if (isUpSubmit == "1" && isParentDept)  //�ϱ����Ҵ����ϼ�����
            {
                //�ϱ��ּ������  �ְ��̣��а��̣���˾����Ա����������Ա
                IList<UserEntity> ulist = new List<UserEntity>(); 

                //���ż��û����ϱ�����˾����
                if (upMark==1) 
                {
                    string args = dataitemdetailbll.GetItemValue("HidOrganize") + "," + dataitemdetailbll.GetItemValue("HidApprovalSetting");
                    //��ȡ����
                    ulist = userbll.GetUserListByDeptCode(null, args, true, curUser.OrganizeId);
                }
                //�а����û����ϱ�����������
                else if (upMark==2)
                {
                    string sendDeptID = departmentbll.GetEntity(curUser.DeptId).SendDeptID;

                    var SendDeptCode = "'" + departmentbll.GetEntity(sendDeptID).EnCode + "'";

                    ulist = userbll.GetUserListByDeptCode(SendDeptCode, dataitemdetailbll.GetItemValue("HidApprovalSetting"), false, curUser.OrganizeId);
                }
                else 
                {
                    //��ȡ�ϼ����ŵİ�ȫ����Ա ,����ֵ participant
                    ulist = userbll.GetParentUserByCurrent(curUser.UserId, dataitemdetailbll.GetItemValue("HidApprovalSetting"));
                }

                foreach (UserEntity u in ulist)
                {
                    participant += u.Account + ",";
                }

                if (!string.IsNullOrEmpty(participant))
                {
                    participant = participant.Substring(0, participant.Length - 1);  //�ϼ����Ű�ȫ����Ա

                    htworkflowbll.SubmitWorkFlowNoChangeStatus(keyValue, participant, curUser.UserId);

                    return Success("�����ɹ�!");
                }
                else
                {
                    return Error("��ǰ�ϼ������ް�ȫ����Ա,�����ϱ�,����ϵϵͳ����Ա��������!");
                }

            }
            else  //���ϱ�������ͨ����Ҫ�ύ���ģ�������ͨ���˻ص��Ǽ�
            {
                //����ID
                string APPROVALID = Request.Form["APPROVALID"] != null ? Request.Form["APPROVALID"].ToString() : "";

                string CHANGEID = Request.Form["CHANGEID"] != null ? Request.Form["CHANGEID"].ToString() : "";

                string ACCEPTID = Request.Form["ACCEPTID"] != null ? Request.Form["ACCEPTID"].ToString() : "";

                APPROVALID = APPROVALID == "&nbsp;" ? "" : APPROVALID;

                //�����ع�
                string EXPOSURESTATE = Request.Form["EXPOSURESTATE"] != null ? Request.Form["EXPOSURESTATE"].ToString() : "";

                /****�жϵ�ǰ���Ƿ�����ͨ��*****/

                //����ͨ������ֱ�ӽ�������
                if (entity.APPROVALRESULT == "1")
                {
                    wfFlag = "2";  //���ı�ʶ
                }
                else  //������ͨ�����˻ص��Ǽ� 
                {
                    wfFlag = "3";   //�ǼǱ�ʶ
                }
                participant = submitUser; //������ Or �Ǽ���

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    //���������ع�״̬
                    HTBaseInfoEntity baseentity = new HTBaseInfoBLL().GetEntity(keyValue);
                    baseentity.EXPOSURESTATE = EXPOSURESTATE;
                    baseentity.EXPOSUREDATETIME = DateTime.Now;
                    htbaseinfobll.SaveForm(keyValue, baseentity);

                    //���ڻ���ԭ�����Ҫ���ԭ���ύ
                    HTChangeInfoEntity cEntity = htchangeinfobll.GetEntityByCode(entity.HIDCODE);
                    if (!string.IsNullOrEmpty(cEntity.BACKREASON))
                    {
                        cEntity.BACKREASON = null;
                        htchangeinfobll.SaveForm(cEntity.ID, cEntity);
                    }

                    //��������
                    if (!string.IsNullOrEmpty(CHANGEID))
                    {
                        var tempEntity = htchangeinfobll.GetEntity(CHANGEID);
                        chEntity.AUTOID = tempEntity.AUTOID;
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

                    //��������������Ϣ
                    htapprovebll.SaveForm(APPROVALID, entity);

                    htworkflowbll.UpdateWorkStreamByObjectId(keyValue);  //����ҵ������״̬

                    return Success("�����ɹ�!");
                }
                else
                {
                    return Error("��ǰ�û�������Ȩ��!");
                }

            }
        }
        #endregion


    }
}
