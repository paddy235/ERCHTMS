using ERCHTMS.Entity.SystemManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.SystemManage
{
    /// <summary>
    /// �� ��������糧�ύ����˵����
    /// </summary>
    public interface OutcommitfileIService
    {
        #region ��ȡ����
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<OutcommitfileEntity> GetList();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        OutcommitfileEntity GetEntity(string keyValue);
        /// <summary>
        /// ���ݻ���Code��ѯ�������Ƿ��Ѿ����
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        bool GetIsExist(string orgCode);
        OutcommitfileEntity GetEntityByOrgCode(string orgCode);
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
        void SaveForm(string keyValue, OutcommitfileEntity entity);
        #endregion
    }
}
