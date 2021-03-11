using ERCHTMS.Entity.LaborProtectionManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.LaborProtectionManage
{
    /// <summary>
    /// �� �����Ͷ��������ű�����
    /// </summary>
    public interface LaborissuedetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<LaborissuedetailEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        LaborissuedetailEntity GetEntity(string keyValue);

        /// <summary>
        /// ������Ʒ��id��ȡ���һ�η��ż�¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        LaborissuedetailEntity GetOrderLabor(string keyValue);
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
        void SaveListForm(string json);
        
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, LaborissuedetailEntity entity, string json, string InfoId);
        #endregion
    }
}
