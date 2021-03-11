using ERCHTMS.Entity.CarManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;

namespace ERCHTMS.IService.CarManage
{
    /// <summary>
    /// �� ����Σ��Ʒ���������Ŀ��
    /// </summary>
    public interface CarcheckitemdetailIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<CarcheckitemdetailEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        CarcheckitemdetailEntity GetEntity(string keyValue);
        #endregion

        #region �ύ����

        /// <summary>
        /// �޸������б��ύ״̬
        /// </summary>
        /// <param name="detaillist"></param>
        void Update(List<CarcheckitemdetailEntity> detaillist);
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
        void SaveForm(string keyValue, CarcheckitemdetailEntity entity);
        #endregion
    }
}
