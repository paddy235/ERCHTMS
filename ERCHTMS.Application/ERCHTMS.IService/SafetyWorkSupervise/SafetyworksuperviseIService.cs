using ERCHTMS.Entity.SafetyWorkSupervise;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.SafetyWorkSupervise
{
    /// <summary>
    /// �� ������ȫ�ص㹤������
    /// </summary>
    public interface SafetyworksuperviseIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SafetyworksuperviseEntity> GetList(string queryJson);

        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SafetyworksuperviseEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡʵ��/�����
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        DataTable GetEntityByT(string keyValue, string fid);
       /// <summary>
       /// ������Ϣ
       /// </summary>
       /// <param name="keyValue"></param>
       /// <param name="modulename"></param>
       /// <returns></returns>
        Flow GetFlow(string keyValue);
        int GetSuperviseNum(string userid, string type);
        /// <summary>
        /// ��ȡ��ҳͳ��ͼ
        /// </summary>
        /// <param name="keyValue">1��ʾ�ϸ���</param>
        /// <returns></returns>
        DataTable GetSuperviseStat(string keyValue);
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
        void SaveForm(string keyValue, SafetyworksuperviseEntity entity);
        #endregion
    }
}
