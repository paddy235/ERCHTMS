using ERCHTMS.Entity.EmergencyPlatform;
using ERCHTMS.IService.EmergencyPlatform;
using ERCHTMS.Service.EmergencyPlatform;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq.Expressions;
using ERCHTMS.Entity.HiddenTroubleManage;
using ERCHTMS.Code;

namespace ERCHTMS.Busines.EmergencyPlatform
{
    /// <summary>
    /// �� ����Ӧ��������¼
    /// </summary>
    public class DrillplanrecordBLL
    {
        private IDrillplanrecordService service = new DrillplanrecordService();
        private DrillrecordevaluateIService evaluateService = new DrillrecordevaluateService();


        #region ��ȡ����

        #region Ӧ������Ԥ������ͳ��
        /// <summary>
        /// Ӧ������Ԥ������ͳ��
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public DataTable GetDrillPlanRecordTypeSta(string condition, int mode)
        {
            try
            {
                return service.GetDrillPlanRecordTypeSta(condition, mode);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable DrillplanStatList(string drillmode, bool isCompany, string deptCode, string starttime, string endtime)
        {
            try
            {
                return service.DrillplanStatList(drillmode, isCompany, deptCode, starttime, endtime);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable DrillplanStat(string drillmode, bool isCompany, string deptCode, string starttime, string endtime)
        {
            try
            {
                return service.DrillplanStat(drillmode, isCompany, deptCode, starttime, endtime);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable DrillplanStatDetail(string drillmode, bool isCompany, string deptCode, string starttime, string endtime)
        {
            try
            {
                return service.DrillplanStatDetail(drillmode, isCompany, deptCode, starttime, endtime);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public IEnumerable<DrillplanrecordEntity> GetListForCon(Expression<Func<DrillplanrecordEntity, bool>> condition)
        {
            return service.GetListForCon(condition);
        }

        public List<DrillrecordAssessEntity> GetAssessList(string drillrecordid) 
        {
            return service.GetAssessList(drillrecordid);
        }
        /// <summary>
        /// ��ȡ����״̬ͼ
        /// </summary>
        /// <param name="keyValue">����Id</param>
        /// <returns></returns>
        public Flow GetEvaluateFlow(string keyValue)
        {
            return service.GetEvaluateFlow(keyValue);
        }
        public DataTable GetHistoryPageListJson(Pagination pagination, string queryJson)
        {
            return service.GetHistoryPageListJson(pagination, queryJson);
        }
        public DataTable GetAssessRecordList(Pagination pagination, string queryJson) {
            return service.GetAssessRecordList(pagination, queryJson);
        }
        /// <summary>
        /// �û��б�
        /// </summary>
        /// <param name="pagination">��ҳ</param>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns></returns>
        public DataTable GetPageList(Pagination pagination, string queryJson)
        {
            return service.GetPageList(pagination, queryJson);
        }
        public DataTable GetEvaluateList(Pagination pagination, string queryJson)
        {
            return evaluateService.GetEvaluateList(pagination, queryJson);
        }
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<DrillplanrecordEntity> GetList(string queryJson)
        {
            return service.GetList(queryJson);
        }
        /// <summary>
        /// ��ȡ��ҳ����������
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetDrillPlanRecordEvaluateNum(Operator user)
        {
            return service.GetDrillPlanRecordEvaluateNum(user);
        }
        /// <summary>
        /// ���ݵ�ǰ��½�˻�ȡ��ҳ����������
        /// </summary>
        /// <returns></returns>
        public int GetDrillPlanRecordAssessNum(Operator user)
        {
            return service.GetDrillPlanRecordAssessNum(user);
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public DrillplanrecordEntity GetEntity(string keyValue)
        {
            return service.GetEntity(keyValue);
        }
        /// <summary>
        /// ��ȡ��ʷ��¼ʵ�� 
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns>���ض���Json</returns>
        public DrillplanrecordHistoryEntity GetHistoryEntity(string keyValue)
        {
            return service.GetHistoryEntity(keyValue);
        }
        #endregion

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            try
            {
                service.RemoveForm(keyValue);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, DrillplanrecordEntity entity)
        {
            try
            {
                service.SaveForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// ������ʷ��¼
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        public void SaveHistoryForm(string keyValue, DrillplanrecordHistoryEntity entity)
        {
            try
            {
                service.SaveHistoryForm(keyValue, entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// ��ȡ����Ӧ�������ƻ�̨��
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public DataTable GetBZList(String strsql)
        {
            return service.GetBZList(strsql);
        }
        #endregion
    }
}
