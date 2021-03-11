using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� �����ල��������Ŀ
    /// </summary>
    public interface SideCheckProjectIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SideCheckProjectEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SideCheckProjectEntity GetEntity(string keyValue);

        /// <summary>
        /// ��������Ŀ��Ϣ
        /// </summary>
        /// <param name="approveid"></param>
        /// <returns></returns>
        IEnumerable<SideCheckProjectEntity> GetBigCheckInfo();


        /// <summary>
        /// ���ݴ�������Ŀid��ȡС������Ŀ
        /// </summary>
        /// <param name="parentid"></param>
        /// <returns></returns>
        IEnumerable<SideCheckProjectEntity> GetAllSmallCheckInfo(string parentid);

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
        void SaveForm(string keyValue, SideCheckProjectEntity entity);
        #endregion
    }
}
