using ERCHTMS.Entity.OutsourcingProject;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.HiddenTroubleManage;

namespace ERCHTMS.IService.OutsourcingProject
{
    /// <summary>
    /// �� �������������Ϣ��
    /// </summary>
    public interface OutsouringengineerIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>���ط�ҳ�б�</returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ҳ��ת
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetIndexToList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<OutsouringengineerEntity> GetList();
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<OutsouringengineerEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        OutsouringengineerEntity GetEntity(string keyValue);
        /// <summary>
        /// ���ݵ�¼��id ��ѯ�Ѿ��ڽ��Ĺ���(�Ѿ�ͨ����������Ĺ���)
        /// </summary>
        /// <returns></returns>
        DataTable GetOnTheStock(Operator currUser);
        /// <summary>
        /// ���������λId��ȡ�������
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        DataTable GetEngineerDataByWBId(string deptId, string mode = "");
        /// <summary>
        /// ���ݵ�ǰ��¼��Id��ȡ�������
        /// </summary>
        /// <param name="outProjectId"></param>
        /// <returns></returns>
        DataTable GetEngineerDataByCurrdeptId(Operator currUser, string mode = "", string orgid = "");

        DataTable GetEngineerDataByCondition(Operator currUser, string mode = "", string orgid = "");

        /// <summary>
        /// ���ݵ�ǰ��¼�� ��ȡ�Ѿ�ͣ���Ĺ�����Ϣ
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        DataTable GetStopEngineerList();
        /// <summary>
        /// ���ݵ�ǰ��¼�� ��ȡ�����б�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        DataTable GetEngineerByCurrDept();
        /// <summary>
        /// ��ȡ���̵�����״̬ͼ
        /// </summary>
        /// <param name="keyValue">����Id</param>
        /// <returns></returns>
        Flow GetProjectFlow(string keyValue);
        string GetTypeCount(string deptid, string year = "", string type = "001,002");
        string GetTypeList(string deptid, string year = "", string type = "001,002");
        string GetStateCount(string deptid, string year = "", string state = "001,002,003");
        string GetStateList(string deptid, string year = "", string state = "001,002,003");
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
        void SaveForm(string keyValue, OutsouringengineerEntity entity);
        bool ProIsOver(string keyValue);
        #endregion
    }
}
