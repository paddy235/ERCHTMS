using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Code;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� ������ȫ��������
    /// </summary>
    public interface TechDisclosureIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetList(Pagination pagination, string queryJson);
        IEnumerable<TechDisclosureEntity> GetList();
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        TechDisclosureEntity GetEntity(string keyValue);
        DataTable GetNameByPorjectId(string projectId, string type);
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
        void SaveForm(string keyValue, TechDisclosureEntity entity);
        /// <summary>
        /// ��˱�
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <param name="aentity"></param>
        void ApporveForm(string keyValue, TechDisclosureEntity entity, AptitudeinvestigateauditEntity aentity);
        #endregion
    }
}
