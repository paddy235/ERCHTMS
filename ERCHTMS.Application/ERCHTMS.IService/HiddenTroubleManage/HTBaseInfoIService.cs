using ERCHTMS.Entity.BaseManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using ERCHTMS.Entity.HiddenTroubleManage;
using System.Data;
using BSFramework.Application.Entity;
using ERCHTMS.Code;
using System.Collections;

namespace ERCHTMS.IService.HiddenTroubleManage
{
    /// <summary>
    /// �� ��������������Ϣ��
    /// </summary>
    public interface HTBaseInfoIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б� 
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<HTBaseInfoEntity> GetList(string queryJson);

        IList<HTBaseInfoEntity> GetListByCode(string hidcode);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        HTBaseInfoEntity GetEntity(string keyValue);
        /// <summary>
        /// Υ���б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        DataTable GetRulerPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ����ͳ�Ƶ���Ǽǵ�Υ�²��ҵ���δ���յ�Υ�������͵���Ǽǵ�Υ�µ�������
        /// </summary>
        /// <param name="currDate">ʱ��</param>
        ///  <param name="deptCode">����Code</param>
        /// <returns></returns>
        DataTable GetLllegalRegisterNumByMonth(string currDate, string deptCode);
        /// <summary>
        /// ����ͳ��
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="year"></param>
        /// <param name="area"></param>
        /// <param name="hidrank"></param>
        /// <param name="userId"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        DataTable QueryStatisticsByAction(StatisticsEntity sentity);

        /// <summary>
        /// ������ȫ�����Ų���������±���
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="curdate"></param>
        /// <returns></returns>
        DataTable GetHiddenSituationOfMonth(string deptcode, string curdate, Operator curUser);

        #region ��ȡ��ҳ����ͳ��
        /// <summary>
        /// ��ȡ��ҳ����ͳ��
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="curYear"></param>
        /// <param name="topNum"></param>
        /// <returns></returns>
        DataTable GetHomePageHiddenByHidType(Operator curUser, int curYear, int topNum, int qType);
        #endregion

        #region ���ݲ��ű����ȡ
        /// <summary>
        /// ���ݲ��ű����ȡ
        /// </summary>
        /// <param name="orginezeId"></param>
        /// <param name="encode"></param>
        /// <param name="curYear"></param>
        /// <param name="qType"></param>
        /// <returns></returns>
        DataTable GetHomePageHiddenByDepart(string orginezeId, string encode, string curYear, int qType);
        #endregion

        /// <summary>
        /// ��ȡ��ȫԤ��
        /// </summary>
        /// <returns></returns>
        DataTable GetHidSafetyWarning(int type, string orgcode);

        /// <summary>
        /// ������鼯��
        /// </summary>
        /// <param name="checkId"></param>
        /// <param name="checkman"></param>
        /// <returns></returns>
        DataTable GetList(string checkId, string checkman, string districtcode, string workstream);

        DataTable GetGeneralQuery(string sql, Pagination pagination);

        /// <summary>
        /// ��ȡͨ�ò�ѯ
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        DataTable GetGeneralQueryBySql(string sql);

        DataTable GetHiddenByKeyValue(string keyValue);

        DataTable GetDescribeListByUserId(string userId, string hiddescribe);

        DataTable QueryHidWorkList(Operator curUser);

        DataTable QueryHidBacklogRecord(string value, string userId);

        DataTable QueryExposureHid(string num);

        DataTable GetAppHidStatistics(string code, int mode, int category);

        DataTable GetBaseInfoForApp(Pagination pagination);

        DataTable GetHiddenInfoOfWarning(Operator user, string startDate, string endDate);

        decimal GetHiddenWarning(Operator user, string startDate);

        object GetHiddenInfoOfEveryMonthWarning(Operator user, string startDate, string endDate);

        DataTable GetSafetyValueOfWarning(int action, string orgCode, string startDate, string endDate);

        #region ��Ҫָ��(ʡ��)
        /// <summary>
        /// ��Ҫָ��(ʡ��)
        /// </summary>
        /// <param name="action"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        DataTable GetImportantIndexForProvincial(int action, Operator user);
        #endregion


        /// <summary>
        /// ʡ��ͳ������
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        DataTable QueryProvStatisticsByAction(ProvStatisticsEntity entity);

        #region ��ȡ���������µ�������Ϣ
        /// <summary>
        /// ��ȡ���������µ�������Ϣ
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        DataTable GetHiddenByRelevanceId(Pagination pagination, string queryJson);
        #endregion
        #endregion

        #region ��ȡ��ҳ����
        /// <summary>
        /// ��ȡ��ҳ����
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        DataTable GetHiddenBaseInfoPageList(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, HTBaseInfoEntity entity);
        #endregion

        #region MyRegion
        /// <summary>
        /// ������ȫ����Ӧ����������
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        DataTable GetHiddenOfSafetyCheck(string keyValue, int mode);
        #endregion

        #region ������ҳ-δ��������
        /// <summary>
        /// ��ȡ������ҳ-δ��������
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DataTable GetNoChangeHidList(string code);
        IList GetCountByArea(List<string> areaCodes);
        #endregion
    }
}
