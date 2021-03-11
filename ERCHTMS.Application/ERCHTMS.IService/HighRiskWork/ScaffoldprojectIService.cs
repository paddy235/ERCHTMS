using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּ�������Ŀ
    /// </summary>
    public interface ScaffoldprojectIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">��ѯ����</param>
        /// <returns>�����б�</returns>
        List<ScaffoldprojectEntity> GetList(string scaffoldid);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        ScaffoldprojectEntity GetEntity(string keyValue);


         /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<ScaffoldprojectEntity> GetListByCondition(string queryJson);
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
        void SaveForm(string keyValue, ScaffoldprojectEntity entity);

        /// <summary>
        /// �����������ʽɾ��
        /// </summary>
        /// <param name="condition"></param>
        void RemoveForm(Expression<Func<ScaffoldprojectEntity, bool>> condition);
        #endregion
    }
}
