using ERCHTMS.Entity.StandardSystem;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.StandardSystem
{
    /// <summary>
    /// �� ������׼����
    /// </summary>
    public interface StcategoryIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<StcategoryEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        StcategoryEntity GetEntity(string keyValue);
        /// <summary>
        /// �жϴ˽ڵ����Ƿ����ӽڵ�
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        bool IsHasChild(string parentId);

        /// <summary>
        /// �Ϲ�������-��ȡ����
        /// </summary>
        /// <returns></returns>
        IEnumerable<StcategoryEntity> GetCategoryList();
        IEnumerable<StcategoryEntity> GetRankList(string Category);
        StcategoryEntity GetQueryEntity(string queryJson);
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
        void SaveForm(string keyValue, StcategoryEntity entity);
        #endregion
    }
}
