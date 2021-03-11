using ERCHTMS.Entity.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System;
using System.Linq.Expressions;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Code;

namespace ERCHTMS.IService.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������¼
    /// </summary>
    public interface IDrillplanrecordService
    {
        #region ��ȡ����


        #region Ӧ������Ԥ������ͳ��
        /// <summary>
        /// Ӧ������Ԥ������ͳ��
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        DataTable GetDrillPlanRecordTypeSta(string condition, int mode);
        #endregion
        DataTable DrillplanStatList(string drillmode, bool isCompany, string deptCode, string starttime, string endtime);
        DataTable DrillplanStat(string drillmode, bool isCompany, string deptCode, string starttime, string endtime);
        DataTable DrillplanStatDetail(string drillmode, bool isCompany, string deptCode, string starttime, string endtime);
        DataTable GetAssessRecordList(Pagination pagination, string queryJson);

        List<DrillrecordAssessEntity> GetAssessList(string drillrecordid);
        /// <summary>
        /// ��ȡ��ҳ����������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        int GetDrillPlanRecordEvaluateNum(Operator user);
        /// <summary>
        /// ���ݵ�ǰ��½�˻�ȡ��ҳ����������
        /// </summary>
        /// <returns></returns>
        int GetDrillPlanRecordAssessNum(Operator user);
        /// <summary>
        /// ��ȡ��������ͼ
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        Flow GetEvaluateFlow(string keyValue);
        IEnumerable<DrillplanrecordEntity> GetListForCon(Expression<Func<DrillplanrecordEntity, bool>> condition);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        DataTable GetPageList(Pagination pagination, string queryJson);
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        IEnumerable<DrillplanrecordEntity> GetList(string queryJson);
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        DrillplanrecordEntity GetEntity(string keyValue);
        /// <summary>
        /// ��ȡ��ʷ��¼ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        DrillplanrecordHistoryEntity GetHistoryEntity(string keyValue);
        /// <summary>
        /// ��ȡ����Ӧ�������ƻ�̨��
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        DataTable GetBZList(String strsql);
        DataTable GetHistoryPageListJson(Pagination pagination, string queryJson);
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
        void SaveForm(string keyValue, DrillplanrecordEntity entity);
        /// <summary>
        /// ������ʷ��¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        void SaveHistoryForm(string keyValue, DrillplanrecordHistoryEntity entity);
        #endregion
    }
}
