using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.IService.OutsourcingProject;
using ERCHTMS.Service.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;

namespace ERCHTMS.Busines.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ա��
    /// </summary>
    public class AptitudeinvestigatepeopleBLL
    {
        private AptitudeinvestigatepeopleIService service = new AptitudeinvestigatepeopleService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<AptitudeinvestigatepeopleEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }


        public IEnumerable<AptitudeinvestigatepeopleEntity> GetPersonInfo(string projectid, string pageindex, string pagesize) {
            return service.GetPersonInfo(projectid, pageindex, pagesize);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public AptitudeinvestigatepeopleEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// �����û�Id��ȡ���û����ڹ����Ƿ�ͨ����Ա�������
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool IsAuditByUserId(string userid) {
            return service.IsAuditByUserId(userid);
        }
        #endregion

        #region �ύ����
        public bool ExistIdentifyID(string IdentifyID, string keyValue)
        {
            return service.ExistIdentifyID(IdentifyID,keyValue);
        }

        public bool ExistAccount(string Account, string keyValue)
        {
            return service.ExistAccount(Account, keyValue);
        }
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
        public void SaveForm(string keyValue, AptitudeinvestigatepeopleEntity entity)
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
        /// �������������Ա�������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void SummitPhyInfo(string keyValue, PhyInfoEntity entity) {
            try
            {
                service.SummitPhyInfo(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
