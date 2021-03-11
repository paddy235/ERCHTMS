using ERCHTMS.Entity.AccidentEvent;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.AccidentEvent
{
    /// <summary>
    /// �� ����δ���¼���������鴦��
    /// </summary>
    public interface IWssjbg_dealService
    {
        #region ��ȡ����
        IEnumerable<Wssjbg_dealEntity> GetListForCon(Expression<Func<Wssjbg_dealEntity, bool>> condition);

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<Wssjbg_dealEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        Wssjbg_dealEntity GetEntity(string keyValue);
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
        void SaveForm(string keyValue, Wssjbg_dealEntity entity);
        #endregion
    }
}
