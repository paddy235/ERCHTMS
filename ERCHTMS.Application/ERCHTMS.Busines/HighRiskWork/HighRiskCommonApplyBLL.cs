using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Service.BaseManage;
using System.Linq;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.IService.BaseManage;
using System.Collections.Generic;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Collections;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� �����߷���ͨ����ҵ����
    /// </summary>
    public class HighRiskCommonApplyBLL
    {
        private HighRiskCommonApplyIService service = new HighRiskCommonApplyService();
        private IManyPowerCheckService manypowercheckservice = new ManyPowerCheckService();
        private IUserService userservice = new UserService();

        #region ��ȡ����

        /// <summary>
        /// �õ���ǰ�����
        /// ��Ź�����������ĸ+���+3λ������J2018001��J2018002��
        /// </summary>
        /// <returns></returns>
        public string GetMaxCode()
        {
            object o = service.GetMaxCode();
            if (o == null || o.ToString() == "")
                return "G" + DateTime.Now.Year + "001";
            int num = Convert.ToInt32(o.ToString().Substring(4));
            return "G" + DateTime.Now.Year + num.ToString().PadLeft(3, '0');
        }

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public IEnumerable<HighRiskCommonApplyEntity> GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<HighRiskCommonApplyEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public HighRiskCommonApplyEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ�߷���ͨ��̨��
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetLedgerList(Pagination pagination, string queryJson, Boolean GetOperate = true)
        {
            return service.GetLedgerList(pagination, queryJson, GetOperate);
        }

        /// <summary>
        /// ��ȡ�߷���ͨ����ҵ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        public DataTable GetPageDataTable(Pagination pagination, string queryJson)
        {
            return service.GetPageDataTable(pagination, queryJson);
        }

        public DataTable GetTable(string sql)
        {
            return service.GetTable(sql);
        }

        /// <summary>
        /// ת������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        public void TransformSendMessage(TransferrecordEntity entity)
        {
            //1���жϵ�ǰ�Ǵ�ʩȷ�ϻ��������׶�
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            HighRiskCommonApplyEntity commonEntity = service.GetEntity(entity.RecId);
            string moduleName = GetModuleName(commonEntity);
            PushMessageData pushdata = new PushMessageData();
            if (commonEntity.FlowName == "ȷ����")
            {
                //��ʩȷ��ת��
                pushdata.SendCode = "ZY001";
            }
            else
            {
                //���ת��
                pushdata.SendCode = "ZY002";
            }
            //��������
            pushdata.EntityId = commonEntity.Id;
            pushdata.UserAccount = entity.InTransferUserAccount;
            PushMessageForCommon(pushdata);
        }

        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetModuleName(HighRiskCommonApplyEntity entity)
        {
            return service.GetModuleName(entity);
        }

        /// <summary>
        /// ��ȡִ�в���
        /// </summary>
        /// <param name="workdepttype">��ҵ��λ����</param>
        /// <param name="workdept">��ҵ��λ</param>
        /// <param name="projectid">�������ID</param>
        /// <param name="Executedept">ִ�в���</param>
        public void GetExecutedept(string workdepttype, string workdept, string projectid, out string Executedept)
        {
            service.GetExecutedept(workdepttype, workdept, projectid, out Executedept);
        }

        /// <summary>
        /// ��ȡ�����λ
        /// </summary>
        /// <param name="workdept">��ҵ��λ</param>
        /// <param name="outsouringengineerdept"></param>
        public void GetOutsouringengineerDept(string workdept, out string outsouringengineerdept)
        {
            service.GetOutsouringengineerDept(workdept, out outsouringengineerdept);
        }

        public List<CheckFlowData> GetAppFlowList(string keyValue, string modulename)
        {
            return service.GetAppFlowList(keyValue, modulename);
        }

        public Flow GetFlow(string keyValue, string modulename)
        {
            return service.GetFlow(keyValue, modulename);
        }

        /// <summary>
        /// �޸�sql���
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int UpdateData(string sql)
        {
            return service.UpdateData(sql);
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
        /// ��������
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateForm(HighRiskCommonApplyEntity entity)
        {
            try
            {
                service.UpdateForm(entity);
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
        public void SaveForm(string keyValue, string type, HighRiskCommonApplyEntity entity, List<HighRiskRecordEntity> list, List<HighRiskApplyMBXXEntity> mbList)
        {
            try
            {
                PushMessageData pushdata = service.SaveForm(keyValue, type, entity, list, mbList);
                if (pushdata != null)
                {
                    if (pushdata.Success == 1 && !string.IsNullOrEmpty(pushdata.SendCode))
                    {
                        pushdata.Content = getName(entity.WorkType, "CommonType");
                        if (pushdata.SendCode == "ZY018")
                        {
                            var high = GetEntity(entity.Id);
                            if (high != null)
                            {
                                String CommonType = pushdata.Content;
                                pushdata.Content = "��ҵ���ݣ�" + high.WorkContent + "&#10;��ҵʱ�䣺" + high.WorkStartTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + high.WorkEndTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;��ҵ�ص㣺" + high.WorkPlace;
                                UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                                UserEntity tutelageuserEntity = userservice.GetEntity(high.WorkTutelageUserId);
                                string[] workuserlist = (high.WorkDutyUserId + "," + high.WorkUserIds).Split(',');
                                //List<string> b = workuserlist.ToList();
                                //b.Add(high.WorkDutyUserId);
                                //workuserlist = b.ToArray();
                                DataTable dutyuserDt = new DataTable();
                                dutyuserDt = userservice.GetUserTable(workuserlist);
                                //���͸���ҵ������
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, pushdata.SendCode, "�߷�����ҵ(" + CommonType + "��������ͨ�����뼰ʱ����", pushdata.Content, pushdata.EntityId);
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
                                    JPushApi.PushMessage(Account, RealName, pushdata.SendCode, "����һ���µĸ߷�����ҵ(" + CommonType + ")�����뼰ʱ����", pushdata.Content, pushdata.EntityId);
                                }
                                //���͸���ҵ�໤��
                                if (tutelageuserEntity != null)
                                {
                                    JPushApi.PushMessage(tutelageuserEntity.Account, tutelageuserEntity.RealName, pushdata.SendCode, "����һ���µĸ߷�����ҵ(" + CommonType + ")�໤�����뼰ʱ����", pushdata.Content, pushdata.EntityId);
                                }
                            }
                        }
                        else
                        {
                            //��������
                            PushMessageForCommon(pushdata);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveApplyForm(string keyValue, HighRiskCommonApplyEntity entity)
        {
            try
            {
                service.SaveApplyForm(keyValue, entity);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ȷ�ϣ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="state"></param>
        /// <param name="recordData"></param>
        /// <param name="entity"></param>
        /// <param name="aentity"></param>
        public void SubmitCheckForm(string keyValue, string state, string recordData, HighRiskCommonApplyEntity entity, ScaffoldauditrecordEntity aentity)
        {
            try
            {
                PushMessageData pushdata = service.SubmitCheckForm(keyValue, state, recordData, entity, aentity);
                if (pushdata != null)
                {
                    if (pushdata.Success == 1 && !string.IsNullOrEmpty(pushdata.SendCode))
                    {
                        pushdata.Content = getName(entity.WorkType, "CommonType");
                        if (pushdata.SendCode == "ZY003")
                        {
                            pushdata.Content = "���ύ��" + pushdata.Content + "����δͨ�����뼰ʱ����";
                            var high = GetEntity(entity.Id);
                            if (high != null)
                            {
                                UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, pushdata.SendCode, "", pushdata.Content, pushdata.EntityId);
                                }
                            }
                        }
                        else if (pushdata.SendCode == "ZY018")
                        {
                            var high = GetEntity(entity.Id);
                            if (high != null)
                            {
                                String CommonType = pushdata.Content;
                                pushdata.Content = "��ҵ���ݣ�" + high.WorkContent + "&#10;��ҵʱ�䣺" + high.WorkStartTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + " �� " + high.WorkEndTime.Value.ToString("yyyy��MM��dd�� HHʱmm��") + "&#10;��ҵ�ص㣺" + high.WorkPlace;
                                UserEntity userEntity = userservice.GetEntity(high.CreateUserId);
                                UserEntity tutelageuserEntity = userservice.GetEntity(high.WorkTutelageUserId);
                                string[] workuserlist = (high.WorkDutyUserId + "," + high.WorkUserIds).Split(',');
                                //List<string> b = workuserlist.ToList();
                                //b.Add(high.WorkDutyUserId);
                                //workuserlist = b.ToArray();
                                DataTable dutyuserDt = new DataTable();
                                dutyuserDt = userservice.GetUserTable(workuserlist);
                                //���͸���ҵ������
                                if (userEntity != null)
                                {
                                    JPushApi.PushMessage(userEntity.Account, userEntity.RealName, pushdata.SendCode, "�߷�����ҵ(" + CommonType + "��������ͨ�����뼰ʱ����", pushdata.Content, pushdata.EntityId);
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
                                    JPushApi.PushMessage(Account, RealName, pushdata.SendCode, "����һ���µĸ߷�����ҵ(" + CommonType + ")�����뼰ʱ����", pushdata.Content, pushdata.EntityId);
                                }
                                //���͸���ҵ�໤��
                                if (tutelageuserEntity != null)
                                {
                                    JPushApi.PushMessage(tutelageuserEntity.Account, tutelageuserEntity.RealName, pushdata.SendCode, "����һ���µĸ߷�����ҵ(" + CommonType + ")�໤�����뼰ʱ����", pushdata.Content, pushdata.EntityId);
                                }
                            }
                        }
                        else
                        {
                            //��������
                            PushMessageForCommon(pushdata);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        public string getName(string type, string encode)
        {
            var cName = new DataItemDetailBLL().GetDataItemListByItemCode("'" + encode + "'").Where(a => a.ItemValue == type).FirstOrDefault().ItemName;
            return cName;
        }


        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="nextActivityName"></param>
        /// <param name="control"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public void PushMessageForCommon(PushMessageData pushdata)
        {
            if (!string.IsNullOrEmpty(pushdata.UserAccount))
            {
                DataTable dtuser = userservice.GetUserTable(pushdata.UserAccount.Split(','));
                string[] usernames = dtuser.AsEnumerable().Select(d => d.Field<string>("realname")).ToArray();
                if (pushdata.SendCode == "ZY001")
                {
                    pushdata.Content = "����һ��" + pushdata.Content + "�������ȫ��ʩȷ�ϣ��뼰ʱ����";
                }
                else if (pushdata.SendCode == "ZY002")
                {
                    pushdata.Content = "����һ��" + pushdata.Content + "������������뼰ʱ����";
                }
                else
                {
                    pushdata.Content = "";
                }
                JPushApi.PushMessage(pushdata.UserAccount, string.Join(",", usernames), pushdata.SendCode, "", pushdata.Content, pushdata.EntityId);
            }
            else
            {
                string flowdeptids = "'" + pushdata.UserDept.Replace(",", "','") + "'";
                string flowroleids = "'" + pushdata.UserRole.Replace(",", "','") + "'";
                IList<UserEntity> users = new UserService().GetUserListByDeptId(flowdeptids, flowroleids, true, string.Empty);
                if (users != null && users.Count > 0)
                {
                    string names = "";
                    string accounts = "";
                    if (!string.IsNullOrEmpty(pushdata.SpecialtyType) && !string.IsNullOrEmpty(pushdata.IsSpecial) && pushdata.IsSpecial == "1")
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
                                        if (str[i] == pushdata.SpecialtyType)
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
                    if (pushdata.SendCode == "ZY001")
                    {
                        pushdata.Content = "����һ��" + pushdata.Content + "�������ȫ��ʩȷ�ϣ��뼰ʱ����";
                    }
                    else if (pushdata.SendCode == "ZY002")
                    {
                        pushdata.Content = "����һ��" + pushdata.Content + "������������뼰ʱ����";
                    }
                    else
                    {
                        pushdata.Content = "";
                    }
                    JPushApi.PushMessage(accounts, names, pushdata.SendCode, "", pushdata.Content, pushdata.EntityId);
                }
            }

        }

        #region ͳ��
        /// <summary>
        /// ����ҵ����ͳ��
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkCount(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetHighWorkCount(starttime, endtime, deptid, deptcode);
        }


        /// <summary>
        ///��ҵ����ͳ��(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        public string GetHighWorkList(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.GetHighWorkList(starttime, endtime, deptid, deptcode);
        }

        /// <summary>
        /// �¶�����(ͳ��ͼ)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearCount(string year, string deptid, string deptcode)
        {
            return service.GetHighWorkYearCount(year, deptid, deptcode);
        }

        /// <summary>
        /// �¶�����(���)
        /// </summary>
        /// <param name="year"></param>
        /// <param name="deptid"></param>
        /// <param name="deptcode"></param>
        /// <returns></returns>
        public string GetHighWorkYearList(string year, string deptid, string deptcode)
        {
            return service.GetHighWorkYearList(year, deptid, deptcode);
        }

        /// <summary>
        /// ��λ�Ա�(ͳ��ͼ)
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public string GetHighWorkDepartCount(string starttime, string endtime)
        {
            return service.GetHighWorkDepartCount(starttime, endtime);
        }

        /// <summary>
        ///��λ�Ա�(ͳ�Ʊ��)
        /// </summary>
        /// <returns></returns>
        public string GetHighWorkDepartList(string starttime, string endtime)
        {
            return service.GetHighWorkDepartList(starttime, endtime);
        }
        #endregion


        #region ��ȡ���ո߷�����ҵ
        /// <summary>
        /// ��ȡ���ո߷�����ҵ(��ҵ̨������ҵ�е�����)
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public DataTable GetTodayWorkList(Pagination pagination, string queryJson)
        {
            return service.GetTodayWorkList(pagination, queryJson);
        }
        #endregion

        #region �ֻ��˸߷�����ҵͳ��
        public DataTable AppGetHighWork(string starttime, string endtime, string deptid, string deptcode)
        {
            return service.AppGetHighWork(starttime, endtime, deptid, deptcode);
        }
        #endregion
        #region
        public bool GetProjectNum(string outProjectId)
        {
            return service.GetProjectNum(outProjectId);
        }

        /// <summary>
        /// ������������ȡ�߷�����ҵ������
        /// </summary>
        /// <param name="areaCodes"></param>
        /// <returns></returns>
        public DataTable GetCountByArea(List<string> areaCodes)
        {
            return service.GetCountByArea(areaCodes);
        }
        #endregion
    }
}
