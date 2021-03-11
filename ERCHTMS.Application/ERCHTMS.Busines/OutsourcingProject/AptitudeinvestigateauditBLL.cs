using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.BaseManage;
using System.Threading.Tasks;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// �� �������������˱�
    /// </summary>
    public class AptitudeinvestigateauditBLL
    {
        private AptitudeinvestigateauditIService service = new AptitudeinvestigateauditService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<AptitudeinvestigateauditEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AptitudeinvestigateauditEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }

        public AptitudeinvestigateauditEntity GetAuditEntity(string FKId)
        {
            return service.GetAuditEntity(FKId);
        }

        public List<AptitudeinvestigateauditEntity> GetAuditList(string keyValue) 
        {
            return service.GetAuditList(keyValue);
        }

        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
          /// <summary>
        /// ��ȡҵ�����������˼�¼
        /// </summary>
        /// <param name="recId">ҵ���¼Id</param>
        /// <returns></returns>
        public DataTable GetAuditRecList(string recId)
        {
            return service.GetAuditRecList(recId);
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
        public void SaveForm(string keyValue, AptitudeinvestigateauditEntity entity)
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
        /// ����������ͨ��ͬ�� ���� ��λ ��Ա��Ϣ
        /// �޸������λ�볡״̬,�޸����̱��������״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>

        public void SaveSynchrodata(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.SaveSynchrodata(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// ��֤�����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveSafetyEamestMoney(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.SaveSafetyEamestMoney(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ��������������޸ģ�
        /// �����������:���¹���״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void AuditReturnForWork(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.AuditReturnForWork(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
         /// <summary>
        /// ��������������޸ģ�
        /// �����������:���¹���״̬
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void AuditStartApply(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                service.AuditStartApply(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AuditPeopleReview(string keyValue, AptitudeinvestigateauditEntity entity)
        {
            try
            {
                List<string> userids = service.AuditPeopleReview(keyValue, entity);
                //�Խ���ѵƽ̨
                if (userids.Count > 0)
                {
                    try
                    {
                        string way = new DataItemDetailBLL().GetItemValue("WhatWay");
                        Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                        DepartmentEntity org = new DepartmentBLL().GetEntity(user.OrganizeId);
                        foreach (var item in userids)
                        {
                            var userInfo = new UserBLL().GetUserInfoEntity(item);
                            var userEntity = new UserBLL().GetEntity(item);
                            if (org.IsTrain == 1)
                            {
                                //�Խ�.net��ѵƽ̨
                                if (way == "0")
                                {

                                }
                                //�Խ�java��ѵƽ̨
                                if (way == "1")
                                {
                                    DepartmentEntity dept = new DepartmentBLL().GetEntity(userInfo.DepartmentId);
                                    if (dept != null)
                                    {
                                        string deptId = dept.DepartmentId;
                                        string enCode = dept.EnCode;
                                        if (!string.IsNullOrWhiteSpace(dept.DeptKey))
                                        {
                                            string[] arr = dept.DeptKey.Split('|');
                                            deptId = arr[0];
                                            if (arr.Length > 1)
                                            {
                                                enCode = arr[1];
                                            }
                                        }
                                        Task.Run(() =>
                                        {
                                            object obj = new
                                            {
                                                action = "add",
                                                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                                userId = userInfo.UserId,
                                                userName = userInfo.RealName,
                                                account = userInfo.Account,
                                                deptId = deptId,
                                                deptCode = enCode,
                                                password = "Abc123456", //Ϊnullʱ��Ҫ�޸�����!
                                                sex = userInfo.Gender,
                                                idCard = userInfo.IdentifyID,
                                                email = userInfo.Email,
                                                mobile = userInfo.Mobile,
                                                birth = userInfo.Birthday == null ? "" : userInfo.Birthday.Value.ToString("yyyy-MM-dd"),//����
                                                postName = userInfo.DutyName,//��λ
                                                age = userInfo.Age,//����
                                                native = userInfo.Native, //����
                                                nation = userInfo.Nation, //����
                                                encode = userInfo.EnCode,//����
                                                companyId = org.InnerPhone,
                                                role = userInfo.IsTrainAdmin == null ? 0 : userInfo.IsTrainAdmin, //��ɫ��0:ѧԱ��1:��ѵ����Ա��
                                                postId = userEntity.DutyId,
                                                jobTitle = userEntity.JobTitle,
                                                techLevel = userEntity.TechnicalGrade,
                                                workType = userEntity.Craft,
                                                trainRoles = userEntity.TrainRoleId
                                            };
                                            List<object> list = new List<object>();
                                            list.Add(obj);
                                            Busines.JPush.JPushApi.PushMessage(list, 1);
                                        });
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        #endregion
    }
}
