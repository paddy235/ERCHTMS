using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.HiddenTroubleManage;
using ERCHTMS.Busines.WorkPlan;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.WorkPlan;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Web.Areas.WorkPlan.Controllers
{
    /// <summary>
    /// �� ����EHS�ƻ������
    /// </summary>
    public class PlanApplyController : MvcControllerBase
    {
        private HTWorkFlowBLL htworkflowbll = new HTWorkFlowBLL(); //����ҵ�����
        private UserBLL userbll = new UserBLL(); //�û���������
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private PlanApplyBLL planApplyBll = new PlanApplyBLL();

        #region ��ͼ����
        [HttpGet]
        public ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// ���Ź����ƻ�����ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DepartFlowDetail()
        {
            return View();
        }
        /// <summary>
        /// ���˹����ƻ�����ͼ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PersonFlowDetail()
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
            ViewBag.ehsDepartCode = "";
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();
            if (ehsDepart != null)
                ViewBag.ehsDepartCode = ehsDepart.ItemValue;
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
        /// ��������ƻ�
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangeForm()
        {
            return View();
        }
        /// <summary>
        /// �����ʷ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult History()
        {
            return View();
        }
        #endregion

        #region ��ȡ����
        /// <summary>
        /// ��ȡ��������ͼ����
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetDepartPlanApplyActionList(string keyValue)
        {
            var josnData = htworkflowbll.GetDepartPlanApplyActionList(keyValue);
            return Content(josnData.ToJson());
        }
        /// <summary>
        /// ��ȡ��������ͼ����
        /// </summary>
        [HttpPost]
        [AjaxOnly]
        public ActionResult GetPersonPlanApplyActionList(string keyValue)
        {
            var josnData = htworkflowbll.GetPersonPlanApplyActionList(keyValue);
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
            var data = planApplyBll.GetList(pagination, queryJson);
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
            var data = planApplyBll.GetEntity(keyValue);                  
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
            planApplyBll.RemoveForm(keyValue);//ɾ������
            new PlanDetailsBLL().RemoveFormByApplyId(keyValue);//ɾ������
            new PlanCheckBLL().RemoveForm(keyValue);//ɾ�����           
            htworkflowbll.DeleteWorkFlowObj(keyValue);//ɾ������

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
        public ActionResult SaveForm(string keyValue, PlanApplyEntity entity)
        {
            string workflow = entity.ApplyType == "���Ź����ƻ�" ? "06" : "07";
            CommonSaveForm(keyValue, workflow, entity);
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
        public ActionResult SubmitForm(string keyValue, PlanApplyEntity entity)
        {
            string workflow = entity.ApplyType == "���Ź����ƻ�" ? "06" : "07";
            CommonSaveForm(keyValue, workflow, entity);
            //����������ʵ����
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }
            string errorMsg = "";
            if (workflow == "06")
            {//���Ź����ƻ�
                errorMsg = DepartFlow(keyValue, entity);
            }
            else
            {//���˹����ƻ�
                errorMsg = PersonFlow(keyValue, entity);
            }

            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                return Error("����ϵϵͳ����Ա��ȷ��" + errorMsg + "!");
            }

            return Success("�����ɹ���");
        }
        /// <summary>
        /// ��������ƻ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SubmitChangeForm(string keyValue, PlanApplyEntity entity)
        {
            string workflow = entity.ApplyType == "���Ź����ƻ�" ? "06" : "07";  
            //������ʷ��¼
            var oldEntity = planApplyBll.GetEntity(keyValue);
            if (oldEntity != null)
            {
                var newApplyId = Request["NewId"];
                oldEntity.ID = newApplyId;
                oldEntity.BaseId = entity.ID;                
                planApplyBll.SaveForm("", oldEntity);

                var planDetailsBll = new PlanDetailsBLL();
                var list = planDetailsBll.GetList(string.Format(" and baseid in(select id from hrs_plandetails d where d.applyid='{0}') and not exists(select 1 from hrs_planapply a where a.id=hrs_plandetails.applyid)", keyValue)).ToList();
                foreach(var epd in list)
                {//������δ�ύ��ʧ����ʷ���ݡ�
                    epd.ApplyId = newApplyId;
                    planDetailsBll.SaveForm(epd.ID, epd);
                }
            }
            //ɾ�������̴���������   
            htworkflowbll.DeleteWorkFlowObj(keyValue);
            CommonSaveForm(keyValue, workflow, entity);
            //����������ʵ����
            if (string.IsNullOrEmpty(keyValue))
            {
                keyValue = entity.ID;
            }
            string errorMsg = "";
            if (workflow == "06")
            {//���Ź����ƻ�
                errorMsg = DepartFlow(keyValue, entity);
            }
            else
            {//���˹����ƻ�
                errorMsg = PersonFlow(keyValue, entity);
            }

            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                return Error("����ϵϵͳ����Ա��ȷ��" + errorMsg + "!");
            }            

            return Success("�����ɹ���");
        }
        /// <summary>
        /// ���Ź����ƻ���������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string DepartFlow(string keyValue, PlanApplyEntity entity)
        {
            var errorMsg = "";

            //�˴���Ҫ�жϵ�ǰ�˽�ɫ��
            string wfFlag = string.Empty;
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();
            //������Ա
            string participant = string.Empty;

            //EHS����encode
            DataItemModel ehsDepart = dataitemdetailbll.GetDataItemListByItemCode("'EHSDepartment'").Where(p => p.ItemName == curUser.OrganizeId).ToList().FirstOrDefault();           
            //�ֹ��쵼
            //var fgLeaderUser = userbll.GetListForCon(x => x.RoleName.Contains("��˾���û�") && x.RoleName.Contains("��ȫ����Ա")).Select(x => x.Account).ToList();        
            //��ȫ����Ա�����Ÿ�����
            //var isSafer = userbll.HaveRoleListByKey(curUser.UserId, "'100105'").Rows.Count > 0;
            var isManager = userbll.HaveRoleListByKey(curUser.UserId, "'100104'").Rows.Count > 0;
            //�����ƽ�
            if (ehsDepart != null && ehsDepart.ItemValue == curUser.DeptCode&&isManager == true) {
                wfFlag = "3"; //����=>����
                errorMsg = ">�ֹ��쵼";
                //�ֹ��쵼
                var fgLeaderUser = userbll.GetListForCon(x => x.RoleName.Contains("��˾���û�") && x.RoleName.Contains("��ȫ����Ա")).Select(x => x.Account).ToList();
                participant = string.Join(",", fgLeaderUser);
            }
            else if ((ehsDepart != null && ehsDepart.ItemValue == curUser.DeptCode) || (curUser.RoleName.Contains("���ż�") && isManager == true))
            {//EHS��
                wfFlag = "2"; //����=>EHS���������
                errorMsg = ">EHS������";
                var deptUser = userbll.GetUserListByRole(ehsDepart.ItemValue, "'100104'", "");
                participant = string.Join(",", deptUser.Select(x => x.Account));
            }
            else if (curUser.RoleName.Contains("�а���"))
            {
                //�а��������й��̵����β���
                wfFlag = "1";//����=>���Ÿ��������
                errorMsg = "�������β��Ÿ�����";
                var projectList = new OutsouringengineerBLL().GetList().Where(x => x.OUTPROJECTID == curUser.DeptId).ToList() ;
                var deptUser = new List<UserEntity>();
                for (int i = 0; i < projectList.Count; i++)
                {
                    var pDept = new DepartmentBLL().GetEntity(projectList[i].ENGINEERLETDEPTID);
                    if (pDept != null) {
                        deptUser = userbll.GetUserListByRole(pDept.EnCode, "'100104'", "").ToList();
                    }
                    if (!string.IsNullOrWhiteSpace(participant))
                    {
                        participant += "," + string.Join(",", deptUser.Select(x => x.Account));
                    }
                    else {
                        participant +=  string.Join(",", deptUser.Select(x => x.Account));
                    }
               
                }
            }
            else {
                //��ȫ����Ա
                wfFlag = "1";//����=>���Ÿ��������
                errorMsg = "���Ÿ�����";
                if (curUser.RoleName.Contains("���ż�"))
                {
                    var deptUser = userbll.GetUserListByRole(curUser.DeptCode, "'100104'", "");
                    participant = string.Join(",", deptUser.Select(x => x.Account));
                }
                else {
                    var pDept = new DepartmentBLL().GetParentDeptBySpecialArgs(curUser.ParentId, "����");
                    var deptUser = userbll.GetUserListByRole(pDept.DeptCode, "'100104'", "");
                    participant = string.Join(",", deptUser.Select(x => x.Account));
                }
                
            }

            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckUserAccount = participant;
                planApplyBll.SaveForm(keyValue, entity);
                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);
                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_PlanApply", "flowstate", keyValue);  //����ҵ������״̬
                }
                errorMsg = "";
            }

            return errorMsg;
        }
        /// <summary>
        /// ���˹����ƻ���������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string PersonFlow(string keyValue, PlanApplyEntity entity)
        {
            var errorMsg = "";

            //�˴���Ҫ�жϵ�ǰ�˽�ɫ��
            string wfFlag = string.Empty;
            //��ǰ�û�
            Operator curUser = OperatorProvider.Provider.Current();        
            //������Ա
            string participant = string.Empty;
            //�����ƽ�
            if (userbll.HaveRoleListByKey(curUser.UserId, "'100108'").Rows.Count > 0)
            {//��˾���û�
                wfFlag = "1";  // ����=>��˾����������
                errorMsg = "��˾������";
                var deptUser = userbll.GetUserListByRole(curUser.DeptCode, "'100104'", "");
                participant = string.Join(",", deptUser.Select(x => x.Account));
            }
            else if (userbll.HaveRoleListByKey(curUser.UserId, "'100104'").Rows.Count > 0)
            { //���Ÿ�����
                wfFlag = "1";  // ����=>�ϼ��쵼����
                errorMsg = "�ϼ��쵼";
                //�ҵ�ǰ�û��ϼ����Ÿ����ˡ�            
                var dept = new DepartmentBLL().GetEntity(curUser.ParentId);
                if (dept != null)
                {
                    var deptUser = userbll.GetUserListByRole(dept.EnCode, "'100104'", "");
                    participant = string.Join(",", deptUser.Select(x => x.Account));
                }
            }            
            else
            { //�����û�
                wfFlag = "1";  // ����=>�����Ÿ���������
                errorMsg = "���Ÿ�����";                
                var deptUser = userbll.GetUserListByRole(curUser.DeptCode, "'100104'", "");
                participant = string.Join(",", deptUser.Select(x => x.Account));
            }

            if (!string.IsNullOrEmpty(participant))
            {
                entity.CheckUserAccount = participant;
                planApplyBll.SaveForm(keyValue, entity);

                int count = htworkflowbll.SubmitWorkFlow(keyValue, participant, wfFlag, curUser.UserId);

                if (count > 0)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_planapply", "flowstate", keyValue);  //����ҵ������״̬
                }
                errorMsg = "";
            }

            return errorMsg;
        }
        /// <summary>
        /// ���÷�������������
        /// </summary>
        public void CommonSaveForm(string keyValue, string workFlow, PlanApplyEntity entity)
        {
            //�ύͨ��
            string userId = OperatorProvider.Provider.Current().UserId;

            //���������Ϣ
            planApplyBll.SaveForm(keyValue, entity);

            //��������ʵ��
            if (!htworkflowbll.IsHavaWFCurrentObject(entity.ID))
            {
                bool isSucess = htworkflowbll.CreateWorkFlowObj(workFlow, entity.ID, userId);
                if (isSucess)
                {
                    htworkflowbll.UpdateFlowStateByObjectId("hrs_planapply", "flowstate", entity.ID);  //����ҵ������״̬
                }
            }
        }
        #endregion
    }
}
