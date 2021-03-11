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
    /// �� �������������
    /// </summary>
    public class StartapplyforBLL
    {
        private StartapplyforIService service = new StartapplyforService();

        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<StartapplyforEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public StartapplyforEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        public StartapplyforEntity GetApplyReturnTime(string outProjectId, string outEngId)
        {
            return service.GetApplyReturnTime(outProjectId, outEngId);
        }
        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        public DataTable GetPageList(Pagination pagination, string queryJson) {
            return service.GetPageList(pagination, queryJson);
        }

         /// <summary>
        /// ��ȡ������������Ŀ������
        /// </summary>
        /// <param name="projectId">����Id</param>
        /// <returns></returns>
        public DataTable GetStartWorkStatus(string projectId)
        {
            return service.GetStartWorkStatus(projectId);
        }
                /// <summary>
        /// ��ȡ����ʩ���ֳ������˺Ͱ�ȫԱ��Ϣ
        /// </summary>
        /// <param name="projectId">����Id</param>
        /// <returns></returns>
        public List<string> GetSafetyUserInfo(string projectId)
        {
            return service.GetSafetyUserInfo(projectId);
        }
        public List<string> GetSafetyUserInfo(string projectId, string roletype, string deptid)
        {
            return service.GetSafetyUserInfo(projectId, roletype, deptid);
        }
         public DataTable GetApplyInfo(string keyValue)
        {
            return service.GetApplyInfo(keyValue);
        }
           /// <summary>
        /// ��ȡ���̺�ͬ���
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public object GetContractSno(string projectId)
         {
             return service.GetContractSno(projectId);
         }


        public DataTable GetStartForItem(string keyValue) {
            return service.GetStartForItem(keyValue);
        }
        /// <summary>
        /// �жϵ�ǰ�û��Ƿ������Ȩ��
        /// </summary>
        /// <param name="nodeId">�ڵ�Id</param>
        /// <param name="user">��ǰ�û�</param>
        /// <param name="projectId">����Id</param>
        /// <returns></returns>
        public bool HasCheckPower(string nodeId,ERCHTMS.Code.Operator user,string projectId)
        {
            return service.HasCheckPower(nodeId, user, projectId);
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
        public bool SaveForm(string keyValue, StartapplyforEntity entity)
        {
            try
            {
                return service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
