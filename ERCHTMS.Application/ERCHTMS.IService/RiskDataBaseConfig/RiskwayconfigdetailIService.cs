using ERCHTMS.Entity.RiskDataBaseConfig;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.RiskDataBaseConfig
{
    /// <summary>
    /// �� ������ȫ���չܿ�ȡֵ���������
    /// </summary>
    public interface RiskwayconfigdetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<RiskwayconfigdetailEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        RiskwayconfigdetailEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, RiskwayconfigdetailEntity entity);
        #endregion
    }
}
