using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּ���˼�¼��
    /// </summary>
    public interface ScaffoldauditrecordIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">��ѯ����</param>
        /// <returns>�����б�</returns>
        List<ScaffoldauditrecordEntity> GetList(string scaffoldid);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        ScaffoldauditrecordEntity GetEntity(string keyValue);


        List<ScaffoldauditrecordEntity> GetApplyAuditList(string keyValue, int AuditType);

          /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">���ּ���ϢID</param>
        /// <param name="departname">������</param>
        /// <param name="rolename">��ɫ��</param>
        /// <returns></returns>
        ScaffoldauditrecordEntity GetEntity(string scaffoldid, string departname, string rolename);

          /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="scaffoldid">���ּ���ϢID</param>
        /// <param name="deppartcode">����</param>
        /// <returns></returns>
        IEnumerable<ScaffoldauditrecordEntity> GetEntitys(string scaffoldid, string departcode);
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
        void SaveForm(string keyValue, ScaffoldauditrecordEntity entity);
        #endregion
    }
}
