using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Service.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using System.Data;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Service.BaseManage;
using ERCHTMS.IService.BaseManage;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� ����ʹ������ˮ
    /// </summary>
    public class FireWaterBLL
    {
        private FireWaterIService service = new FireWaterService();
        private PeopleReviewIService peopleReviwservice = new PeopleReviewService();
        private IUserService userservice = new UserService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetList(Pagination page, string queryJson, string authType, Operator user)
        {
            return service.GetList(page, queryJson, authType, user);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public FireWaterEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public FireWaterCondition GetConditionEntity(string fireWaterId)
        {
            return service.GetConditionEntity(fireWaterId);
        }

        /// <summary>
        /// ��ȡִ���������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public IList<FireWaterCondition> GetConditionList(string keyValue)
        {
            return service.GetConditionList(keyValue);
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
        /// ��ȡAPP����ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="modulename"></param>
        /// <returns></returns>
        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            return service.GetAppFlowList(keyValue, modulename);
        }
        #endregion

        #region ̨���б�
        /// <summary>
        /// ��ȡ����ˮʹ��̨��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetLedgerList(Pagination pagination, string queryJson, Operator user)
        {
            return service.GetLedgerList(pagination, queryJson,user);
        }
        #endregion
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
        public void SaveForm(string keyValue, FireWaterEntity entity)
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
        public void SaveForm(string keyValue, FireWaterModel model)
        {
            try
            {
                Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
                ManyPowerCheckEntity mpcEntity = null;
                if (model.WorkDeptType == "0")
                {
                    mpcEntity = peopleReviwservice.CheckAuditForNextByWorkUnit(currUser, "����ˮʹ��-�ڲ����", model.WorkDeptId, model.FlowId, false);
                }
                else
                {
                    mpcEntity = peopleReviwservice.CheckAuditForNextByOutsourcing(currUser, "����ˮʹ��-�ⲿ���", model.WorkDeptId, model.FlowId, false, true, model.EngineeringId);
                }
                if (model.ApplyState == "0")
                {
                    model.FlowName = "������";
                    model.InvestigateState = "0";
                }
                if (model.ApplyState == "1")
                {
                    //�����˲��費Ϊ�գ�����������Ϣ��״̬
                    if (mpcEntity != null)
                    {
                        model.FlowDept = mpcEntity.CHECKDEPTID;
                        model.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                        model.FlowRole = mpcEntity.CHECKROLEID;
                        model.FlowRoleName = mpcEntity.CHECKROLENAME;
                        model.FlowId = mpcEntity.ID;
                        model.FlowName = mpcEntity.FLOWNAME;
                        model.ApplyState = "1";
                        model.InvestigateState = "2";
                        model.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                        //������Ϣ��������Ȩ�޵���
                        string type = model.FlowRemark != "1" ? "0" : "1";
                        new ScaffoldBLL().SendMessage(model.FlowDept, model.FlowRole, "ZY100", model.Id, "", "", type, !string.IsNullOrEmpty(model.SpecialtyType) ? model.SpecialtyType : "");

                    }
                    else
                    {
                        model.FlowRemark = "";
                        model.FlowDept = " ";
                        model.FlowDeptName = " ";
                        model.FlowRole = " ";
                        model.FlowRoleName = " ";
                        model.ApplyState = "3";
                        model.FlowName = "�����";
                        model.InvestigateState = "3";

                        string Content = "��ҵ���ݣ�" + model.WorkContent + "&#10;��ҵʱ�䣺" + model.WorkStartTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + model.WorkEndTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;��ҵ�ص㣺" + model.WorkPlace;
                        UserEntity userEntity = userservice.GetEntity(model.CreateUserId);
                        string[] workuserlist = model.WorkUserIds.Split(',');
                        DataTable dutyuserDt = new DataTable();
                        dutyuserDt = userservice.GetUserTable(workuserlist);
                        //���͸���ҵ������
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY020", "����ˮʹ�����������ͨ�����뼰ʱ����", Content, keyValue);
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
                            JPushApi.PushMessage(Account, RealName, "ZY020", "����һ���µ�����ˮʹ����ҵ�����뼰ʱ����", Content, keyValue);
                        }
                    }
                }

                service.SaveForm(keyValue, model);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// �������
        /// ���������ʱ���޸�ҵ��״̬��Ϣ
        /// </summary>
        /// <param name="key">������Ϣ����ID</param>
        /// <param name="auditEntity">��˼�¼</param>
        public void ApplyCheck(string keyValue, ScaffoldauditrecordEntity auditEntity)
        {

            FireWaterEntity fireWaterEntity = service.GetEntity(keyValue);
            Operator currUser = ERCHTMS.Code.OperatorProvider.Provider.Current();
            ManyPowerCheckEntity mpcEntity = null;

            //�ѵ�ǰҵ�����̽ڵ㸳ֵ����˼�¼��
            auditEntity.FlowId = fireWaterEntity.FlowId;
            if (fireWaterEntity.WorkDeptType == "0")
            {
                mpcEntity = peopleReviwservice.CheckAuditForNextByWorkUnit(currUser, "����ˮʹ��-�ڲ����", fireWaterEntity.WorkDeptId, fireWaterEntity.FlowId, false);
            }
            else
            {
                mpcEntity = peopleReviwservice.CheckAuditForNextByOutsourcing(currUser, "����ˮʹ��-�ⲿ���", fireWaterEntity.WorkDeptId, fireWaterEntity.FlowId, false, true, fireWaterEntity.EngineeringId);
            }
            //�����˼�¼��Ϊ�գ���Ϊ��ͬ�⣬���̽���
            if (auditEntity.AuditState == 1)
            {
                //��һ�����̲�Ϊ��
                if (null != mpcEntity)
                {
                    fireWaterEntity.FlowDept = mpcEntity.CHECKDEPTID;
                    fireWaterEntity.FlowDeptName = mpcEntity.CHECKDEPTNAME;
                    fireWaterEntity.FlowRole = mpcEntity.CHECKROLEID;
                    fireWaterEntity.FlowRoleName = mpcEntity.CHECKROLENAME;
                    fireWaterEntity.FlowId = mpcEntity.ID;
                    fireWaterEntity.FlowName = mpcEntity.FLOWNAME;
                    fireWaterEntity.ApplyState = "1";
                    fireWaterEntity.InvestigateState = "2";
                    fireWaterEntity.FlowRemark = !string.IsNullOrEmpty(mpcEntity.REMARK) ? mpcEntity.REMARK : "";

                    //������Ϣ��������Ȩ�޵���
                    string type = fireWaterEntity.FlowRemark != "1" ? "0" : "1";
                    new ScaffoldBLL().SendMessage(fireWaterEntity.FlowDept, fireWaterEntity.FlowRole, "ZY100", fireWaterEntity.Id, "", "", type, !string.IsNullOrEmpty(fireWaterEntity.SpecialtyType) ? fireWaterEntity.SpecialtyType : "");
                }
                else
                {
                    fireWaterEntity.FlowRemark = "";
                    fireWaterEntity.FlowDept = " ";
                    fireWaterEntity.FlowDeptName = " ";
                    fireWaterEntity.FlowRole = " ";
                    fireWaterEntity.FlowRoleName = " ";
                    fireWaterEntity.ApplyState = "3";
                    fireWaterEntity.FlowName = "�����";
                    fireWaterEntity.InvestigateState = "3";
                    fireWaterEntity.FlowId = "";


                    var high = GetEntity(fireWaterEntity.Id);
                    if (high != null)
                    {
                        string Content = "��ҵ���ݣ�" + high.WorkContent + "&#10;��ҵʱ�䣺" + high.WorkStartTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + high.WorkEndTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;��ҵ�ص㣺" + high.WorkPlace;
                        UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                        string[] workuserlist = high.WorkUserIds.Split(',');
                        string[] copyuserlist = high.CopyUserIds.Split(',');
                        DataTable dutyuserDt = new DataTable();
                        dutyuserDt = userservice.GetUserTable(workuserlist);
                        //���͸���ҵ������
                        if (userEntity != null)
                        {
                            JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY020", "����ˮʹ�����������ͨ�����뼰ʱ����", Content, fireWaterEntity.Id);
                        }
                        DataTable copyuserdt = new DataTable();
                        copyuserdt = userservice.GetUserTable(copyuserlist);
                       
                       
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
                            JPushApi.PushMessage(Account, RealName, "ZY020", "����һ���µ�����ˮʹ����ҵ�����뼰ʱ����", Content, fireWaterEntity.Id);
                        }
                        //���͸�������
                        if (copyuserdt.Rows.Count > 0)
                        {
                            string Account = "";
                            string RealName = "";
                            foreach (DataRow item in copyuserdt.Rows)
                            {
                                Account += item["account"].ToString() + ",";
                                RealName += item["realname"].ToString() + ",";
                            }
                            if (!string.IsNullOrEmpty(Account))
                            {
                                Account = Account.Substring(0, Account.Length - 1);
                                RealName = RealName.Substring(0, RealName.Length - 1);
                            }
                            JPushApi.PushMessage(Account, RealName, "ZY020", "����ˮʹ�������������Ѿ�ͨ������֪��", Content, fireWaterEntity.Id);
                        }
                    }
                }
            }
            else
            {
                fireWaterEntity.FlowRemark = "";
                fireWaterEntity.FlowDept = " ";
                fireWaterEntity.FlowDeptName = " ";
                fireWaterEntity.FlowRole = " ";
                fireWaterEntity.FlowRoleName = " ";
                fireWaterEntity.ApplyState = "2";
                fireWaterEntity.FlowName = "�����";
                fireWaterEntity.InvestigateState = "3";
                fireWaterEntity.FlowId = "";
                //������ͨ��,����Ϣ��������
                UserEntity userEntity = new UserService().GetEntity(fireWaterEntity.CreateUserId);
                if (userEntity != null)
                {
                    var high = GetEntity(fireWaterEntity.Id);
                    if (high != null)
                    {
                        string Content = "��ҵ���ݣ�" + high.WorkContent + "&#10;��ҵʱ�䣺" + high.WorkStartTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + high.WorkEndTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;��ҵ�ص㣺" + high.WorkPlace;
                        JPushApi.PushMessage(userEntity.Account, userEntity.RealName, "ZY019", "����ˮʹ���������δͨ�����뼰ʱ����", Content, fireWaterEntity.Id);
                    }
                }
            }
            this.service.UpdateForm(fireWaterEntity, auditEntity);

        }


        /// <summary>
        /// �ύִ�����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public void SubmitCondition(string keyValue, FireWaterCondition entity) {
            this.service.SubmitCondition(keyValue, entity);
        }
    }
}
