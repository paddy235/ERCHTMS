using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace ERCHTMS.Web.Areas.StandardSystem.Controllers
{
    /// <summary>
    /// �� ������׼�ޱ������
    /// </summary>
    public class StandardApplyController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //����ҵ�����
        private UserBLL userbll = new UserBLL(); //�û���������
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private StandardApplyBLL standardApplyBll = new StandardApplyBLL();
        private StandardCheckBLL standardCheckBll = new StandardCheckBLL();

        #region ��ͼ����
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// ����ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FlowDetail()
        {
            return View();
        }
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
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ��׼�ޱ������ͼ����
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetStandardApplyActionList(string keyValue)
        {
            var josnData = htworkflowbll.GetStandardApplyActionList(keyValue);
            return Content(josnData.ToJson());
        }
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetInitDataJson()
        {
            var FlowState = standardApplyBll.GetWorkDetailList("76c4c857-a3e1-45eb-9c61-e8e5dd9bf880");//��׼�ޣ�������ˣ���������
            var DataScope = new List<object>() { new { value = "1", text = "������ļ�¼" }, new { value = "2", text = "�Ҵ���ļ�¼" } };           
            //����ֵ
            var josnData = new
            {
                FlowState,//����״̬
                DataScope//���ݷ�Χ           
            };

            return Content(josnData.ToJson());
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            var watch = CommonHelper.TimerStart();            
            var data = standardApplyBll.GetList(pagination, queryJson);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// ��ȡʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = standardApplyBll.GetEntity(keyValue);                  
            //����ֵ
            var josnData = new
            {
                data = data
            };

            return Content(josnData.ToJson());
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
            standardApplyBll.RemoveForm(keyValue);//ɾ������
            standardCheckBll.RemoveForm(keyValue);//ɾ�����
            DeleteFiles(keyValue);//ɾ���ļ�

            return Success("ɾ���ɹ���");
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, StandardApplyEntity entity)
        {
            CommonSaveForm(keyValue,"05", entity);
            return Success("�����ɹ���");
        }
        /// <summary>
        /// �ύ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitForm(string keyValue, StandardApplyEntity entity)
        {
            CommonSaveForm(keyValue, "05", entity);
            //����������ʵ����
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }
            string errorMsg = "";
            if (!string.IsNullOrWhiteSpace(entity.CheckBackFlag))
            {// ���˺�������������
                errorMsg = ForwordFlowAfeterBack(keyValue, entity);
            }
            else
            {//�״���������
                errorMsg = ForwordFlowByDirectly(keyValue, entity);
            }
            if(!string.IsNullOrWhiteSpace(errorMsg))
            {
                return Error("����ϵϵͳ����Ա��ȷ��" + errorMsg + "!");
            }

            return Success("�����ɹ���");
        }
        /// <summary>
        /// ���˺�������������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string ForwordFlowAfeterBack(string keyValue, StandardApplyEntity entity)
        {
            var errorMsg = "";

            string wfFlag = entity.CheckBackFlag;           
            string checkUserId = entity.CheckBackUserID;
            string checkUserName = entity.CheckBackUserName;
            string checkDeptId = entity.CheckBackDeptID;
            string checkDeptName = entity.CheckBackDeptName;
            var uList = new UserBLL().GetListForCon(x => checkUserId.Contains(x.UserId)).Select(x => x.Account).ToList();
            string participant = string.Join(",", uList);

            Operator curUser = OperatorProvider.Provider.Current();
            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckDeptID = checkDeptId;
                entity.CheckDeptName = checkDeptName;
                entity.CheckUserID = checkUserId;
                entity.CheckUserName = checkUserName;
                standardApplyBll.SaveForm(keyValue, entity);

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //����ҵ������״̬
                    if (wfFlag == "7")
                        SendMessage(entity);
                }
            }
            else
            {
                errorMsg= "������";
            }

            return errorMsg;
        }
        /// <summary>
        /// �״���������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string ForwordFlowByDirectly(string keyValue, StandardApplyEntity entity)
        {
            var errorMsg = "";

            //�˴���Ҫ�жϵ�ǰ�˽�ɫ��
            string wfFlag = string.Empty;
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            string checkUserId = "";
            string checkUserName = "";
            string checkDeptId = "";
            string checkDeptName = "";
            //������Ա
            string participant = string.Empty;

            //�ܾ����û�
            DataItemModel president = dataitemdetailbll.GetDataItemListByItemCode("'PresidentApprove'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            //��׼���칫������û�
            DataItemModel checkuser = dataitemdetailbll.GetDataItemListByItemCode("'CheckUserAccount'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            //ֱ������2����˵Ĳ���
            DataItemModel check2dept = dataitemdetailbll.GetDataItemListByItemCode("'Check2Dept'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            //��ί���û�
            var fwhUser = new List<string>();
            if (president != null)
                fwhUser = userbll.GetListForCon(x => x.Account != president.ItemValue && x.RoleName.Contains("��˾���û�")).Select(x => x.Account).ToList();
            else
                fwhUser = userbll.GetListForCon(x => x.RoleName.Contains("��˾���û�")).Select(x=>x.Account).ToList();
            //�ҵ�ǰ�û����в��ţ��������ڰ����רҵ��            
            var dept = new DepartmentBLL().GetDepts(curUser.OrganizeId).Where(x => x.Nature == "����" && curUser.DeptCode.StartsWith(x.EnCode)).FirstOrDefault();
            //�����ƽ�
            if (president != null && president.ItemValue.Contains(curUser.Account))
            {//�ܾ���
                wfFlag = "7"; //����=>����
                errorMsg = "�ܾ���";
                participant = curUser.Account;
                checkUserId = curUser.UserId;
                checkUserName = curUser.UserName;
                checkDeptId = curUser.DeptId;
                checkDeptName = curUser.DeptName;
            }
            else if (fwhUser != null && fwhUser.Contains(curUser.Account))
            {//��ί��
                wfFlag = "6";//����=>�ܾ�������
                errorMsg = "�ܾ���";
                if (president != null)
                {
                    participant = president.ItemValue;
                    var pUser = userbll.GetUserInfoByAccount(participant);
                    if (pUser != null)
                    {
                        checkUserId = pUser.UserId;
                        checkUserName = pUser.RealName;
                        checkDeptId = pUser.DepartmentId;
                        checkDeptName = pUser.DeptName;
                    }
                }
            }
            else if (checkuser != null && checkuser.ItemValue.Contains(curUser.Account))
            {//��׼���칫������û�
                wfFlag = "3";//����=>��˷����ǩ
                errorMsg = "��׼���칫������û�";
                var chkStr = checkuser.ItemValue.Split(new char[] { '|' });
                participant = chkStr[0];
                var uList = userbll.GetListForCon(x => participant.Contains(x.Account)).ToList();
                checkUserId = string.Join(",", uList.Select(x => x.UserId));
                checkUserName = string.Join(",", uList.Select(x => x.RealName));
                checkDeptId = chkStr.Length >= 3 ? chkStr[1] : "";
                checkDeptName = chkStr.Length >= 3 ? chkStr[2] : "";
            }
            else if (dept != null && dept.DepartmentId == curUser.DeptId && userbll.HaveRoleListByKey(curUser.UserId, "'100104'").Rows.Count > 0)
            {//���ż������ˣ�����/���ܣ�
                wfFlag = "3";//����=>��˷����ǩ
                errorMsg = "��׼���칫������û�";
                var chkStr = checkuser.ItemValue.Split(new char[] { '|' });
                participant = chkStr[0];
                var uList = userbll.GetListForCon(x => participant.Contains(x.Account)).ToList();
                checkUserId = string.Join(",", uList.Select(x => x.UserId));
                checkUserName = string.Join(",", uList.Select(x => x.RealName));
                checkDeptId = chkStr.Length >= 3 ? chkStr[1] : "";
                checkDeptName = chkStr.Length >= 3 ? chkStr[2] : "";
            }
            else if (dept != null && dept.DepartmentId == curUser.DeptId && userbll.HaveRoleListByKey(curUser.UserId, "'100114'").Rows.Count > 0)
            { //���ż������û���������/���ܣ�
                wfFlag = "2";  // ����=>2�����
                errorMsg = "����/����";
                if (dept != null)
                {
                    var mgUser = userbll.GetUserListByRole(dept.EnCode, "'100104'", "");
                    participant = string.Join(",", mgUser.Select(x => x.Account));
                    checkUserId = string.Join(",", mgUser.Select(x => x.UserId));
                    checkUserName = string.Join(",", mgUser.Select(x => x.RealName));
                    checkDeptId = curUser.DeptId;
                    checkDeptName = curUser.DeptName;
                }
            }
            else if (check2dept != null && check2dept.ItemValue.Contains(curUser.DeptId))
            {//ֱ����������/������˵Ĳ���
                wfFlag = "2";  // ����=>2�����
                errorMsg = "����/����";
                if (dept != null)
                {
                    var mgUser = userbll.GetUserListByRole(dept.EnCode, "'100104'", "");
                    participant = string.Join(",", mgUser.Select(x => x.Account));
                    checkUserId = string.Join(",", mgUser.Select(x => x.UserId));
                    checkUserName = string.Join(",", mgUser.Select(x => x.RealName));
                    checkDeptId = curUser.DeptId;
                    checkDeptName = curUser.DeptName;
                }
            }
            else
            { //�����û�
                wfFlag = "1";  // ����=>1�����
                errorMsg = "������/����";                    
                if (dept != null)
                {
                    var mgUser = userbll.GetUserListByRole(dept.EnCode, "'100114'", "");
                    participant = string.Join(",", mgUser.Select(x => x.Account));
                    checkUserId = string.Join(",", mgUser.Select(x => x.UserId));
                    checkUserName = string.Join(",", mgUser.Select(x => x.RealName));
                    checkDeptId = curUser.DeptId;
                    checkDeptName = curUser.DeptName;
                }
            }

            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckDeptID = checkDeptId;
                entity.CheckDeptName = checkDeptName;
                entity.CheckUserID = checkUserId;
                entity.CheckUserName = checkUserName;
                standardApplyBll.SaveForm(keyValue, entity);

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", keyValue);  //����ҵ������״̬
                    if (wfFlag == "7")
                        SendMessage(entity);
                }
                errorMsg = "";
            }

            return errorMsg;
        }
        /// <summary>
        /// ���÷�������������
        /// </summary>
        public void CommonSaveForm(string keyValue, string workFlow, StandardApplyEntity entity)
        {
            //�ύͨ��
            string userId = OperatorProvider.Provider.Current().UserId;

            //���������Ϣ
            standardApplyBll.SaveForm(keyValue, entity);

            //��������ʵ��
            if (!htworkflowbll.IsHavaWFCurrentObject(entity.ID))
            {
                bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                if (isSucess)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_standardapply", "flowstate", entity.ID);  //����ҵ������״̬
                }
            }
        }
        /// <summary>
        /// ���Ͷ���Ϣ���Ѱ칫����Ա�����±�׼��ϵ���ݡ�
        /// </summary>
        /// <param name="entity"></param>
        private void SendMessage(StandardApplyEntity entity)
        {
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            //��׼���칫������û�
            DataItemModel checkuser = dataitemdetailbll.GetDataItemListByItemCode("'CheckUserAccount'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (checkuser != null)
            {
                var chkStr = checkuser.ItemValue.Split(new char[] { '|' });
                var userAccount = chkStr[0];
                var officeuser = new UserBLL().GetUserInfoByAccount(userAccount);
                MessageEntity msg = new MessageEntity()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userAccount,
                    UserName = officeuser.RealName,
                    SendTime = DateTime.Now,
                    SendUser = curUser.Account,
                    SendUserName = curUser.UserName,
                    Title = "��׼�ļ���������",
                    Content = string.Format("��{0}����׼�ޣ�����������������ɣ��뼴ʱ������", entity.FileName),
                    Category = "����"
                };
                if (new MessageBLL().SaveForm("", msg))
                {
                    JPushApi.PublicMessage(msg);
                }
            }
        }
        #endregion
    }
}
