using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� �������������
    /// </summary>
    public interface StartapplyforIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<StartapplyforEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        StartapplyforEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        StartapplyforEntity GetApplyReturnTime(string outProjectId, string outEngId);

         /// <summary>
        /// ��ȡ������������Ŀ������
        /// </summary>
        /// <param name="projectId">����Id</param>
        /// <returns></returns>
        DataTable GetStartWorkStatus(string projectId);
                /// <summary>
        /// ��ȡ����ʩ���ֳ������˺Ͱ�ȫԱ��Ϣ
        /// </summary>
        /// <param name="projectId">����Id</param>
        /// <returns></returns>
        List<string> GetSafetyUserInfo(string projectId);
        DataTable GetApplyInfo(string keyValue);
           /// <summary>
        /// ��ȡ���̺�ͬ���
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
       object GetContractSno(string projectId);
        /// <summary>
        /// �жϵ�ǰ�û��Ƿ������Ȩ��
        /// </summary>
        /// <param name="nodeId">�ڵ�Id</param>
        /// <param name="user">��ǰ�û�</param>
        /// <param name="projectId">����Id</param>
        /// <returns></returns>
        bool HasCheckPower(string nodeId, ERCHTMS.Code.Operator user, string projectId);

        List<string> GetSafetyUserInfo(string projectId, string roletype, string deptid);
        DataTable GetStartForItem(string keyValue);
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        bool SaveForm(string keyValue, StartapplyforEntity entity);
        #endregion
    }
}
