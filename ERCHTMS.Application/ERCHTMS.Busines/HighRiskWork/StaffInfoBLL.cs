using ERCHTMS.Entity.HighRiskWork;
using ERCHTMS.IService.HighRiskWork;
using ERCHTMS.Service.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Busines.SystemManage;
using System.Net;

namespace ERCHTMS.Busines.HighRiskWork
{
    /// <summary>
    /// �� �������������Ա
    /// </summary>
    public class StaffInfoBLL
    {
        private StaffInfoIService service = new StaffInfoService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StaffInfoEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StaffInfoEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        /// <summary>
        /// ��ȡ�ල�����б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetDataTable(Pagination page, string queryJson)
        {
            return service.GetDataTable(page, queryJson);
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
        public string SaveForm(string keyValue, StaffInfoEntity model)
        {
            try
            {

                keyValue = service.SaveForm(keyValue, model);
                return keyValue;
            }
            catch (Exception ex)
            {
                return "0";
            }
        }


        #region  �ԽӰ���(��ʱִ��)
        public void SendTaskInfo()
        {
            try
            {
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                var tasklist = new TeamsInfoBLL().GetAllList(string.Format(" and dataissubmit='1' and teamstarttime<=to_date('{1}','yyyy-mm-dd hh24:mi:ss') and  teamendtime>=to_date('{0}','yyyy-mm-dd hh24:mi:ss') and (isaccomplish!='1' or isaccomplish is null)", DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), DateTime.Now.ToString("yyyy-MM-dd 23:59:59")));
                List<object> datas = new List<object>();
                foreach (TeamsInfoEntity item in tasklist)
                {
                    string[] arr = item.WorkInfoId.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        var workentiy = new SuperviseWorkInfoBLL().GetEntity(arr[i]);
                        StaffInfoEntity staff = new StaffInfoEntity();
                        staff.Id = Guid.NewGuid().ToString();
                        staff.PTeamName = item.TeamName;
                        staff.PTeamCode = item.TeamCode;
                        staff.PTeamId = item.TeamId;
                        staff.PStartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                        staff.PEndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                        staff.SumTimeStr = 0;
                        staff.TaskLevel = "1";
                        staff.DataIsSubmit = "0";
                        staff.SuperviseState = "0";//��������ල���Ϊ1(�Ѽල)
                        staff.TaskShareId = item.TaskShareId;
                        staff.IsSynchronization = "1";//ͬ������
                        staff.WorkInfoId = workentiy.Id;
                        staff.WorkInfoName = workentiy.WorkName;
                        staff.CreateUserId = item.CreateUserId;
                        staff.CreateUserName = item.CreateUserName;
                        staff.CreateUserDeptCode = item.CreateUserDeptCode;
                        staff.CreateUserOrgCode = item.CreateUserOrgCode;
                        string result = new StaffInfoBLL().SaveForm("", staff);
                        if (result != "0")
                        {
                            var tempdata = new
                            {
                                Job = workentiy.WorkName + "��վ�ල����",
                                StartTime = staff.PStartTime,
                                EndTime = staff.PEndTime,
                                JobProject1 = string.IsNullOrEmpty(workentiy.WorkContent) ? "" : workentiy.WorkContent,//��ҵ����
                                JobDept = string.IsNullOrEmpty(workentiy.WorkDeptName) ? "" : workentiy.WorkDeptName,//��ҵ��λ
                                JobCategory = string.IsNullOrEmpty(workentiy.WorkInfoType) ? "" : workentiy.WorkInfoType,//��ҵ����
                                JobProject2 = string.IsNullOrEmpty(workentiy.EngineeringName) ? "" : workentiy.EngineeringName,//��������
                                JobNo = string.IsNullOrEmpty(workentiy.WorkTicketNo) ? "" : workentiy.WorkTicketNo,//����Ʊ��
                                JobAddr = string.IsNullOrEmpty(workentiy.WorkPlace) ? "" : workentiy.WorkPlace,//��ҵ�ص�
                                RecId = staff.Id,//����id
                                GroupId = item.TeamId //����id
                            };
                            datas.Add(tempdata);
                        }
                        else
                        {
                            System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "/logs/" + fileName, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "���쳣����Ϊ:" + Newtonsoft.Json.JsonConvert.SerializeObject(staff) + "\r\n");
                        }

                    }
                }
                WebClient wc = new WebClient();
                wc.Credentials = CredentialCache.DefaultCredentials;
                //��������web api����ȡ����ֵ��Ĭ��Ϊpost��ʽ
                System.Collections.Specialized.NameValueCollection nc = new System.Collections.Specialized.NameValueCollection();
                nc.Add("json", Newtonsoft.Json.JsonConvert.SerializeObject(datas));
                // wc.UploadValuesCompleted += wc_UploadValuesCompleted1;
                System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "/logs/" + fileName, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "��ͬ���ɹ�,����Ϊ:" + Newtonsoft.Json.JsonConvert.SerializeObject(datas) + "\r\n");

                wc.UploadValuesAsync(new Uri(new DataItemDetailBLL().GetItemValue("bzurl") + "PostMonitorJob"), nc);
            }
            catch (Exception ex)
            {
                //��ͬ�����д����־�ļ�
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
                System.IO.File.AppendAllText(new DataItemDetailBLL().GetItemValue("imgPath") + "/logs/" + fileName, "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "������ʧ��" + ",�쳣��Ϣ��" + ex.Message + "\r\n");
            }
        }
        #endregion
        #endregion
    }
}
