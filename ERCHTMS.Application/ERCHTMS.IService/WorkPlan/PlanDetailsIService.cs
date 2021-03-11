using ERCHTMS.Entity.WorkPlan;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.WorkPlan
{
    /// <summary>
    /// �� ���������ƻ�����
    /// </summary>
    public interface PlanDetailsIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<PlanDetailsEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡ��ҳ�б�
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        PlanDetailsEntity GetEntity(string keyValue);
        /// <summary>
        /// ���±��״̬
        /// </summary>
        /// <param name="applyId"></param>
        void UpdateChangedData(string applyId);
        #endregion

        #region ͳ��
        /// <summary>
        /// ͳ�ƹ����ƻ�
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        DataTable Statistics(string deptId, string starttime, string endtime, string applytype = "");
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        void RemoveForm(string keyValue);
        void RemoveFormByApplyId(string keyValue);        
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, PlanDetailsEntity entity);
        #endregion
    }
}
