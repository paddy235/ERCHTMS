using ERCHTMS.Entity.WorkPlan;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;

namespace ERCHTMS.IService.WorkPlan
{
    /// <summary>
    /// �� ����EHS�ƻ������
    /// </summary>
    public interface PlanApplyIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<PlanApplyEntity> GetList(string queryJson);        
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
        PlanApplyEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡ�������ڵ�
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        DataTable GetWorkDetailList(string objectId);

        int GetPlanApplyBMNum(ERCHTMS.Code.Operator user);
        /// <summary>
        /// ��ȡ����˸��˹����ƻ�
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetPlanApplyGRNum(ERCHTMS.Code.Operator user);
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
        void SaveForm(string keyValue, PlanApplyEntity entity);
        #endregion
    }
}
