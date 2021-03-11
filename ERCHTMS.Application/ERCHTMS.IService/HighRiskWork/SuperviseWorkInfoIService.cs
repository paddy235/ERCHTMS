using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� ������վ�ල��ҵ��Ϣ
    /// </summary>
    public interface SuperviseWorkInfoIService
    {
        #region ��ȡ����
        /// <summary>
        /// ���ݼල�����ȡ��ҵ��Ϣ
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<SuperviseWorkInfoEntity> GetList(string strwhere);
         /// <summary>
        /// ���ݷ�������id�Ͱ���id��ȡ��ҵ��Ϣ
        /// </summary>
        /// <param name="taskshareid"></param>
        /// <returns></returns>
        IEnumerable<SuperviseWorkInfoEntity> GetList(string taskshareid, string teamid);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        SuperviseWorkInfoEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, SuperviseWorkInfoEntity entity);

        /// <summary>
        ///���ݷ���idɾ����ҵ��Ϣ
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveWorkByTaskShareId(string keyValue);
        #endregion
    }
}
