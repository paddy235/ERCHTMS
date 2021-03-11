using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ա��
    /// </summary>
    public interface AptitudeinvestigatepeopleIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<AptitudeinvestigatepeopleEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        AptitudeinvestigatepeopleEntity GetEntity(string keyValue);

        IEnumerable<AptitudeinvestigatepeopleEntity> GetPersonInfo(string projectid, string pageindex, string pagesize);
        
        bool IsAuditByUserId(string userid);
        DataTable GetPageList(Pagination pagination, string queryJson);
        #endregion

        #region �ύ����
        bool ExistIdentifyID(string IdentifyID, string keyValue);

        bool ExistAccount(string Account, string keyValue);
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
        void SaveForm(string keyValue, AptitudeinvestigatepeopleEntity entity);
        /// <summary>
        /// �������������Ա�������Ϣ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        void SummitPhyInfo(string keyValue, PhyInfoEntity entity);
        #endregion
    }
}
