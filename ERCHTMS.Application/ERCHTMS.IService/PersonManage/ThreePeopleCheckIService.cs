using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// �� ��������������ҵ���
    /// </summary>
    public interface ThreePeopleCheckIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<ThreePeopleCheckEntity> GetList(string queryJson);
        DataTable GetItemList(string id);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        ThreePeopleCheckEntity GetEntity(string keyValue);

        DataTable GetPageList(Pagination pagination, string queryJson);

         /// <summary>
        /// ��ȡ��Ա��Ϣ
        /// </summary>
        /// <param name="applyId">�����Id</param>
        /// <returns></returns>
        IEnumerable<ThreePeopleInfoEntity> GetUserList(string applyId);
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
        void SaveForm(string keyValue, ThreePeopleCheckEntity entity);
        bool SaveForm(string keyValue, ThreePeopleCheckEntity entity, List<ThreePeopleInfoEntity> list, AptitudeinvestigateauditEntity auditInfo = null);

        #endregion

         /// <summary>
        /// ��ǰ��¼���Ƿ���Ȩ����˲���ȡ��һ�����Ȩ��ʵ��
        /// </summary>
        /// <param name="currUser">��ǰ��¼��</param>
        /// <param name="state">�Ƿ���Ȩ����� 1������� 0 ���������</param>
        /// <param name="moduleName">ģ������</param>
        /// <param name="createdeptid">�����˲���ID</param>
        /// <returns>null-��ǰΪ���һ�����,ManyPowerCheckEntity����һ�����Ȩ��ʵ��</returns>
       ManyPowerCheckEntity CheckAuditPower(Operator currUser, out string state, string moduleName, string createdeptid,string applyId="");


    }
}
