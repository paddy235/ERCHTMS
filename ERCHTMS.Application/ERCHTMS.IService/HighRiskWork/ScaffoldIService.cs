using ERCHTMS.Entity.HighRiskWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HighRiskWork.ViewModel;
using System.Data;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Code;

namespace ERCHTMS.IService.HighRiskWork
{
    /// <summary>
    /// �� ����1.���ּܴ��衢���ա��������2.���ּܴ��衢���ա��������
    /// </summary>
    public interface ScaffoldIService
    {
        #region ��ȡ����

        /// <summary>
        /// �õ���ǰ�����
        /// </summary>
        /// <returns></returns>
        string GetMaxCode();

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        DataTable GetList(Pagination page, string queryJson);
        /// <summary>
        /// ̨���б�
        /// </summary>
        /// <param name="page"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetLedgerList(Pagination page, string queryJson, string authType);


        /// <summary>
        /// �õ�����ͼ
        /// </summary>
        /// <param name="keyValue">ҵ���ID</param>
        /// <param name="modulename">�����ģ����</param>
        /// <returns></returns>
        Flow GetFlow(string keyValue, string modulename);

        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        ScaffoldEntity GetEntity(string keyValue);

        /// <summary>
        /// ��ȡѡ����ּܴ���Ͳ��
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetSelectPageList(Pagination pagination, string queryJson);


        /// <summary>
        /// ��ȡ��Ա
        /// </summary>
        /// <param name="flowdeptid"></param>
        /// <param name="flowrolename"></param>
        /// <param name="type"></param>
        string GetUserName(string flowdeptid, string flowrolename, string type = "", string specialtytype = "");

        List<CheckFlowData> GetAppFlowList(string keyValue, string modulename);
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
        void SaveForm(string keyValue, ScaffoldEntity entity);


        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="model">ʵ�����</param>
        /// <returns></returns>
        void SaveForm(string keyValue, ScaffoldModel model);


        /// <summary>
        /// ����ҵ�����˱�������Ŀ
        /// </summary>
        /// <param name="scaffoldEntity">ҵ������ʵ��</param>
        /// <param name="auditEntity">��˱�ʵ��</param>
        /// <param name="projects">������Ŀ ScaffoldType=1 ʱ����</param>
        void UpdateForm(ScaffoldEntity scaffoldEntity, ScaffoldauditrecordEntity auditEntity, List<ScaffoldprojectEntity> projects);

        #endregion
    }
}
