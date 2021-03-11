using ERCHTMS.Entity.PersonManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.PersonManage
{
    /// <summary>
    /// �� ����ת����Ϣ��
    /// </summary>
    public interface TransferIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<TransferEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TransferEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TransferEntity GetUsertraEntity(string keyValue);

        /// <summary>
        /// ��ȡ��ǰ�û�����ת�ڴ�������
        /// </summary>
        /// <returns></returns>
        int GetTransferNum();

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetTransferList(Pagination pagination, string queryJson);

        /// <summary>
        /// ���ݵ�ǰ����id��ȡ�㼶��ʾ����
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        string GetDeptName(string deptid);
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
        void SaveForm(string keyValue, TransferEntity entity);

        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void AppSaveForm(string keyValue, TransferEntity entity, string Userid);

        ///// <summary>
        ///// ת��ȷ�ϲ���
        ///// </summary>
        ///// <param name="keyValue"></param>
        ///// <param name="entity"></param>
        //void Update(string keyValue, TransferEntity entity);

        #endregion
    }
}
