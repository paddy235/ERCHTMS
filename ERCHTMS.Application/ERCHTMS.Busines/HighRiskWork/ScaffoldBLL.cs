using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Busines.JPush;
using ERCHTMS.IService.SystemManage;
using ERCHTMS.Service.SystemManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.OutsourcingProject;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּܴ��衢���ա��������2.���ּܴ��衢���ա��������
    /// </summary>
    public class ScaffoldBLL
    {
        private ScaffoldIService service = new ScaffoldService();
        private PeopleReviewIService peopleReviwservice = new PeopleReviewService();
        private IRoleService rolesevice = new RoleService();
        private ScaffoldauditrecordIService scaffoldauditrecordservice = new ScaffoldauditrecordService();
        private IDepartmentService departmentservice = new DepartmentService();
        private IManyPowerCheckService powerCheck = new ManyPowerCheckService();
        private IUserService userservice = new UserService();
        private IDataItemDetailService dataitemdetailservice = new DataItemDetailService();
        private HighRiskCommonApplyBLL highriskcommonapplybll = new HighRiskCommonApplyBLL();
        private SafetychangeBLL safetychangebll = new SafetychangeBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private FireWaterBLL firewaterbll = new FireWaterBLL();

        #region ��ȡ����

        /// <summary>
        /// �õ���ǰ�����
        /// ��Ź�����������ĸ+���+3λ������J2018001��J2018002��
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            return service.GetMaxCode();

        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination page, string queryJson)
        {
            return service.GetList(page, queryJson);
        }

        /// <summary>
        /// ̨���б�
        /// </summary>
        /// <param name="page"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetLedgerList(Pagination page, string queryJson, string authType)
        {
            return service.GetLedgerList(page, queryJson, authType);
        }

        /// <summary>
        /// ��ȡ�����µ�רҵ���
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public List<itemClass> GetSpecialtyToJson(string deptid, string specialtytype, string workdepttype)
        {
            List<itemClass> list = new List<itemClass>();
            if (!string.IsNullOrEmpty(specialtytype))
            {
                itemClass itementity = new itemClass();
                var strtype = getName(!string.IsNullOrEmpty(specialtytype) ? specialtytype : "", "SpecialtyType");
                itementity.itemvalue = specialtytype;
                itementity.itemname = strtype;
                list.Add(itementity);
            }
            else
            {
                IList<DepartmentEntity> dept = new List<DepartmentEntity>();
                if (workdepttype == "1" && departmentbll.GetEntity(deptid) == null)//�����λ
                {
                    dept.Add(departmentbll.GetEntity(new OutsouringengineerBLL().GetEntity(deptid).ENGINEERLETDEPTID));
                }
                else
                {

                    DepartmentEntity department = departmentbll.GetEntity(deptid);
                    dept.Add(department);
                    while (department.Nature != "����")
                    {
                        department = departmentbll.GetEntity(department.ParentId);
                        dept.Add(department);
                    }
                }
                if (dept.Count>0)
                {
                    string departmentid = "";
                    foreach (var item in dept)
                    {
                        departmentid += item.DepartmentId + ",";
                    }
                    if (!string.IsNullOrEmpty(departmentid))
                    {
                        departmentid = departmentid.Substring(0, departmentid.Length - 1);
                    }
                    IList<UserEntity> users = userservice.GetUserListByDeptId("'" + departmentid.Replace(",","','") + "'", "", false, string.Empty).OrderBy(t => t.RealName).ToList();
                    if (users != null && users.Count > 0)
                    {
                        foreach (var item in users)
                        {
                            if (!string.IsNullOrEmpty(item.SpecialtyType) && item.SpecialtyType != "null")
                            {
                                string[] str = item.SpecialtyType.Split(',');
                                for (int i = 0; i < str.Length; i++)
                                {
                                    var slist = list.Where(x => x.itemvalue.Equals(str[i])).ToList();
                                    if (slist.Count <= 0)
                                    {
                                        itemClass itementity = new itemClass();
                                        var strtype =getName(str[i], "SpecialtyType");
                                        if (!string.IsNullOrEmpty(strtype))
                                        {
                                            itementity.itemvalue = str[i];
                                            itementity.itemname = strtype;
                                            if (!list.Contains(itementity))
                                            {
                                                list.Add(itementity);
                                            } 
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// �õ�����ͼ
        /// </summary>
        /// <param name="keyValue">ҵ���ID</param>
        /// <param name="modulename">�����ģ����</param>
        /// <returns></returns>
        public Flow GetFlow(string keyValue, string modulename)
        {
            return service.GetFlow(keyValue, modulename);
        }

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public ScaffoldEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ���ּܴ���Ͳ��
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetSelectPageList(Pagination pagination, string queryJson)
        {
            return service.GetSelectPageList(pagination, queryJson);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, ScaffoldEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// �����ڱ��漰�޸�ʱ������ҵ�����ݼ���ʼ״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="model">ʵ�����</param>
        /// <param name="auditEntity">���ʵ�����</param>
        /// <returns></returns> 
        public void SaveForm(string keyValue, ScaffoldModel model)
        {
            try
            {
                List<RoleEntity> roles = (List<RoleEntity>)rolesevice.GetList();
                Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                ManyPowerCheckEntity mpcEntity = null;
                //û����˽ڵ�ʱ��Ĭ����˵Ľ�ɫ����Ϊ��ǰ�û�
                model.FlowDeptId = currUser.DeptId;
                model.FlowDeptName = currUser.DeptName;
                model.FlowRoleId = currUser.RoleId;
                model.FlowRoleName = currUser.RoleName;
                string moduleName = "";
                switch (model.ScaffoldType)
                {
                    case 0:

                        #region �����������
                        //���ͬ�⣬ֻ����6�����ϵ�
                        if (model.SetupCompanyType == 0)
                        {
                            moduleName = "(��������-�ڲ�-" + model.SetupTypeName + ")���";
                        }
                        else
                        {
                            moduleName = "(��������-���-" + model.SetupTypeName + ")���";
                        }
                        //if (model.SetupType == 1)
                        //{
                        //    if (model.SetupCompanyType == 0)
                        //    {
                        //        moduleName = "(��������-�ڲ�-6������)���";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(��������-���-6������)���";
                        //    }
                        //}
                        //else
                        //{
                        //    if (model.SetupCompanyType == 0)
                        //    {
                        //        moduleName = "(��������-�ڲ�-6������)���";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(��������-���-6������)���";
                        //    }
                        //}
                        mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, model.FlowId);
                        if (model.AuditState == 0)
                        {
                            model.FlowName = "������";
                            model.InvestigateState = 0;
                        }
                        if (model.AuditState == 1)
                        {
                            //�����˲��費Ϊ�գ�����������Ϣ��״̬
                            if (mpcEntity != null)
                            {
                                model.FlowDeptId = mpcEntity.CHECKDEPTID;
                                model.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                                model.FlowRoleId = mpcEntity.CHECKROLEID;
                                model.FlowRoleName = mpcEntity.CHECKROLENAME;
                                model.FlowId = mpcEntity.ID;
                                model.FlowName = mpcEntity.FLOWNAME;
                                model.AuditState = 1;
                                model.InvestigateState = 2;
                                model.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                                

                            }
                            else
                            {
                                model.FlowRemark = "";
                                model.FlowId = "";
                                model.FlowDeptId = "";
                                model.FlowDeptName = "";
                                model.FlowRoleId = "";
                                model.FlowRoleName = "";
                                model.AuditState = 3;
                                model.FlowName = "�����";
                                model.InvestigateState = 3;
                                
                                string Content = "���ּܴ������ͣ�" + model.SetupTypeName + "&#10;����ʱ�䣺" + model.SetupStartDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + model.SetupEndDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;����ص㣺" + model.SetupAddress;
                                UserEntity userEntity = userservice.GetEntity(model.ApplyUserId);
                                string[] workuserlist = (model.SetupChargePerson + "," + model.SetupPersons).Split(',');
                                DataTable dutyuserDt = new DataTable();
                                dutyuserDt = userservice.GetUserTable(workuserlist);
                                //���͸���ҵ������
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY021", "���ּܴ���������ͨ�����뼰ʱ����", Content, keyValue);
                                }
                                //���͸���ҵ������/��ҵ��
                                if (dutyuserDt.Rows.Count > 0)
                                {
                                    string Account = "";
                                    string RealName = "";
                                    foreach (DataRow item in dutyuserDt.Rows)
                                    {
                                        Account += item["account"].ToString() + ",";
                                        RealName += item["realname"].ToString() + ",";
                                    }
                                    if (!string.IsNullOrEmpty(Account))
                                    {
                                        Account = Account.Substring(0, Account.Length - 1);
                                        RealName = RealName.Substring(0, RealName.Length - 1);
                                    }
                                    JPushApi.PushMessage(Account, RealName, "ZY021", "����һ���µĽ��ּܴ�����ҵ�����뼰ʱ����", Content, keyValue);
                                }
                            }
                        }
                        #endregion

                        break;
                    case 1:

                        #region �����������
                        if (model.SetupCompanyType == 0)
                        {

                            moduleName = "(��������-�ڲ�-" + model.SetupTypeName + ")���";
                        }
                        else
                        {
                            moduleName = "(��������-���-" + model.SetupTypeName + ")���";
                        }
                        //if (model.SetupCompanyType == 0)
                        //{
                        //    if (model.SetupType == 0)
                        //    {

                        //        moduleName = "(��������-�ڲ�-6������)���";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(��������-�ڲ�-6������)���";
                        //    }
                        //}
                        //else
                        //{
                        //    if (model.SetupType == 0)
                        //    {
                        //        moduleName = "(��������-���-6������)���";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(��������-���-6������)���";
                        //    }

                        //}
                        mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, model.FlowId);
                        if (model.AuditState == 0)
                        {
                            model.FlowName = "������";
                            model.InvestigateState = 0;
                        }
                        if (model.AuditState == 1)
                        {
                            //��λ�ڲ��Ļ�����һ���������գ�����״̬��Ϊ������
                            
                            model.InvestigateState = 2;

                            if (mpcEntity != null)
                            {
                                model.FlowDeptId = mpcEntity.CHECKDEPTID;
                                model.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                                model.FlowRoleId = mpcEntity.CHECKROLEID;
                                model.FlowRoleName = mpcEntity.CHECKROLENAME;
                                model.FlowId = mpcEntity.ID;
                                model.FlowName = mpcEntity.FLOWNAME;
                                model.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                                if (model.FlowName.Contains("����ȷ��"))
                                {
                                    model.AuditState = 4;
                                }
                                else
                                {
                                    model.AuditState = 1;
                                }
                            }
                            else
                            {
                                model.FlowRemark = "";
                                model.AuditState = 3;
                                model.FlowName = "�����";
                                model.FlowDeptId = "";
                                model.FlowDeptName = "";
                                model.FlowRoleId = "";
                                model.FlowRoleName = "";
                                model.FlowId = "";
                                model.InvestigateState = 3;
                                //����ʵ�ʴ���ʱ��
                                var buildentity = this.GetEntity(model.SetupInfoId);
                                buildentity.ActSetupStartDate = model.ActSetupStartDate;
                                buildentity.ActSetupEndDate = model.ActSetupEndDate;
                                service.SaveForm(model.SetupInfoId, buildentity);
                                
                                string Content = "���ּ��������ͣ�" + model.SetupTypeName + "&#10;����ʱ�䣺" + model.SetupStartDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + model.SetupEndDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;���յص㣺" + model.SetupAddress;
                                UserEntity userEntity = userservice.GetEntity(model.ApplyUserId);
                                //���͸���ҵ������
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY022", "���ּܴ�������������ͨ����������ʱ����", Content, keyValue);
                                }
                            }
                        }

                        #endregion

                        break;
                    case 2:

                        #region ����������
                        if (model.SetupCompanyType == 0)
                        {
                            moduleName = "(������-�ڲ�-" + model.SetupTypeName + ")���";
                        }
                        else
                        {
                            moduleName = "(������-���-" + model.SetupTypeName + ")���";
                        }
                        //if (model.SetupType == 1)
                        //{
                        //    if (model.SetupCompanyType == 0)
                        //    {
                        //        moduleName = "(������-�ڲ�-6������)���";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(������-���-6������)���";
                        //    }
                        //}
                        //else
                        //{
                        //    if (model.SetupCompanyType == 0)
                        //    {
                        //        moduleName = "(������-�ڲ�-6������)���";
                        //    }
                        //    else
                        //    {
                        //        moduleName = "(������-���-6������)���";
                        //    }
                        //}
                        mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, model.FlowId);
                        if (model.AuditState == 0)
                        {
                            model.FlowName = "������";
                            model.InvestigateState = 0;
                        }
                        if (model.AuditState == 1)
                        {
                            //�����˲��費Ϊ�գ�����������Ϣ��״̬
                            if (mpcEntity != null)
                            {
                                model.FlowDeptId = mpcEntity.CHECKDEPTID;
                                model.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                                model.FlowRoleId = mpcEntity.CHECKROLEID;
                                model.FlowRoleName = mpcEntity.CHECKROLENAME;
                                model.FlowId = mpcEntity.ID;
                                model.FlowName = mpcEntity.FLOWNAME;
                                model.AuditState = 1;
                                model.InvestigateState = 2;
                                model.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                                
                            }
                            else
                            {
                                model.FlowRemark = "";
                                model.AuditState = 3;
                                model.FlowName = "�����";
                                model.FlowDeptId = "";
                                model.FlowDeptName = "";
                                model.FlowId = "";
                                model.FlowRoleId = "";
                                model.FlowRoleName = "";
                                model.InvestigateState = 3;

                                string Content = "���ּܲ�����ͣ�" + model.SetupTypeName + "&#10;���ʱ�䣺" + model.DismentleStartDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + model.DismentleEndDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;����ص㣺" + model.SetupAddress;
                                UserEntity userEntity = userservice.GetEntity(model.ApplyUserId);
                                string[] workuserlist = (model.SetupChargePersonIds + "," + model.DismentlePersonsIds).Split(',');
                                DataTable dutyuserDt = new DataTable();
                                dutyuserDt = userservice.GetUserTable(workuserlist);
                                //���͸���ҵ������
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY023", "���ּܲ��������ͨ�����뼰ʱ����", Content, keyValue);
                                }
                                //���͸���ҵ������/��ҵ��
                                if (dutyuserDt.Rows.Count > 0)
                                {
                                    string Account = "";
                                    string RealName = "";
                                    foreach (DataRow item in dutyuserDt.Rows)
                                    {
                                        Account += item["account"].ToString() + ",";
                                        RealName += item["realname"].ToString() + ",";
                                    }
                                    if (!string.IsNullOrEmpty(Account))
                                    {
                                        Account = Account.Substring(0, Account.Length - 1);
                                        RealName = RealName.Substring(0, RealName.Length - 1);
                                    }
                                    JPushApi.PushMessage(Account, RealName, "ZY023", "����һ���µĽ��ּܲ����ҵ�����뼰ʱ����", Content, keyValue);
                                }
                            }
                        }
                        #endregion

                        break;
                    default:
                        break;
                }

                service.SaveForm(keyValue, model);

                var entity = GetEntity(keyValue);
                if (entity.AuditState == 1)
                {
                    string executedept = string.Empty;
                    highriskcommonapplybll.GetExecutedept(entity.SetupCompanyType.ToString(), entity.SetupCompanyId, entity.OutProjectId, out executedept);//��ȡִ�в���
                    string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //��ȡ��������ID
                    string outsouringengineerdept = string.Empty;
                    highriskcommonapplybll.GetOutsouringengineerDept(entity.SetupCompanyId, out outsouringengineerdept);
                    string accountstr = powerCheck.GetApproveUserAccount(entity.FlowId, entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //��ȡ������˺�
                                                                                                                                                                                         //������Ϣ��������Ȩ�޵���
                    DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
                    string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                    switch (entity.ScaffoldType)
                    {
                        case 0:
                            JPushApi.PushMessage(accountstr, string.Join(",",usernames), "ZY005", "", "", entity.Id);
                            break;
                        case 1:
                            if (entity.AuditState == 4)
                            {
                                JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY007", "", "", entity.Id);
                            }
                            else
                            {
                                JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY008", "", "", entity.Id);
                            }
                            break;
                        case 2:
                            JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY010", "", "", entity.Id);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ����ϵͳ����Ϣ
        /// </summary>
        /// <param name="model"></param>
        public void SendMessage(string flowdeptid, string flowroleid, string code, string entityid, string title = "", string content = "", string type = "", string specialtytype = "")
        {
            string names = "";
            string accounts = "";
            string flowdeptids = "'" + flowdeptid.Replace(",", "','") + "'";
            string flowroleids = "'" + flowroleid.Replace(",", "','") + "'";
            IList<UserEntity> users = userservice.GetUserListByDeptId(flowdeptids, flowroleids, true, string.Empty).OrderBy(t => t.RealName).ToList();
            if (users != null && users.Count > 0)
            {
                if (!string.IsNullOrEmpty(specialtytype) && type == "1")
                {
                    foreach (var item in users)
                    {
                        if (item.RoleName.Contains("ר��"))
                        {
                            if (!string.IsNullOrEmpty(item.SpecialtyType) && item.SpecialtyType != "null")
                            {
                                string[] str = item.SpecialtyType.Split(',');
                                for (int i = 0; i < str.Length; i++)
                                {
                                    if (str[i] == specialtytype)
                                    {
                                        names += item.RealName + ",";
                                        accounts += item.Account + ",";
                                    }
                                }
                            }
                        }
                        else
                        {
                            names += item.RealName + ",";
                            accounts += item.Account + ",";
                        }
                    }
                    if (!string.IsNullOrEmpty(names))
                    {
                        names = names.TrimEnd(',');
                    }
                    if (!string.IsNullOrEmpty(accounts))
                    {
                        accounts = accounts.TrimEnd(',');
                    }
                }
                else
                {
                    names = string.Join(",", users.Select(x => x.RealName).ToArray());
                    accounts = string.Join(",", users.Select(x => x.Account).ToArray());
                }
                if (!string.IsNullOrEmpty(content))
                {
                    JPushApi.PushMessage(accounts, names, code, title, content, entityid);
                }
                else
                {
                    JPushApi.PushMessage(accounts, names, code, entityid);
                }
            }
        }

        /// <summary>
        /// �������
        /// ���������ʱ���޸�ҵ��״̬��Ϣ
        /// </summary>
        /// <param name="key">������Ϣ����ID</param>
        /// <param name="auditEntity">��˼�¼</param>
        /// <param name="checktype">Ϊ�����Ƿ�����Ŀ����ȷ�ϣ�1Ϊ��Ŀ����ȷ�� ������Ϊ������������</param>
        public void ApplyCheck(string keyValue, ScaffoldauditrecordEntity auditEntity, List<ScaffoldprojectEntity> projects, int checktype)
        {
            List<RoleEntity> roles = (List<RoleEntity>)rolesevice.GetList();
            ScaffoldEntity scaffoldEntity = service.GetEntity(keyValue);
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ManyPowerCheckEntity mpcEntity = null;
            string moduleName = "";

            //�ѵ�ǰҵ�����̽ڵ㸳ֵ����˼�¼��
            auditEntity.FlowId = scaffoldEntity.FlowId;

            //û����˽ڵ�ʱ��Ĭ����˵Ľ�ɫ����Ϊ��ǰ�û�
            scaffoldEntity.FlowDeptId = currUser.DeptId;
            scaffoldEntity.FlowDeptName = currUser.DeptName;
            scaffoldEntity.FlowRoleId = currUser.RoleId;
            scaffoldEntity.FlowRoleName = currUser.RoleName;
            switch (scaffoldEntity.ScaffoldType)
            {
                case 0:

                    #region �����������
                    //�����˼�¼��Ϊ�գ���Ϊ��ͬ�⣬���̽���
                    if (auditEntity != null && auditEntity.AuditState == 0)
                    {
                        scaffoldEntity.AuditState = 2;
                        scaffoldEntity.FlowName = "�����";
                        scaffoldEntity.InvestigateState = 3;
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";

                        //������ͨ��,����Ϣ�������� 2019.01.10 sx �ݲ�����
                        UserEntity userEntity = userservice.GetEntity(scaffoldEntity.CreateUserId);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY004", scaffoldEntity.Id);
                        }

                        break;
                    }

                    //���ͬ�⣬ֻ����6�����ϵ�
                    if (scaffoldEntity.SetupCompanyType == 0)
                    {
                        moduleName = "(��������-�ڲ�-" + scaffoldEntity.SetupTypeName + ")���";
                    }
                    else
                    {
                        moduleName = "(��������-���-" + scaffoldEntity.SetupTypeName + ")���";
                    }
                    //if (scaffoldEntity.SetupType == 1)
                    //{
                    //    if (scaffoldEntity.SetupCompanyType == 0)
                    //    {
                    //        moduleName = "(��������-�ڲ�-6������)���";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(��������-���-6������)���";
                    //    }
                    //}
                    //else
                    //{
                    //    if (scaffoldEntity.SetupCompanyType == 0)
                    //    {
                    //        moduleName = "(��������-�ڲ�-6������)���";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(��������-���-6������)���";
                    //    }
                    //}
                    mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, scaffoldEntity.FlowId);
                    //�����˲��費Ϊ�գ�����������Ϣ��״̬
                    if (mpcEntity != null)
                    {
                        scaffoldEntity.FlowDeptId = mpcEntity.CHECKDEPTID;
                        scaffoldEntity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        scaffoldEntity.FlowRoleId = mpcEntity.CHECKROLEID;
                        scaffoldEntity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        scaffoldEntity.FlowId = mpcEntity.ID;
                        scaffoldEntity.FlowName = mpcEntity.FLOWNAME;
                        scaffoldEntity.AuditState = 1;
                        scaffoldEntity.InvestigateState = 2;
                        scaffoldEntity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                        ////������Ϣ��������Ȩ�޵���
                        //string type = scaffoldEntity.FlowRemark != "1" ? "0" : "1";
                        //this.SendMessage(scaffoldEntity.FlowDeptId, scaffoldEntity.FlowRoleId, "ZY005", scaffoldEntity.Id, "", "", type, !string.IsNullOrEmpty(scaffoldEntity.SpecialtyType) ? scaffoldEntity.SpecialtyType : "");

                    }
                    else
                    {
                        scaffoldEntity.FlowRemark = "";
                        scaffoldEntity.AuditState = 3;
                        scaffoldEntity.FlowName = "�����";
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";
                        scaffoldEntity.InvestigateState = 3;

                        var high = GetEntity(scaffoldEntity.Id);
                        if (high != null)
                        {
                            string Content = "���ּܴ������ͣ�" + (high.SetupType == 0 ? "6�����½��ּܴ�������" : "6�����Ͻ��ּܴ�������") + "&#10;����ʱ�䣺" + high.SetupStartDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + high.SetupEndDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;����ص㣺" + high.SetupAddress;
                            UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                            string[] workuserlist = (high.SetupChargePersonIds + "," + high.SetupPersonIds).Split(',');
                            DataTable dutyuserDt = new DataTable();
                            dutyuserDt = userservice.GetUserTable(workuserlist);
                            //���͸���ҵ������
                            if (userEntity != null)
                            {
                                JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY021", "���ּܴ���������ͨ�����뼰ʱ����", Content, scaffoldEntity.Id);
                            }
                            //���͸���ҵ������/��ҵ��
                            if (dutyuserDt.Rows.Count > 0)
                            {
                                string Account = "";
                                string RealName = "";
                                foreach (DataRow item in dutyuserDt.Rows)
                                {
                                    Account += item["account"].ToString() + ",";
                                    RealName += item["realname"].ToString() + ",";
                                }
                                if (!string.IsNullOrEmpty(Account))
                                {
                                    Account = Account.Substring(0, Account.Length - 1);
                                    RealName = RealName.Substring(0, RealName.Length - 1);
                                }
                                JPushApi.PushMessage(Account, RealName, "ZY021", "����һ���µĽ��ּܴ�����ҵ�����뼰ʱ����", Content, scaffoldEntity.Id);
                            }
                        }
                    }
                    #endregion

                    break;
                case 1:

                    #region �����������

                    //�����˼�¼��Ϊ�գ���Ϊ��ͬ�⣬���̽���
                    if (auditEntity != null && auditEntity.AuditState == 0)
                    {
                        if (scaffoldEntity.FlowName.Contains("����ȷ��"))
                        {
                            scaffoldEntity.AuditState = 5; //�����ר������״̬Ϊ���ղ�ͨ��
                            scaffoldEntity.FlowName = string.IsNullOrEmpty(scaffoldEntity.FlowName) ? "��Ŀ����ȷ�ϲ�ͨ��" : scaffoldEntity.FlowName; ;
                            scaffoldEntity.InvestigateState = 1;
                            scaffoldEntity.FlowDeptId = "";
                            scaffoldEntity.FlowDeptName = "";
                            scaffoldEntity.FlowId = "";
                            scaffoldEntity.FlowRoleId = "";
                            scaffoldEntity.FlowRoleName = "";

                        }
                        else
                        {
                            scaffoldEntity.AuditState = 2;
                            scaffoldEntity.FlowName = string.IsNullOrEmpty(scaffoldEntity.FlowName) ? "��˲�ͨ��" : scaffoldEntity.FlowName;
                            scaffoldEntity.InvestigateState = 2;
                            scaffoldEntity.FlowDeptId = "";
                            scaffoldEntity.FlowDeptName = "";
                            scaffoldEntity.FlowId = "";
                            scaffoldEntity.FlowRoleId = "";
                            scaffoldEntity.FlowRoleName = "";
                        }

                        //������ͨ��,����Ϣ�������� 2019.01.10 sx �ݲ�����
                        UserEntity userEntity = userservice.GetEntity(scaffoldEntity.CreateUserId);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY006", scaffoldEntity.Id);
                        }

                        break;
                    }

                    //��λ�ڲ�
                    if (scaffoldEntity.SetupCompanyType == 0)
                    {
                        moduleName = "(��������-�ڲ�-" + scaffoldEntity.SetupTypeName + ")���";
                    }
                    else
                    {
                        moduleName = "(��������-���-" + scaffoldEntity.SetupTypeName + ")���";
                    }
                    //if (scaffoldEntity.SetupCompanyType == 0)
                    //{
                    //    if (scaffoldEntity.SetupType == 0)
                    //    {
                    //        moduleName = "(��������-�ڲ�-6������)���";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(��������-�ڲ�-6������)���";
                    //    }
                    //}
                    //else
                    //{
                    //    if (scaffoldEntity.SetupType == 0)
                    //    {
                    //        moduleName = "(��������-���-6������)���";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(��������-���-6������)���";
                    //    }
                    //}
                    mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, scaffoldEntity.FlowId);

                    //�����˲��費Ϊ�գ�����������Ϣ��״̬
                    if (mpcEntity != null)
                    {
                        scaffoldEntity.FlowDeptId = mpcEntity.CHECKDEPTID;
                        scaffoldEntity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        scaffoldEntity.FlowRoleId = mpcEntity.CHECKROLEID;
                        scaffoldEntity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        scaffoldEntity.FlowId = mpcEntity.ID;
                        scaffoldEntity.FlowName = mpcEntity.FLOWNAME;
                        scaffoldEntity.AuditState = 1;
                        scaffoldEntity.InvestigateState = 2;
                        scaffoldEntity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";
                        //������ͬ��
                        if (auditEntity != null && auditEntity.AuditState == 1)
                        {
                            //���̽ڵ�Ϊ����ȷ��ʱ��״̬Ϊ������
                            if (scaffoldEntity.FlowName.Contains("����ȷ��"))
                            {
                                scaffoldEntity.AuditState = 4;
                            }
                            if (checktype == 1)
                            {
                                scaffoldEntity.AuditState = 6;
                                scaffoldEntity.FlowName = string.IsNullOrEmpty(scaffoldEntity.FlowName) ? "��Ŀ����ȷ��ͨ��" : scaffoldEntity.FlowName;
                                scaffoldEntity.InvestigateState = 1;

                                //����ʵ�ʴ���ʱ��
                                var buildentity = this.GetEntity(scaffoldEntity.SetupInfoId);
                                buildentity.ActSetupStartDate = scaffoldEntity.ActSetupStartDate;
                                buildentity.ActSetupEndDate = scaffoldEntity.ActSetupEndDate;
                                service.SaveForm(scaffoldEntity.SetupInfoId, buildentity);
                            }
                            else
                            {
                                //����״̬Ϊ�����
                                scaffoldEntity.FlowName = string.IsNullOrEmpty(scaffoldEntity.FlowName) ? "�����" : scaffoldEntity.FlowName;
                            }

                            //string type = scaffoldEntity.FlowRemark != "1" ? "0" : "1";
                            //if (scaffoldEntity.AuditState == 4)
                            //{
                            //    //������Ϣ�����ּ�������Ŀȷ����
                            //    this.SendMessage(scaffoldEntity.FlowDeptId, scaffoldEntity.FlowRoleId, "ZY007", scaffoldEntity.Id, "", "", type, !string.IsNullOrEmpty(scaffoldEntity.SpecialtyType) ? scaffoldEntity.SpecialtyType : "");
                            //}
                            //else
                            //{
                            //    //������Ϣ�����ּ�����������
                            //    this.SendMessage(scaffoldEntity.FlowDeptId, scaffoldEntity.FlowRoleId, "ZY008", scaffoldEntity.Id, "", "", type, !string.IsNullOrEmpty(scaffoldEntity.SpecialtyType) ? scaffoldEntity.SpecialtyType : "");
                            //}
                        }

                    }
                    else
                    {
                        scaffoldEntity.FlowRemark = "";
                        scaffoldEntity.AuditState = 3;
                        scaffoldEntity.FlowName = "�����";
                        scaffoldEntity.InvestigateState = 3;
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";

                        //����ʵ�ʴ���ʱ��
                        var buildentity = this.GetEntity(scaffoldEntity.SetupInfoId);
                        buildentity.ActSetupStartDate = scaffoldEntity.ActSetupStartDate;
                        buildentity.ActSetupEndDate = scaffoldEntity.ActSetupEndDate;
                        service.SaveForm(scaffoldEntity.SetupInfoId, buildentity);


                        var high = GetEntity(scaffoldEntity.Id);
                        if (high != null)
                        {
                            string Content = "���ּ��������ͣ�" + (high.SetupType == 0 ? "6�����½��ּܴ�������" : "6�����Ͻ��ּܴ�������") + "&#10;����ʱ�䣺" + high.SetupStartDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + high.SetupEndDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;���յص㣺" + high.SetupAddress;
                            UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                            //���͸���ҵ������
                            if (userEntity != null)
                            {
                                JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY022", "���ּܴ�������������ͨ����������ʱ����", Content, scaffoldEntity.Id);
                            }
                        }
                    }

                    #endregion

                    break;
                case 2:

                    #region ����������

                    //�����˼�¼��Ϊ�գ���Ϊ��ͬ�⣬���̽���
                    if (auditEntity != null && auditEntity.AuditState == 0)
                    {
                        scaffoldEntity.AuditState = 2;
                        scaffoldEntity.FlowName = "�����";
                        scaffoldEntity.InvestigateState = 3;
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";

                        //������ͨ��,����Ϣ�������� 2019.01.10 sx �ݲ�����
                        UserEntity userEntity = userservice.GetEntity(scaffoldEntity.CreateUserId);
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY009", scaffoldEntity.Id);
                        }

                        break;
                    }
                    //���ͬ�⣬ֻ����6�����ϵ�
                    if (scaffoldEntity.SetupCompanyType == 0)
                    {
                        moduleName = "(������-�ڲ�-" + scaffoldEntity.SetupTypeName + ")���";
                    }
                    else
                    {
                        moduleName = "(������-���-" + scaffoldEntity.SetupTypeName + ")���";
                    }
                    //if (scaffoldEntity.SetupType == 1)
                    //{
                    //    if (scaffoldEntity.SetupCompanyType == 0)
                    //    {
                    //        moduleName = "(������-�ڲ�-6������)���";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(������-���-6������)���";
                    //    }
                    //}
                    //else
                    //{
                    //    if (scaffoldEntity.SetupCompanyType == 0)
                    //    {
                    //        moduleName = "(������-�ڲ�-6������)���";
                    //    }
                    //    else
                    //    {
                    //        moduleName = "(������-���-6������)���";
                    //    }
                    //}
                    mpcEntity = powerCheck.CheckAuditForNext(currUser, moduleName, scaffoldEntity.FlowId);
                    //�����˲��費Ϊ�գ�����������Ϣ��״̬
                    if (mpcEntity != null)
                    {
                        scaffoldEntity.FlowDeptId = mpcEntity.CHECKDEPTID;
                        scaffoldEntity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        scaffoldEntity.FlowRoleId = mpcEntity.CHECKROLEID;
                        scaffoldEntity.FlowRoleName = mpcEntity.CHECKROLENAME;
                        scaffoldEntity.FlowId = mpcEntity.ID;
                        scaffoldEntity.FlowName = mpcEntity.FLOWNAME;
                        scaffoldEntity.AuditState = 1;
                        scaffoldEntity.InvestigateState = 2;
                        scaffoldEntity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                        ////������Ϣ��������Ȩ�޵���
                        //string type = scaffoldEntity.FlowRemark != "1" ? "0" : "1";
                        //this.SendMessage(scaffoldEntity.FlowDeptId, scaffoldEntity.FlowRoleId, "ZY010", scaffoldEntity.Id, "", "", type, !string.IsNullOrEmpty(scaffoldEntity.SpecialtyType) ? scaffoldEntity.SpecialtyType : "");
                    }
                    else
                    {
                        scaffoldEntity.FlowRemark = "";
                        scaffoldEntity.AuditState = 3;
                        scaffoldEntity.FlowName = "�����";
                        scaffoldEntity.InvestigateState = 3;
                        scaffoldEntity.FlowDeptId = "";
                        scaffoldEntity.FlowDeptName = "";
                        scaffoldEntity.FlowId = "";
                        scaffoldEntity.FlowRoleId = "";
                        scaffoldEntity.FlowRoleName = "";


                        var high = GetEntity(scaffoldEntity.Id);
                        if (high != null)
                        {
                            string Content = "���ּܲ�����ͣ�" + (high.SetupType == 0 ? "6�����½��ּܴ�������" : "6�����Ͻ��ּܴ�������") + "&#10;���ʱ�䣺" + high.DismentleStartDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + high.DismentleEndDate.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;����ص㣺" + high.SetupAddress;
                            UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                            string[] workuserlist = (high.SetupChargePersonIds + "," + high.DismentlePersonsIds).Split(',');
                            DataTable dutyuserDt = new DataTable();
                            dutyuserDt = userservice.GetUserTable(workuserlist);
                            //���͸���ҵ������
                            if (userEntity != null)
                            {
                                JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY023", "���ּܲ��������ͨ�����뼰ʱ����", Content, scaffoldEntity.Id);
                            }
                            //���͸���ҵ������/��ҵ��
                            if (dutyuserDt.Rows.Count > 0)
                            {
                                string Account = "";
                                string RealName = "";
                                foreach (DataRow item in dutyuserDt.Rows)
                                {
                                    Account += item["account"].ToString() + ",";
                                    RealName += item["realname"].ToString() + ",";
                                }
                                if (!string.IsNullOrEmpty(Account))
                                {
                                    Account = Account.Substring(0, Account.Length - 1);
                                    RealName = RealName.Substring(0, RealName.Length - 1);
                                }
                                JPushApi.PushMessage(Account, RealName, "ZY023", "����һ���µĽ��ּܲ����ҵ�����뼰ʱ����", Content, scaffoldEntity.Id);
                            }
                        }
                    }

                    #endregion

                    break;
                default:
                    break;
            }

            this.service.UpdateForm(scaffoldEntity, auditEntity, projects);
            var entity = GetEntity(keyValue);
            if (entity.AuditState == 1)
            {
                string executedept = string.Empty;
                highriskcommonapplybll.GetExecutedept(entity.SetupCompanyType.ToString(), entity.SetupCompanyId, entity.OutProjectId, out executedept);//��ȡִ�в���
                string createdetpid = departmentservice.GetEntityByCode(entity.CreateUserDeptCode).IsEmpty() ? "" : departmentservice.GetEntityByCode(entity.CreateUserDeptCode).DepartmentId; //��ȡ��������ID
                string outsouringengineerdept = string.Empty;
                highriskcommonapplybll.GetOutsouringengineerDept(entity.SetupCompanyId, out outsouringengineerdept);
                string accountstr = powerCheck.GetApproveUserAccount(entity.FlowId, entity.Id, "", entity.SpecialtyType, executedept, outsouringengineerdept, createdetpid, "", ""); //��ȡ������˺�
                                                                                                                                                                                     //������Ϣ��������Ȩ�޵���
                DataTable dtuser = userservice.GetUserTable(accountstr.Split(','));
                string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                switch (entity.ScaffoldType)
                {
                    case 0:
                        JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY005", "", "", entity.Id);
                        break;
                    case 1:
                        if (entity.AuditState == 4)
                        {
                            JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY007", "", "", entity.Id);
                        }
                        else
                        {
                            JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY008", "", "", entity.Id);
                        }
                        break;
                    case 2:
                        JPushApi.PushMessage(accountstr, string.Join(",", usernames), "ZY010", "", "", entity.Id);
                        break;
                    default:
                        break;
                }
            }
        }
        
        /// <summary>
        /// ̨��״̬����
        /// </summary>
        /// <returns></returns>
        public void LedgerOp(string keyValue, string ledgerType, string type, string worktime, string issendmessage, string conditioncontent, string conditionid = "", string iscomplete = "")
        {
            string title = string.Empty;
            string message = string.Empty;
            string projectid = "";
            string workdeptid = "";
            var time = Convert.ToDateTime(worktime);
            Operator curUser = OperatorProvider.Provider.Current();
            string userids = "";
            if (type == "1")
            {
                #region ͨ�ø߷�����ҵ
                HighRiskCommonApplyEntity entity = highriskcommonapplybll.GetEntity(keyValue);
                if (entity != null)
                {
                    string sql = "";
                    workdeptid = entity.WorkDeptId;
                    if (entity.WorkDeptType == "1")
                    {
                        projectid = entity.EngineeringId;
                    }
                    title = "�߷�����ҵ��Ϣ";
                    if (ledgerType == "0")
                    {
                        sql = string.Format("update bis_highriskcommonapply set RealityWorkStartTime=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='0',RealityWorkEndTime='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                        entity.RealityWorkStartTime = time;
                        entity.WorkOperate = "0";
                        entity.RealityWorkEndTime = null;
                        //������ҵ
                        //����һ������Ϣ�����͸��������źͰ�ȫ���ܲ��ŵİ�ȫ����Ա��ר����������
                        //��Ϣ����Ϊ����ҵ��λ+��+��ҵ�ص�+��ʼ+��ҵ���ͣ����ã�2018��12��18��15ʱ33�֣����޲��ڻ�����ˮ��������ص�װ��ҵ
                        message = string.Format("����,{0},{1}��{2}��ʼ{3}��", time.ToString("yyyy��MM��dd�� HHʱmm��"), entity.WorkDeptName, entity.WorkPlace, getName(entity.WorkType, "CommonType"));

                        //
                        userids = entity.WorkUserIds;
                        HdgzUserPower(userids, 1);
                    }
                    else if (ledgerType == "1")
                    {
                        entity.RealityWorkEndTime = time;
                        if (iscomplete == "0")
                        {
                            entity.WorkOperate = "1";
                        }
                        sql = string.Format("update bis_highriskcommonapply set WorkOperate='{2}',RealityWorkEndTime=to_date('{1}','yyyy-mm-dd hh24:mi:ss') where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"), entity.WorkOperate);
                        //��ҵ��
                        //����һ������Ϣ�����͸��������źͰ�ȫ���ܲ��ŵİ�ȫ����Ա��ר����������
                        //��Ϣ����Ϊ��ʵ����ҵ����ʱ��,��ҵ��λ+��+��ҵ�ص�+��� +��ҵ���ͣ����ã�2018��12��18��15ʱ33�֣����޲��ڻ�����ˮ��������ص�װ��ҵ
                        message = string.Format("����,{0},{1}��{2}���{3}��", time.ToString("yyyy��MM��dd�� HHʱmm��"), entity.WorkDeptName, entity.WorkPlace, getName(entity.WorkType, "CommonType"));
                    }
                    highriskcommonapplybll.UpdateData(sql);
                    //highriskcommonapplybll.SaveApplyForm(keyValue, entity);
                }
                #endregion
            }
            else if (type == "2")
            {
                #region ���ּ���ҵ
                ScaffoldEntity entity = this.GetEntity(keyValue);
                if (entity != null)
                {
                    string sql = "";
                    workdeptid = entity.SetupCompanyId;
                    if (entity.SetupCompanyType == 1)
                    {
                        projectid = entity.OutProjectId;
                    }
                    if (ledgerType == "0")
                    {
                        sql = string.Format("update bis_scaffold set actsetupstartdate=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='0',actsetupenddate='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                        entity.ActSetupStartDate = time;
                        entity.WorkOperate = "0";
                        //��������
                        //����һ������Ϣ�����͸��������źͰ�ȫ���ܲ��ŵİ�ȫ����Ա��ר����������
                        //��Ϣ����Ϊ�����迪ʼʱ�䣬���赥λ+��+����ص�+��ʼ+��ҵ���ͣ���2018��12��18��15ʱ33�֣����޲��ڻ�����ˮ�ۿ�ʼ���ּܴ�����ҵ
                        message = string.Format("����,{0},{1}��{2}��ʼ{3}��", time.ToString("yyyy��MM��dd�� HHʱmm��"), entity.SetupCompanyName, entity.SetupAddress, "���ּܴ�����ҵ");
                        title = "���ּܴ�����ҵ��Ϣ";
                    }
                    if (ledgerType == "1")
                    {
                        
                        entity.ActSetupEndDate = time;
                        if (iscomplete == "0")
                        {
                            entity.WorkOperate = "1";
                        }
                        sql = string.Format("update bis_scaffold set actsetupenddate=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='{2}' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"),entity.WorkOperate);
                        //������
                        //����һ������Ϣ�����͸��������źͰ�ȫ���ܲ��ŵİ�ȫ����Ա��ר����������
                        //��Ϣ����Ϊ���������ʱ�䣬���赥λ+��+����ص�+��ʼ+���ּܴ�����ҵ����2018��12��18��15ʱ33�֣����޲��ڻ�����ˮ����ɽ��ּܴ�����ҵ
                        message = string.Format("����,{0},{1}��{2}���{3}��", time.ToString("yyyy��MM��dd�� HHʱmm��"), entity.SetupCompanyName, entity.SetupAddress, "���ּܴ�����ҵ");
                        title = "���ּܴ�����ҵ��Ϣ";
                    }
                    if (ledgerType == "4")
                    {
                        sql = string.Format("update bis_scaffold set realitydismentlestartdate=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='0',realitydismentleenddate='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                        entity.RealityDismentleStartDate = time;
                        entity.WorkOperate = "0";
                        //�������
                        //����һ������Ϣ�����͸��������źͰ�ȫ���ܲ��ŵİ�ȫ����Ա��ר����������
                        //��Ϣ����Ϊ�������ʼʱ�䣬�����λ+��+����ص�+��ʼ+��ҵ���ͣ���2018��12��18��15ʱ33�֣����޲��ڻ�����ˮ�ۿ�ʼ���ּܲ����ҵ
                        message = string.Format("����,{0},{1}��{2}��ʼ{3}��", time.ToString("yyyy��MM��dd�� HHʱmm��"), entity.SetupCompanyName, entity.SetupAddress, "���ּܲ����ҵ");
                        title = "���ּܲ����ҵ��Ϣ";
                    }
                    if (ledgerType == "5")
                    {
                        entity.RealityDismentleEndDate = time;
                        if (iscomplete == "0")
                        {
                            entity.WorkOperate = "1";
                        }
                        sql = string.Format("update bis_scaffold set realitydismentleenddate=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='{2}' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"), entity.WorkOperate);
                        //�����
                        //����һ������Ϣ�����͸��������źͰ�ȫ���ܲ��ŵİ�ȫ����Ա��ר����������
                        //��Ϣ����Ϊ���������ʱ�䣬�����λ+��+����ص�+��ʼ+���ּܲ����ҵ����2018��12��18��15ʱ33�֣����޲��ڻ�����ˮ����ɽ��ּܲ����ҵ
                        message = string.Format("����,{0},{1}��{2}���{3}��", time.ToString("yyyy��MM��dd�� HHʱmm��"), entity.SetupCompanyName, entity.SetupAddress, "���ּܲ����ҵ");
                        title = "���ּܲ����ҵ��Ϣ";
                    }
                    highriskcommonapplybll.UpdateData(sql);
                    //this.SaveForm(keyValue, entity);
                }
                #endregion
            }
            else if (type == "3")
            {
                #region ��ȫ��ʩ�䶯
                SafetychangeEntity entity = safetychangebll.GetEntity(keyValue);
                if (entity != null)
                {
                    workdeptid = entity.WORKUNITID;
                    if (entity.WORKUNITTYPE == "1")
                    {
                        projectid = entity.PROJECTID;
                    }
                    title = "��ȫ��ʩ�䶯��Ϣ";
                    if (ledgerType == "0")
                    {
                        entity.REALITYCHANGETIME = time;
                        //�����䶯
                        //����һ������Ϣ�����͸��������źͰ�ȫ���ܲ��ŵİ�ȫ����Ա��ר����������
                        //��Ϣ����Ϊ�����ã�ʵ�ʱ䶯��ʼʱ�䣬��ҵ��λ+��+��ҵ�ص�+��ʼ+��ȫ��ʩ����+�䶯��ʽ
                        message = string.Format("����,{0},{1}��{2}��ʼ{3}{4}��", time.ToString("yyyy��MM��dd�� HHʱmm��"), entity.WORKUNIT, entity.WORKPLACE, entity.CHANGENAME, entity.CHANGETYPE);

                    }
                    safetychangebll.SaveForm(keyValue, entity);
                }
                #endregion
            }
            else if (type == "4")
            {
                #region ʹ������ˮ
                FireWaterEntity entity = firewaterbll.GetEntity(keyValue);
                if (entity != null)
                {
                    workdeptid = entity.WorkDeptId;
                    if (entity.WorkDeptType == "1")
                    {
                        projectid = entity.EngineeringId;
                    }
                    title = "ʹ������ˮ��Ϣ";
                    string sql = "";
                    if (ledgerType == "0")
                    {
                        sql = string.Format("update bis_firewater set realityworkstarttime=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='0',realityworkendtime='' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"));
                        entity.RealityWorkStartTime = time;
                        entity.WorkOperate = "0";
                        //������ҵ
                        //����һ������Ϣ�����͸��������źͰ�ȫ���ܲ��ŵİ�ȫ����Ա��ר����������
                        //��Ϣ����Ϊ�� ʵ��ʹ������ˮ��ʼʱ�䣬ʹ������ˮ��λ+��+ʹ������ˮ�ص�+��ʼ��������ˮʹ����ҵ,��֪����
                        message = string.Format("����,{0},{1}��{2}��ʼ��������ˮʹ����ҵ,��֪����", time.ToString("yyyy��MM��dd�� HHʱmm��"), entity.WorkDeptName, entity.WorkPlace);

                    }
                    else if (ledgerType == "1")
                    {
                        entity.RealityWorkEndTime = time;
                        if (iscomplete == "0")
                        {
                            entity.WorkOperate = "1";
                        }
                        sql = string.Format("update bis_firewater set realityworkendtime=to_date('{1}','yyyy-mm-dd hh24:mi:ss'),WorkOperate='{2}' where id='{0}'", keyValue, time.ToString("yyyy-MM-dd HH:mm:ss"),entity.WorkOperate);
                        //��ҵ��
                        //����һ������Ϣ�����͸��������źͰ�ȫ���ܲ��ŵİ�ȫ����Ա��ר����������
                        //��Ϣ����Ϊ��ʵ��ʹ������ˮ����ʱ��,ʹ������ˮ��λ+��+ʹ������ˮ�ص�+��������ˮʹ����ҵ����֪����
                        message = string.Format("����,{0},{1}��{2}��������ˮʹ����ҵ,��֪����", time.ToString("yyyy��MM��dd�� HHʱmm��"), entity.WorkDeptName, entity.WorkPlace);
                    }
                    highriskcommonapplybll.UpdateData(sql);
                    //firewaterbll.SaveForm(keyValue, entity);
                }
                #endregion
            }
            #region ���ִ�������Ϣ
            FireWaterCondition Conditionentity = new FireWaterCondition();
            Conditionentity.Id = !string.IsNullOrEmpty(conditionid) ? conditionid : "";
            Conditionentity.LedgerType = ledgerType;
            Conditionentity.ConditionTime = time;
            Conditionentity.ConditionContent = conditioncontent;
            Conditionentity.ConditionDept = curUser.DeptName;
            Conditionentity.ConditionDeptCode = curUser.DeptCode;
            Conditionentity.ConditionDeptId = curUser.DeptId;
            Conditionentity.ConditionPerson = curUser.UserName;
            Conditionentity.ConditionPersonId = curUser.UserId;
            Conditionentity.FireWaterId = keyValue;
            firewaterbll.SubmitCondition(conditionid, Conditionentity);

            #endregion

            #region ���Ͷ���Ϣ
            //���Ͷ���Ϣ
            if (issendmessage == "1")
            {
                IList<UserEntity> users1 = null;
                if (!string.IsNullOrEmpty(projectid))
                {
                    //���õĽ�ɫ
                    string rolenames = dataitemdetailservice.GetItemValue("LedgerSendDept");
                    //���β���
                    DepartmentEntity departEntity = departmentservice.GetEntity(new OutsouringengineerService().GetEntity(projectid).ENGINEERLETDEPTID);
                    if (departEntity != null && !string.IsNullOrEmpty(rolenames))
                    {
                        rolenames = "'" + rolenames.Replace(",", "','") + "'";
                        users1 = userservice.GetUserListByRoleName("'" + departEntity.DepartmentId + "'", rolenames, true, string.Empty);
                    }
                }
                //��ȫ���ܲ���
                Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                IEnumerable<DepartmentEntity> departs = departmentservice.GetDepts(currUser.OrganizeId, 1);
                string deptids = string.Empty;
                //���õĽ�ɫ
                string orgrolenames = dataitemdetailservice.GetItemValue("LedgerManageDept");
                IList<UserEntity> users2 = null;
                if (departs != null && departs.Count() > 0 && !string.IsNullOrEmpty(orgrolenames))
                {
                    deptids = string.Join(",", departs.Select(x => x.DepartmentId).ToArray());
                    deptids = "'" + deptids.Replace(",", "','") + "'";
                    orgrolenames = "'" + orgrolenames.Replace(",", "','") + "'";
                    //��ȫ���ܲ��ţ���ɫ���û�
                    users2 = userservice.GetUserListByRoleName(deptids, orgrolenames, true, string.Empty);
                }
                //��ҵ��λ����
                IList<UserEntity> users3 = null;
                if (!string.IsNullOrEmpty(workdeptid))
                {
                    //���õĽ�ɫ
                    string rolenames = dataitemdetailservice.GetItemValue("LedgerWorkDept");
                    if (!string.IsNullOrEmpty(rolenames))
                    {
                        rolenames = "'" + rolenames.Replace(",", "','") + "'";
                        users3 = userservice.GetUserListByRoleName("'" + workdeptid + "'", rolenames, true, string.Empty);
                    }
                }
                List<UserEntity> users = new List<UserEntity>();
                if (users1 != null && users1.Count > 0)
                {
                    users.AddRange(users1);
                    users = users.Union(users1).ToList();
                }
                if (users2 != null && users2.Count > 0)
                {
                    users.AddRange(users2);
                    users = users.Union(users2).ToList();
                }
                if (users3 != null && users3.Count > 0)
                {
                    users.AddRange(users3);
                    users = users.Union(users3).ToList();
                }
                if (users != null && users.Count > 0)
                {
                    string names = string.Join(",", users.Select(x => x.RealName).ToArray());
                    string accounts = string.Join(",", users.Select(x => x.Account).ToArray());
                    var senduser = new UserBLL().GetUserInfoByAccount("System");
                    MessageEntity msg = new MessageEntity
                    {
                        Title = title,
                        Content = message,
                        UserId = accounts,
                        UserName = names,
                        Status = "",
                        Url = string.Empty,
                        SendUser = senduser.Account,
                        SendUserName = senduser.RealName,
                        Category = "�߷�����ҵ"
                    };
                    if (new MessageBLL().SaveForm("", msg))
                    {
                        JPushApi.PublicMessage(msg);
                    }
                }
            }
            #endregion
        }
        #endregion
        /// <summary>
        /// ���ñϽ���ԱȨ��
        /// </summary>
        public void HdgzUserPower(string userids,int isQx)
        {
            try
            {
                string isGZBJ = new ERCHTMS.Busines.SystemManage.DataItemDetailBLL().GetItemValue("���ݱϽڰ汾");
                if (string.IsNullOrWhiteSpace(isGZBJ))
                {
                    return;
                }
                DataItemDetailBLL data = new DataItemDetailBLL();
                var Key = data.GetItemValue("Hdgzappkey");//�Ͻ�URL��Կ
                var baseurl = data.GetItemValue("HdgzBaseUrl");//�Ͻ�API��������ַ

                string Url = "/api/v2/employee/level/";//�ӿڵ�ַ
                string[] levels = { "1"};
                string[] useridList = userids.Split(',');
                List<HdgzLevel> hdgzLevelList = new List<HdgzLevel>();
                foreach (string userid in useridList)
                {
                    HdgzLevel hdgzLevel = new HdgzLevel();
                    //ids += "'" + s + "',";
                    hdgzLevel.pin = new UserBLL().GetEntity(userid).IdentifyID;
                    hdgzLevel.levels = levels;
                    hdgzLevel.tag = isQx;
                    hdgzLevelList.Add(hdgzLevel);
                }
                
                //var model = new
                //{
                //    //pin = uentity.IdentifyID,
                //    levels = 1,
                //    tag = isQx
                //};
                SocketHelper.LoadHdgzCameraList(hdgzLevelList, baseurl, Url, Key);
            }
            catch { }
        }

        public string getName(string type, string encode)
        {
            string strName = "";
            var entity = new DataItemDetailBLL().GetDataItemListByItemCode("'" + encode + "'").Where(a => a.ItemValue == type).FirstOrDefault();
            if (entity != null)
                strName = entity.ItemName;
            return strName;
        }
        /// <summary>
        /// ��ȡ��Ա
        /// </summary>
        /// <param name="flowdeptid"></param>
        /// <param name="flowrolename"></param>
        /// <param name="type"></param>
        public string GetUserName(string flowdeptid, string flowrolename, string type = "", string specialtytype = "")
        {
            return service.GetUserName(flowdeptid, flowrolename, type, specialtytype);
        }

        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            return service.GetAppFlowList(keyValue, modulename);
        }
        public class itemClass
        {
            public string itemvalue { get; set; }
            public string itemname { get; set; }
        }
        //
        public class HdgzLevel
        {
            public string pin { get; set; }
            public object levels { get; set; }
            public object tag { get; set; }
        }
    }
}
